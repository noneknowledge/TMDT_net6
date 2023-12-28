namespace TMDT_Project.ViewModel
{
    public class RegisterVM
    {
        public string FullName { get; set; } = null!;

        public string UserName { get; set; } = null!;

        public string PassWord { get; set; } = null!;

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? IsActive { get; set; }

        public string? IsHide { get; set; }
    }
}
