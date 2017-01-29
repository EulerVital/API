using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class eBasketPeriodo
    {

        //1° Periodo
        public int DiaDe { get; set; }
        public int MesDe { get; set; }
        public int AnoDe { get; set; }
        public int Dia { get; set; }
        public int Mes { get; set; }
        public int Ano { get; set; }

        //2° Periodo
        public int SegDiaDe { get; set; }
        public int SegMesDe { get; set; }
        public int SegAnoDe { get; set; }
        public int SegDia { get; set; }
        public int SegMes { get; set; }
        public int SegAno { get; set; }

        public string BasketId { get; set; }
        public int? RegiaoOperacional { get; set; }
        public int? RegiaoGeografico { get; set; }
        public int? RegiaoMetropolitana { get; set; }
        public int? Cidade { get; set; }
        public int? Estado { get; set; }
        public int? AreaOperacional { get; set; }

        public string Descricao { get; set; }
        public string Codigo { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Versao { get; set; }
        public string DiasUteis1 { get; set; }
        public string DiasUteis2 { get; set; }
        public string Periodo1 { get; set; }
        public string Periodo2 { get; set; }
    }
}