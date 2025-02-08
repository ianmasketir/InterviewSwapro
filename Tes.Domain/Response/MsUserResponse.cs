using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tes.Domain
{
    public class MsUserResponse
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public List<string>? Roles { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDtm { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDtm { get; set; }
    }
}
