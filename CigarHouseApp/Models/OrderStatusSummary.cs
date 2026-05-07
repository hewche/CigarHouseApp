using System;
using System.Collections.Generic;

namespace CigarHouseApp.Models;

public partial class OrderStatusSummary
{
    public int? OrderStatusId { get; set; }

    public string? StatusName { get; set; }

    public long? OrdersCount { get; set; }

    public decimal? TotalRevenue { get; set; }

    public DateTime? OldestOrder { get; set; }

    public DateTime? NewestOrder { get; set; }

    public decimal? AvgOrderValue { get; set; }
}
