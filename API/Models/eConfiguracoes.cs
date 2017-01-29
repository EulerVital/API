using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static API.Models.eFTPArquivosImportacao;

namespace API.Models
{
    public class eConfiguracoes
    {
        public int ID { get; set; }
        public string EnderecoSite { get; set; }
        public string Sistema { get; set; }
        public string Marca { get; set; }
        public string LogoCliente { get; set; }
        public string Email { get; set; }
        public string EmailAlias { get; set; }

        public string EmailDesenvolvedor { get; set; }

        public string SMTPHost { get; set; }
        public string SMTPPort { get; set; }
        public string SMTPUser { get; set; }
        public string SMTPSenha { get; set; }

        public string UsuarioReport { get; set; }
        public string PassReport { get; set; }
        public string DomainReport { get; set; }
        public string ServidorReport { get; set; }

        public string RelatorioCorTitulo { get; set; }
        public string RelatorioCorCabecalho { get; set; }
        public string RelatorioCorLinhaAlternada { get; set; }

        public string LayoutCorMenu { get; set; }
        public string LayoutCorBotoes { get; set; }

        public string RodapeTexto { get; set; }
        public string TituloPaginas { get; set; }

        public string SiteTema { get; set; }

        public string FTPFenabrave { get; set; }
        public string FTPUsuario { get; set; }
        public string FTPSenha { get; set; }

        public bool? ConfirmacaoLeitura { get; set; }
        public bool? ExibirNovidades { get; set; }
        public bool? FTPModoPassivo { get; set; }
        public bool? BoletimLinkMobile { get; set; }
        public bool? BoletimLinkParametro { get; set; }
        public bool? HabilitarPrimeiroAcesso { get; set; }

        public bool? MobileExibirTodasModalidades { get; set; }

        public LFTPArquivosImportacao LFTPArquivosImportacao { get; set; }

        public string ImportacaoDiretorio { get; set; }
        public string ImportacaoConexao { get; set; }
        public string ImportacaoTempoExecucao { get; set; }
        public string ImportacaoTempoErroBaixar { get; set; }
        public string ImportacaoTempoDepoisBaixar { get; set; }

        public string ImportacaoBCP { get; set; }

        public int? LimiteMarcas { get; set; }

        public bool ExibirParticipacaoMarca { get; set; }

        public bool BoletimExibirEvolucao { get; set; }

        public bool BoletimTipoRanking { get; set; }

        //Add por Caroline 13/01/2016. Escolha se loga ou não pelo emplacamento ou só pelo portal.
        public bool LoginHabilitar { get; set; }
        public string LoginInformacao { get; set; }
        public string LoginLink { get; set; }
    }

}