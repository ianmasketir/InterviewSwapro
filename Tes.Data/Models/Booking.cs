using System;
using System.Collections.Generic;

namespace Tes.Data.Models
{
    public partial class Booking
    {
        public int Id { get; set; }
        public string ObjectId { get; set; } = null!;
        public string RoomCode { get; set; } = null!;
        public string Username { get; set; } = null!;
        public byte Duration { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public string Status { get; set; } = null!;
        public string CreatedBy { get; set; } = null!;
        public DateTime CreatedDtm { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDtm { get; set; }
    }
}
