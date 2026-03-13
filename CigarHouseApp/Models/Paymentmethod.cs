using System;
using System.Collections.Generic;

namespace CigarHouseApp.Models;

public partial class Paymentmethod
{
    public int PaymentmethodId { get; set; }

    public string PaymentName { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
