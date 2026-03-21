using System;
using System.Collections.Generic;

namespace CigarHouseApp.Models;

public partial class Country
{
    public int CountryId { get; set; }

    public string CountryName { get; set; } = null!;

    public virtual ICollection<Delivery> DeliveryDeliveryLocationFromNavigations { get; set; } = new List<Delivery>();

    public virtual ICollection<Delivery> DeliveryDeliveryLocationToNavigations { get; set; } = new List<Delivery>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
