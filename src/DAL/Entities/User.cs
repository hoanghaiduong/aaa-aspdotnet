using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace aaa_aspdotnet.src.DAL.Entities;

public partial class User
{
    public string UserId { get; set; } = null!;

    public string Username { get; set; } = null!;
    [JsonIgnore]
    public string Password { get; set; } = null!;

    public string? Email { get; set; }

    public bool? Gender { get; set; }

    public string? Avatar { get; set; }

    public string? PhoneNumber { get; set; }

    public string? RefreshToken { get; set; }

    public string? RoleId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    [DefaultValue(true)]
    public bool? IsActived { get; set; } = true;

    [DefaultValue(false)]
    public bool? IsDeleted { get; set; } = false;

    public string? Address { get; set; }

    public string? PhoneNumber2 { get; set; }

    public string? Zalo { get; set; }

    public virtual ICollection<DailyDivision> DailyDivisions { get; set; } = new List<DailyDivision>();

    public virtual ICollection<Factory> Factories { get; set; } = new List<Factory>();

    public virtual Role? Role { get; set; }
}
