using StudentManagementApplication.Models;

namespace StudentManagementApplication.Contracts;

public static class CreateResponse
{

    public static Response CreateSuccessfulResponse(Object obj, string description)
    {
        return new Response()
        {
            Error = null,
            ResponseCode = "00",
            ResponseDescription = description,
            ResponseData = obj
        };

    }

    public static Response CreateUnsuccessfulResponse(string ResponseCode, string ResponseDescription, string ErrorTitle, string ErrorMessage)
    {
        return new Response()
        {
            ResponseCode = ResponseCode,
            ResponseDescription = ResponseDescription,
            ResponseData = null,
            Error = new ErrorResponse() { Title = ErrorTitle, ErrorMessage = ErrorMessage }
        };

    }
}
