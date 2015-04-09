using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELabel.QC.AlertClasses
{
    public class RangeAlert
    {
        public string ErrorCause { get; set; }
        public string SampleID { get; set; }
        public string DispatchNo { get; set; }
        public string StandardID { get; set; }
        public string ActualValue { get; set; }
        public string StandardValue { get; set; }
        public string Difference { get; set; }
        public DateTime DateRaised { get; set; }
    }
}
