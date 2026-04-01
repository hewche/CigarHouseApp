using System;
using System.Collections.Generic;

namespace CigarHouseApp.Models;

public partial class Usercart
{
    public int CartId { get; set; }

    public int UserId { get; set; }

    public virtual User User { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
