﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELabel.QC
{
    public class ContaminationCheckModel
    {
        public string SAMPLEID { get; set; }
        public string DUPLICATENO { get; set; }
        public Nullable<double> DESPATCHNO { get; set; }
        public string LABJOBNO { get; set; }
        public string STANDARDID { get; set; }
        public string PCHECKID { get; set; }
        public string PDUPLICATENO { get; set; }
        public string CHECKSTAGE { get; set; }
        public string Ag_MEICP41s_ppm { get; set; }
        public Nullable<double> STD_Ag_MEICP41s_ppm { get; set; }
        public Nullable<double> MIN_Ag_MEICP41s_ppm { get; set; }
        public Nullable<double> MAX_Ag_MEICP41s_ppm { get; set; }
        public Nullable<double> Al_MEICP41s_pct { get; set; }
        public Nullable<double> STD_Al_MEICP41s_pct { get; set; }
        public Nullable<double> MIN_Al_MEICP41s_pct { get; set; }
        public Nullable<double> MAX_Al_MEICP41s_pct { get; set; }
        public Nullable<double> As_MEICP41s_ppm { get; set; }
        public Nullable<double> STD_As_MEICP41s_ppm { get; set; }
        public Nullable<double> MIN_As_MEICP41s_ppm { get; set; }
        public Nullable<double> MAX_As_MEICP41s_ppm { get; set; }
        public Nullable<double> Au_AA25_ppm { get; set; }
        public Nullable<double> STD_Au_AA25_ppm { get; set; }
        public Nullable<double> MIN_Au_AA25_ppm { get; set; }
        public Nullable<double> MAX_Au_AA25_ppm { get; set; }
    }
}
