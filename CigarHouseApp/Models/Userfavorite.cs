using System;
using System.Collections.Generic;

namespace CigarHouseApp.Models;

public partial class Userfavorite
{
    public int FavoritesId { get; set; }

    public int UserId { get; set; }

    public virtual User User { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
