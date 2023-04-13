using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyManagement.Core.Models
{
    public class CurrencyDimension
    {
        public int Id { get; set; }
        public decimal PriceInUSD { get; set; }
        public DateTime? FromDate { get; set; } = DateTime.Now;
        public DateTime? EndDate { get; set; }
        public bool IsCurrent { get; set; }

        public int CurrencyId { get; set; }
        public Currency? Currency { get; set; }
    }
}
