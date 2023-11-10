using System;
using System.Collections.Generic;

namespace aaa_aspdotnet.src.DAL.Entities;

public partial class DetailPlan
{
    public int DeviceId { get; set; }

    public int PlanId { get; set; }

    public int Quantity { get; set; }

    public string? Unit { get; set; }

    public string? Specification { get; set; }

    public DateTime? ExpectedDate { get; set; }

    public string? Notes { get; set; }

    public double Percents { get; set; }

    public string TypePlan { get; set; } = null!;

    public int StatusId { get; set; }

    public virtual Device Device { get; set; } = null!;

    public virtual Plan Plan { get; set; } = null!;

    public virtual WorkStatus Status { get; set; } = null!;
}
