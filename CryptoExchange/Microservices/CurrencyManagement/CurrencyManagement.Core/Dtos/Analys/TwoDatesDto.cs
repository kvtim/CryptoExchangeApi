using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyManagement.Core.Dtos.Analys
{
    public class TwoDatesDto
    {
        public required DateTime FromDate { get; set; }
        public required DateTime EndDate { get; set; }
    }
}
