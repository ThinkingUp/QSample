using System;

namespace ELabel.Web.Models
{
    public class RecoveryIntervalsDTO
    {
        public string HOLEID { get; set; }
        public string PROJECTCODE { get; set; }
        public double? GEOLFROM { get; set; }
        public double? GEOLTO { get; set; }
        public double? PRIORITY { get; set; }
        public double? Recovery_m { get; set; }
        public string RecovGeo { get; set; }
        public DateTime? RecovGeoDate { get; set; }
        public double? CoreLength { get; set; }
        public double? RecoveryRatio { get; set; }
        public int ID { get; set; }
        public string LL_WGS84_Calc_X { get; set; }
        public string LL_WGS84_Calc_Y { get; set; }
    }
}