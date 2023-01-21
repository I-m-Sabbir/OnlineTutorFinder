using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Imaging;

namespace OnlineTutorFinder.Web.Extensions;

public class ImageAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        var file = value as IFormFile;
        if (file == null)
            return true;

        if(file.Length > 1 * 1024 * 1024)
            return false;

        try
        {
            using (var img = Image.FromStream(file!.OpenReadStream()))
            {
                return img.RawFormat.Equals(ImageFormat.Png) || img.RawFormat.Equals(ImageFormat.Jpeg);
            }
        }
        catch { }
        return false;
        
    }
}
