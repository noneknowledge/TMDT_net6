
using TMDT_PROJECT.Data;

namespace TMDT_Project.ViewModel
{
    public class GameVM
    {
        public string GameId { get; set; } = null!;

        public string? GameName { get; set; }

        public string? GameDes { get; set; }

        public double? Price { get; set; }

        public string? Thumbnail { get; set; }

        public string? Video { get; set; }

        public string? DevId { get; set; }

        public string? PublisherId { get; set; }

        public int? Liked { get; set; }

        public int? Disliked { get; set; }

        public DateTime? ReleaseDate { get; set; }
        public int? View { get; set; }
        public string? IsActive { get; set; }
        public int? Discount { get; set; }
        public DateTime? StartDis { get; set; }
        public DateTime? EndDis { get; set; }

        public string? Alias { get; set; }
        public int? Sales { get {
            if (StartDis < DateTime.Now && DateTime.Now < EndDis) {  return Discount; }
                else if (StartDis == null && DateTime.Now < EndDis) return Discount;
                else if (StartDis < DateTime.Now && EndDis == null) return Discount;
                return 0;
            } }
        public double? CurrPrice
        {
            get {
                
                return Math.Round((Price - Price * Sales / 100).Value, 2);
            }
        }

        public virtual Developer? Dev { get; set; }

        public virtual ICollection<GameCategory> GameCategories { get; set; } = new List<GameCategory>();

        public virtual ICollection<GameImage> GameImages { get; set; } = new List<GameImage>();

        public virtual ICollection<Library> Libraries { get; set; } = new List<Library>();

        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

        public virtual Developer? Publisher { get; set; }

        public virtual ICollection<ShoppingCart> ShoppingCarts { get; set; } = new List<ShoppingCart>();
    }
}
