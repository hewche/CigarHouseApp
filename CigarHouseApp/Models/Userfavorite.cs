using System;
using System.Collections.Generic;

namespace CigarHouseApp;

public partial class Userfavorite
{
    public int FavoritesId { get; set; }

    public int UserId { get; set; }

    public virtual User User { get; set; } = null!;
}
