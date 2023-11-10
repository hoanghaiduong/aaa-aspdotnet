using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace aaa_aspdotnet.src.DAL.Entities;

[Index(nameof(Username), nameof(Email), IsUnique = true)]
public partial class User
{
    public string UserId { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Email { get; set; }

    public bool? Gender { get; set; }

    public string? Avatar { get; set; }
    public string? RefreshToken { get; set; }

    public string? PhoneNumber { get; set; }

    public string? RoleId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool? IsActived { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual Role? Role { get; set; }
}
