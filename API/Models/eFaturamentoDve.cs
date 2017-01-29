using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class eFaturamentoDve
    {
        public string TipoCliente { get; set; }
        public string Sigla { get; set; }
        public string TipoDeCliente { get; set; }
        public string AreaDivisao { get; set; }
        public int UltimoMes { get; set; }
        public int UltimoAno { get; set; }
        public int UltimoDia { get; set; }
        public List<eFaturamentoDve> ListaTotalMes { get; set; }
        public int TotalMes { get; set; }
        public int TotalGeral { get; set; }
        public int TotalSigla { get; set; }
        public double Participacao { get; set; }

        //Parâmetros
        public int Dia { get; set; }
        public int Mes { get; set; }
        public int Ano { get; set; }
        public string ModalidadeVenda { get; set; }
    }
}