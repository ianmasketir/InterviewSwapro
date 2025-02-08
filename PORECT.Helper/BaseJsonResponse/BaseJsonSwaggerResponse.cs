using Newtonsoft.Json;

namespace PORECT.Helper
{
    public class BaseJsonSwaggerResponse<T>
    {
        public BaseJsonResponseHeader Header { set; get; }
        public T Data { set; get; }

        public BaseJsonSwaggerResponse()
        {
            this.Header = new BaseJsonResponseHeader();
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
