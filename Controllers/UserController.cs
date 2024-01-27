

using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using signiel.Contexts;
using signiel.Models;
using signiel.Models.Requests;
using signiel.Models.Responses;

namespace signiel.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase {
    private readonly PasswordHasher<User> _hasher = new();
    private readonly ILogger<UserController> _logger;
    private readonly SignielContext _context;
    private readonly IConfiguration _configuration;

    public UserController(ILogger<UserController> logger, SignielContext context, IConfiguration configuration) {
        _logger = logger;
        _context = context;
        _configuration = configuration;
    }

    [HttpPost("login")]
    public async Task<APIResponse<string>> Login([FromBody] LoginRequest request) {
        var user = await _context.Users.FirstOrDefaultAsync(user => user.Name == request.Username);

        if (user == null) {
            HttpContext.Response.StatusCode = HttpStatusCode.NotFound.GetHashCode();
            return APIResponse<string>.FromError("User not found.");
        }

        var result = _hasher.VerifyHashedPassword(user, user.Password, request.Password);

        if (result == PasswordVerificationResult.Failed) {
            HttpContext.Response.StatusCode = HttpStatusCode.Unauthorized.GetHashCode();
            return APIResponse<string>.FromError("Password is incorrect.");
        }

        var claims = new List<Claim> {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Name)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddDays(30),
            signingCredentials: credentials
        );

        return APIResponse<string>.FromData(new JwtSecurityTokenHandler().WriteToken(token));
    }

    [HttpPost("register")]
    public async Task<APIResponse<string>> Register([FromBody] RegisterRequest request) {
        var user = await _context.Users.FirstOrDefaultAsync(user => user.Name == request.Username);

        if (user != null) {
            HttpContext.Response.StatusCode = HttpStatusCode.Conflict.GetHashCode();
            return APIResponse<string>.FromError("User already exists.");
        }

        var newUser = new User {
            Name = request.Username,
            Password = _hasher.HashPassword(null!, request.Password),
            Nickname = request.Nickname,
        };

        await _context.Users.AddAsync(newUser);
        await _context.SaveChangesAsync();

        return APIResponse<string>.FromData("User created.");
    }
}