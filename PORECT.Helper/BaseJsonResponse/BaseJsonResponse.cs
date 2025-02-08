using Newtonsoft.Json;

namespace PORECT.Helper
{
    public class BaseJsonResponse
    {
        public BaseJsonResponseHeader Header { set; get; }
        public object Data { set; get; }

        public BaseJsonResponse()
        {
            this.Header = new BaseJsonResponseHeader();
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
