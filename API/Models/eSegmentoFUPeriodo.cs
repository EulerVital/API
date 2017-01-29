using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class eSegmentoFUPeriodo
    {
        //1° Periodo
        public int DiaDe { get; set; }
        public int MesDe { get; set; }
        public int AnoDe { get; set; }
        public int DiaAte { get; set; }
        public int MesAte { get; set; }
        public int AnoAte { get; set; }

        //2° Periodo
        public int SegDiaDe { get; set; }
        public int SegMesDe { get; set; }
        public int SegAnoDe { get; set; }
        public int SegDiaAte { get; set; }
        public int SegMesAte { get; set; }
        public int SegAnoAte { get; set; }

        public int? TipoVenda { get; set; }
        public int? RegiaoOperacional { get; set; }
        public int? RegiaoGeografico { get; set; }
        public int? RegiaoMetropolitana { get; set; }
        public int? Cidade { get; set; }
        public int? Estado { get; set; }
        public int? AreaOperacional { get; set; }

        public string Segmento { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string MarcaCliente { get; set; }
        public string TotalSeg1 { get; set; }
        public string TotalSeg2 { get; set; }
        public string TotalGeral1 { get; set; }
        public string TotalGeral2 { get; set; }
        public string PorcSeg1 { get; set; }
        public string PorcTotal1 { get; set; }
        public string PorcSeg2 { get; set; }
        public string PorcTotal2 { get; set; }
        public string PorcenGeral1 { get; set; }
        public string PorcenGeral2 { get; set; }
        public string Primeiro { get; set; }
        public string Segundo { get; set; }

    }
}