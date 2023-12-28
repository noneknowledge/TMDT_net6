using System;
using System.Collections.Generic;

namespace TMDT_PROJECT.Data
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public string OrderId { get; set; } = null!;
        public string Uid { get; set; } = null!;
        public DateTime? OrderDate { get; set; }
        public string? PaymentId { get; set; }
        public double? Amount { get; set; }
        public double? Total { get; set; }

        public virtual Client UidNavigation { get; set; } = null!;
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
