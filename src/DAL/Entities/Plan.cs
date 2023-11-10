using System;
using System.Collections.Generic;

namespace aaa_aspdotnet.src.DAL.Entities;

public partial class Plan
{
    public int PlanId { get; set; }

    public string? Contents { get; set; }

    public DateTime CreateDate { get; set; }

    public int? CreateBy { get; set; }

    public DateTime? BeginDate { get; set; }

    public DateTime? EndDate { get; set; }

    public bool IsDelete { get; set; }

    public virtual ICollection<DailyDivision> DailyDivisions { get; set; } = new List<DailyDivision>();

    public virtual ICollection<DetailPlan> DetailPlans { get; set; } = new List<DetailPlan>();
}
