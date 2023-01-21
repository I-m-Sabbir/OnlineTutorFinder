using System.ComponentModel.DataAnnotations;

namespace OnlineTutorFinder.Web.Extensions;

public class OnlyAcceptAttribute : ValidationAttribute
{
    private readonly string[] _acceptedvalues;

    public OnlyAcceptAttribute(params string[] acceptedvalues)
    {
        _acceptedvalues = acceptedvalues;
    }

    public override bool IsValid(object? value)
    {
        if (value == null)
            return true;

        if(_acceptedvalues.Length != 0)
        {
            return _acceptedvalues.Contains(value);
        }

        return false;
    }
}
