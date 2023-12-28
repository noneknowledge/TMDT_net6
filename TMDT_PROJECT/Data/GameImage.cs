using System;
using System.Collections.Generic;

namespace TMDT_PROJECT.Data
{
    public partial class GameImage
    {
        public string GameId { get; set; } = null!;
        public string ImageId { get; set; } = null!;
        public string? ImageName { get; set; }

        public virtual Game Game { get; set; } = null!;
    }
}
