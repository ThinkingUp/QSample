using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELabel.QC.AlertClasses
{
    public class MethodAlert
    {
        public string MethodName { get; set; }
        public string TotalUses { get; set; }
        public string TotalFailures { get; set; }
        public string Percentage { get; set; }
        public DateTime DateRaised { get; set; }
    }
}
