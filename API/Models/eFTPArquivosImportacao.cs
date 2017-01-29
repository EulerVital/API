using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class eFTPArquivosImportacao
    {

        public int id { get; set; }
        public int? configuracaoID { get; set; }
        public int? ordem { get; set; }
        public string arquivo { get; set; }
        public bool arquivoMensal { get; set; }
        public int tipo { get; set; } // 0 comun, 1 Giro

        public class LFTPArquivosImportacao : List<eFTPArquivosImportacao> { }

    }
}