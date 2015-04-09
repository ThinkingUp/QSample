using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELabel.QC
{
    // this class represents a single row in the lab analysis csv
    // do not change names of fields since they have to match the csv
    public class Label
    {
        public string SAMPLEID { get; set; }
        public string DUPLICATENO { get; set; }
        public string DESPATCHNO { get; set; }
        public string LABJOBNO { get; set; }
        public string STANDARDID { get; set; }
        public string PCHECKID { get; set; }
        public string PDUPLICATENO { get; set; }
        public string CHECKSTAGE { get; set; }
        public string Ag_MEICP41s_ppm { get; set; }
        public string Ag_MEICP61s_ppm { get; set; }
        public string Ag_MEMS81_ppm { get; set; }
        public string Al_MEICP41s_pct { get; set; }
        public string Al2O3_MEICP85_pct { get; set; }
        public string As_MEICP41s_ppm { get; set; }
        public string Au_AA25_ppm { get; set; }
        public string Au_PGMICP23_ppm { get; set; }
        public string Au_TL43_ppm { get; set; }
        public string B_MEICP41s_ppm { get; set; }
        public string Ba_MEICP41s_ppm { get; set; }
        public string Ba_MEICP43_ppm { get; set; }
        public string Ba_MEMS81_ppm { get; set; }
        public string BaO_MEICP85_pct { get; set; }
        public string Be_MEICP41s_ppm { get; set; }
        public string Bi_MEICP41s_ppm { get; set; }
        public string Ca_MEICP41s_pct { get; set; }
        public string Ca_MEICP43_pct { get; set; }
        public string CaO_MEICP85_pct { get; set; }
        public string Cd_MEICP41s_ppm { get; set; }
        public string Ce_MEICP41s_ppm { get; set; }
        public string Ce_MEMS81_ppm { get; set; }
        public string Co_MEICP41s_ppm { get; set; }
        public string Co_MEICP61s_ppm { get; set; }
        public string Co_MEMS81_ppm { get; set; }
        public string Cr_MEICP41s_ppm { get; set; }
        public string Cr_MEMS81_ppm { get; set; }
        public string Cs_MEMS81_ppm { get; set; }
        public string Cu_CUOG46_pct { get; set; }
        public string Cu_MEICP41s_ppm { get; set; }
        public string Cu_MEICP43_ppm { get; set; }
        public string Cu_MEICP61s_ppm { get; set; }
        public string Cu_MEMS81_ppm { get; set; }
        public string Cu_MEOG46_pct { get; set; }
        public string Cu_MEOG62_pct { get; set; }
        public string Dy_MEMS81_ppm { get; set; }
        public string Er_MEMS81_ppm { get; set; }
        public string Eu_MEMS81_ppm { get; set; }
        public string Fe_MEICP41s_pct { get; set; }
        public string Fe_MEICP43_pct { get; set; }
        public string Ga_MEICP41s_ppm { get; set; }
        public string Ga_MEMS81_ppm { get; set; }
        public string Gd_MEMS81_ppm { get; set; }
        public string Hf_MEMS81_ppm { get; set; }
        public string Hg_MEICP41s_ppm { get; set; }
        public string Ho_MEMS81_ppm { get; set; }
        public string K_MEICP41s_pct { get; set; }
        public string K2O_MEICP85_pct { get; set; }
        public string La_MEICP41s_ppm { get; set; }
        public string La_MEMS81_ppm { get; set; }
        public string LOI_MEGRA05_pct { get; set; }
        public string Lu_MEMS81_ppm { get; set; }
        public string Mg_MEICP41s_pct { get; set; }
        public string Mg_MEICP43_pct { get; set; }
        public string MgO_MEICP85_pct { get; set; }
        public string Mn_MEICP41s_ppm { get; set; }
        public string Mn_MEICP43_ppm { get; set; }
        public string MnO_MEICP85_pct { get; set; }
        public string Mo_MEICP41s_ppm { get; set; }
        public string Mo_MEICP61s_ppm { get; set; }
        public string Mo_MEMS81_ppm { get; set; }
        public string Mo_MEOG62_pct { get; set; }
        public string Mo_MOOG62_pct { get; set; }
        public string Na_MEICP41s_pct { get; set; }
        public string Na2O_MEICP85_pct { get; set; }
        public string Nb_MEMS81_ppm { get; set; }
        public string Nd_MEMS81_ppm { get; set; }
        public string Ni_MEICP41s_ppm { get; set; }
        public string Ni_MEICP43_ppm { get; set; }
        public string Ni_MEICP61s_ppm { get; set; }
        public string Ni_MEMS81_ppm { get; set; }
        public string P_MEICP41s_ppm { get; set; }
        public string P2O5_MEICP85_pct { get; set; }
        public string Pass4mm_CRUQC_pct { get; set; }
        public string Pass75um_PULQC_pct { get; set; }
        public string Pb_MEICP41s_ppm { get; set; }
        public string Pb_MEICP43_ppm { get; set; }
        public string Pb_MEICP61s_ppm { get; set; }
        public string Pb_MEMS81_ppm { get; set; }
        public string Pd_PGMICP23_ppm { get; set; }
        public string Pr_MEMS81_ppm { get; set; }
        public string Pt_PGMICP23_ppm { get; set; }
        public string Rb_MEMS81_ppm { get; set; }
        public string Re_REOG62_ppm { get; set; }
        public string S_MEICP41s_pct { get; set; }
        public string S_MEICP43_pct { get; set; }
        public string Sb_MEICP41s_ppm { get; set; }
        public string Sc_MEICP41s_ppm { get; set; }
        public string SiO2_MEICP85_pct { get; set; }
        public string Sm_MEMS81_ppm { get; set; }
        public string Sn_MEICP41s_ppm { get; set; }
        public string Sn_MEMS81_ppm { get; set; }
        public string Sr_MEICP41s_ppm { get; set; }
        public string Sr_MEMS81_ppm { get; set; }
        public string SrO_MEICP85_pct { get; set; }
        public string Ta_MEMS81_ppm { get; set; }
        public string Tb_MEMS81_ppm { get; set; }
        public string Th_MEICP41s_ppm { get; set; }
        public string Th_MEMS81_ppm { get; set; }
        public string Ti_MEICP41s_pct { get; set; }
        public string TiO2_MEICP85_pct { get; set; }
        public string Tl_MEICP41s_ppm { get; set; }
        public string Tl_MEMS81_ppm { get; set; }
        public string Tm_MEMS81_ppm { get; set; }
        public string U_MEICP41s_ppm { get; set; }
        public string U_MEMS81_ppm { get; set; }
        public string V_MEICP41s_ppm { get; set; }
        public string V_MEMS81_ppm { get; set; }
        public string W_MEICP41s_ppm { get; set; }
        public string W_MEMS81_ppm { get; set; }
        public string Y_MEMS81_ppm { get; set; }
        public string Yb_MEMS81_ppm { get; set; }
        public string Zn_MEICP41s_ppm { get; set; }
        public string Zn_MEICP43_ppm { get; set; }
        public string Zn_MEICP61s_ppm { get; set; }
        public string Zn_MEMS81_ppm { get; set; }
        public string Zn_MEOG62_pct { get; set; }
        public string Zr_MEMS81_ppm { get; set; }
    }
}
