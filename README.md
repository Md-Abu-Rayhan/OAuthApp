# Complete API Testing Guide with curl and Postman

## Step 1: Get Your JWT Token

1. Open browser and go to: `https://localhost:7000/api/auth/google`
2. Sign in with Google
3. You'll get a response like this:
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "user": {
    "id": 1,
    "email": "your-email@gmail.com",
    "name": "Your Name",
    "providerId": "google_123456",
    "provider": "Google"
  }
}
```
4. Copy the `token` value - you'll need it for all API calls

## Step 2: Test All API Endpoints with curl

**Replace `YOUR_JWT_TOKEN` with your actual token from Step 1**

### Get Current User Info
```bash
curl -X GET "https://localhost:7000/api/auth/me" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN" \
  -H "Content-Type: application/json" \
  -k
```

### Get All Users
```bash
curl -X GET "https://localhost:7000/api/users" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN" \
  -H "Content-Type: application/json" \
  -k
```

### Get Specific User by ID
```bash
curl -X GET "https://localhost:7000/api/users/1" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN" \
  -H "Content-Type: application/json" \
  -k
```

### Update User Information
```bash
curl -X PUT "https://localhost:7000/api/users/1" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "id": 1,
    "email": "your-email@gmail.com",
    "name": "Your Updated Name",
    "providerId": "your_provider_id",
    "provider": "Google"
  }' \
  -k
```

### Delete User (Optional - be careful!)
```bash
curl -X DELETE "https://localhost:7000/api/users/1" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN" \
  -H "Content-Type: application/json" \
  -k
```

### Test Error Handling (should return 404)
```bash
curl -X GET "https://localhost:7000/api/users/999" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN" \
  -H "Content-Type: application/json" \
  -k
```

### Test Unauthorized Access (should return 401)
```bash
curl -X GET "https://localhost:7000/api/users" \
  -H "Content-Type: application/json" \
  -k
```

### Health Check Endpoints
```bash
# Check API health
curl -X GET "https://localhost:7000/api/test/health" -k

# Check Google configuration
curl -X GET "https://localhost:7000/api/test/google-test" -k
```

## Step 3: Complete Postman Collection

### Import This Postman Collection:

1. **Open Postman**
2. **Click "Import"**
3. **Paste this JSON:**

```json
{
  "info": {
    "name": "User Auth App API",
    "description": "Complete API testing collection for User Auth App",
    "schema": "https://schema.getpostman.com/json/collection/v2.1.0/collection.json"
  },
  "variable": [
    {
      "key": "baseUrl",
      "value": "https://localhost:7000",
      "type": "string"
    },
    {
      "key": "token",
      "value": "YOUR_JWT_TOKEN_HERE",
      "type": "string"
    }
  ],
  "auth": {
    "type": "bearer",
    "bearer": [
      {
        "key": "token",
        "value": "{{token}}",
        "type": "string"
      }
    ]
  },
  "item": [
    {
      "name": "Authentication",
      "item": [
        {
          "name": "Google OAuth Login",
          "request": {
            "method": "GET",
            "header": [],
            "url": {
              "raw": "{{baseUrl}}/api/auth/google",
              "host": ["{{baseUrl}}"],
              "path": ["api", "auth", "google"]
            },
            "description": "Initiates Google OAuth flow. Use browser for this endpoint."
          }
        },
        {
          "name": "Get Current User",
          "request": {
            "method": "GET",
            "header": [
              {
                "key": "Authorization",
                "value": "Bearer {{token}}",
                "type": "text"
              }
            ],
            "url": {
              "raw": "{{baseUrl}}/api/auth/me",
              "host": ["{{baseUrl}}"],
              "path": ["api", "auth", "me"]
            },
            "description": "Get information about the currently authenticated user"
          }
        }
      ]
    },
    {
      "name": "User Management",
      "item": [
        {
          "name": "Get All Users",
          "request": {
            "method": "GET",
            "header": [
              {
                "key": "Authorization",
                "value": "Bearer {{token}}",
                "type": "text"
              }
            ],
            "url": {
              "raw": "{{baseUrl}}/api/users",
              "host": ["{{baseUrl}}"],
              "path": ["api", "users"]
            },
            "description": "Retrieve all users in the system"
          }
        },
        {
          "name": "Get User by ID",
          "request": {
            "method": "GET",
            "header": [
              {
                "key": "Authorization",
                "value": "Bearer {{token}}",
                "type": "text"
              }
            ],
            "url": {
              "raw": "{{baseUrl}}/api/users/1",
              "host": ["{{baseUrl}}"],
              "path": ["api", "users", "1"]
            },
            "description": "Get a specific user by their ID"
          }
        },
        {
          "name": "Update User",
          "request": {
            "method": "PUT",
            "header": [
              {
                "key": "Authorization",
                "value": "Bearer {{token}}",
                "type": "text"
              },
              {
                "key": "Content-Type",
                "value": "application/json",
                "type": "text"
              }
            ],
            "body": {
              "mode": "raw",
              "raw": "{\n  \"id\": 1,\n  \"email\": \"updated-email@gmail.com\",\n  \"name\": \"Updated Name\",\n  \"providerId\": \"your_provider_id\",\n  \"provider\": \"Google\"\n}"
            },
            "url": {
              "raw": "{{baseUrl}}/api/users/1",
              "host": ["{{baseUrl}}"],
              "path": ["api", "users", "1"]
            },
            "description": "Update user information"
          }
        },
        {
          "name": "Delete User",
          "request": {
            "method": "DELETE",
            "header": [
              {
                "key": "Authorization",
                "value": "Bearer {{token}}",
                "type": "text"
              }
            ],
            "url": {
              "raw": "{{baseUrl}}/api/users/1",
              "host": ["{{baseUrl}}"],
              "path": ["api", "users", "1"]
            },
            "description": "Delete a user (be careful with this!)"
          }
        }
      ]
    },
    {
      "name": "Error Testing",
      "item": [
        {
          "name": "Test 404 - Non-existent User",
          "request": {
            "method": "GET",
            "header": [
              {
                "key": "Authorization",
                "value": "Bearer {{token}}",
                "type": "text"
              }
            ],
            "url": {
              "raw": "{{baseUrl}}/api/users/999",
              "host": ["{{baseUrl}}"],
              "path": ["api", "users", "999"]
            },
            "description": "Should return 404 Not Found"
          }
        },
        {
          "name": "Test 401 - Unauthorized Access",
          "request": {
            "method": "GET",
            "header": [],
            "url": {
              "raw": "{{baseUrl}}/api/users",
              "host": ["{{baseUrl}}"],
              "path": ["api", "users"]
            },
            "description": "Should return 401 Unauthorized (no token provided)"
          }
        }
      ]
    },
    {
      "name": "Health Checks",
      "item": [
        {
          "name": "API Health Check",
          "request": {
            "method": "GET",
            "header": [],
            "url": {
              "raw": "{{baseUrl}}/api/test/health",
              "host": ["{{baseUrl}}"],
              "path": ["api", "test", "health"]
            },
            "description": "Check if API is running properly"
          }
        },
        {
          "name": "Google Config Test",
          "request": {
            "method": "GET",
            "header": [],
            "url": {
              "raw": "{{baseUrl}}/api/test/google-test",
              "host": ["{{baseUrl}}"],
              "path": ["api", "test", "google-test"]
            },
            "description": "Verify Google OAuth configuration"
          }
        }
      ]
    }
  ]
}
```

## How to Use Postman Collection:

1. **Set your JWT token:**
   - Click on the collection name
   - Go to "Variables" tab
   - Update the `token` variable with your actual JWT token

2. **Test the endpoints:**
   - Start with "Health Checks" folder
   - Then test "Authentication" → "Get Current User"
   - Finally test all "User Management" endpoints

3. **Expected Results:**
   - Health Check: Returns API status
   - Get Current User: Returns your user info
   - Get All Users: Returns array of users
   - Update User: Returns updated user data
   - Error tests: Return appropriate error codes

## Quick Test Results Summary

| Test | Expected Status | What You Should See |
|------|----------------|-------------------|
| Health Check | 200 OK | `{"status": "Healthy"}` |
| Get Current User | 200 OK | Your user information |
| Get All Users | 200 OK | Array of all users |
| Get User by ID | 200 OK | Specific user details |
| Update User | 200 OK | Updated user data |
| Delete User | 204 No Content | Empty response |
| Non-existent User | 404 Not Found | Error message |
| Unauthorized Access | 401 Unauthorized | Authentication error |

## Pro Tips for Testing:

1. **Always test health endpoints first** to make sure API is running
2. **Get your JWT token through browser** (OAuth won't work in Postman/curl)
3. **Copy the full token** including all the dots and characters
4. **Test error scenarios** to make sure security is working
5. **Use Swagger UI** at `https://localhost:7000/swagger` for interactive testing

## Manual Testing Steps:

### Step 1: Basic Health Checks
1. Test API health endpoint
2. Test Google configuration endpoint
3. Verify both return successful responses

### Step 2: OAuth Flow
1. Open browser to OAuth endpoint
2. Complete Google sign-in
3. Copy JWT token from response

### Step 3: Protected Endpoints
1. Test current user endpoint with token
2. Test get all users with token
3. Test update user with token
4. Test error scenarios

### Step 4: Security Testing
1. Test endpoints without token (should fail)
2. Test with invalid token (should fail)
3. Test non-existent resources (should return 404)

This comprehensive guide covers all possible testing scenarios for your API!