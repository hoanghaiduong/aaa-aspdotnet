using System;
using System.Collections.Generic;

namespace aaa_aspdotnet.src.DAL.Entities;

public partial class Device
{
    public int TypeId { get; set; }

    public int DeviceId { get; set; }

    public string DeviceName { get; set; } = null!;

    public string Code { get; set; } = null!;

    public string? Color { get; set; }

    public string? Descriptions { get; set; }

    public string? Qrcode { get; set; }

    public bool IsDelete { get; set; }

    public int FacId { get; set; }

    public virtual ICollection<DailyDivision> DailyDivisions { get; set; } = new List<DailyDivision>();

    public virtual ICollection<DetailPlan> DetailPlans { get; set; } = new List<DetailPlan>();

    public virtual Factory Fac { get; set; } = null!;

    public virtual DeviceType Type { get; set; } = null!;
}
