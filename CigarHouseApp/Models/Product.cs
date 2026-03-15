using System;
using System.Collections.Generic;

namespace CigarHouseApp;

public partial class Product
{
    public int ProductId { get; set; }

    public int? DeliveryId { get; set; }

    public int? BrandId { get; set; }

    public string ProductName { get; set; } = null!;

    public int Quantity { get; set; }

    public decimal CostProduct { get; set; }

    public virtual Accessory? Accessory { get; set; }

    public virtual Brand? Brand { get; set; }

    public virtual Cigar? Cigar { get; set; }

    public virtual Delivery? Delivery { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
