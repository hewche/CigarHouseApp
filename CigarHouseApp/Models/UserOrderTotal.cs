using System;
using System.Collections.Generic;

namespace CigarHouseApp.Models;

public partial class UserOrderTotal
{
    public int? UserId { get; set; }

    public long? TotalOrders { get; set; }

    public decimal? TotalSpent { get; set; }

    public decimal? AvgOrderValue { get; set; }

    public DateTime? LastOrderDate { get; set; }
}
