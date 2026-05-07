using System;
using System.Collections.Generic;

namespace CigarHouseApp.Models;

public partial class CartSummary
{
    public int? CartId { get; set; }

    public int? UserId { get; set; }

    public long? TotalItems { get; set; }

    public decimal? CartTotal { get; set; }

    public string? ProductsList { get; set; }
}
