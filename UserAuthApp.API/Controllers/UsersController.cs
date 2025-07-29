using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserAuthApp.API.Interfaces;
using UserAuthApp.API.Models;

namespace UserAuthApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public UsersController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userRepository.GetAllAsync();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        
        if (user == null)
            return NotFound();

        return Ok(user);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] User user)
    {
        if (id != user.Id)
            return BadRequest("ID mismatch");

        var existingUser = await _userRepository.GetByIdAsync(id);
        if (existingUser == null)
            return NotFound();

        var success = await _userRepository.UpdateAsync(user);
        
        if (!success)
            return BadRequest("Update failed");

        return Ok(user);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
            return NotFound();

        var success = await _userRepository.DeleteAsync(id);
        
        if (!success)
            return BadRequest("Delete failed");

        return NoContent();
    }
}