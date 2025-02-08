using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Tes.Domain
{
    public class MsRoomRequest
    {
        public int? ID { get; set; }
        public string? TransactionType { get; set; } = "Insert";
        public string? Code { get; set; }
        public string? Name { get; set; }
        public byte? Capacity { get; set; }
        public decimal? Price { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDtm { get; set; }
    }

    public class SearchRoomRequest
    {
        public int? ID { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public byte? Capacity { get; set; }
        public string? BookingStatus { get; set; }
        public decimal? PriceFrom { get; set; }
        public decimal? PriceTo { get; set; }
        public DateTime? AvailableFrom { get; set; }
        public DateTime? AvailableTo { get; set; }
    }

    public class BookingRequest
    {
        public int? ID { get; set; }
        public string? TransactionType { get; set; } = "Insert";
        public string? ObjectID { get; set; }
        public string? Code { get; set; }
        public string? Username { get; set; }
        public byte? Duration { get; set; }
        public DateTime? CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }
        /// <summary>
        /// Booked/CheckedIn/CheckedOut
        /// </summary>
        public string? Status { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDtm { get; set; }
    }
}
