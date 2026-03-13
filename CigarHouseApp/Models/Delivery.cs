using System;
using System.Collections.Generic;

namespace CigarHouseApp.Models;

public partial class Delivery
{
    public int DeliveryId { get; set; }

    public DateOnly DeliveryDate { get; set; }

    public string DeliveryLocation { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
