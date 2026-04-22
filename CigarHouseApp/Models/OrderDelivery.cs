using System;
using System.Collections.Generic;

namespace CigarHouseApp.Models;

public partial class OrderDelivery
{
    public int? DeliveryId { get; set; }

    public DateOnly? DeliveryDate { get; set; }

    public int? DeliveryLocationTo { get; set; }

    public int? DeliveryLocationFrom { get; set; }

    public int? OrderId { get; set; }

    public string? Customer { get; set; }
}
