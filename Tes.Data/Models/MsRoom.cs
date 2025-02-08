using System;
using System.Collections.Generic;

namespace Tes.Data.Models
{
    public partial class MsRoom
    {
        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public byte Capacity { get; set; }
        public decimal Price { get; set; }
        public bool? IsActive { get; set; }
        public string CreatedBy { get; set; } = null!;
        public DateTime CreatedDtm { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDtm { get; set; }
    }
}
