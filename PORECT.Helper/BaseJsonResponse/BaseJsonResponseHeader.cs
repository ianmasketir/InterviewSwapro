using System.Collections.Generic;

namespace PORECT.Helper
{
    public class BaseJsonResponseHeader
    {
        //public long processTime = 0;
        public IList<BaseJsonResponseError> Errors = new List<BaseJsonResponseError>();
    }
}
