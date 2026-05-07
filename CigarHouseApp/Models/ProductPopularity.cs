using System;
using System.Collections.Generic;

namespace CigarHouseApp.Models;

public partial class ProductPopularity
{
    public int? ProductId { get; set; }

    public string? ProductName { get; set; }

    public int? StockQuantity { get; set; }

    public long? TimesInCart { get; set; }

    public long? UniqueCarts { get; set; }
}
