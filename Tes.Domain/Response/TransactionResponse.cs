using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tes.Domain
{
    public class TransactionResponse
    {
        public bool? IsSuccess { get; set; } = true;
        public string? Message { get; set; }
        public object? Data { get; set; }
    }
}
