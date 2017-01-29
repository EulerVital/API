using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class eSegmentoFU
    {
        public string MarcaCliente { get; set; }
        public string VD { get; set; }
        public string Varejo { get; set; }
        public string TotalVol { get; set; }
        public string TotalVolVD { get; set; }
        public string TotalVolVarejo { get; set; }
        public string TotalVolTotal { get; set; }
        public string SomaTotaisVD { get; set; }
        public string SomaTotaisVarejo { get; set; }
        public string SomaTotaisTotal { get; set; }
        public string PorcenSegVD { get; set; }
        public string PorcenSegVarejo { get; set; }
        public string PorcenSegTotal { get; set; }
        public string PorcenTotalVD { get; set; }
        public string PorcenTotalVarejo { get; set; }
        public string PorcenTotalTotal { get; set; }
        public string TotalPorcentSegVD { get; set; }
        public string TotalPorcentSegVarejo { get; set; }
        public string TotalPorcenTotalVD { get; set; }
        public string TotalPorcenTotalVarejo { get; set; }
        public string TotalPorcenTotalTotal { get; set; }
        public List<eSegmentoFU> listaSub { get; set; }
        public string TituloLocalizacao { get; set; }
        public string Localizacao { get; set; }
        public string Qtd { get; set; }
        public string TotalGeralSegmento { get; set; }
        public string TotalGeralAnual { get; set; }
        public string PorcentSeg { get; set; }
        public string PorcentTotal { get; set; }
        public string PorcentTotalGeralSeg { get; set; }
        public string ListaAnos { get; set; }
        public string DiaUtil { get; set; }


        //Parametros
        public int Dia { get; set; }
        public int Mes { get; set; }
        public int Ano { get; set; }
        public int DiaDe { get; set; }
        public int MesDe { get; set; }
        public int AnoDe { get; set; }
        public int TipoVenda { get; set; }
        public int RegiaoOperacional { get; set; }
        public int RegiaoGeografico { get; set; }
        public int RegiaoMetropolitana { get; set; }
        public int Estado { get; set; }
        public int Cidade { get; set; }
        public int AreaOperacional { get; set; }
        public string Segmento { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
    }
}