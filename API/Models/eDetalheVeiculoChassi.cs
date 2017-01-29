using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class eDetalheVeiculoChassi
    {
        public string Data { get; set; }
        public string NomeVeiculo { get; set; }
        public string Modelo { get; set; }
        public string Chassis { get; set; }
        public string Placa { get; set; }
        public int IdEmpresa { get; set; }

        //Parametros
        public int Mes { get; set; }
        public int Ano { get; set; }
        public bool Grupo { get; set; }
        public int IdLocalizacao { get; set; }
        public int Area { get; set; } 


    }
}