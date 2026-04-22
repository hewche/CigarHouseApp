using System;
using System.Collections.Generic;

namespace CigarHouseApp;

public partial class User
{
    public int UserId { get; set; }

    public string FirstName { get; set; } = null!;

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Phone { get; set; }

    public int? RoleId { get; set; }

    public int? Cart { get; set; }

    public int? Favorites { get; set; }

    public string? Email { get; set; }

    public string? LastName { get; set; }

    public DateTime? Birthday { get; set; }

    public virtual ICollection<News> News { get; set; } = new List<News>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual Role? Role { get; set; }

    public virtual Usercart? Usercart { get; set; }

    public virtual Userfavorite? Userfavorite { get; set; }
}
