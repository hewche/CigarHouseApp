using System;
using System.Collections.Generic;

namespace CigarHouseApp.Models;

public partial class TopRatedProduct
{
    public int? ProductId { get; set; }

    public string? ProductName { get; set; }

    public string? BrandName { get; set; }

    public decimal? AvgRating { get; set; }

    public long? ReviewCount { get; set; }

    public decimal? Price { get; set; }

    public int? StockQuantity { get; set; }
}
