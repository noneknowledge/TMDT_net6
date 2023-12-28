
using TMDT_PROJECT.Data;

namespace TMDT_Project.ViewModel
{
    public class LibraryVM
    {
        public string Uid { get; set; } = null!;

        public string GameId { get; set; } = null!;

        public string? FeedBack { get; set; }

        public string? IsLiked { get; set; }

        public virtual Game Game { get; set; } = null!;
        public ICollection<Library> libraries { get; set; }
    }
}
