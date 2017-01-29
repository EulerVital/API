using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class eAcessos
    {
        public int? Dia { get; set; }
        public int? Mes { get; set; }
        public int? Ano { get; set; }
        public int? DiaDe { get; set; }
        public int? MesDe { get; set; }
        public int? AnoDe { get; set; }
        public int? Grupo { get; set; }
        public int? Empresa { get; set; }
        public int? Usuario { get; set; }
        public string Relatorio { get; set; }
        public string Log { get; set; }

        //Dados que retorna do Banco
        public string Conta { get; set; }
        public string Nome { get; set; }
        public string EmpresaNome { get; set; }
        public string Volume { get; set; }
        public string GrupoNome { get; set; }
    }
}