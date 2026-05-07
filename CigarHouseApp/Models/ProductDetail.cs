using System;
using System.Collections.Generic;

namespace CigarHouseApp.Models;

public partial class ProductDetail
{
    public int? ProductId { get; set; }

    public string? ProductName { get; set; }

    public string? BrandName { get; set; }

    public string? BrandPhone { get; set; }

    public int? Quantity { get; set; }

    public decimal? CostProduct { get; set; }

    public string? Image { get; set; }
}
