using System;
using System.Collections.Generic;

namespace aaa_aspdotnet.src.DAL.Entities;

public partial class Factory
{
    public int FacId { get; set; }

    public string FacName { get; set; } = null!;

    public string? Alias { get; set; }

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public string? Phone2 { get; set; }

    public int CustomerId { get; set; }

    public bool IsDelete { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<Device> Devices { get; set; } = new List<Device>();
}
