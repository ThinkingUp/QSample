using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELabel.QC.AlertClasses
{
    public class BasicAlert
    {
        public string AlertDefinition { get; set; }

        public string LabelID { get; set; }

        public DateTime Date { get; set; }

        public string FieldName { get; set; }

        public string Description { get; set; }
    }
}
