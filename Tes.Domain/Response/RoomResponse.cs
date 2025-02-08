using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tes.Domain
{
    public class MsRoomResponse
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public byte? Capacity { get; set; }
        public decimal Price { get; set; }
        public string? BookingStatus { get; set; }
        public BookingResponse? Booking { get; set; }
        public bool? IsAvailable { get; set; }
        public DateTime? AvailableDate { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDtm { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDtm { get; set; }
    }
    public class BookingResponse
    {
        public string? Code { get; set; }
        public DateTime? CheckOutDate { get; set; }
        public string? Status { get; set; }
    }

}
