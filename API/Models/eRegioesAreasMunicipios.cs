using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class eRegioesAreasMunicipios
    {
        public int emp_id { get; set; }
        public string nm_empresa { get; set; }
        public string nm_fantasia { get; set; }
        public int id_grupo { get; set; }
        public string nm_grupo { get; set; }
        public int id_estado { get; set; }
        public string nm_sigla { get; set; }
        public int ID_regiao_operacional { get; set; }
        public string nm_regiao_operacional { get; set; }
        public int id_area_operacional { get; set; }
        public string nm_area_operacional{get; set; }
        public int id_cidade { get; set; }
        public string nm_cidade { get; set; }
        public string cd_ibge { get; set; }
        public string cd_serpro { get; set; }

        public byte bygroup { get; set; }
        public int concessionaria { get; set; }
    }
}