using System;
using System.Collections.Generic;

namespace CigarHouseApp;

public partial class Country
{
    public int CountryId { get; set; }

    public string CountryName { get; set; } = null!;

    public virtual ICollection<Delivery> DeliveryDeliveryLocationFromNavigations { get; set; } = new List<Delivery>();

    public virtual ICollection<Delivery> DeliveryDeliveryLocationToNavigations { get; set; } = new List<Delivery>();
}
