namespace TMDT_Project.ViewModel
{
    public class Dev_CateVM
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? logo { get; set; }

        public string? Alias { get; set; }
        public IEnumerable<GameVM> Games { get; set; }
    }
}
