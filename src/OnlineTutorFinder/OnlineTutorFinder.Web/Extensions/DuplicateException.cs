namespace OnlineTutorFinder.Web.Extensions;

public class DuplicateException : Exception
{
    public DuplicateException(string message)
        : base(message)
    {
    }
}
