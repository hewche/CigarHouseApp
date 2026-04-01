using System;
using System.Collections.Generic;

namespace CigarHouseApp.Models;

public partial class DeliveryProduct
{
    public int? ProductId { get; set; }

    public string? ProductName { get; set; }

    public int? DeliveryId { get; set; }

    public int? DeliveryLocationFrom { get; set; }

    public DateOnly? DeliveryDate { get; set; }
}
