using System;
using System.Collections.Generic;

namespace CigarHouseApp.Models;

public partial class BestProduct
{
    public int? ProductId { get; set; }

    public string? ProductName { get; set; }

    public int? BrandId { get; set; }

    public int? Quantity { get; set; }

    public decimal? CostProduct { get; set; }

    public string? Image { get; set; }

    public int? Country { get; set; }

    public decimal? AvgRating { get; set; }

    public long? ReviewCount { get; set; }
}
