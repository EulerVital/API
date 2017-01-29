using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class eBasket
    {
        public string Descricao { get; set; }
        public string Codigo { get; set; }
        public int Qtd { get; set; }
        public string NomenclaturaMarca { get; set; }
        public string NomenclaturaModelo { get; set; }
        public string NomenclaturaVersao { get; set; }
        public int Ano { get; set; }
        public int Mes { get; set; }
        public int Dia { get; set; }
        public string TotalMes { get; set; }
        public string TotalMesGeral { get; set; }
        public string Total { get; set; }
        public string TotalGeral { get; set; }
        public string TotalAno { get; set; }
        public string Porcentagem { get; set; }
        public List<eBasket> listaSub { get; set; }

        //Parâmetros
        public int? DiaDe { get; set; }
        public int? MesDe { get; set; }
        public int? AnoDe { get; set; }
        public string BasketId { get; set; }
        public int? RegiaoOperacional { get; set; }
        public int? RegiaoGeografico { get; set; }
        public int? RegiaoMetropolitana { get; set; }
        public int? Cidade { get; set; }
        public int? Estado { get; set; }
        public int? AreaOperacional { get; set; }

    }
}