using System;
using System.Collections.Generic;

namespace CigarHouseApp.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int? OrderStatusId { get; set; }

    public int? UserId { get; set; }

    public int? PaymentMethod { get; set; }

    public string Address { get; set; } = null!;

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual OrderStatus? OrderStatus { get; set; }

    public virtual Paymentmethod? PaymentMethodNavigation { get; set; }

    public virtual User? User { get; set; }
}
