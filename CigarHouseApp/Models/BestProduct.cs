using System;
using System.Collections.Generic;

namespace CigarHouseApp.Models;

public partial class BestProduct
{
    public int? ProductId { get; set; }

    public decimal? AvgRating { get; set; }

    public long? ReviewCount { get; set; }
}
