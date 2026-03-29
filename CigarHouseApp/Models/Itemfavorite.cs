using System;
using System.Collections.Generic;

namespace CigarHouseApp;

public partial class Itemfavorite
{
    public int FavoritesId { get; set; }

    public int ProductId { get; set; }

    public virtual Userfavorite Favorites { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
