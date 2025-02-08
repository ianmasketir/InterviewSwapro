using System;
using System.Collections.Generic;

namespace Tes.Data.Models
{
    public partial class MsRole
    {
        public int Id { get; set; }
        public string Role { get; set; } = null!;
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
        public string CreatedBy { get; set; } = null!;
        public DateTime CreatedDtm { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDtm { get; set; }
    }
}
