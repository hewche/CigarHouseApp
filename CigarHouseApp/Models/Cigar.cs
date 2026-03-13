using System;
using System.Collections.Generic;

namespace CigarHouseApp.Models;

public partial class Cigar
{
    public int ProductId { get; set; }

    public string? Strength { get; set; }

    public string? RingGauge { get; set; }

    public string? Vitola { get; set; }

    public string? FlavorProfile { get; set; }

    public virtual Product Product { get; set; } = null!;
}
