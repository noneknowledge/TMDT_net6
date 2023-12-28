using System;
using System.Collections.Generic;

namespace TMDT_PROJECT.Data
{
    public partial class Game
    {
        public Game()
        {
            GameCategories = new HashSet<GameCategory>();
            GameImages = new HashSet<GameImage>();
            Libraries = new HashSet<Library>();
            OrderDetails = new HashSet<OrderDetail>();
            ShoppingCarts = new HashSet<ShoppingCart>();
        }

        public string GameId { get; set; } = null!;
        public string? GameName { get; set; }
        public string? GameDes { get; set; }
        public double? Price { get; set; }
        public string? Thumbnail { get; set; }
        public string? Video { get; set; }
        public string? DevId { get; set; }
        public string? PublisherId { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public int? View { get; set; }
        public string? IsActive { get; set; }
        public int? Discount { get; set; }
        public DateTime? StartDis { get; set; }
        public DateTime? EndDis { get; set; }
        public string? Alias { get; set; }
        public int? Liked { get; set; }
        public int? Disliked { get; set; }

        public virtual Developer? Dev { get; set; }
        public virtual Developer? Publisher { get; set; }
        public virtual ICollection<GameCategory> GameCategories { get; set; }
        public virtual ICollection<GameImage> GameImages { get; set; }
        public virtual ICollection<Library> Libraries { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<ShoppingCart> ShoppingCarts { get; set; }
    }
}
