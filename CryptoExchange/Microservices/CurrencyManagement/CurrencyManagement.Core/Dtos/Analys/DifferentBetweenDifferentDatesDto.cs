using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyManagement.Core.Dtos.Analys
{
    public class DifferentBetweenDifferentDatesDto
    {
        public double Different { get; set; }
        public double Percents { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime EndDate { get; set; }

        public string? Message { get; set; }
    }
}
