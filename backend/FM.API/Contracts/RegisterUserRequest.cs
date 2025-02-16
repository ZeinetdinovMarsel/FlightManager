using System.ComponentModel.DataAnnotations;

namespace FM.API.Contracts;
public record SignUpUserRequest
(
    [Required] string UserName,
    [Required] string Password,
    [Required] string Email,
    int Role
);
