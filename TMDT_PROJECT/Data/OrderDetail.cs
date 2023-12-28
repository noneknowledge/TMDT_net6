using System;
using System.Collections.Generic;

namespace TMDT_PROJECT.Data
{
    public partial class OrderDetail
    {
        public string OrderId { get; set; } = null!;
        public string GameId { get; set; } = null!;
        public double? Prices { get; set; }

        public virtual Game Game { get; set; } = null!;
        public virtual Order Order { get; set; } = null!;
    }
}
