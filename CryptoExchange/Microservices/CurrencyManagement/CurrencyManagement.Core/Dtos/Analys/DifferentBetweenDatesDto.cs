using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyManagement.Core.Dtos.Analys
{
    public class DifferentBetweenDatesDto
    {
        public double Different { get; set; }
        public double Percents { get; set; }

        public string? Message { get; set; }
    }
}
