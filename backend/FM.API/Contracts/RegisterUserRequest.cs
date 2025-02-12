using System.ComponentModel.DataAnnotations;

namespace FM.API.Contracts;
public record RegisterUserRequest
(
    [Required] string UserName,
    [Required] string Password,
    [Required] string Email,
    int Role
);
