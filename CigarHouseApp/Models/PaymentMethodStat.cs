using System;
using System.Collections.Generic;

namespace CigarHouseApp.Models;

public partial class PaymentMethodStat
{
    public string? PaymentName { get; set; }

    public long? TotalOrders { get; set; }

    public decimal? TotalRevenue { get; set; }

    public decimal? AvgOrderValue { get; set; }
}
