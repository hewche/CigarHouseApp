using System;
using System.Collections.Generic;

namespace CigarHouseApp.Models;

public partial class UserCartItem
{
    public int? CartId { get; set; }

    public int? UserId { get; set; }

    public int? ProductId { get; set; }

    public string? ProductName { get; set; }

    public int? Quantity { get; set; }

    public decimal? CostProduct { get; set; }
}
