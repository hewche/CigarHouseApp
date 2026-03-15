using System;
using System.Collections.Generic;

namespace CigarHouseApp;

public partial class Delivery
{
    public int DeliveryId { get; set; }

    public DateOnly DeliveryDate { get; set; }

    public int? DeliveryLocationTo { get; set; }

    public int? DeliveryLocationFrom { get; set; }

    public virtual Country? DeliveryLocationFromNavigation { get; set; }

    public virtual Country? DeliveryLocationToNavigation { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
