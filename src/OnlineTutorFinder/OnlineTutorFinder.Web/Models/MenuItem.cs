namespace OnlineTutorFinder.Web.Models
{
    public class MenuBar
    {
        public IList<MenuItem>? MenuItems { get; set; }
    }

    public class MenuItem
    {
        public string? Name { get; set; }
        public string? Link { get; set; }
        public string? Icon { get; set; }
        public bool HasChild { get; set; }

        public IList<ChildItem>? ChildItems { get; set; }
    }

    public class ChildItem
    {
        public string? Name { get; set; }
        public string? Link { get; set; }
        public string? Icon { get; set; }
    }
}
