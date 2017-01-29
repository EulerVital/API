using API.DAO.Util;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using API.Models;
using System.Data;
using static API.Models.eFTPArquivosImportacao;

namespace API.DAO
{
    public class dConfiguracoes
    {
        SqlHelp objSqlHelp;

        public eConfiguracoes RetornarRegistro(string Sistema)
        {
            return RetornarRegistro(null, Sistema);
        }

        public bool GravarRegistro(eConfiguracoes configuracoes)
        {
            bool retorno = false;

            try
            {
                objSqlHelp = new SqlHelp();
                objSqlHelp.AddParameterToSQLCommand("@ID", SqlDbType.Int, configuracoes.ID);
                objSqlHelp.AddParameterToSQLCommand("@EnderecoSite", SqlDbType.VarChar, configuracoes.EnderecoSite);
                objSqlHelp.AddParameterToSQLCommand("@Sistema", SqlDbType.VarChar, configuracoes.Sistema);
                objSqlHelp.AddParameterToSQLCommand("@Email", SqlDbType.VarChar, configuracoes.Email);
                objSqlHelp.AddParameterToSQLCommand("@EmailAlias", SqlDbType.VarChar, configuracoes.EmailAlias);
                objSqlHelp.AddParameterToSQLCommand("@ConfirmacaoLeitura", SqlDbType.Bit, configuracoes.ConfirmacaoLeitura);
                objSqlHelp.AddParameterToSQLCommand("@ExibirNovidades", SqlDbType.Bit, configuracoes.ExibirNovidades);

                objSqlHelp.AddParameterToSQLCommand("@SMTPHost", SqlDbType.VarChar, configuracoes.SMTPHost);
                objSqlHelp.AddParameterToSQLCommand("@SMTPPort", SqlDbType.VarChar, configuracoes.SMTPPort);
                objSqlHelp.AddParameterToSQLCommand("@SMTPSenha", SqlDbType.VarChar, configuracoes.SMTPSenha);
                objSqlHelp.AddParameterToSQLCommand("@SMTPUser", SqlDbType.VarChar, configuracoes.SMTPUser);

                objSqlHelp.AddParameterToSQLCommand("@DomainReport", SqlDbType.VarChar, configuracoes.DomainReport);
                objSqlHelp.AddParameterToSQLCommand("@UsuarioReport", SqlDbType.VarChar, configuracoes.UsuarioReport);
                objSqlHelp.AddParameterToSQLCommand("@PassReport", SqlDbType.VarChar, configuracoes.PassReport);
                objSqlHelp.AddParameterToSQLCommand("@ServidorReport", SqlDbType.VarChar, configuracoes.ServidorReport);

                objSqlHelp.AddParameterToSQLCommand("@LogoCliente", SqlDbType.VarChar, configuracoes.LogoCliente);
                objSqlHelp.AddParameterToSQLCommand("@RelatorioCorTitulo", SqlDbType.VarChar, configuracoes.RelatorioCorTitulo);
                objSqlHelp.AddParameterToSQLCommand("@RelatorioCorCabecalho", SqlDbType.VarChar, configuracoes.RelatorioCorCabecalho);
                objSqlHelp.AddParameterToSQLCommand("@RelatorioCorLinhaAlternada", SqlDbType.VarChar, configuracoes.RelatorioCorLinhaAlternada);

                objSqlHelp.AddParameterToSQLCommand("@EmailDesenvolvedor", SqlDbType.VarChar, configuracoes.EmailDesenvolvedor);

                objSqlHelp.AddParameterToSQLCommand("@LayoutCorMenu", SqlDbType.VarChar, configuracoes.LayoutCorMenu);
                objSqlHelp.AddParameterToSQLCommand("@LayoutCorBotoes", SqlDbType.VarChar, configuracoes.LayoutCorBotoes);

                objSqlHelp.AddParameterToSQLCommand("@RodapeTexto", SqlDbType.VarChar, configuracoes.RodapeTexto);
                objSqlHelp.AddParameterToSQLCommand("@TituloPaginas", SqlDbType.VarChar, configuracoes.TituloPaginas);
                objSqlHelp.AddParameterToSQLCommand("@SiteTema", SqlDbType.VarChar, configuracoes.SiteTema);

                objSqlHelp.AddParameterToSQLCommand("@FTPFenabrave", SqlDbType.VarChar, configuracoes.FTPFenabrave);
                objSqlHelp.AddParameterToSQLCommand("@FTPUsuario", SqlDbType.VarChar, configuracoes.FTPUsuario);
                objSqlHelp.AddParameterToSQLCommand("@FTPSenha", SqlDbType.VarChar, configuracoes.FTPSenha);

                if (configuracoes.FTPModoPassivo != null)
                {
                    objSqlHelp.AddParameterToSQLCommand("@FTPModoPassivo", SqlDbType.Bit, configuracoes.FTPModoPassivo);
                }

                objSqlHelp.AddParameterToSQLCommand("@Marca", SqlDbType.Bit, configuracoes.Marca);

                objSqlHelp.AddParameterToSQLCommand("@HabilitarPrimeiroAcesso", SqlDbType.Bit, configuracoes.HabilitarPrimeiroAcesso);
                objSqlHelp.AddParameterToSQLCommand("@BoletimLinkMobile", SqlDbType.Bit, configuracoes.BoletimLinkMobile);
                objSqlHelp.AddParameterToSQLCommand("@BoletimLinkParametro", SqlDbType.Bit, configuracoes.BoletimLinkParametro);
                objSqlHelp.AddParameterToSQLCommand("@MobileExibirTodasModalidades", SqlDbType.Bit, configuracoes.MobileExibirTodasModalidades);

                objSqlHelp.AddParameterToSQLCommand("@ExibirParticipacaoMarca", SqlDbType.Bit, configuracoes.ExibirParticipacaoMarca);

                objSqlHelp.AddParameterToSQLCommand("@BoletimExibirEvolucao", SqlDbType.Bit, configuracoes.BoletimExibirEvolucao);

                //Add por Caroline 13/01/2016. Escolha se loga ou não pelo emplacamento ou só pelo portal.
                objSqlHelp.AddParameterToSQLCommand("@LoginHabilitar", SqlDbType.Bit, configuracoes.LoginHabilitar);
                objSqlHelp.AddParameterToSQLCommand("@LoginInformacao", SqlDbType.VarChar, configuracoes.LoginInformacao);
                objSqlHelp.AddParameterToSQLCommand("@LoginLink", SqlDbType.VarChar, configuracoes.LoginLink);


                if (objSqlHelp.GetExecuteScalarByCommand("STP_GRAVAR_CONFIGURACOES_APLICACAO") > 0)
                {
                    retorno = true;
                }

                return retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objSqlHelp.Dispose();
                objSqlHelp.FecharConexao();
                objSqlHelp = null;
            }
        }

        public eConfiguracoes RetornarRegistro(string dbConexao, string Sistema)
        {
            eConfiguracoes registros = new eConfiguracoes();

            if (string.IsNullOrEmpty(dbConexao))
            {
                objSqlHelp = new SqlHelp();
            }
            else
            {
                objSqlHelp = new SqlHelp(dbConexao);
            }

            objSqlHelp.AddParameterToSQLCommand("@Sistema", SqlDbType.VarChar, Sistema);

            try
            {
                SqlDataReader dr = objSqlHelp.GetReaderByCmd("STP_SEL_CONFIGURACOES_APLICACAO");
                if (dr != null)
                {
                    using (dr)
                    {
                        if (dr.HasRows)
                        {
                            if (dr.Read())
                            {
                                registros.ID = Int32.Parse(dr["id"].ToString());
                                registros.LogoCliente = dr["LogoCliente"].ToString();
                                registros.Sistema = dr["Sistema"].ToString();
                                registros.Marca = dr["Marca"].ToString();
                                registros.EnderecoSite = dr["EnderecoSite"].ToString();
                                registros.Email = dr["Email"].ToString();
                                registros.EmailAlias = dr["EmailAlias"].ToString();
                                registros.ConfirmacaoLeitura = Convert.ToBoolean(dr["ConfirmacaoLeitura"]);
                                registros.ExibirNovidades = Convert.ToBoolean(dr["ExibirNovidades"]);

                                registros.HabilitarPrimeiroAcesso = Convert.ToBoolean(dr["HabilitarPrimeiroAcesso"]);
                                registros.BoletimLinkMobile = Convert.ToBoolean(dr["BoletimLinkMobile"]);
                                registros.BoletimLinkParametro = Convert.ToBoolean(dr["BoletimLinkParametro"]);

                                registros.SMTPHost = dr["SMTPHost"].ToString();
                                registros.SMTPPort = dr["SMTPPort"].ToString();
                                registros.SMTPUser = dr["SMTPUser"].ToString();
                                registros.SMTPSenha = dr["SMTPSenha"].ToString();

                                registros.UsuarioReport = dr["UsuarioReport"].ToString();
                                registros.PassReport = dr["PassReport"].ToString();
                                registros.DomainReport = dr["DomainReport"].ToString();
                                registros.ServidorReport = dr["ServidorReport"].ToString();

                                registros.EmailDesenvolvedor = dr["EmailDesenvolvedor"].ToString();

                                registros.RelatorioCorTitulo = dr["RelatorioCorTitulo"].ToString();
                                registros.RelatorioCorCabecalho = dr["RelatorioCorCabecalho"].ToString();
                                registros.RelatorioCorLinhaAlternada = dr["RelatorioCorLinhaAlternada"].ToString();

                                registros.LayoutCorMenu = dr["LayoutCorMenu"].ToString();
                                registros.LayoutCorBotoes = dr["LayoutCorBotoes"].ToString();

                                registros.RodapeTexto = dr["RodapeTexto"].ToString();
                                registros.TituloPaginas = dr["TituloPaginas"].ToString();
                                registros.SiteTema = dr["SiteTema"].ToString();

                                registros.FTPFenabrave = dr["FTPFenabrave"].ToString();
                                registros.FTPUsuario = dr["FTPUsuario"].ToString();
                                registros.FTPSenha = dr["FTPSenha"].ToString();
                                registros.FTPModoPassivo = Convert.ToBoolean(dr["FTPModoPassivo"].ToString());
                                registros.LFTPArquivosImportacao = ListarFtpArquivosImportacao(registros.ID, dbConexao);

                                registros.ExibirParticipacaoMarca = Convert.ToBoolean(dr["ExibirParticipacaoMarca"].ToString());
                                registros.BoletimExibirEvolucao = Convert.ToBoolean(dr["BoletimExibirEvolucao"].ToString());
                                registros.BoletimTipoRanking = Convert.ToBoolean(dr["BoletimTipoRanking"].ToString());

                                //Add por Caroline 13/01/2016. Escolha se loga ou não pelo emplacamento ou só pelo portal.
                                registros.LoginHabilitar = Convert.ToBoolean(dr["LoginHabilitar"].ToString());
                                registros.LoginInformacao = dr["LoginInformacao"].ToString();
                                registros.LoginLink = dr["LoginLink"].ToString();

                                try
                                {
                                    registros.MobileExibirTodasModalidades = Convert.ToBoolean(dr["MobileExibirTodasModalidades"].ToString());

                                    registros.ImportacaoDiretorio = dr["ImportacaoDiretorio"].ToString();
                                    registros.ImportacaoTempoExecucao = dr["ImportacaoTempoExecucao"].ToString();
                                    registros.ImportacaoTempoErroBaixar = dr["ImportacaoTempoErroBaixar"].ToString();
                                    registros.ImportacaoTempoDepoisBaixar = dr["ImportacaoTempoDepoisBaixar"].ToString();

                                    registros.LimiteMarcas = Convert.ToInt32(dr["LimiteMarcas"]);
                                }
                                catch (Exception) { }
                            }
                        }
                    }
                }
                dr.Close();
                dr = null;

                return registros;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objSqlHelp.Dispose();
                objSqlHelp.FecharConexao();
                objSqlHelp = null;
            }
        }

        private LFTPArquivosImportacao ListarFtpArquivosImportacao(int ConfiguracaoID, string dbConexao)
        {
            LFTPArquivosImportacao Lista = new LFTPArquivosImportacao();
            eFTPArquivosImportacao Registro;

            SqlHelp objSql;

            if (string.IsNullOrEmpty(dbConexao))
            {
                objSql = new SqlHelp();
            }
            else
            {
                objSql = new SqlHelp(dbConexao);
            }

            try
            {
                SqlDataReader dr = objSql.GetReaderByCmd("STP_SEL_FTP_ARQUIVOS_IMPORTACAO");
                if (dr != null)
                {
                    using (dr)
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                Registro = new eFTPArquivosImportacao();
                                Registro.id = Int32.Parse(dr["id"].ToString());
                                Registro.configuracaoID = ConfiguracaoID;
                                Registro.arquivo = dr["arquivo"].ToString();
                                Registro.arquivoMensal = Convert.ToBoolean(dr["arquivoMensal"].ToString());
                                Registro.ordem = Int32.Parse(dr["ordem"].ToString());
                                Registro.tipo = Int32.Parse(dr["tipo"].ToString());
                                Lista.Add(Registro);
                            }
                        }
                    }
                }
                dr.Close();
                dr = null;

                return Lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objSql.Dispose();
                objSql.FecharConexao();
                objSql = null;
            }
        }
    }
}