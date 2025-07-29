using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserAuthApp.API.Interfaces;
using UserAuthApp.API.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace UserAuthApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public AuthController(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    [HttpGet("google")]
    public IActionResult GoogleLogin()
    {
        var properties = new AuthenticationProperties
        {
            RedirectUri = Url.Action(nameof(GoogleCallback)),
            Items = { { "scheme", GoogleDefaults.AuthenticationScheme } }
        };
        return Challenge(properties, GoogleDefaults.AuthenticationScheme);
    }

    [HttpGet("google-callback")]
    public async Task<IActionResult> GoogleCallback()
    {
        try
        {
            // Authenticate using the cookie scheme (where Google auth result is stored)
            var result = await HttpContext.AuthenticateAsync("Cookies");
            
            if (!result.Succeeded || result.Principal == null)
            {
                return BadRequest("Authentication failed - no valid authentication result");
            }

            var claims = result.Principal.Claims.ToList();
            var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var name = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            var providerId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(providerId))
            {
                return BadRequest($"Required user information not available. Email: {email}, ProviderId: {providerId}");
            }

            // Check if user exists
            var existingUser = await _userRepository.GetByProviderIdAsync(providerId, "Google");
            
            if (existingUser == null)
            {
                // Create new user
                var newUser = new User
                {
                    Email = email,
                    Name = name ?? "",
                    ProviderId = providerId,
                    Provider = "Google"
                };
                
                var userId = await _userRepository.CreateAsync(newUser);
                newUser.Id = userId;
                existingUser = newUser;
            }

            // Generate JWT token
            var token = GenerateJwtToken(existingUser);
            
            // Clear the temporary cookie
            await HttpContext.SignOutAsync("Cookies");
            
            return Ok(new { Token = token, User = existingUser });
        }
        catch (Exception ex)
        {
            return BadRequest($"Authentication error: {ex.Message}");
        }
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUser()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (!int.TryParse(userIdClaim, out var userId))
            return Unauthorized();

        var user = await _userRepository.GetByIdAsync(userId);
        
        if (user == null)
            return NotFound();

        return Ok(user);
    }

    private string GenerateJwtToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? ""));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Name)
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddDays(7),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}