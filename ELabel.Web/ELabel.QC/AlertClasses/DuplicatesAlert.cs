using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELabel.QC.AlertClasses
{
    public class DuplicatesAlert
    {
        public string SampleID {get;set;}
        public string DespatchNo { get; set; }
        public string Component { get; set; }
        public string ValuePrimary { get; set; }
        public string ValueDuplicate { get; set; }
        public string Difference { get; set; }
        public DateTime DateRaised { get; set; }
    }
}
