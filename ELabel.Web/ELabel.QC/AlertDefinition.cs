using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELabel.QC
{
    public class AlertDefinition
    {
        public int Id { get; set; }
        public string DefinitionName { get; set; }
        public string compareField { get; set; } // field name to compare values from
        public string MineralPrefix { get; set; } // eg. Ag, Au, Al
        public bool Active { get; set; }
        public DateTime LastUpdated { get; set; }
        public double Minvalue { get; set; }
        public double MaxValue { get; set; }
        public double TolerancePercentage { get; set; } // will be compared to +- this percentage
        public string Valueoperator { get; set; }
    }
}
