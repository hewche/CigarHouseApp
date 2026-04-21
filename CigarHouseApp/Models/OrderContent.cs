using System;
using System.Collections.Generic;

namespace CigarHouseApp;

public partial class OrderContent
{
    public int? OrderItemId { get; set; }

    public int? OrderId { get; set; }

    public string? ProductName { get; set; }

    public string? BrandName { get; set; }

    public int? Quantity { get; set; }

    public decimal? PricePerUnit { get; set; }

    public decimal? TotalPrice { get; set; }
}
