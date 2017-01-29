using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Controller
{
    public class Configuracoes
    {
        public eConfiguracoes RetornarRegistro(string Sistema)
        {
            DAO.dConfiguracoes db = new DAO.dConfiguracoes();

            return db.RetornarRegistro(Sistema);
        }

        public eConfiguracoes RetornarRegistro(string dbConexao,string Sistema)
        {
            DAO.dConfiguracoes db = new DAO.dConfiguracoes();

            return db.RetornarRegistro(dbConexao,Sistema);
        }

        public bool GravarRegistro(eConfiguracoes configuracao)
        {
            DAO.dConfiguracoes db = new DAO.dConfiguracoes();

            return db.GravarRegistro(configuracao);
        }


    }
}