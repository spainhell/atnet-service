using System;
using System.Collections.Generic;
using System.Text;

namespace yrno
{
    public class ForecastRecordModel
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public decimal Temperature { get; set; }
    }
}
