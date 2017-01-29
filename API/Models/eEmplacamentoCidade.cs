using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class eEmplacamentoCidade
    {
        public string Ranking { get; set; }
        public string CorLinha { get; set; }
        public string EmpresaEmpId { get; set; }
        public string NomeCidade { get; set; }
        public string IdCidade { get; set; }
        public string Ano { get; set; }
        public string Mes { get; set; }
        public string ConfereEmpresa { get; set; }
        public string NomeEmpresa { get; set; }
        public string NomeclaturaModelos { get; set; }
        public string IdLocalizacaoOperacional { get; set; }
        public string NomeLocalizacaoOperacional { get; set; }
        public int Ordem { get; set; }
        public string VolMarca { get; set; }
        public string VolMarcaTotal { get; set; }
        public string PorcentMarca { get; set; }
        public string PorcentTotalMarca { get; set; }
        public string Vol { get; set; }
        public string VolTotal { get; set; }
        public string Total { get; set; }
        public string TotalGeral { get; set; }
        public string Porcent { get; set; }
        public string PorcentTotal { get; set; }
        public string DiasUtil { get; set; }
        public string NomeAreaOperacional { get; set; }

        public int AteDia { get; set; }
        public int AteMes { get; set; }
        public int AteAno { get; set; }
        public int Categoria { get; set; }
        public int TipoVenda { get; set; }
        public string Seguimento { get; set; }
        public int Concessionaria { get; set; }
        public bool ByGroup { get; set; }
        public bool Anual { get; set; }
        public bool Ranking2 { get; set; }

        
    }
}