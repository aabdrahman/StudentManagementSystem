namespace StudentManagementApplication.Models;

public class Response
{
    public string ResponseCode { get; set; }
    public string ResponseDescription { get; set; }
    public Object ResponseData { get; set; }
    public ErrorResponse Error { get; set; } = null!;

}
