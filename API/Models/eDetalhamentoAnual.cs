using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class eDetalhamentoAnual
    {
        public string AreaConc { get; set; }
        public string IdLocalizacao { get; set; }
        public string IdAreaOperacional { get; set; }
        public string NomeAreaOperacional { get; set; }
        public string IdCidade { get; set; }
        public string NomeCidade { get; set; }
        public string NomeEmpresa { get; set; }
        public string Grupo { get; set; }
        public string IdEmpresa { get; set; }
        public string Vol_Ano_Anterior { get; set; }
        public string Vol_Ano_Atual { get; set; }
        public string Vol_Janeiro { get; set; }
        public string Vol_Fevereiro { get; set; }
        public string Vol_Marco { get; set; }
        public string Vol_Abril { get; set; }
        public string Vol_Maio { get; set; }
        public string Vol_Junho { get; set; }
        public string Vol_Julho { get; set; }
        public string Vol_Agosto { get; set; }
        public string Vol_Setembro { get; set; }
        public string Vol_Outubro { get; set; }
        public string Vol_Novembro { get; set; }
        public string Vol_Dezembro { get; set; }
        public string Total_Ano_Anterior { get; set; }
        public string Total_Ano_Atual { get; set; }
        public string Total_Janeiro { get; set; }
        public string Total_Fevereiro { get; set; }
        public string Total_Marco { get; set; }
        public string Total_Abril { get; set; }
        public string Total_Maio { get; set; }
        public string Total_Junho { get; set; }
        public string Total_Julho { get; set; }
        public string Total_Agosto { get; set; }
        public string Total_Setembro { get; set; }
        public string Total_Outubro { get; set; }
        public string Total_Novembro { get; set; }
        public string Total_Dezembro { get; set; }
        public string Porcent_Ano_Anterior { get; set; }
        public string Porcent_Ano_Atual { get; set; }
        public string Porcent_Janeiro { get; set; }
        public string Porcent_Fevereiro { get; set; }
        public string Porcent_Marco { get; set; }
        public string Porcent_Abril { get; set; }
        public string Porcent_Maio { get; set; }
        public string Porcent_Junho { get; set; }
        public string Porcent_Julho { get; set; }
        public string Porcent_Agosto { get; set; }
        public string Porcent_Setembro { get; set; }
        public string Porcent_Outubro { get; set; }
        public string Porcent_Novembro { get; set; }
        public string Porcent_Dezembro { get; set; }

        //Parametros

        public int Ano { get; set; }
        public int Categoria { get; set; }
        public int TipoVenda { get; set; }
        public int Segmento { get; set; }
        public int RegiaoOperacional { get; set; }
        public int RegiaoGeografico { get; set; }
        public int Estado { get; set; }
        public int RegiaoMetropolitana { get; set; }
        public int Cidade { get; set; }
        public int AreaOperacional { get; set; }
        public int Concessionaria { get; set; }
        public int ByGroup { get; set; }
        

    }
}