using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PORECT.Helper
{
    public static class GuidExtensions
    {
        /// <summary>
        /// Get Object ID from GUID
        /// </summary>
        /// <param name="guid">Guid</param>
        /// <returns>Object ID as string</returns>
        public static string GetObjectID(this Guid guid)
        {
            try
            {
                string[] split = guid.ToString().Split('-');
                return split[0];
            }
            catch
            {
                throw;
            }
        }
    }
}
