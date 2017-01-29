using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class eDetalhamento
    {
        public string AreaConc { get; set; }
        public string IdLocalizacao { get; set; }
        public string IdAreaOperacional { get; set; }
        public string NomeAreaOperacional { get; set; }
        public string IdCidade { get; set; }
        public string NomeCidade { get; set; }
        public string Vol1 { get; set; }
        public string Vol2 { get; set; }
        public string Vol3 { get; set; }
        public string Vol4 { get; set; }
        public string Vol5 { get; set; }
        public string Acumulado { get; set; }
        public string TotalVol1 { get; set; }
        public string TotalVol2 { get; set; }
        public string TotalVol3 { get; set; }
        public string TotalVol4 { get; set; }
        public string TotalVol5 { get; set; }
        public string TotalAcumulado { get; set; }
        public string PorcentVol1 { get; set; }
        public string PorcentVol2 { get; set; }
        public string PorcentVol3 { get; set; }
        public string PorcentVol4 { get; set; }
        public string PorcentVol5 { get; set; }
        public string PorcentAcumulado { get; set; }
        public string Nome { get; set; }
        public string Grupo { get; set; }

        //Parametros
        public int AteDia { get; set; }
        public int AteMes { get; set; }
        public int AteAno { get; set; }
        public int Categoria { get; set; }
        public int TipoVenda { get; set; }
        public string Segmento { get; set; }
        public int RegiaoOperacional { get; set; }
        public int RegiaoGeografico { get; set; }
        public int Estado { get; set; }
        public int RegiaoMetropolitana { get; set; }
        public int Cidade { get; set; }
        public int AreaOperacional { get; set; }
        public int Concessionaria { get; set; }
        public int ByGroup { get; set; }
        public int FlDiasUteis { get; set; }

        //Dias uteis e Validos

        public string Dia1 { get; set; }
        public string Dia2 { get; set; }
        public string Dia3 { get; set; }
        public string Dia4 { get; set; }
        public string Dia5 { get; set; }
        public string DiasValidos { get; set; }
    }
}