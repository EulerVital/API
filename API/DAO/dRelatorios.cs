using API.DAO.Util;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using API.Models;
using System.Data;

namespace API.DAO
{
    public class dRelatorios
    {
        SqlHelp objSqlHelp;
        SqlDataReader dr;

        //List<eEmplacamentoCidade> listaEmplacamentoCidade;
        //List<eDetalhamento> listaDetalhamento;

        //public List<eEmplacamentoCidade> SelecionarEmplacamentoForaCidade(eEmplacamentoCidade emplacamentoFora)
        //{
        //    listaEmplacamentoCidade = new List<eEmplacamentoCidade>();
        //    objSqlHelp = new SqlHelp();

        //    objSqlHelp.AddParameterToSQLCommand("@ATE_DIA", SqlDbType.Int, emplacamentoFora.AteDia);
        //    objSqlHelp.AddParameterToSQLCommand("@ATE_MES", SqlDbType.Int, emplacamentoFora.AteMes);
        //    objSqlHelp.AddParameterToSQLCommand("@ATE_ANO", SqlDbType.Int, emplacamentoFora.AteAno);
        //    objSqlHelp.AddParameterToSQLCommand("@CATEGORIA", SqlDbType.Int, emplacamentoFora.Categoria);
        //    objSqlHelp.AddParameterToSQLCommand("@TIPO_VENDA", SqlDbType.Int, emplacamentoFora.TipoVenda);
        //    objSqlHelp.AddParameterToSQLCommand("@SEGMENTO", SqlDbType.NVarChar, emplacamentoFora.Seguimento);
        //    objSqlHelp.AddParameterToSQLCommand("@CONCESSIONARIA", SqlDbType.Int, emplacamentoFora.Concessionaria);
        //    objSqlHelp.AddParameterToSQLCommand("@BYGROUP", SqlDbType.Bit, emplacamentoFora.ByGroup);
        //    objSqlHelp.AddParameterToSQLCommand("@ANUAL", SqlDbType.Bit, emplacamentoFora.Anual);
        //    objSqlHelp.AddParameterToSQLCommand("@RANKING", SqlDbType.Bit, emplacamentoFora.Ranking2);

        //    dr = objSqlHelp.GetReaderByCmd("STP_EMPLACAMENTO_FORA_CIDADE");
        //    try
        //    {
        //        if (dr.HasRows)
        //        {
        //            while (dr.Read())
        //            {
        //                eEmplacamentoCidade ec = new eEmplacamentoCidade();

        //                ec.Ranking = Convert.ToInt32(dr["ranking"]);
        //                ec.CorLinha = Convert.ToInt32(dr["CorLinha"]);
        //                ec.EmpresaEmpId = Convert.ToInt32(dr["empresa_emp_id"]);
        //                ec.NomeCidade = dr["nm_cidade"].ToString();
        //                ec.DataValidaAno = Convert.ToDateTime(dr["datavalida_ano"]);
        //                ec.NomeDataValidaMes = dr["datavalida_nm_do_mes"].ToString();
        //                ec.ConfereEmpresa = Convert.ToInt32(dr["confere_empresa"]);
        //                ec.NomeclaturaModelos = dr["NomenclaturaModelosValores_nomenclaturaMarca"].ToString();
        //                ec.IdLocalizacaoOperacional = Convert.ToInt32(dr["localizacao_id_area_operacional"]);
        //                ec.NomeLocalizacaoOperacional = dr["localizacao_nm_area_operacional"].ToString();
        //                ec.OrdemMarca = Convert.ToInt32(dr["ordemMarca"]);
        //                ec.Qtd = Convert.ToInt32(dr["qtd"]);

        //                listaEmplacamentoCidade.Add(ec);
        //            }
        //        }

        //        return listaEmplacamentoCidade;
        //    }
        //    catch (SqlException e)
        //    {
        //        throw e;
        //    }
        //    finally
        //    {
        //        objSqlHelp.Dispose();
        //        objSqlHelp.FecharConexao();
        //        objSqlHelp = null;
        //    }
        //}

        //public int SelecionarDiasUteis(eEmplacamentoCidade emplacamentoDia)
        //{
        //    int DiaUtil = 0;
        //    objSqlHelp = new SqlHelp();

        //    objSqlHelp.AddParameterToSQLCommand("@ATE_DIA", SqlDbType.Int, emplacamentoDia.AteDia);
        //    objSqlHelp.AddParameterToSQLCommand("@ATE_MES", SqlDbType.Int, emplacamentoDia.AteMes);
        //    objSqlHelp.AddParameterToSQLCommand("@ATE_ANO", SqlDbType.Int, emplacamentoDia.AteAno);
        //    objSqlHelp.AddParameterToSQLCommand("@CATEGORIA", SqlDbType.Int, emplacamentoDia.Categoria);
        //    objSqlHelp.AddParameterToSQLCommand("@TIPO_VENDA", SqlDbType.Int, emplacamentoDia.TipoVenda);
        //    objSqlHelp.AddParameterToSQLCommand("@SEGMENTO", SqlDbType.NVarChar, emplacamentoDia.Seguimento);
        //    objSqlHelp.AddParameterToSQLCommand("@CONCESSIONARIA", SqlDbType.Int, emplacamentoDia.Concessionaria);
        //    objSqlHelp.AddParameterToSQLCommand("@BYGROUP", SqlDbType.Bit, emplacamentoDia.ByGroup);
        //    objSqlHelp.AddParameterToSQLCommand("@ANUAL", SqlDbType.Bit, emplacamentoDia.Anual);
        //    objSqlHelp.AddParameterToSQLCommand("@RANKING", SqlDbType.Bit, emplacamentoDia.Ranking2);

        //    dr = objSqlHelp.GetReaderByCmd("STP_DIAS_UTEIS");
        //    try
        //    {
        //        if (dr.HasRows)
        //        {
        //            while (dr.Read())
        //            {
        //                DiaUtil = Convert.ToInt32(dr["DIAS_UTEIS"]);
        //            }
        //        }

        //        return DiaUtil;
        //    }
        //    catch (SqlException e)
        //    {
        //        throw e;
        //    }
        //    finally
        //    {
        //        objSqlHelp.Dispose();
        //        objSqlHelp.FecharConexao();
        //        objSqlHelp = null;
        //    }
        //}


        #region VeiculosInfo

        public List<eVeiculosInfo> GetUserRegistroVeiculos(string chassi)
        {
            List<eVeiculosInfo> listaVeiculos = new List<eVeiculosInfo>();

            objSqlHelp = new SqlHelp();

            try
            {
                objSqlHelp.AddParameterToSQLCommand("@CHASSI", SqlDbType.VarChar, chassi);


                dr = objSqlHelp.GetReaderByCmd("STP_VEICULO_INFO");
                if (dr != null)
                {
                    using (dr)
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                eVeiculosInfo veiculos = new eVeiculosInfo();

                                veiculos.cd_cnpj = dr["CD_CNPJ"].ToString();
                                veiculos.nm_razao = dr["NM_RAZAO_SOCIAL"].ToString();
                                veiculos.dt_emplacamento = dr["dt_emplacamento"].ToString();
                                veiculos.cd_chassi = dr["CD_CHASSI"].ToString();
                                veiculos.cd_placa = dr["CD_PLACA"].ToString();
                                veiculos.nm_fabricante = dr["NM_FABRICANTE"].ToString();
                                veiculos.nm_grupo_modelo_veiculo = dr["NM_GRUPO_MODELO_VEICULO"].ToString();
                                veiculos.nm_modelo = dr["NM_MODELO"].ToString();
                                veiculos.nm_segmento = dr["NM_SEGMENTO"].ToString();
                                veiculos.nm_sub_segmento = dr["NM_SUB_SEGMENTO"].ToString();
                                veiculos.nm_municipio = dr["NM_MUNICIPIO"].ToString();
                                veiculos.nm_estado = dr["NM_ESTADO"].ToString();
                                veiculos.nm_ano_fabricacao = dr["NM_ANO_FABRICACAO"].ToString();
                                veiculos.nm_cilindrada = dr["NM_CILINDRADA"].ToString();

                                listaVeiculos.Add(veiculos);
                            }
                        }
                    }
                }

                return listaVeiculos;

            }
            catch (SqlException e)
            {
                throw e;
            }
            finally
            {
                objSqlHelp.Dispose();
                objSqlHelp.FecharConexao();
                objSqlHelp = null;
            }


        }

        #endregion

        #region Acessos

        public List<eAcessos> GetUserRegistroAcesso(eAcessos Useracesso)
        {
            List<eAcessos> lista = new List<eAcessos>();

            objSqlHelp = new SqlHelp();

            try
            {
                objSqlHelp.AddParameterToSQLCommand("@dia", SqlDbType.Int, Useracesso.Dia);
                objSqlHelp.AddParameterToSQLCommand("@mes", SqlDbType.Int, Useracesso.Mes);
                objSqlHelp.AddParameterToSQLCommand("@ano", SqlDbType.Int, Useracesso.Ano);
                objSqlHelp.AddParameterToSQLCommand("@dia_de", SqlDbType.Int, Useracesso.DiaDe);
                objSqlHelp.AddParameterToSQLCommand("@mes_de", SqlDbType.Int, Useracesso.MesDe);
                objSqlHelp.AddParameterToSQLCommand("@ano_de", SqlDbType.Int, Useracesso.AnoDe);
                objSqlHelp.AddParameterToSQLCommand("@grupo", SqlDbType.Int, Useracesso.Grupo);
                objSqlHelp.AddParameterToSQLCommand("@empresa", SqlDbType.Int, Useracesso.Empresa);
                objSqlHelp.AddParameterToSQLCommand("@usuario", SqlDbType.Int, Useracesso.Usuario);
                objSqlHelp.AddParameterToSQLCommand("@relatorio", SqlDbType.VarChar, Useracesso.Relatorio);
                objSqlHelp.AddParameterToSQLCommand("@log", SqlDbType.VarChar, Useracesso.Log);

                dr = objSqlHelp.GetReaderByCmd("STP_ACESSOS");
                if (dr != null)
                {
                    using (dr)
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                eAcessos acesso = new eAcessos();

                                acesso.Conta = dr["conta"].ToString();
                                acesso.EmpresaNome = dr["empresa"].ToString();
                                acesso.Nome = dr["pessoa_nome"].ToString();
                                acesso.Volume = dr["qtd"].ToString();
                                acesso.GrupoNome = dr["grupo"].ToString();

                                lista.Add(acesso);
                            }
                        }
                    }
                }
                return lista;
            }
            catch (SqlException ex)
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

        #endregion

        #region RegioesAreasMunicipios

        public List<eRegioesAreasMunicipios> GetUserRegistroRegioesAreasMunicipios(int concessionaria, byte bygroup)
        {
            List<eRegioesAreasMunicipios> lista = new List<eRegioesAreasMunicipios>();

            objSqlHelp = new SqlHelp();

            try
            {
                objSqlHelp.AddParameterToSQLCommand("@concessionaria", SqlDbType.Int, concessionaria);
                objSqlHelp.AddParameterToSQLCommand("@bygroup", SqlDbType.Bit, bygroup);

                dr = objSqlHelp.GetReaderByCmd("USP_REGIOES_AREAS_MUNICIPIOS");
                if (dr != null)
                {
                    using (dr)
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                eRegioesAreasMunicipios regiao = new eRegioesAreasMunicipios();

                                regiao.emp_id = Convert.ToInt32(dr["emp_id"]);
                                regiao.nm_empresa = dr["nm_empresa"].ToString();
                                regiao.nm_fantasia = dr["nm_fantasia"].ToString();
                                regiao.id_grupo = Convert.ToInt32(dr["id_grupo"]);
                                regiao.nm_grupo = dr["nm_grupo"].ToString();
                                regiao.id_estado = Convert.ToInt32(dr["id_estado"]);
                                regiao.nm_sigla = dr["nm_sigla"].ToString();
                                regiao.ID_regiao_operacional = Convert.ToInt32(dr["ID_regiao_operacional"]);
                                regiao.nm_regiao_operacional = dr["nm_regiao_operacional"].ToString();
                                regiao.id_area_operacional = Convert.ToInt32(dr["id_area_operacional"]);
                                regiao.nm_area_operacional = dr["nm_area_operacional"].ToString();
                                regiao.id_cidade = Convert.ToInt32(dr["id_cidade"]);
                                regiao.nm_cidade = dr["nm_cidade"].ToString();
                                regiao.cd_ibge = dr["cd_ibge"].ToString();
                                regiao.cd_serpro = dr["cd_serpro"].ToString();

                                lista.Add(regiao);
                            }
                        }
                    }
                }
                return lista;

            }
            catch (SqlException)
            {

                throw;
            }
            finally
            {
                objSqlHelp.Dispose();
                objSqlHelp.FecharConexao();
                objSqlHelp = null;
            }
        }

        #endregion

        #region DuploEmplacamento

        public List<eDuploEmplacamento> GetUserRegistroDuploEmplacamento(eDuploEmplacamento duplo)
        {
            List<eDuploEmplacamento> lista = new List<eDuploEmplacamento>();

            objSqlHelp = new SqlHelp();

            try
            {
                objSqlHelp.AddParameterToSQLCommand("@dia", SqlDbType.Int, duplo.Dia);
                objSqlHelp.AddParameterToSQLCommand("@mes", SqlDbType.Int, duplo.Mes);
                objSqlHelp.AddParameterToSQLCommand("@ano", SqlDbType.Int, duplo.Ano);
                objSqlHelp.AddParameterToSQLCommand("@regiao_operacional", SqlDbType.Int, duplo.RegiaoOperacional);
                objSqlHelp.AddParameterToSQLCommand("@estado", SqlDbType.Int, duplo.Estado);
                objSqlHelp.AddParameterToSQLCommand("@Concessionaria", SqlDbType.Int, duplo.Concessionaria);
                objSqlHelp.AddParameterToSQLCommand("@dia_de", SqlDbType.Int, duplo.DiaDe);
                objSqlHelp.AddParameterToSQLCommand("@mes_de", SqlDbType.Int, duplo.MesDe);
                objSqlHelp.AddParameterToSQLCommand("@ano_de", SqlDbType.Int, duplo.AnoDe);
                objSqlHelp.AddParameterToSQLCommand("@modalidadeVenda", SqlDbType.VarChar, duplo.ModalidadeVenda);
                objSqlHelp.AddParameterToSQLCommand("@sigla", SqlDbType.VarChar, duplo.Sigla);
                objSqlHelp.AddParameterToSQLCommand("@bygroup", SqlDbType.Bit, duplo.Bygroup);

                dr = objSqlHelp.GetReaderByCmd("USP_DUPLO_EMPLACAMENTO");
                if (dr != null)
                {
                    using (dr)
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                eDuploEmplacamento dubloEmplacamento = new eDuploEmplacamento();

                                dubloEmplacamento.DataEmplacamento = Convert.ToDateTime(dr["DATA_EMPLACAMENTO"]);
                                dubloEmplacamento.DataTransacao = Convert.ToDateTime(dr["DATA_TRANSACAO"]);
                                dubloEmplacamento.ModeloVeiculo = dr["MODELO_VEICULO"].ToString();
                                dubloEmplacamento.SegmentoVeiculo = dr["SEGMENTO_VEICULO"].ToString();
                                dubloEmplacamento.MunicipioEmplacamento = dr["MUNICIPIO_EMPLACAMENTO"].ToString();
                                dubloEmplacamento.UfEmplacamento = dr["UF_EMPLACAMENTO"].ToString();
                                dubloEmplacamento.CnpjEmplacamento = dr["CNPJ_EMPLACAMENTO"].ToString();
                                dubloEmplacamento.RazaoSocialEmplacamento = dr["RAZAO_SOCIAL_EMPLACAMENTO"].ToString();
                                dubloEmplacamento.RazaoSocial = dr["RAZAO_SOCIAL"].ToString();
                                dubloEmplacamento.NomeFantasia = dr["NOME_FANTASIA"].ToString();
                                dubloEmplacamento.NumeroChassi = dr["NUMERO_CHASSI"].ToString();
                                dubloEmplacamento.NumeroPlaca = dr["NUMERO_PLACA"].ToString();
                                dubloEmplacamento.DiferencaDias = Convert.ToInt32(dr["DIFERENCA_DIAS"]);
                                dubloEmplacamento.TipoTransacao = dr["TIPO_TRANSACAO"].ToString();
                                dubloEmplacamento.TipoPessoa = dr["TIPO_PESSOA"].ToString();
                                dubloEmplacamento.IdUsuario = Convert.ToInt32(dr["id_usuario"]);
                                dubloEmplacamento.DataImportacao = Convert.ToDateTime(dr["DATA_IMPORTACAO"]);
                                dubloEmplacamento.FatoData = Convert.ToDateTime(dr["FATO_DATA"]);
                                dubloEmplacamento.FatoPlaca = dr["FATO_PLACA"].ToString();
                                dubloEmplacamento.Cliente = dr["Cliente"].ToString();
                                dubloEmplacamento.DataNFe = Convert.ToDateTime(dr["DataNFe"]);
                                dubloEmplacamento.Bir = dr["BIR"].ToString();
                                dubloEmplacamento.TipoCliente = dr["TipoCliente"].ToString();
                                dubloEmplacamento.RegionalDve = dr["RegionalDVE"].ToString();
                                dubloEmplacamento.DataRegistro = dr["DataRegistro"].ToString();
                                dubloEmplacamento.Registro = Convert.ToInt32(dr["REGISTROS"]);
                                dubloEmplacamento.EmpId = Convert.ToInt32(dr["emp_id"]);

                                lista.Add(dubloEmplacamento);
                            }
                        }
                    }
                }

                return lista;
            }
            catch (SqlException)
            {
                throw;
            }
            finally
            {
                objSqlHelp.Dispose();
                objSqlHelp.FecharConexao();
                objSqlHelp = null;
            }

        }

        #endregion

        #region FaturamentoDve

        public List<eFaturamentoDve> GetUserRegistroFaturamentoDve(eFaturamentoDve faturamento)
        {
            List<eFaturamentoDve> lista = new List<eFaturamentoDve>();

            objSqlHelp = new SqlHelp();

            try
            {
                objSqlHelp.AddParameterToSQLCommand("@dia", SqlDbType.Int, faturamento.Dia);
                objSqlHelp.AddParameterToSQLCommand("@mes", SqlDbType.Int, faturamento.Mes);
                objSqlHelp.AddParameterToSQLCommand("@ano", SqlDbType.Int, faturamento.Ano);
                objSqlHelp.AddParameterToSQLCommand("@modalidadeVenda", SqlDbType.VarChar, faturamento.ModalidadeVenda);
                objSqlHelp.AddParameterToSQLCommand("@sigla", SqlDbType.VarChar, faturamento.Sigla);

                dr = objSqlHelp.GetReaderByCmd("USP_RELATORIO_FATURAMENTODVE_1");

                if (dr != null)
                {
                    using (dr)
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                eFaturamentoDve fat = new eFaturamentoDve();

                                fat.TipoCliente = dr["TipoCliente"].ToString();
                                fat.Sigla = dr["Sigla"].ToString();
                                fat.TipoDeCliente = dr["TipoDeCliente"].ToString();
                                fat.AreaDivisao = dr["AreaDivisao"].ToString();
                                fat.Mes = Convert.ToInt32(dr["Mes"]);
                                fat.UltimoMes = Convert.ToInt32(dr["UltimoMes"]);
                                fat.UltimoAno = Convert.ToInt32(dr["UltimoAno"]);
                                fat.UltimoDia = Convert.ToInt32(dr["UltimoDia"]);
                                fat.TotalMes = Convert.ToInt32(dr["TotalMes"]);
                                fat.TotalGeral = Convert.ToInt32(dr["TotalGeral"]);
                                fat.TotalSigla = Convert.ToInt32(dr["TotalSigla"]);
                                fat.Participacao = Convert.ToDouble(dr["Participacao"]);

                                lista.Add(fat);
                            }
                        }
                    }
                }

                return lista;
            }
            catch (SqlException)
            {
                throw;
            }
            finally
            {
                objSqlHelp.Dispose();
                objSqlHelp.FecharConexao();
                objSqlHelp = null;
            }


        }

        #endregion

        #region FAturamentoDvr

        public List<eFaturamentoDvr> GetUserRegistroFaturamentoDvr(int ano, int mes, int dia)
        {
            List<eFaturamentoDvr> lista = new List<eFaturamentoDvr>();

            objSqlHelp = new SqlHelp();

            try
            {
                objSqlHelp.AddParameterToSQLCommand("@dia", SqlDbType.Int, dia);
                objSqlHelp.AddParameterToSQLCommand("@mes", SqlDbType.Int, mes);
                objSqlHelp.AddParameterToSQLCommand("@ano", SqlDbType.Int, ano);

                dr = objSqlHelp.GetReaderByCmd("USP_RELATORIO_FATURAMENTODVR_1");

                if (dr != null)
                {
                    using (dr)
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                eFaturamentoDvr fat = new eFaturamentoDvr();

                                fat.Bir = dr["BIR"].ToString();
                                fat.Concessionaria = dr["Concessionaria"].ToString();
                                fat.Mes = dr["Mes"].ToString();
                                fat.Qtd = Convert.ToInt32(dr["qtd"]);
                                fat.UltimoMes = dr["UltimoMes"].ToString();
                                fat.UltimoAno = dr["UltimoAno"].ToString();
                                fat.UltimoDia = dr["UltimoDia"].ToString();
                                fat.Total = Convert.ToInt32(dr["Total"]);
                                fat.TotalGeral = Convert.ToInt32(dr["TotalGeral"]);
                                fat.TotalMes = dr["TotalMes"].ToString();
                                fat.Participacao = dr["Participacao"].ToString();

                                lista.Add(fat);
                            }
                        }
                    }
                }

                return lista;
            }
            catch (SqlException)
            {
                throw;
            }
            finally
            {
                objSqlHelp.Dispose();
                objSqlHelp.FecharConexao();
                objSqlHelp = null;
            }
        }

        #endregion

        #region FaturamentoDveCliente

        public List<eFaturamentoDVEClienteConcessionaria> GetUserRegistroFaturamentoDveCliente(int ano, int mes, int dia)
        {
            List<eFaturamentoDVEClienteConcessionaria> lista = new List<eFaturamentoDVEClienteConcessionaria>();


            objSqlHelp = new SqlHelp();

            try
            {
                objSqlHelp.AddParameterToSQLCommand("@dia", SqlDbType.Int, dia);
                objSqlHelp.AddParameterToSQLCommand("@mes", SqlDbType.Int, mes);
                objSqlHelp.AddParameterToSQLCommand("@ano", SqlDbType.Int, ano);

                dr = objSqlHelp.GetReaderByCmd("USP_RELATORIO_FATURAMENTODVECLIENTE_1");

                if (dr != null)
                {
                    using (dr)
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                eFaturamentoDVEClienteConcessionaria fatCli = new eFaturamentoDVEClienteConcessionaria();

                                fatCli.Bir = dr["bir"].ToString();
                                fatCli.RazaoSocial = dr["RazaoSocial"].ToString();
                                fatCli.Cliente = dr["Cliente"].ToString();
                                fatCli.TotalCliente = dr["TotalCliente"].ToString();
                                fatCli.TotalFaturamento = dr["TotalFaturamento"].ToString();
                                fatCli.Participacao = dr["Participacao"].ToString();
                                fatCli.Jan = dr["JAN"].ToString();
                                fatCli.Fev = dr["FEV"].ToString();
                                fatCli.Mar = dr["MAR"].ToString();
                                fatCli.Abr = dr["ABR"].ToString();
                                fatCli.Mai = dr["MAI"].ToString();
                                fatCli.Jun = dr["JUN"].ToString();
                                fatCli.Jul = dr["JUL"].ToString();
                                fatCli.Ago = dr["AGO"].ToString();
                                fatCli.Set = dr["SET"].ToString();
                                fatCli.Out = dr["OUT"].ToString();
                                fatCli.Nov = dr["NOV"].ToString();
                                fatCli.Dez = dr["DEZ"].ToString();
                                fatCli.UltimoMes = dr["UltimoMes"].ToString();
                                fatCli.UltimoAno = dr["UltimoAno"].ToString();
                                fatCli.UltimoDia = dr["UltimoDia"].ToString();
                                fatCli.TJan = dr["TotalJAN"].ToString();
                                fatCli.TFev = dr["TotalFEV"].ToString();
                                fatCli.TMar = dr["TotalMAR"].ToString();
                                fatCli.TAbr = dr["TotalABR"].ToString();
                                fatCli.TMai = dr["TotalMAI"].ToString();
                                fatCli.TJun = dr["TotalJUN"].ToString();
                                fatCli.TJul = dr["TotalJUL"].ToString();
                                fatCli.TAgo = dr["TotalAGO"].ToString();
                                fatCli.TSet = dr["TotalSET"].ToString();
                                fatCli.TOut = dr["TotalOUT"].ToString();
                                fatCli.TNov = dr["TotalNOV"].ToString();
                                fatCli.TDez = dr["TotalDEZ"].ToString();

                                lista.Add(fatCli);
                            }
                        }
                    }
                }

                return lista;
            }
            catch (SqlException ex)
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

        #endregion

        #region FaturamentoDveConcessionaria

        public List<eFaturamentoDVEClienteConcessionaria> GetUserRegistroFaturamentoDveConcessionaria(int ano, int mes, int dia)
        {
            List<eFaturamentoDVEClienteConcessionaria> lista = new List<eFaturamentoDVEClienteConcessionaria>();


            objSqlHelp = new SqlHelp();

            try
            {
                objSqlHelp.AddParameterToSQLCommand("@dia", SqlDbType.Int, dia);
                objSqlHelp.AddParameterToSQLCommand("@mes", SqlDbType.Int, mes);
                objSqlHelp.AddParameterToSQLCommand("@ano", SqlDbType.Int, ano);

                dr = objSqlHelp.GetReaderByCmd("USP_RELATORIO_FATURAMENTODVECONCESSIONARIA_1");

                if (dr != null)
                {
                    using (dr)
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                eFaturamentoDVEClienteConcessionaria fatCli = new eFaturamentoDVEClienteConcessionaria();

                                fatCli.Bir = dr["bir"].ToString();
                                fatCli.RazaoSocial = dr["RazaoSocial"].ToString();
                                fatCli.NomeFantasia = dr["NomeFantasia"].ToString();
                                fatCli.TotalFantasia = dr["TotalFantasia"].ToString();
                                fatCli.TotalFaturamento = dr["TotalFaturamento"].ToString();
                                fatCli.Participacao = dr["Participacao"].ToString();
                                fatCli.Jan = dr["JAN"].ToString();
                                fatCli.Fev = dr["FEV"].ToString();
                                fatCli.Mar = dr["MAR"].ToString();
                                fatCli.Abr = dr["ABR"].ToString();
                                fatCli.Mai = dr["MAI"].ToString();
                                fatCli.Jun = dr["JUN"].ToString();
                                fatCli.Jul = dr["JUL"].ToString();
                                fatCli.Ago = dr["AGO"].ToString();
                                fatCli.Set = dr["SET"].ToString();
                                fatCli.Out = dr["OUT"].ToString();
                                fatCli.Nov = dr["NOV"].ToString();
                                fatCli.Dez = dr["DEZ"].ToString();
                                fatCli.UltimoMes = dr["UltimoMes"].ToString();
                                fatCli.UltimoAno = dr["UltimoAno"].ToString();
                                fatCli.UltimoDia = dr["UltimoDia"].ToString();
                                fatCli.TJan = dr["TotalJAN"].ToString();
                                fatCli.TFev = dr["TotalFEV"].ToString();
                                fatCli.TMar = dr["TotalMAR"].ToString();
                                fatCli.TAbr = dr["TotalABR"].ToString();
                                fatCli.TMai = dr["TotalMAI"].ToString();
                                fatCli.TJun = dr["TotalJUN"].ToString();
                                fatCli.TJul = dr["TotalJUL"].ToString();
                                fatCli.TAgo = dr["TotalAGO"].ToString();
                                fatCli.TSet = dr["TotalSET"].ToString();
                                fatCli.TOut = dr["TotalOUT"].ToString();
                                fatCli.TNov = dr["TotalNOV"].ToString();
                                fatCli.TDez = dr["TotalDEZ"].ToString();

                                lista.Add(fatCli);
                            }
                        }
                    }
                }

                return lista;
            }
            catch (SqlException ex)
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

        #endregion

        #region Invasao

        public List<eInvasao> GetUserRegistroInvasao(eInvasao invasao)
        {
            List<eInvasao> lista = new List<eInvasao>();

            objSqlHelp = new SqlHelp();

            try
            {
                objSqlHelp.AddParameterToSQLCommand("@ate_dia", SqlDbType.Int, invasao.AteDia);
                objSqlHelp.AddParameterToSQLCommand("@ate_mes", SqlDbType.Int, invasao.AteMes);
                objSqlHelp.AddParameterToSQLCommand("@ate_ano", SqlDbType.Int, invasao.AteAno);
                objSqlHelp.AddParameterToSQLCommand("@de_dia", SqlDbType.Int, invasao.DeDia);
                objSqlHelp.AddParameterToSQLCommand("@de_mes", SqlDbType.Int, invasao.DeMes);
                objSqlHelp.AddParameterToSQLCommand("@de_ano", SqlDbType.Int, invasao.DeAno);
                objSqlHelp.AddParameterToSQLCommand("@area_operacional", SqlDbType.Int, invasao.AreaOperacional);

                dr = objSqlHelp.GetReaderByCmd("USP_SEL_RELATORIO_INVASAO_1");

                if (dr != null)
                {
                    using (dr)
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                eInvasao inv = new eInvasao();

                                inv.Qtd = Convert.ToInt32(dr["Qtd"]);
                                inv.EmpresaFantasia = dr["EmpresaFantasia"].ToString();
                                inv.SiglaAreaOperacional = dr["SiglaAreaOperacional"].ToString();
                                inv.Cidade = dr["Cidade"].ToString();
                                inv.NomeAreaOperacional = dr["NomeAreaOperacional"].ToString();
                                inv.TotalArea = dr["TotalArea"].ToString();
                                inv.TotalVolume = dr["TotalVolume"].ToString();

                                lista.Add(inv);
                            }
                        }
                    }
                }
                return lista;
            }
            catch (SqlException sqlex)
            {

                throw sqlex;
            }
            finally
            {
                objSqlHelp.Dispose();
                objSqlHelp.FecharConexao();
                objSqlHelp = null;
            }
        }

        #endregion

        #region InvasaoArea

        public List<eInvasao> GetUserRegistroInvasaoArea(eInvasao invasao)
        {
            List<eInvasao> lista = new List<eInvasao>();

            objSqlHelp = new SqlHelp();

            try
            {
                objSqlHelp.AddParameterToSQLCommand("@ate_dia", SqlDbType.Int, invasao.AteDia);
                objSqlHelp.AddParameterToSQLCommand("@ate_mes", SqlDbType.Int, invasao.AteMes);
                objSqlHelp.AddParameterToSQLCommand("@ate_ano", SqlDbType.Int, invasao.AteAno);
                objSqlHelp.AddParameterToSQLCommand("@de_dia", SqlDbType.Int, invasao.DeDia);
                objSqlHelp.AddParameterToSQLCommand("@de_mes", SqlDbType.Int, invasao.DeMes);
                objSqlHelp.AddParameterToSQLCommand("@de_ano", SqlDbType.Int, invasao.DeAno);
                objSqlHelp.AddParameterToSQLCommand("@area_operacional", SqlDbType.Int, invasao.AreaOperacional);

                dr = objSqlHelp.GetReaderByCmd("USP_SEL_RELATORIO_INVASAO_POR_AREA_1");

                if (dr != null)
                {
                    using (dr)
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                eInvasao inv = new eInvasao();

                                inv.Qtd = Convert.ToInt32(dr["Qtd"]);
                                inv.EmpresaFantasia = dr["EmpresaFantasia"].ToString();
                                inv.SiglaAreaOperacional = dr["SiglaAreaOperacional"].ToString();
                                inv.Cidade = dr["Cidade"].ToString();
                                inv.TVolAreaMunicipio = dr["TVolMunicipio"].ToString();
                                inv.TotalArea = dr["TVolArea"].ToString();
                                inv.PorcentagemArea = dr["PorcentagemArea"].ToString();
                                inv.PorcentagemMunicipio = dr["PorcentagemMunicipio"].ToString();

                                lista.Add(inv);
                            }
                        }
                    }
                }

                return lista;
            }
            catch (SqlException sqlex)
            {
                throw sqlex;
            }
            finally
            {
                objSqlHelp.Dispose();
                objSqlHelp.FecharConexao();
                objSqlHelp = null;
            }

        }

        #endregion

        #region BasketMes

        public List<eBasket> GetUserRegistroBasketMes(eBasket basket)
        {
            List<eBasket> lista = new List<eBasket>();

            objSqlHelp = new SqlHelp();

            try
            {
                objSqlHelp.AddParameterToSQLCommand("@dia", SqlDbType.Int, basket.Dia);
                objSqlHelp.AddParameterToSQLCommand("@mes", SqlDbType.Int, basket.Mes);
                objSqlHelp.AddParameterToSQLCommand("@ano", SqlDbType.Int, basket.Ano);
                objSqlHelp.AddParameterToSQLCommand("@dia_de", SqlDbType.Int, basket.DiaDe);
                objSqlHelp.AddParameterToSQLCommand("@mes_de", SqlDbType.Int, basket.MesDe);
                objSqlHelp.AddParameterToSQLCommand("@ano_de", SqlDbType.Int, basket.AnoDe);
                objSqlHelp.AddParameterToSQLCommand("@BASKETID", SqlDbType.VarChar, basket.BasketId);
                objSqlHelp.AddParameterToSQLCommand("@regiao_operacional", SqlDbType.Int, basket.RegiaoOperacional);
                objSqlHelp.AddParameterToSQLCommand("@regiao_geografico", SqlDbType.Int, basket.RegiaoGeografico);
                objSqlHelp.AddParameterToSQLCommand("@regiao_metropolitana", SqlDbType.Int, basket.RegiaoMetropolitana);
                objSqlHelp.AddParameterToSQLCommand("@estado", SqlDbType.Int, basket.Estado);
                objSqlHelp.AddParameterToSQLCommand("@cidade", SqlDbType.Int, basket.Cidade);
                objSqlHelp.AddParameterToSQLCommand("@area_operacional", SqlDbType.Int, basket.AreaOperacional);

                dr = objSqlHelp.GetReaderByCmd("STP_BASKET_MENSAL_1");

                if (dr != null)
                {
                    using (dr)
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                eBasket bask = new eBasket();

                                bask.Descricao = dr["descricao"].ToString();
                                bask.Codigo = dr["codigo"].ToString();
                                bask.Qtd = Convert.ToInt32(dr["qtd"]);
                                bask.NomenclaturaMarca = dr["nomenclaturaMarca"].ToString();
                                bask.NomenclaturaModelo = dr["nomenclaturaModelo"].ToString();
                                bask.NomenclaturaVersao = dr["nomenclaturaVersao"].ToString();
                                bask.Ano = Convert.ToInt32(dr["ano"]);
                                bask.Mes = Convert.ToInt32(dr["mes"]);
                                bask.Dia = Convert.ToInt32(dr["dia"]);
                                bask.TotalMes = dr["TotalMes"].ToString();

                                lista.Add(bask);
                            }
                        }
                    }
                }

                return lista;
            }
            catch (SqlException sqlex)
            {
                throw sqlex;
            }
            finally
            {
                objSqlHelp.Dispose();
                objSqlHelp.FecharConexao();
                objSqlHelp = null;
            }
        }

        #endregion

        #region BasketAno

        public List<eBasket> GetUserRegistroBasketAno(eBasket basket)
        {

            List<eBasket> lista = new List<eBasket>();

            objSqlHelp = new SqlHelp();

            try
            {
                objSqlHelp.AddParameterToSQLCommand("@dia", SqlDbType.Int, basket.Dia);
                objSqlHelp.AddParameterToSQLCommand("@mes", SqlDbType.Int, basket.Mes);
                objSqlHelp.AddParameterToSQLCommand("@ano", SqlDbType.Int, basket.Ano);
                objSqlHelp.AddParameterToSQLCommand("@dia_de", SqlDbType.Int, basket.DiaDe);
                objSqlHelp.AddParameterToSQLCommand("@mes_de", SqlDbType.Int, basket.MesDe);
                objSqlHelp.AddParameterToSQLCommand("@ano_de", SqlDbType.Int, basket.AnoDe);
                objSqlHelp.AddParameterToSQLCommand("@BASKETID", SqlDbType.VarChar, basket.BasketId);
                objSqlHelp.AddParameterToSQLCommand("@regiao_operacional", SqlDbType.Int, basket.RegiaoOperacional);
                objSqlHelp.AddParameterToSQLCommand("@regiao_geografico", SqlDbType.Int, basket.RegiaoGeografico);
                objSqlHelp.AddParameterToSQLCommand("@regiao_metropolitana", SqlDbType.Int, basket.RegiaoMetropolitana);
                objSqlHelp.AddParameterToSQLCommand("@estado", SqlDbType.Int, basket.Estado);
                objSqlHelp.AddParameterToSQLCommand("@cidade", SqlDbType.Int, basket.Cidade);
                objSqlHelp.AddParameterToSQLCommand("@area_operacional", SqlDbType.Int, basket.AreaOperacional);

                dr = objSqlHelp.GetReaderByCmd("STP_BASKET_ANUAL_1");

                if (dr != null)
                {
                    using (dr)
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                eBasket bask = new eBasket();

                                bask.BasketId = dr["IdBasket"].ToString();
                                bask.Descricao = dr["descricao"].ToString();
                                bask.Codigo = dr["codigo"].ToString();
                                bask.Qtd = Convert.ToInt32(dr["qtd"]);
                                bask.NomenclaturaMarca = dr["nomenclaturaMarca"].ToString();
                                bask.NomenclaturaModelo = dr["nomenclaturaModelo"].ToString();
                                bask.NomenclaturaVersao = dr["nomenclaturaVersao"].ToString();
                                bask.Ano = Convert.ToInt32(dr["ano"]);
                                bask.Total = dr["Total"].ToString();
                                bask.TotalAno = dr["TotalAno"].ToString();
                                bask.TotalGeral = dr["TotalGeral"].ToString();
                                bask.Porcentagem = dr["Porcentagem"].ToString();

                                lista.Add(bask);
                            }
                        }
                    }
                }

                return lista;
            }
            catch (SqlException sqlex)
            {
                throw sqlex;
            }
            finally
            {
                objSqlHelp.Dispose();
                objSqlHelp.FecharConexao();
                objSqlHelp = null;
            }
        }

        #endregion

        #region BasketPeriodo

        public List<eBasketPeriodo> GetUserRegistroBaasketPeriodo(eBasketPeriodo periodo)
        {
            List<eBasketPeriodo> lista = new List<eBasketPeriodo>();

            objSqlHelp = new SqlHelp();

            try
            {
                objSqlHelp.AddParameterToSQLCommand("@dia", SqlDbType.Int, periodo.Dia);
                objSqlHelp.AddParameterToSQLCommand("@mes", SqlDbType.Int, periodo.Mes);
                objSqlHelp.AddParameterToSQLCommand("@ano", SqlDbType.Int, periodo.Ano);
                objSqlHelp.AddParameterToSQLCommand("@dia_de", SqlDbType.Int, periodo.DiaDe);
                objSqlHelp.AddParameterToSQLCommand("@mes_de", SqlDbType.Int, periodo.MesDe);
                objSqlHelp.AddParameterToSQLCommand("@ano_de", SqlDbType.Int, periodo.AnoDe);
                objSqlHelp.AddParameterToSQLCommand("@SegDiaAte", SqlDbType.Int, periodo.SegDia);
                objSqlHelp.AddParameterToSQLCommand("@SegMesAte", SqlDbType.Int, periodo.SegMes);
                objSqlHelp.AddParameterToSQLCommand("@SegAnoAte", SqlDbType.Int, periodo.SegAno);
                objSqlHelp.AddParameterToSQLCommand("@SegDiaDe", SqlDbType.Int, periodo.SegDiaDe);
                objSqlHelp.AddParameterToSQLCommand("@SegMesDe", SqlDbType.Int, periodo.SegMesDe);
                objSqlHelp.AddParameterToSQLCommand("@SegAnoDe", SqlDbType.Int, periodo.SegAnoDe);
                objSqlHelp.AddParameterToSQLCommand("@BASKETID", SqlDbType.VarChar, periodo.BasketId);
                objSqlHelp.AddParameterToSQLCommand("@regiao_operacional", SqlDbType.Int, periodo.RegiaoOperacional);
                objSqlHelp.AddParameterToSQLCommand("@regiao_geografico", SqlDbType.Int, periodo.RegiaoGeografico);
                objSqlHelp.AddParameterToSQLCommand("@regiao_metropolitana", SqlDbType.Int, periodo.RegiaoMetropolitana);
                objSqlHelp.AddParameterToSQLCommand("@estado", SqlDbType.Int, periodo.Estado);
                objSqlHelp.AddParameterToSQLCommand("@cidade", SqlDbType.Int, periodo.Cidade);
                objSqlHelp.AddParameterToSQLCommand("@area_operacional", SqlDbType.Int, periodo.AreaOperacional);


                dr = objSqlHelp.GetReaderByCmd("STP_BASKET_PERIODO_1");

                if (dr != null)
                {
                    using (dr)
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                eBasketPeriodo baskPer = new eBasketPeriodo();

                                baskPer.Descricao = dr["descricao"].ToString();
                                baskPer.Codigo = dr["codigo"].ToString();
                                baskPer.Marca = dr["marca"].ToString();
                                baskPer.Modelo = dr["modelo"].ToString();
                                baskPer.Versao = dr["versao"].ToString();
                                baskPer.DiasUteis1 = dr["DIASUTEIS01"].ToString();
                                baskPer.DiasUteis2 = dr["DIASUTEIS02"].ToString();
                                baskPer.Periodo1 = dr["periodo1"].ToString();
                                baskPer.Periodo2 = dr["periodo2"].ToString();

                                lista.Add(baskPer);
                            }
                        }
                    }
                }

                return lista;
            }
            catch (SqlException sqlex)
            {
                throw sqlex;
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


        #endregion

        #region SegmentoFUModalidade

        public List<eSegmentoFU> GetUserRegistroSegmentoFUModalidade(eSegmentoFU fuMod)
        {
            List<eSegmentoFU> lista = new List<eSegmentoFU>();

            objSqlHelp = new SqlHelp();

            try
            {
                objSqlHelp.AddParameterToSQLCommand("@dia", SqlDbType.Int, fuMod.Dia);
                objSqlHelp.AddParameterToSQLCommand("@mes", SqlDbType.Int, fuMod.Mes);
                objSqlHelp.AddParameterToSQLCommand("@ano", SqlDbType.Int, fuMod.Ano);
                objSqlHelp.AddParameterToSQLCommand("@dia_de", SqlDbType.Int, fuMod.DiaDe);
                objSqlHelp.AddParameterToSQLCommand("@mes_de", SqlDbType.Int, fuMod.MesDe);
                objSqlHelp.AddParameterToSQLCommand("@ano_de", SqlDbType.Int, fuMod.AnoDe);
                objSqlHelp.AddParameterToSQLCommand("@tipo_venda", SqlDbType.Int, fuMod.TipoVenda);
                objSqlHelp.AddParameterToSQLCommand("@regiao_operacional", SqlDbType.Int, fuMod.RegiaoOperacional);
                objSqlHelp.AddParameterToSQLCommand("@regiao_geografico", SqlDbType.Int, fuMod.RegiaoGeografico);
                objSqlHelp.AddParameterToSQLCommand("@regiao_metropolitana", SqlDbType.Int, fuMod.RegiaoMetropolitana);
                objSqlHelp.AddParameterToSQLCommand("@estado", SqlDbType.Int, fuMod.Estado);
                objSqlHelp.AddParameterToSQLCommand("@cidade", SqlDbType.Int, fuMod.Cidade);
                objSqlHelp.AddParameterToSQLCommand("@area_operacional", SqlDbType.Int, fuMod.AreaOperacional);
                objSqlHelp.AddParameterToSQLCommand("@segmento", SqlDbType.VarChar, fuMod.Segmento);
                objSqlHelp.AddParameterToSQLCommand("@marca", SqlDbType.VarChar, fuMod.Marca);
                objSqlHelp.AddParameterToSQLCommand("@modelo", SqlDbType.VarChar, fuMod.Modelo);

                dr = objSqlHelp.GetReaderByCmd("USP_RELATORIO_FU_MODALIDADE_1");

                if (dr != null)
                {
                    using (dr)
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                eSegmentoFU seg = new eSegmentoFU();

                                seg.Segmento = dr["Segmento"].ToString();
                                seg.Marca = dr["Marca"].ToString();
                                seg.Modelo = dr["Modelo"].ToString();
                                seg.MarcaCliente = dr["MarcaCliente"].ToString();
                                seg.VD = dr["VD"].ToString();
                                seg.Varejo = dr["Varejo"].ToString();
                                seg.TotalVol = dr["TotalVol"].ToString();
                                seg.TotalVolVD = dr["TotalVolVd"].ToString();
                                seg.TotalVolVarejo = dr["TotalVolVarejo"].ToString();
                                seg.TotalVolTotal = dr["TotalVolTotal"].ToString();
                                seg.SomaTotaisVD = dr["SomaTotaisVD"].ToString();
                                seg.SomaTotaisVarejo = dr["SomaTotaisVarejo"].ToString();
                                seg.SomaTotaisTotal = dr["SomaTotaisTotal"].ToString();
                                seg.PorcenSegVD = dr["PorcenSegVD"].ToString();
                                seg.PorcenSegVarejo = dr["PorcenSegVarejo"].ToString();
                                seg.PorcenSegTotal = dr["PorcenSegTotal"].ToString();
                                seg.PorcenTotalVD = dr["PorcenTotalVD"].ToString();
                                seg.PorcenTotalVarejo = dr["PorcenTotalVarejo"].ToString();
                                seg.PorcenTotalTotal = dr["PorcenTotalTotal"].ToString();
                                seg.TotalPorcentSegVD = dr["TotalPorcentSegVD"].ToString();
                                seg.TotalPorcentSegVarejo = dr["TotalPorcentSegVarejo"].ToString();
                                seg.TotalPorcenTotalVD = dr["TotalPorcenTotalVD"].ToString();
                                seg.TotalPorcenTotalVarejo = dr["TotalPorcenTotalVarejo"].ToString();
                                seg.TotalPorcenTotalTotal = dr["TotalPorcenTotalTotal"].ToString();

                                lista.Add(seg);
                            }
                        }
                    }
                }

                return lista;
            }
            catch (SqlException sqlex)
            {
                throw sqlex;
            }
            finally
            {
                objSqlHelp.Dispose();
                objSqlHelp.FecharConexao();
                objSqlHelp = null;
            }
        }

        #endregion

        #region SegmentoFUPeriodo

        public List<eSegmentoFUPeriodo> GetUserRegistroSegmentoFUPeriodo(eSegmentoFUPeriodo segmento)
        {
            List<eSegmentoFUPeriodo> lista = new List<eSegmentoFUPeriodo>();

            objSqlHelp = new SqlHelp();

            try
            {
                objSqlHelp.AddParameterToSQLCommand("@dia_de", SqlDbType.Int, segmento.DiaDe);
                objSqlHelp.AddParameterToSQLCommand("@mes_de", SqlDbType.Int, segmento.MesDe);
                objSqlHelp.AddParameterToSQLCommand("@ano_de", SqlDbType.Int, segmento.AnoDe);
                objSqlHelp.AddParameterToSQLCommand("@dia", SqlDbType.Int, segmento.DiaAte);
                objSqlHelp.AddParameterToSQLCommand("@mes", SqlDbType.Int, segmento.MesAte);
                objSqlHelp.AddParameterToSQLCommand("@ano", SqlDbType.Int, segmento.AnoAte);
                objSqlHelp.AddParameterToSQLCommand("@SegDiaDe", SqlDbType.Int, segmento.SegDiaDe);
                objSqlHelp.AddParameterToSQLCommand("@SegMesDe", SqlDbType.Int, segmento.SegMesDe);
                objSqlHelp.AddParameterToSQLCommand("@SegAnoDe", SqlDbType.Int, segmento.SegAnoDe);
                objSqlHelp.AddParameterToSQLCommand("@SegDiaAte", SqlDbType.Int, segmento.SegDiaAte);
                objSqlHelp.AddParameterToSQLCommand("@SegMesAte", SqlDbType.Int, segmento.SegMesAte);
                objSqlHelp.AddParameterToSQLCommand("@SegAnoAte", SqlDbType.Int, segmento.SegAnoAte);
                objSqlHelp.AddParameterToSQLCommand("@tipo_venda", SqlDbType.Int, segmento.TipoVenda);
                objSqlHelp.AddParameterToSQLCommand("@regiao_operacional", SqlDbType.Int, segmento.RegiaoOperacional);
                objSqlHelp.AddParameterToSQLCommand("@regiao_geografico", SqlDbType.Int, segmento.RegiaoGeografico);
                objSqlHelp.AddParameterToSQLCommand("@regiao_metropolitana", SqlDbType.Int, segmento.RegiaoMetropolitana);
                objSqlHelp.AddParameterToSQLCommand("@estado", SqlDbType.Int, segmento.Estado);
                objSqlHelp.AddParameterToSQLCommand("@cidade", SqlDbType.Int, segmento.Cidade);
                objSqlHelp.AddParameterToSQLCommand("@area_operacional", SqlDbType.Int, segmento.AreaOperacional);
                objSqlHelp.AddParameterToSQLCommand("@segmento", SqlDbType.Int, segmento.Segmento);
                objSqlHelp.AddParameterToSQLCommand("@marca", SqlDbType.Int, segmento.Marca);
                objSqlHelp.AddParameterToSQLCommand("@modelo", SqlDbType.Int, segmento.Modelo);

                dr = objSqlHelp.GetReaderByCmd("USP_RELATORIO_FU_PERIODO_1");


                if (dr != null)
                {
                    using (dr)
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                eSegmentoFUPeriodo segP = new eSegmentoFUPeriodo();

                                segP.Segmento = dr["Segmento"].ToString();
                                segP.Marca = dr["Marca"].ToString();
                                segP.Modelo = dr["Modelo"].ToString();
                                segP.Primeiro = dr["Primeiro"].ToString();
                                segP.Segundo = dr["Segundo"].ToString();
                                segP.MarcaCliente = dr["MarcaCliente"].ToString();
                                segP.TotalSeg1 = dr["TotalSegmento1"].ToString();
                                segP.TotalSeg2 = dr["TotalSegmento2"].ToString();
                                segP.TotalGeral1 = dr["TotalGeral1"].ToString();
                                segP.TotalGeral2 = dr["TotalGeral2"].ToString();
                                segP.PorcSeg1 = dr["PorcenSeg1"].ToString();
                                segP.PorcSeg2 = dr["PorcenSeg2"].ToString();
                                segP.PorcTotal1 = dr["PorcenTotal1"].ToString();
                                segP.PorcTotal2 = dr["PorcenTotal2"].ToString();
                                segP.PorcenGeral1 = dr["PorcenGeral1"].ToString();
                                segP.PorcenGeral2 = dr["PorcenGeral2"].ToString();

                                lista.Add(segP);
                            }
                        }
                    }
                }

                return lista;
            }
            catch (SqlException sqlex)
            {
                throw sqlex;
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

        #endregion

        #region SegmentoFULocalidade

        public List<eSegmentoFU> GetUserRegistroSegmentoFULocalidade(eSegmentoFU fuLoc)
        {
            List<eSegmentoFU> lista = new List<eSegmentoFU>();

            objSqlHelp = new SqlHelp();

            try
            {
                objSqlHelp.AddParameterToSQLCommand("@dia", SqlDbType.Int, fuLoc.Dia);
                objSqlHelp.AddParameterToSQLCommand("@mes", SqlDbType.Int, fuLoc.Mes);
                objSqlHelp.AddParameterToSQLCommand("@ano", SqlDbType.Int, fuLoc.Ano);
                objSqlHelp.AddParameterToSQLCommand("@dia_de", SqlDbType.Int, fuLoc.DiaDe);
                objSqlHelp.AddParameterToSQLCommand("@mes_de", SqlDbType.Int, fuLoc.MesDe);
                objSqlHelp.AddParameterToSQLCommand("@ano_de", SqlDbType.Int, fuLoc.AnoDe);
                objSqlHelp.AddParameterToSQLCommand("@tipo_venda", SqlDbType.Int, fuLoc.TipoVenda);
                objSqlHelp.AddParameterToSQLCommand("@regiao_operacional", SqlDbType.Int, fuLoc.RegiaoOperacional);
                objSqlHelp.AddParameterToSQLCommand("@regiao_geografico", SqlDbType.Int, fuLoc.RegiaoGeografico);
                objSqlHelp.AddParameterToSQLCommand("@regiao_metropolitana", SqlDbType.Int, fuLoc.RegiaoMetropolitana);
                objSqlHelp.AddParameterToSQLCommand("@estado", SqlDbType.Int, fuLoc.Estado);
                objSqlHelp.AddParameterToSQLCommand("@cidade", SqlDbType.Int, fuLoc.Cidade);
                objSqlHelp.AddParameterToSQLCommand("@area_operacional", SqlDbType.Int, fuLoc.AreaOperacional);
                objSqlHelp.AddParameterToSQLCommand("@segmento", SqlDbType.VarChar, fuLoc.Segmento);
                objSqlHelp.AddParameterToSQLCommand("@marca", SqlDbType.VarChar, fuLoc.Marca);
                objSqlHelp.AddParameterToSQLCommand("@modelo", SqlDbType.VarChar, fuLoc.Modelo);

                dr = objSqlHelp.GetReaderByCmd("USP_RELATORIO_FU_LOCALIZACAO_1");

                if (dr != null)
                {
                    using (dr)
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                eSegmentoFU segFuLoc = new eSegmentoFU();

                                segFuLoc.Segmento = dr["Segmento"].ToString();
                                segFuLoc.TituloLocalizacao = dr["TituloLocalizacao"].ToString();
                                segFuLoc.MarcaCliente = dr["MarcaCliente"].ToString();
                                segFuLoc.Localizacao = dr["localizacao"].ToString();
                                segFuLoc.VD = dr["VolVD"].ToString();
                                segFuLoc.Varejo = dr["VolVarejo"].ToString();
                                segFuLoc.TotalVol = dr["VolTotal"].ToString();
                                segFuLoc.TotalVolVD = dr["VolTotalVD"].ToString();
                                segFuLoc.TotalVolVarejo = dr["VolTotalVarejo"].ToString();
                                segFuLoc.TotalVolTotal = dr["VolTotalTotal"].ToString();
                                segFuLoc.PorcenSegVD = dr["PorcentVD"].ToString();
                                segFuLoc.PorcenSegVarejo = dr["PorcentVarejo"].ToString();
                                segFuLoc.PorcenSegTotal = dr["PorcentTotal"].ToString();

                                lista.Add(segFuLoc);
                            }
                        }
                    }
                }

                return lista;
            }
            catch (SqlException sqlex)
            {

                throw sqlex;
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

        #endregion

        #region SegmentoFUAnual

        public List<eSegmentoFU> GetUserRegistroSegmentoFUAnual(eSegmentoFU segmento)
        {
            List<eSegmentoFU> lista = new List<eSegmentoFU>();


            objSqlHelp = new SqlHelp();

            try
            {
                objSqlHelp.AddParameterToSQLCommand("@dia", SqlDbType.Int, segmento.Dia);
                objSqlHelp.AddParameterToSQLCommand("@mes", SqlDbType.Int, segmento.Mes);
                objSqlHelp.AddParameterToSQLCommand("@ano", SqlDbType.Int, segmento.Ano);
                objSqlHelp.AddParameterToSQLCommand("@dia_de", SqlDbType.Int, segmento.DiaDe);
                objSqlHelp.AddParameterToSQLCommand("@mes_de", SqlDbType.Int, segmento.MesDe);
                objSqlHelp.AddParameterToSQLCommand("@ano_de", SqlDbType.Int, segmento.AnoDe);
                objSqlHelp.AddParameterToSQLCommand("@tipo_venda", SqlDbType.Int, segmento.TipoVenda);
                objSqlHelp.AddParameterToSQLCommand("@regiao_operacional", SqlDbType.Int, segmento.RegiaoOperacional);
                objSqlHelp.AddParameterToSQLCommand("@regiao_geografico", SqlDbType.Int, segmento.RegiaoGeografico);
                objSqlHelp.AddParameterToSQLCommand("@regiao_metropolitana", SqlDbType.Int, segmento.RegiaoMetropolitana);
                objSqlHelp.AddParameterToSQLCommand("@estado", SqlDbType.Int, segmento.Estado);
                objSqlHelp.AddParameterToSQLCommand("@cidade", SqlDbType.Int, segmento.Cidade);
                objSqlHelp.AddParameterToSQLCommand("@area_operacional", SqlDbType.Int, segmento.AreaOperacional);
                objSqlHelp.AddParameterToSQLCommand("@segmento", SqlDbType.VarChar, segmento.Segmento);
                objSqlHelp.AddParameterToSQLCommand("@marca", SqlDbType.VarChar, segmento.Marca);
                objSqlHelp.AddParameterToSQLCommand("@modelo", SqlDbType.VarChar, segmento.Modelo);
                objSqlHelp.AddParameterToSQLCommand("@listaanos", SqlDbType.VarChar, segmento.ListaAnos);

                dr = objSqlHelp.GetReaderByCmd("USP_RELATORIO_FU_ANUAL_1");

                if (dr != null)
                {
                    using (dr)
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                eSegmentoFU seg = new eSegmentoFU();

                                seg.Segmento = dr["Segmento"].ToString();
                                seg.Marca = dr["Marca"].ToString();
                                seg.Modelo = dr["Modelo"].ToString();
                                seg.Ano = Convert.ToInt32(dr["Ano"]);
                                seg.MarcaCliente = dr["MarcaCliente"].ToString();
                                seg.Qtd = dr["Qtd"].ToString();
                                seg.TotalGeralSegmento = dr["TotalGeralSegmento"].ToString();
                                seg.TotalGeralAnual = dr["TotalGeralAnual"].ToString();
                                seg.PorcentSeg = dr["PorcentSeg"].ToString();
                                seg.PorcentTotal = dr["PorcentTotal"].ToString();
                                seg.PorcentTotalGeralSeg = dr["PorcentTotalGeralSeg"].ToString();
                                seg.DiaUtil = dr["DiaUtil"].ToString();

                                lista.Add(seg);
                            }
                        }
                    }
                }


                return lista;
            }
            catch (SqlException sqlex)
            {
                throw sqlex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Detalhamento

        public List<eDetalhamento> GetUserRegistroDetalhamento(eDetalhamento detalhe)
        {
            List<eDetalhamento> lista = new List<eDetalhamento>();


            objSqlHelp = new SqlHelp();

            try
            {
                objSqlHelp.AddParameterToSQLCommand("@ate_dia", SqlDbType.Int, detalhe.AteDia);
                objSqlHelp.AddParameterToSQLCommand("@ate_mes", SqlDbType.Int, detalhe.AteMes);
                objSqlHelp.AddParameterToSQLCommand("@ate_ano", SqlDbType.Int, detalhe.AteAno);
                objSqlHelp.AddParameterToSQLCommand("@fl_dias_uteis", SqlDbType.Int, detalhe.FlDiasUteis);
                objSqlHelp.AddParameterToSQLCommand("@categoria", SqlDbType.Int, detalhe.Categoria);
                objSqlHelp.AddParameterToSQLCommand("@tipo_venda", SqlDbType.Int, detalhe.TipoVenda);
                objSqlHelp.AddParameterToSQLCommand("@segmento", SqlDbType.NVarChar, detalhe.Segmento);
                objSqlHelp.AddParameterToSQLCommand("@regiao_operacional", SqlDbType.Int, detalhe.RegiaoOperacional);
                objSqlHelp.AddParameterToSQLCommand("@regiao_geografico", SqlDbType.Int, detalhe.RegiaoGeografico);
                objSqlHelp.AddParameterToSQLCommand("@estado", SqlDbType.Int, detalhe.Estado);
                objSqlHelp.AddParameterToSQLCommand("@regiao_metropolitana", SqlDbType.Int, detalhe.RegiaoMetropolitana);
                objSqlHelp.AddParameterToSQLCommand("@cidade", SqlDbType.Int, detalhe.Cidade);
                objSqlHelp.AddParameterToSQLCommand("@area_operacional", SqlDbType.Int, detalhe.AreaOperacional);
                objSqlHelp.AddParameterToSQLCommand("@concessionaria", SqlDbType.Int, detalhe.Concessionaria);
                objSqlHelp.AddParameterToSQLCommand("@bygroup", SqlDbType.Int, detalhe.ByGroup);


                dr = objSqlHelp.GetReaderByCmd("USP_DETALHAMENTO_1");

                if (dr != null)
                {
                    using (dr)
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                eDetalhamento deta = new eDetalhamento();

                                deta.AreaConc = dr["AreaConc"].ToString();
                                deta.IdLocalizacao = dr["IdLocalizacao"].ToString();
                                deta.IdAreaOperacional = dr["IdAreaOperacional"].ToString();
                                deta.NomeAreaOperacional = dr["NomeAreaOperacional"].ToString();
                                deta.IdCidade = dr["IdCidade"].ToString();
                                deta.NomeCidade = dr["NomeCidade"].ToString();
                                deta.Vol1 = dr["Dia1"].ToString();
                                deta.Vol2 = dr["Dia2"].ToString();
                                deta.Vol3 = dr["Dia3"].ToString();
                                deta.Vol4 = dr["Dia4"].ToString();
                                deta.Vol5 = dr["Dia5"].ToString();
                                deta.Acumulado = dr["Acumulado"].ToString();
                                deta.TotalVol1 = dr["TotalDia1"].ToString();
                                deta.TotalVol2 = dr["TotalDia2"].ToString();
                                deta.TotalVol3 = dr["TotalDia3"].ToString();
                                deta.TotalVol4 = dr["TotalDia4"].ToString();
                                deta.TotalVol5 = dr["TotalDia5"].ToString();
                                deta.TotalAcumulado = dr["TotalAcumulado"].ToString();
                                deta.PorcentVol1 = dr["PorcentDia1"].ToString();
                                deta.PorcentVol2 = dr["PorcentDia2"].ToString();
                                deta.PorcentVol3 = dr["PorcentDia3"].ToString();
                                deta.PorcentVol4 = dr["PorcentDia4"].ToString();
                                deta.PorcentVol5 = dr["PorcentDia5"].ToString();
                                deta.PorcentAcumulado = dr["PorcentAcumulado"].ToString();
                                deta.Nome = dr["Nome"].ToString();
                                deta.Grupo = dr["Grupo"].ToString();

                                lista.Add(deta);

                            }
                        }
                    }
                }

                return lista;
            }
            catch (SqlException sqlex)
            {
                throw sqlex;
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

        public List<eDetalhamento> GetUserRegistroDetalhamentoDias(eDetalhamento detalhe)
        {
            List<eDetalhamento> lista = new List<eDetalhamento>();

            objSqlHelp = new SqlHelp();

            try
            {
                objSqlHelp.AddParameterToSQLCommand("@ate_dia", SqlDbType.Int, detalhe.AteDia);
                objSqlHelp.AddParameterToSQLCommand("@ate_mes", SqlDbType.Int, detalhe.AteMes);
                objSqlHelp.AddParameterToSQLCommand("@ate_ano", SqlDbType.Int, detalhe.AteAno);

                dr = objSqlHelp.GetReaderByCmd("USP_RELATORIO_DETALHAMENTO_DIAS_1");

                if (dr != null)
                {
                    using (dr)
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                eDetalhamento deta = new eDetalhamento();

                                deta.Dia1 = dr["dia1"].ToString();
                                deta.Dia2 = dr["dia2"].ToString();
                                deta.Dia3 = dr["dia3"].ToString();
                                deta.Dia4 = dr["dia4"].ToString();
                                deta.Dia5 = dr["dia5"].ToString();
                                deta.DiasValidos = dr["DiaValidos"].ToString();

                                lista.Add(deta);
                            }
                        }
                    }
                }
                return lista;
            }
            catch (SqlException sqlex)
            {
                throw sqlex;
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

        #endregion

        #region DetalhesVeiculoChassi

        public List<eDetalheVeiculoChassi> GetUserRegistroDetalhesVeiculoChassi(eDetalheVeiculoChassi detaV)
        {
            List<eDetalheVeiculoChassi> lista = new List<eDetalheVeiculoChassi>();

            objSqlHelp = new SqlHelp();

            try
            {
                objSqlHelp.AddParameterToSQLCommand("@EMPRESA", SqlDbType.Int, detaV.IdEmpresa);
                objSqlHelp.AddParameterToSQLCommand("@MES", SqlDbType.Int, detaV.Mes);
                objSqlHelp.AddParameterToSQLCommand("@ANO", SqlDbType.Int, detaV.Ano);
                objSqlHelp.AddParameterToSQLCommand("@grupo", SqlDbType.Bit, detaV.Grupo);
                objSqlHelp.AddParameterToSQLCommand("@Area", SqlDbType.Int, detaV.Area);
                objSqlHelp.AddParameterToSQLCommand("@LOCALIZACAO", SqlDbType.Int, detaV.IdLocalizacao);

                dr = objSqlHelp.GetReaderByCmd("STP_DETALHES_VEICULOS_CHASSI_1");

                if (dr != null)
                {
                    using (dr)
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                eDetalheVeiculoChassi detaVeiculos = new eDetalheVeiculoChassi();

                                detaVeiculos.Data = dr["Data"].ToString();
                                detaVeiculos.NomeVeiculo = dr["NomeVeiculo"].ToString();
                                detaVeiculos.Modelo = dr["Modelo"].ToString();
                                detaVeiculos.Chassis = dr["Chassis"].ToString();
                                detaVeiculos.Placa = dr["Placa"].ToString();
                                detaVeiculos.IdEmpresa = Convert.ToInt32(dr["IdEmpresa"]);

                                lista.Add(detaVeiculos);
                            }
                        }
                    }
                }

                return lista;
            }
            catch (SqlException sqlex)
            {
                throw sqlex;
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

        #endregion

        #region DetalhamentoAnual

        public List<eDetalhamentoAnual> GetUserRelatorioDetalhamentoAnual(eDetalhamentoAnual deteAnual)
        {
            List<eDetalhamentoAnual> lista = new List<eDetalhamentoAnual>();

            objSqlHelp = new SqlHelp();

            try
            {
                objSqlHelp.AddParameterToSQLCommand("@ano", SqlDbType.Int, deteAnual.Ano);
                objSqlHelp.AddParameterToSQLCommand("@categoria", SqlDbType.Int, deteAnual.Categoria);
                objSqlHelp.AddParameterToSQLCommand("@tipo_venda", SqlDbType.Int, deteAnual.TipoVenda);
                objSqlHelp.AddParameterToSQLCommand("@segmento", SqlDbType.Int, deteAnual.Segmento);
                objSqlHelp.AddParameterToSQLCommand("@regiao_operacional", SqlDbType.Int, deteAnual.RegiaoOperacional);
                objSqlHelp.AddParameterToSQLCommand("@regiao_geografico", SqlDbType.Int, deteAnual.RegiaoGeografico);
                objSqlHelp.AddParameterToSQLCommand("@regiao_metropolitana", SqlDbType.Int, deteAnual.RegiaoMetropolitana);
                objSqlHelp.AddParameterToSQLCommand("@estado", SqlDbType.Int, deteAnual.Estado);
                objSqlHelp.AddParameterToSQLCommand("@cidade", SqlDbType.Int, deteAnual.IdCidade);
                objSqlHelp.AddParameterToSQLCommand("@area_operacional", SqlDbType.Int, deteAnual.AreaOperacional);
                objSqlHelp.AddParameterToSQLCommand("@concessionaria", SqlDbType.Int, deteAnual.Concessionaria);
                objSqlHelp.AddParameterToSQLCommand("@bygroup", SqlDbType.Int, deteAnual.ByGroup);

                dr = objSqlHelp.GetReaderByCmd("USP_DETALHAMENTO_ANUAL_1");

                if (dr != null)
                {
                    using (dr)
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                eDetalhamentoAnual dAnual = new eDetalhamentoAnual();

                                dAnual.AreaConc = dr["AreaCon"].ToString();
                                dAnual.IdLocalizacao = dr["IdLocalizacao"].ToString();
                                dAnual.IdAreaOperacional = dr["IdAreaOperacional"].ToString();
                                dAnual.NomeAreaOperacional = dr["NomeAreaOperacional"].ToString();
                                dAnual.IdCidade = dr["IdCidade"].ToString();
                                dAnual.NomeCidade = dr["NomeCidade"].ToString();
                                dAnual.NomeEmpresa = dr["NomeEmpresa"].ToString();
                                dAnual.Grupo = dr["Grupo"].ToString();
                                dAnual.IdEmpresa = dr["IdEmpresa"].ToString();
                                dAnual.Vol_Ano_Anterior = dr["Vol_Ano_Anterior"].ToString();
                                dAnual.Vol_Ano_Atual = dr["Vol_Ano_Atual"].ToString();
                                dAnual.Vol_Janeiro = dr["Vol_Janeiro"].ToString();
                                dAnual.Vol_Fevereiro = dr["Vol_Fevereiro"].ToString();
                                dAnual.Vol_Marco = dr["Vol_Marco"].ToString();
                                dAnual.Vol_Abril = dr["Vol_Abril"].ToString();
                                dAnual.Vol_Maio = dr["Vol_Maio"].ToString();
                                dAnual.Vol_Junho = dr["Vol_Junho"].ToString();
                                dAnual.Vol_Julho = dr["Vol_Julho"].ToString();
                                dAnual.Vol_Agosto = dr["Vol_Agosto"].ToString();
                                dAnual.Vol_Setembro = dr["Vol_Setembro"].ToString();
                                dAnual.Vol_Outubro = dr["Vol_Outubro"].ToString();
                                dAnual.Vol_Novembro = dr["Vol_Novembro"].ToString();
                                dAnual.Vol_Dezembro = dr["Vol_Dezembro"].ToString();
                                dAnual.Total_Ano_Anterior = dr["Total_Ano_Anterior"].ToString();
                                dAnual.Total_Ano_Atual = dr["Total_Ano_Atual"].ToString();
                                dAnual.Total_Janeiro = dr["Total_Janeiro"].ToString();
                                dAnual.Total_Fevereiro = dr["Total_Fevereiro"].ToString();
                                dAnual.Total_Marco = dr["Total_Marco"].ToString();
                                dAnual.Total_Abril = dr["Total_Abril"].ToString();
                                dAnual.Total_Maio = dr["Total_Maio"].ToString();
                                dAnual.Total_Junho = dr["Total_Junho"].ToString();
                                dAnual.Total_Julho = dr["Total_Julho"].ToString();
                                dAnual.Total_Agosto = dr["Total_Agosto"].ToString();
                                dAnual.Total_Setembro = dr["Total_Setembro"].ToString();
                                dAnual.Total_Outubro = dr["Total_Outubro"].ToString();
                                dAnual.Total_Novembro = dr["Total_Novembro"].ToString();
                                dAnual.Total_Dezembro = dr["Total_Dezembro"].ToString();
                                dAnual.Porcent_Ano_Anterior = dr["Porcent_Ano_Anterior"].ToString();
                                dAnual.Porcent_Ano_Atual = dr["Porcent_Ano_Atual"].ToString();
                                dAnual.Porcent_Janeiro = dr["Porcent_Janeiro"].ToString();
                                dAnual.Porcent_Fevereiro = dr["Porcent_Fevereiro"].ToString();
                                dAnual.Porcent_Marco = dr["Porcent_Marco"].ToString();
                                dAnual.Porcent_Abril = dr["Porcent_Abril"].ToString();
                                dAnual.Porcent_Maio = dr["Porcent_Maio"].ToString();
                                dAnual.Porcent_Junho = dr["Porcent_Junho"].ToString();
                                dAnual.Porcent_Julho = dr["Porcent_Julho"].ToString();
                                dAnual.Porcent_Agosto = dr["Porcent_Agosto"].ToString();
                                dAnual.Porcent_Setembro = dr["Porcent_Setembro"].ToString();
                                dAnual.Porcent_Outubro = dr["Porcent_Outubro"].ToString();
                                dAnual.Porcent_Novembro = dr["Porcent_Novembro"].ToString();
                                dAnual.Porcent_Dezembro = dr["Porcent_Dezembro"].ToString();

                                lista.Add(dAnual);

                            }
                        }
                    }
                }

                return lista;
            }
            catch (SqlException sqlex)
            {
                throw sqlex;
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

        #endregion

        #region EmplacamentoCidade

        public List<eEmplacamentoCidade> GetUserRelatorioEmplacamentoCidade(eEmplacamentoCidade emplac)
        {
            List<eEmplacamentoCidade> listaEmplacamentoCidade = new List<eEmplacamentoCidade>();

            objSqlHelp = new SqlHelp();

            objSqlHelp.AddParameterToSQLCommand("@ATE_DIA", SqlDbType.Int, emplac.AteDia);
            objSqlHelp.AddParameterToSQLCommand("@ATE_MES", SqlDbType.Int, emplac.AteMes);
            objSqlHelp.AddParameterToSQLCommand("@ATE_ANO", SqlDbType.Int, emplac.AteAno);
            objSqlHelp.AddParameterToSQLCommand("@CATEGORIA", SqlDbType.Int, emplac.Categoria);
            objSqlHelp.AddParameterToSQLCommand("@TIPO_VENDA", SqlDbType.Int, emplac.TipoVenda);
            objSqlHelp.AddParameterToSQLCommand("@SEGMENTO", SqlDbType.NVarChar, emplac.Seguimento);
            objSqlHelp.AddParameterToSQLCommand("@CONCESSIONARIA", SqlDbType.Int, emplac.Concessionaria);
            objSqlHelp.AddParameterToSQLCommand("@BYGROUP", SqlDbType.Bit, emplac.ByGroup);
            objSqlHelp.AddParameterToSQLCommand("@ANUAL", SqlDbType.Bit, emplac.Anual);
            objSqlHelp.AddParameterToSQLCommand("@RANKING", SqlDbType.Bit, emplac.Ranking2);


            dr = objSqlHelp.GetReaderByCmd("USP_RELATORIO_CIDADE_1");
            try
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        eEmplacamentoCidade emplac1 = new eEmplacamentoCidade();

                        emplac1.Ranking = dr["ranking"].ToString();
                        emplac1.CorLinha = dr["CorLinha"].ToString();
                        emplac1.IdCidade = dr["id_cidade"].ToString();
                        emplac1.NomeCidade = dr["nm_cidade"].ToString();
                        emplac1.Ano = dr["datavalida_ano"].ToString();
                        emplac1.Mes = dr["datavalida_nm_do_mes"].ToString();
                        emplac1.EmpresaEmpId = dr["empresa_emp_id"].ToString();
                        emplac1.ConfereEmpresa = dr["confere_empresa"].ToString();
                        emplac1.NomeclaturaModelos = dr["NomenclaturaModelosValores_nomenclaturaMarca"].ToString();
                        emplac1.IdLocalizacaoOperacional = dr["localizacao_id_area_operacional"].ToString();
                        emplac1.NomeLocalizacaoOperacional = dr["localizacao_nm_area_operacional"].ToString();
                        emplac1.NomeEmpresa = dr["NomeFantasia"].ToString();
                        emplac1.Ordem = Convert.ToInt32(dr["Ordem"]);
                        emplac1.VolMarca = dr["VolMarca"].ToString();
                        emplac1.VolMarcaTotal = dr["VolMarcaTotal"].ToString();
                        emplac1.PorcentMarca = dr["PorcentMarca"].ToString();
                        emplac1.PorcentTotalMarca = dr["PorcentTotalMarca"].ToString();
                        emplac1.Vol = dr["Vol"].ToString();
                        emplac1.Total = dr["Total"].ToString();
                        emplac1.TotalGeral = dr["TotalGeral"].ToString();
                        emplac1.Porcent = dr["Porcent"].ToString();
                        emplac1.PorcentTotal = dr["PorcentTotal"].ToString();
                        emplac1.DiasUtil = dr["DiaUtil"].ToString();

                        listaEmplacamentoCidade.Add(emplac1);
                    }
                }

                return listaEmplacamentoCidade;
            }
            catch (SqlException sqlex)
            {
                throw sqlex;
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

        public List<eEmplacamentoCidade> GetUserRelatorioEmplacamentoCidadeFora(eEmplacamentoCidade emplac)
        {
            List<eEmplacamentoCidade> listaEmplacamentoCidade = new List<eEmplacamentoCidade>();

            objSqlHelp = new SqlHelp();

            objSqlHelp.AddParameterToSQLCommand("@ATE_DIA", SqlDbType.Int, emplac.AteDia);
            objSqlHelp.AddParameterToSQLCommand("@ATE_MES", SqlDbType.Int, emplac.AteMes);
            objSqlHelp.AddParameterToSQLCommand("@ATE_ANO", SqlDbType.Int, emplac.AteAno);
            objSqlHelp.AddParameterToSQLCommand("@CATEGORIA", SqlDbType.Int, emplac.Categoria);
            objSqlHelp.AddParameterToSQLCommand("@TIPO_VENDA", SqlDbType.Int, emplac.TipoVenda);
            objSqlHelp.AddParameterToSQLCommand("@SEGMENTO", SqlDbType.NVarChar, emplac.Seguimento);
            objSqlHelp.AddParameterToSQLCommand("@CONCESSIONARIA", SqlDbType.Int, emplac.Concessionaria);
            objSqlHelp.AddParameterToSQLCommand("@BYGROUP", SqlDbType.Bit, emplac.ByGroup);
            objSqlHelp.AddParameterToSQLCommand("@ANUAL", SqlDbType.Bit, emplac.Anual);
            objSqlHelp.AddParameterToSQLCommand("@RANKING", SqlDbType.Bit, emplac.Ranking2);


            dr = objSqlHelp.GetReaderByCmd("USP_RELATORIO_EMPLACAMENTO_CIDADE_FORA");
            try
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        eEmplacamentoCidade emplac1 = new eEmplacamentoCidade();

                        emplac1.Ranking = dr["ranking"].ToString();
                        emplac1.CorLinha = dr["CorLinha"].ToString();
                        emplac1.IdCidade = dr["id_cidade"].ToString();
                        emplac1.NomeCidade = dr["nm_cidade"].ToString();
                        emplac1.Ano = dr["datavalida_ano"].ToString();
                        emplac1.Mes = dr["datavalida_nm_do_mes"].ToString();
                        emplac1.EmpresaEmpId = dr["empresa_emp_id"].ToString();
                        emplac1.ConfereEmpresa = dr["confere_empresa"].ToString();
                        emplac1.NomeclaturaModelos = dr["NomenclaturaModelosValores_nomenclaturaMarca"].ToString();
                        emplac1.IdLocalizacaoOperacional = dr["localizacao_id_area_operacional"].ToString();
                        emplac1.NomeLocalizacaoOperacional = dr["localizacao_nm_area_operacional"].ToString();
                        emplac1.NomeEmpresa = dr["NomeFantasia"].ToString();
                        emplac1.Ordem = Convert.ToInt32(dr["Ordem"]);
                        emplac1.VolMarca = dr["VolMarca"].ToString();
                        emplac1.VolMarcaTotal = dr["VolMarcaTotal"].ToString();
                        emplac1.PorcentMarca = dr["PorcentMarca"].ToString();
                        emplac1.PorcentTotalMarca = dr["PorcentTotalMarca"].ToString();
                        emplac1.Vol = dr["Vol"].ToString();
                        emplac1.Total = dr["Total"].ToString();
                        emplac1.TotalGeral = dr["TotalGeral"].ToString();
                        emplac1.Porcent = dr["Porcent"].ToString();
                        emplac1.PorcentTotal = dr["PorcentTotal"].ToString();
                        emplac1.DiasUtil = dr["DiaUtil"].ToString();

                        listaEmplacamentoCidade.Add(emplac1);
                    }
                }

                return listaEmplacamentoCidade;
            }
            catch (SqlException sqlex)
            {
                throw sqlex;
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

        #endregion

        #region EmplacamentoCidadeGrupo

        public List<eEmplacamentoCidade> GetUserRelatorioEmplacamentoCidadeGrupo(eEmplacamentoCidade emplac)
        {
            List<eEmplacamentoCidade> listaEmplacamentoCidade = new List<eEmplacamentoCidade>();

            objSqlHelp = new SqlHelp();

            objSqlHelp.AddParameterToSQLCommand("@ATE_DIA", SqlDbType.Int, emplac.AteDia);
            objSqlHelp.AddParameterToSQLCommand("@ATE_MES", SqlDbType.Int, emplac.AteMes);
            objSqlHelp.AddParameterToSQLCommand("@ATE_ANO", SqlDbType.Int, emplac.AteAno);
            objSqlHelp.AddParameterToSQLCommand("@CATEGORIA", SqlDbType.Int, emplac.Categoria);
            objSqlHelp.AddParameterToSQLCommand("@TIPO_VENDA", SqlDbType.Int, emplac.TipoVenda);
            objSqlHelp.AddParameterToSQLCommand("@SEGMENTO", SqlDbType.NVarChar, emplac.Seguimento);
            objSqlHelp.AddParameterToSQLCommand("@CONCESSIONARIA", SqlDbType.Int, emplac.Concessionaria);
            objSqlHelp.AddParameterToSQLCommand("@BYGROUP", SqlDbType.Bit, emplac.ByGroup);
            objSqlHelp.AddParameterToSQLCommand("@ANUAL", SqlDbType.Bit, emplac.Anual);
            objSqlHelp.AddParameterToSQLCommand("@RANKING", SqlDbType.Bit, emplac.Ranking2);


            dr = objSqlHelp.GetReaderByCmd("USP_RELATORIO_EMPLACAMENTO_CIDADE_GRUPO");
            try
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        eEmplacamentoCidade emplac1 = new eEmplacamentoCidade();

                        emplac1.Ranking = dr["ranking"].ToString();
                        emplac1.CorLinha = dr["CorLinha"].ToString();
                        emplac1.IdCidade = dr["id_cidade"].ToString();
                        emplac1.NomeCidade = dr["nm_cidade"].ToString();
                        emplac1.Ano = dr["datavalida_ano"].ToString();
                        emplac1.Mes = dr["datavalida_nm_do_mes"].ToString();
                        emplac1.EmpresaEmpId = dr["empresa_emp_id"].ToString();
                        emplac1.ConfereEmpresa = dr["confere_grupo"].ToString();
                        emplac1.NomeclaturaModelos = dr["NomenclaturaModelosValores_nomenclaturaMarca"].ToString();
                        emplac1.IdLocalizacaoOperacional = dr["localizacao_id_area_operacional"].ToString();
                        emplac1.NomeLocalizacaoOperacional = dr["localizacao_nm_area_operacional"].ToString();
                        emplac1.NomeEmpresa = dr["NomeFantasia"].ToString();
                        emplac1.DiasUtil = dr["DiaUtil"].ToString();
                        emplac1.NomeAreaOperacional = dr["NomeAreaOP"].ToString();
                        emplac1.Ordem = Convert.ToInt32(dr["Ordem"]);
                        emplac1.Vol = dr["Vol"].ToString();
                        emplac1.Total = dr["Total"].ToString();
                        emplac1.Porcent = dr["Porcent"].ToString();
                        emplac1.VolMarca = dr["VolMarcas"].ToString();
                        emplac1.VolMarcaTotal = dr["TotalMarcas"].ToString();
                        emplac1.PorcentMarca = dr["PorcentMarca"].ToString();
                        emplac1.VolTotal = dr["TotalGeralMarcas"].ToString();
                        emplac1.TotalGeral = dr["TotalGeral"].ToString();

                        listaEmplacamentoCidade.Add(emplac1);
                    }
                }

                return listaEmplacamentoCidade;
            }
            catch (SqlException sqlex)
            {
                throw sqlex;
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

        #endregion
    }
}