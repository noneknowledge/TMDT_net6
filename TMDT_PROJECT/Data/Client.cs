using System;
using System.Collections.Generic;

namespace TMDT_PROJECT.Data
{
    public partial class Client
    {
        public Client()
        {
            Libraries = new HashSet<Library>();
            Orders = new HashSet<Order>();
            ShoppingCarts = new HashSet<ShoppingCart>();
        }

        public string Uid { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string PassWord { get; set; } = null!;
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? IsActive { get; set; }
        public string? IsHide { get; set; }
        public string? RandomKey { get; set; }

        public virtual ICollection<Library> Libraries { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<ShoppingCart> ShoppingCarts { get; set; }
    }
}
