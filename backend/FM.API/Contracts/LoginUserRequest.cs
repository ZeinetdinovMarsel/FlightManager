using System.ComponentModel.DataAnnotations;

namespace FM.API.Contracts;
public record LoginUserRequest
(
    [Required] string Email,
    [Required] string Password);

