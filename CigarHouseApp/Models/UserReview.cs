using System;
using System.Collections.Generic;

namespace CigarHouseApp.Models;

public partial class UserReview
{
    public int? ReviewId { get; set; }

    public int? UserId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Login { get; set; }

    public string? ReviewText { get; set; }

    public int? Rating { get; set; }

    public DateTime? CreatedAt { get; set; }
}
