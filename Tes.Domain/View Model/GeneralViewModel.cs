using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Tes.Domain
{
    public class GeneralViewModel
    {
        public int? ID { get; set; }
        /// <summary>
        /// Insert/Update/View
        /// </summary>
        public string? FormMode { get; set; } = "View";
        public string? Value { get; set; }
    }
    public class UploadViewModel
    {
        public IFormFile File { get; set; }
        public string CreatedBy { get; set; } = "System";
    }
}
