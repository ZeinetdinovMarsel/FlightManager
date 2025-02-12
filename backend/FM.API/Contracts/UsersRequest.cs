using System.ComponentModel.DataAnnotations;
using FM.Core.Enums;

namespace FM.API.Contracts;
public record UsersRequest
(
    [Required] Guid UserId,
    [Required] string name
);

public record UsersAdminResponse
(
    [Required] Guid UserId,
    [Required] string UserName,
    [Required] string Password,
    [Required] string Email,
    [Required] Role Role
);
public record UsersAdminRequest
(
    Guid UserId,
    [Required] string UserName,
    string Password,
    [Required] string Email,
    [Required] Role Role
);