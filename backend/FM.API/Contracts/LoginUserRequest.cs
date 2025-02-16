using System.ComponentModel.DataAnnotations;

namespace FM.API.Contracts;
public record SignInUserRequest
(
    [Required] string Email,
    [Required] string Password);

