using System;
using System.Collections.Generic;

namespace aaa_aspdotnet.src.DAL.Entities;

public partial class WorkStatus
{
    public int StatusId { get; set; }

    public string? StatusName { get; set; }

    public bool IsDelete { get; set; }

    public string? Notes { get; set; }

    public virtual ICollection<DailyDivision> DailyDivisions { get; set; } = new List<DailyDivision>();

    public virtual ICollection<DetailPlan> DetailPlans { get; set; } = new List<DetailPlan>();
}
