using System;
using System.Collections.Generic;

namespace CigarHouseApp;

public partial class Itemcart
{
    public int CartId { get; set; }

    public int ProductId { get; set; }

    public virtual Usercart Cart { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
