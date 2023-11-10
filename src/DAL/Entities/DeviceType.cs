using System;
using System.Collections.Generic;

namespace aaa_aspdotnet.src.DAL.Entities;

public partial class DeviceType
{
    public int TypeId { get; set; }

    public string? TypeName { get; set; }

    public bool IsDelete { get; set; }

    public virtual ICollection<Device> Devices { get; set; } = new List<Device>();
}
