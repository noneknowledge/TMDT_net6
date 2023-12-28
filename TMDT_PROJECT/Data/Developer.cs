using System;
using System.Collections.Generic;

namespace TMDT_PROJECT.Data
{
    public partial class Developer
    {
        public Developer()
        {
            GameDevs = new HashSet<Game>();
            GamePublishers = new HashSet<Game>();
        }

        public string DevId { get; set; } = null!;
        public string Developer1 { get; set; } = null!;
        public string? Description { get; set; }
        public string? Logo { get; set; }
        public string? IsHide { get; set; }
        public string? Alias { get; set; }

        public virtual ICollection<Game> GameDevs { get; set; }
        public virtual ICollection<Game> GamePublishers { get; set; }
    }
}
