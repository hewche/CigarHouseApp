using System;
using System.Collections.Generic;

namespace CigarHouseApp;

public partial class Review
{
    public int ReviewId { get; set; }

    public int? UserId { get; set; }

    public int? ProductId { get; set; }

    public string Title { get; set; } = null!;

    public string ReviewText { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public virtual Product? Product { get; set; }

    public virtual User? User { get; set; }
}
