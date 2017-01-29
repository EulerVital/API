using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    //classe responsavel pelos relatorios: Invasao,InvasaoArea

    public class eInvasao
    {
        public int Qtd { get; set; }
        public string EmpresaFantasia {get; set; }
        public string SiglaAreaOperacional { get; set; }
        public string Cidade { get; set; }
        public string NomeAreaOperacional { get; set; }
        public string TotalArea { get; set; }
        public string TotalVolume { get; set; }
        public string TVolAreaMunicipio { get; set; }
        public string PorcentagemArea { get; set; }
        public string PorcentagemMunicipio { get; set; }
        public List<eInvasao> listaSub { get; set; }

        //Parametros
        public int AteDia { get; set; }
        public int AteMes { get; set; }
        public int AteAno { get; set; }
        public int DeDia { get; set; }
        public int DeMes { get; set; }
        public int DeAno { get; set; }
        public int AreaOperacional { get; set; }

    }
}