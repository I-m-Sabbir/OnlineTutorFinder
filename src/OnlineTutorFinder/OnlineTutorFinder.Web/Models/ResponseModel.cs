namespace OnlineTutorFinder.Web.Models
{
    public class ResponseModel
    {
        public string? Message { get; set; }
        public ResponseTypes Type { get; set; }
    }

    public enum ResponseTypes
    {
        Success,
        Danger
    }
}
