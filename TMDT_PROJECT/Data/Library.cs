using System;
using System.Collections.Generic;

namespace TMDT_PROJECT.Data
{
    public partial class Library
    {
        public string Uid { get; set; } = null!;
        public string GameId { get; set; } = null!;
        public string? FeedBack { get; set; }
        public string? IsLiked { get; set; }

        public virtual Game Game { get; set; } = null!;
        public virtual Client UidNavigation { get; set; } = null!;
    }
}
