using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PORECT.Helper
{
    public class ReportBookingResponse
    {
        public int? No { get; set; }
        public string RoomCode { get; set; }
        public string RoomName { get; set; }
        public int BookedQty { get; set; }
        public int TotalDuration { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
