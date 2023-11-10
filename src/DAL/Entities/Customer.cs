using System;
using System.Collections.Generic;

namespace aaa_aspdotnet.src.DAL.Entities;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string CustomerName { get; set; } = null!;

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public string? Phone2 { get; set; }

    public string? Email { get; set; }

    public bool IsDelete { get; set; }

    public virtual ICollection<Factory> Factories { get; set; } = new List<Factory>();
}
