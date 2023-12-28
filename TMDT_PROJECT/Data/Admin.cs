using System;
using System.Collections.Generic;

namespace TMDT_PROJECT.Data
{
    public partial class Admin
    {
        public string AdminId { get; set; } = null!;
        public string? AdName { get; set; }
        public string? PassWord { get; set; }
        public string? FullName { get; set; }
        public string? IsActive { get; set; }
        public string? IsHide { get; set; }
        public string? RandomKey { get; set; }
    }
}
