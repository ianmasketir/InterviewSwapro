using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tes.Domain
{
    public class MsRoomViewModel
    {
        public string? FormMode { get; set; } = "View";
        public MsRoomResponse? Room { get; set; }
    }
}
