namespace Reactivities.Aplication.Core
{
    public class AppException
    {
        public AppException(int statusCode, string message, string details = null)
        {
            StatusCode = statusCode;
            Message = message;
            Details = details;
        }

        public int StatusCode { get; set; } //This is the [Error Code] basically.
        public string Message { get; set; } //This will be the [Standart Message] for [Example] -> [Server Error].
        public string Details { get; set; } // This is the [Stack Trace] From the [Exception].
    }
}
