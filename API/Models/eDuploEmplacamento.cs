using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class eDuploEmplacamento
    {
        public DateTime DataEmplacamento { get; set; }
        public DateTime DataTransacao { get; set; }
        public string ModeloVeiculo { get; set; }
        public string SegmentoVeiculo { get; set; }
        public string MunicipioEmplacamento { get; set; }
        public string UfEmplacamento { get; set; }
        public string CnpjEmplacamento { get; set; }
        public string RazaoSocialEmplacamento { get; set; }
        public string RazaoSocial { get; set; }
        public string NomeFantasia { get; set; }
        public string NumeroChassi { get; set; }
        public string NumeroPlaca { get; set; }
        public int DiferencaDias { get; set; }
        public string TipoTransacao { get; set; }
        public string TipoVenda { get; set; }
        public string TipoPessoa { get; set; }
        public int IdUsuario { get; set; }
        public DateTime DataImportacao { get; set; }
        public DateTime FatoData { get; set; }
        public string FatoPlaca { get; set; }
        public string Cliente { get; set; }
        public DateTime DataNFe { get; set; }
        public string Bir { get; set; }
        public string TipoCliente { get; set; }
        public string RegionalDve { get; set; }
        public string DataRegistro { get; set; }
        public int Registro { get; set; }
        public int EmpId { get; set; }

        //Parâmetros

        public int? Dia { get; set; }
        public int? Mes { get; set; }
        public int? Ano { get; set; }
        public int? RegiaoOperacional { get; set; }
        public int? Estado { get; set; }
        public int? Concessionaria { get; set; }
        public int? DiaDe { get; set; }
        public int? MesDe { get; set; }
        public int? AnoDe { get; set; }
        public string ModalidadeVenda { get; set; }
        public string Sigla { get; set; }
        public byte? Bygroup { get; set; }

    }
}