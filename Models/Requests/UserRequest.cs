
namespace signiel.Models.Requests;

public class RegisterRequest {
    [Required, MinLength(4), MaxLength(20)]
    public required string Username { get; set; }
    [Required, MinLength(2), MaxLength(20)]
    public required string Nickname { get; set; }
    // password
    [Required, MinLength(1), MaxLength(20)]
    public required string Password { get; set; }
}

public class LoginRequest {
    [Required, MinLength(4), MaxLength(20)]
    public required string Username { get; set; }
    [Required, MinLength(1), MaxLength(20)]
    public required string Password { get; set; }
}