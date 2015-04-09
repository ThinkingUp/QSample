using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELabel.QC.AlertClasses;

namespace ELabel.QC
{
    public class Alerts
    {
        // this is the view model that has the info about the alert
        public List<RangeAlert> AlertsStandardReference { get; set; }

        public List<RangeAlert> AlertsContaminationCheck { get; set; }

        public List<BasicAlert> AlertsBasic { get; set; }

        public List<DuplicatesAlert> AlertsDuplicates { get; set; }

        public List<MethodAlert> AlertsMethod { get; set; }

    }
}