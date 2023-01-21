namespace OnlineTutorFinder.Web.Models;

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
