using System;
using System.Collections.Generic;

namespace aaa_aspdotnet.src.DAL.Entities;

public partial class DailyDivision
{
    public int DeviceId { get; set; }

    public int PlanId { get; set; }

    public string UserId { get; set; } = null!;

    public DateTime? WorkDay { get; set; }

    public TimeSpan? StartTime { get; set; }

    public TimeSpan? EstimateFinishTime { get; set; }

    public double? TotalTime { get; set; }

    public string? SpecificContents { get; set; }

    public int Quantity { get; set; }

    public string? Reason { get; set; }

    public string? JobDescription { get; set; }

    public string? BeforeImage { get; set; }

    public string? AfterImage { get; set; }

    public DateTime? CompletedDate { get; set; }

    public int? CheckedBy { get; set; }

    public int StatusId { get; set; }

    public virtual Device Device { get; set; } = null!;

    public virtual Plan Plan { get; set; } = null!;

    public virtual WorkStatus Status { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
