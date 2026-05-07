using System;
using System.Collections.Generic;

namespace CigarHouseApp.Models;

public partial class BrandInventory
{
    public int? BrandId { get; set; }

    public string? BrandName { get; set; }

    public string? BrandPhone { get; set; }

    public long? TotalProducts { get; set; }

    public long? TotalStockQuantity { get; set; }

    public decimal? TotalInventoryValue { get; set; }
}
