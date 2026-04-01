using System;
using System.Collections.Generic;

namespace CigarHouseApp.Models;

public partial class OrderBasic
{
    public int? OrderId { get; set; }

    public string? Customer { get; set; }

    public string? Status { get; set; }

    public string? Payment { get; set; }

    public string? Address { get; set; }
}
