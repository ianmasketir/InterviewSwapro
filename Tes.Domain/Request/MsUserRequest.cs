using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PORECT.Helper;

namespace Tes.Domain
{
    public class MsUserRequest
    {
        public int? ID { get; set; }
        public string? TransactionType { get; set; } = "View";
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        //public string? Role { get; set; }
        /// <summary>
        /// colName = "add/disable/enable", value = "rolename"
        /// </summary>
        public List<ParamTaskViewModel>? Roles { get; set; }
        public bool? IsActive { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDtm { get; set; }
    }

    public class SearchUserRequest
    {
        public int? ID { get; set; }
        public string? Username { get; set; }
        public string? Role { get; set; }
    }
}
