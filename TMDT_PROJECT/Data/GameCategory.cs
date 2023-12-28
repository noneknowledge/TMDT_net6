using System;
using System.Collections.Generic;

namespace TMDT_PROJECT.Data
{
    public partial class GameCategory
    {
        public string GameId { get; set; } = null!;
        public string CateId { get; set; } = null!;
        public string? Note { get; set; }

        public virtual Category Cate { get; set; } = null!;
        public virtual Game Game { get; set; } = null!;
    }
}
