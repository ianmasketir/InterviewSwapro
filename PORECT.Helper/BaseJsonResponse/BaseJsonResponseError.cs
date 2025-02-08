using Newtonsoft.Json;

namespace PORECT.Helper
{
    public class BaseJsonResponseError
    {
        public string Message { get; set; }
        public string Cause { get; set; }
        public string Code { get; set; }

        public BaseJsonResponseError() { }

        public BaseJsonResponseError(string message, string cause, string code)
        {
            this.Message = message;
            this.Cause = cause;
            this.Code = code;
        }

      
    }
}
