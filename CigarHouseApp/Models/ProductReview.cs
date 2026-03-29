using System;
using System.Collections.Generic;

namespace CigarHouseApp;

public partial class ProductReview
{
    public int? ReviewId { get; set; }

    public string? Title { get; set; }

    public string? ReviewText { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? Author { get; set; }

    public string? ProductName { get; set; }

    public string? BrandName { get; set; }
}
