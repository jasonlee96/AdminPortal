namespace CommonService.Models
{
    public class CommonApiResponse
    {
        public object Result { get; set; }
        public bool Success { get; set; } = true;
        public string Message { get; set; } = "Success";
        public string StackTrace { get; set; }

        public CommonApiResponse()
        {

        }
    }

    public class CommonApiResponse<T> : CommonApiResponse
    {
        public CommonApiResponse(T result)
        {
            Result = result;
        }
    }
}
