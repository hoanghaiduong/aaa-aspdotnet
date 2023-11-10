using System;
using System.Collections.Generic;

namespace aaa_aspdotnet.src.DAL.Entities;

public partial class Technical
{
    public int TechnicalId { get; set; }

    public string TechnicalName { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Phone2 { get; set; }

    public string? Address { get; set; }

    public string? Zalo { get; set; }

    public string? Email { get; set; }

    public bool IsDelete { get; set; }

    public virtual ICollection<DailyDivision> DailyDivisions { get; set; } = new List<DailyDivision>();
}
