using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class eFaturamentoDvr
    {
        public string Bir { get; set; }
        public string Concessionaria { get; set; }
        public int TotalGeral { get; set; }
        public string Participacao { get; set; }
        public string Mes { get; set; }
        public string UltimoMes { get; set; }
        public string UltimoAno { get; set; }
        public string UltimoDia { get; set; }
        public int Total { get; set; }
        public string TotalMes { get; set; }
        public int Qtd { get; set; }

        //parametros
        public string dia { get; set; }
        public string mes { get; set; }
        public string ano { get; set; }

    }
}