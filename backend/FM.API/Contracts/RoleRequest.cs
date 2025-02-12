using System.ComponentModel.DataAnnotations;

namespace FM.API.Contracts;
public record RoleRequest
(
    [Required] int role
    );