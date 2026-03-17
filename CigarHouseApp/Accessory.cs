using CigarHouseApp.Models;
using System;
using System.Collections.Generic;

namespace CigarHouseApp;

public partial class Accessory
{
    public int ProductId { get; set; }

    public string? Material { get; set; }

    public string? Color { get; set; }

    public virtual Product Product { get; set; } = null!;
}
