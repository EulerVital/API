using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using API.DAO;
using API.Models;
using API.Controllers;
using API.Controller;
using System.Web;
using System.IO;

namespace API.Controllers
{
    [RoutePrefix("api/relatorio")]
    public class RelatorioController : ApiController
    {
        dRelatorios relatorio;
        StringBuilder gerarHtml;
        StringBuilder retornarRelatorio;
        eConfiguracoes eConfig;
        Configuracoes nConfig;

        static string emplacamento;
        string EstiloTabelaBase = "style='font-size: [SIZE]pt; font-family: Tahoma,sans-serif; color: #FFF; margin-left:20pt; border-collapse: collapse; border-color:[CorBordaLinha]; text-align:center; font-weight:bold;' cellpadding='[NCelPadding]'";
        string EstiloTabelaBaseGrupo = "style='font-size: [SIZE]pt;font-family: Tahoma,sans-serif; color:#000;  border-collapse: collapse; border-color:[CorBordaLinha]; font-weight:normal;' cellpadding='[NCelPadding]'";
        string EstiloLinha = "style='border-top:1px solid; border-color:[CorBordaLinha]; color:#000; font-weight:normal; font-family: Verdana;' align='left'";

        //EmplamentoCidade
        string DataTitulo = "15° DIA UTIL - SETEMBRO 2016";
        string Concessionaria = "A. ALVES ORLANDIA";
        string Categoria = "AUTOMÓVEIS + COMERCIAIS LEVES";
        string GrupoInfluencia = "DIVINOPOLIS";
        string GrupoInfluenciaFora = "A0/MG";
        string Tipo = "Grupo";

        //Ultimo 12 Meses
        string Abrangencia = "NACIONAL";

        //AreaDeInfluencia
        string Modalidade = "VENDAS DIRETAS + NO VAREJO";
        string Area = "DIVINOPOIS";
        string Sigla = "MG";
        string TipoRelatorio = "RELATÓRIO MENSAL: ";

        //Localidade
        string AreaAbrangenciaLocalidade = "ESTADOS";

        //MarcaAno
        string ValorAcumulado = "2016 (ATÉ O 2º DIA ÚTIL OUTUBRO)";

        //Ranking Modelo
        string DiaUtil = "2º DIA ÚTIL";

        //RankingConcessionariaGrupo
        string GrupoOuNao = "RANKING DE EMPLACAMENTO POR CONCESSIONÁRIAS";

        //VeiculosInfo

        int Regis = 0;

        public RelatorioController(string id)
        {
            GetRelatorio(id);
        }

        [Route("{id}")]
        public string GetRelatorio(string id)
        {

            using (TextWriter d = File.CreateText("E:\\perl.txt"))
            {
                HttpResponse c = new HttpResponse(d);
                //c.Redirect("Relatorios.aspx?id=" + id);
            }

            Request.CreateResponse("Relatorios.aspx?id=" + id);
            

            eDetalhamento detalhe = new eDetalhamento();

            detalhe.AteDia = 29;
            detalhe.AteMes = 02;
            detalhe.AteAno = 2016;
            detalhe.FlDiasUteis = 1;
            detalhe.Categoria = 3;
            detalhe.TipoVenda = 2;
            detalhe.Segmento = "*";
            detalhe.RegiaoOperacional = -1;
            detalhe.RegiaoGeografico = -1;
            detalhe.Estado = -1;
            detalhe.RegiaoMetropolitana = -1;
            detalhe.Cidade = -1;
            detalhe.AreaOperacional = -1;
            detalhe.Concessionaria = 36580;
            detalhe.ByGroup = 0;

            return Detalhamento_1HtmlTabela(detalhe) + Detalhamanto_1HtmlTitulo(detalhe); 
        }

        public void InstanciarObjeto(string Sistema)
        {
            eConfig = new eConfiguracoes();
            nConfig = new Configuracoes();
            eConfig = nConfig.RetornarRegistro(Sistema);
            emplacamento = eConfig.SiteTema;

        }

        public string ReplacesTitulo(string sb)
        {
            //sb = sb.Replace("[IMG]", eConfig.LogoCliente);
            //sb = sb.Replace("[COR]", "#" + eConfig.RelatorioCorTitulo);

            sb = sb.Replace("[DataTitulo]", DataTitulo);
            sb = sb.Replace("[Concessionaria]", Concessionaria);
            sb = sb.Replace("[Categoria]", Categoria);
            sb = sb.Replace("[Tipo]", Tipo);
            sb = sb.Replace("[ABRANGENCIA]", Abrangencia);
            sb = sb.Replace("[Area]", Area);
            sb = sb.Replace("[Modalidade]", Modalidade);
            sb = sb.Replace("[Sigla]", Sigla);
            sb = sb.Replace("[AnualOuMensal]", TipoRelatorio);
            sb = sb.Replace("[Abrangencia]", AreaAbrangenciaLocalidade);
            sb = sb.Replace("[ValorAcumulado]", ValorAcumulado);
            sb = sb.Replace("[DiaUtil]", DiaUtil);
            sb = sb.Replace("[GrupoOuNao]", GrupoOuNao);
            //sb = sb.Replace("[MARCA]", eConfig.Marca);

            return sb;
        }

        //Metodo responsavel por unir as informações dos relatórios exemplo titulo e tabela.
        public string Relatorios(string tabela, string titulo)
        {
            retornarRelatorio = new StringBuilder();

            retornarRelatorio.Append(titulo);
            retornarRelatorio.Append(tabela);

            return retornarRelatorio.ToString();
        }

        #region DETALHAMENTO

        List<eDetalhamento> listaDetalhamentoDias = new List<eDetalhamento>();
        List<eDetalhamento> listaDetalhamento = new List<eDetalhamento>();

        public string Detalhamanto_1HtmlTitulo(eDetalhamento detalhe)
        {
            gerarHtml = new StringBuilder();

            gerarHtml.Append("<div align='center' style='width: 1194px; display:inline-block;'>");
            gerarHtml.Append("<div align='left'style='width: 202px; display:inline; float:left; height: 150px;'>");
            gerarHtml.Append("<img src='[IMG]' alt='Smiley face' height='100' width='200' style='float:right;' /></div>");
            gerarHtml.Append("<div style='width: 807px; display:inline; float:inherit'>");
            gerarHtml.Append("<h3 align='center' style='width:800px; color:[COR]'; font-family:Tahoma; font-size:14pt; font-weight:bold;'>");
            gerarHtml.Append("ACOMPANHAMENTO EMPLACAMENTO - DETALHAMENTO [MARCA]<br/>" + detalhe.AteDia + " DE " + NomeMes(detalhe.AteMes));
            gerarHtml.Append(" - " + detalhe.AteAno);
            if (detalhe.ByGroup == 1)
                gerarHtml.Append("<br/>" + listaDetalhamento[0].Grupo);
            else
                gerarHtml.Append("<br/>" + listaDetalhamento[0].Grupo + " - " + listaDetalhamento[0].Nome);

            if (detalhe.Categoria == 1)
                gerarHtml.Append("<br/>AUTOMOVÉIS");
            else if (detalhe.Categoria == 2)
                gerarHtml.Append("<br/>COMERCIAIS LEVES");
            else
                gerarHtml.Append("<br/>AUTOMOVÉIS + COMERCIAIS LEVES");

            gerarHtml.Append("</h3></div></div>");

            return ReplacesTitulo(gerarHtml.ToString());
        }

        public string Detalhamento_1HtmlTabela(eDetalhamento detalhe)
        {
            gerarHtml = new StringBuilder();

            gerarHtml.Append("<table [EstiloTabelaBase] id='display'><tr style='background-color:[CorCabecalho];'>");
            gerarHtml.Append("<td width='170' [B]></td><td width='200' [B] [AL]>DIA DO MÊS</td>");

            DetalhamentoGerarCedulas(gerarHtml, false, true, true, detalhe);

            gerarHtml.Append("<tr style='background-color:[CorCabecalho];'><td [B] [AL]>ÁREA DE INFLUÊNCIA</td>");
            gerarHtml.Append("<td [B] [AL]>DIAS ÚTIL</td>");

            DetalhamentoGerarCedulas(gerarHtml, false, false, false, detalhe);

            gerarHtml.Append("<tr style='background-color:[CorCabecalho];'><td [B]></td><td [B] [AL]>MUNICÍPIOS</td>");

            DetalhamentoGerarCedulas(gerarHtml, true, false, false, detalhe);

            DetalhamentoSeparaDados(gerarHtml, detalhe);

            return DetalhamantoReplaces(gerarHtml);
        }

        private void DetalhamentoSeparaDados(StringBuilder gerarHtml, eDetalhamento detalhe)
        {
            listaDetalhamento = GetUserRegistroDetalhamento(detalhe);

            List<eDetalhamento> lista = new List<eDetalhamento>();
            List<eDetalhamento> listaAux = new List<eDetalhamento>(); //Lista seraá responsavel por amarzenar os ids que já foram incluidos
            int[] TotalGeral = new int[6];
            for (int i = 0; i < listaDetalhamento.Count; i++)
            {
                if (listaDetalhamento[i].AreaConc != "-1" && !listaAux.Exists(c => c.IdAreaOperacional == listaDetalhamento[i].IdAreaOperacional))
                {
                    lista = listaDetalhamento.Where(c => c.AreaConc != "-1" && c.IdAreaOperacional == listaDetalhamento[i].IdAreaOperacional).ToList();
                    DetalhamentoDadosTabela(gerarHtml, detalhe, lista, 0);
                    listaAux.Add(new eDetalhamento { IdAreaOperacional = listaDetalhamento[i].IdAreaOperacional });

                    TotalGeral[0] += int.Parse(lista[0].TotalVol1);
                    TotalGeral[1] += int.Parse(lista[0].TotalVol2);
                    TotalGeral[2] += int.Parse(lista[0].TotalVol3);
                    TotalGeral[3] += int.Parse(lista[0].TotalVol4);
                    TotalGeral[4] += int.Parse(lista[0].TotalVol5);
                    TotalGeral[5] += int.Parse(lista[0].TotalAcumulado);
                }
            }
            DetalhamentoTotalGeral(gerarHtml, TotalGeral);
            gerarHtml.Append("</table>");
            lista.Clear();
            TotalGeral = new int[6];
            //Criando Cebeçalho da area de influência
            DetalhamentoHtmlForaAreaInfluencia_1(detalhe, gerarHtml);
            for (int i = 0; i < listaDetalhamento.Count; i++)
            {
                if (listaDetalhamento[i].AreaConc == "-1" && !listaAux.Exists(c => c.IdAreaOperacional == listaDetalhamento[i].IdAreaOperacional))
                {
                    lista = listaDetalhamento.Where(c => c.AreaConc == "-1" && c.IdAreaOperacional == listaDetalhamento[i].IdAreaOperacional).ToList();
                    DetalhamentoDadosTabela(gerarHtml, detalhe, lista, 1);
                    listaAux.Add(new eDetalhamento { IdAreaOperacional = listaDetalhamento[i].IdAreaOperacional });

                    TotalGeral[0] += int.Parse(lista[0].TotalVol1);
                    TotalGeral[1] += int.Parse(lista[0].TotalVol2);
                    TotalGeral[2] += int.Parse(lista[0].TotalVol3);
                    TotalGeral[3] += int.Parse(lista[0].TotalVol4);
                    TotalGeral[4] += int.Parse(lista[0].TotalVol5);
                    TotalGeral[5] += int.Parse(lista[0].TotalAcumulado);
                }

            }
            DetalhamentoTotalGeral(gerarHtml, TotalGeral);
        }

        private void DetalhamentoDadosTabela(StringBuilder gerarHtml, eDetalhamento detalhe, List<eDetalhamento> lista, int tabela)
        {

            #region Conteudo
            gerarHtml.Append("<tr style='color:#000; font-weight:normal;'><td [B] [AL]><a href='#' [LINK]>" + lista[0].NomeAreaOperacional + "</a></td>");
            //Espaço em branco
            for (int i = 0; i < 12; i++)
            {
                gerarHtml.Append("<td></td>");
            }
            gerarHtml.Append("<td></td></tr>");

            bool Zebrado = true;
            for (int i = 0; i < lista.Count; i++)
            {
                if (i.Equals(lista.Count - 1))
                {
                    if (Zebrado)
                    {
                        gerarHtml.Append("<tr style='background-color:#dcdcdc; color:#000; font-weight:normal;'><td [BB]></td>");
                        gerarHtml.Append("<td [BB] [AL]><a href='Relatorios.aspx?DetalheVeiculoChassi' [LINK]>" + lista[i].NomeCidade + "</a></td>");
                        gerarHtml.Append("<td [BB]><b>" + lista[i].Vol1 + "</b></td><td [BB]>" + lista[i].PorcentVol1 + "%</td>");
                        gerarHtml.Append("<td [BB]><b>" + lista[i].Vol2 + "</b></td><td [BB]>" + lista[i].PorcentVol2 + "%</td>");
                        gerarHtml.Append("<td [BB]><b>" + lista[i].Vol3 + "</b></td><td [BB]>" + lista[i].PorcentVol3 + "%</td>");
                        gerarHtml.Append("<td [BB]><b>" + lista[i].Vol4 + "</b></td><td [BB]>" + lista[i].PorcentVol4 + "%</td>");
                        gerarHtml.Append("<td [BB]><b>" + lista[i].Vol5 + "</b></td><td [BB]>" + lista[i].PorcentVol5 + "%</td>");
                        gerarHtml.Append("<td [BB]><b>" + lista[i].Acumulado + "</b></td><td [BB]>" + lista[i].PorcentAcumulado + "%</td></tr>");
                        Zebrado = false;
                    }
                    else
                    {
                        gerarHtml.Append("<tr style='color:#000; font-weight:normal;'><td [BB]></td>");
                        gerarHtml.Append("<td [BB] [AL]><a href='Relatorios.aspx?DetalheVeiculoChassi' [LINK]>" + lista[i].NomeCidade + "</a></td>");
                        gerarHtml.Append("<td [BB]><b>" + lista[i].Vol1 + "</b></td><td [BB]>" + lista[i].PorcentVol1 + "%</td>");
                        gerarHtml.Append("<td [BB]><b>" + lista[i].Vol2 + "</b></td><td [BB]>" + lista[i].PorcentVol2 + "%</td>");
                        gerarHtml.Append("<td [BB]><b>" + lista[i].Vol3 + "</b></td><td [BB]>" + lista[i].PorcentVol3 + "%</td>");
                        gerarHtml.Append("<td [BB]><b>" + lista[i].Vol4 + "</b></td><td [BB]>" + lista[i].PorcentVol4 + "%</td>");
                        gerarHtml.Append("<td [BB]><b>" + lista[i].Vol5 + "</b></td><td [BB]>" + lista[i].PorcentVol5 + "%</td>");
                        gerarHtml.Append("<td [BB]><b>" + lista[i].Acumulado + "</b></td><td [BB]>" + lista[i].PorcentAcumulado + "%</td></tr>");
                        Zebrado = true;
                    }
                }
                else
                {

                    if (Zebrado)
                    {
                        gerarHtml.Append("<tr style='background-color:#dcdcdc; color:#000; font-weight:normal;'><td [B]></td>");
                        gerarHtml.Append("<td [B] [AL]><a href='Relatorios.aspx?DetalheVeiculoChassi' [LINK]>" + lista[i].NomeCidade + "</a></td>");
                        gerarHtml.Append("<td [B]><b>" + lista[i].Vol1 + "</b></td><td [B]>" + lista[i].PorcentVol1 + "%</td>");
                        gerarHtml.Append("<td [B]><b>" + lista[i].Vol2 + "</b></td><td [B]>" + lista[i].PorcentVol2 + "%</td>");
                        gerarHtml.Append("<td [B]><b>" + lista[i].Vol3 + "</b></td><td [B]>" + lista[i].PorcentVol3 + "%</td>");
                        gerarHtml.Append("<td [B]><b>" + lista[i].Vol4 + "</b></td><td [B]>" + lista[i].PorcentVol4 + "%</td>");
                        gerarHtml.Append("<td [B]><b>" + lista[i].Vol5 + "</b></td><td [B]>" + lista[i].PorcentVol5 + "%</td>");
                        gerarHtml.Append("<td [B]><b>" + lista[i].Acumulado + "</b></td><td [B]>" + lista[i].PorcentAcumulado + "%</td></tr>");
                        Zebrado = false;
                    }
                    else
                    {
                        gerarHtml.Append("<tr style='color:#000; font-weight:normal;'><td [B]></td>");
                        gerarHtml.Append("<td [B] [AL]><a href='Relatorios.aspx?DetalheVeiculoChassi' [LINK]>" + lista[i].NomeCidade + "</a></td>");
                        gerarHtml.Append("<td [B]><b>" + lista[i].Vol1 + "</b></td><td [B]>" + lista[i].PorcentVol1 + "%</td>");
                        gerarHtml.Append("<td [B]><b>" + lista[i].Vol2 + "</b></td><td [B]>" + lista[i].PorcentVol2 + "%</td>");
                        gerarHtml.Append("<td [B]><b>" + lista[i].Vol3 + "</b></td><td [B]>" + lista[i].PorcentVol3 + "%</td>");
                        gerarHtml.Append("<td [B]><b>" + lista[i].Vol4 + "</b></td><td [B]>" + lista[i].PorcentVol4 + "%</td>");
                        gerarHtml.Append("<td [B]><b>" + lista[i].Vol5 + "</b></td><td [B]>" + lista[i].PorcentVol5 + "%</td>");
                        gerarHtml.Append("<td [B]><b>" + lista[i].Acumulado + "</b></td><td [B]>" + lista[i].PorcentAcumulado + "%</td></tr>");
                        Zebrado = true;
                    }
                }
            }

            #endregion

            #region Totais
            gerarHtml.Append("<tr style='color:#000;'><td colspan='2' [AL] [BU]>TOTAL " + lista[0].NomeAreaOperacional + "</td>");
            gerarHtml.Append("<td [BU]><b>" + lista[0].TotalVol1 + "</b></td><td [BU]>100%</td>");
            gerarHtml.Append("<td [BU]><b>" + lista[0].TotalVol2 + "</b></td><td [BU]>100%</td>");
            gerarHtml.Append("<td [BU]><b>" + lista[0].TotalVol3 + "</b></td><td [BU]>100%</td>");
            gerarHtml.Append("<td [BU]><b>" + lista[0].TotalVol4 + "</b></td><td [BU]>100%</td>");
            gerarHtml.Append("<td [BU]><b>" + lista[0].TotalVol5 + "</b></td><td [BU]>100%</td>");
            gerarHtml.Append("<td [BU]><b>" + lista[0].TotalAcumulado + "</b></td><td [BU]>100%</td>");

            #endregion
        }

        private void DetalhamentoHtmlForaAreaInfluencia_1(eDetalhamento detalhe, StringBuilder gerarHtml)
        {
            gerarHtml.Append("<br/><br/><table [EstiloTabelaBase]><tr style='background-color:[CorCabecalho];'>");
            gerarHtml.Append("<td width='170' [B]></td><td width='200' [B] [AL]>DIA DO MÊS</td>");

            DetalhamentoGerarCedulas(gerarHtml, false, true, true, detalhe);

            gerarHtml.Append("<tr style='background-color:[CorCabecalho];'><td [B] [AL]>FORA DE ÁREA</td>");
            gerarHtml.Append("<td [B] [AL]>DIAS ÚTIL</td>");

            DetalhamentoGerarCedulas(gerarHtml, false, false, false, detalhe);

            gerarHtml.Append("<tr style='background-color:[CorCabecalho];'><td [B]></td><td [B] [AL]>MUNICÍPIOS</td>");

            DetalhamentoGerarCedulas(gerarHtml, true, false, false, detalhe);
        }

        private void DetalhamentoTotalGeral(StringBuilder gerarHtml, int[] TotalGeral)
        {
            gerarHtml.Append("<tr style='color:#000;'><td colspan='2' [AL]>TOTAL GERAL</td>");
            gerarHtml.Append("<td><b>" + TotalGeral[0] + "</b></td><td>100%</td>");
            gerarHtml.Append("<td><b>" + TotalGeral[1] + "</b></td><td>100%</td>");
            gerarHtml.Append("<td><b>" + TotalGeral[2] + "</b></td><td>100%</td>");
            gerarHtml.Append("<td><b>" + TotalGeral[3] + "</b></td><td>100%</td>");
            gerarHtml.Append("<td><b>" + TotalGeral[4] + "</b></td><td>100%</td>");
            gerarHtml.Append("<td><b>" + TotalGeral[5] + "</b></td><td>100%</td></tr>");
        }

        private void DetalhamentoGerarCedulas(StringBuilder gerarHtml, bool eVol, bool DiaMes, bool primeira, eDetalhamento detalhe)
        {
            if (eVol)
            {
                for (int i = 0; i < (listaDetalhamentoDias.Count + 1); i++)
                {
                    if (i.Equals(listaDetalhamentoDias.Count))
                    {
                        gerarHtml.Append("<td [B]>Vol</td><td [B]>%</td></tr>");
                    }
                    else
                    {
                        gerarHtml.Append("<td [B]>Vol</td><td [B]>%</td>");
                    }
                }
            }
            else
            {
                if (primeira)
                {
                    listaDetalhamentoDias = GetUserRegistroDetalhamentoDias(detalhe);
                }

                if (DiaMes)
                {
                    for (int i = 0; i < listaDetalhamentoDias.Count; i++)
                    {
                        if (listaDetalhamentoDias[i].DiasValidos.Length.Equals(4))
                        {
                            listaDetalhamentoDias[i].DiasValidos = listaDetalhamentoDias[i].DiasValidos.Replace("/", "/0");
                        }
                        gerarHtml.Append("<td width='130' [B] colspan='2'>" + listaDetalhamentoDias[i].DiasValidos + "</td>");
                    }
                    gerarHtml.Append("<td width='140' [B] colspan='2'></td></tr>");
                }
                else
                {
                    gerarHtml.Append("<td [B] colspan='2'>" + listaDetalhamentoDias[0].Dia1 + "</td><td [B] colspan='2'>" + listaDetalhamentoDias[0].Dia2 + "</td>");
                    gerarHtml.Append("<td [B] colspan='2'>" + listaDetalhamentoDias[0].Dia3 + "</td><td [B] colspan='2'>" + listaDetalhamentoDias[0].Dia4 + "</td>");
                    gerarHtml.Append("<td [B] colspan='2'>" + listaDetalhamentoDias[0].Dia5 + "</td><td [B] colspan='2'>ACUMULADO DO MÊS</td></tr>");
                }
            }
        }

        private string DetalhamantoReplaces(StringBuilder gerarHtml)
        {
            gerarHtml = gerarHtml.Replace("[EstiloTabelaBase]", EstiloTabelaBase);
            gerarHtml = gerarHtml.Replace("[SIZE]", "10");
            gerarHtml = gerarHtml.Replace("[NCelPadding]", "3");
            gerarHtml = gerarHtml.Replace("[CorBordaLinha]", "#000");
            gerarHtml = gerarHtml.Replace("[CorCabecalho]", "#DDD" );// eConfig.RelatorioCorCabecalho);
            gerarHtml = gerarHtml.Replace("[B]", "style='border: 1px solid #D3D3D3;'");
            gerarHtml = gerarHtml.Replace("[A]", "border: 1px solid #000;");
            gerarHtml = gerarHtml.Replace("[AL]", "align='left'");
            gerarHtml = gerarHtml.Replace("[CA]", "style='border: 1px solid #000; background-color:#D3D3D3; color:#000;'");
            gerarHtml = gerarHtml.Replace("[CB]", "style='background-color:#DED9C5; color:#000; border: 1px solid #000;'");
            gerarHtml = gerarHtml.Replace("[LINK]", "style='text-decoration: none; color:#000;'");
            gerarHtml = gerarHtml.Replace("[BB]", "style='border-top: 1px solid #D3D3D3; border-left: 1px solid #D3D3D3; border-right: 1px solid #D3D3D3; border-bottom: 3px solid #808080;'");
            gerarHtml = gerarHtml.Replace("[BU]", "style='border-bottom: 3px solid #808080;'");

            return gerarHtml.ToString();
        }

        private List<eDetalhamento> GetUserRegistroDetalhamento(eDetalhamento detalhe)
        {
            try
            {
                dRelatorios db = new dRelatorios();
                return db.GetUserRegistroDetalhamento(detalhe);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private List<eDetalhamento> GetUserRegistroDetalhamentoDias(eDetalhamento detalhe)
        {
            try
            {
                dRelatorios db = new dRelatorios();
                return db.GetUserRegistroDetalhamentoDias(detalhe);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion

        #region DetalhesVeiculoChassi

        public string DetalhesVeiculoChassiHtmlTitulo(eDetalheVeiculoChassi detaV)
        {
            gerarHtml = new StringBuilder();

            gerarHtml.Append("<div align='center' style='width: 1194px; display:inline-block;'>");
            gerarHtml.Append("<div align='left'style='width: 202px; display:inline; float:left; height: 150px;'>");
            gerarHtml.Append("<img src='[IMG]' alt='Smiley face' height='100' width='200' style='float:right;' /></div>");
            gerarHtml.Append("<div style='width: 807px; display:inline; float:inherit'>");
            gerarHtml.Append("<h3 align='center' style='width:800px; color:[COR]'; font-family:Tahoma; font-size:14pt; font-weight:bold;'>");
            gerarHtml.Append("DETALHAMENTO DE VEÍCULOS<br/>[NOME FANTASIA]<br/> MUNUCÍPIO: [NOME_MUNICIPIO]</h3></div></br>");

            return ReplacesTitulo(gerarHtml.ToString());
        }

        public string DetalhesVeiculoChassiHtmlTabela(eDetalheVeiculoChassi detaV)
        {
            gerarHtml = new StringBuilder();

            List<eDetalheVeiculoChassi> lista = GetUserRegistroDetalhesVeiculoChassi(detaV);

            gerarHtml.Append("<table border='1' [EstiloTabelaBase] id='display'><tr style='background-color:[CorCabecalho];'>");
            gerarHtml.Append("<td width='95' [AL]>DATA</td><td width='155' [AL]>MODELO</td><td width='230' [AL]>VERSÃO</td>");
            gerarHtml.Append("<td width='185' [AL]>CHASSI</td><td width='80' [AL]>PLACA</td></tr>");

            DetalhesVeiculoChassiDadosTabela(gerarHtml, lista);

            return SegmentoFuAnualReplace(gerarHtml);
        }

        private void DetalhesVeiculoChassiDadosTabela(StringBuilder gerarHtml, List<eDetalheVeiculoChassi> lista)
        {
            bool Zebrado = true;
            for (int i = 0; i < lista.Count; i++)
            {
                if (Zebrado)
                {
                    gerarHtml.Append("<tr style='color:#000; font-weight:normal; background-color:#D3D3D3;'>");
                    gerarHtml.Append("<td>" + lista[i].Data + "</td><td>" + lista[i].NomeVeiculo + "</td><td>" + lista[i].Modelo + "</td>");
                    gerarHtml.Append("<td>" + lista[i].Chassis + "</td><td>" + lista[i].Placa + "</td></tr>");
                    Zebrado = false;
                }
                else
                {
                    gerarHtml.Append("<tr style='color:#000; font-weight:normal;'>");
                    gerarHtml.Append("<td>" + lista[i].Data + "</td><td>" + lista[i].NomeVeiculo + "</td><td>" + lista[i].Modelo + "</td>");
                    gerarHtml.Append("<td>" + lista[i].Chassis + "</td><td>" + lista[i].Placa + "</td></tr>");
                    Zebrado = true;
                }
            }
            gerarHtml.Append("</table></div>");
        }

        private List<eDetalheVeiculoChassi> GetUserRegistroDetalhesVeiculoChassi(eDetalheVeiculoChassi detaV)
        {
            try
            {
                dRelatorios db = new dRelatorios();
                return db.GetUserRegistroDetalhesVeiculoChassi(detaV);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion

        #region DetalhamentoAnual

        List<eDetalhamentoAnual> listaDetalhamentoAnual = new List<eDetalhamentoAnual>();

        public string DetalhamentoAnualHtmlTitulo(eDetalhamentoAnual detaAnu)
        {
            gerarHtml = new StringBuilder();

            return gerarHtml.ToString();
        }

        public string DetalhamentoAnualHtmlTabela(eDetalhamentoAnual detaAnu)
        {
            gerarHtml = new StringBuilder();

            listaDetalhamentoAnual = GetUserRelatorioDetalhamentoAnual(detaAnu);

            gerarHtml.Append("<table [EstiloTabelaBase] width='2000' id='display'><tr style='background-color:[CorCabecalho];'>");
            gerarHtml.Append("<td [AL] [B]>ÁREA DE INFLUENCIA</td><td [AL]>MÊS</td>");
            for (int i = 1; i <= 12; i++)
            {
                gerarHtml.Append("<td colspan='2' width='110px' [B]>" + NomeMes(i) + "</td>");
            }
            gerarHtml.Append("<td colspan='2' [B]>" + detaAnu.Ano + "</td></tr>");
            gerarHtml.Append("<tr style='background-color:[CorCabecalho];'><td width='170px' [B]></td><td width='200px' [B]>MUNUCÍPIOS</td>");
            for (int i = 1; i <= 12; i++)
            {
                gerarHtml.Append("<td [B]>Vol</td><td  [B]>%</td>");
            }
            gerarHtml.Append("<td [B]>Vol</td><td [B]>%</td></tr>");

            DetalhamentoAnualSeparaDados(gerarHtml, detaAnu);

            return DetalhamantoReplaces(gerarHtml);
        }

        private void DetalhamentoAnualSeparaDados(StringBuilder gerarHtml, eDetalhamentoAnual detaAnu)
        {

            List<eDetalhamentoAnual> lista = new List<eDetalhamentoAnual>();
            List<eDetalhamentoAnual> listaAux = new List<eDetalhamentoAnual>(); //Lista seraá responsavel por amarzenar os ids que já foram incluidos
            int[] TotalGeral = new int[13];
            for (int i = 0; i < listaDetalhamentoAnual.Count; i++)
            {
                if (listaDetalhamentoAnual[i].AreaConc != "-1" && !listaAux.Exists(c => c.IdAreaOperacional == listaDetalhamentoAnual[i].IdAreaOperacional))
                {
                    lista = listaDetalhamentoAnual.Where(c => c.AreaConc != "-1" && c.IdAreaOperacional == listaDetalhamentoAnual[i].IdAreaOperacional).ToList();
                    DetalhamentoAnualDadosTabela(gerarHtml, detaAnu, lista, 0);
                    listaAux.Add(new eDetalhamentoAnual { IdAreaOperacional = listaDetalhamentoAnual[i].IdAreaOperacional });

                    #region SomandoTotaisDaAreaDeInfluencia
                    TotalGeral[0] += int.Parse(lista[0].Total_Janeiro);
                    TotalGeral[1] += int.Parse(lista[0].Total_Fevereiro);
                    TotalGeral[2] += int.Parse(lista[0].Total_Marco);
                    TotalGeral[3] += int.Parse(lista[0].Total_Abril);
                    TotalGeral[4] += int.Parse(lista[0].Total_Maio);
                    TotalGeral[5] += int.Parse(lista[0].Total_Junho);
                    TotalGeral[6] += int.Parse(lista[0].Total_Julho);
                    TotalGeral[7] += int.Parse(lista[0].Total_Agosto);
                    TotalGeral[8] += int.Parse(lista[0].Total_Setembro);
                    TotalGeral[9] += int.Parse(lista[0].Total_Outubro);
                    TotalGeral[10] += int.Parse(lista[0].Total_Novembro);
                    TotalGeral[11] += int.Parse(lista[0].Total_Dezembro);
                    TotalGeral[12] += int.Parse(lista[0].Total_Ano_Atual);
                    #endregion
                }
            }
            DetalhamentoAnualTotalGeral(gerarHtml, TotalGeral);
            gerarHtml.Append("</table>");
            lista.Clear();
            TotalGeral = new int[13];
            //Criando Cebeçalho da area de influência
            DetalhamentoAnualHtmlForaAreaInfluencia_1(detaAnu, gerarHtml);
            for (int i = 0; i < listaDetalhamentoAnual.Count; i++)
            {
                if (listaDetalhamentoAnual[i].AreaConc == "-1" && !listaAux.Exists(c => c.IdAreaOperacional == listaDetalhamentoAnual[i].IdAreaOperacional))
                {
                    lista = listaDetalhamentoAnual.Where(c => c.AreaConc == "-1" && c.IdAreaOperacional == listaDetalhamentoAnual[i].IdAreaOperacional).ToList();
                    DetalhamentoAnualDadosTabela(gerarHtml, detaAnu, lista, 1);
                    listaAux.Add(new eDetalhamentoAnual { IdAreaOperacional = listaDetalhamentoAnual[i].IdAreaOperacional });

                    #region SomandoTotaisForaDaAreaDeInfluencia 
                    TotalGeral[0] += int.Parse(lista[0].Total_Janeiro);
                    TotalGeral[1] += int.Parse(lista[0].Total_Fevereiro);
                    TotalGeral[2] += int.Parse(lista[0].Total_Marco);
                    TotalGeral[3] += int.Parse(lista[0].Total_Abril);
                    TotalGeral[4] += int.Parse(lista[0].Total_Maio);
                    TotalGeral[5] += int.Parse(lista[0].Total_Junho);
                    TotalGeral[6] += int.Parse(lista[0].Total_Julho);
                    TotalGeral[7] += int.Parse(lista[0].Total_Agosto);
                    TotalGeral[8] += int.Parse(lista[0].Total_Setembro);
                    TotalGeral[9] += int.Parse(lista[0].Total_Outubro);
                    TotalGeral[10] += int.Parse(lista[0].Total_Novembro);
                    TotalGeral[11] += int.Parse(lista[0].Total_Dezembro);
                    TotalGeral[12] += int.Parse(lista[0].Total_Ano_Atual);
                    #endregion
                }

            }
            DetalhamentoAnualTotalGeral(gerarHtml, TotalGeral);

        }

        private void DetalhamentoAnualHtmlForaAreaInfluencia_1(eDetalhamentoAnual detaAnu, StringBuilder gerarHtml)
        {
            gerarHtml.Append("<br/><br/><table [EstiloTabelaBase] width='2000' id='display'><tr style='background-color:[CorCabecalho];'>");
            gerarHtml.Append("<td width='170' [AL] [B]>FORA DE ÁREA</td><td width='200' [AL] [B]>MÊS</td>");
            for (int i = 1; i <= 12; i++)
            {
                gerarHtml.Append("<td colspan='2' [B]>" + NomeMes(i) + "</td>");
            }
            gerarHtml.Append("<td colspan='2' [B]>" + detaAnu.Ano + "</td></tr>");
            gerarHtml.Append("<tr style='background-color:[CorCabecalho];'><td [B]></td><td [B]>MUNUCÍPIOS</td>");
            for (int i = 1; i <= 12; i++)
            {
                gerarHtml.Append("<td width='55' [B]>Vol</td><td width='55' [B]>%</td>");
            }
            gerarHtml.Append("<td width='55' [B]>Vol</td><td width='55' [B]>%</td></tr>");
        }

        private void DetalhamentoAnualTotalGeral(StringBuilder gerarHtml, int[] TotalGeral)
        {
            gerarHtml.Append("<tr style='color:#000;'><td colspan='2' [AL]>TOTAL GERAL</td>");
            gerarHtml.Append("<td><b>" + TotalGeral[0] + "</b></td><td>100%</td>");
            gerarHtml.Append("<td><b>" + TotalGeral[1] + "</b></td><td>100%</td>");
            gerarHtml.Append("<td><b>" + TotalGeral[2] + "</b></td><td>100%</td>");
            gerarHtml.Append("<td><b>" + TotalGeral[3] + "</b></td><td>100%</td>");
            gerarHtml.Append("<td><b>" + TotalGeral[4] + "</b></td><td>100%</td>");
            gerarHtml.Append("<td><b>" + TotalGeral[5] + "</b></td><td>100%</td>");
            gerarHtml.Append("<td><b>" + TotalGeral[6] + "</b></td><td>100%</td>");
            gerarHtml.Append("<td><b>" + TotalGeral[7] + "</b></td><td>100%</td>");
            gerarHtml.Append("<td><b>" + TotalGeral[8] + "</b></td><td>100%</td>");
            gerarHtml.Append("<td><b>" + TotalGeral[9] + "</b></td><td>100%</td>");
            gerarHtml.Append("<td><b>" + TotalGeral[10] + "</b></td><td>100%</td>");
            gerarHtml.Append("<td><b>" + TotalGeral[11] + "</b></td><td>100%</td>");
            gerarHtml.Append("<td><b>" + TotalGeral[12] + "</b></td><td>100%</td></tr>");
        }

        private void DetalhamentoAnualDadosTabela(StringBuilder gerarHtml, eDetalhamentoAnual detaAnu, List<eDetalhamentoAnual> lista, int p)
        {
            #region Conteudo
            gerarHtml.Append("<tr style='color:#000; font-weight:normal;'><td [B] [AL]><a href='#' [LINK]>" + lista[0].NomeAreaOperacional + "</a></td>");
            //Espaço em branco
            gerarHtml.Append("<td></td>");
            for (int i = 0; i < 12; i++)
            {
                gerarHtml.Append("<td></td><td></td>");
            }
            gerarHtml.Append("<td></td></tr>");

            bool Zebrado = true;
            for (int i = 0; i < lista.Count; i++)
            {
                if (i.Equals(lista.Count - 1))
                {
                    if (Zebrado)
                    {
                        gerarHtml.Append("<tr style='background-color:#dcdcdc; color:#000; font-weight:normal;'><td [BB]></td>");
                        gerarHtml.Append("<td [BB] [AL]><a href='Relatorios.aspx?DetalheVeiculoChassi' [LINK]>" + lista[i].NomeCidade + "</a></td>");
                        gerarHtml.Append("<td [BB]><b>" + lista[i].Vol_Janeiro + "</b></td><td [BB]>" + lista[i].Porcent_Janeiro + "%</td>");
                        gerarHtml.Append("<td [BB]><b>" + lista[i].Vol_Fevereiro + "</b></td><td [BB]>" + lista[i].Porcent_Fevereiro + "%</td>");
                        gerarHtml.Append("<td [BB]><b>" + lista[i].Vol_Marco + "</b></td><td [BB]>" + lista[i].Porcent_Marco + "%</td>");
                        gerarHtml.Append("<td [BB]><b>" + lista[i].Vol_Abril + "</b></td><td [BB]>" + lista[i].Porcent_Abril + "%</td>");
                        gerarHtml.Append("<td [BB]><b>" + lista[i].Vol_Maio + "</b></td><td [BB]>" + lista[i].Porcent_Maio + "%</td>");
                        gerarHtml.Append("<td [BB]><b>" + lista[i].Vol_Junho + "</b></td><td [BB]>" + lista[i].Porcent_Junho + "%</td>");
                        gerarHtml.Append("<td [BB]><b>" + lista[i].Vol_Julho + "</b></td><td [BB]>" + lista[i].Porcent_Julho + "%</td>");
                        gerarHtml.Append("<td [BB]><b>" + lista[i].Vol_Agosto + "</b></td><td [BB]>" + lista[i].Porcent_Agosto + "%</td>");
                        gerarHtml.Append("<td [BB]><b>" + lista[i].Vol_Setembro + "</b></td><td [BB]>" + lista[i].Porcent_Setembro + "%</td>");
                        gerarHtml.Append("<td [BB]><b>" + lista[i].Vol_Outubro + "</b></td><td [BB]>" + lista[i].Porcent_Outubro + "%</td>");
                        gerarHtml.Append("<td [BB]><b>" + lista[i].Vol_Novembro + "</b></td><td [BB]>" + lista[i].Porcent_Novembro + "%</td>");
                        gerarHtml.Append("<td [BB]><b>" + lista[i].Vol_Dezembro + "</b></td><td [BB]>" + lista[i].Porcent_Dezembro + "%</td>");
                        gerarHtml.Append("<td [BB]><b>" + lista[i].Vol_Ano_Atual + "</b></td><td [BB]>" + lista[i].Porcent_Ano_Atual + "%</td></tr>");
                        Zebrado = false;
                    }
                    else
                    {
                        gerarHtml.Append("<tr style='color:#000; font-weight:normal;'><td [BB]></td>");
                        gerarHtml.Append("<td [BB] [AL]><a href='Relatorios.aspx?DetalheVeiculoChassi' [LINK]>" + lista[i].NomeCidade + "</a></td>");
                        gerarHtml.Append("<td [BB]><b>" + lista[i].Vol_Janeiro + "</b></td><td [BB]>" + lista[i].Porcent_Janeiro + "%</td>");
                        gerarHtml.Append("<td [BB]><b>" + lista[i].Vol_Fevereiro + "</b></td><td [BB]>" + lista[i].Porcent_Fevereiro + "%</td>");
                        gerarHtml.Append("<td [BB]><b>" + lista[i].Vol_Marco + "</b></td><td [BB]>" + lista[i].Porcent_Marco + "%</td>");
                        gerarHtml.Append("<td [BB]><b>" + lista[i].Vol_Abril + "</b></td><td [BB]>" + lista[i].Porcent_Abril + "%</td>");
                        gerarHtml.Append("<td [BB]><b>" + lista[i].Vol_Maio + "</b></td><td [BB]>" + lista[i].Porcent_Maio + "%</td>");
                        gerarHtml.Append("<td [BB]><b>" + lista[i].Vol_Junho + "</b></td><td [BB]>" + lista[i].Porcent_Junho + "%</td>");
                        gerarHtml.Append("<td [BB]><b>" + lista[i].Vol_Julho + "</b></td><td [BB]>" + lista[i].Porcent_Julho + "%</td>");
                        gerarHtml.Append("<td [BB]><b>" + lista[i].Vol_Agosto + "</b></td><td [BB]>" + lista[i].Porcent_Agosto + "%</td>");
                        gerarHtml.Append("<td [BB]><b>" + lista[i].Vol_Setembro + "</b></td><td [BB]>" + lista[i].Porcent_Setembro + "%</td>");
                        gerarHtml.Append("<td [BB]><b>" + lista[i].Vol_Outubro + "</b></td><td [BB]>" + lista[i].Porcent_Outubro + "%</td>");
                        gerarHtml.Append("<td [BB]><b>" + lista[i].Vol_Novembro + "</b></td><td [BB]>" + lista[i].Porcent_Novembro + "%</td>");
                        gerarHtml.Append("<td [BB]><b>" + lista[i].Vol_Dezembro + "</b></td><td [BB]>" + lista[i].Porcent_Dezembro + "%</td>");
                        gerarHtml.Append("<td [BB]><b>" + lista[i].Vol_Ano_Atual + "</b></td><td [BB]>" + lista[i].Porcent_Ano_Atual + "%</td></tr>");
                        Zebrado = true;
                    }
                }
                else
                {
                    if (Zebrado)
                    {
                        gerarHtml.Append("<tr style='background-color:#dcdcdc; color:#000; font-weight:normal;'><td [B]></td>");
                        gerarHtml.Append("<td [B] [AL]><a href='Relatorios.aspx?DetalheVeiculoChassi' [LINK]>" + lista[i].NomeCidade + "</a></td>");
                        gerarHtml.Append("<td [B]><b>" + lista[i].Vol_Janeiro + "</b></td><td [B]>" + lista[i].Porcent_Janeiro + "%</td>");
                        gerarHtml.Append("<td [B]><b>" + lista[i].Vol_Fevereiro + "</b></td><td [B]>" + lista[i].Porcent_Fevereiro + "%</td>");
                        gerarHtml.Append("<td [B]><b>" + lista[i].Vol_Marco + "</b></td><td [B]>" + lista[i].Porcent_Marco + "%</td>");
                        gerarHtml.Append("<td [B]><b>" + lista[i].Vol_Abril + "</b></td><td [B]>" + lista[i].Porcent_Abril + "%</td>");
                        gerarHtml.Append("<td [B]><b>" + lista[i].Vol_Maio + "</b></td><td [B]>" + lista[i].Porcent_Maio + "%</td>");
                        gerarHtml.Append("<td [B]><b>" + lista[i].Vol_Junho + "</b></td><td [B]>" + lista[i].Porcent_Junho + "%</td>");
                        gerarHtml.Append("<td [B]><b>" + lista[i].Vol_Julho + "</b></td><td [B]>" + lista[i].Porcent_Julho + "%</td>");
                        gerarHtml.Append("<td [B]><b>" + lista[i].Vol_Agosto + "</b></td><td [B]>" + lista[i].Porcent_Agosto + "%</td>");
                        gerarHtml.Append("<td [B]><b>" + lista[i].Vol_Setembro + "</b></td><td [B]>" + lista[i].Porcent_Setembro + "%</td>");
                        gerarHtml.Append("<td [B]><b>" + lista[i].Vol_Outubro + "</b></td><td [B]>" + lista[i].Porcent_Outubro + "%</td>");
                        gerarHtml.Append("<td [B]><b>" + lista[i].Vol_Novembro + "</b></td><td [B]>" + lista[i].Porcent_Novembro + "%</td>");
                        gerarHtml.Append("<td [B]><b>" + lista[i].Vol_Dezembro + "</b></td><td [B]>" + lista[i].Porcent_Dezembro + "%</td>");
                        gerarHtml.Append("<td [B]><b>" + lista[i].Vol_Ano_Atual + "</b></td><td [B]>" + lista[i].Porcent_Ano_Atual + "%</td></tr>");
                        Zebrado = false;
                    }
                    else
                    {
                        gerarHtml.Append("<tr style='color:#000; font-weight:normal;'><td [B]></td>");
                        gerarHtml.Append("<td [B] [AL]><a href='Relatorios.aspx?DetalheVeiculoChassi' [LINK]>" + lista[i].NomeCidade + "</a></td>");
                        gerarHtml.Append("<td [B]><b>" + lista[i].Vol_Janeiro + "</b></td><td [B]>" + lista[i].Porcent_Janeiro + "%</td>");
                        gerarHtml.Append("<td [B]><b>" + lista[i].Vol_Fevereiro + "</b></td><td [B]>" + lista[i].Porcent_Fevereiro + "%</td>");
                        gerarHtml.Append("<td [B]><b>" + lista[i].Vol_Marco + "</b></td><td [B]>" + lista[i].Porcent_Marco + "%</td>");
                        gerarHtml.Append("<td [B]><b>" + lista[i].Vol_Abril + "</b></td><td [B]>" + lista[i].Porcent_Abril + "%</td>");
                        gerarHtml.Append("<td [B]><b>" + lista[i].Vol_Maio + "</b></td><td [B]>" + lista[i].Porcent_Maio + "%</td>");
                        gerarHtml.Append("<td [B]><b>" + lista[i].Vol_Junho + "</b></td><td [B]>" + lista[i].Porcent_Junho + "%</td>");
                        gerarHtml.Append("<td [B]><b>" + lista[i].Vol_Julho + "</b></td><td [B]>" + lista[i].Porcent_Julho + "%</td>");
                        gerarHtml.Append("<td [B]><b>" + lista[i].Vol_Agosto + "</b></td><td [B]>" + lista[i].Porcent_Agosto + "%</td>");
                        gerarHtml.Append("<td [B]><b>" + lista[i].Vol_Setembro + "</b></td><td [B]>" + lista[i].Porcent_Setembro + "%</td>");
                        gerarHtml.Append("<td [B]><b>" + lista[i].Vol_Outubro + "</b></td><td [B]>" + lista[i].Porcent_Outubro + "%</td>");
                        gerarHtml.Append("<td [B]><b>" + lista[i].Vol_Novembro + "</b></td><td [B]>" + lista[i].Porcent_Novembro + "%</td>");
                        gerarHtml.Append("<td [B]><b>" + lista[i].Vol_Dezembro + "</b></td><td [B]>" + lista[i].Porcent_Dezembro + "%</td>");
                        gerarHtml.Append("<td [B]><b>" + lista[i].Vol_Ano_Atual + "</b></td><td [B]>" + lista[i].Porcent_Ano_Atual + "%</td></tr>");
                        Zebrado = true;
                    }
                }
            }


            #endregion

            #region Totais
            gerarHtml.Append("<tr style='color:#000;'><td colspan='2' [AL] [BU]>TOTAL " + lista[0].NomeAreaOperacional + "</td>");
            gerarHtml.Append("<td [BU]><b>" + lista[0].Total_Janeiro + "</b></td><td [BU]>100%</td>");
            gerarHtml.Append("<td [BU]><b>" + lista[0].Total_Fevereiro + "</b></td><td [BU]>100%</td>");
            gerarHtml.Append("<td [BU]><b>" + lista[0].Total_Marco + "</b></td><td [BU]>100%</td>");
            gerarHtml.Append("<td [BU]><b>" + lista[0].Total_Abril + "</b></td><td [BU]>100%</td>");
            gerarHtml.Append("<td [BU]><b>" + lista[0].Total_Maio + "</b></td><td [BU]>100%</td>");
            gerarHtml.Append("<td [BU]><b>" + lista[0].Total_Junho + "</b></td><td [BU]>100%</td>");
            gerarHtml.Append("<td [BU]><b>" + lista[0].Total_Julho + "</b></td><td [BU]>100%</td>");
            gerarHtml.Append("<td [BU]><b>" + lista[0].Total_Agosto + "</b></td><td [BU]>100%</td>");
            gerarHtml.Append("<td [BU]><b>" + lista[0].Total_Setembro + "</b></td><td [BU]>100%</td>");
            gerarHtml.Append("<td [BU]><b>" + lista[0].Total_Outubro + "</b></td><td [BU]>100%</td>");
            gerarHtml.Append("<td [BU]><b>" + lista[0].Total_Novembro + "</b></td><td [BU]>100%</td>");
            gerarHtml.Append("<td [BU]><b>" + lista[0].Total_Dezembro + "</b></td><td [BU]>100%</td>");
            gerarHtml.Append("<td [BU]><b>" + lista[0].Total_Ano_Atual + "</b></td><td [BU]>100%</td></tr>");

            #endregion
        }

        private List<eDetalhamentoAnual> GetUserRelatorioDetalhamentoAnual(eDetalhamentoAnual deteAnual)
        {
            try
            {
                dRelatorios db = new dRelatorios();
                return db.GetUserRelatorioDetalhamentoAnual(deteAnual);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region EmplacamentoCidade

        List<eEmplacamentoCidade> listaEmplacamentoCidade = new List<eEmplacamentoCidade>();

        public string EmplacamentoCidadeHtmlTitulo(eEmplacamentoCidade emplaca)
        {
            gerarHtml = new StringBuilder();

            gerarHtml.Append("<div align='center' style='width: 1194px; display:inline-block;'>");
            gerarHtml.Append("<div align='left'style='width: 202px; display:inline; float:left; height: 150px;'>");
            gerarHtml.Append("<img src='[IMG]' alt='Smiley face' height='100' width='200' style='float:right;' /></div>");
            gerarHtml.Append("<div style='width: 807px; display:inline; float:inherit'>");
            gerarHtml.Append("<h2 align='center' style='width:800px; color:[COR]'; font-family:Tahoma; font-size:14pt; font-weight:bold;'>");
            gerarHtml.Append("ACOMPANHAMENTO EMPLACAMENTO<br/>");
            if (emplaca.Anual)
                gerarHtml.Append("DETALHAMENTO POR MONTADORA - ACUMULADO " + emplaca.AteAno);
            else
                gerarHtml.Append("RELATÓRIO MENSAL ATÉ O " + listaEmplacamentoCidade[0].DiasUtil + "° DIA UTIL - DEZEMBRO " + emplaca.AteAno);

            if (emplaca.ByGroup)
                gerarHtml.Append("<br/>GRUPO: " + listaEmplacamentoCidade[0].NomeEmpresa);
            else
                gerarHtml.Append("<br/>CONCESSIONARIA: " + listaEmplacamentoCidade[0].NomeEmpresa);

            if (emplaca.TipoVenda == 0)
                gerarHtml.Append("<br/>AUTOMÓVEIS");
            else if (emplaca.TipoVenda == 1)
                gerarHtml.Append("<br/>COMERCIAIS LEVES");
            else
                gerarHtml.Append("<br/>AUTOMÓVEIS + COMERCIAIS LEVES");

            gerarHtml.Append("</h2></div></div>");

            return ReplacesTitulo(gerarHtml.ToString());
        }

        public string EmplacamentoCidadeHtmlTabela(eEmplacamentoCidade emplaca)
        {
            gerarHtml = new StringBuilder();
            List<eEmplacamentoCidade> listaMarcas = new List<eEmplacamentoCidade>();

            #region Tabela1 Area

            listaEmplacamentoCidade = GetUserRegistroEmplacamentoCidade(emplaca);

            for (int i = 0; i < listaEmplacamentoCidade.Count; i++)
            {
                //Percorre a lista para verificar se existe a marca, essa lista de marca sera  usada para gerar as colunas 
                if (!listaMarcas.Exists(c => c.NomeclaturaModelos == listaEmplacamentoCidade[i].NomeclaturaModelos))
                    listaMarcas.Add(new eEmplacamentoCidade { NomeclaturaModelos = listaEmplacamentoCidade[i].NomeclaturaModelos, Ordem = listaEmplacamentoCidade[i].Ordem });
            }
            listaMarcas = listaMarcas.OrderBy(c => c.Ordem).ToList();

            gerarHtml.Append("<table border='1' [EstiloTabelaBase] id='display'><tr style='background-color:[CorCabecalho];'>");
            gerarHtml.Append("<td colspan='" + ((listaMarcas.Count * 2) + 4) + "' [BB]>EMPLACAMENTO DENTRO DA ÁREA OPERACIONAL: " + listaEmplacamentoCidade[0].NomeLocalizacaoOperacional + "</td></tr>");
            gerarHtml.Append("<tr style='background-color:[CorCabecalho];'><td [BB] width='200'>MUNICÍPIOS</td>");
            gerarHtml.Append("<td width='115' [Z] colspan='2'>" + listaEmplacamentoCidade[0].NomeEmpresa + "</td>");
            for (int i = 0; i < listaMarcas.Count; i++)
            {
                gerarHtml.Append("<td width='115' [BB] colspan='2'>" + listaMarcas[i].NomeclaturaModelos + "</td>");
            }
            gerarHtml.Append("<td width='55' [BB]>TOTAL</td></tr>");

            gerarHtml.Append("<tr style='background-color:[CorCabecalho];'><td [BB]></td>");
            gerarHtml.Append("<td [Z]>VOL</td><td [Z]>%</td>");
            for (int i = 0; i < listaMarcas.Count; i++)
            {
                gerarHtml.Append("<td [BB]>VOL</td><td [BB]>%</td>");
            }
            gerarHtml.Append("<td [BB]></td></tr>");

            EmplacamentoCidadeSeparaDados(listaMarcas, gerarHtml, true);
            gerarHtml.Append("</table><br/><br/>");
            #endregion

            #region Tabela2 Fora

            listaEmplacamentoCidade = GetUserRegistroEmplacamentoCidadeFora(emplaca);
            if (listaEmplacamentoCidade.Count > 0)
            {
                for (int i = 0; i < listaEmplacamentoCidade.Count; i++)
                {
                    //Percorre a lista para verificar se existe a marca, essa lista de marca sera  usada para gerar as colunas 
                    if (!listaMarcas.Exists(c => c.NomeclaturaModelos == listaEmplacamentoCidade[i].NomeclaturaModelos))
                        listaMarcas.Add(new eEmplacamentoCidade { NomeclaturaModelos = listaEmplacamentoCidade[i].NomeclaturaModelos, Ordem = listaEmplacamentoCidade[i].Ordem });
                }
                listaMarcas = listaMarcas.OrderBy(c => c.Ordem).ToList();

                gerarHtml.Append("<table border='1' [EstiloTabelaBase] id='display'><tr style='background-color:[CorCabecalho];'>");
                gerarHtml.Append("<td colspan='" + ((listaMarcas.Count * 2) + 4) + "' [BB]>EMPLACAMENTO FORA DA ÁREA OPERACIONAL</td></tr>");
                gerarHtml.Append("<tr style='background-color:[CorCabecalho];'><td [BB] width='200'>MUNICÍPIOS</td>");
                gerarHtml.Append("<td width='115' [Z] colspan='2'>" + listaEmplacamentoCidade[0].NomeEmpresa + "</td>");
                for (int i = 0; i < listaMarcas.Count; i++)
                {
                    gerarHtml.Append("<td width='115' [BB] colspan='2'>" + listaMarcas[i].NomeclaturaModelos + "</td>");
                }
                gerarHtml.Append("<td width='55' [BB]>TOTAL</td></tr>");

                gerarHtml.Append("<tr style='background-color:[CorCabecalho];'><td [BB]></td>");
                gerarHtml.Append("<td [Z]>VOL</td><td [Z]>%</td>");
                for (int i = 0; i < listaMarcas.Count; i++)
                {
                    gerarHtml.Append("<td [BB]>VOL</td><td [BB]>%</td>");
                }
                gerarHtml.Append("<td [BB]></td></tr>");
                EmplacamentoCidadeSeparaDados(listaMarcas, gerarHtml, false);
            }

            #endregion

            return EmplacamentoCidadeReplaces(gerarHtml);
        }

        private void EmplacamentoCidadeSeparaDados(List<eEmplacamentoCidade> listaMarcas, StringBuilder gerarHtml, bool dentroArea)
        {
            List<eEmplacamentoCidade> listaAux = new List<eEmplacamentoCidade>();
            List<eEmplacamentoCidade> listaExiste = new List<eEmplacamentoCidade>();
            int indiceLista = 0;
            int[] Totais = new int[listaMarcas.Count];
            string[] PorcentTotal = new string[listaMarcas.Count];

            if (dentroArea)
            {

                for (int i = 0; i < listaEmplacamentoCidade.Count; i++)
                {
                    if (!listaExiste.Exists(c => c.NomeCidade == listaEmplacamentoCidade[i].NomeCidade))
                    {
                        listaAux = listaEmplacamentoCidade.Where(w => w.NomeCidade == listaEmplacamentoCidade[i].NomeCidade).ToList();
                        EmplacamentoCidadeDadosTabela(listaAux, gerarHtml, listaMarcas, Totais, PorcentTotal, false, dentroArea);
                        listaExiste.Insert(indiceLista, new eEmplacamentoCidade { NomeCidade = listaEmplacamentoCidade[i].NomeCidade });
                        indiceLista++;
                    }
                }
                EmplacamentoCidadeDadosTabela(listaAux, gerarHtml, listaMarcas, Totais, PorcentTotal, true, dentroArea);
            }
            else
            {
                indiceLista = 0;
                Totais = new int[listaMarcas.Count];
                PorcentTotal = new string[listaMarcas.Count];

                for (int i = 0; i < listaEmplacamentoCidade.Count; i++)
                {
                    if (!listaExiste.Exists(c => c.NomeCidade == listaEmplacamentoCidade[i].NomeCidade))
                    {
                        listaAux = listaEmplacamentoCidade.Where(w => w.NomeCidade == listaEmplacamentoCidade[i].NomeCidade).ToList();
                        EmplacamentoCidadeDadosTabela(listaAux, gerarHtml, listaMarcas, Totais, PorcentTotal, false, dentroArea);
                        listaExiste.Insert(indiceLista, new eEmplacamentoCidade { NomeCidade = listaEmplacamentoCidade[i].NomeCidade });
                        indiceLista++;
                    }
                }
                EmplacamentoCidadeDadosTabela(listaAux, gerarHtml, listaMarcas, Totais, PorcentTotal, true, dentroArea);
            }

        }

        private void EmplacamentoCidadeDadosTabela(List<eEmplacamentoCidade> listaAux, StringBuilder gerarHtml, List<eEmplacamentoCidade> listaMarcas, int[] Totais, string[] PorcentTotal, bool GerarTotal, bool dentroArea)
        {
            List<eEmplacamentoCidade> marcas = listaAux.Where(c => c.ConfereEmpresa == "1").ToList();
            listaAux = listaAux.OrderBy(c => c.Ordem).ToList();
            string Total = listaAux[0].Total;
            List<eEmplacamentoCidade> listaExistente = new List<eEmplacamentoCidade>();
            List<eEmplacamentoCidade> listaAux2 = new List<eEmplacamentoCidade>();

            #region Separando Marcas
            for (int i = 0; i < listaAux.Count; i++)
            {
                if (i.Equals(listaAux.Count - 1))
                {
                    if (!listaAux2.Exists(c => c.NomeclaturaModelos == listaAux[i].NomeclaturaModelos))
                    {
                        eEmplacamentoCidade emplac = new eEmplacamentoCidade();

                        emplac.CorLinha = listaAux[i].CorLinha;
                        emplac.IdCidade = listaAux[i].CorLinha;
                        emplac.Ano = listaAux[i].Ano;
                        emplac.Mes = listaAux[i].Mes;
                        emplac.EmpresaEmpId = listaAux[i].EmpresaEmpId;
                        emplac.ConfereEmpresa = listaAux[i].ConfereEmpresa;
                        emplac.NomeclaturaModelos = listaAux[i].NomeclaturaModelos;
                        emplac.IdLocalizacaoOperacional = listaAux[i].IdLocalizacaoOperacional;
                        emplac.NomeLocalizacaoOperacional = listaAux[i].NomeLocalizacaoOperacional;
                        emplac.NomeEmpresa = listaAux[i].NomeEmpresa;
                        emplac.Ordem = listaAux[i].Ordem;
                        emplac.Vol = listaAux[i].Vol;
                        emplac.VolTotal = listaAux[i].VolTotal;
                        emplac.Total = listaAux[i].Total;
                        emplac.TotalGeral = listaAux[i].TotalGeral;
                        emplac.Porcent = listaAux[i].Porcent;
                        emplac.PorcentTotal = listaAux[i].PorcentTotal;

                        listaAux2.Add(emplac);
                    }
                }
                else if (listaAux[i].NomeclaturaModelos != listaAux[i + 1].NomeclaturaModelos)
                {
                    eEmplacamentoCidade emplac = new eEmplacamentoCidade();

                    emplac.CorLinha = listaAux[i].CorLinha;
                    emplac.IdCidade = listaAux[i].CorLinha;
                    emplac.Ano = listaAux[i].Ano;
                    emplac.Mes = listaAux[i].Mes;
                    emplac.EmpresaEmpId = listaAux[i].EmpresaEmpId;
                    emplac.ConfereEmpresa = listaAux[i].ConfereEmpresa;
                    emplac.NomeclaturaModelos = listaAux[i].NomeclaturaModelos;
                    emplac.IdLocalizacaoOperacional = listaAux[i].IdLocalizacaoOperacional;
                    emplac.NomeLocalizacaoOperacional = listaAux[i].NomeLocalizacaoOperacional;
                    emplac.NomeEmpresa = listaAux[i].NomeEmpresa;
                    emplac.Ordem = listaAux[i].Ordem;
                    emplac.Vol = listaAux[i].Vol;
                    emplac.VolTotal = listaAux[i].VolTotal;
                    emplac.Total = listaAux[i].Total;
                    emplac.TotalGeral = listaAux[i].TotalGeral;
                    emplac.Porcent = listaAux[i].Porcent;
                    emplac.PorcentTotal = listaAux[i].PorcentTotal;

                    listaAux2.Add(emplac);
                }
            }
            #endregion

            int count = 0;
            listaAux2 = listaAux2.OrderBy(c => c.Ordem).ToList();

            if (dentroArea)
            {

                #region Conteudo
                if (!GerarTotal)
                {
                    if (marcas.Count > 0)
                    {
                        if (string.IsNullOrEmpty(marcas[0].PorcentMarca))
                            marcas[0].PorcentMarca = "0,0";
                    }

                    if (listaAux2.Count.Equals(listaMarcas.Count))
                    {
                        for (int i = 0; i < listaAux2.Count; i++)
                        {
                            PorcentTotal[i] = listaAux2[i].PorcentTotal;
                        }
                    }

                    if (listaAux[0].CorLinha.Equals("1"))
                    {
                        gerarHtml.Append("<tr style='color:#000; font-weight:normal; background-color:#DDD;'><td [B] [AL]>" + listaAux[0].NomeCidade + "</td>");
                        if (marcas.Count > 0)
                            gerarHtml.Append("<td [B]>" + marcas[0].VolMarca + "</td><td [B]>" + marcas[0].PorcentMarca + "%</td>");
                        else
                            gerarHtml.Append("<td [B]></td><td [B]>0,0%</td>");

                        for (int i = 0; i < listaMarcas.Count; i++)
                        {
                            if (listaAux2[i].Ordem == listaMarcas[i].Ordem && listaMarcas[i].NomeclaturaModelos != listaAux2[i].NomeclaturaModelos)
                            {
                                listaAux2[i].Ordem = listaAux2[i].Ordem + 1;
                                listaAux2 = listaAux2.OrderBy(c => c.Ordem).ToList();
                            }

                            if (listaAux2.Count < listaMarcas.Count)
                            {
                                for (int d = 0; d < listaMarcas.Count; d++)
                                {
                                    if (!listaAux2.Exists(c => c.NomeclaturaModelos == listaMarcas[d].NomeclaturaModelos))
                                    {
                                        listaAux2.Insert(d, new eEmplacamentoCidade
                                        {
                                            NomeclaturaModelos = listaMarcas[d].NomeclaturaModelos,
                                            Vol = "",
                                            Porcent = "0,0",
                                            VolMarca = "",
                                            PorcentMarca = "0,0",
                                        });
                                    }
                                }
                            }

                            if (!listaExistente.Exists(c => c.NomeclaturaModelos == listaAux2[i].NomeclaturaModelos))
                            {
                                if (count.Equals(0))
                                {
                                    if (string.IsNullOrEmpty(listaAux2[i].Vol))
                                        Totais[i] += 0;
                                    else
                                        Totais[i] += int.Parse(listaAux2[i].Vol);
                                }
                                else
                                {
                                    if (string.IsNullOrEmpty(listaAux2[i].Vol))
                                        Totais[count] += 0;
                                    else
                                        Totais[count] += int.Parse(listaAux2[i].Vol);

                                    count++;
                                }

                                listaExistente.Add(new eEmplacamentoCidade
                                {
                                    NomeclaturaModelos = listaAux2[i].NomeclaturaModelos,
                                    EmpresaEmpId = listaAux2[i].EmpresaEmpId,
                                    ConfereEmpresa = listaAux2[i].ConfereEmpresa
                                });
                                gerarHtml.Append("<td [B]>" + listaAux2[i].Vol + "</td><td [B]>" + listaAux2[i].Porcent + "%</td>");
                            }
                            else
                            {
                                count = i;
                            }

                        }

                        gerarHtml.Append("<td [B]>" + Total + "</td></tr>");
                    }
                    else
                    {
                        gerarHtml.Append("<tr style='color:#000; font-weight:normal;'><td [B] [AL]>" + listaAux[0].NomeCidade + "</td>");
                        if (marcas.Count > 0)
                            gerarHtml.Append("<td [B]>" + marcas[0].VolMarca + "</td><td [B]>" + marcas[0].PorcentMarca + "%</td>");
                        else
                            gerarHtml.Append("<td [B]></td><td [B]>0,0%</td>");

                        for (int i = 0; i < listaMarcas.Count; i++)
                        {
                            if (listaAux2[i].Ordem == listaMarcas[i].Ordem && listaMarcas[i].NomeclaturaModelos != listaAux2[i].NomeclaturaModelos)
                            {
                                listaAux2[i].Ordem = listaAux2[i].Ordem + 1;
                                listaAux2 = listaAux2.OrderBy(c => c.Ordem).ToList();
                            }

                            if (listaAux2.Count < listaMarcas.Count)
                            {
                                for (int d = 0; d < listaMarcas.Count; d++)
                                {
                                    if (!listaAux2.Exists(c => c.NomeclaturaModelos == listaMarcas[d].NomeclaturaModelos))
                                    {
                                        listaAux2.Insert(d, new eEmplacamentoCidade
                                        {
                                            NomeclaturaModelos = listaMarcas[d].NomeclaturaModelos,
                                            Vol = "",
                                            Porcent = "0,0",
                                            VolMarca = "",
                                            PorcentMarca = "0,0",
                                        });
                                    }
                                }
                            }

                            if (string.IsNullOrEmpty(listaAux2[i].Vol))
                                Totais[i] += 0;
                            else
                                Totais[i] += int.Parse(listaAux2[i].Vol);

                            gerarHtml.Append("<td [B]>" + listaAux2[i].Vol + "</td><td [B]>" + listaAux2[i].Porcent + "%</td>");
                        }

                        gerarHtml.Append("<td [B]>" + Total + "</td></tr>");
                    }
                }
                #endregion

                #region Total
                if (GerarTotal)
                {
                    gerarHtml.Append("<tr style='color:#000; font-weight:normal;'><td [AL] [B]>TOTAL</td>");
                    gerarHtml.Append("<td [B]>" + listaAux[0].VolMarcaTotal + "</td>");
                    gerarHtml.Append("<td [B]>" + string.Format("{0:N1}", (float.Parse(listaAux[0].VolMarcaTotal) * 100) / Totais[0]) + "%</td>");
                    for (int i = 0; i < Totais.Length; i++)
                    {
                        gerarHtml.Append("<td [B]>" + Totais[i] + "</td>");
                        gerarHtml.Append("<td [B]>" + PorcentTotal[i] + "%</td>");
                    }
                    gerarHtml.Append("<td [B]><b>" + listaAux2[0].TotalGeral + "</b></td></tr>");
                }
                #endregion
            }
            else
            {
                #region Conteudo

                if (!GerarTotal)
                {
                    if (marcas.Count > 0)
                    {
                        if (string.IsNullOrEmpty(marcas[0].PorcentMarca))
                            marcas[0].PorcentMarca = "0,0";
                    }

                    if (listaAux2.Count.Equals(listaMarcas.Count))
                    {
                        for (int i = 0; i < listaAux2.Count; i++)
                        {
                            PorcentTotal[i] = listaAux2[i].PorcentTotal;
                        }
                    }


                    gerarHtml.Append("<tr style='color:#000; font-weight:normal;'><td [B] [AL]>" + listaAux[0].NomeCidade + "</td>");
                    if (marcas.Count > 0)
                        gerarHtml.Append("<td [B]>" + marcas[0].VolMarca + "</td><td [B]>" + marcas[0].PorcentMarca + "%</td>");
                    else
                        gerarHtml.Append("<td [B]></td><td [B]>0,0%</td>");

                    for (int i = 0; i < listaMarcas.Count; i++)
                    {
                        if (listaAux2[i].Ordem == listaMarcas[i].Ordem && listaMarcas[i].NomeclaturaModelos != listaAux2[i].NomeclaturaModelos)
                        {
                            listaAux2[i].Ordem = listaAux2[i].Ordem + 1;
                            listaAux2 = listaAux2.OrderBy(c => c.Ordem).ToList();
                        }

                        for (int d = 0; d < listaMarcas.Count; d++)
                        {
                            if (!listaAux2.Exists(c => c.NomeclaturaModelos == listaMarcas[d].NomeclaturaModelos))
                            {
                                listaAux2.Insert(d, new eEmplacamentoCidade
                                {
                                    NomeclaturaModelos = listaMarcas[d].NomeclaturaModelos,
                                    Vol = "",
                                    Porcent = "0,0",
                                    VolMarca = "",
                                    PorcentMarca = "0,0",
                                });
                            }
                        }

                        if (string.IsNullOrEmpty(listaAux2[i].Vol))
                            Totais[i] += 0;
                        else
                            Totais[i] += int.Parse(listaAux2[i].Vol);

                        gerarHtml.Append("<td [B]>" + listaAux2[i].Vol + "</td><td [B]>" + listaAux2[i].Porcent + "%</td>");

                    }

                    gerarHtml.Append("<td [B]>" + Total + "</td></tr>");
                }

                #endregion

                #region Total

                if (GerarTotal)
                {
                    gerarHtml.Append("<tr style='color:#000; font-weight:normal;'><td [AL] [B]>TOTAL</td>");
                    gerarHtml.Append("<td [B]>" + listaAux[0].VolMarcaTotal + "</td>");
                    gerarHtml.Append("<td [B]>" + string.Format("{0:N1}", (float.Parse(listaAux[0].VolMarcaTotal) * 100) / Totais[0]) + "%</td>");
                    for (int i = 0; i < Totais.Length; i++)
                    {
                        gerarHtml.Append("<td [B]>" + Totais[i] + "</td>");
                        gerarHtml.Append("<td [B]>" + PorcentTotal[i] + "%</td>");
                    }
                    gerarHtml.Append("<td [B]><b>" + listaAux2[0].TotalGeral + "</b></td></tr>");
                }

                #endregion
            }

        }

        private string EmplacamentoCidadeReplaces(StringBuilder gerarHtml)
        {
            gerarHtml = gerarHtml.Replace("[EstiloTabelaBase]", EstiloTabelaBase);
            gerarHtml = gerarHtml.Replace("[SIZE]", "10");
            gerarHtml = gerarHtml.Replace("[NCelPadding]", "3");
            gerarHtml = gerarHtml.Replace("[CorBordaLinha]", "#000");
            gerarHtml = gerarHtml.Replace("[CorCabecalho]", "#" + eConfig.RelatorioCorCabecalho);
            gerarHtml = gerarHtml.Replace("[B]", "style='border: 1px solid #D3D3D3;'");
            gerarHtml = gerarHtml.Replace("[A]", "border: 1px solid #000;");
            gerarHtml = gerarHtml.Replace("[AL]", "align='left'");
            gerarHtml = gerarHtml.Replace("[CA]", "style='border: 1px solid #000; background-color:#D3D3D3; color:#000;'");
            gerarHtml = gerarHtml.Replace("[CB]", "style='background-color:#DED9C5; color:#000; border: 1px solid #000;'");
            gerarHtml = gerarHtml.Replace("[LINK]", "style='text-decoration: none; color:#000;'");
            gerarHtml = gerarHtml.Replace("[BB]", "style='border: 1px solid #FFF;'");
            gerarHtml = gerarHtml.Replace("[Z]", "style='background-color:#808080; border: 1px solid #FFF;'");

            return gerarHtml.ToString();
        }

        private List<eEmplacamentoCidade> GetUserRegistroEmplacamentoCidade(eEmplacamentoCidade emplac)
        {
            try
            {
                dRelatorios db = new dRelatorios();

                if (emplac.ByGroup)
                    return db.GetUserRelatorioEmplacamentoCidadeGrupo(emplac);
                else
                    return db.GetUserRelatorioEmplacamentoCidade(emplac);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<eEmplacamentoCidade> GetUserRegistroEmplacamentoCidadeFora(eEmplacamentoCidade emplac)
        {
            try
            {
                dRelatorios db = new dRelatorios();
                return db.GetUserRelatorioEmplacamentoCidadeFora(emplac);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        //implementando
        #region EmplacamentoCidadeGrupo

        public string EmplacamentoCidadeGrupoTabela(eEmplacamentoCidade emplac)
        {
            List<eEmplacamentoCidade> listaMarcas = new List<eEmplacamentoCidade>();
            listaEmplacamentoCidade = GetUserRegistroEmplacamentoCidade(emplac);
            gerarHtml = new StringBuilder();

            #region Tabela1 Dentro da Area
            for (int i = 0; i < listaEmplacamentoCidade.Count; i++)
            {
                //Percorre a lista para verificar se existe a marca, essa lista de marca sera  usada para gerar as colunas 
                if (!listaMarcas.Exists(c => c.NomeclaturaModelos == listaEmplacamentoCidade[i].NomeclaturaModelos))
                    listaMarcas.Add(new eEmplacamentoCidade { NomeclaturaModelos = listaEmplacamentoCidade[i].NomeclaturaModelos, Ordem = listaEmplacamentoCidade[i].Ordem });
            }
            listaMarcas = listaMarcas.OrderBy(c => c.Ordem).ToList();

            gerarHtml.Append("<table border='1' [EstiloTabelaBase] id='display'><tr style='background-color:[CorCabecalho];'>");
            gerarHtml.Append("<td [BB]></td><td colspan='" + ((listaMarcas.Count * 2) + 4) + "' [BB]>EMPLACAMENTO DENTRO DA ÁREA OPERACIONAL: " + listaEmplacamentoCidade[0].NomeAreaOperacional + "</td></tr>");
            gerarHtml.Append("<tr style='background-color:[CorCabecalho];'><td [BB] width='190'>INFLUÊNCIA</td><td [BB] width='200'>MUNICÍPIOS</td>");
            gerarHtml.Append("<td width='115' [Z] colspan='2'>GRUPO</td>");
            for (int i = 0; i < listaMarcas.Count; i++)
            {
                gerarHtml.Append("<td width='115' [BB] colspan='2'>" + listaMarcas[i].NomeclaturaModelos + "</td>");
            }
            gerarHtml.Append("<td width='55' [BB]>TOTAL</td></tr>");

            gerarHtml.Append("<tr style='background-color:[CorCabecalho];'><td [BB]></td><td [BB]></td>");
            gerarHtml.Append("<td [Z]>VOL</td><td [Z]>%</td>");
            for (int i = 0; i < listaMarcas.Count; i++)
            {
                gerarHtml.Append("<td [BB]>VOL</td><td [BB]>%</td>");
            }
            gerarHtml.Append("<td [BB]></td></tr>");

            EmplacamentoCidadeGrupoSeparaDados(listaMarcas, gerarHtml);

            #endregion

            return EmplacamentoCidadeReplaces(gerarHtml);
        }

        private void EmplacamentoCidadeGrupoSeparaDados(List<eEmplacamentoCidade> listaMarcas, StringBuilder gerarHtml)
        {
            listaEmplacamentoCidade = listaEmplacamentoCidade.OrderBy(c => c.NomeLocalizacaoOperacional).ToList();
            List<eEmplacamentoCidade> listaExis = new List<eEmplacamentoCidade>();
            List<eEmplacamentoCidade> listaAux = new List<eEmplacamentoCidade>();


            for (int i = 0; i < listaEmplacamentoCidade.Count; i++)
            {
                if (!listaExis.Exists(c => c.NomeLocalizacaoOperacional == listaEmplacamentoCidade[i].NomeLocalizacaoOperacional))
                {
                    listaExis.Add(new eEmplacamentoCidade { NomeLocalizacaoOperacional = listaEmplacamentoCidade[i].NomeLocalizacaoOperacional });
                    listaAux = listaEmplacamentoCidade.Where(c => c.NomeLocalizacaoOperacional == listaEmplacamentoCidade[i].NomeLocalizacaoOperacional).ToList();
                    EmplacamentoCidadeGrupoDadosTabela(listaAux, listaMarcas, gerarHtml, false);
                }
            }
        }

        private void EmplacamentoCidadeGrupoDadosTabela(List<eEmplacamentoCidade> listaAux, List<eEmplacamentoCidade> listaMarcas, StringBuilder gerarHtml, bool GerarTotal)
        {
            List<eEmplacamentoCidade> listaAuxiliar = new List<eEmplacamentoCidade>();//Lista que pgara os dados de aocordo com a cidade
            List<eEmplacamentoCidade> listaExiste = new List<eEmplacamentoCidade>();//lista responsalvel por verificar se a cidade já foi incluida
            List<eEmplacamentoCidade> listaGrupo = new List<eEmplacamentoCidade>(); // lista responsavel por 
            bool Zebrado = true;
            int rows = 0;
            string nomeCidade = string.Empty;
            string Total = string.Empty;

            for (int i = 0; i < listaAux.Count; i++)
            {
                if (i.Equals(listaAux.Count - 1))
                {
                    if (listaAux[i].NomeCidade != listaAux[i - 1].NomeCidade)
                    {
                        rows += 1;
                    }
                }
                else if (listaAux[i].NomeCidade != listaAux[i + 1].NomeCidade)
                {
                    rows += 1;
                }
            }

            gerarHtml.Append("<tr style='color:#000; font-weight:normal;'><td [B] rowspan='" + (rows + 2) + "'>" + listaAux[0].NomeLocalizacaoOperacional + "</td>");
            for (int i = 0; i < listaAux.Count; i++)
            {
                if (!listaExiste.Exists(c => c.NomeCidade == listaAux[i].NomeCidade))
                {
                    listaExiste.Add(new eEmplacamentoCidade { NomeCidade = listaAux[i].NomeCidade });
                    listaAuxiliar = listaAux.Where(c => c.NomeCidade == listaAux[i].NomeCidade).ToList();

                    //Preenchendo caso não exista a marca para essa cidade
                    if (listaAuxiliar.Count < listaMarcas.Count)
                    {
                        for (int d = 0; d < listaMarcas.Count; d++)
                        {
                            if (!listaAuxiliar.Exists(c => c.NomeclaturaModelos == listaMarcas[d].NomeclaturaModelos))
                            {
                                listaAuxiliar.Insert(d, new eEmplacamentoCidade
                                {
                                    NomeclaturaModelos = listaMarcas[d].NomeclaturaModelos,
                                    Vol = "",
                                    Porcent = "0,0",
                                });
                            }
                        }
                    }//retirando caso venha com duas marcas iguais.
                    else if (listaAuxiliar.Count > listaMarcas.Count)
                    {
                        for (int d = 0; d < listaMarcas.Count; d++)
                        {

                            if (listaAuxiliar[d].NomeclaturaModelos != listaMarcas[d].NomeclaturaModelos)
                            {
                                listaAuxiliar.RemoveAt(d);
                                d--;
                            }
                        }
                    }

                    listaGrupo = listaAuxiliar.Where(c => c.ConfereEmpresa == "1").ToList();
                    nomeCidade = listaAuxiliar.Where(c => c.NomeCidade != null).First().NomeCidade;
                    Total = listaAuxiliar.Where(c => c.Total != null).First().Total;
                    if (Zebrado)
                    {
                        gerarHtml.Append("<tr style='color:#000; font-weight:normal; background-color:#DCDCDC;'><td [B] [AL]>" + nomeCidade + "</td>");
                        if (listaGrupo.Count > 0)
                            gerarHtml.Append("<td [B]>" + listaGrupo[0].VolMarca + "</td><td [B]>" + listaGrupo[0].PorcentMarca + "</td>");
                        else
                            gerarHtml.Append("<td [B]></td><td [B]>0,0</td>");
                        for (int d = 0; d < listaAuxiliar.Count; d++)
                        {
                            gerarHtml.Append("<td [B]>" + listaAuxiliar[d].Vol + "</td><td [B]>" + listaAuxiliar[d].Porcent + "</td>");
                        }
                        gerarHtml.Append("<td [B]>" + Total + "</td></tr>");
                        Zebrado = false;
                    }
                    else
                    {
                        gerarHtml.Append("<tr style='color:#000; font-weight:normal;'><td [B] [AL]>" + nomeCidade + "</td>");
                        if (listaGrupo.Count > 0)
                            gerarHtml.Append("<td [B]>" + listaGrupo[0].VolMarca + "</td><td [B]>" + listaGrupo[0].PorcentMarca + "</td>");
                        else
                            gerarHtml.Append("<td [B]></td><td [B]>0,0</td>");
                        for (int d = 0; d < listaAuxiliar.Count; d++)
                        {
                            gerarHtml.Append("<td [B]>" + listaAuxiliar[d].Vol + "</td><td [B]>" + listaAuxiliar[d].Porcent + "</td>");
                        }
                        gerarHtml.Append("<td [B]>" + Total + "</td></tr>");
                        Zebrado = true;
                    }
                }
            }
            gerarHtml.Append("</tr>");
        }

        #endregion

        //Falta ajustar para ser mais dinamico
        #region DiarioMarcas

        public string DiarioMarcasTitulo()
        {
            gerarHtml = new StringBuilder();

            gerarHtml.Append("<div align='center' style='width: 1194px; display:inline-block;'>");
            gerarHtml.Append("<div align='left'style='width: 202px; display:inline; float:left;'>");
            gerarHtml.Append("<img src='[IMG]' alt='Smiley face' height='100' width='200' style='float:right;' /></div>");
            gerarHtml.Append("<div style='width: 807px; display:inline; float:inherit'>");
            gerarHtml.Append("<h2 align='center' style='width:800px; color:[COR]'; font-family:Tahoma; font-size:14pt; fontweight:bold;'>EMPLACAMENTO DIARIO POR MARCA - ");
            gerarHtml.Append("[DIAUTIL]° DIA UTIL<br/>[DataTitulo]<br/>[ABRANGENCIA] - [MODALIDADE]<br/>[CATEGORIA]</h2></div></div>");

            return ReplacesTitulo(gerarHtml.ToString());
        }

        public string DiarioMarcasTabela()
        {
            gerarHtml = new StringBuilder();

            gerarHtml.Append("<table border='1px' [EstiloTabelaBase] id='display'>");

            gerarHtml.Append("<tr align='center' style='background:[CorCabecalho]'><td colspan='2'>ACUMULADO</td><td colspan='13'></td><td colspan='2'>ACUMULADO</td><td></td><td></td></tr>");

            gerarHtml.Append("<tr align='center' style='background:[CorCabecalho]'><td colspan='2'>ANO</td><td width='110px'>DIA DO MÊS</td><td colspan='2'>[MES0]</td><td colspan='2'>[MES1]</td>");
            gerarHtml.Append("<td colspan='2'>[MES2]</td><td colspan='2'>[MES3]</td><td colspan='2'>[MES4]</td><td colspan='2'>ACUMULADO</td>");
            gerarHtml.Append("<td colspan='2'>[MESMESANO]</td><td colspan='2'>Diferença</td></tr>");

            gerarHtml.Append("<tr align='center' style='background:[CorCabecalho]'><td colspan='2'>[ANO]</td><td>DATA ÚTIL</td><td colspan='2'>[DIA0]</td><td colspan='2'>[DIA1]</td>");
            gerarHtml.Append("<td colspan='2'>[DIA2]</td><td colspan='2'>[DIA3]</td><td colspan='2'>[DIA4]</td><td colspan='2'>[UTILMESANO]</td>");
            gerarHtml.Append("<td colspan='2'>ATÉ A DATA</td><td colspan='2'>em:</td></tr>");

            gerarHtml.Append("<tr align='center' style='background:[CorCabecalho]'><td width='70px'>VOL</td><td width='70px'>%</td><td>MARCAS</td><td width='70px'>VOL</td><td width='70px'>%</td>");
            gerarHtml.Append("<td width='70px'>VOL</td><td width='70px'>%</td><td width='70px'>VOL</td><td width='70px'>%</td><td width='70px'>VOL</td>");
            gerarHtml.Append("<td width='70px'>%</td><td width='70px'>VOL</td><td width='70px'>%</td><td width='70px'>VOL</td><td width='70px'>%</td>");
            gerarHtml.Append("<td width='70px'>VOL</td><td width='70px'>%</td><td width='70px'>VOL</td><td width='70px'>%</td></tr>");

            DiarioMarcaDadosTabela(gerarHtml);

            gerarHtml.Append("<tr align='center' style='background:#FFF; color:#000;font-weight:normal;font-size: 8pt;'><td>[TAVOL]</td><td>[TAPOR]</td><td><b>TOTAL</b></td>");
            gerarHtml.Append("<td><b>[TVOL0]</b></td><td>[TPOR0]</td><td><b>[TVOL1]</b></td><td>[TPOR1]</td><td><b>[TVOL2]</b></td><td>[TPOR2]</td><td><b>[TVOL3]</b></td><td>[TPOR3]</td>");
            gerarHtml.Append("<td><b>[TVOL4]</b></td><td>[TPOR4]</td><td><b>[TUMAVOL]</b></td><td>[TUMAPOR]</td><td><b>[TADVOL]</b></td><td>[TADPOR]</td><td><b>[TDVOL]</b></td><td>[TDPOR]</td></tr></table><br />");

            return DiarioMarcaReplacesTabela(gerarHtml);
        }

        private void DiarioMarcaDadosTabela(StringBuilder gerarHtml)
        {
            for (int i = 0; i <= 7; i++)
            {
                if (i % 2 == 0)
                {
                    gerarHtml.Append("<tr align='center' style='background:#DDD; color:#000; font-weight:normal;font-size: 8pt;'><td>[AVOL" + i + "]</td><td>[APOR" + i + "]</td><td><b>[MARCA" + i + "]</b></td>");
                    gerarHtml.Append("<td><b>[VOL0" + i + "]</b></td><td>[POR0" + i + "]</td><td><b>[VOL1" + i + "]</b></td><td>[POR1" + i + "]</td><td><b>[VOL2" + i + "]</b></td><td>[POR2" + i + "]</td><td><b>[VOL3" + i + "]</b></td><td>[POR3" + i + "]</td");
                    gerarHtml.Append("><td><b>[VOL4" + i + "]</b></td><td>[POR4" + i + "]</td><td><b>[UMAVOL" + i + "]</b></td><td>[UMAPOR" + i + "]</td><td><b>[ADVOL" + i + "]</b></td><td>[ADPOR" + i + "]</td><td><b>[DVOL" + i + "]</b></td><td>[DPOR" + i + "]</td></tr>");
                }
                else
                {
                    gerarHtml.Append("<tr align='center' style='background:#FFF; color:#000; font-weight:normal;font-size: 8pt;'><td>[AVOL" + i + "]</td><td>[APOR" + i + "]</td><td><b>[MARCA" + i + "]</b></td>");
                    gerarHtml.Append("<td><b>[VOL0" + i + "]</b></td><td>[POR0" + i + "]</td><td><b>[VOL1" + i + "]</b></td><td>[POR1" + i + "]</td><td><b>[VOL2" + i + "]</b></td><td>[POR2" + i + "]</td><td><b>[VOL3" + i + "]</b></td><td>[POR3" + i + "]</td");
                    gerarHtml.Append("><td><b>[VOL4" + i + "]</b></td><td>[POR4" + i + "]</td><td><b>[UMAVOL" + i + "]</b></td><td>[UMAPOR" + i + "]</td><td><b>[ADVOL" + i + "]</b></td><td>[ADPOR" + i + "]</td><td><b>[DVOL" + i + "]</b></td><td>[DPOR" + i + "]</td></tr>");
                }
            }
        }

        private string DiarioMarcaReplacesTabela(StringBuilder gerarHtml)
        {

            gerarHtml = gerarHtml.Replace("[EstiloTabelaBase]", EstiloTabelaBase);
            gerarHtml = gerarHtml.Replace("[SIZE]", "10");
            gerarHtml = gerarHtml.Replace("[CorCabecalho]", "#" + eConfig.RelatorioCorCabecalho);
            gerarHtml = gerarHtml.Replace("[CorBordaLinha]", "#" + eConfig.RelatorioCorLinhaAlternada);
            gerarHtml = gerarHtml.Replace("[NCelPadding]", "2");

            string[] Mes = new string[5];
            string[] Dia = new string[5];
            string[] AcuVol = new string[8];
            string[] AcuPor = new string[8];
            string[] Marcas = new string[8];
            string[] Vol = new string[8];
            string[] Por = new string[8];
            string[] UMAVol = new string[8];
            string[] UMAPor = new string[8];
            string[] ADVol = new string[8];
            string[] ADPor = new string[8];
            string[] DVol = new string[8];
            string[] DPor = new string[8];
            string[] TotalVol = new string[5];
            string[] TotalPor = new string[5];


            string Ano = "2016";
            string UtilMesAno = "SETEMBRO/2016";
            string MesMesano = "AGOSTO/2016";
            string AcuTotalVol = "100";
            string AcuTotalPor = "100%";
            string TUMAVol = "100";
            string TUMAPor = "100%";
            string TADVol = "100";
            string TADPor = "100%";
            string TDVol = "100";
            string TDPor = "100%";

            for (int i = 0; i <= 7; i++)
            {
                AcuVol[i] = i.ToString();
                gerarHtml = gerarHtml.Replace("[AVOL" + i + "]", AcuVol[i]);
                AcuPor[i] = i.ToString();
                gerarHtml = gerarHtml.Replace("[APOR" + i + "]", AcuPor[i]);
                Marcas[i] = i.ToString();
                gerarHtml = gerarHtml.Replace("[MARCA" + i + "]", Marcas[i]);
                UMAVol[i] = i.ToString();
                gerarHtml = gerarHtml.Replace("[UMAVOL" + i + "]", UMAVol[i]);
                UMAPor[i] = i.ToString() + "%";
                gerarHtml = gerarHtml.Replace("[UMAPOR" + i + "]", UMAPor[i]);
                ADVol[i] = i.ToString();
                gerarHtml = gerarHtml.Replace("[ADVOL" + i + "]", ADVol[i]);
                ADPor[i] = i.ToString() + "%";
                gerarHtml = gerarHtml.Replace("[ADPOR" + i + "]", ADPor[i]);
                DVol[i] = i.ToString();
                gerarHtml = gerarHtml.Replace("[DVOL" + i + "]", DVol[i]);
                DPor[i] = i.ToString() + "%";
                gerarHtml = gerarHtml.Replace("[DPOR" + i + "]", DPor[i]);

                Vol[i] = i.ToString();
                gerarHtml = gerarHtml.Replace("[VOL0" + i + "]", Vol[i]);
                Vol[i] = i.ToString();
                gerarHtml = gerarHtml.Replace("[VOL1" + i + "]", Vol[i]);
                Vol[i] = i.ToString();
                gerarHtml = gerarHtml.Replace("[VOL2" + i + "]", Vol[i]);
                Vol[i] = i.ToString();
                gerarHtml = gerarHtml.Replace("[VOL3" + i + "]", Vol[i]);
                Vol[i] = i.ToString();
                gerarHtml = gerarHtml.Replace("[VOL4" + i + "]", Vol[i]);

                Por[i] = i.ToString() + "%";
                gerarHtml = gerarHtml.Replace("[POR0" + i + "]", Por[i]);
                Por[i] = i.ToString() + "%";
                gerarHtml = gerarHtml.Replace("[POR1" + i + "]", Por[i]);
                Por[i] = i.ToString() + "%";
                gerarHtml = gerarHtml.Replace("[POR2" + i + "]", Por[i]);
                Por[i] = i.ToString() + "%";
                gerarHtml = gerarHtml.Replace("[POR3" + i + "]", Por[i]);
                Por[i] = i.ToString() + "%";
                gerarHtml = gerarHtml.Replace("[POR4" + i + "]", Por[i]);
            }

            for (int i = 0; i <= 4; i++)
            {
                Mes[i] = "Mes Nº" + i.ToString();
                gerarHtml = gerarHtml.Replace("[MES" + i + "]", Mes[i]);
                Dia[i] = "Dia Util Nº" + i.ToString();
                gerarHtml = gerarHtml.Replace("[DIA" + i + "]", Dia[i]);
                TotalVol[i] = i.ToString();
                gerarHtml = gerarHtml.Replace("[TVOL" + i + "]", TotalVol[i]);
                TotalPor[i] = i.ToString() + "%";
                gerarHtml = gerarHtml.Replace("[TPOR" + i + "]", TotalPor[i]);
            }

            gerarHtml = gerarHtml.Replace("[ANO]", Ano);
            gerarHtml = gerarHtml.Replace("[TAVOL]", AcuTotalVol);
            gerarHtml = gerarHtml.Replace("[TAPOR]", AcuTotalPor);
            gerarHtml = gerarHtml.Replace("[TUMAVOL]", TUMAVol);
            gerarHtml = gerarHtml.Replace("[TUMAPOR]", TUMAPor);
            gerarHtml = gerarHtml.Replace("[TADVOL]", TADVol);
            gerarHtml = gerarHtml.Replace("[TADPOR]", TADPor);
            gerarHtml = gerarHtml.Replace("[TDVOL]", TDVol);
            gerarHtml = gerarHtml.Replace("[TDPOR]", TDPor);
            gerarHtml = gerarHtml.Replace("[MESMESANO]", MesMesano);
            gerarHtml = gerarHtml.Replace("[UTILMESANO]", UtilMesAno);

            return gerarHtml.ToString();
        }

        #endregion

        //Falta ajustar para ser mais dinamico
        #region Ultimos12Meses

        public string Ultimo12MesesHtmlTitulo()
        {
            gerarHtml = new StringBuilder();

            gerarHtml.Append("<div align='center' style='width: 1194px; display:inline-block;'>");
            gerarHtml.Append("<div align='left'style='width: 202px; display:inline; float:left; height: 245px;'>");
            gerarHtml.Append("<img src='[IMG]' alt='Smiley face' height='100' width='200' style='float:right;' /></div>");
            gerarHtml.Append("<div style='width: 807px; display:inline; float:inherit'>");
            gerarHtml.Append("<h2 align='center' style='width:800px; color:[COR]'; font-family:Tahoma; font-size:14pt; fontweight:bold;'>EMPLACAMENTO ÚLTIMOS 12 MESES");
            gerarHtml.Append("<br/>[DataTitulo]<br/>[ABRANGENCIA]<br/>[Categoria]</h2></div></div>");

            return ReplacesTitulo(gerarHtml.ToString());
        }

        public string Ultimo12MesesTabela()
        {
            gerarHtml = new StringBuilder();

            gerarHtml.Append("<table border='1' [EstiloTabelaBase] width='1300px' id='display'>");
            gerarHtml.Append("<tr align='center' style='background-color:[CorCabecalho];'><td width='200px'>&nbsp</td><td colspan='26'></td></tr>");
            gerarHtml.Append("<tr align='center' style='background-color:#F2D44E;'><td>Meses</td><td colspan='2'>[mes0]</td>");
            gerarHtml.Append("<td colspan='2'>[mes1]</td><td colspan='2'>[mes2]</td><td colspan='2'>[mes3]</td><td colspan='2'>[mes4]</td>");
            gerarHtml.Append("<td colspan='2'>[mes5]</td><td colspan='2'>[mes6]</td><td colspan='2'>[mes7]</td><td colspan='2'>[mes8]</td>");
            gerarHtml.Append("<td colspan='2'>[mes9]</td><td colspan='2'>[mes10]</td><td colspan='2'>[mes11]</td>");
            gerarHtml.Append("<td colspan='2' rowspan='2' style='color:#000; font-size:10pt; background-color:#DCDCDC;'>TOTAL</td></tr>");

            gerarHtml.Append("<tr align='center' style='background-color:[CorCabecalho];'><td>Dia Útil</td><td colspan='2'>[dia0]</td>");
            gerarHtml.Append("<td colspan='2'>[dia1]</td><td colspan='2'>[dia2]</td><td colspan='2'>[dia3]</td><td colspan='2'>[dia4]</td>");
            gerarHtml.Append("<td colspan='2'>[dia5]</td><td colspan='2'>[dia6]</td><td colspan='2'>[dia7]</td><td colspan='2'>[dia8]</td>");
            gerarHtml.Append("<td colspan='2'>[dia9]</td><td colspan='2'>[dia10]</td><td colspan='2'>[dia11]</td></tr>");

            gerarHtml.Append("<tr align='center' style='background-color:[CorCabecalho];'><td>Marcas</td><td width='50px'>Vol</td><td width='50px'>%</td>");
            gerarHtml.Append("<td width='50px'>Vol</td><td width='50px'>%</td><td width='50px'>Vol</td><td width='50px'>%</td><td width='50px'>Vol</td>");
            gerarHtml.Append("<td width='50px'>%</td><td width='50px'>Vol</td><td width='50px'>%</td><td width='50px'>Vol</td><td width='50px'>%</td>");
            gerarHtml.Append("<td width='50px'>Vol</td><td width='50px'>%</td><td width='50px'>Vol</td><td width='50px'>%</td><td width='50px'>Vol</td>");
            gerarHtml.Append("<td width='50px'>%</td><td width='50px'>Vol</td><td width='50px'>%</td><td width='50px'>Vol</td><td width='50px'>%</td>");
            gerarHtml.Append("<td width='50px'>Vol</td>");
            gerarHtml.Append("<td width='50px'>%</td><td width='80px' style='color:#000; background-color:#DCDCDC;'>Vol</td>");
            gerarHtml.Append("<td width='50px' style='color:#000; background-color:#DCDCDC;'>%</td></tr>");

            Ultimos12MesesDadosTabela(gerarHtml);

            gerarHtml.Append("<tr align='center' style='color:#000; font-weight:normal;'><td align='left'><b>TOTAL</b></td><td>[Tvol0]</td><td></td>");
            gerarHtml.Append("<td>[Tvol1]</td><td></td><td>[Tvol2]</td><td></td><td>[Tvol3]</td><td></td><td>[Tvol4]</td><td></td><td>[Tvol5]</td>");
            gerarHtml.Append("<td></td><td>[Tvol6]</td><td></td><td>[Tvol7]</td><td></td><td>[Tvol8]</td><td></td>");
            gerarHtml.Append("<td>[Tvol9]</td><td></td><td>[Tvol10]</td><td></td><td>[Tvol11]</td><td></td><td><b>[TvolT]</b></td><td></td></tr>");

            gerarHtml.Append("<tr align='center' style='color:#000; font-weight:bold;'><td align='left'><b>MÉDIA DIÁRIA</b></td><td>[Mvol0]</td><td></td>");
            gerarHtml.Append("<td>[Mvol1]</td><td></td><td>[Mvol2]</td><td></td><td>[Mvol3]</td><td></td><td>[Mvol4]</td><td></td><td>[Mvol5]</td>");
            gerarHtml.Append("<td></td><td>[Mvol6]</td><td></td><td>[Mvol7]</td><td></td><td>[Mvol8]</td><td></td>");
            gerarHtml.Append("<td>[Mvol9]</td><td></td><td>[Mvol10]</td><td></td><td>[Mvol11]</td><td></td><td></td><td></td></tr></table>");

            return Ultimo12MesesReplacesTabela(gerarHtml);
        }

        public void Ultimos12MesesDadosTabela(StringBuilder gerarHtml)
        {
            for (int i = 0; i <= 10; i++)
            {
                if (i % 2 == 0)
                {
                    gerarHtml.Append("<tr align='center' style='color:#000; font-weight:normal; background-color:#FFF;'><td align='left'><b>[marcas" + i + "]</b></td>");

                    for (int d = 0; d < 12; d++)
                    {
                        gerarHtml = gerarHtml.Append("<td>[vol" + d + i + "]</td><td>[por" + d + i + "]</td>");
                    }
                    gerarHtml.Append("<td><b>[volT" + i + "]</b></td><td>[porT" + i + "]</td></tr>");

                }
                else
                {
                    gerarHtml.Append("<tr align='center' style='color:#000; font-weight:normal; background-color:#DDD;'><td align='left'><b>[marcas" + i + "]</b></td>");

                    for (int d = 0; d < 12; d++)
                    {
                        gerarHtml = gerarHtml.Append("<td>[vol" + d + i + "]</td><td>[por" + d + i + "]</td>");
                    }
                    gerarHtml.Append("<td><b>[volT" + i + "]</b></td><td>[porT" + i + "]</td></tr>");
                }
            }
        }

        public string Ultimo12MesesReplacesTabela(StringBuilder gerarHtml)
        {
            gerarHtml = gerarHtml.Replace("[EstiloTabelaBase]", EstiloTabelaBase);
            gerarHtml = gerarHtml.Replace("[SIZE]", "8");
            gerarHtml = gerarHtml.Replace("[NCelPadding]", "3");
            gerarHtml = gerarHtml.Replace("[CorCabecalho]", "#" + eConfig.RelatorioCorCabecalho);
            gerarHtml = gerarHtml.Replace("[CorBordaLinha]", "#" + eConfig.RelatorioCorLinhaAlternada);

            string[] Ultimos12Meses_dia_meses_Tvol_Tpor = new string[12];
            string[] Ultimos12Meses_marca_Tvol_Tpor = new string[11];
            string[,] Ultimos12Meses_vol_por = new string[11, 12];

            string TvolT = "1.000";

            for (int i = 0; i < Ultimos12Meses_dia_meses_Tvol_Tpor.Length; i++)
            {
                Ultimos12Meses_dia_meses_Tvol_Tpor[i] = "Mês N°" + i.ToString();
                gerarHtml = gerarHtml.Replace("[mes" + i + "]", Ultimos12Meses_dia_meses_Tvol_Tpor[i]);
                Ultimos12Meses_dia_meses_Tvol_Tpor[i] = "Dia N°" + i.ToString();
                gerarHtml = gerarHtml.Replace("[dia" + i + "]", Ultimos12Meses_dia_meses_Tvol_Tpor[i]);
                Ultimos12Meses_dia_meses_Tvol_Tpor[i] = (i + 100).ToString();
                gerarHtml = gerarHtml.Replace("[Tvol" + i + "]", Ultimos12Meses_dia_meses_Tvol_Tpor[i]);
                Ultimos12Meses_dia_meses_Tvol_Tpor[i] = (i + 50).ToString();
                gerarHtml = gerarHtml.Replace("[Mvol" + i + "]", Ultimos12Meses_dia_meses_Tvol_Tpor[i]);

            }

            for (int i = 0; i < Ultimos12Meses_marca_Tvol_Tpor.Length; i++)
            {
                Ultimos12Meses_marca_Tvol_Tpor[i] = "Marca N°" + i.ToString();
                gerarHtml = gerarHtml.Replace("[marcas" + i + "]", Ultimos12Meses_marca_Tvol_Tpor[i]);
                Ultimos12Meses_marca_Tvol_Tpor[i] = i.ToString();
                gerarHtml = gerarHtml.Replace("[volT" + i + "]", Ultimos12Meses_marca_Tvol_Tpor[i]);
                Ultimos12Meses_marca_Tvol_Tpor[i] = i.ToString() + "%";
                gerarHtml = gerarHtml.Replace("[porT" + i + "]", Ultimos12Meses_marca_Tvol_Tpor[i]);

                for (int d = 0; d < 12; d++)
                {
                    Ultimos12Meses_vol_por[i, d] = d.ToString();
                    gerarHtml = gerarHtml.Replace("[vol" + d + i + "]", Ultimos12Meses_vol_por[i, d]);
                    Ultimos12Meses_vol_por[i, d] = d.ToString() + "%";
                    gerarHtml = gerarHtml.Replace("[por" + d + i + "]", Ultimos12Meses_vol_por[i, d]);
                }
            }
            gerarHtml = gerarHtml.Replace("[TvolT]", TvolT);

            return gerarHtml.ToString();
        }

        #endregion

        //Falta Conexão Com o Banco
        #region AreaDeInfluencia

        public string AreaDeInfluenciaHtmlTitulo()
        {
            gerarHtml = new StringBuilder();

            gerarHtml.Append("<div align='center' style='width: 1194px; display:inline-block;'>");
            gerarHtml.Append("<div align='left'style='width: 202px; display:inline; float:left; height: 245px;'>");
            gerarHtml.Append("<img src='[IMG]' alt='Smiley face' height='100' width='200' style='float:right;' /></div>");
            gerarHtml.Append("<div style='width: 807px; display:inline; float:inherit'>");
            gerarHtml.Append("<h2 align='center' style='width:800px; color:[COR]'; font-family:Tahoma; font-size:14pt; fontweight:bold;'>ACOMPANHAMENTO EMPLACAMENTO <br/> [AnualOuMensal]");
            gerarHtml.Append("[DataTitulo]<br/>ÁREA DE INFLUENCIA: [Area] / [Sigla] <br/>[Categoria]<br/>[Modalidade]</h2></div></div>");

            return ReplacesTitulo(gerarHtml.ToString());
        }

        public string AreaDeInfluenciaHtmlTabela(int NMarcas)
        {
            gerarHtml = new StringBuilder();
            gerarHtml.Append("<table border='1' [EstiloTabelaBase] id='display'><tr style='background-color:[CorCabecalho];'><td>MUNICÍPIOS</td>");
            AreaDeInfleunciaCabecalho(gerarHtml, NMarcas);
            AreaDeInfluenciaDadosTabela(gerarHtml, NMarcas);
            return AreaDeInfluenciaReplaces(gerarHtml, NMarcas);
        }

        public void AreaDeInfleunciaCabecalho(StringBuilder gerarHtml, int colunas)
        {
            for (int i = 0; i <= colunas; i++)
            {
                gerarHtml.Append("<td colspan='2'>[Marca" + i + "]</td>");
            }
            gerarHtml.Append("<td>TOTAL</td></tr><tr style='background-color:[CorCabecalho];'><td></td>");

            for (int i = 0; i <= colunas; i++)
            {
                gerarHtml = gerarHtml.Append("<td width='50px'>VOL</td><td width='50px'>%</td>");
            }
            gerarHtml.Append("<td></td></tr>");
        }

        public void AreaDeInfluenciaDadosTabela(StringBuilder gerarHtml, int colunas)
        {
            for (int d = 0; d <= 10; d++)
            {
                if (d % 2 == 0)
                {
                    gerarHtml.Append("<tr style='background-color:#FFF; color:#000; font-weight:normal;' ><td>[MUNICIPIO" + d + "]</td>");
                }
                else
                {
                    gerarHtml.Append("<tr style='background-color:#DDD; color:#000; font-weight:normal;'><td>[MUNICIPIO" + d + "]</td>");
                }

                for (int i = 0; i <= colunas; i++)
                {
                    gerarHtml.Append("<td>[vol" + d + i + "]</td><td>[por" + d + i + "]</td>");
                }
                gerarHtml.Append("<td>[Total" + d + "]</td></tr>");
            }

            gerarHtml.Append("<tr style='color:#000; font-weight:normal;'><td>TOTAL</td>");

            for (int i = 0; i <= colunas; i++)
            {
                gerarHtml.Append("<td>[Tvol" + i + "]</td><td>[Tpor" + i + "]</td>");
            }

            gerarHtml.Append("<td>[TTotal]</tr></table>");
        }

        public string AreaDeInfluenciaReplaces(StringBuilder gerarHtml, int colunas)
        {
            gerarHtml = gerarHtml.Replace("[EstiloTabelaBase]", EstiloTabelaBase);
            gerarHtml = gerarHtml.Replace("[SIZE]", "10");
            gerarHtml = gerarHtml.Replace("[NCelPadding]", "4");
            gerarHtml = gerarHtml.Replace("[CorCabecalho]", "#" + eConfig.RelatorioCorCabecalho);
            gerarHtml = gerarHtml.Replace("[CorBordaLinha]", "#" + eConfig.RelatorioCorLinhaAlternada);

            string[] AreaDeInfluencia_marca_Tvp = new string[colunas + 1];
            string[] AreaDeInfluencia_municipio_Total = new string[11];
            string[,] AreaDeInfluencia_vol_por = new string[11, colunas + 1];

            string TotalGeral = "1000";

            for (int i = 0; i < AreaDeInfluencia_marca_Tvp.Length; i++)
            {
                AreaDeInfluencia_marca_Tvp[i] = "Marca N°" + i.ToString();
                gerarHtml = gerarHtml.Replace("[Marca" + i + "]", AreaDeInfluencia_marca_Tvp[i]);
                AreaDeInfluencia_marca_Tvp[i] = (i + 100).ToString();
                gerarHtml = gerarHtml.Replace("[Tvol" + i + "]", AreaDeInfluencia_marca_Tvp[i]);
                AreaDeInfluencia_marca_Tvp[i] = (i + 50).ToString() + "%";
                gerarHtml = gerarHtml.Replace("[Tpor" + i + "]", AreaDeInfluencia_marca_Tvp[i]);
            }

            for (int i = 0; i < AreaDeInfluencia_municipio_Total.Length; i++)
            {
                AreaDeInfluencia_municipio_Total[i] = "Municipio N°" + i.ToString();
                gerarHtml = gerarHtml.Replace("[MUNICIPIO" + i + "]", AreaDeInfluencia_municipio_Total[i]);
                AreaDeInfluencia_municipio_Total[i] = (i + 100).ToString();
                gerarHtml = gerarHtml.Replace("[Total" + i + "]", AreaDeInfluencia_municipio_Total[i]);

                for (int d = 0; d <= colunas; d++)
                {
                    AreaDeInfluencia_vol_por[i, d] = (d + i).ToString();
                    gerarHtml = gerarHtml.Replace("[vol" + i + d + "]", AreaDeInfluencia_vol_por[i, d]);
                    AreaDeInfluencia_vol_por[i, d] = (d + i).ToString();
                    gerarHtml = gerarHtml.Replace("[por" + i + d + "]", AreaDeInfluencia_vol_por[i, d]);
                }
            }

            gerarHtml = gerarHtml.Replace("[TTotal]", TotalGeral);

            return gerarHtml.ToString();
        }

        #endregion

        //Faltar conexão com o Banco
        #region Localidade

        public string LocalidadeHtmlTitulo()
        {
            gerarHtml = new StringBuilder();

            gerarHtml.Append("<div align='center' style='width: 1194px; display:inline-block;'>");
            gerarHtml.Append("<div align='left'style='width: 202px; display:inline; float:left; height: 245px;'>");
            gerarHtml.Append("<img src='[IMG]' alt='Smiley face' height='100' width='200' style='float:right;' /></div>");
            gerarHtml.Append("<div style='width: 807px; display:inline; float:inherit'>");
            gerarHtml.Append("<h2 align='center' style='width:800px; color:[COR]'; font-family:Tahoma; font-size:14pt; fontweight:bold;'>EMPLACAMENTO POR [Abrangencia]");
            gerarHtml.Append("<br/>[DataTitulo]<br/>AUTOMÓVEIS + COMERCIAIS LEVES <br/>[Modalidade]</h2></div></div>");

            return ReplacesTitulo(gerarHtml.ToString());
        }

        public string LocalidadeHtmlTabela(int colunas)
        {
            gerarHtml = new StringBuilder();

            gerarHtml.Append("<table border='1' width='1200px' [EstiloTabelaBase] id='display'><tr style='background-color:[CorCabecalho];'><td rowspan='2' width='200px'>[Abrangencia]</td>");
            LocalidadeCabecalho(gerarHtml, colunas);
            LocalidadeDadosTabela(gerarHtml, colunas);
            return LocalidadeReplaces(gerarHtml, false, colunas);
        }

        public void LocalidadeCabecalho(StringBuilder gerarHtml, int colunas)
        {
            for (int i = 0; i <= colunas; i++)
            {
                gerarHtml.Append("<td colspan='2'>[Marca" + i + "]</td>");
            }
            gerarHtml.Append("<td>TOTAL</td></tr><tr style='background-color:[CorCabecalho];'>");
            for (int i = 0; i <= colunas; i++)
            {
                gerarHtml.Append("<td width='90px'>VOL</td><td width='50px'>%</td>");
            }
            gerarHtml.Append("<td>VOL</td></tr>");
        }

        public void LocalidadeDadosTabela(StringBuilder gerarHtml, int colunas)
        {
            for (int d = 0; d < colunas; d++)
            {
                gerarHtml.Append("<tr style='background-color:#DDD; color:#000; font-weight:normal; border-color:#000; border-top:2px solid #000;'>");
                gerarHtml.Append("<td>[NomeAbrangencia" + d + "]</td>");

                for (int i = 0; i <= colunas; i++)
                {
                    gerarHtml.Append("<td style='border-left-style:dashed; border-right-style:dashed;'>[vol" + d + i + "]</td>");
                    gerarHtml.Append("<td style='border-left-style:dashed; border-right-style:dashed;'><b>[por" + d + i + "]</b></td>");
                }
                gerarHtml.Append("<td>[Total" + d + "]</td></tr>");
            }

            gerarHtml.Append("</table>");
            gerarHtml.Append(LocalidadeHtmlTotal(colunas));
        }

        public string LocalidadeHtmlTotal(int colunas)
        {
            gerarHtml = new StringBuilder();

            gerarHtml.Append("<br/><br/><table border='1' width='1200px' [EstiloTabelaBase]><tr style='background-color:[CorCabecalho];'><td width='200px'></td>");
            for (int i = 0; i <= colunas; i++)
            {
                gerarHtml.Append("<td>[Marca" + i + "]</td>");
            }
            gerarHtml.Append("<td>TOTAL</td><tr style='color:#000; font-weight:normal;'><td><b>TOTAL</b></td>");
            for (int i = 0; i <= colunas; i++)
            {
                gerarHtml.Append("<td><b>[TTotal" + i + "]</b></td>");
            }
            gerarHtml.Append("<td><b>[TTotal]</b></td></tr></table>");

            return LocalidadeReplaces(gerarHtml, true, colunas);
        }

        public string LocalidadeReplaces(StringBuilder gerarHtml, bool total, int colunas)
        {
            gerarHtml = gerarHtml.Replace("[EstiloTabelaBase]", EstiloTabelaBase);
            gerarHtml = gerarHtml.Replace("[SIZE]", "10");
            gerarHtml = gerarHtml.Replace("[CorCabecalho]", "#" + eConfig.RelatorioCorCabecalho);

            if (total)
            {
                gerarHtml = gerarHtml.Replace("[NCelPadding]", "4");
                gerarHtml = gerarHtml.Replace("[CorBordaLinha]", "#" + eConfig.RelatorioCorLinhaAlternada);
            }
            else
            {
                gerarHtml = gerarHtml.Replace("[NCelPadding]", "10");
                gerarHtml = gerarHtml.Replace("[CorBordaLinha]", "#FFF");
            }

            string[] Localidade_marca_TTotal = new string[colunas + 1];
            string[] Localidade_nomeAbragencia_Total = new string[6];

            string[,] Localidade_vol_por = new string[6, colunas + 1];

            string TotalGeral = "100";

            gerarHtml = gerarHtml.Replace("[Abrangencia]", AreaAbrangenciaLocalidade);

            for (int d = 0; d < 6; d++)
            {

                Localidade_nomeAbragencia_Total[d] = "Nome Abragencia N°" + d.ToString();
                gerarHtml = gerarHtml.Replace("[NomeAbrangencia" + d + "]", Localidade_nomeAbragencia_Total[d]);
                Localidade_nomeAbragencia_Total[d] = (d + 28).ToString();
                gerarHtml = gerarHtml.Replace("[Total" + d + "]", Localidade_nomeAbragencia_Total[d]);

                for (int i = 0; i < Localidade_marca_TTotal.Length; i++)
                {
                    if (d.Equals(0))
                    {
                        Localidade_marca_TTotal[i] = "Marca N°" + i.ToString();
                        gerarHtml = gerarHtml.Replace("[Marca" + i + "]", Localidade_marca_TTotal[i]);
                        Localidade_marca_TTotal[i] = (i + 201).ToString();
                        gerarHtml = gerarHtml.Replace("[TTotal" + i + "]", Localidade_marca_TTotal[i]);
                    }

                    Localidade_vol_por[d, i] = (i + d + 12).ToString();
                    gerarHtml = gerarHtml.Replace("[vol" + d + i + "]", Localidade_vol_por[d, i]);
                    Localidade_vol_por[d, i] = (i + d + 12).ToString() + "%";
                    gerarHtml = gerarHtml.Replace("[por" + d + i + "]", Localidade_vol_por[d, i]);
                }

            }

            gerarHtml = gerarHtml.Replace("[TTotal]", TotalGeral);

            return gerarHtml.ToString();
        }

        #endregion

        //Não Implementado
        #region ModeloAno

        public string ModeloAnoHtmlTitulo()
        {
            gerarHtml = new StringBuilder();



            return gerarHtml.ToString();
        }

        public string ModeloAnoHtmlTabela(int colunas)
        {
            gerarHtml = new StringBuilder();

            gerarHtml.Append("<table border='1' style='border-collapse: collapse; text-align:center;' cellpadding='30' id='display'>");
            gerarHtml.Append("<tr><td style='font-size:15pt; font-weight:bold;' width='300px'>MODELOS</td>");
            ModeloAnoCabecalho(gerarHtml, colunas);
            ModeloAnoDadosTabela(gerarHtml, colunas, 3);

            return gerarHtml.ToString();
        }

        private void ModeloAnoCabecalho(StringBuilder gerarHtml, int colunas)
        {
            for (int i = 0; i <= colunas; i++)
            {
                if (i.Equals(colunas))
                {
                    gerarHtml.Append("<td colspan='2' style='background-color:#DED9C5; font-size:15pt; font-weight:bold;'>[Ano" + i + "] Até [AteMes]</td>");
                }
                else
                {
                    gerarHtml.Append("<td colspan='2' style='font-size:15pt; font-weight:bold;'>[Ano" + i + "]</td>");
                }
            }

            gerarHtml.Append("</tr>");
        }

        private void ModeloAnoDadosTabela(StringBuilder gerarHtml, int colunas, int linhas)
        {
            gerarHtml.Append("<tr><td><table border='1' style='border-collapse: collapse;>");
            for (int i = 0; i <= linhas; i++)
            {
                if (i.Equals(linhas))
                {
                    gerarHtml.Append("<tr style='background-color:#D3D3D3;'>");

                }
                else
                {
                    gerarHtml.Append("<tr>");
                }

                gerarHtml.Append("<td>[Modelo" + i + "]</td></tr>");
            }
            gerarHtml.Append("</table></td></tr>");
        }

        #endregion

        //Falta Conexão Com o Banco
        #region MarcaAno

        public string MarcaAnoTitulo()
        {
            gerarHtml = new StringBuilder();

            gerarHtml.Append("<div align='center' style='width: 1194px; display:inline-block;'>");
            gerarHtml.Append("<div align='left'style='width: 202px; display:inline; float:left; height: 245px;'>");
            gerarHtml.Append("<img src='[IMG]' alt='Smiley face' height='100' width='200' style='float:right;' /></div>");
            gerarHtml.Append("<div style='width: 807px; display:inline; float:inherit'>");
            gerarHtml.Append("<h3 align='center' style='width:800px; color:[COR]'; font-family:Tahoma; font-size:14pt; font-weight:bold;'>ACOMPANHAMENTO EMPLACAMENTO ");
            gerarHtml.Append("<br/>ACUMULADO [ValorAcumulado]<br/>[ABRANGENCIA]<br/>[Categoria]<br/>[Modalidade]</h3></div></div>");

            return ReplacesTitulo(gerarHtml.ToString());
        }

        public string MarcaAnoHtmlTabela(int colunas)
        {
            gerarHtml = new StringBuilder();

            gerarHtml.Append("<table border='1' style='border-collapse: collapse; border-color:#D3D3D3; text-align:center; font-weight:bold; font-family: Tahoma,sans-serif;' cellpadding='4' id='display'>");
            gerarHtml.Append("<tr><td colspan='" + colunas + 1 + "' style='color:#FFF; background-color:#808080; font-size:14pt;'>");
            gerarHtml.Append("EMPLACAMENTO POR MARCA TOTAL</td></tr>");
            MarcaAnoCabecalho(gerarHtml, colunas);
            MarcaAnoDadosTabela(gerarHtml, colunas);

            return MarcaAnoReplaces(gerarHtml, colunas);
        }

        private void MarcaAnoCabecalho(StringBuilder gerarHtml, int colunas)
        {
            gerarHtml.Append("<tr><td width='300px'>MARCA</td>");
            for (int i = 0; i <= colunas; i++)
            {
                if (i.Equals(colunas))
                {
                    gerarHtml.Append("<td colspan='2' style='background-color:#DED9C5;'>[Ano" + i + "]</td>");
                }
                else
                {
                    gerarHtml.Append("<td colspan='2'>[Ano" + i + "]</td>");
                }
            }
            gerarHtml.Append("</tr><tr style='border-color:#FFF;'><td></td>");
            for (int i = 0; i <= colunas; i++)
            {
                if (i.Equals(colunas))
                {
                    gerarHtml.Append("<td width='100px' style='background-color:#DED9C5;'>VOL</td><td width='100px' style='background-color:#DED9C5;'>%</td>");
                }
                else
                {
                    gerarHtml.Append("<td width='100px'>VOL</td><td width='100px'>%</td>");
                }

            }
            gerarHtml.Append("</tr>");
        }

        private void MarcaAnoDadosTabela(StringBuilder gerarHtml, int colunas)
        {
            for (int i = 0; i < 20; i++)
            {
                if (i % 2 == 0)
                {
                    gerarHtml.Append("<tr style='border-color:#FFF; background-color:#DDD; font-weight:normal;'>");
                }
                else
                {
                    gerarHtml.Append("<tr style='border-color:#FFF; background-color:#FFF; font-weight:normal;'>");
                }
                gerarHtml.Append("<td align='left'>[Marca" + i + "]</td>");
                for (int d = 0; d <= colunas; d++)
                {
                    if (d.Equals(colunas))
                    {
                        gerarHtml.Append("<td style='background-color:#DED9C5;'>[vol" + i + d + "]</td>");
                        gerarHtml.Append("<td style='background-color:#DED9C5;'>[por" + i + d + "]</td>");
                    }
                    else
                    {
                        gerarHtml.Append("<td>[vol" + i + d + "]</td><td>[por" + i + d + "]</td>");
                    }
                }
                gerarHtml.Append("</tr>");
            }
            gerarHtml.Append("<tr style='border-color:#D3D3D3; background-color:#FFF; font-weight:bold;'><td>TOTAL</td>");
            for (int i = 0; i <= colunas; i++)
            {
                if (i.Equals(colunas))
                {
                    gerarHtml.Append("<td style='background-color:#DED9C5;'>[Tvol" + i + "]</td><td style='background-color:#DED9C5;'></td>");
                }
                else
                {
                    gerarHtml.Append("<td>[Tvol" + i + "]</td><td></td>");
                }
            }
        }

        private string MarcaAnoReplaces(StringBuilder gerarHtml, int colunas)
        {
            string[] MarcaAno_Tvol_Ano = new string[colunas + 1];

            string[] MarcaAno_Marcas = new string[20];
            string[,] MarcaAno_vol_por = new string[20, colunas + 1];

            for (int i = 0; i <= colunas; i++)
            {
                MarcaAno_Tvol_Ano[i] = "201" + i.ToString();
                gerarHtml = gerarHtml.Replace("[Ano" + i + "]", MarcaAno_Tvol_Ano[i]);
                MarcaAno_Tvol_Ano[i] = (i * 296).ToString();
                gerarHtml = gerarHtml.Replace("[Tvol" + i + "]", MarcaAno_Tvol_Ano[i]);
            }

            for (int i = 0; i < MarcaAno_Marcas.Length; i++)
            {
                MarcaAno_Marcas[i] = "Marca N°" + i.ToString();
                gerarHtml = gerarHtml.Replace("[Marca" + i + "]", MarcaAno_Marcas[i]);

                for (int d = 0; d <= colunas; d++)
                {
                    MarcaAno_vol_por[i, d] = (i * 88).ToString();
                    gerarHtml = gerarHtml.Replace("[vol" + i + d + "]", MarcaAno_vol_por[i, d]);
                    MarcaAno_vol_por[i, d] = (i * 44).ToString() + "%";
                    gerarHtml = gerarHtml.Replace("[por" + i + d + "]", MarcaAno_vol_por[i, d]);
                }
            }

            return gerarHtml.ToString();
        }

        #endregion

        //Não Implementado
        #region ModeloMes



        #endregion

        //Falta Conexão Com o Banco
        #region RankingGrupoModelos

        public string RankingGrupoModelosHtmlTitulo()
        {
            gerarHtml = new StringBuilder();


            gerarHtml.Append("<div align='center' style='width: 1194px; display:inline-block;'>");
            gerarHtml.Append("<div align='left'style='width: 202px; display:inline; float:left; height: 245px;'>");
            gerarHtml.Append("<img src='[IMG]' alt='Smiley face' height='100' width='200' style='float:right;' /></div>");
            gerarHtml.Append("<div style='width: 807px; display:inline; float:inherit'>");
            gerarHtml.Append("<h3 align='center' style='width:800px; color:[COR]'; font-family:Tahoma; font-size:14pt; fontweight:bold;'>RANKING DE VENDAS POR MODELO DE VEÍCULO - ");
            gerarHtml.Append("[DiaUtil]<br/>[DataTitulo]<br/>[Categoria]<br/>[ABRANGENCIA]</h3></div></div>");

            return ReplacesTitulo(gerarHtml.ToString());
        }

        public string RankingGrupoModelosHtmlTabela(int colunas)
        {
            gerarHtml = new StringBuilder();

            gerarHtml.Append("<div style='display:table; width:100%;'>");
            gerarHtml.Append("<div style='float:left;  margin:10px;'>");
            RankingGrupoModelosTabela(gerarHtml, true);
            gerarHtml.Append("</div>");
            gerarHtml.Append("<div style='float:right;  margin:10px;'>");
            RankingGrupoModelosTabela(gerarHtml, false);
            gerarHtml.Append("</div></div>");

            return gerarHtml.ToString();
        }

        private string RankingGrupoModelosTabela(StringBuilder gerarHtml, bool tabela)
        {
            gerarHtml.Append("<table [EstiloTabelaBase]><tr style='background-color:[CorCabecalho]; width:45%;' id='display'><td colspan='6' [borda]>[MesOuAno]</td>");
            gerarHtml.Append("</tr><tr style='background-color:[CorCabecalho];'><td width='150' [borda]>MODELO</td>");
            gerarHtml.Append("<td width='75' [borda]>VAREJO</td><td width='75' [borda]>DIRETA</td>");
            gerarHtml.Append("<td width='75' [borda]>TOTAL</td><td width='75' [borda]>%</td><td width='75' [borda]>CLASSIF.</td>");

            for (int i = 0; i < 50; i++)
            {
                gerarHtml.Append("<tr style='color:#000; border:0; font-weight:normal;'><td align='left'>[Modelo" + i + "]</td><td>[Varejo" + i + "]</td>");
                gerarHtml.Append("<td>[Direta" + i + "]</td><td>[Total" + i + "]</td><td>[Por" + i + "]</td><td>[Class" + i + "]</td></tr>");
            }

            gerarHtml.Append("<tr style='color:#000; border-top:2px solid;' border='0'><td align='left'>TOTAL</td>");
            gerarHtml.Append("<td>[T0]</td><td>[T1]</td><td>[T2]</td><td></td><td></td></tr></table>");

            return RankingGrupoModelosReplaces(gerarHtml, tabela);
        }

        private string RankingGrupoModelosReplaces(StringBuilder gerarHtml, bool tabela)
        {
            gerarHtml = gerarHtml.Replace("[EstiloTabelaBase]", EstiloTabelaBase);
            gerarHtml = gerarHtml.Replace("[SIZE]", "7");
            gerarHtml = gerarHtml.Replace("[CorCabecalho]", "#" + eConfig.RelatorioCorCabecalho);
            gerarHtml = gerarHtml.Replace("[NCelPadding]", "3");
            gerarHtml = gerarHtml.Replace("[CorBordaLinha]", "#" + eConfig.RelatorioCorLinhaAlternada);
            gerarHtml = gerarHtml.Replace("[borda]", "style='border:1px solid #D3D3D3;'");

            string[] RankingGrupoModelos = new string[50];
            string[] TToatl = new string[3];

            if (tabela)
            {
                for (int i = 0; i < 50; i++)
                {
                    RankingGrupoModelos[i] = ("Modelo N°" + i.ToString()).ToUpper();
                    gerarHtml = gerarHtml.Replace("[Modelo" + i + "]", RankingGrupoModelos[i]);
                    RankingGrupoModelos[i] = i.ToString();
                    gerarHtml = gerarHtml.Replace("[Class" + i + "]", RankingGrupoModelos[i]);
                    RankingGrupoModelos[i] = (i * 292).ToString();
                    gerarHtml = gerarHtml.Replace("[Varejo" + i + "]", RankingGrupoModelos[i]);
                    RankingGrupoModelos[i] = (i * 222).ToString();
                    gerarHtml = gerarHtml.Replace("[Direta" + i + "]", RankingGrupoModelos[i]);
                    RankingGrupoModelos[i] = (i * 465).ToString();
                    gerarHtml = gerarHtml.Replace("[Total" + i + "]", RankingGrupoModelos[i]);
                    RankingGrupoModelos[i] = (i * 2).ToString() + "%";
                    gerarHtml = gerarHtml.Replace("[Por" + i + "]", RankingGrupoModelos[i]);
                }

                for (int i = 0; i < 3; i++)
                {
                    TToatl[i] = (i * 597).ToString();
                    gerarHtml = gerarHtml.Replace("[T" + i + "]", TToatl[i]);

                }

                gerarHtml = gerarHtml.Replace("[MesOuAno]", "SETEMBRO/2016");
            }
            else
            {
                for (int i = 0; i < 50; i++)
                {
                    RankingGrupoModelos[i] = ("Modelo N°" + i.ToString()).ToUpper();
                    gerarHtml = gerarHtml.Replace("[Modelo" + i + "]", RankingGrupoModelos[i]);
                    RankingGrupoModelos[i] = i.ToString();
                    gerarHtml = gerarHtml.Replace("[Class" + i + "]", RankingGrupoModelos[i]);
                    RankingGrupoModelos[i] = (i * 292).ToString();
                    gerarHtml = gerarHtml.Replace("[Varejo" + i + "]", RankingGrupoModelos[i]);
                    RankingGrupoModelos[i] = (i * 222).ToString();
                    gerarHtml = gerarHtml.Replace("[Direta" + i + "]", RankingGrupoModelos[i]);
                    RankingGrupoModelos[i] = (i * 465).ToString();
                    gerarHtml = gerarHtml.Replace("[Total" + i + "]", RankingGrupoModelos[i]);
                    RankingGrupoModelos[i] = (i * 2).ToString() + "%";
                    gerarHtml = gerarHtml.Replace("[Por" + i + "]", RankingGrupoModelos[i]);
                }

                for (int i = 0; i < 3; i++)
                {
                    TToatl[i] = (i * 597).ToString();
                    gerarHtml = gerarHtml.Replace("[T" + i + "]", TToatl[i]);

                }

                gerarHtml = gerarHtml.Replace("[MesOuAno]", "OUTUBRO/2016");
            }

            return gerarHtml.ToString();
        }

        #endregion

        //Falta Conexão Com o Banco
        #region SegmentoMesAno

        public string SegmentoMesAnoHtmlTitulo()
        {
            gerarHtml = new StringBuilder();

            gerarHtml.Append("<div align='center' style='width: 1194px; display:inline-block;'>");
            gerarHtml.Append("<div align='left'style='width: 202px; display:inline; float:left; height: 245px;'>");
            gerarHtml.Append("<img src='[IMG]' alt='Smiley face' height='100' width='200' style='float:right;' /></div>");
            gerarHtml.Append("<div style='width: 807px; display:inline; float:inherit'>");
            gerarHtml.Append("<h2 align='center' style='width:800px; color:[COR]'; font-family:Tahoma; font-size:14pt; fontweight:bold;'>MERCADO DE VEÍCULOS POR SEGMENTO");
            gerarHtml.Append("<br/>[DataTitulo]<br/>[Categoria]<br/>[ABRANGENCIA]</h2></div></div>");

            return ReplacesTitulo(gerarHtml.ToString());
        }

        public string SegmentoMesAnoHtmlTabela(int NSegmento, int Marcas, int RegisMarca)
        {
            gerarHtml = new StringBuilder();

            gerarHtml.Append("<table [EstiloTabelaBase] id='display'><tr style='background-color:[CorCabecalho]; border:1px solid;'><td [B]></td><td width='150' [B]></td><td colspan='7' [B]>[Mes]</td><td [B]></td>");
            gerarHtml.Append("<td colspan='7' [B]>[MesAno]</td></tr><tr style='background-color:[CorCabecalho]; border:1px solid;'><td [B]></td><td [B]></td><td colspan='3' [B]>[Ano0]</td><td colspan='3' [B]>[Ano1]</td>");
            gerarHtml.Append("<td [B]></td><td [B]></td><td colspan='3' [B]>[Ano2]</td><td colspan='3' [B]>[Ano3]</td><td [B]></td></tr><tr style='background-color:[CorCabecalho]; border:1px solid;'>");
            gerarHtml.Append("<td width='50' [B]>SEG</td><td [B]>MODELO</td><td width='70' [B]>VOL</td><td width='70' [B]>% MERC</td><td width='70' [B]>% SEG</td><td width='70' [B]>VOL</td><td width='70' [B]>% MERC</td>");
            gerarHtml.Append("<td width='70' [B]>% SEG</td><td width='70' [B]>DIF %</td><td [B]'></td><td width='70' [B]>VOL.</td><td width='70' [B]>% MERC</td>");
            gerarHtml.Append("<td width='70' [B]>% SEG</td><td width='70' [B]>VOL.</td><td width='70' [B]>% MERC</td><td [B]>% SEG</td><td width='70' [B]>DIF %</td></tr>");

            SegmentoMesAnoDadosTabela(gerarHtml, NSegmento, Marcas, RegisMarca);


            return SegmentoMesAnoReplaces(gerarHtml, NSegmento, Marcas, RegisMarca);
        }

        private void SegmentoMesAnoDadosTabela(StringBuilder gerarHtml, int NSegmento, int Marcas, int RegisMarca)
        {
            for (int i = 0; i < NSegmento; i++)
            {
                gerarHtml.Append("<tr style='border-top:2px solid #000;'><td colspan='17' style='border:0; color:#000;' align='left'><b>[Letra" + i + "]</b></td></tr>");
                for (int c = 0; c < Marcas; c++)
                {
                    for (int d = 0; d < RegisMarca; d++)
                    {
                        gerarHtml.Append("<tr style='color:#000; font-weight:normal;'><td></td><td align='left'>[Modelo" + c + d + i + "]</td><td align='right'>[0Vol" + c + d + i + "]</td>");
                        gerarHtml.Append("<td>[0Merc" + c + d + i + "]</td><td>[0Seg" + c + d + i + "]</td><td>[1Vol" + c + d + i + "]</td><td>[1Merc" + c + d + i + "]</td><td>[1Seg" + c + d + i + "]</td>");
                        gerarHtml.Append("<td>[1Dif" + c + d + i + "]</td><td border='0'></td><td>[2Vol" + c + d + i + "]</td><td>[2Merc" + c + d + i + "]</td>");
                        gerarHtml.Append("<td>[2Seg" + c + d + i + "]</td><td>[3Vol" + c + d + i + "]</td><td>[3Merc" + c + d + i + "]</td><td>[3Seg" + c + d + i + "]</td>");
                        gerarHtml.Append("<td>[3Dif" + c + d + i + "]</td></tr>");
                    }
                    gerarHtml.Append("<tr style='color:#000;'><td></td><td align='left'>[" + c + "TotalMarca" + i + "]</td><td align='right'>[0TVol" + c + i + "]</td><td>[0TMerc" + c + i + "]</td>");
                    gerarHtml.Append("<td>[0TSeg" + c + i + "]</td><td>[1TVol" + c + i + "]</td><td>[1TMerc" + c + i + "]</td><td>[1TSeg" + c + i + "]</td>");
                    gerarHtml.Append("<td>[1TDif" + c + i + "]</td><td border='0'></td><td>[2TVol" + c + i + "]</td><td>[2TMerc" + c + i + "]</td>");
                    gerarHtml.Append("<td>[2TSeg" + c + i + "]</td><td>[3TVol" + c + i + "]</td><td>[3TMerc" + c + i + "]</td><td>[3TSeg" + c + i + "]</td>");
                    gerarHtml.Append("<td>[3TDif" + c + i + "]</td></tr>");
                }

                gerarHtml.Append("<tr style='border-top:1px solid #000; color:#000; font-weight:normal;'><td></td><td align='left'>TOTAL [Letra" + i + "]</td>");
                gerarHtml.Append("<td align='right'>[0TTVol" + i + "]</td><td>[0TTMerc" + i + "]</td><td>[0TTSeg" + i + "]</td><td>[1TTVol" + i + "]</td>");
                gerarHtml.Append("<td>[1TTMerc" + i + "]</td><td>[1TTSeg" + i + "]</td><td>[1TTDif" + i + "]</td><td border='0'></td><td>[2TTVol" + i + "]</td>");
                gerarHtml.Append("<td>[2TTMerc" + i + "]</td><td>[2TTSeg" + i + "]</td><td>[3TTVol" + i + "]</td><td>[3TTMerc" + i + "]</td><td>[3TTSeg" + i + "]</td>");
                gerarHtml.Append("<td>[3TTDif" + i + "]</td></tr>");
            }
        }

        public string SegmentoMesAnoReplaces(StringBuilder gerarHtml, int NSegmento, int Marcas, int RegisMarca)
        {
            gerarHtml = gerarHtml.Replace("[EstiloTabelaBase]", EstiloTabelaBase);
            gerarHtml = gerarHtml.Replace("[SIZE]", "8");
            gerarHtml = gerarHtml.Replace("[B]", "style='border:1px solid; border-color:#" + eConfig.RelatorioCorLinhaAlternada + ";'");
            gerarHtml = gerarHtml.Replace("[NCelPadding]", "2");
            gerarHtml = gerarHtml.Replace("[CorCabecalho]", "#" + eConfig.RelatorioCorCabecalho);

            //////////

            string Mes = "OUTUBRO";
            string MesAno = "JANEIRO/OUTUBRO";

            string[] Ano = new string[4];

            string[] LetraTToltal = new string[NSegmento];

            string[,] Modelo = new string[Marcas, RegisMarca];
            string[,] SegmentoMesAno = new string[Marcas, NSegmento];

            //////////////////

            //AnoCabecalho
            for (int i = 0; i < 4; i++)
            {
                Ano[i] = "2015";
                gerarHtml = gerarHtml.Replace("[Ano" + i + "]", Ano[i]);
            }

            gerarHtml = gerarHtml.Replace("[Mes]", Mes);
            gerarHtml = gerarHtml.Replace("[MesAno]", MesAno);

            //Replaces Das Letras Segmento 
            for (int i = 0; i < LetraTToltal.Length; i++)
            {
                LetraTToltal[i] = i.ToString();
                gerarHtml = gerarHtml.Replace("[Letra" + i + "]", LetraTToltal[i]);

                //Replaces Das Marcas Total 
                for (int c = 0; c < Marcas; c++)
                {
                    SegmentoMesAno[c, i] = "Total M Nº" + c + i;
                    gerarHtml = gerarHtml.Replace("[" + c + "TotalMarca" + i + "]", SegmentoMesAno[c, i]);

                    //Replaces dos Dados Totais do Modelo
                    for (int d = 0; d < 4; d++)
                    {
                        SegmentoMesAno[c, i] = (c * i + (d + 1)).ToString();
                        gerarHtml = gerarHtml.Replace("[" + d + "TVol" + c + i + "]", SegmentoMesAno[c, i]);
                        SegmentoMesAno[c, i] = (c * i - (d + 1)).ToString() + "%";
                        gerarHtml = gerarHtml.Replace("[" + d + "TMerc" + c + i + "]", SegmentoMesAno[c, i]);
                        SegmentoMesAno[c, i] = (c * i - (d + 1)).ToString() + "%";
                        gerarHtml = gerarHtml.Replace("[" + d + "TSeg" + c + i + "]", SegmentoMesAno[c, i]);
                        SegmentoMesAno[c, i] = (c * i - (d + 1)).ToString() + "%";
                        gerarHtml = gerarHtml.Replace("[" + d + "TDif" + c + i + "]", SegmentoMesAno[c, i]);

                        LetraTToltal[i] = ((i + 1) * 23).ToString();
                        gerarHtml = gerarHtml.Replace("[" + d + "TTVol" + i + "]", LetraTToltal[i]);
                        LetraTToltal[i] = ((i + 1) * 23).ToString() + "%";
                        gerarHtml = gerarHtml.Replace("[" + d + "TTMerc" + i + "]", LetraTToltal[i]);
                        LetraTToltal[i] = ((i + 1) * 23).ToString() + "%";
                        gerarHtml = gerarHtml.Replace("[" + d + "TTSeg" + i + "]", LetraTToltal[i]);
                        LetraTToltal[i] = ((i + 1) * 23).ToString() + "%";
                        gerarHtml = gerarHtml.Replace("[" + d + "TTDif" + i + "]", LetraTToltal[i]);

                        //Replaces dos Dados por Modelo
                        for (int f = 0; f < RegisMarca; f++)
                        {
                            SegmentoMesAno[c, i] = (f + i + (c * d)).ToString();
                            gerarHtml = gerarHtml.Replace("[" + d + "Vol" + c + f + i + "]", SegmentoMesAno[c, i]);
                            SegmentoMesAno[c, i] = (f + i + (c * d)).ToString();
                            gerarHtml = gerarHtml.Replace("[" + d + "Merc" + c + f + i + "]", SegmentoMesAno[c, i]);
                            SegmentoMesAno[c, i] = (f + i + (c * d)).ToString();
                            gerarHtml = gerarHtml.Replace("[" + d + "Seg" + c + f + i + "]", SegmentoMesAno[c, i]);
                            SegmentoMesAno[c, i] = (f + i + (c * d)).ToString();
                            gerarHtml = gerarHtml.Replace("[" + d + "Dif" + c + f + i + "]", SegmentoMesAno[c, i]);
                            Modelo[c, f] = "MODELO N°" + c.ToString() + f.ToString();
                            gerarHtml = gerarHtml.Replace("[Modelo" + c + f + i + "]", Modelo[c, f]);
                        }
                    }
                }
            }

            return gerarHtml.ToString();
        }

        #endregion

        //Falta Conexão Com o Banco
        #region EvolucaoMercado

        public string EvolulcaoMercadoHtmlTitulo()
        {
            gerarHtml = new StringBuilder();

            gerarHtml.Append("<div align='center' style='width: 1194px; display:inline-block;'>");
            gerarHtml.Append("<div align='left'style='width: 202px; display:inline; float:left; height: 245px;'>");
            gerarHtml.Append("<img src='[IMG]' alt='Smiley face' height='100' width='200' style='float:right;' /></div>");
            gerarHtml.Append("<div style='width: 807px; display:inline; float:inherit'>");
            gerarHtml.Append("<h2 align='center' style='width:800px; color:[COR]'; font-family:Tahoma; font-size:14pt; fontweight:bold;'>EVOLUÇÃO MERCADO DE VEÍCULOS POR SEGMENTO");
            gerarHtml.Append("<br/>PARTICIPAÇÃO % SEGMENTO E MERCADO <br/> [Modalidade]<br/>[ABRANGENCIA]</h2></div></div>");

            return ReplacesTitulo(gerarHtml.ToString());
        }

        public string EvolucaoMercadoHtmlTabela(int NSegmento, int Marcas, int RegisMarca)
        {
            gerarHtml = new StringBuilder();

            gerarHtml.Append("<table width='2800' [EstiloTabelaBase] id='display'><tr style='background-color:[CorCabecalho]; border:1px solid;'><td [B]></td><td width='150' [B]></td><td colspan='3' [B]>[Ano0]</td>");

            for (int i = 0; i < 12; i++)
            {
                gerarHtml.Append("<td colspan='3' [B]>[Mes" + i + "]</td>");
            }
            gerarHtml.Append("<td colspan='3' [B]>[Ano1]</td></tr>");
            gerarHtml.Append("<tr style='background-color:[CorCabecalho]; border:1px solid;'><td width='70' [B]>SEG</td><td [B]>MODELO</td>");
            for (int i = 0; i < 14; i++)
            {
                gerarHtml.Append("<td width='70' [B]>VOL.</td><td width='70' [B]>% MERC</td><td width='70' [B]>% SEG</td>");
            }
            gerarHtml.Append("</tr>");

            EvolucaoMercadoDadosTabela(gerarHtml, NSegmento, Marcas, RegisMarca);
            EvolucaoMercadoTotalGeral(gerarHtml, NSegmento, Marcas, RegisMarca);

            gerarHtml.Append("</table>");

            return EvolucaoMercadoReplaces(gerarHtml, NSegmento, Marcas, RegisMarca);
        }

        private void EvolucaoMercadoDadosTabela(StringBuilder gerarHtml, int NSegmento, int Marcas, int RegisMarca)
        {
            for (int i = 0; i < NSegmento; i++)
            {
                gerarHtml.Append("<tr style='border-top:2px solid #000;'><td colspan='44' style='border:0; color:#000;' align='left'><b>[Letra" + i + "]</b></td></tr>");

                for (int c = 0; c < Marcas; c++)
                {
                    for (int f = 0; f < RegisMarca; f++)
                    {
                        gerarHtml.Append("<tr style='color:#000; font-weight:normal;'><td></td><td align='left'>[Modelo" + f + c + i + "]</td>");
                        for (int d = 0; d < 14; d++)
                        {
                            gerarHtml.Append("<td align='right'>[Vol" + d + f + c + i + "]</td><td>[Merc" + d + f + c + i + "]</td><td>[Seg" + d + f + c + i + "]</td>");
                        }
                        gerarHtml.Append("</tr>");
                    }

                    gerarHtml.Append("<tr style='color:#000;'><td></td><td align='left'>[TotalMarca" + c + i + "]</td>");
                    for (int d = 0; d < 14; d++)
                    {
                        gerarHtml.Append("<td align='right'>[TVol" + d + c + i + "]</td><td>[TMerc" + d + c + i + "]</td><td>[TSeg" + d + c + i + "]</td>");
                    }
                    gerarHtml.Append("</tr>");
                }

                gerarHtml.Append("<tr style='color:#000; font-weight:normal; border-top:1px solid #000;'><td></td><td align='left'>Total [Letra" + i + "]</td>");
                for (int d = 0; d < 14; d++)
                {
                    gerarHtml.Append("<td align='right'>[TTVol" + d + i + "]</td><td>[TTMerc" + d + i + "]</td><td>[TTSeg" + d + i + "]</td>");
                }
                gerarHtml.Append("</tr>");
            }
        }

        private void EvolucaoMercadoTotalGeral(StringBuilder gerarHtml, int NSegmento, int Marcas, int RegisMarca)
        {
            gerarHtml.Append("<tr style='font-size:10pt; color:#000;'><td align='left'>TOTAL GERAL</td>");
            for (int i = 0; i < 14; i++)
            {
                gerarHtml.Append("<td align='right'>[TGVol" + i + "]</td><td>[TGMerc" + i + "]</td><td></td>");
            }
            gerarHtml.Append("</tr>");
        }

        private string EvolucaoMercadoReplaces(StringBuilder gerarHtml, int NSegmento, int Marcas, int RegisMarca)
        {
            gerarHtml = gerarHtml.Replace("[EstiloTabelaBase]", EstiloTabelaBase);
            gerarHtml = gerarHtml.Replace("[SIZE]", "7");
            gerarHtml = gerarHtml.Replace("[B]", "style='border:1px solid; border-color:#" + eConfig.RelatorioCorLinhaAlternada + ";'");
            gerarHtml = gerarHtml.Replace("[NCelPadding]", "2");
            gerarHtml = gerarHtml.Replace("[CorCabecalho]", "#" + eConfig.RelatorioCorCabecalho);

            string Ano1 = "2015";
            string Ano2 = "2016";

            string[] Meses = new string[12];
            string[] EvolucaoMercado_Letra = new string[NSegmento];
            string[] TotalMarca = new string[Marcas];

            string[,] EvolucaoMercado = new string[Marcas, RegisMarca];
            string[,] TotalEvolucaoMercado = new string[14, NSegmento];
            string[] EvolucaoMercado_TG = new string[14];

            for (int i = 0; i < Meses.Length; i++)
            {
                Meses[i] = "Mês N°" + i.ToString();
                gerarHtml = gerarHtml.Replace("[Mes" + i + "]", Meses[i]);
            }

            for (int i = 0; i < NSegmento; i++)
            {
                EvolucaoMercado_Letra[i] = i.ToString();
                gerarHtml = gerarHtml.Replace("[Letra" + i + "]", EvolucaoMercado_Letra[i]);

                for (int c = 0; c < Marcas; c++)
                {
                    TotalMarca[c] = "Total M Nº" + c.ToString();
                    gerarHtml = gerarHtml.Replace("[TotalMarca" + c + i + "]", TotalMarca[c]);

                    for (int f = 0; f < RegisMarca; f++)
                    {
                        EvolucaoMercado[c, i] = "Modelo N°" + c.ToString() + i.ToString();
                        gerarHtml = gerarHtml.Replace("[Modelo" + f + c + i + "]", EvolucaoMercado[c, i]);

                        for (int d = 0; d < 14; d++)
                        {
                            EvolucaoMercado[c, i] = d.ToString() + f.ToString() + c.ToString() + i.ToString();
                            gerarHtml = gerarHtml.Replace("[Vol" + d + f + c + i + "]", EvolucaoMercado[c, i]);
                            EvolucaoMercado[c, i] = d.ToString() + f.ToString() + c.ToString() + i.ToString() + "%";
                            gerarHtml = gerarHtml.Replace("[Merc" + d + f + c + i + "]", EvolucaoMercado[c, i]);
                            EvolucaoMercado[c, i] = d.ToString() + f.ToString() + c.ToString() + i.ToString() + "%";
                            gerarHtml = gerarHtml.Replace("[Seg" + d + f + c + i + "]", EvolucaoMercado[c, i]);

                            EvolucaoMercado[c, i] = i.ToString() + d.ToString();
                            gerarHtml = gerarHtml.Replace("[TVol" + d + c + i + "]", EvolucaoMercado[c, i]);
                            EvolucaoMercado[c, i] = i.ToString() + d.ToString() + "%";
                            gerarHtml = gerarHtml.Replace("[TMerc" + d + c + i + "]", EvolucaoMercado[c, i]);
                            EvolucaoMercado[c, i] = i.ToString() + d.ToString() + "%";
                            gerarHtml = gerarHtml.Replace("[TSeg" + d + c + i + "]", EvolucaoMercado[c, i]);

                            TotalEvolucaoMercado[d, i] = i.ToString() + d.ToString();
                            gerarHtml = gerarHtml.Replace("[TTVol" + d + i + "]", TotalEvolucaoMercado[d, i]);
                            TotalEvolucaoMercado[d, i] = i.ToString() + d.ToString() + "%";
                            gerarHtml = gerarHtml.Replace("[TTMerc" + d + i + "]", TotalEvolucaoMercado[d, i]);
                            TotalEvolucaoMercado[d, i] = i.ToString() + d.ToString() + "%";
                            gerarHtml = gerarHtml.Replace("[TTSeg" + d + i + "]", TotalEvolucaoMercado[d, i]);
                        }
                    }
                }
            }

            for (int i = 0; i < 14; i++)
            {
                EvolucaoMercado_TG[i] = ((i + 2) * 1000).ToString();
                gerarHtml = gerarHtml.Replace("[TGVol" + i + "]", EvolucaoMercado_TG[i]);
                EvolucaoMercado_TG[i] = (i + 2).ToString() + "%";
                gerarHtml = gerarHtml.Replace("[TGMerc" + i + "]", EvolucaoMercado_TG[i]);
            }

            gerarHtml = gerarHtml.Replace("[Ano0]", Ano1);
            gerarHtml = gerarHtml.Replace("[Ano1]", Ano2);

            return gerarHtml.ToString();
        }

        #endregion

        //Falta Conexão Com o Banco
        #region RankingConcessionariaGrupo

        public string RankingConcessionariaGrupoHtmlTitulo()
        {
            gerarHtml = new StringBuilder();

            gerarHtml.Append("<div align='center' style='width: 1194px; display:inline-block;'>");
            gerarHtml.Append("<div align='left'style='width: 202px; display:inline; float:left; height: 150px;'>");
            gerarHtml.Append("<img src='[IMG]' alt='Smiley face' height='100' width='200' style='float:right;' /></div>");
            gerarHtml.Append("<div style='width: 807px; display:inline; float:inherit'>");
            gerarHtml.Append("<h3 align='center' style='width:800px; color:[COR]'; font-family:Tahoma; font-size:14pt; fontweight:bold;'>[GrupoOuNao]");
            gerarHtml.Append("<br/>ACUMUDADO MÊS [ValorAcumulado]<br/>[ABRANGENCIA]<br/>[Categoria]</h3s></div></div>");

            return ReplacesTitulo(gerarHtml.ToString());
        }

        public string RankingConcessionariaGrupoHtmlTabela(bool ehGrupo, int colunas)
        {
            gerarHtml = new StringBuilder();

            gerarHtml.Append("<table border='1' [EstiloTabelaBase] id='display'><tr style='background-color:[CorCabecalho]; font-size:12pt;'>");

            if (ehGrupo)
            {
                gerarHtml.Append("<td align='left' width='100'>Pos</td><td align='left' width='150'>Código</td><td align='left' width='300'>Grupo</td>");
                gerarHtml.Append("<td align='left' width='400'>Nome</td><td align='left' width='250'>Região Operacional</td><td align='left' width='200'>Quantidade</td>");
                gerarHtml.Append("</tr>");

                RankingConcessionariaGrupoDadosTabela(gerarHtml, colunas);
            }
            else
            {
                gerarHtml.Append("<td align='right' width='100'>Pos</td><td align='left' width='150'>Código</td><td align='left' width='500'>Concessionária</td>");
                gerarHtml.Append("<td align='left' width='250'>Grupo</td><td align='left' width='250'>Região Operacionaal</td><td align='left' width='150'>Quantidade</td>");
                gerarHtml.Append("</tr>");

                RankingConcessionariaDadosTabela(gerarHtml, colunas);
            }

            return RankingConcessionariaGrupoReplaces(gerarHtml, ehGrupo, colunas);
        }

        private void RankingConcessionariaDadosTabela(StringBuilder gerarHtml, int colunas)
        {
            for (int i = 0; i < 10; i++)
            {
                gerarHtml.Append("<tr [EstiloLinha]><td align='right'>[Pos" + i + "]</td><td align='left'>[Cod" + i + "]</td><td align='left'>[Conce" + i + "]</td>");
                gerarHtml.Append("<td align='left'>[Grup" + i + "]</td><td align='left'>[Regi" + i + "]</td><td align='center'>[Quant" + i + "]</td></tr>");
            }
            gerarHtml.Append("<tr style='background:#DDDDDD; color:#000; font-weight:bold;'><td></td><td></td><td></td><td></td><td align='left'>TOTAL</td><td>[TotalG]</td></tr>");
        }

        private void RankingConcessionariaGrupoDadosTabela(StringBuilder gerarHtml, int colunas)
        {
            for (int i = 0; i < 10; i++)
            {
                gerarHtml.Append("<tr style='font-weight:normal; color:#000;'><td rolspan='4'>[Pos" + i + "]</td>");
                gerarHtml.Append("<td rolspan='4'>[Cod" + i + "]</td><td rolspan='4' align='left'>[Grup" + i + "]</td><td><table width='400' [EstiloTabelaBaseGrupo]>");
                for (int d = 0; d < 3; d++)
                {
                    gerarHtml.Append("<tr [EstiloLinha] ><td>[Nome" + d + i + "]</td></tr>");
                }
                gerarHtml.Append("<tr [EstiloLinha]><td style='background-color:[CorCabecalho];'>&nbsp</td></tr></table></td><td><table width='250' [EstiloTabelaBaseGrupo]>");
                for (int d = 0; d < 3; d++)
                {
                    gerarHtml.Append("<tr [EstiloLinha]><td>[Regiao" + d + i + "]</td></tr>");
                }
                gerarHtml.Append("<tr [EstiloLinha]><td style='background-color:[CorCabecalho]; color:#FFF; font-weight:bold;'>TOTAL</td></tr></table></td>");
                gerarHtml.Append("<td><table width='200' [EstiloTabelaBaseGrupo]>");
                for (int d = 0; d < 3; d++)
                {
                    gerarHtml.Append("<tr [EstiloLinha] ><td align='center'>[Quant" + d + i + "]</td></tr>");
                }
                gerarHtml.Append("<tr [EstiloLinha]><td style='background-color:[CorCabecalho]; color:#FFF; font-weight:bold;' align='center'>[T" + i + "]</td></tr></table></td></tr>");
            }
        }

        private string RankingConcessionariaGrupoReplaces(StringBuilder gerarHtml, bool ehGrupo, int colunas)
        {
            gerarHtml = gerarHtml.Replace("[EstiloTabelaBase]", EstiloTabelaBase);
            gerarHtml = gerarHtml.Replace("[EstiloTabelaBaseGrupo]", EstiloTabelaBaseGrupo);
            gerarHtml = gerarHtml.Replace("[SIZE]", "10");
            gerarHtml = gerarHtml.Replace("[NCelPadding]", "3");
            gerarHtml = gerarHtml.Replace("[CorCabecalho]", "#" + eConfig.RelatorioCorCabecalho);
            gerarHtml = gerarHtml.Replace("[EstiloLinha]", EstiloLinha);
            gerarHtml = gerarHtml.Replace("[CorBordaLinha]", "#" + eConfig.RelatorioCorLinhaAlternada);

            string[] RankingConcessionariaGrupo = new string[10];

            if (ehGrupo)
            {
                for (int i = 0; i < RankingConcessionariaGrupo.Length; i++)
                {
                    RankingConcessionariaGrupo[i] = i.ToString();
                    gerarHtml = gerarHtml.Replace("[Pos" + i + "]", RankingConcessionariaGrupo[i]);
                    RankingConcessionariaGrupo[i] = "12345" + i.ToString();
                    gerarHtml = gerarHtml.Replace("[Cod" + i + "]", RankingConcessionariaGrupo[i]);
                    RankingConcessionariaGrupo[i] = "Grupo " + i.ToString();
                    gerarHtml = gerarHtml.Replace("[Grup" + i + "]", RankingConcessionariaGrupo[i]);
                    RankingConcessionariaGrupo[i] = ((i + 1) * 22).ToString();
                    gerarHtml = gerarHtml.Replace("[T" + i + "]", RankingConcessionariaGrupo[i]);

                    for (int d = 0; d < 3; d++)
                    {
                        RankingConcessionariaGrupo[i] = "Nome " + d.ToString() + i.ToString();
                        gerarHtml = gerarHtml.Replace("[Nome" + d + i + "]", RankingConcessionariaGrupo[i]);
                        RankingConcessionariaGrupo[i] = "R" + i.ToString();
                        gerarHtml = gerarHtml.Replace("[Regiao" + d + i + "]", RankingConcessionariaGrupo[i]);
                        RankingConcessionariaGrupo[i] = ((i + 1) * 3).ToString();
                        gerarHtml = gerarHtml.Replace("[Quant" + d + i + "]", RankingConcessionariaGrupo[i]);
                    }
                }
            }
            else
            {
                string TG = "654";

                //Replaces da tabela Sem o filtro de agrupado
                for (int i = 0; i < RankingConcessionariaGrupo.Length; i++)
                {
                    RankingConcessionariaGrupo[i] = i.ToString();
                    gerarHtml = gerarHtml.Replace("[Pos" + i + "]", RankingConcessionariaGrupo[i]);
                    RankingConcessionariaGrupo[i] = "12345" + i.ToString();
                    gerarHtml = gerarHtml.Replace("[Cod" + i + "]", RankingConcessionariaGrupo[i]);
                    RankingConcessionariaGrupo[i] = "Concessionaria " + i.ToString();
                    gerarHtml = gerarHtml.Replace("[Conce" + i + "]", RankingConcessionariaGrupo[i]);
                    RankingConcessionariaGrupo[i] = "Grupo " + i.ToString();
                    gerarHtml = gerarHtml.Replace("[Grup" + i + "]", RankingConcessionariaGrupo[i]);
                    RankingConcessionariaGrupo[i] = "R" + i.ToString();
                    gerarHtml = gerarHtml.Replace("[Regi" + i + "]", RankingConcessionariaGrupo[i]);
                    RankingConcessionariaGrupo[i] = ((i + 1) * 3).ToString();
                    gerarHtml = gerarHtml.Replace("[Quant" + i + "]", RankingConcessionariaGrupo[i]);
                }

                gerarHtml = gerarHtml.Replace("[TotalG]", TG);
            }

            return gerarHtml.ToString();
        }

        #endregion

        #region VeiculosInfo

        public string VeiculosInfoHtmlTitulo()
        {
            gerarHtml = new StringBuilder();

            gerarHtml.Append("<div align='center' style='width: 1194px; display:inline-block;'>");
            gerarHtml.Append("<div align='left'style='width: 202px; display:inline; float:left; height: 150px;'>");
            gerarHtml.Append("<img src='[IMG]' alt='Smiley face' height='100' width='200' style='float:right;' /></div>");
            gerarHtml.Append("<div style='width: 807px; display:inline; float:inherit'>");
            gerarHtml.Append("<h2 align='center' style='width:800px; color:[COR]'; font-family:Tahoma; font-size:14pt; fontweight:bold;'>");
            gerarHtml.Append("Informações sobre Veiculo</h2></div></div>");

            return ReplacesTitulo(gerarHtml.ToString());
        }

        public string VeiculosInfoHtmlTabela(string chassi)
        {
            gerarHtml = new StringBuilder();

            List<eVeiculosInfo> lista = new List<eVeiculosInfo>();

            lista = GetUserVeiculosInfo(chassi);

            Regis = lista.Count();

            gerarHtml.Append("<table border='1' width='2000' [EstiloTabelaBase] id='display'><tr style='background-color:[CorCabecalho];'>");
            gerarHtml.Append("<td width='200'>CNPJ</td><td width='500'>Razão Social</td><td width='200'>Data Emplac</td>");
            gerarHtml.Append("<td width='200'>Chassi</td><td width='100'>Placa</td><td width='170'>Fabricante</td>");
            gerarHtml.Append("<td width='170'>Grupo</td><td width='250'>Modelo</td><td width='170'>Categoria</td>");
            gerarHtml.Append("<td width='170'>Segmento</td><td width='250'>Municipio</td><td width='70'>Estado</td>");
            gerarHtml.Append("<td width='150'>Ano Fabricação</td><td width='270'>Cilindrada</td></tr>");

            VeiculosInfoDadosTabela(gerarHtml, Regis);

            return VeiculosInfoReplaces(gerarHtml, Regis, lista);
        }

        private void VeiculosInfoDadosTabela(StringBuilder gerarHtml, int Regis)
        {
            for (int i = 0; i < Regis; i++)
            {
                gerarHtml.Append("<tr style='font-weight:normal; color:#000;' align='left'><td>[Cnpj" + i + "]</td><td>[RS" + i + "]</td><td>[DataE" + i + "]</td>");
                gerarHtml.Append("<td>[Chas" + i + "]</td><td>[Placa" + i + "]</td><td>[Fabri" + i + "]</td><td>[Grup" + i + "]</td><td>[Mod" + i + "]</td><td>[Categ" + i + "]</td>");
                gerarHtml.Append("<td>[Seg" + i + "]</td><td>[Muni" + i + "]</td><td>[Est" + i + "]</td><td>[AnoFabri" + i + "]</td><td>[Cilin" + i + "]</td</tr>");
            }
            gerarHtml.Append("</table>");
        }

        private string VeiculosInfoReplaces(StringBuilder gerarHtml, int Regis, List<eVeiculosInfo> lista)
        {

            eVeiculosInfo veiculos = new eVeiculosInfo();

            gerarHtml = gerarHtml.Replace("[EstiloTabelaBase]", EstiloTabelaBase);
            gerarHtml = gerarHtml.Replace("[SIZE]", "12");
            gerarHtml = gerarHtml.Replace("[CorBordaLinha]", "#" + eConfig.RelatorioCorLinhaAlternada);
            gerarHtml = gerarHtml.Replace("[NCelPadding]", "3");
            gerarHtml = gerarHtml.Replace("[CorCabecalho]", "#" + eConfig.RelatorioCorCabecalho);

            for (int i = 0; i < Regis; i++)
            {
                veiculos = lista[i];

                gerarHtml = gerarHtml.Replace("[Cnpj" + i + "]", veiculos.cd_cnpj);
                gerarHtml = gerarHtml.Replace("[RS" + i + "]", veiculos.nm_razao);
                gerarHtml = gerarHtml.Replace("[DataE" + i + "]", veiculos.dt_emplacamento);
                gerarHtml = gerarHtml.Replace("[Chas" + i + "]", veiculos.cd_chassi);
                gerarHtml = gerarHtml.Replace("[Placa" + i + "]", veiculos.cd_placa);
                gerarHtml = gerarHtml.Replace("[Fabri" + i + "]", veiculos.nm_fabricante);
                gerarHtml = gerarHtml.Replace("[Grup" + i + "]", veiculos.nm_grupo_modelo_veiculo);
                gerarHtml = gerarHtml.Replace("[Mod" + i + "]", veiculos.nm_modelo);
                gerarHtml = gerarHtml.Replace("[Categ" + i + "]", veiculos.nm_sub_segmento);
                gerarHtml = gerarHtml.Replace("[Seg" + i + "]", veiculos.nm_segmento);
                gerarHtml = gerarHtml.Replace("[Muni" + i + "]", veiculos.nm_municipio);
                gerarHtml = gerarHtml.Replace("[Est" + i + "]", veiculos.nm_estado);
                gerarHtml = gerarHtml.Replace("[AnoFabri" + i + "]", veiculos.nm_ano_fabricacao);
                gerarHtml = gerarHtml.Replace("[Cilin" + i + "]", veiculos.nm_cilindrada);
            }

            return gerarHtml.ToString();
        }

        public List<eVeiculosInfo> GetUserVeiculosInfo(string chassi)
        {
            try
            {
                List<eVeiculosInfo> listaVeiculos = new List<eVeiculosInfo>();

                DAO.dRelatorios db = new DAO.dRelatorios();

                return db.GetUserRegistroVeiculos(chassi);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        //Acessos
        public List<eAcessos> GetUserRegistroAcessos(eAcessos acesso)
        {
            try
            {
                dRelatorios db = new dRelatorios();
                return db.GetUserRegistroAcesso(acesso);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region AcessoUsuario 

        public string AcessoUsuarioHtmlTitulo(eAcessos acesso)
        {
            gerarHtml = new StringBuilder();

            gerarHtml.Append("<div align='center' style='width: 1194px; display:inline-block;'>");
            gerarHtml.Append("<div align='left'style='width: 202px; display:inline; float:left; height: 150px;'>");
            gerarHtml.Append("<img src='[IMG]' alt='Smiley face' height='100' width='200' style='float:right;' /></div>");
            gerarHtml.Append("<div style='width: 807px; display:inline; float:inherit'>");
            gerarHtml.Append("<h2 align='center' style='width:800px; color:[COR]'; font-family:Tahoma; font-size:14pt; fontweight:bold;'>");
            gerarHtml.Append("ACESSOS POR USUÁRIO<br/> DE " + acesso.DiaDe + " DE " + NomeMes(acesso.MesDe) + " - " + acesso.AnoDe + "<br/>");
            gerarHtml.Append("ATÉ " + acesso.Dia + " DE " + NomeMes(acesso.Mes) + " - " + acesso.Ano + "<br/>(Login na ferramenta)</h2></div></div>");

            return ReplacesTitulo(gerarHtml.ToString());
        }

        public string AcessoUsuarioHtmlTabela(eAcessos acesso)
        {
            gerarHtml = new StringBuilder();

            List<eAcessos> listaAcesso = new List<eAcessos>();
            listaAcesso = GetUserRegistroAcessos(acesso);

            Regis = listaAcesso.Count();

            gerarHtml.Append("<table border='1' [EstiloTabelaBase] id='display'><tr style='background-color:[CorCabecalho];'>");
            gerarHtml.Append("<td width='400'>NOME</td><td width='400'>USUÁRIOS</td><td width='400'>EMPRESA</td><td width='100'>VOL</td></tr>");

            AcessoUsuarioDadosTabela(gerarHtml, Regis);

            return AcessoUsuarioReplces(gerarHtml, Regis, listaAcesso);
        }

        private void AcessoUsuarioDadosTabela(StringBuilder gerarHtml, int Regis)
        {
            for (int i = 0; i < Regis; i++)
            {
                gerarHtml.Append("<tr style='color:#000; font-weight:normal;'><td>[Nome" + i + "]</td><td>[User" + i + "]</td><td>[Empre" + i + "]</td>");
                gerarHtml.Append("<td><a href='javascript:chamarPagina();' style='color:000; text-decoration:none;'>[Vol" + i + "]</a></td></tr>");


            }
        }

        private string AcessoUsuarioReplces(StringBuilder gerarHtml, int Regis, List<eAcessos> lista)
        {
            gerarHtml = gerarHtml.Replace("[EstiloTabelaBase]", EstiloTabelaBase);
            gerarHtml = gerarHtml.Replace("[SIZE]", "10");
            gerarHtml = gerarHtml.Replace("[NCelPadding]", "2");
            gerarHtml = gerarHtml.Replace("[CorBordaLinha]", "#" + eConfig.RelatorioCorLinhaAlternada);
            gerarHtml = gerarHtml.Replace("[CorCabecalho]", "#" + eConfig.RelatorioCorCabecalho);

            eAcessos acesso = new eAcessos();

            for (int i = 0; i < Regis; i++)
            {
                acesso = lista[i];

                gerarHtml = gerarHtml.Replace("[Nome" + i + "]", acesso.Nome);
                gerarHtml = gerarHtml.Replace("[User" + i + "]", acesso.Conta);
                gerarHtml = gerarHtml.Replace("[Empre" + i + "]", acesso.EmpresaNome);
                gerarHtml = gerarHtml.Replace("[Vol" + i + "]", acesso.Volume);
            }

            return gerarHtml.ToString();
        }

        #endregion

        #region AcessosGruposEmpresa

        public string AcessoGrupoEmpresaHtmlTitulo(eAcessos acesso)
        {
            gerarHtml = new StringBuilder();

            gerarHtml.Append("<div align='center' style='width: 1194px; display:inline-block;'>");
            gerarHtml.Append("<div align='left'style='width: 202px; display:inline; float:left; height: 150px;'>");
            gerarHtml.Append("<img src='[IMG]' alt='Smiley face' height='100' width='200' style='float:right;' /></div>");
            gerarHtml.Append("<div style='width: 807px; display:inline; float:inherit'>");
            gerarHtml.Append("<h2 align='center' style='width:800px; color:[COR]'; font-family:Tahoma; font-size:14pt; fontweight:bold;'>");
            gerarHtml.Append("ACESSOS GRUPOS E EMPRESA<br/> DE " + acesso.DiaDe + " DE " + NomeMes(acesso.MesDe) + " - " + acesso.AnoDe + "<br/>");
            gerarHtml.Append("ATÉ " + acesso.Dia + " DE " + NomeMes(acesso.Mes) + " - " + acesso.Ano + "</h2></div></div>");

            return ReplacesTitulo(gerarHtml.ToString());
        }

        public string AcessoGrupoEmpresaHtmlTabela(eAcessos acesso)
        {
            gerarHtml = new StringBuilder();

            List<eAcessos> listaAcesso = new List<eAcessos>();

            listaAcesso = GetUserRegistroAcessos(acesso);

            Regis = listaAcesso.Count();
            int qtdEmpre = Regis - 1;

            gerarHtml.Append("<table border='1' [EstiloTabelaBase] id='display'><tr style='background-color:[CorCabecalho];' align='left'><td width='279'>GRUPOS</td>");
            gerarHtml.Append("<td width='429'>EMPRESAS</td><td width='119'>VOL</td></tr>");

            AcessoGrupoEmpresaDadosTabela(gerarHtml, Regis, qtdEmpre);

            return AcessoGrupoEmpresaReplaces(gerarHtml, Regis, qtdEmpre);
        }

        private void AcessoGrupoEmpresaDadosTabela(StringBuilder gerarHtml, int Regis, int qtdEmpre)
        {
            for (int i = 0; i < Regis; i++)
            {
                gerarHtml.Append("<tr style='color:#000; font-weight:normal; font-family: Arial;'><td rowspan='" + (qtdEmpre + 1) + "'>[Grupo" + i + "]</td>");
                for (int d = 0; d < qtdEmpre; d++)
                {
                    if (d.Equals(qtdEmpre - 1))
                    {
                        gerarHtml.Append("<tr style='background-color:#D3D3D3; color:#000;'><td>TOTAL GRUPO [Grupo" + i + "] :</td><td>[TVol" + i + "] :</td></tr>");
                    }
                    else
                    {
                        gerarHtml.Append("<tr style='color:#000; font-weight:normal;'><td>[Empresa" + i + d + "]</td><td>[Vol" + i + d + "]</td></tr>");
                    }
                }
                gerarHtml.Append("</tr>");
            }
        }

        private string AcessoGrupoEmpresaReplaces(StringBuilder gerarHtml, int Regis, int qtdEmpre)
        {
            gerarHtml = gerarHtml.Replace("[EstiloTabelaBase]", EstiloTabelaBase);
            gerarHtml = gerarHtml.Replace("[SIZE]", "10");
            gerarHtml = gerarHtml.Replace("[NCelPadding]", "2");
            gerarHtml = gerarHtml.Replace("[CorBordaLinha]", "#" + eConfig.RelatorioCorLinhaAlternada);
            gerarHtml = gerarHtml.Replace("[CorCabecalho]", "#" + eConfig.RelatorioCorCabecalho);


            return gerarHtml.ToString();
        }

        #endregion

        //Metodo para Recupera o nome do mês
        public string NomeMes(int? mes)
        {
            string nomemes = string.Empty;

            switch (mes)
            {
                case 1:
                    nomemes = "JANEIRO";
                    break;
                case 2:
                    nomemes = "FEVEREIRO";
                    break;
                case 3:
                    nomemes = "MARÇO";
                    break;
                case 4:
                    nomemes = "ABRIL";
                    break;
                case 5:
                    nomemes = "MAIO";
                    break;
                case 6:
                    nomemes = "JUNHO";
                    break;
                case 7:
                    nomemes = "JULHO";
                    break;
                case 8:
                    nomemes = "AGOSTO";
                    break;
                case 9:
                    nomemes = "SETEMBRO";
                    break;
                case 10:
                    nomemes = "OUTUBRO";
                    break;
                case 11:
                    nomemes = "NOVEMBRO";
                    break;
                case 12:
                    nomemes = "DEZEMBRO";
                    break;
                default:
                    nomemes = null;
                    break;
            }

            return nomemes;
        }

        #region AcessosRelatorios

        public string AcessosRelatoriosHtmlTitulo(eAcessos acesso)
        {
            gerarHtml = new StringBuilder();

            gerarHtml.Append("<div align='center' style='width: 1194px; display:inline-block;'>");
            gerarHtml.Append("<div align='left'style='width: 202px; display:inline; float:left; height: 150px;'>");
            gerarHtml.Append("<img src='[IMG]' alt='Smiley face' height='100' width='200' style='float:right;' /></div>");
            gerarHtml.Append("<div style='width: 807px; display:inline; float:inherit'>");
            gerarHtml.Append("<h2 align='center' style='width:800px; color:[COR]'; font-family:Tahoma; font-size:14pt; fontweight:bold;'>");
            gerarHtml.Append("ACESSOS AOS RELATÓRIOS<br/> DE " + acesso.DiaDe + " DE " + NomeMes(acesso.MesDe) + " - " + acesso.AnoDe + "<br/>");
            gerarHtml.Append("ATÉ " + acesso.Dia + " DE " + NomeMes(acesso.Mes) + " - " + acesso.Ano + "</h2></div></div>");

            return ReplacesTitulo(gerarHtml.ToString());
        }

        public string AcessosRelatoriosHtmlTabela(eAcessos acesso)
        {
            gerarHtml = new StringBuilder();

            Regis = 0;

            gerarHtml.Append("<table border='1' [EstiloTabelaBase] id='display'><tr style='color:#000;'><td></td><td style='background-color:#D3D3D3;'>TOTAL</td>");
            for (int i = 0; i < 70; i++)
            {
                gerarHtml.Append("<td width='85'>[Relatorio" + i + "]</td>");
            }
            gerarHtml.Append("</tr>");

            AcessosRelatoriosDadosTabela(gerarHtml, Regis);

            return AcessosRelatoriosReplaces(gerarHtml, Regis);
        }

        private void AcessosRelatoriosDadosTabela(StringBuilder gerarHtml, int Regis)
        {
            for (int i = 0; i < 20; i++)
            {
                if (i.Equals(20 - 1))
                {
                    gerarHtml.Append("<tr style='color:#000; background-color:#D3D3D3;'><td>TOTAL</td><td style='background-color:#D3D3D3;'>[Total" + i + "]</td>");
                }
                else
                {
                    gerarHtml.Append("<tr style='color:#000; font-weight:normal;'><td>[Data" + i + "]</td><td style='background-color:#D3D3D3;'>[Total" + i + "]</td>");
                }

                for (int d = 0; d < 70; d++)
                {
                    gerarHtml.Append("<td>[ValorRelat" + i + d + "]</td>");
                }
                gerarHtml.Append("</tr>");
            }
        }

        private string AcessosRelatoriosReplaces(StringBuilder gerarHtml, int Regis)
        {
            gerarHtml = gerarHtml.Replace("[EstiloTabelaBase]", EstiloTabelaBase);
            gerarHtml = gerarHtml.Replace("[SIZE]", "9");
            gerarHtml = gerarHtml.Replace("[NCelPadding]", "2");
            gerarHtml = gerarHtml.Replace("[CorBordaLinha]", "#" + eConfig.RelatorioCorLinhaAlternada);
            gerarHtml = gerarHtml.Replace("[CorCabecalho]", "#" + eConfig.RelatorioCorCabecalho);


            return gerarHtml.ToString();
        }

        #endregion

        #region EstadosCidades

        public string EstadosCidadesHtmlTitulo()
        {
            gerarHtml = new StringBuilder();


            return gerarHtml.ToString();
        }

        public string EstadosCidadesHtmlTabela()
        {
            gerarHtml = new StringBuilder();

            Regis = 20;

            gerarHtml.Append("<table border='1' [EstiloTabelaBase] id='display'><tr style='background-color:[CorCabecalho];' cellpadding='15'><td width='238'>Estados</td>");
            gerarHtml.Append("<td width='342'>Cidades</td><td width='145'>Acumulado até a data</td>");
            gerarHtml.Append("<td width='185'>Participação no total estado</td></tr>");

            EstadosCidadesDadosTabela(gerarHtml, Regis, 10);

            gerarHtml.Append("</table>");

            return EstadosCidadesReplaces(gerarHtml, Regis);
        }

        private void EstadosCidadesDadosTabela(StringBuilder gerarHtml, int Regis, int qtdCity)
        {
            for (int i = 0; i < Regis; i++)
            {

                gerarHtml.Append("<tr style='color:#000; font-weight:normal; font-family: Arial;' align='left'><td rowspan='" + (qtdCity + 1) + "'>[Estado" + i + "]</td>");
                for (int d = 0; d < qtdCity; d++)
                {
                    if (d.Equals(qtdCity - 1))
                    {
                        gerarHtml.Append("<tr style='background-color:#DFDFDF;color:#000;font-family: Arial;' align='left'><td>TOTAL</td><td>[TAcumulado" + i + "]</td><td></td></tr>");
                    }
                    else
                    {
                        gerarHtml.Append("<tr style='color:#000; font-weight:normal; font-family: Arial;'  align='left'><td>[Cidade" + i + d + "]</td><td>[Acumulado" + i + +d + "]</td><td>[Part" + i + d + "]</td></tr>");
                    }
                }
                gerarHtml.Append("</tr>");
            }
        }

        private string EstadosCidadesReplaces(StringBuilder gerarHtml, int Regis)
        {
            gerarHtml = gerarHtml.Replace("[EstiloTabelaBase]", EstiloTabelaBase);
            gerarHtml = gerarHtml.Replace("[SIZE]", "10");
            gerarHtml = gerarHtml.Replace("[NCelPadding]", "2");
            gerarHtml = gerarHtml.Replace("[CorBordaLinha]", "#" + eConfig.RelatorioCorLinhaAlternada);
            gerarHtml = gerarHtml.Replace("[CorCabecalho]", "#" + eConfig.RelatorioCorCabecalho);

            return gerarHtml.ToString();
        }

        #endregion

        #region RegioesAreasMunicipios

        public string RegioesAreasMunicipiosHtmlTitulo(int concessionaria, byte bygroup)
        {
            gerarHtml = new StringBuilder();

            List<eRegioesAreasMunicipios> lista = new List<eRegioesAreasMunicipios>();
            eRegioesAreasMunicipios regiao = new eRegioesAreasMunicipios();

            lista = GetUserRegioesAreasMunicipios(concessionaria, bygroup);
            regiao = lista[1];

            gerarHtml.Append("<div align='center' style='width: 1194px; display:inline-block;'>");
            gerarHtml.Append("<div align='left'style='width: 202px; display:inline; float:left; height: 150px;'>");
            gerarHtml.Append("<img src='[IMG]' alt='Smiley face' height='100' width='200' style='float:right;' /></div>");
            gerarHtml.Append("<div style='width: 807px; display:inline; float:inherit'>");
            gerarHtml.Append("<h2 align='center' style='width:800px; color:[COR]'; font-family:Tahoma; font-size:14pt; fontweight:bold;'>");
            if (bygroup.Equals(1))
            {
                gerarHtml.Append("EMPLACAMENTO REGIÕES, AREA DE INFLUENCIA e MUNICÍPIOS<br/>GRUPO: " + regiao.nm_grupo.ToUpper());
            }
            else
            {
                gerarHtml.Append("EMPLACAMENTO REGIÕES, AREA DE INFLUENCIA e MUNICÍPIOS<br/>EMPRESA: " + regiao.nm_empresa.ToUpper() + "<br/>");
                gerarHtml.Append(regiao.nm_fantasia.ToUpper());
            }
            gerarHtml.Append("</h2></div></div>");

            return ReplacesTitulo(gerarHtml.ToString());
        }

        public string RegioesAreasMunicipiosHtmlTabela(int concessionaria, byte bygroup)
        {
            gerarHtml = new StringBuilder();

            List<eRegioesAreasMunicipios> lista = new List<eRegioesAreasMunicipios>();

            lista = GetUserRegioesAreasMunicipios(concessionaria, bygroup);
            int qtdCity = 0;

            Regis = 1;
            qtdCity = lista.Where(r => r.nm_cidade != null).ToList().Count();

            gerarHtml.Append("<table border='1' [EstiloTabelaBase] id='display'><tr style='background-color:[CorCabecalho];'><td width='309'>REGIÃO</td>");
            gerarHtml.Append("<td width='323'>ÁREA DE INFLUENCIA</td><td width='269'>MUNICÍPIOS</td></tr>");

            RegioesAreasMunicipiosDadosTabela(gerarHtml, Regis, qtdCity);

            gerarHtml.Append("</table>");

            return RegioesAreasMunicipiosReplaces(gerarHtml, Regis, qtdCity, lista);
        }

        private void RegioesAreasMunicipiosDadosTabela(StringBuilder gerarHtml, int Regis, int qtdCity)
        {
            for (int i = 0; i < Regis; i++)
            {
                gerarHtml.Append("<tr style='color:#000; font-weight:normal; font-family:Arial;' align='left'>");
                gerarHtml.Append("<td rowspan='" + (qtdCity + 1) + "'>[Regiao" + i + "]</td><td rowspan='" + (qtdCity + 1) + "'>[AreaInflu" + i + "]</td>");
                for (int d = 0; d < qtdCity; d++)
                {
                    gerarHtml.Append("<tr style='color:#000; font-weight:normal; font-family:Arial;' align='left' ><td>[Municipio" + i + d + "]</td></tr>");
                }
                gerarHtml.Append("</tr>");
            }
        }

        private string RegioesAreasMunicipiosReplaces(StringBuilder gerarHtml, int Regis, int qtdCity, List<eRegioesAreasMunicipios> lista)
        {
            gerarHtml = gerarHtml.Replace("[EstiloTabelaBase]", EstiloTabelaBase);
            gerarHtml = gerarHtml.Replace("[SIZE]", "10");
            gerarHtml = gerarHtml.Replace("[NCelPadding]", "2");
            gerarHtml = gerarHtml.Replace("[CorBordaLinha]", "#" + eConfig.RelatorioCorLinhaAlternada);
            gerarHtml = gerarHtml.Replace("[CorCabecalho]", "#" + eConfig.RelatorioCorCabecalho);

            eRegioesAreasMunicipios regiao = new eRegioesAreasMunicipios();

            regiao = lista[1];
            gerarHtml = gerarHtml.Replace("[Regiao0]", regiao.nm_regiao_operacional);
            gerarHtml = gerarHtml.Replace("[AreaInflu0]", regiao.nm_area_operacional);

            for (int i = 0; i < qtdCity; i++)
            {
                regiao = lista[i];
                gerarHtml = gerarHtml.Replace("[Municipio0" + i + "]", regiao.nm_cidade);
            }

            return gerarHtml.ToString();
        }

        public List<eRegioesAreasMunicipios> GetUserRegioesAreasMunicipios(int concessionaria, byte bygroup)
        {
            try
            {
                dRelatorios r = new dRelatorios();
                return r.GetUserRegistroRegioesAreasMunicipios(concessionaria, bygroup);
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        #endregion

        #region DuploEmplacamento

        public string DuploEmplacamentoHtmlTitulo(eDuploEmplacamento duplo)
        {
            gerarHtml = new StringBuilder();

            gerarHtml.Append("<div align='center' style='width: 1194px; display:inline-block;'>");
            gerarHtml.Append("<div align='left'style='width: 202px; display:inline; float:left; height: 150px;'>");
            gerarHtml.Append("<img src='[IMG]' alt='Smiley face' height='100' width='200' style='float:right;' /></div>");
            gerarHtml.Append("<div style='width: 807px; display:inline; float:inherit'>");
            gerarHtml.Append("<h3 align='center' style='width:800px; color:[COR]'; font-family:Tahoma; font-size:14pt; fontweight:bold;'>");
            gerarHtml.Append("DUPLO EMPLACAMNETO<br/>DE " + duplo.DiaDe + " DE " + NomeMes(duplo.Mes) + " - " + duplo.AnoDe);
            gerarHtml.Append("<br/>ATÉ " + duplo.Dia + " DE " + NomeMes(duplo.Mes) + " - " + duplo.Ano + "<br/>MODALIDADE DE VENDAS: TODAS<br/>");
            gerarHtml.Append("SIGLA: TODAS</h3></div></div>");

            return ReplacesTitulo(gerarHtml.ToString());
        }

        public string DuploEmplacamentoHtmlTabela(eDuploEmplacamento duplo)
        {
            gerarHtml = new StringBuilder();

            List<eDuploEmplacamento> lista = new List<eDuploEmplacamento>();

            lista = GetUserRegistroDuploEmplacamento(duplo);
            Regis = lista.Count();

            gerarHtml.Append("<table border='1' [EstiloTabelaBase] id='display'><tr style='background-color:[CorCabecalho];'><td></td><td width='190'></td>");
            gerarHtml.Append("<td width='60'></td><td width='70'></td><td width='227'></td><td colspan='2'></td><td width='79'>OCORRENCIA 1</td>");
            gerarHtml.Append("<td style='background-color:#FFF8DC; color:#000;' width='87'>OCORRENCIA 2</td><td width='66'></td></tr>");
            gerarHtml.Append("<tr style='background-color:[CorCabecalho];'><td>BIR</td><td>NOME FANTASIA</td><td>SIGLA</td><td>DATA NF</td>");
            gerarHtml.Append("<td>CLIENTE</td><td width='122'>CHASSI</td><td width='190'>MODELO</td><td>DATA</td>");
            gerarHtml.Append("<td style='background-color:#FFF8DC; color:#000;'>DATA</td><td>DIAS</td></tr>");

            DuploEmplacamentoDadosTabela(gerarHtml, Regis);


            return DuploEmplacamentoReplaces(gerarHtml, Regis, lista);

        }

        private void DuploEmplacamentoDadosTabela(StringBuilder gerarHtml, int Regis)
        {
            for (int i = 0; i < Regis; i++)
            {
                if (i % 2 == 0)
                {
                    gerarHtml.Append("<tr style='background-color:#DDD; color:#000; font-weight:normal;'>");
                }
                else
                {
                    gerarHtml.Append("<tr style='background-color:#FFF; color:#000; font-weight:normal;'>");
                }

                for (int d = 0; d < 10; d++)
                {
                    if (d.Equals(8))
                    {
                        gerarHtml.Append("<td style='background-color:#FFF8DC;'>[" + d + i + "]</td>");
                    }
                    else
                    {
                        gerarHtml.Append("<td align='left'>[" + d + i + "]</td>");
                    }
                }
                gerarHtml.Append("</tr>");
            }
        }

        private string DuploEmplacamentoReplaces(StringBuilder gerarHtml, int Regis, List<eDuploEmplacamento> lista)
        {
            gerarHtml = gerarHtml.Replace("[EstiloTabelaBase]", EstiloTabelaBase);
            gerarHtml = gerarHtml.Replace("[SIZE]", "8");
            gerarHtml = gerarHtml.Replace("[NCelPadding]", "2");
            gerarHtml = gerarHtml.Replace("[CorBordaLinha]", "#" + eConfig.RelatorioCorLinhaAlternada);
            gerarHtml = gerarHtml.Replace("[CorCabecalho]", "#" + eConfig.RelatorioCorCabecalho);

            eDuploEmplacamento duplo = new eDuploEmplacamento();

            for (int i = 0; i < Regis; i++)
            {
                duplo = lista[i];

                gerarHtml = gerarHtml.Replace("[0" + i + "]", duplo.Bir);
                gerarHtml = gerarHtml.Replace("[1" + i + "]", duplo.NomeFantasia);
                gerarHtml = gerarHtml.Replace("[2" + i + "]", duplo.TipoCliente);
                gerarHtml = gerarHtml.Replace("[3" + i + "]", duplo.DataNFe.ToString());
                gerarHtml = gerarHtml.Replace("[4" + i + "]", duplo.Cliente);
                gerarHtml = gerarHtml.Replace("[5" + i + "]", duplo.NumeroChassi);
                gerarHtml = gerarHtml.Replace("[6" + i + "]", duplo.ModeloVeiculo);
                gerarHtml = gerarHtml.Replace("[7" + i + "]", duplo.DataEmplacamento.ToShortDateString());
                gerarHtml = gerarHtml.Replace("[7" + i + "]", duplo.DataEmplacamento.ToShortDateString());
                gerarHtml = gerarHtml.Replace("[8" + i + "]", duplo.DataTransacao.ToShortDateString());
                gerarHtml = gerarHtml.Replace("[9" + i + "]", duplo.DiferencaDias.ToString());
            }
            return gerarHtml.ToString();
        }

        public List<eDuploEmplacamento> GetUserRegistroDuploEmplacamento(eDuploEmplacamento duplo)
        {
            try
            {
                dRelatorios relatorio = new dRelatorios();
                return relatorio.GetUserRegistroDuploEmplacamento(duplo);
            }
            catch (Exception)
            {
                throw;
            }

        }

        #endregion

        //Não Implementado
        #region FaturamentoConsolidado

        #endregion

        #region FaturamentoDVE

        public string FaturamentoDVEHtmlTitulo(eFaturamentoDve faturamento)
        {
            gerarHtml = new StringBuilder();

            gerarHtml.Append("<div align='center' style='width: 1194px; display:inline-block;'>");
            gerarHtml.Append("<div align='left'style='width: 202px; display:inline; float:left; height: 150px;'>");
            gerarHtml.Append("<img src='[IMG]' alt='Smiley face' height='100' width='200' style='float:right;' /></div>");
            gerarHtml.Append("<div style='width: 807px; display:inline; float:inherit'>");
            gerarHtml.Append("<h2 align='center' style='width:800px; color:[COR]'; font-family:Tahoma; font-size:14pt; fontweight:bold;'>");
            gerarHtml.Append("FATURAMENTO DVE " + faturamento.Ano + " <br/>ACUMULADO ATÉ " + NomeMes(faturamento.Mes - 1) + "</h2></div></div>");

            return ReplacesTitulo(gerarHtml.ToString());
        }

        public string FaturamentoDVEHtmlTabela(eFaturamentoDve faturamento)
        {
            gerarHtml = new StringBuilder();


            List<eFaturamentoDve> lista = new List<eFaturamentoDve>();
            List<eFaturamentoDve> listaCerta = new List<eFaturamentoDve>();

            lista = GeUserRegistroFaturamentoDVE(faturamento);

            int qtdMes = lista[0].UltimoMes;

            gerarHtml.Append("<table border='1' [EstiloTabelaBase] id='display'><tr style='background-color:#000;'><td width='82'>Sigla</td>");
            gerarHtml.Append("<td width='170'>Tipo de Cliente</td><td width='136'>Área Divisão</td><td width='65'>Total</td>");
            gerarHtml.Append("<td width='85'>Participação</td>");
            for (int i = 0; i < qtdMes; i++)
            {
                gerarHtml.Append("<td width='50'>" + NomeMes(i + 1).Substring(0, 3).ToUpper() + "</td>");
            }
            gerarHtml.Append("</tr>");


            return FaturamentoDVEDadosTabela(gerarHtml, qtdMes, lista);
        }

        private List<eFaturamentoDve> GeUserRegistroFaturamentoDVE(eFaturamentoDve faturamento)
        {
            try
            {
                dRelatorios relatorio = new dRelatorios();
                return relatorio.GetUserRegistroFaturamentoDve(faturamento);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string FaturamentoDVEDadosTabela(StringBuilder gerarHtml, int qtdMes, List<eFaturamentoDve> lista)
        {
            Regis = 0;
            eFaturamentoDve fat;
            List<eFaturamentoDve> listaCerta = new List<eFaturamentoDve>();
            List<eFaturamentoDve> listaInteira = new List<eFaturamentoDve>();

            listaInteira = lista;


            gerarHtml.Append("<tr style='background-color:#D3D3D3; color:#000;'><td></td><td></td><td></td><td>[TotalG]</td><td>100%</td>");
            for (int i = 0; i < qtdMes; i++)
            {
                gerarHtml.Append("<td>[TMes" + i + "]</td>");
            }
            gerarHtml.Append("</tr>");
            for (int i = 0; i < lista.Count - 1; i++)
            {
                if (lista[i].Sigla != lista[i + 1].Sigla)
                {
                    fat = new eFaturamentoDve();

                    fat.Sigla = lista[i].Sigla;
                    fat.TipoDeCliente = lista[i].TipoDeCliente;
                    fat.AreaDivisao = lista[i].AreaDivisao;
                    fat.TotalSigla = lista[i].TotalSigla;
                    fat.Participacao = lista[i].Participacao;
                    listaCerta.Add(fat);

                    Regis += 1;
                }
                else
                {
                    if (i.Equals(lista.Count - 2))
                    {
                        fat = new eFaturamentoDve();

                        fat.Sigla = lista[i].Sigla;
                        fat.TipoDeCliente = lista[i].TipoDeCliente;
                        fat.AreaDivisao = lista[i].AreaDivisao;
                        fat.TotalSigla = lista[i].TotalSigla;
                        fat.Participacao = lista[i].Participacao;
                        listaCerta.Add(fat);
                    }
                }
            }

            for (int i = 0; i < listaCerta.Count; i++)
            {
                gerarHtml.Append("<tr style='font-weight:normal; color:#000;'><td>[Sigla" + i + "]</td><td align='left'>[TdC" + i + "]</td>");
                gerarHtml.Append("<td align='left'>[AD" + i + "]</td><td>[Total" + i + "]</td><td>[Part" + i + "]%</td>");

                for (int d = 1; d <= qtdMes; d++)
                {
                    gerarHtml.Append("<td>[Mes" + i + d + "]</td>");
                }
                gerarHtml.Append("</tr>");
            }


            return FaturamentoDVEReplacesTabela(gerarHtml, Regis, qtdMes, listaCerta, listaInteira);
        }

        private string FaturamentoDVEReplacesTabela(StringBuilder gerarHtml, int Regis, int qtdMes, List<eFaturamentoDve> listaCerta, List<eFaturamentoDve> listaInteira)
        {

            int c1 = listaCerta.Count;
            string nome = string.Empty;
            List<eFaturamentoDve> listaSilgas = new List<eFaturamentoDve>();
            eFaturamentoDve fatur;

            gerarHtml = gerarHtml.Replace("[EstiloTabelaBase]", EstiloTabelaBase);
            gerarHtml = gerarHtml.Replace("[SIZE]", "8");
            gerarHtml = gerarHtml.Replace("[NCelPadding]", "2");
            gerarHtml = gerarHtml.Replace("[CorBordaLinha]", "#" + eConfig.RelatorioCorLinhaAlternada);
            gerarHtml = gerarHtml.Replace("[CorCabecalho]", "#" + eConfig.RelatorioCorCabecalho);

            gerarHtml = gerarHtml.Replace("[TotalG]", listaInteira[0].TotalGeral.ToString());
            int TotalMes = 0;

            for (int i = 0; i < qtdMes; i++)
            {
                TotalMes = listaInteira.Where(s => s.Mes.Equals(i + 1)).Sum(s => s.TotalMes);
                gerarHtml = gerarHtml.Replace("[TMes" + i + "]", TotalMes.ToString());
            }

            listaCerta = listaCerta.OrderByDescending(o => o.Participacao).ToList();

            gerarHtml = gerarHtml.Replace("[Sigla" + 0 + "]", listaCerta[0].Sigla.ToString());
            gerarHtml = gerarHtml.Replace("[TdC" + 0 + "]", listaCerta[0].TipoDeCliente);
            gerarHtml = gerarHtml.Replace("[AD" + 0 + "]", listaCerta[0].AreaDivisao);
            gerarHtml = gerarHtml.Replace("[Total" + 0 + "]", listaCerta[0].TotalSigla.ToString());
            gerarHtml = gerarHtml.Replace("[Part" + 0 + "]", listaCerta[0].Participacao.ToString());

            fatur = new eFaturamentoDve();
            fatur.ListaTotalMes = new List<eFaturamentoDve>();
            listaInteira = listaInteira.OrderByDescending(o => o.Participacao).ToList();
            int contagem = 0;


            for (int i = 0; i < listaCerta.Count; i++)
            {
                gerarHtml = gerarHtml.Replace("[Sigla" + i + "]", listaCerta[i].Sigla.ToString());
                gerarHtml = gerarHtml.Replace("[TdC" + i + "]", listaCerta[i].TipoDeCliente);
                gerarHtml = gerarHtml.Replace("[AD" + i + "]", listaCerta[i].AreaDivisao);
                gerarHtml = gerarHtml.Replace("[Total" + i + "]", listaCerta[i].TotalSigla.ToString());
                gerarHtml = gerarHtml.Replace("[Part" + i + "]", listaCerta[i].Participacao.ToString());

                fatur.ListaTotalMes = listaInteira.Where(d => d.Sigla == listaCerta[i].Sigla).ToList();

                if (fatur.ListaTotalMes.Count < qtdMes)
                {
                    contagem = qtdMes - fatur.ListaTotalMes.Count;

                    for (int f = 0; f < contagem; f++)
                    {
                        eFaturamentoDve fat = new eFaturamentoDve();

                        fat.Sigla = "";
                        fat.Mes = 0;
                        fat.TotalMes = 0;
                        fatur.ListaTotalMes.Insert(f, fat);
                    }
                }



                for (int d = 0; d < fatur.ListaTotalMes.Count; d++)
                {
                    gerarHtml = gerarHtml.Replace("[Mes" + i + (d + 1) + "]", fatur.ListaTotalMes[d].TotalMes.ToString());
                }
            }

            for (int i = 0; i < listaCerta.Count; i++)
            {
                for (int d = 1; d <= qtdMes; d++)
                {
                    gerarHtml = gerarHtml.Replace("[Mes" + i + d + "]", "0");
                }
            }


            return gerarHtml.ToString();
        }

        #endregion

        #region FaturamentoDVR

        public string FaturamentoDVRHtmlTitulo(int ano, int mes, int dia)
        {
            gerarHtml = new StringBuilder();

            gerarHtml.Append("<div align='center' style='width: 1194px; display:inline-block;'>");
            gerarHtml.Append("<div align='left'style='width: 202px; display:inline; float:left; height: 150px;'>");
            gerarHtml.Append("<img src='[IMG]' alt='Smiley face' height='100' width='200' style='float:right;' /></div>");
            gerarHtml.Append("<div style='width: 807px; display:inline; float:inherit'>");
            gerarHtml.Append("<h2 align='center' style='width:800px; color:[COR]'; font-family:Tahoma; font-size:14pt; fontweight:bold;'>");
            gerarHtml.Append("FATURAMENTO ATACADO " + ano + " <br/>ACUMULADO ATÉ " + NomeMes(mes - 1) + "</h2></div></div>");

            return ReplacesTitulo(gerarHtml.ToString());
        }

        public string FaturamentoDVRHtmlTabela(int ano, int mes, int dia)
        {
            gerarHtml = new StringBuilder();

            List<eFaturamentoDvr> lista = new List<eFaturamentoDvr>();
            List<eFaturamentoDvr> listaConcessionarias = new List<eFaturamentoDvr>();

            lista = GeUserRegistroFaturamentoDVR(ano, mes, dia);

            #region DividindoDados

            List<eFaturamentoDvr> listaTab1 = new List<eFaturamentoDvr>();
            List<eFaturamentoDvr> listaTab2 = new List<eFaturamentoDvr>();
            List<eFaturamentoDvr> listaColaborador = new List<eFaturamentoDvr>();

            //Seperando os dados 
            for (int i = 0; i < lista.Count; i++)
            {
                eFaturamentoDvr fat = new eFaturamentoDvr();
                if (Convert.ToInt32(lista[i].Bir) > 0 && Convert.ToInt32(lista[i].Bir) < 7)
                {
                    fat.Bir = lista[i].Bir;
                    fat.Concessionaria = lista[i].Concessionaria;
                    fat.Mes = lista[i].Mes;
                    fat.Qtd = lista[i].Qtd;
                    fat.UltimoMes = lista[i].UltimoMes;
                    fat.UltimoAno = lista[i].UltimoAno;
                    fat.UltimoDia = lista[i].UltimoDia;
                    fat.Total = lista[i].Total;
                    fat.TotalGeral = lista[i].TotalGeral;
                    fat.TotalMes = lista[i].TotalMes;
                    fat.Participacao = lista[i].Participacao;

                    listaTab1.Add(fat);
                }
                else if (lista[i].Concessionaria.Equals("Colaborador"))
                {
                    fat.Bir = lista[i].Bir;
                    fat.Concessionaria = lista[i].Concessionaria;
                    fat.Mes = lista[i].Mes;
                    fat.Qtd = lista[i].Qtd;
                    fat.UltimoMes = lista[i].UltimoMes;
                    fat.UltimoAno = lista[i].UltimoAno;
                    fat.UltimoDia = lista[i].UltimoDia;
                    fat.Total = lista[i].Total;
                    fat.TotalGeral = lista[i].TotalGeral;
                    fat.TotalMes = lista[i].TotalMes;
                    fat.Participacao = lista[i].Participacao;

                    listaColaborador.Add(fat);
                }
                else
                {
                    fat.Bir = lista[i].Bir;
                    fat.Concessionaria = lista[i].Concessionaria;
                    fat.Mes = lista[i].Mes;
                    fat.Qtd = lista[i].Qtd;
                    fat.UltimoMes = lista[i].UltimoMes;
                    fat.UltimoAno = lista[i].UltimoAno;
                    fat.UltimoDia = lista[i].UltimoDia;
                    fat.Total = lista[i].Total;
                    fat.TotalGeral = lista[i].TotalGeral;
                    fat.TotalMes = lista[i].TotalMes;
                    fat.Participacao = lista[i].Participacao;

                    listaTab2.Add(fat);
                }

            }

            #endregion

            listaTab2 = listaTab2.OrderBy(o => o.Concessionaria).ToList();

            eFaturamentoDvr ldvr;

            for (int i = 0; i < listaTab2.Count - 1; i++)
            {
                if (listaTab2[i].Concessionaria != listaTab2[i + 1].Concessionaria)
                {
                    ldvr = new eFaturamentoDvr();

                    ldvr.Bir = listaTab2[i].Bir;
                    ldvr.Concessionaria = listaTab2[i].Concessionaria;
                    ldvr.Total = listaTab2[i].Total;
                    ldvr.Participacao = listaTab2[i].Participacao;
                    ldvr.Mes = listaTab2[i].Mes;
                    listaConcessionarias.Add(ldvr);
                }
                else
                {
                    if (i.Equals(listaTab2.Count - 2))
                    {
                        ldvr = new eFaturamentoDvr();

                        ldvr.Bir = listaTab2[i].Bir;
                        ldvr.Concessionaria = listaTab2[i].Concessionaria;
                        ldvr.Total = listaTab2[i].Total;
                        ldvr.Participacao = listaTab2[i].Participacao;
                        ldvr.Mes = listaTab2[i].Mes;
                        listaConcessionarias.Add(ldvr);
                    }
                }
            }

            int qtdMes = Convert.ToInt32(lista[0].UltimoMes);
            int RegisTab2 = listaConcessionarias.Count();

            gerarHtml.Append("<table border='1' [EstiloTabelaBase] id='display'><tr style='background-color:#000;'><td width='64'>BIR</td><td width='188px'>Descrição</td>");
            gerarHtml.Append("<td width='60'>Total</td><td width='94'>Participação</td>");
            for (int i = 0; i < qtdMes; i++)
            {
                gerarHtml.Append("<td width='44'>" + NomeMes(i + 1).Substring(0, 3).ToUpper() + "</td>");
            }
            gerarHtml.Append("</tr>");


            FaturamentoDVRDadosTabela(gerarHtml, qtdMes, lista, RegisTab2);

            return FaturamentoDVRReplacesTabela(gerarHtml, ano, mes, dia, listaTab1, listaTab2, listaColaborador, qtdMes, listaConcessionarias);
        }

        private void FaturamentoDVRDadosTabela(StringBuilder gerarHtml, int qtdMes, List<eFaturamentoDvr> lista, int RegisTab2)
        {
            gerarHtml.Append("<tr style='background-color:#D3D3D3; color:#000;'><td></td><td align='left'>Total resumo</td><td>[TotalG]</td><td></td>");
            for (int i = 1; i <= qtdMes; i++)
            {
                gerarHtml.Append("<td>[TMes" + i + "]</td>");
            }

            for (int i = 1; i <= 6; i++)
            {
                gerarHtml.Append("<tr style='color:#000; font-weight:normal;'><td style='background-color:#D3D3D3;'></td>");
                gerarHtml.Append("<td align='left'>[DescTab1-" + i + "]</td><td>[TotalTab1-" + i + "]</td><td>[PartTab1-" + i + "]</td>");
                for (int d = 1; d <= qtdMes; d++)
                {
                    gerarHtml.Append("<td>[MesTab1-" + i + d + "]</td>");
                }
            }
            gerarHtml.Append("</tr>");


            gerarHtml.Append("<tr style='boder:none;'><td [B]>a</td><td [B]></td><td [B]></td><td [B]></td>");
            for (int i = 1; i <= qtdMes; i++)
            {
                gerarHtml.Append("<td [B]></td>");
            }
            gerarHtml.Append("</tr><tr style='color:#000;font-weight:normal;'><td align='left'>[BirC]</td>");
            gerarHtml.Append("<td align='left'>[C]</td><td>[TotalC]</td><td></td>");
            for (int d = 1; d <= qtdMes; d++)
            {
                gerarHtml.Append("<td>[CMes" + d + "]</td>");
            }
            gerarHtml.Append("</tr>");


            gerarHtml.Append("<tr style='boder:none;'><td [B]>a</td><td [B]></td><td [B]></td><td [B]></td>");
            for (int i = 1; i <= qtdMes; i++)
            {
                gerarHtml.Append("<td [B]></td>");
            }
            gerarHtml.Append("</tr>");

            gerarHtml.Append("<tr style='background-color:#D3D3D3; color:#000;'><td></td><td align='left'>Total detalhe</td><td>[TotalG]</td><td></td>");
            for (int i = 1; i <= qtdMes; i++)
            {
                gerarHtml.Append("<td>[TMes" + i + "]</td>");
            }
            gerarHtml.Append("</tr>");

            for (int i = 1; i <= RegisTab2; i++)
            {
                gerarHtml.Append("<tr style='color:#000; font-weight:normal;'><td style='background-color:#D3D3D3;' align='left'>[BirTab2-" + i + "]</td>");
                gerarHtml.Append("<td align='left'>[DescTab2-" + i + "]</td><td>[TotalTab2-" + i + "]</td><td>[PartTab2-" + i + "]</td>");
                for (int d = 1; d <= qtdMes; d++)
                {
                    gerarHtml.Append("<td>[MesTab2-" + i + d + "]</td>");
                }
            }
            gerarHtml.Append("</tr></table>");
        }

        private List<eFaturamentoDvr> GeUserRegistroFaturamentoDVR(int ano, int mes, int dia)
        {
            try
            {
                dRelatorios relatorio = new dRelatorios();
                return relatorio.GetUserRegistroFaturamentoDvr(ano, mes, dia);
            }
            catch (Exception)
            {
                throw;
            }

        }

        private string FaturamentoDVRReplacesTabela(StringBuilder gerarHtml, int ano, int mes, int dia, List<eFaturamentoDvr> listaTab1, List<eFaturamentoDvr> listaTab2, List<eFaturamentoDvr> listaColaborador, int qtdMes, List<eFaturamentoDvr> listaConcessionaria)
        {
            gerarHtml = gerarHtml.Replace("[EstiloTabelaBase]", EstiloTabelaBase);
            gerarHtml = gerarHtml.Replace("[SIZE]", "8");
            gerarHtml = gerarHtml.Replace("[NCelPadding]", "2");
            gerarHtml = gerarHtml.Replace("[CorBordaLinha]", "#" + eConfig.RelatorioCorLinhaAlternada);
            gerarHtml = gerarHtml.Replace("[CorCabecalho]", "#" + eConfig.RelatorioCorCabecalho);
            gerarHtml = gerarHtml.Replace("[B]", "style='border:none;'");

            List<eFaturamentoDvr> lisTab2 = new List<eFaturamentoDvr>();
            List<eFaturamentoDvr> lisTab1 = new List<eFaturamentoDvr>();

            gerarHtml = gerarHtml.Replace("[TotalG]", listaTab1[0].TotalGeral.ToString());

            //Prenchendo dados da Primeira Tabela (R1...)
            for (int i = 0; i < 6; i++)
            {
                gerarHtml = gerarHtml.Replace("[TotalTab1-" + (i + 1) + "]", listaTab1[i].Total.ToString());
                gerarHtml = gerarHtml.Replace("[DescTab1-" + (i + 1) + "]", listaTab1[i].Concessionaria);
                gerarHtml = gerarHtml.Replace("[PartTab1-" + (i + 1) + "]", listaTab1[i].Participacao + "%");
            }

            string regiao = string.Empty;
            int countTab1 = 0;

            for (int i = 0; i < listaTab1.Count;)
            {
                regiao = listaTab1[i].Concessionaria;

                if (regiao.Equals(listaTab1[i].Concessionaria))
                {
                    lisTab1 = listaTab1.Where(c => c.Concessionaria.Equals(regiao)).ToList();

                    for (int d = 0; d < lisTab1.Count; d++)
                    {
                        gerarHtml = gerarHtml.Replace("[MesTab1-" + (countTab1 + 1) + (d + 1) + "]", lisTab1[d].Qtd.ToString());
                    }

                    countTab1 += 1;
                    i += lisTab1.Count;
                }
            }

            //Preenchendo linha Colaborador
            gerarHtml = gerarHtml.Replace("[BirC]", listaColaborador[0].Bir);
            gerarHtml = gerarHtml.Replace("[C]", listaColaborador[0].Concessionaria);
            gerarHtml = gerarHtml.Replace("[TotalC]", listaColaborador[0].Total.ToString());

            string Total = string.Empty;
            for (int i = 0; i < qtdMes; i++)
            {
                gerarHtml = gerarHtml.Replace("[CMes" + (i + 1) + "]", listaColaborador[i].Qtd.ToString());
                gerarHtml = gerarHtml.Replace("[TMes" + (i + 1) + "]", listaTab1[i].TotalMes);
            }

            //Preenchedo dados da Segunda Tabela (Eiffel...)
            listaConcessionaria = listaConcessionaria.OrderByDescending(c => c.Total).ToList();
            listaTab2 = listaTab2.OrderByDescending(c => c.Total).ToList();
            int count = 0;
            int valor = 0;

            for (int i = 0; i < listaConcessionaria.Count; i++)
            {
                gerarHtml = gerarHtml.Replace("[BirTab2-" + (i + 1) + "]", listaConcessionaria[i].Bir);
                gerarHtml = gerarHtml.Replace("[DescTab2-" + (i + 1) + "]", listaConcessionaria[i].Concessionaria);
                gerarHtml = gerarHtml.Replace("[TotalTab2-" + (i + 1) + "]", listaConcessionaria[i].Total.ToString());
                gerarHtml = gerarHtml.Replace("[PartTab2-" + (i + 1) + "]", listaConcessionaria[i].Participacao + "%");
            }


            for (int i = 0; i < listaTab2.Count;)
            {
                if (listaTab2[i].Concessionaria.Equals(listaConcessionaria[count].Concessionaria))
                {
                    lisTab2 = listaTab2.Where(d => d.Concessionaria.Equals(listaConcessionaria[count].Concessionaria)).ToList();
                    valor = lisTab2.Count();

                    if (lisTab2.Count < qtdMes)
                    {
                        for (int f = 0; f < lisTab2.Count; f++)
                        {
                            if (!lisTab2[f].Mes.Contains((f + 1).ToString()))
                            {
                                lisTab2.Insert(f, new eFaturamentoDvr
                                {
                                    Mes = "0",
                                    Qtd = 0
                                });
                            }
                        }
                    }

                    for (int d = 0; d < lisTab2.Count; d++)
                    {
                        gerarHtml = gerarHtml.Replace("[MesTab2-" + (count + 1) + (d + 1) + "]", lisTab2[d].Qtd.ToString());
                    }

                }

                count += 1;
                i += valor;
            }

            for (int i = 1; i <= listaConcessionaria.Count; i++)
            {
                for (int d = 1; d <= qtdMes; d++)
                {
                    gerarHtml = gerarHtml.Replace("[MesTab2-" + i + d + "]", "0");
                }
            }




            //for (int i = 0; i < listaTab2.Count(); i++)
            //{

            //    for (int d = 1; d <= qtdMes; d++)
            //    {
            //        gerarHtml = gerarHtml.Replace("[MesTab2" + (i + 1) + d + "]", listaTab2[d].TotalMes);
            //    }
            //}

            return gerarHtml.ToString();
        }

        #endregion

        #region FaturamentoDveCliente

        public string FaturamentoDveClienteHtmlTitulo(int dia, int mes, int ano)
        {
            gerarHtml = new StringBuilder();

            gerarHtml.Append("<div align='center' style='width: 1194px; display:inline-block;'>");
            gerarHtml.Append("<div align='left'style='width: 202px; display:inline; float:left; height: 150px;'>");
            gerarHtml.Append("<img src='[IMG]' alt='Smiley face' height='100' width='200' style='float:right;' /></div>");
            gerarHtml.Append("<div style='width: 807px; display:inline; float:inherit'>");
            gerarHtml.Append("<h3 align='center' style='width:800px; color:[COR]'; font-family:Tahoma; font-size:14pt; fontweight:bold;'>");
            gerarHtml.Append("FATURAMENTO DVE CLIENTE " + ano + " <br/>ACUMULADO ATÉ " + NomeMes(mes - 1) + "</h3></div></div>");

            return ReplacesTitulo(gerarHtml.ToString());
        }

        public string FaturamentoDveClienteHtmlTabela(int dia, int mes, int ano)
        {
            gerarHtml = new StringBuilder();

            List<eFaturamentoDVEClienteConcessionaria> lista = new List<eFaturamentoDVEClienteConcessionaria>();

            lista = GeUserRegistroFaturamentoDveCliente(ano, mes, dia);

            int qtdMes = Convert.ToInt32(lista[0].UltimoMes);
            int Regis = lista.Count;

            gerarHtml.Append("<table border='1' [EstiloTabelaBase]><tr style='background-color:#000; font-family:Arial;'><td width='60'>BIR</td>");
            gerarHtml.Append("<td width='172'>Razão Social</td><td width='172'>Cliente</td><td width='90'>Total Cliente</td><td width='76'>Participação</td>");
            for (int i = 0; i < qtdMes; i++)
            {
                gerarHtml.Append("<td width='50'>" + NomeMes(i + 1).Substring(0, 3) + "</td>");
            }
            gerarHtml.Append("</tr><tr style='background-color:#DDD; color:#000;'><td></td><td></td><td></td><td>[TotalG]</td><td>100%</td>");
            for (int i = 0; i < qtdMes; i++)
            {
                gerarHtml.Append("<td>[TMes" + i + "]</td>");
            }
            gerarHtml.Append("</tr>");

            FaturamentoDveClienteConcessionariaDadosTabela(gerarHtml, qtdMes, Regis);

            return FaturamentoDveClienteConcessionariaReplaces(gerarHtml, qtdMes, lista);
        }

        private void FaturamentoDveClienteConcessionariaDadosTabela(StringBuilder gerarHtml, int qtdMes, int Regis)
        {
            for (int i = 0; i < Regis; i++)
            {
                gerarHtml.Append("<tr style='color:#000; font-weight:normal; font-family:Arial;'><td align='left'>[Bir" + i + "]</td><td align='left'>[RS" + i + "]</td>");
                gerarHtml.Append("<td align='left'>[C" + i + "]</td><td>[TC" + i + "]</td><td>[Part" + i + "]</td>");
                for (int d = 0; d < qtdMes; d++)
                {
                    gerarHtml.Append("<td>[Mes" + i + d + "]</td>");
                }
                gerarHtml.Append("</tr></table>");
            }
        }

        private string FaturamentoDveClienteConcessionariaReplaces(StringBuilder gerarHtml, int qtdMes, List<eFaturamentoDVEClienteConcessionaria> lista)
        {
            gerarHtml = gerarHtml.Replace("[EstiloTabelaBase]", EstiloTabelaBase);
            gerarHtml = gerarHtml.Replace("[SIZE]", "9");
            gerarHtml = gerarHtml.Replace("[NCelPadding]", "2");
            gerarHtml = gerarHtml.Replace("[CorBordaLinha]", "#" + eConfig.RelatorioCorLinhaAlternada);
            gerarHtml = gerarHtml.Replace("[CorCabecalho]", "#" + eConfig.RelatorioCorCabecalho);
            //gerarHtml = gerarHtml.Replace("[B]", "style='border:none;'");

            gerarHtml = gerarHtml.Replace("[TotalG]", lista[0].TotalFaturamento);
            gerarHtml = gerarHtml.Replace("[TMes0]", lista[0].TJan);
            gerarHtml = gerarHtml.Replace("[TMes1]", lista[0].TFev);
            gerarHtml = gerarHtml.Replace("[TMes2]", lista[0].TMar);
            gerarHtml = gerarHtml.Replace("[TMes3]", lista[0].TAbr);
            gerarHtml = gerarHtml.Replace("[TMes4]", lista[0].TMai);
            gerarHtml = gerarHtml.Replace("[TMes5]", lista[0].TJun);
            gerarHtml = gerarHtml.Replace("[TMes6]", lista[0].TJul);
            gerarHtml = gerarHtml.Replace("[TMes7]", lista[0].TAgo);
            gerarHtml = gerarHtml.Replace("[TMes8]", lista[0].TSet);
            gerarHtml = gerarHtml.Replace("[TMes9]", lista[0].TOut);
            gerarHtml = gerarHtml.Replace("[TMes10]", lista[0].TNov);
            gerarHtml = gerarHtml.Replace("[TMes11]", lista[0].TDez);


            for (int i = 0; i < lista.Count; i++)
            {
                gerarHtml = gerarHtml.Replace("[Bir" + i + "]", lista[i].Bir);
                gerarHtml = gerarHtml.Replace("[RS" + i + "]", lista[i].RazaoSocial);
                if (string.IsNullOrEmpty(lista[i].Cliente))
                {
                    gerarHtml = gerarHtml.Replace("[C" + i + "]", lista[i].NomeFantasia);
                    gerarHtml = gerarHtml.Replace("[TC" + i + "]", lista[i].TotalFantasia);
                }
                else
                {
                    gerarHtml = gerarHtml.Replace("[C" + i + "]", lista[i].Cliente);
                    gerarHtml = gerarHtml.Replace("[TC" + i + "]", lista[i].TotalCliente);
                }
                gerarHtml = gerarHtml.Replace("[Part" + i + "]", lista[i].Participacao);

                gerarHtml = gerarHtml.Replace("[Mes" + i + "0]", lista[i].Jan);
                gerarHtml = gerarHtml.Replace("[Mes" + i + "1]", lista[i].Fev);
                gerarHtml = gerarHtml.Replace("[Mes" + i + "2]", lista[i].Mar);
                gerarHtml = gerarHtml.Replace("[Mes" + i + "3]", lista[i].Abr);
                gerarHtml = gerarHtml.Replace("[Mes" + i + "4]", lista[i].Mai);
                gerarHtml = gerarHtml.Replace("[Mes" + i + "5]", lista[i].Jun);
                gerarHtml = gerarHtml.Replace("[Mes" + i + "6]", lista[i].Jul);
                gerarHtml = gerarHtml.Replace("[Mes" + i + "7]", lista[i].Ago);
                gerarHtml = gerarHtml.Replace("[Mes" + i + "8]", lista[i].Set);
                gerarHtml = gerarHtml.Replace("[Mes" + i + "9]", lista[i].Out);
                gerarHtml = gerarHtml.Replace("[Mes" + i + "10]", lista[i].Nov);
                gerarHtml = gerarHtml.Replace("[Mes" + i + "11]", lista[i].Dez);

            }

            return gerarHtml.ToString();
        }

        private List<eFaturamentoDVEClienteConcessionaria> GeUserRegistroFaturamentoDveCliente(int ano, int mes, int dia)
        {
            try
            {
                dRelatorios relatorio = new dRelatorios();
                return relatorio.GetUserRegistroFaturamentoDveCliente(ano, mes, dia);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion

        #region FaturamentoDVEConcessionaria

        public string FaturamentoDVEConcessionariaHtmlTitulo(int dia, int mes, int ano)
        {
            gerarHtml = new StringBuilder();

            gerarHtml.Append("<div align='center' style='width: 1194px; display:inline-block;'>");
            gerarHtml.Append("<div align='left'style='width: 202px; display:inline; float:left; height: 150px;'>");
            gerarHtml.Append("<img src='[IMG]' alt='Smiley face' height='100' width='200' style='float:right;' /></div>");
            gerarHtml.Append("<div style='width: 807px; display:inline; float:inherit'>");
            gerarHtml.Append("<h3 align='center' style='width:800px; color:[COR]'; font-family:Tahoma; font-size:14pt; fontweight:bold;'>");
            gerarHtml.Append("FATURAMENTO DVE CONCESSIONARIA " + ano + " <br/>ACUMULADO ATÉ " + NomeMes(mes - 1) + "</h3></div></div>");

            return ReplacesTitulo(gerarHtml.ToString());
        }

        public string FaturamentoDveConcessionariaHtmlTabela(int dia, int mes, int ano)
        {
            gerarHtml = new StringBuilder();

            List<eFaturamentoDVEClienteConcessionaria> lista = new List<eFaturamentoDVEClienteConcessionaria>();

            lista = GetUserRegistroFaturamentoDveConcessionaria(ano, mes, dia);

            int qtdMes = Convert.ToInt32(lista[0].UltimoMes);
            int Regis = lista.Count;

            gerarHtml.Append("<table border='1' [EstiloTabelaBase] id='display'><tr style='background-color:#000;'><td width='60'>BIR</td>");
            gerarHtml.Append("<td width='172'>Concessionária</td><td width='172'>Razão Social</td><td width='90'>Total</td><td width='76'>Participação</td>");
            for (int i = 0; i < qtdMes; i++)
            {
                gerarHtml.Append("<td width='50'>" + NomeMes(i + 1).Substring(0, 3) + "</td>");
            }
            gerarHtml.Append("</tr><tr style='background-color:#DDD; color:#000;'><td></td><td></td><td></td><td>[TotalG]</td><td>100%</td>");
            for (int i = 0; i < qtdMes; i++)
            {
                gerarHtml.Append("<td>[TMes" + i + "]</td>");
            }
            gerarHtml.Append("</tr>");

            FaturamentoDveClienteConcessionariaDadosTabela(gerarHtml, qtdMes, Regis);

            return FaturamentoDveClienteConcessionariaReplaces(gerarHtml, qtdMes, lista);
        }

        private List<eFaturamentoDVEClienteConcessionaria> GetUserRegistroFaturamentoDveConcessionaria(int ano, int mes, int dia)
        {
            try
            {
                dRelatorios relatorio = new dRelatorios();
                return relatorio.GetUserRegistroFaturamentoDveConcessionaria(ano, mes, dia);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion

        #region Invasao

        public string InvasaoHtmlTitulo(eInvasao invasao)
        {
            gerarHtml = new StringBuilder();

            gerarHtml.Append("<div align='center' style='width: 1194px; display:inline-block;'>");
            gerarHtml.Append("<div align='left'style='width: 202px; display:inline; float:left; height: 150px;'>");
            gerarHtml.Append("<img src='[IMG]' alt='Smiley face' height='100' width='200' style='float:right;' /></div>");
            gerarHtml.Append("<div style='width: 807px; display:inline; float:inherit'>");
            gerarHtml.Append("<h2 align='center' style='width:800px; color:[COR]'; font-family:Tahoma; font-size:14pt; fontweight:bold;'>");
            gerarHtml.Append("RELATÓRIO DE INVASÃO <br/>DE " + invasao.DeDia + " DE " + NomeMes(invasao.DeMes) + " - " + invasao.DeAno);
            gerarHtml.Append("<br/>ATÉ " + invasao.AteDia + " DE " + NomeMes(invasao.AteMes) + " - " + invasao.AteAno);
            gerarHtml.Append("<br/>ÁREA EMPLACADA: [Sigla]</h2></div></div>");

            return ReplacesTitulo(gerarHtml.ToString());
        }

        public string InvasaoHtmlTabela(eInvasao invasao)
        {
            gerarHtml = new StringBuilder();

            List<eInvasao> lista = new List<eInvasao>();

            lista = GetUserRegistroInvasao(invasao);

            gerarHtml.Append("<table border='1' [EstiloTabelaBase] id='display'><tr style='color:#000; font-size:20px;'>");
            gerarHtml.Append("<td align='right' width='384'>TOTAL ÁREA: [Sigla]</td><td width='95'>[TArea]</td>");
            gerarHtml.Append("<td align='right' width='368'>TOTAL DE INVASÃO:</td><td width='95'>[TInvasao]</td><td width='95'>[Porc]</td></tr>");
            gerarHtml.Append("<tr style='border:none; font-size:20px;'><td [B]>A</td><td [B]></td><td [B]></td><td [B]></td></tr>");
            gerarHtml.Append("<tr style='background-color:[CorCabecalho];'><td colspan='2'>EMPRESAS</td><td>MUNICIPIOS</td><td>VOL</td><td>%</td></tr>");

            InvasaoSeperandoDados(lista, gerarHtml);

            return InvasaoReplaces(gerarHtml);
        }

        private void InvasaoSeperandoDados(List<eInvasao> lista, StringBuilder gerarHtml)
        {
            List<eInvasao> listaAux = new List<eInvasao>();
            eInvasao inva;

            for (int i = 0; i < lista.Count; i++)
            {
                if (i.Equals(lista.Count - 1))
                {
                    inva = new eInvasao();

                    inva.Qtd = lista[i].Qtd;
                    inva.EmpresaFantasia = lista[i].EmpresaFantasia;
                    inva.SiglaAreaOperacional = lista[i].SiglaAreaOperacional;
                    inva.NomeAreaOperacional = lista[i].NomeAreaOperacional;
                    inva.TotalVolume = lista[i].TotalVolume;
                    inva.TotalArea = lista[i].TotalArea;
                    inva.listaSub = lista.Where(d => d.EmpresaFantasia.Equals(lista[i].EmpresaFantasia)).ToList();

                    listaAux.Add(inva);
                }
                else if (lista[i].EmpresaFantasia != lista[i + 1].EmpresaFantasia)
                {
                    inva = new eInvasao();

                    inva.Qtd = lista[i].Qtd;
                    inva.EmpresaFantasia = lista[i].EmpresaFantasia;
                    inva.SiglaAreaOperacional = lista[i].SiglaAreaOperacional;
                    inva.NomeAreaOperacional = lista[i].NomeAreaOperacional;
                    inva.TotalVolume = lista[i].TotalVolume;
                    inva.TotalArea = lista[i].TotalArea;
                    inva.listaSub = lista.Where(d => d.EmpresaFantasia.Equals(lista[i].EmpresaFantasia)).ToList();

                    listaAux.Add(inva);
                }
            }

            InvasaoDadosTabela(gerarHtml, listaAux);
        }

        private void InvasaoDadosTabela(StringBuilder gerarHtml, List<eInvasao> listaAux)
        {
            int qtdEmp = listaAux.Count;
            int qtdMuni = 0;
            double porcetagem = 0;
            double totalPorc = 0;
            double Porc = 0;
            int totalInvasao = 0;

            for (int i = 0; i < listaAux.Count; i++)
            {
                totalInvasao += Convert.ToInt32(listaAux[i].TotalVolume);
            }

            listaAux = listaAux.OrderByDescending(c => Convert.ToInt32(c.TotalVolume)).ToList();

            for (int i = 0; i < qtdEmp; i++)
            {
                totalPorc = 0;

                totalPorc = (Convert.ToDouble(listaAux[i].TotalVolume) * 100) / totalInvasao;

                qtdMuni = listaAux[i].listaSub.Count();
                gerarHtml.Append("<tr style='color:#000; font-weight:normal;'><td [A] colspan='2' rowspan='" + (qtdMuni + 1) + "'>" + listaAux[i].EmpresaFantasia + "</td>");
                for (int d = 0; d < qtdMuni; d++)
                {
                    porcetagem = (Convert.ToDouble(listaAux[i].listaSub[d].Qtd) * 100) / totalInvasao;

                    gerarHtml.Append("<tr style='color:#000; font-weight:normal;'><td [A]>" + listaAux[i].listaSub[d].Cidade + "</td>");
                    gerarHtml.Append("<td>" + listaAux[i].listaSub[d].Qtd + "</td><td>" + string.Format("{0:0.0}", porcetagem) + "%</td></tr>");
                }
                gerarHtml.Append("<tr style='background-color:#EFEFEF; color:#000;'><td colspan='2' style='border-right:none;'></td><td align='right'>TOTAL</td>");
                gerarHtml.Append("<td>" + listaAux[i].TotalVolume + "</td><td>" + string.Format("{0:0.0}", totalPorc) + "%</td></tr>");
            }
            gerarHtml.Append("</table>");

            Porc = (Convert.ToDouble(totalInvasao) * 100) / Convert.ToDouble(listaAux[0].TotalArea);

            gerarHtml = gerarHtml.Replace("[Sigla]", listaAux[0].SiglaAreaOperacional);
            gerarHtml = gerarHtml.Replace("[TInvasao]", totalInvasao.ToString());
            gerarHtml = gerarHtml.Replace("[TArea]", listaAux[0].TotalArea);
            gerarHtml = gerarHtml.Replace("[Porc]", string.Format("{0:0.0}", Porc) + "%");
        }

        private string InvasaoReplaces(StringBuilder gerarHtml)
        {
            gerarHtml = gerarHtml.Replace("[EstiloTabelaBase]", EstiloTabelaBase);
            gerarHtml = gerarHtml.Replace("[SIZE]", "10");
            gerarHtml = gerarHtml.Replace("[NCelPadding]", "5");
            gerarHtml = gerarHtml.Replace("[CorBordaLinha]", "#" + eConfig.RelatorioCorLinhaAlternada);
            gerarHtml = gerarHtml.Replace("[CorCabecalho]", "#" + eConfig.RelatorioCorCabecalho);
            gerarHtml = gerarHtml.Replace("[B]", "style='border:none;'");
            gerarHtml = gerarHtml.Replace("[A]", "align='left'");

            return gerarHtml.ToString();
        }

        private List<eInvasao> GetUserRegistroInvasao(eInvasao invasao)
        {
            try
            {
                dRelatorios relatorio = new dRelatorios();
                return relatorio.GetUserRegistroInvasao(invasao);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion

        #region InvasaoArea

        public string InvasaoAreaHtmlTitulo(eInvasao invasao)
        {
            gerarHtml = new StringBuilder();

            gerarHtml.Append("<div align='center' style='width: 1194px; display:inline-block;'>");
            gerarHtml.Append("<div align='left'style='width: 202px; display:inline; float:left; height: 150px;'>");
            gerarHtml.Append("<img src='[IMG]' alt='Smiley face' height='100' width='200' style='float:right;' /></div>");
            gerarHtml.Append("<div style='width: 807px; display:inline; float:inherit'>");
            gerarHtml.Append("<h2 align='center' style='width:800px; color:[COR]'; font-family:Tahoma; font-size:14pt; fontweight:bold;'>");
            gerarHtml.Append("RELATÓRIO DE INVASÃO POR ÁREAS <br/>DE " + invasao.DeDia + " DE " + NomeMes(invasao.DeMes) + " - " + invasao.DeAno);
            gerarHtml.Append("<br/>ATÉ " + invasao.AteDia + " DE " + NomeMes(invasao.AteMes) + " - " + invasao.AteAno);
            gerarHtml.Append("</h2></div></div>");

            return ReplacesTitulo(gerarHtml.ToString());
        }

        public string InvasaoAreaHtmlTabela(eInvasao invasao)
        {
            gerarHtml = new StringBuilder();

            List<eInvasao> lista = new List<eInvasao>();

            lista = GetUserRegistroInvasaoArea(invasao);

            gerarHtml.Append("<table border='1' [EstiloTabelaBase] id='display'><tr style='background-color:[CorCabecalho]; font-size:18px;'>");
            gerarHtml.Append("<td width='240'>ÁREA INVADIDA</td><td width='95'>VOL</td><td width='240'>MUNICIPIOS</td><td width='95'>VOL</td>");
            gerarHtml.Append("<td width='250'>EMPRESAS</td><td width='95'>VOL</td><td width='95'>% Invasão Municipio</td><td width='95'>% Invasão Área</td></tr>");

            InvasaoAreaSeperandoDados(lista, gerarHtml);

            return InvasaoAreaReplaces(gerarHtml);
        }

        private void InvasaoAreaSeperandoDados(List<eInvasao> lista, StringBuilder gerarHtml)
        {
            List<eInvasao> listaAux = new List<eInvasao>();
            eInvasao inva;

            for (int i = 0; i < lista.Count; i++)
            {
                if (i.Equals(lista.Count - 1))
                {
                    inva = new eInvasao();

                    inva.SiglaAreaOperacional = lista[i].SiglaAreaOperacional;
                    inva.Cidade = lista[i].Cidade;
                    inva.TotalArea = lista[i].TotalArea;
                    inva.TVolAreaMunicipio = lista[i].TVolAreaMunicipio;
                    inva.listaSub = lista.Where(d => d.Cidade.Equals(lista[i].Cidade)).ToList();

                    listaAux.Add(inva);
                }
                else if (lista[i].Cidade != lista[i + 1].Cidade)
                {
                    inva = new eInvasao();

                    inva.SiglaAreaOperacional = lista[i].SiglaAreaOperacional;
                    inva.Cidade = lista[i].Cidade;
                    inva.TotalArea = lista[i].TotalArea;
                    inva.TVolAreaMunicipio = lista[i].TVolAreaMunicipio;
                    inva.listaSub = lista.Where(d => d.Cidade.Equals(lista[i].Cidade)).ToList();

                    listaAux.Add(inva);
                }
            }

            InvasaoAreaDadosTabela(gerarHtml, listaAux);
        }

        private void InvasaoAreaDadosTabela(StringBuilder gerarHtml, List<eInvasao> listaAux)
        {
            gerarHtml.Append("<tr style='color:#000; font-weight:normal; background-color:#FFF;'><td align='left' rowspan='[Rows]'>" + listaAux[0].SiglaAreaOperacional + "</td>");
            gerarHtml.Append("<td rowspan='[Rows]'>" + listaAux[0].TotalArea + "</td>");

            bool corZebrado = true;
            int linha = 0;

            for (int i = 0; i < listaAux.Count; i++)
            {
                gerarHtml.Append("<tr style='color:#000; font-weight:normal;'><td rowspan='" + (listaAux[i].listaSub.Count + 1) + "'>" + listaAux[i].Cidade + "</td>");
                gerarHtml.Append("<td rowspan='" + (listaAux[i].listaSub.Count + 1) + "'><b>" + listaAux[i].TVolAreaMunicipio + "</b></td>");
                for (int d = 0; d < listaAux[i].listaSub.Count; d++)
                {
                    linha += listaAux[i].listaSub.Count;

                    if (corZebrado)
                    {
                        gerarHtml.Append("<tr style='color:#000; font-weight:normal; background-color:#D3D3D3;'><td align='left'>" + listaAux[i].listaSub[d].EmpresaFantasia + "</td>");
                        gerarHtml.Append("<td>" + listaAux[i].listaSub[d].Qtd + "</td><td><b>" + listaAux[i].listaSub[d].PorcentagemMunicipio + "</b></td>");
                        gerarHtml.Append("<td><b>" + listaAux[i].listaSub[d].PorcentagemArea + "</b></td></tr>");

                        corZebrado = false;
                    }
                    else
                    {
                        gerarHtml.Append("<tr style='color:#000; font-weight:normal; background-color:#FFF;'><td align='left'>" + listaAux[i].listaSub[d].EmpresaFantasia + "</td>");
                        gerarHtml.Append("<td>" + listaAux[i].listaSub[d].Qtd + "</td><td><b>" + listaAux[i].listaSub[d].PorcentagemMunicipio + "</b></td>");
                        gerarHtml.Append("<td><b>" + listaAux[i].listaSub[d].PorcentagemArea + "</b></td></tr>");

                        corZebrado = true;
                    }

                }
                gerarHtml.Append("</tr>");
            }
            gerarHtml.Append("</tr></table>");
            gerarHtml = gerarHtml.Replace("[Rows]", (linha + 1).ToString());
        }

        private string InvasaoAreaReplaces(StringBuilder gerarHtml)
        {
            gerarHtml = gerarHtml.Replace("[EstiloTabelaBase]", EstiloTabelaBase);
            gerarHtml = gerarHtml.Replace("[SIZE]", "10");
            gerarHtml = gerarHtml.Replace("[NCelPadding]", "2");
            gerarHtml = gerarHtml.Replace("[CorBordaLinha]", "#" + eConfig.RelatorioCorLinhaAlternada);
            gerarHtml = gerarHtml.Replace("[CorCabecalho]", "#" + eConfig.RelatorioCorCabecalho);
            gerarHtml = gerarHtml.Replace("[B]", "style='border:none;'");
            gerarHtml = gerarHtml.Replace("[A]", "align='left'");

            return gerarHtml.ToString();
        }

        private List<eInvasao> GetUserRegistroInvasaoArea(eInvasao invasao)
        {
            try
            {
                dRelatorios relatorio = new dRelatorios();
                return relatorio.GetUserRegistroInvasaoArea(invasao);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion

        //Não implementado
        #region BasketMes


        int qtdBasket = 0;
        int qtdCodigo = 0;
        List<eBasket> listaAuxBaskets1 = new List<eBasket>();
        List<eBasket> listaAuxBaskets2 = new List<eBasket>();
        List<eBasket> listaAux1 = new List<eBasket>();
        List<eBasket> listaAux2 = new List<eBasket>();
        List<eBasket> listaUsada = new List<eBasket>();

        public string BasketMesHtmlTitulo(eBasket basket)
        {
            gerarHtml = new StringBuilder();

            return ReplacesTitulo(gerarHtml.ToString());
        }

        public string BasketMesHtmlTabela(eBasket basket)
        {
            gerarHtml = new StringBuilder();

            List<eBasket> lista = new List<eBasket>();
            lista = GetUserRegistroBasketMes(basket);

            int? qtdAnos = basket.Ano - basket.AnoDe;

            BasketMesSeperandoDados(lista);

            gerarHtml.Append("<table border='1' [EstiloTabelaBase]>");

            return gerarHtml.ToString();
        }

        private void BasketMesSeperandoDados(List<eBasket> lista)
        {
            lista = lista.OrderBy(c => c.Descricao).ToList();

            eBasket bask;

            for (int i = 0; i < lista.Count; i++)
            {
                if (i.Equals(lista.Count - 1))
                {

                }
                else if (lista[i].Ano != lista[i + 1].Ano)
                {
                    listaAuxBaskets1 = lista.Where(c => c.Ano == lista[i].Ano).ToList();

                    listaAuxBaskets1 = listaAuxBaskets1.OrderByDescending(c => c.Descricao).ToList();
                    for (int d = 0; d < listaAuxBaskets1.Count; d++)
                    {
                        if (d.Equals(listaAuxBaskets1.Count - 1))
                        {
                            listaAux2 = listaAuxBaskets1.Where(c => c.Descricao == listaAuxBaskets1[d].Descricao).ToList();
                        }
                        else if (listaAuxBaskets1[d].Descricao != listaAuxBaskets1[d + 1].Descricao)
                        {
                            listaAux1 = listaAuxBaskets1.Where(c => c.Descricao == listaAuxBaskets1[d].Descricao).ToList();
                            listaAux1 = listaAux1.OrderByDescending(c => c.Codigo).ToList();

                            for (int f = 0; f < listaAux1.Count; f++)
                            {
                                if (f.Equals(listaAux1.Count - 1))
                                {
                                    //listaAux1 = listaAuxBaskets1.Where(c => c.Descricao == listaAuxBaskets1[d].Descricao).ToList();
                                }
                                else if (listaAux1[f].Codigo != listaAux1[f + 1].Codigo)
                                {
                                    bask = new eBasket();

                                    bask.Ano = listaAux1[f].Ano;
                                    bask.Descricao = listaAux1[f].Descricao;
                                    bask.Codigo = listaAux1[f].Codigo;
                                    bask.NomenclaturaMarca = listaAux1[f].NomenclaturaMarca;
                                    bask.NomenclaturaModelo = listaAux1[f].NomenclaturaModelo;
                                    bask.NomenclaturaVersao = listaAux1[f].NomenclaturaVersao;
                                    bask.listaSub = listaAux1.Where(c => c.Codigo == listaAux1[f].Codigo).ToList();
                                    bask.listaSub = bask.listaSub.OrderBy(c => c.Mes).ToList();
                                }
                            }
                        }
                    }
                }
            }

            listaAuxBaskets1 = lista.Where(c => c.Descricao == lista[0].Descricao).ToList();
            listaAuxBaskets2 = lista.Where(c => c.Descricao == lista[listaAuxBaskets1.Count].Descricao).ToList();

            listaAuxBaskets1 = listaAuxBaskets1.OrderBy(c => c.Codigo).ToList();
            listaAuxBaskets2 = listaAuxBaskets2.OrderBy(c => c.Codigo).ToList();

            for (int i = 0; i < listaAuxBaskets1.Count; i++)
            {
                if (i.Equals(listaAuxBaskets1.Count - 1))
                {
                    bask = new eBasket();

                    bask.Descricao = listaAuxBaskets1[i].Descricao;
                    bask.Codigo = listaAuxBaskets1[i].Codigo;
                    bask.Qtd = listaAuxBaskets1[i].Qtd;
                    bask.NomenclaturaMarca = listaAuxBaskets1[i].NomenclaturaMarca;
                    bask.NomenclaturaModelo = listaAuxBaskets1[i].NomenclaturaModelo;
                    bask.NomenclaturaVersao = listaAuxBaskets1[i].NomenclaturaVersao;
                    bask.Ano = listaAuxBaskets1[i].Ano;
                    bask.Mes = listaAuxBaskets1[i].Mes;
                    bask.Dia = listaAuxBaskets1[i].Dia;
                    bask.TotalMes = listaAuxBaskets1[i].TotalMes;
                    bask.TotalMesGeral = listaAuxBaskets1[i].TotalMesGeral;
                    bask.listaSub = listaAuxBaskets1.Where(c => c.Codigo == listaAuxBaskets1[i].Codigo).ToList();
                    bask.listaSub = bask.listaSub.OrderBy(c => c.Ano).ToList();

                    listaAux1.Add(bask);

                }
                else if (listaAuxBaskets1[i].Codigo != listaAuxBaskets1[i + 1].Codigo)
                {
                    bask = new eBasket();

                    bask.Descricao = listaAuxBaskets1[i].Descricao;
                    bask.Codigo = listaAuxBaskets1[i].Codigo;
                    bask.Qtd = listaAuxBaskets1[i].Qtd;
                    bask.NomenclaturaMarca = listaAuxBaskets1[i].NomenclaturaMarca;
                    bask.NomenclaturaModelo = listaAuxBaskets1[i].NomenclaturaModelo;
                    bask.NomenclaturaVersao = listaAuxBaskets1[i].NomenclaturaVersao;
                    bask.Ano = listaAuxBaskets1[i].Ano;
                    bask.Mes = listaAuxBaskets1[i].Mes;
                    bask.Dia = listaAuxBaskets1[i].Dia;
                    bask.TotalMes = listaAuxBaskets1[i].TotalMes;
                    bask.TotalMesGeral = listaAuxBaskets1[i].TotalMesGeral;
                    bask.listaSub = listaAuxBaskets1.Where(c => c.Codigo == listaAuxBaskets1[i].Codigo).ToList();
                    bask.listaSub = bask.listaSub.OrderBy(c => c.Ano).ToList();

                    listaAux1.Add(bask);
                }
            }

            for (int i = 0; i < listaAuxBaskets2.Count; i++)
            {
                if (i.Equals(listaAuxBaskets2.Count - 1))
                {
                    bask = new eBasket();

                    bask.Descricao = listaAuxBaskets2[i].Descricao;
                    bask.Codigo = listaAuxBaskets2[i].Codigo;
                    bask.Qtd = listaAuxBaskets2[i].Qtd;
                    bask.NomenclaturaMarca = listaAuxBaskets2[i].NomenclaturaMarca;
                    bask.NomenclaturaModelo = listaAuxBaskets2[i].NomenclaturaModelo;
                    bask.NomenclaturaVersao = listaAuxBaskets2[i].NomenclaturaVersao;
                    bask.Ano = listaAuxBaskets2[i].Ano;
                    bask.Mes = listaAuxBaskets2[i].Mes;
                    bask.Dia = listaAuxBaskets2[i].Dia;
                    bask.TotalMes = listaAuxBaskets2[i].TotalMes;
                    bask.TotalMesGeral = listaAuxBaskets2[i].TotalMesGeral;
                    bask.listaSub = listaAuxBaskets2.Where(c => c.Codigo == listaAuxBaskets2[i].Codigo).ToList();
                    bask.listaSub = bask.listaSub.OrderBy(c => c.Ano).ToList();

                    listaAux2.Add(bask);

                }
                else if (listaAuxBaskets2[i].Codigo != listaAuxBaskets2[i + 1].Codigo)
                {
                    bask = new eBasket();

                    bask.Descricao = listaAuxBaskets2[i].Descricao;
                    bask.Codigo = listaAuxBaskets2[i].Codigo;
                    bask.Qtd = listaAuxBaskets2[i].Qtd;
                    bask.NomenclaturaMarca = listaAuxBaskets2[i].NomenclaturaMarca;
                    bask.NomenclaturaModelo = listaAuxBaskets2[i].NomenclaturaModelo;
                    bask.NomenclaturaVersao = listaAuxBaskets2[i].NomenclaturaVersao;
                    bask.Ano = listaAuxBaskets2[i].Ano;
                    bask.Mes = listaAuxBaskets2[i].Mes;
                    bask.Dia = listaAuxBaskets2[i].Dia;
                    bask.TotalMes = listaAuxBaskets2[i].TotalMes;
                    bask.TotalMesGeral = listaAuxBaskets2[i].TotalMesGeral;
                    bask.listaSub = listaAuxBaskets2.Where(c => c.Codigo == listaAuxBaskets2[i].Codigo).ToList();
                    bask.listaSub = bask.listaSub.OrderBy(c => c.Ano).ToList();

                    listaAux2.Add(bask);
                }
            }

            listaAux1 = listaAux1.OrderBy(c => c.Ano).ToList();
            listaAux2 = listaAux2.OrderBy(c => c.Ano).ToList();
        }

        private List<eBasket> GetUserRegistroBasketMes(eBasket basket)
        {
            try
            {
                dRelatorios relatorio = new dRelatorios();
                return relatorio.GetUserRegistroBasketMes(basket);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region BasketAno

        int? qtdAno = 0;
        int?[] Anos;

        public string BasketAnoHtmlTitulo(eBasket basket)
        {
            gerarHtml = new StringBuilder();

            gerarHtml.Append("<div align='center' style='width: 1194px; display:inline-block;'>");
            gerarHtml.Append("<div align='left'style='width: 202px; display:inline; float:left; height: 150px;'>");
            gerarHtml.Append("<img src='[IMG]' alt='Smiley face' height='100' width='200' style='float:right;' /></div>");
            gerarHtml.Append("<div style='width: 807px; display:inline; float:inherit'>");
            gerarHtml.Append("<h2 align='center' style='width:800px; color:[COR]'; font-family:Tahoma; font-size:14pt; fontweight:bold;'>");
            gerarHtml.Append("RELATÓRIO BASKET ANUAL<br/>DE " + basket.DiaDe + " DE " + NomeMes(basket.MesDe) + " - " + basket.AnoDe);
            gerarHtml.Append("<br/>ATÉ " + basket.Dia + " DE " + NomeMes(basket.Mes) + " - " + basket.Ano);

            if (basket.RegiaoOperacional == null && basket.RegiaoGeografico == null && basket.RegiaoMetropolitana == null && basket.Estado == null
                && basket.Cidade == null && basket.AreaOperacional == null)
            {
                gerarHtml.Append("<br/>NACIONAL TODAS</h2></div></div>");
            }
            else
            {
                if (basket.RegiaoOperacional != null)
                {
                    gerarHtml.Append("<br/>REGIONAL: [RO]</h2></div></div>");
                }
                if (basket.RegiaoGeografico != null)
                {
                    gerarHtml.Append("<br/>REGIÃO: [RG]</h2></div></div>");
                }
                if (basket.RegiaoMetropolitana != null)
                {
                    gerarHtml.Append("<br/>METRÓPOLE: [RM]</h2></div></div>");
                }
                if (basket.Estado != null)
                {
                    gerarHtml.Append("<br/>ESTADO: [ES]</h2></div></div>");
                }
                if (basket.Cidade != null)
                {
                    gerarHtml.Append("<br/>CIDADE: [CI]</h2></div></div>");
                }
                if (basket.AreaOperacional != null)
                {
                    gerarHtml.Append("<br/>ÁREA DE INFLUENCIA: [AO]</h2></div></div>");
                }
            }

            return ReplacesTitulo(gerarHtml.ToString());
        }

        public string BasketAnoHtmlTabela(eBasket basket)
        {
            gerarHtml = new StringBuilder();

            List<eBasket> lista = new List<eBasket>();

            lista = GetUserRegistroBasketAno(basket);

            qtdAno = (basket.Ano - basket.AnoDe) + 1;
            Anos = new int?[Convert.ToInt32(qtdAno.ToString())];
            for (int i = 0; i < qtdAno; i++)
            {
                Anos[i] = basket.AnoDe + i;
            }

            gerarHtml.Append("<table border='1' [EstiloTabelaBase] id='display'><tr style='background-color:[CorCabecalho]'><td width='95'>BASKET</td>");
            gerarHtml.Append("<td width='95'>CÓDIGO</td><td width='260'>MODELO</td>");

            if (qtdAno != null)
            {
                for (int i = 0; i < qtdAno; i++)
                {
                    gerarHtml.Append("<td width='95'>[Ano" + i + "]</td>");
                }
            }
            gerarHtml.Append("<td width='95'>TOTAL</td><td width='95'>%</td></tr>");

            BasketAnoSeperandoDados(lista, gerarHtml);

            return BasketAnoReplaces(gerarHtml);
        }

        private void BasketAnoSeperandoDados(List<eBasket> lista, StringBuilder gerarHtml)
        {
            List<eBasket> listaAux1 = new List<eBasket>();
            List<eBasket> listaAux2 = new List<eBasket>();
            int qtdAux1 = 1;
            int qtdAux2 = 1;

            eBasket bask;

            lista = lista.OrderByDescending(c => c.BasketId).ToList();

            for (int i = 0; i < lista.Count; i++)
            {
                if (i.Equals(lista.Count - 1))
                {
                    for (int d = 0; d < lista.Count; d++)
                    {
                        if (lista[d].Descricao != listaAux1[0].Descricao)
                        {
                            bask = new eBasket();

                            bask.BasketId = lista[d].BasketId;
                            bask.Descricao = lista[d].Descricao;
                            bask.Codigo = lista[d].Codigo;
                            bask.NomenclaturaMarca = lista[d].NomenclaturaMarca;
                            bask.NomenclaturaModelo = lista[d].NomenclaturaModelo;
                            bask.NomenclaturaVersao = lista[d].NomenclaturaVersao;
                            bask.Total = lista[d].Total;
                            bask.TotalAno = lista[d].TotalAno;
                            bask.TotalGeral = lista[d].TotalGeral;
                            bask.Porcentagem = lista[d].Porcentagem;
                            bask.listaSub = lista.Where(c => c.Codigo == lista[d].Codigo).ToList();
                            bask.listaSub = bask.listaSub.OrderBy(c => c.Ano).ToList();

                            listaAux2.Add(bask);
                        }
                    }
                }
                else if (lista[i].Descricao != lista[lista.Count - 1].Descricao)
                {
                    bask = new eBasket();

                    bask.BasketId = lista[i].BasketId;
                    bask.Descricao = lista[i].Descricao;
                    bask.Codigo = lista[i].Codigo;
                    bask.NomenclaturaMarca = lista[i].NomenclaturaMarca;
                    bask.NomenclaturaModelo = lista[i].NomenclaturaModelo;
                    bask.NomenclaturaVersao = lista[i].NomenclaturaVersao;
                    bask.Total = lista[i].Total;
                    bask.TotalAno = lista[i].TotalAno;
                    bask.TotalGeral = lista[i].TotalGeral;
                    bask.Porcentagem = lista[i].Porcentagem;
                    bask.listaSub = lista.Where(c => c.Codigo == lista[i].Codigo).ToList();
                    bask.listaSub = bask.listaSub.OrderBy(c => c.Ano).ToList();

                    listaAux1.Add(bask);
                }
            }

            for (int i = 0; i < listaAux1.Count; i++)
            {
                if (i.Equals(listaAux1.Count - 1))
                {

                }
                else if (listaAux1[i].Codigo != listaAux1[i + 1].Codigo)
                {
                    qtdAux1 += 1;
                }
            }

            for (int i = 0; i < listaAux2.Count; i++)
            {
                if (i.Equals(listaAux2.Count - 1))
                {

                }
                else if (listaAux2[i].Codigo != listaAux2[i + 1].Codigo)
                {
                    qtdAux2 += 1;
                }
            }

            BasketAnoDadosTabela(gerarHtml, listaAux1, listaAux2, qtdAux1, qtdAux2);
        }

        private void BasketAnoDadosTabela(StringBuilder gerarHtml, List<eBasket> listaAux1, List<eBasket> listaAux2, int qtdAux1, int qtdAux2)
        {
            int index = 0;
            gerarHtml.Append("<tr style='color:#000; font-weight:normal;'><td rowspan='" + (qtdAux1 + 2) + "'>" + listaAux1[0].Descricao + "</td>");
            for (int i = 0; i < listaAux1.Count; i++)
            {
                if (i.Equals(listaAux1.Count - 1))
                {
                    gerarHtml.Append("<tr style='color:#000; font-weight:normal;'><td align='left'>" + listaAux1[i].Codigo + "</td>");
                    gerarHtml.Append("<td align='left'>" + listaAux1[i].NomenclaturaMarca + " \\ " + listaAux1[i].NomenclaturaModelo + " \\ " + listaAux1[i].NomenclaturaVersao + "</td>");
                    if (listaAux1[i].listaSub.Count != qtdAno)
                    {
                        for (int d = 0; d < qtdAno; d++)
                        {
                            if (listaAux1[i].listaSub[listaAux1[i].listaSub.Count - 1].Ano != Anos[Convert.ToInt32(qtdAno) - 1])
                            {
                                listaAux1[i].listaSub.Insert(listaAux1[i].listaSub.Count - 1, new eBasket { Ano = Convert.ToInt32(Anos[Convert.ToInt32(qtdAno) - 1].ToString()), Total = "", Qtd = 0 });
                            }

                            listaAux1[i].listaSub = listaAux1[i].listaSub.OrderBy(c => c.Ano).ToList();

                            if (listaAux1[i].listaSub[d].Ano != Anos[d])
                            {
                                listaAux1[i].listaSub.Insert(d, new eBasket { Ano = Convert.ToInt32(Anos[d].ToString()), Total = "", Qtd = 0 });
                            }
                        }

                        listaAux1[i].listaSub.OrderBy(c => c.Ano).ToList();

                        for (int d = 0; d < qtdAno; d++)
                        {
                            if (listaAux1[i].listaSub[d].Qtd == 0)
                            {
                                gerarHtml.Append("<td></td>");
                            }
                            else
                            {
                                gerarHtml.Append("<td>" + listaAux1[i].listaSub[d].Qtd + "</td>");
                            }
                        }

                        gerarHtml.Append("<td><b>" + listaAux1[i].Total + "</b></td><td>" + listaAux1[i].Porcentagem + "%</td></tr>");
                    }
                    else
                    {
                        for (int d = 0; d < qtdAno; d++)
                        {
                            if (listaAux1[i].listaSub[d].Qtd == 0)
                            {
                                gerarHtml.Append("<td></td>");
                            }
                            else
                            {
                                gerarHtml.Append("<td>" + listaAux1[i].listaSub[d].Qtd + "</td>");
                            }
                        }

                        gerarHtml.Append("<td><b>" + listaAux1[i].Total + "</b></td><td>" + listaAux1[i].Porcentagem + "%</td></tr>");
                    }
                    gerarHtml.Append("<tr style='background-color:#D3D3D3; font-weight:bold; color:#000;'><td></td><td align='left'>TOTAL " + listaAux1[0].Descricao + "</td>");
                    for (int d = 0; d < qtdAno; d++)
                    {
                        gerarHtml.Append("<td>" + listaAux1[index].listaSub[d].TotalAno + "</td>");
                    }
                    gerarHtml.Append("<td>" + listaAux1[i].TotalGeral + "</td><td style='background-color:#FFF;'></td>");
                }
                else if (listaAux1[i].Codigo != listaAux1[i + 1].Codigo)
                {
                    gerarHtml.Append("<tr style='color:#000; font-weight:normal;'><td align='left'>" + listaAux1[i].Codigo + "</td>");
                    gerarHtml.Append("<td align='left'>" + listaAux1[i].NomenclaturaMarca + " \\ " + listaAux1[i].NomenclaturaModelo + " \\ " + listaAux1[i].NomenclaturaVersao + "</td>");
                    if (listaAux1[i].listaSub.Count != qtdAno)
                    {
                        for (int d = 0; d < qtdAno; d++)
                        {
                            if (listaAux1[i].listaSub[listaAux1[i].listaSub.Count - 1].Ano != Anos[Convert.ToInt32(qtdAno) - 1])
                            {
                                listaAux1[i].listaSub.Insert(listaAux1[i].listaSub.Count - 1, new eBasket { Ano = Convert.ToInt32(Anos[Convert.ToInt32(qtdAno) - 1].ToString()), Total = "", Qtd = 0 });
                            }

                            listaAux1[i].listaSub = listaAux1[i].listaSub.OrderBy(c => c.Ano).ToList();

                            if (listaAux1[i].listaSub[d].Ano != Anos[d])
                            {
                                listaAux1[i].listaSub.Insert(d, new eBasket { Ano = Convert.ToInt32(Anos[d].ToString()), Total = "", Qtd = 0 });
                            }
                        }

                        for (int d = 0; d < qtdAno; d++)
                        {
                            if (listaAux1[i].listaSub[d].Qtd == 0)
                            {
                                gerarHtml.Append("<td></td>");
                            }
                            else
                            {
                                gerarHtml.Append("<td>" + listaAux1[i].listaSub[d].Qtd + "</td>");
                            }
                        }

                        gerarHtml.Append("<td><b>" + listaAux1[i].Total + "</b></td><td>" + listaAux1[i].Porcentagem + "%</td></tr>");
                    }
                    else
                    {
                        index = i;
                        for (int d = 0; d < qtdAno; d++)
                        {
                            if (listaAux1[i].listaSub[d].Qtd == 0)
                            {
                                gerarHtml.Append("<td></td>");
                            }
                            else
                            {
                                gerarHtml.Append("<td>" + listaAux1[i].listaSub[d].Qtd + "</td>");
                            }
                        }

                        gerarHtml.Append("<td><b>" + listaAux1[i].Total + "</b></td><td>" + listaAux1[i].Porcentagem + "%</td></tr>");
                    }
                }

            }


            //----------

            gerarHtml.Append("<tr style='color:#000; font-weight:normal;'><td rowspan='" + (qtdAux2 + 2) + "'>" + listaAux2[0].Descricao + "</td>");
            for (int i = 0; i < listaAux2.Count; i++)
            {
                if (i.Equals(listaAux2.Count - 1))
                {
                    gerarHtml.Append("<tr style='color:#000; font-weight:normal;'><td align='left'>" + listaAux2[i].Codigo + "</td>");
                    gerarHtml.Append("<td align='left'>" + listaAux2[i].NomenclaturaMarca + " \\ " + listaAux2[i].NomenclaturaModelo + " \\ " + listaAux2[i].NomenclaturaVersao + "</td>");
                    if (listaAux2[i].listaSub.Count != qtdAno)
                    {
                        for (int d = 0; d < qtdAno; d++)
                        {
                            if (listaAux2[i].listaSub[listaAux2[i].listaSub.Count - 1].Ano != Anos[Convert.ToInt32(qtdAno) - 1])
                            {
                                listaAux2[i].listaSub.Insert(listaAux2[i].listaSub.Count - 1, new eBasket { Ano = Convert.ToInt32(Anos[Convert.ToInt32(qtdAno) - 1].ToString()), Total = "", Qtd = 0 });
                            }

                            listaAux2[i].listaSub = listaAux2[i].listaSub.OrderBy(c => c.Ano).ToList();

                            if (listaAux2[i].listaSub[d].Ano != Anos[d])
                            {
                                listaAux2[i].listaSub.Insert(d, new eBasket { Ano = Convert.ToInt32(Anos[d].ToString()), Total = "", Qtd = 0 });
                            }
                        }

                        listaAux2[i].listaSub.OrderBy(c => c.Ano).ToList();

                        for (int d = 0; d < qtdAno; d++)
                        {
                            if (listaAux2[i].listaSub[d].Qtd == 0)
                            {
                                gerarHtml.Append("<td></td>");
                            }
                            else
                            {
                                gerarHtml.Append("<td>" + listaAux2[i].listaSub[d].Qtd + "</td>");
                            }
                        }

                        gerarHtml.Append("<td><b>" + listaAux2[i].Total + "</b></td><td>" + listaAux2[i].Porcentagem + "%</td></tr>");
                    }
                    else
                    {
                        for (int d = 0; d < qtdAno; d++)
                        {
                            if (listaAux2[i].listaSub[d].Qtd == 0)
                            {
                                gerarHtml.Append("<td></td>");
                            }
                            else
                            {
                                gerarHtml.Append("<td>" + listaAux2[i].listaSub[d].Qtd + "</td>");
                            }
                        }

                        gerarHtml.Append("<td><b>" + listaAux2[i].Total + "</b></td><td>" + listaAux2[i].Porcentagem + "%</td></tr>");
                    }
                    gerarHtml.Append("<tr style='background-color:#D3D3D3; font-weight:bold;color:#000;'><td></td><td align='left'>TOTAL " + listaAux2[0].Descricao + "</td>");
                    for (int d = 0; d < qtdAno; d++)
                    {
                        gerarHtml.Append("<td>" + listaAux2[index].listaSub[d].TotalAno + "</td>");
                    }
                    gerarHtml.Append("<td>" + listaAux2[i].TotalGeral + "</td><td style='background-color:#FFF;'></td>");
                }
                else if (listaAux2[i].Codigo != listaAux2[i + 1].Codigo)
                {
                    gerarHtml.Append("<tr style='color:#000; font-weight:normal;'><td align='left'>" + listaAux2[i].Codigo + "</td>");
                    gerarHtml.Append("<td align='left'>" + listaAux2[i].NomenclaturaMarca + " \\ " + listaAux2[i].NomenclaturaModelo + " \\ " + listaAux2[i].NomenclaturaVersao + "</td>");
                    if (listaAux2[i].listaSub.Count != qtdAno)
                    {
                        for (int d = 0; d < qtdAno; d++)
                        {
                            if (listaAux2[i].listaSub[listaAux2[i].listaSub.Count - 1].Ano != Anos[Convert.ToInt32(qtdAno) - 1])
                            {
                                listaAux2[i].listaSub.Insert(listaAux2[i].listaSub.Count - 1, new eBasket { Ano = Convert.ToInt32(Anos[Convert.ToInt32(qtdAno) - 1].ToString()), Total = "", Qtd = 0 });
                            }

                            listaAux2[i].listaSub = listaAux2[i].listaSub.OrderBy(c => c.Ano).ToList();

                            if (listaAux2[i].listaSub[d].Ano != Anos[d])
                            {
                                listaAux2[i].listaSub.Insert(d, new eBasket { Ano = Convert.ToInt32(Anos[d].ToString()), Total = "", Qtd = 0 });
                            }
                        }

                        for (int d = 0; d < qtdAno; d++)
                        {
                            if (listaAux2[i].listaSub[d].Qtd == 0)
                            {
                                gerarHtml.Append("<td></td>");
                            }
                            else
                            {
                                gerarHtml.Append("<td>" + listaAux2[i].listaSub[d].Qtd + "</td>");
                            }
                        }
                        gerarHtml.Append("<td><b>" + listaAux2[i].Total + "</b></td><td>" + listaAux2[i].Porcentagem + "%</td></tr>");
                    }
                    else
                    {
                        index = i;
                        for (int d = 0; d < qtdAno; d++)
                        {
                            if (listaAux2[i].listaSub[d].Qtd == 0)
                            {
                                gerarHtml.Append("<td></td>");
                            }
                            else
                            {
                                gerarHtml.Append("<td>" + listaAux2[i].listaSub[d].Qtd + "</td>");
                            }
                        }

                        gerarHtml.Append("<td><b>" + listaAux2[i].Total + "</b></td><td>" + listaAux2[i].Porcentagem + "%</td></tr>");
                    }
                }
            }

        }

        private string BasketAnoReplaces(StringBuilder gerarHtml)
        {
            gerarHtml = gerarHtml.Replace("[EstiloTabelaBase]", EstiloTabelaBase);
            gerarHtml = gerarHtml.Replace("[SIZE]", "10");
            gerarHtml = gerarHtml.Replace("[NCelPadding]", "2");
            gerarHtml = gerarHtml.Replace("[CorBordaLinha]", "#" + eConfig.RelatorioCorLinhaAlternada);
            gerarHtml = gerarHtml.Replace("[CorCabecalho]", "#" + eConfig.RelatorioCorCabecalho);

            for (int i = 0; i < qtdAno; i++)
            {
                gerarHtml = gerarHtml.Replace("[Ano" + i + "]", Anos[i].ToString());
            }

            return gerarHtml.ToString();
        }

        private List<eBasket> GetUserRegistroBasketAno(eBasket basket)
        {
            try
            {
                dRelatorios relatorio = new dRelatorios();
                return relatorio.GetUserRegistroBasketAno(basket);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region BasketPeriodo

        int Zebrado = 0;

        public string BasketPeriodoHtmlTitulo(eBasketPeriodo bp)
        {
            gerarHtml = new StringBuilder();

            gerarHtml.Append("<div align='center' style='width: 1194px; display:inline-block;'>");
            gerarHtml.Append("<div align='left'style='width: 202px; display:inline; float:left; height: 150px;'>");
            gerarHtml.Append("<img src='[IMG]' alt='Smiley face' height='100' width='200' style='float:right;' /></div>");
            gerarHtml.Append("<div style='width: 807px; display:inline; float:inherit'>");
            gerarHtml.Append("<h3 align='center' style='width:800px; color:[COR]'; font-family:Tahoma; font-size:14pt; fontweight:bold;'>");
            gerarHtml.Append("BASKET PERÍODO<br/>1° PERIODO:" + bp.DiaDe + "/" + bp.MesDe + "/" + bp.AnoDe + " ATÉ " + bp.Dia + "/" + bp.Mes + "/" + bp.Ano);
            gerarHtml.Append("<br/>2° PERÍODO: ATÉ " + bp.SegDiaDe + "/" + bp.SegMesDe + "/" + bp.SegAnoDe + " ATÉ " + bp.SegDia + "/" + bp.SegMes + "/" + bp.SegAno);

            if (bp.RegiaoOperacional == null && bp.RegiaoGeografico == null && bp.RegiaoMetropolitana == null && bp.Estado == null
               && bp.Cidade == null && bp.AreaOperacional == null)
            {
                gerarHtml.Append("<br/>NACIONAL TODAS</h3></div></div>");
            }
            else
            {
                if (bp.RegiaoOperacional != null)
                {
                    gerarHtml.Append("<br/>REGIONAL: [RO]</h3></div></div>");
                }
                if (bp.RegiaoGeografico != null)
                {
                    gerarHtml.Append("<br/>REGIÃO: [RG]</h3></div></div>");
                }
                if (bp.RegiaoMetropolitana != null)
                {
                    gerarHtml.Append("<br/>METRÓPOLE: [RM]</h3></div></div>");
                }
                if (bp.Estado != null)
                {
                    gerarHtml.Append("<br/>ESTADO: [ES]</h3></div></div>");
                }
                if (bp.Cidade != null)
                {
                    gerarHtml.Append("<br/>CIDADE: [CI]</h3></div></div>");
                }
                if (bp.AreaOperacional != null)
                {
                    gerarHtml.Append("<br/>ÁREA DE INFLUENCIA: [AO]</h3></div></div>");
                }
            }

            return ReplacesTitulo(gerarHtml.ToString());
        }

        public string BasketPeriodoHtmlTabela(eBasketPeriodo bp)
        {
            gerarHtml = new StringBuilder();

            List<eBasketPeriodo> lista = new List<eBasketPeriodo>();
            lista = GetUserRegistroBaasketPeriodo(bp);

            gerarHtml.Append("<table border='1' [EstiloTabelaBase] id='display'><tr style='background-color:[CorCabecalho];'><td width='95'></td>");
            gerarHtml.Append("<td width='95'></td><td width='280'></td><td width='190'>1º PERIODO</td><td width='190'>2º PERIODO</td>");
            gerarHtml.Append("<td width='95'></td></tr><tr style='background-color:[CorCabecalho];'><td>BASKET</td><td>CÓDIGO</td><td>MODELO</td>");
            gerarHtml.Append("<td>" + bp.DiaDe + "/" + bp.MesDe + "/" + bp.AnoDe + " ATÉ " + bp.Dia + "/" + bp.Mes + "/" + bp.Ano + "</td><td>" + bp.SegDiaDe + "/");
            gerarHtml.Append(bp.SegMesDe + "/" + bp.SegAnoDe + " ATÉ " + bp.SegDia + "/" + bp.SegMes + "/" + bp.SegAno + "</td><td>Evolução periodos</td></tr>");
            gerarHtml.Append("<tr style='color:#000; font-wight:normal;'><td>&nbsp</td><td>&nbsp</td><td align='right' style='color:[CorCabecalho];'>DIAS UTEIS</td>");
            gerarHtml.Append("<td style='color:[CorCabecalho];'>" + lista[0].DiasUteis1 + "</td><td style='color:[CorCabecalho];'>" + lista[0].DiasUteis2 + "</td><td>&nbsp</td></tr>");

            BasketPeriodoSeparandoTabela(gerarHtml, lista);

            return BasketPeriodoReplaces(gerarHtml);
        }

        private void BasketPeriodoSeparandoTabela(StringBuilder gerarHtml, List<eBasketPeriodo> lista)
        {
            List<eBasketPeriodo> listaDescricao = new List<eBasketPeriodo>();
            List<eBasketPeriodo> listaUsada = new List<eBasketPeriodo>();
            listaDescricao = lista;
            listaDescricao = listaDescricao.OrderBy(o => o.Descricao).ToList();

            for (int i = 0; i < lista.Count; i++)
            {
                if (i.Equals(lista.Count - 1))
                {
                    listaUsada = listaDescricao.Where(w => w.Descricao == listaDescricao[i].Descricao).ToList();

                    BasketPeriodoDadosTabela(gerarHtml, listaUsada);
                }
                else if (listaDescricao[i].Descricao != listaDescricao[i + 1].Descricao)
                {
                    listaUsada = listaDescricao.Where(w => w.Descricao == listaDescricao[i].Descricao).ToList();

                    BasketPeriodoDadosTabela(gerarHtml, listaUsada);
                }
            }
        }

        private void BasketPeriodoDadosTabela(StringBuilder gerarHtml, List<eBasketPeriodo> listaUsada)
        {

            listaUsada = listaUsada.OrderByDescending(od1 => od1.Periodo1).ToList();

            if (Zebrado % 2 == 0)
            {
                gerarHtml.Append("<tr style='color:#000; font-weight:normal;'><td rowspan='" + (listaUsada.Count + 2) + "'>" + listaUsada[0].Descricao + "</td>");
            }
            else
            {
                gerarHtml.Append("<tr style='color:#000; font-weight:normal; [Z]'><td rowspan='" + (listaUsada.Count + 2) + "'>" + listaUsada[0].Descricao + "</td>");
            }

            for (int i = 0; i < listaUsada.Count; i++)
            {
                if (i % 2 == 0)
                {
                    int evolucao = Convert.ToInt32(listaUsada[i].Periodo2) - Convert.ToInt32(listaUsada[i].Periodo1);

                    gerarHtml.Append("<tr style='color:#000; font-weight:normal; [Z]'><td align='left'>" + listaUsada[i].Codigo + "</td>");
                    gerarHtml.Append("<td align='left'>" + listaUsada[i].Marca + " \\ " + listaUsada[i].Modelo + " \\ " + listaUsada[i].Versao + "</td>");
                    gerarHtml.Append("<td>" + listaUsada[i].Periodo1 + "</td><td>" + listaUsada[i].Periodo2 + "</td>");
                    if (evolucao < 0)
                    {
                        gerarHtml.Append("<td style='color:#FC1467;'>" + evolucao + "</td>");
                    }
                    else
                    {
                        gerarHtml.Append("<td>" + evolucao + "</td>");
                    }
                }
                else
                {
                    int evolucao = Convert.ToInt32(listaUsada[i].Periodo2) - Convert.ToInt32(listaUsada[i].Periodo1);

                    gerarHtml.Append("<tr style='color:#000; font-weight:normal;'><td align='left'>" + listaUsada[i].Codigo + "</td>");
                    gerarHtml.Append("<td align='left'>" + listaUsada[i].Marca + " \\ " + listaUsada[i].Modelo + " \\ " + listaUsada[i].Versao + "</td>");
                    gerarHtml.Append("<td>" + listaUsada[i].Periodo1 + "</td><td>" + listaUsada[i].Periodo2 + "</td>");

                    if (evolucao < 0)
                    {
                        gerarHtml.Append("<td style='color:#FC1467;'>" + evolucao + "</td>");
                    }
                    else
                    {
                        gerarHtml.Append("<td>" + evolucao + "</td>");
                    }
                }
            }

            gerarHtml.Append("<tr style='color:#000; background-color:#EFEFEF;'><td></td><td align='right'>TOTAL " + listaUsada[0].Descricao + "</td>");
            gerarHtml.Append("<td>" + listaUsada.Sum(s => Convert.ToInt32(s.Periodo1)) + "</td><td>" + listaUsada.Sum(s => Convert.ToInt32(s.Periodo2)) + "</td>");
            gerarHtml.Append("<td></td></tr></tr>");

            Zebrado++;
        }

        private string BasketPeriodoReplaces(StringBuilder gerarHtml)
        {
            gerarHtml = gerarHtml.Replace("[EstiloTabelaBase]", EstiloTabelaBase);
            gerarHtml = gerarHtml.Replace("[SIZE]", "10");
            gerarHtml = gerarHtml.Replace("[NCelPadding]", "2");
            gerarHtml = gerarHtml.Replace("[CorBordaLinha]", "#" + eConfig.RelatorioCorLinhaAlternada);
            gerarHtml = gerarHtml.Replace("[CorCabecalho]", "#" + eConfig.RelatorioCorCabecalho);
            gerarHtml = gerarHtml.Replace("[Z]", "background-color:#DDD;");

            gerarHtml.Append("</table>");

            return gerarHtml.ToString();
        }

        private List<eBasketPeriodo> GetUserRegistroBaasketPeriodo(eBasketPeriodo perido)
        {
            try
            {
                dRelatorios relatorio = new dRelatorios();
                return relatorio.GetUserRegistroBaasketPeriodo(perido);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        //Não implementado
        #region GraficoMarketShare
        #endregion

        //Não implementado
        #region GraficoComparativoMontadoras
        #endregion

        #region SegmentoFUModalidade

        public string SegmentoFUModalidadeHtmlTitulo(eSegmentoFU fuMod)
        {
            gerarHtml = new StringBuilder();

            gerarHtml.Append("<div align='center' style='width: 1194px; display:inline-block;'>");
            gerarHtml.Append("<div align='left'style='width: 202px; display:inline; float:left; height: 150px;'>");
            gerarHtml.Append("<img src='[IMG]' alt='Smiley face' height='100' width='200' style='float:right;' /></div>");
            gerarHtml.Append("<div style='width: 807px; display:inline; float:inherit'>");
            gerarHtml.Append("<h2 align='center' style='width:800px; color:[COR]'; font-family:Tahoma; font-size:14pt; fontweight:bold;'>");
            gerarHtml.Append("FU MODALIDADES<br/>DE " + fuMod.DiaDe + " DE " + NomeMes(fuMod.MesDe) + " - " + fuMod.AnoDe);
            gerarHtml.Append("<br/>ATÉ " + fuMod.Dia + " DE " + NomeMes(fuMod.Mes) + " - " + fuMod.Ano + "</h2></div></div>");

            return ReplacesTitulo(gerarHtml.ToString());
        }

        public string SegmentoFUModalidadeHtmlTabela(eSegmentoFU fuMod, int valor)
        {
            gerarHtml = new StringBuilder();

            List<eSegmentoFU> lista = new List<eSegmentoFU>();
            int qtdColunasCabecalho = 0;
            string Venda = "T";
            string[] vetor = { "VD", "Varejo" };

            lista = GetUserRegistroSegmentoFUModalidade(fuMod);

            if (valor.Equals(0))
            {
                qtdColunasCabecalho = 2;
            }
            else if (valor.Equals(1))
            {
                Venda = "VD";
                qtdColunasCabecalho = 1;
            }
            else
            {
                Venda = "VJ";
                qtdColunasCabecalho = 1;
            }
            gerarHtml.Append("<table [EstiloTabelaBase] id='display'><tr style='color:#000;'><td colspan='3' style='border:none;'></td>");
            for (int i = 0; i < qtdColunasCabecalho; i++)
            {
                if (i.Equals(1))
                {
                    gerarHtml.Append("<td width='185' colspan='3' style='background-color:#D3D3D3; [A]'>Varejo</td>");
                }
                else
                {
                    if (Venda.Equals("VD"))
                    {
                        gerarHtml.Append("<td width='185' colspan='3' [B]>VD</td>");
                    }
                    else if (Venda.Equals("VJ"))
                    {
                        gerarHtml.Append("<td width='185' colspan='3' style='background-color:#D3D3D3; [A]'>Varejo</td>");
                    }
                    else
                    {
                        gerarHtml.Append("<td width='185' colspan='3' [B]>VD</td>");
                    }
                }
            }
            if (Venda.Equals("T"))
            {
                gerarHtml.Append("<td width='200' colspan='3' [B]>Total de Vendas</td></tr>");
            }

            gerarHtml.Append("<tr style='background-color:[CorCabecalho]; [A]'><td width='95' [B]>Segmento</td><td width='200' [B]>Modelo veiculo</td><td width='150' [B]>Marca</td>");
            for (int i = 0; i < qtdColunasCabecalho; i++)
            {
                gerarHtml.Append("<td width='75' [B]>VOL</td><td width='75' [B]>% Seg</td><td width='75' [B]>% Total</td>");
            }
            if (Venda.Equals("T"))
            {
                gerarHtml.Append("<td width='90' [B]>VOL</td><td width='75' [B]>% Seg</td><td width='75' [B]>% Total</td></tr>");
            }

            SegmentoFUModalidadeSeparandoDados(lista, gerarHtml, qtdColunasCabecalho, Venda);

            return SegmentoFUModalidadeReplaces(gerarHtml);
        }

        private void SegmentoFUModalidadeSeparandoDados(List<eSegmentoFU> lista, StringBuilder gerarHtml, int qtdColunasCabecalho, string Venda)
        {
            List<eSegmentoFU> listaSepara = new List<eSegmentoFU>();

            for (int i = 0; i < lista.Count; i++)
            {
                if (i.Equals(lista.Count - 1))
                {
                    eSegmentoFU seg = new eSegmentoFU();

                    seg.Segmento = lista[i].Segmento;
                    seg.SomaTotaisVD = lista[i].SomaTotaisVD;
                    seg.SomaTotaisVarejo = lista[i].SomaTotaisVarejo;
                    seg.SomaTotaisTotal = lista[i].SomaTotaisTotal;
                    seg.TotalVolVD = lista[i].TotalVolVD;
                    seg.TotalVolVarejo = lista[i].TotalVolVarejo;
                    seg.TotalVolTotal = lista[i].TotalVolTotal;
                    seg.TotalPorcentSegVD = lista[i].TotalPorcentSegVD;
                    seg.TotalPorcentSegVarejo = lista[i].TotalPorcentSegVarejo;
                    seg.TotalPorcenTotalVD = lista[i].TotalPorcenTotalVD;
                    seg.TotalPorcenTotalVarejo = lista[i].TotalPorcenTotalVarejo;
                    seg.TotalPorcenTotalTotal = lista[i].TotalPorcenTotalTotal;
                    seg.MarcaCliente = lista[i].MarcaCliente;
                    seg.listaSub = lista.Where(c => c.Segmento == lista[i].Segmento).ToList();

                    listaSepara.Clear();
                    listaSepara.Add(seg);

                    SegementoFUModalidadeDadosTabela(listaSepara, gerarHtml, qtdColunasCabecalho, Venda);
                }
                else if (lista[i].Segmento != lista[i + 1].Segmento)
                {
                    eSegmentoFU seg = new eSegmentoFU();

                    seg.Segmento = lista[i].Segmento;
                    seg.SomaTotaisVD = lista[i].SomaTotaisVD;
                    seg.SomaTotaisVarejo = lista[i].SomaTotaisVarejo;
                    seg.SomaTotaisTotal = lista[i].SomaTotaisTotal;
                    seg.TotalVolVD = lista[i].TotalVolVD;
                    seg.TotalVolVarejo = lista[i].TotalVolVarejo;
                    seg.TotalVolTotal = lista[i].TotalVolTotal;
                    seg.TotalPorcentSegVD = lista[i].TotalPorcentSegVD;
                    seg.TotalPorcentSegVarejo = lista[i].TotalPorcentSegVarejo;
                    seg.TotalPorcenTotalVD = lista[i].TotalPorcenTotalVD;
                    seg.TotalPorcenTotalVarejo = lista[i].TotalPorcenTotalVarejo;
                    seg.TotalPorcenTotalTotal = lista[i].TotalPorcenTotalTotal;
                    seg.MarcaCliente = lista[i].MarcaCliente;
                    seg.listaSub = lista.Where(c => c.Segmento == lista[i].Segmento).ToList();

                    listaSepara.Clear();
                    listaSepara.Add(seg);

                    SegementoFUModalidadeDadosTabela(listaSepara, gerarHtml, qtdColunasCabecalho, Venda);
                }
            }

            if (Venda.Equals("VD"))
            {
                gerarHtml.Append("<tr [VJ]'><td colspan='3' [B]>Total Geral</td><td [B]>" + lista[0].SomaTotaisVD + "</td><td [B]></td><td [B]></td></tr></table>");
            }
            else if (Venda.Equals("VJ"))
            {
                gerarHtml.Append("<tr [VJ]'><td colspan='3' [B]>Total Geral</td>");
                gerarHtml.Append("<td [B]>" + lista[0].SomaTotaisVarejo + "</td><td [B]></td><td [B]></td></tr></table>");
            }
            else
            {
                gerarHtml.Append("<tr [VJ]'><td colspan='3' [B]>Total Geral</td><td [B]>" + lista[0].SomaTotaisVD + "</td><td [B]></td><td [B]></td>");
                gerarHtml.Append("<td [B]>" + lista[0].SomaTotaisVarejo + "</td><td [B]></td><td [B]></td>");
                gerarHtml.Append("<td [B]>" + lista[0].SomaTotaisTotal + "</td><td [B]></td><td [B]></td></tr></table>");
            }

        }

        private void SegementoFUModalidadeDadosTabela(List<eSegmentoFU> listaSepara, StringBuilder gerarHtml, int qtdColunasCabecalho, string Venda)
        {
            List<eSegmentoFU> lista = new List<eSegmentoFU>();
            lista = listaSepara[0].listaSub;

            gerarHtml.Append("<tr style='color:#000; font-weight:normal; [A]'>");
            gerarHtml.Append("<td style='font-weight:bold; background-color:#EFEFEF; [A]' [L] rowspan='" + (listaSepara[0].listaSub.Count + 2) + "'>" + listaSepara[0].Segmento + "</td>");
            for (int i = 0; i < lista.Count; i++)
            {
                gerarHtml.Append("<tr style='color:#000; font-weight:normal; [A]'><td [L]>" + lista[i].Modelo + "</td>");
                gerarHtml.Append("<td [L] [B]>" + lista[i].Marca + "</td>");

                for (int d = 0; d < qtdColunasCabecalho; d++)
                {
                    if (qtdColunasCabecalho.Equals(1))
                    {
                        if (Venda.Equals("VD"))
                        {
                            gerarHtml.Append("<td [B]>" + lista[i].VD + "</td><td [B]>" + lista[i].PorcenSegVD + "%</td><td [B]>" + lista[i].PorcenTotalVD + "%</td>");
                        }
                        else if (Venda.Equals("VJ"))
                        {
                            gerarHtml.Append("<td [VJ]>" + lista[i].Varejo + "</td><td [VJ]>" + lista[i].PorcenSegVarejo + "%</td><td [VJ]>" + lista[i].PorcenTotalVarejo + "%</td>");
                        }
                    }
                    else
                    {
                        if (d.Equals(1))
                        {
                            gerarHtml.Append("<td [VJ]>" + lista[i].Varejo + "</td><td [VJ]>" + lista[i].PorcenSegVarejo + "%</td><td [VJ]>" + lista[i].PorcenTotalVarejo + "%</td>");
                        }
                        else
                        {
                            gerarHtml.Append("<td [B]>" + lista[i].VD + "</td><td [B]>" + lista[i].PorcenSegVD + "%</td><td [B]>" + lista[i].PorcenTotalVD + "%</td>");
                        }
                    }
                }

                if (Venda.Equals("T"))
                {
                    gerarHtml.Append("<td [B]>" + lista[i].TotalVol + "</td><td [B]>" + lista[i].PorcenSegTotal + "%</td><td [B]>" + lista[i].PorcenTotalTotal + "%</td></tr>");
                }


            }

            gerarHtml.Append("<tr style='background-color:#EFEFEF; color:#000;'><td [L] colspan='2' [B]>Total do Segmento " + lista[0].Segmento + "</td>");

            for (int i = 0; i < qtdColunasCabecalho; i++)
            {
                if (qtdColunasCabecalho.Equals(1))
                {
                    if (Venda.Equals("VD"))
                    {
                        gerarHtml.Append("<td [B]>" + listaSepara[0].TotalVolVD + "</td><td [B]>" + listaSepara[0].TotalPorcentSegVD + "%</td><td [B]>" + lista[0].TotalPorcenTotalVD + "%</td>");
                    }
                    else if (Venda.Equals("VJ"))
                    {
                        gerarHtml.Append("<td [B] [VJ]>" + listaSepara[0].TotalVolVarejo + "</td><td [B] [VJ]>" + listaSepara[0].TotalPorcentSegVarejo + "%</td><td [B] [VJ]>" + listaSepara[0].TotalPorcenTotalVarejo + "%</td>");
                    }
                }
                else
                {
                    if (i.Equals(1))
                    {
                        gerarHtml.Append("<td [B] [VJ]>" + listaSepara[0].TotalVolVarejo + "</td><td [B] [VJ]>" + listaSepara[0].TotalPorcentSegVarejo + "%</td><td [B] [VJ]>" + listaSepara[0].TotalPorcenTotalVarejo + "%</td>");
                    }
                    else
                    {
                        gerarHtml.Append("<td [B]>" + listaSepara[0].TotalVolVD + "</td><td [B]>" + listaSepara[0].TotalPorcentSegVD + "%</td><td [B]>" + lista[0].TotalPorcenTotalVD + "%</td>");
                    }
                }
            }

            if (Venda.Equals("T"))
            {
                gerarHtml.Append("<td [B]>" + listaSepara[0].TotalVolTotal + "</td><td [B]></td><td [B]>" + listaSepara[0].TotalPorcenTotalTotal + "%</td></tr>");
            }

        }

        private string SegmentoFUModalidadeReplaces(StringBuilder gerarHtml)
        {
            gerarHtml = gerarHtml.Replace("[EstiloTabelaBase]", EstiloTabelaBase);
            gerarHtml = gerarHtml.Replace("[SIZE]", "10");
            gerarHtml = gerarHtml.Replace("[NCelPadding]", "5");
            gerarHtml = gerarHtml.Replace("[CorBordaLinha]", "#000");
            gerarHtml = gerarHtml.Replace("[CorCabecalho]", "#" + eConfig.RelatorioCorCabecalho);
            gerarHtml = gerarHtml.Replace("[B]", "style='border: 1px solid #000;'");
            gerarHtml = gerarHtml.Replace("[A]", "border: 1px solid #000;");
            gerarHtml = gerarHtml.Replace("[L]", "align='left'");
            gerarHtml = gerarHtml.Replace("[VJ]", "style='border: 1px solid #000; background-color:#D3D3D3; color:#000;'");

            return gerarHtml.ToString();
        }

        public List<eSegmentoFU> GetUserRegistroSegmentoFUModalidade(eSegmentoFU fuMod)
        {
            try
            {
                dRelatorios relatorio = new dRelatorios();
                return relatorio.GetUserRegistroSegmentoFUModalidade(fuMod);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion

        #region SegmentoFUPeriodo

        public string SegmentoFUPeriodoHtmlTitulo(eSegmentoFUPeriodo seg)
        {
            gerarHtml = new StringBuilder();

            return ReplacesTitulo(gerarHtml.ToString());
        }

        public string SegmentoFUPeriodoHtmlTabela(eSegmentoFUPeriodo seg)
        {
            gerarHtml = new StringBuilder();

            List<eSegmentoFUPeriodo> lista = new List<eSegmentoFUPeriodo>();
            lista = GetUserRegistroSegmentoFUPeriodo(seg);

            gerarHtml.Append("<table [EstiloTabelaBase] id='display'><tr style='color:#000; border:none;'><td colspan='3'></td>");
            gerarHtml.Append("<td [B] colspan='3'>1° Periodo</td><td [CA] colspan='3'>2° Periodo</td><td></td></tr>");

            gerarHtml.Append("<tr style='background-color:[CorCabecalho]; [A]'><td width='75' [B]>Segmento</td><td width='170' [B]>Modelo Veiculo</td><td width='140' [B]>Marca</td>");
            gerarHtml.Append("<td width='70' [B]>Vol</td><td width='70' [B]>% Seg</td><td width='70' [B]>% Total</td>");
            gerarHtml.Append("<td width='70' [B]>Vol</td><td width='70' [B]>% Seg</td><td width='70' [B]>% Total</td><td [B]>Evolução %</td></tr>");

            SegmentoFUPeriodoSepara(gerarHtml, lista);

            return SegmentoFUPeriodoReplaces(gerarHtml);
        }

        private void SegmentoFUPeriodoSepara(StringBuilder gerarHtml, List<eSegmentoFUPeriodo> lista)
        {
            List<eSegmentoFUPeriodo> listaAux = new List<eSegmentoFUPeriodo>();

            for (int i = 0; i < lista.Count; i++)
            {
                if (i.Equals(lista.Count - 1))
                {
                    listaAux = lista.Where(w => w.Segmento.Equals(lista[i].Segmento)).ToList();
                    SegmentoFUPeriodoDadosTabela(gerarHtml, listaAux);
                }
                else if (lista[i].Segmento != lista[i + 1].Segmento)
                {
                    listaAux = lista.Where(w => w.Segmento.Equals(lista[i].Segmento)).ToList();
                    SegmentoFUPeriodoDadosTabela(gerarHtml, listaAux);
                }
            }

            gerarHtml.Append("<tr style='background-color:#DDD; color:#000;'><td [B] colspan='3'>Total Geral</td><td [B]>" + lista[0].TotalGeral1 + "</td>");
            gerarHtml.Append("<td [B]></td><td [B]></td><td [B]>" + lista[0].TotalGeral2 + "</td><td [B]></td><td [B]></td><td [B]>EVolGeral</td></tr>");
            gerarHtml.Append("</table>");

        }

        private void SegmentoFUPeriodoDadosTabela(StringBuilder gerarHtml, List<eSegmentoFUPeriodo> lista)
        {
            gerarHtml.Append("<tr style='color:#000; [A]'><td rowspan='" + (lista.Count + 2) + "' [AL] [B]>" + lista[0].Segmento + "</td>");
            for (int i = 0; i < lista.Count; i++)
            {
                gerarHtml.Append("<tr style='color:#000; font-weight:normal; [A]'><td [AL] [B]>" + lista[i].Modelo + "</td>");
                gerarHtml.Append("<td [AL] [B]>" + lista[i].Marca + "</td><td [B]>" + lista[i].Primeiro + "</td><td [B]>" + lista[i].PorcSeg1 + "</td>");
                gerarHtml.Append("<td [B]>" + lista[i].PorcTotal1 + "</td>");
                gerarHtml.Append("<td [CA]>" + lista[i].Segundo + "</td><td [CA]>" + lista[i].PorcSeg2 + "</td><td [CA]>" + lista[i].PorcTotal2 + "</td>");
                gerarHtml.Append("<td [B]>" + "Evo" + "</td></tr>");
            }

            gerarHtml.Append("<tr style='color:#000; background-color:#EFEFEF; [A]'><td [AL] [B] colspan='2'>Total do Segmento " + lista[0].Segmento + "</td>");
            gerarHtml.Append("<td [B]>" + lista[0].TotalSeg1 + "</td><td [B]></td><td [B]>" + lista[0].PorcenGeral1 + "</td>");
            gerarHtml.Append("<td [B]>" + lista[0].TotalSeg2 + "</td><td [B]></td><td [B]>" + lista[0].PorcenGeral2 + "</td>");
            gerarHtml.Append("<td [B]>" + "TEvo" + "</td></tr></tr>");
        }

        private string SegmentoFUPeriodoReplaces(StringBuilder gerarHtml)
        {
            gerarHtml = gerarHtml.Replace("[EstiloTabelaBase]", EstiloTabelaBase);
            gerarHtml = gerarHtml.Replace("[SIZE]", "10");
            gerarHtml = gerarHtml.Replace("[NCelPadding]", "3");
            gerarHtml = gerarHtml.Replace("[CorBordaLinha]", "#000");
            gerarHtml = gerarHtml.Replace("[CorCabecalho]", "#" + eConfig.RelatorioCorCabecalho);
            gerarHtml = gerarHtml.Replace("[B]", "style='border: 1px solid #000;'");
            gerarHtml = gerarHtml.Replace("[A]", "border: 1px solid #000;");
            gerarHtml = gerarHtml.Replace("[AL]", "align='left'");
            gerarHtml = gerarHtml.Replace("[CA]", "style='border: 1px solid #000; background-color:#D3D3D3; color:#000;'");

            return gerarHtml.ToString();
        }

        public List<eSegmentoFUPeriodo> GetUserRegistroSegmentoFUPeriodo(eSegmentoFUPeriodo segmento)
        {
            try
            {
                dRelatorios db = new dRelatorios();
                return db.GetUserRegistroSegmentoFUPeriodo(segmento);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion

        #region SegmentoFULocalidade
        List<eSegmentoFU> listaSegmentoFu = new List<eSegmentoFU>();
        //Vendas = 0 --> VD Vendas = 1 -->Varejo Vendas != 0 e 1 --> VD e Varejo

        public string SegmentoFULocalidadeHtmlTitulo(eSegmentoFU segmento, int vendas)
        {
            List<eSegmentoFU> segme = new List<eSegmentoFU>();

            segme = GetUserRegistroSegmentoFULocalidade(segmento).ToList();

            gerarHtml = new StringBuilder();

            gerarHtml.Append("<div align='center' style='width: 1194px; display:inline-block;'>");
            gerarHtml.Append("<div align='left'style='width: 202px; display:inline; float:left; height: 150px;'>");
            gerarHtml.Append("<img src='[IMG]' alt='Smiley face' height='100' width='200' style='float:right;' /></div>");
            gerarHtml.Append("<div style='width: 807px; display:inline; float:inherit'>");
            gerarHtml.Append("<h2 align='center' style='width:800px; color:[COR]'; font-family:Tahoma; font-size:14pt; fontweight:bold;'>");
            gerarHtml.Append("FU LOCALIDADE<br/>DE " + segmento.DiaDe + " DE " + NomeMes(segmento.MesDe) + " - " + segmento.AnoDe);
            gerarHtml.Append("<br/>ATÉ " + segmento.Dia + " DE " + NomeMes(segmento.Mes) + " - " + segmento.Ano);

            if (segmento.RegiaoOperacional == -1 && segmento.RegiaoGeografico == -1 && segmento.RegiaoMetropolitana == -1 && segmento.Estado == -1
                    && segmento.Cidade == -1 && segmento.AreaOperacional == -1)
            {
                gerarHtml.Append("<br/>NACIONAL TODAS");
            }
            else
            {
                if (segmento.RegiaoOperacional != -1)
                {
                    gerarHtml.Append("<br/>REGIONAL: " + segme[0].Localizacao);
                }
                if (segmento.RegiaoGeografico != -1)
                {
                    gerarHtml.Append("<br/>REGIÃO:" + segme[0].Localizacao);
                }
                if (segmento.RegiaoMetropolitana != -1)
                {
                    gerarHtml.Append("<br/>METRÓPOLE:" + segme[0].Localizacao);
                }
                if (segmento.Estado != -1)
                {
                    gerarHtml.Append("<br/>ESTADO:" + segme[0].Localizacao);
                }
                if (segmento.Cidade != -1)
                {
                    gerarHtml.Append("<br/>CIDADE:" + segme[0].Localizacao);
                }
                if (segmento.AreaOperacional != -1)
                {
                    gerarHtml.Append("<br/>ÁREA DE INFLUENCIA:" + segme[0].Localizacao);
                }
            }

            if (vendas == 0)
                gerarHtml.Append("<br/>VENDAS DIRETAS</h2></div></div>");
            else if (vendas == 1)
                gerarHtml.Append("<br/>VENDAS VAREJO</h2></div></div>");
            else
                gerarHtml.Append("<br/>VENDAS DIRETAS + NO VAREJO</h2></div></div>");

            return ReplacesTitulo(gerarHtml.ToString());
        }

        public string SegmentoFULocalidadeHtmlTabela(eSegmentoFU segmento, int vendas)
        {
            gerarHtml = new StringBuilder();
            listaSegmentoFu = new List<eSegmentoFU>();
            listaSegmentoFu = GetUserRegistroSegmentoFULocalidade(segmento);

            gerarHtml.Append("<table [EstiloTabelaBase] id='display'><tr style='color:#000;'><td colspan='2'></td>");
            if (vendas.Equals(0))
            {
                gerarHtml.Append("<td width='160' colspan='2' [B]>VD</td>");
            }
            else if (vendas.Equals(1))
            {
                gerarHtml.Append("<td width='160' colspan='2' [CA]>Varejo</td>");
            }
            else
            {
                gerarHtml.Append("<td width='160' colspan='2' [B]>VD</td><td width='160' colspan='2' [CA]>Varejo</td>");
            }

            gerarHtml.Append("<td width='185' colspan='2' [B]>Total</td></tr>");
            gerarHtml.Append("<tr style='background-color:[CorCabecalho];'><td width='250' [B]>" + listaSegmentoFu[0].TituloLocalizacao + "</td><td width='200' [B]>SEGMENTO</td>");
            if (vendas == 0 || vendas == 1)
            {
                gerarHtml.Append("<td width='75' [B]>VOL</td><td width='85' [B]>%</td>");
            }
            else
            {
                gerarHtml.Append("<td width='75' [B]>VOL</td><td width='85' [B]>%</td><td width='75' [B]>VOL</td><td width='85' [B]>%</td>");
            }
            gerarHtml.Append("<td width='90' [B]>VOL</td><td width='100' [B]>%</td></tr>");

            SegmentoFULocalidadeDadosTabela(listaSegmentoFu, gerarHtml, vendas);
            gerarHtml.Append("<br/><br/><br/><br/>");
            SegmentoFULocalidadeHtmlTabela2(segmento, vendas, gerarHtml);

            return SegmentoFULocalidadeReplaces(gerarHtml);
        }

        //Tabela MarketShere
        public string SegmentoFULocalidadeHtmlTabela2(eSegmentoFU segmaneto, int vendas, StringBuilder gerarHtml)
        {
            List<eSegmentoFU> lista = new List<eSegmentoFU>();
            segmaneto.Marca = "renault";
            lista = GetUserRegistroSegmentoFULocalidade(segmaneto);


            gerarHtml.Append("<table [EstiloTabelaBase] id='display'><tr style='color:#000;'><td colspan='2'></td>");
            if (vendas.Equals(0))
            {
                gerarHtml.Append("<td colspan='4' [B]>Market Shere</td></tr>");
                gerarHtml.Append("<tr style='color:#000;'><td colspan='2'></td><td width='160' colspan='2' [B]>VD</td>");
            }
            else if (vendas.Equals(1))
            {
                gerarHtml.Append("<td colspan='4' [B]>Market Shere</td></tr>");
                gerarHtml.Append("<tr style='color:#000;'><td colspan='2'></td><td width='160' colspan='2' [CA]>Varejo</td>");
            }
            else
            {
                gerarHtml.Append("<td colspan='6' [B]>Market Shere</td></tr>");
                gerarHtml.Append("<tr style='color:#000;'><td colspan='2'></td><td width='160' colspan='2' [B]>VD</td><td width='160' colspan='2' [CA]>Varejo</td>");
            }
            gerarHtml.Append("<td colspan='2' [B]>Total</td></tr>");

            gerarHtml.Append("<tr style='background-color:[CorCabecalho];'><td width='130' [B]>Marca</td><td width='200' [B]>" + lista[0].Localizacao + "</td>");
            if (vendas == 0 || vendas == 1)
            {
                gerarHtml.Append("<td width='75' [B]>VOL</td><td width='85' [B]>%</td>");
            }
            else
            {
                gerarHtml.Append("<td width='75' [B]>VOL</td><td width='85' [B]>%</td><td width='75' [B]>VOL</td><td width='85' [B]>%</td>");
            }
            gerarHtml.Append("<td width='90' [B]>VOL</td><td width='100' [B]>%</td></tr>");

            SegmentoFULocalidadeDadosTabela2(lista, gerarHtml, vendas);

            return gerarHtml.ToString();
        }

        private void SegmentoFULocalidadeDadosTabela2(List<eSegmentoFU> lista, StringBuilder gerarHtml, int vendas)
        {
            gerarHtml.Append("<tr style='color:#000; font-weight:normal;'><td [B] rowspan='3'>" + lista[0].MarcaCliente + "</td>");
            gerarHtml.Append("<tr style='color:#6D757D; font-weight:normal;'><td [B] [AL]>" + lista[0].Localizacao + "</td>");
            if (vendas == 0)
            {
                gerarHtml.Append("<td [B]>" + lista[0].TotalVolVD + "</td><td [B]>" + int.Parse(lista[0].TotalVolVD) / int.Parse(lista[0].TotalVolVD) + "00%</td>");
            }
            else if (vendas == 1)
            {
                gerarHtml.Append("<td [B]>" + lista[0].TotalVolVarejo + "</td><td [B]>" + int.Parse(lista[0].TotalVolVarejo) / int.Parse(lista[0].TotalVolVarejo) + "00%</td>");
            }
            else
            {
                gerarHtml.Append("<td [B]>" + lista[0].TotalVolVD + "</td><td [B]>" + int.Parse(lista[0].TotalVolVD) / int.Parse(lista[0].TotalVolVD) + "00%</td>");
                gerarHtml.Append("<td [B]>" + lista[0].TotalVolVarejo + "</td><td [B]>" + int.Parse(lista[0].TotalVolVarejo) / int.Parse(lista[0].TotalVolVarejo) + "00%</td>");
            }
            gerarHtml.Append("<td [B]>" + lista[0].TotalVolTotal + "</td><td [B]>" + int.Parse(lista[0].TotalVolTotal) / int.Parse(lista[0].TotalVolTotal) + "00%</td></tr>");


            gerarHtml.Append("<tr style='color:#6D757D; font-weight:normal;'><td style='color:#000; [A]' [AL]>Total marca | Market Shere %</td>");
            if (vendas == 0)
            {
                gerarHtml.Append("<td [B]>" + lista[0].TotalVolVD + "</td><td [CA]><b>" + string.Format("{0:N1}", (float.Parse(lista[0].TotalVolVD) * 100) / int.Parse(listaSegmentoFu[0].TotalVolVD)) + "%</b></td>");
            }
            else if (vendas == 1)
            {
                gerarHtml.Append("<td [B]>" + lista[0].TotalVolVarejo + "</td><td [CA]><b>" + string.Format("{0:N1}", (float.Parse(lista[0].TotalVolVarejo) * 100) / int.Parse(listaSegmentoFu[0].TotalVolVarejo)) + "%</b></td>");
            }
            else
            {
                gerarHtml.Append("<td [B]>" + lista[0].TotalVolVD + "</td><td [CA]><b>" + string.Format("{0:N1}", (float.Parse(lista[0].TotalVolVD) * 100) / int.Parse(listaSegmentoFu[0].TotalVolVD)) + "%</b></td>");
                gerarHtml.Append("<td [B]>" + lista[0].TotalVolVarejo + "</td><td [CA]><b>" + string.Format("{0:N1}", (float.Parse(lista[0].TotalVolVarejo) * 100) / int.Parse(listaSegmentoFu[0].TotalVolVarejo)) + "%</b></td>");
            }
            gerarHtml.Append("<td [B]>" + lista[0].TotalVolTotal + "</td><td [B]>" + string.Format("{0:N1}", (float.Parse(lista[0].TotalVolTotal) * 100) / int.Parse(listaSegmentoFu[0].TotalVolTotal)) + "%</td></tr></tr>");

            gerarHtml.Append("<tr style='color:#6D757D; font-weight:normal;'><td colspan='2' [CA]>Total Geral</td>");
            if (vendas == 0)
            {
                gerarHtml.Append("<td [CA]>" + listaSegmentoFu[0].TotalVolVD + "</td><td [B]></td>");
            }
            else if (vendas == 1)
            {
                gerarHtml.Append("<td [CA]>" + listaSegmentoFu[0].TotalVolVarejo + "</td><td [B]></td>");
            }
            else
            {
                gerarHtml.Append("<td [CA]>" + listaSegmentoFu[0].TotalVolVD + "</td><td [B]></td>");
                gerarHtml.Append("<td [CA]>" + listaSegmentoFu[0].TotalVolVarejo + "</td><td [B]></td>");
            }
            gerarHtml.Append("<td [CA]>" + listaSegmentoFu[0].TotalVolTotal + "</td><td></td></tr></table>");
        }

        private void SegmentoFULocalidadeDadosTabela(List<eSegmentoFU> lista, StringBuilder gerarHtml, int vendas)
        {
            gerarHtml.Append("<tr style='color:#000; font-weight:normal;'><td rowspan='" + (lista.Count + 2) + "' style='[A] background-color:#EFEFEF;'>" + lista[0].Localizacao + "</td>");

            for (int i = 0; i < lista.Count; i++)
            {
                gerarHtml.Append("<tr style='color:#000; font-weight:normal;'><td [AL] [B]>" + lista[i].Segmento + "</td>");
                if (vendas == 0)
                    gerarHtml.Append("<td [B]>" + lista[i].VD + "</td><td [B]>" + lista[i].PorcenSegVD + "%</td>");
                else if (vendas == 1)
                    gerarHtml.Append("<td [B]>" + lista[i].Varejo + "</td><td [B]>" + lista[i].PorcenSegVarejo + "%</td>");
                else
                    gerarHtml.Append("<td [B]>" + lista[i].VD + "</td><td [B]>" + lista[i].PorcenSegVD + "%</td><td [B]>" + lista[i].Varejo + "</td><td [B]>" + lista[i].PorcenSegVarejo + "%</td>");

                gerarHtml.Append("<td [B]>" + lista[i].TotalVol + "</td><td [B]>" + lista[i].PorcenSegTotal + "%</td></tr>");
            }
            gerarHtml.Append("<tr style='color:#000; font-weight:normal; background-color:#EFEFEF;'><td [AL] [B]>Total " + lista[0].Localizacao.ToLower() + "</td>");

            if (vendas == 0)
                gerarHtml.Append("<td [B]><b>" + lista[0].TotalVolVD + "</b></td><td [B]></td>");
            else if (vendas == 1)
                gerarHtml.Append("<td [B]><b>" + lista[0].TotalVolVarejo + "</b></td><td [B]></td>");
            else
                gerarHtml.Append("<td [B]><b>" + lista[0].TotalVolVD + "</b></td><td [B]></td><td [B]><b>" + lista[0].TotalVolVarejo + "</b></td><td [B]></td>");

            gerarHtml.Append("<td [B]><b>" + lista[0].TotalVolTotal + "</b></td><td style='background-color:#FFF;'></td></tr></table>");
        }

        public List<eSegmentoFU> GetUserRegistroSegmentoFULocalidade(eSegmentoFU segmento)
        {
            try
            {
                dRelatorios db = new dRelatorios();
                return db.GetUserRegistroSegmentoFULocalidade(segmento);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private string SegmentoFULocalidadeReplaces(StringBuilder gerarHtml)
        {
            gerarHtml = gerarHtml.Replace("[EstiloTabelaBase]", EstiloTabelaBase);
            gerarHtml = gerarHtml.Replace("[SIZE]", "10");
            gerarHtml = gerarHtml.Replace("[NCelPadding]", "3");
            gerarHtml = gerarHtml.Replace("[CorBordaLinha]", "#000");
            gerarHtml = gerarHtml.Replace("[CorCabecalho]", "#" + eConfig.RelatorioCorCabecalho);
            gerarHtml = gerarHtml.Replace("[B]", "style='border: 1px solid #000;'");
            gerarHtml = gerarHtml.Replace("[A]", "border: 1px solid #000;");
            gerarHtml = gerarHtml.Replace("[AL]", "align='left'");
            gerarHtml = gerarHtml.Replace("[CA]", "style='border: 1px solid #000; background-color:#D3D3D3; color:#000;'");

            return gerarHtml.ToString();
        }

        #endregion

        #region SegmentoFUAnual

        List<eSegmentoFU> listaSegmentoFUAnula = new List<eSegmentoFU>();

        public string SegmentoFUAnualHtmlTitulo(eSegmentoFU segmento, int vendas)
        {
            gerarHtml = new StringBuilder();

            gerarHtml.Append("<div align='center' style='width: 1194px; display:inline-block;'>");
            gerarHtml.Append("<div align='left'style='width: 202px; display:inline; float:left; height: 150px;'>");
            gerarHtml.Append("<img src='[IMG]' alt='Smiley face' height='100' width='200' style='float:right;' /></div>");
            gerarHtml.Append("<div style='width: 807px; display:inline; float:inherit'>");
            gerarHtml.Append("<h2 align='center' style='width:800px; color:[COR]'; font-family:Tahoma; font-size:14pt; fontweight:bold;'>");


            if (segmento.RegiaoOperacional == -1 && segmento.RegiaoGeografico == -1 && segmento.RegiaoMetropolitana == -1 && segmento.Estado == -1
                    && segmento.Cidade == -1 && segmento.AreaOperacional == -1)
            {
                gerarHtml.Append("FU ANUAL <br/>");
                gerarHtml.Append("NACIONAL TODAS");
            }
            else
            {
                if (segmento.RegiaoOperacional != -1)
                {
                    gerarHtml.Append("FU ANUAL <br/>");
                    gerarHtml.Append("REGIONAL: " + segmento.RegiaoOperacional);
                }
                if (segmento.RegiaoGeografico != -1)
                {
                    gerarHtml.Append("FU ANUAL <br/>");
                    gerarHtml.Append("REGIÃO:" + segmento.RegiaoGeografico);
                }
                if (segmento.RegiaoMetropolitana != -1)
                {
                    gerarHtml.Append("FU ANUAL <br/>");
                    gerarHtml.Append("METRÓPOLE:" + segmento.RegiaoMetropolitana);
                }
                if (segmento.Estado != -1)
                {
                    gerarHtml.Append("FU ANUAL <br/>");
                    gerarHtml.Append("ESTADO:" + segmento.Estado);
                }
                if (segmento.Cidade != -1)
                {
                    gerarHtml.Append("FU ANUAL <br/>");
                    gerarHtml.Append("CIDADE:" + segmento.Cidade);
                }
                if (segmento.AreaOperacional != -1)
                {
                    gerarHtml.Append("FU ANUAL <br/>");
                    gerarHtml.Append("ÁREA DE INFLUENCIA:" + segmento.AreaOperacional);
                }
                if (segmento.Segmento != "-1")
                {
                    gerarHtml.Append("FU ANUAL = " + segmento.Segmento + "<br/>");
                }
            }

            if (segmento.TipoVenda == 0)
                gerarHtml.Append("<br/>VENDAS DIRETAS</h2></div></div>");
            else if (segmento.TipoVenda == 1)
                gerarHtml.Append("<br/>VENDAS VAREJO</h2></div></div>");
            else
                gerarHtml.Append("<br/>VENDAS DIRETAS + NO VAREJO</h2></div></div>");

            gerarHtml.Append("</h2></div></div>");

            return ReplacesTitulo(gerarHtml.ToString());
        }

        public string SegmentoFUAnualHtmlTabela(eSegmentoFU segmento, int vendas)
        {
            gerarHtml = new StringBuilder();
            string[] separaAnos = segmento.ListaAnos.Split(',');

            listaSegmentoFUAnula = GetUserRegistroSegmentoFUAnual(segmento);

            gerarHtml.Append("<table [EstiloTabelaBase] id='display'><tr style='color:#000;'><td colspan='3'></td>");

            for (int i = 0; i < separaAnos.Length; i++)
            {
                if (i.Equals(separaAnos.Length - 1))
                {
                    if (segmento.Segmento == "-1")
                        gerarHtml.Append("<td [CB] colspan='3'>" + separaAnos[i] + " até " + listaSegmentoFUAnula[0].DiaUtil + "º DIA ÚTIL " + NomeMes(segmento.Mes).ToUpper() + "</td></tr>");
                    else
                        gerarHtml.Append("<td [CB] colspan='2'>" + separaAnos[i] + " até " + listaSegmentoFUAnula[0].DiaUtil + "º DIA ÚTIL " + NomeMes(segmento.Mes).ToUpper() + "</td></tr>");
                }
                else
                {
                    if (segmento.Segmento == "-1")
                        gerarHtml.Append("<td [B] colspan='3'>" + separaAnos[i] + "</td>");
                    else
                        gerarHtml.Append("<td [B] colspan='2'>" + separaAnos[i] + "</td>");
                }
            }

            gerarHtml.Append("<tr style='background-color:[CorCabecalho];'>");
            gerarHtml.Append("<td [B] width='95'>Segmento</td><td [B] width='155'>Modelo veiculo</td><td [B] width='110'>Marca</td>");
            for (int i = 0; i < separaAnos.Length; i++)
            {
                if (i.Equals(separaAnos.Length - 1))
                {
                    if (segmento.Segmento == "-1")
                        gerarHtml.Append("<td [CB] width='60'>VOL</td><td [CB] width='60'>% Seg</td><td [CB] width='60'>% Total</td></tr>");
                    else
                        gerarHtml.Append("<td [CB] width='60'>VOL</td><td [CB] width='60'>% Seg</td></tr>");
                }
                else
                {
                    if (segmento.Segmento == "-1")
                        gerarHtml.Append("<td [B] width='60'>VOL</td><td [B] width='60'>% Seg</td><td [B] width='60'>% Total</td>");
                    else
                        gerarHtml.Append("<td [B] width='60'>VOL</td><td [B] width='60'>% Seg</td>");
                }
            }

            SegmentoFuAnualSeparaDados(listaSegmentoFUAnula, gerarHtml, separaAnos, segmento);

            return SegmentoFuAnualReplace(gerarHtml);
        }

        public string SegmentoFUAnualHtmlMarketShere(eSegmentoFU segmento, string[] separaAnos, StringBuilder gerarHtml, List<eSegmentoFU> lista)
        {

            gerarHtml.Append("</table><br/><br/><table [EstiloTabelaBase] id='display'><tr style='color:#000;'><td colspan='2'><h3>Market Shere</h3></td>");

            for (int i = 0; i < separaAnos.Length; i++)
            {
                if (i.Equals(separaAnos.Length - 1))
                {
                    gerarHtml.Append("<td [CB] colspan='2'>" + separaAnos[i] + " até " + listaSegmentoFUAnula[0].DiaUtil + "º DIA ÚTIL " + NomeMes(segmento.Mes).ToUpper() + "</td></tr>");
                }
                else
                {
                    gerarHtml.Append("<td [B] colspan='2'>" + separaAnos[i] + "</td>");
                }
            }
            
            gerarHtml.Append("<tr style='background-color:[CorCabecalho];'>");
            gerarHtml.Append("<td [B] width='180'>Marca</td><td [B] width='180'>Segmento</td>");
            for (int i = 0; i < separaAnos.Length; i++)
            {
                if (i.Equals(separaAnos.Length - 1))
                {
                    gerarHtml.Append("<td [CB] width='90'>VOL</td><td [CB] width='90'>% Seg</td></tr>");
                }
                else
                {
                    gerarHtml.Append("<td [B] width='90'>VOL</td><td [B] width='90'>% Seg</td>");
                }
            }

            SegmentoFUAnualDadosTabelaMarketShere(segmento, separaAnos, gerarHtml, lista);

            return gerarHtml.ToString();
        }

        private void SegmentoFUAnualDadosTabelaMarketShere(eSegmentoFU segmento, string[] separaAnos, StringBuilder gerarHtml, List<eSegmentoFU> lista)
        {
            lista = lista.Where(c => c.Marca == lista[0].MarcaCliente.ToUpper()).ToList();
            int SomaMarca = 0;
            int qtdSeg = 0;
            List<eSegmentoFU> listaSeg = new List<eSegmentoFU>();
            int SomaMarcaAnoSeg = 0;

            for (int i = 0; i < lista.Count; i++)
            {
                if (i.Equals(lista.Count - 1))
                {
                    if (lista.Count.Equals(1))
                    {
                        qtdSeg++;
                        listaSeg.Add(lista[i]);
                    }
                    else if (lista[i - 1].Segmento != lista[i].Segmento)
                    {
                        qtdSeg++;
                        listaSeg.Add(lista[i]);
                    }

                }
                else if (lista[i].Segmento != lista[i + 1].Segmento)
                {
                    qtdSeg++;
                    listaSeg.Add(lista[i]);
                }
            }

            gerarHtml.Append("<tr style='color:#000;'><td rowspan='" + (qtdSeg + 2) + "' [B] [AL]><b>" + lista[0].MarcaCliente.ToUpper() + "</b></td>");
            for (int i = 0; i < (qtdSeg + 1); i++)
            {
                if (i.Equals(qtdSeg))
                {
                    gerarHtml.Append("<tr style='color:#6C6969; font-weight:normal;'><td [AL] [B]>Total marca | Market Shere</td>");
                    for (int d = 0; d < separaAnos.Length; d++)
                    {
                        SomaMarca = listaSeg.Where(c => c.Marca == listaSeg[0].MarcaCliente.ToUpper()
                        && c.listaSub[d].Ano == int.Parse(separaAnos[d])).Sum(s => int.Parse(s.listaSub[d].Qtd));
                        gerarHtml.Append("<td [B]>" + SomaMarca + "</td>");
                        gerarHtml.Append("<td [CA]><b>" + string.Format("{0:N1}", (float.Parse(SomaMarca.ToString()) * 100) / float.Parse(listaSeg[0].listaSub[d].TotalGeralAnual)) + "%</b></td>");
                    }
                    gerarHtml.Append("</tr></tr>");
                }
                else
                {
                    gerarHtml.Append("<tr style='color:#6C6969; font-weight:normal;'><td [Al] [B]>" + listaSeg[i].Segmento + "</td>");
                    for (int d = 0; d < separaAnos.Length; d++)
                    {
                        SomaMarcaAnoSeg = listaSeg.Where(c => c.Marca == listaSeg[0].MarcaCliente.ToUpper()
                            && c.listaSub[d].Ano == int.Parse(separaAnos[d])
                            && c.Segmento == listaSeg[i].Segmento).Sum(s => int.Parse(s.listaSub[d].Qtd));

                        SomaMarca = listaSeg.Where(c => c.Marca == listaSeg[0].MarcaCliente.ToUpper()
                            && c.listaSub[d].Ano == int.Parse(separaAnos[d])).Sum(s => int.Parse(s.listaSub[d].Qtd));

                        if (d.Equals(separaAnos.Length - 1))
                        {
                            gerarHtml.Append("<td [CB]>" + SomaMarcaAnoSeg + "</td>");
                            gerarHtml.Append("<td [CB]>" + string.Format("{0:N1}", (float.Parse(SomaMarcaAnoSeg.ToString()) * 100) / float.Parse(SomaMarca.ToString())) + "%</td>");
                        }
                        else
                        {
                            gerarHtml.Append("<td [B]>" + SomaMarcaAnoSeg + "</td>");
                            gerarHtml.Append("<td [B]>" + string.Format("{0:N1}", (float.Parse(SomaMarcaAnoSeg.ToString()) * 100) / float.Parse(SomaMarca.ToString())) + "%</td>");
                        }
                    }
                }
            }

            gerarHtml.Append("<tr style='color:#6C6969; background-color:#D3D3D3; font-weight:normal;'><td [B] colspan='2'>Total geral</td>");
            for (int i = 0; i < separaAnos.Length; i++)
            {
                gerarHtml.Append("<td [B]>" + listaSeg[0].listaSub[i].TotalGeralAnual + "</td><td style='background-color:#FFF;'></td>");
            }
            gerarHtml.Append("</tr></table>");
        }

        private void SegmentoFuAnualSeparaDados(List<eSegmentoFU> lista, StringBuilder gerarHtml, string[] separaAnos, eSegmentoFU segmento)
        {
            List<eSegmentoFU> listaAux = new List<eSegmentoFU>();
            List<eSegmentoFU> listaAux2 = new List<eSegmentoFU>();
            List<eSegmentoFU> listaMarket = new List<eSegmentoFU>();
            bool naoEntrou = true;


            for (int i = 0; i < lista.Count; i++)
            {
                if (i.Equals(lista.Count - 1))
                {
                    listaAux = lista.Where(c => c.Segmento.Equals(lista[i].Segmento)).ToList();

                    listaAux = listaAux.OrderBy(c => c.Marca).ToList();

                    for (int d = 0; d < listaAux.Count; d++)
                    {
                        if (d.Equals(listaAux.Count - 1))
                        {
                            eSegmentoFU seg = new eSegmentoFU();

                            seg.Marca = listaAux[d].Marca;
                            seg.Segmento = listaAux[d].Segmento;
                            seg.Modelo = listaAux[d].Modelo;
                            seg.TotalGeralAnual = listaAux[d].TotalGeralAnual;
                            seg.TotalGeralSegmento = listaAux[d].TotalGeralSegmento;
                            seg.PorcentTotalGeralSeg = listaAux[d].PorcentTotalGeralSeg;
                            seg.MarcaCliente = listaAux[d].MarcaCliente;
                            seg.listaSub = listaAux.Where(c => c.Marca.Equals(listaAux[d].Marca) && c.Modelo == listaAux[d].Modelo).ToList();

                            listaMarket.Add(seg);
                            listaAux2.Add(seg);
                        }
                        else if (listaAux[d].Modelo != listaAux[d + 1].Modelo && !listaAux2.Exists(e => e.Modelo == listaAux[d].Modelo))
                        {
                            eSegmentoFU seg = new eSegmentoFU();

                            seg.Marca = listaAux[d].Marca;
                            seg.Segmento = listaAux[d].Segmento;
                            seg.Modelo = listaAux[d].Modelo;
                            seg.TotalGeralAnual = listaAux[d].TotalGeralAnual;
                            seg.TotalGeralSegmento = listaAux[d].TotalGeralSegmento;
                            seg.PorcentTotalGeralSeg = listaAux[d].PorcentTotalGeralSeg;
                            seg.MarcaCliente = listaAux[d].MarcaCliente;
                            seg.listaSub = listaAux.Where(c => c.Marca.Equals(listaAux[d].Marca) && c.Modelo == listaAux[d].Modelo).ToList();

                            listaMarket.Add(seg);
                            listaAux2.Add(seg);
                        }

                    }
                    SegmentoFuAnualDadosTabela(gerarHtml, listaAux2, separaAnos, segmento);
                }
                else if (lista[i].Segmento != lista[i + 1].Segmento && naoEntrou)
                {
                    listaAux = lista.Where(c => c.Segmento.Equals(lista[i].Segmento)).ToList();
                    listaAux = listaAux.OrderBy(c => c.Marca).ToList();
                    naoEntrou = false;

                    for (int d = 0; d < listaAux.Count; d++)
                    {
                        if (d.Equals(listaAux.Count - 1))
                        {
                            eSegmentoFU seg = new eSegmentoFU();

                            seg.Marca = listaAux[d].Marca;
                            seg.Segmento = listaAux[d].Segmento;
                            seg.Modelo = listaAux[d].Modelo;
                            seg.TotalGeralAnual = listaAux[d].TotalGeralAnual;
                            seg.TotalGeralSegmento = listaAux[d].TotalGeralSegmento;
                            seg.PorcentTotalGeralSeg = listaAux[d].PorcentTotalGeralSeg;
                            seg.MarcaCliente = listaAux[d].MarcaCliente;
                            seg.listaSub = listaAux.Where(c => c.Marca.Equals(listaAux[d].Marca) && c.Modelo == listaAux[d].Modelo).ToList();

                            listaMarket.Add(seg);
                            listaAux2.Add(seg);
                        }
                        else if ((listaAux[d].Modelo != listaAux[d + 1].Modelo) && !listaAux2.Exists(e => e.Modelo == listaAux[d].Modelo))
                        {
                            eSegmentoFU seg = new eSegmentoFU();

                            seg.Marca = listaAux[d].Marca;
                            seg.Segmento = listaAux[d].Segmento;
                            seg.Modelo = listaAux[d].Modelo;
                            seg.TotalGeralAnual = listaAux[d].TotalGeralAnual;
                            seg.TotalGeralSegmento = listaAux[d].TotalGeralSegmento;
                            seg.PorcentTotalGeralSeg = listaAux[d].PorcentTotalGeralSeg;
                            seg.MarcaCliente = listaAux[d].MarcaCliente;
                            seg.listaSub = listaAux.Where(c => c.Marca.Equals(listaAux[d].Marca) && c.Modelo == listaAux[d].Modelo).ToList();

                            listaMarket.Add(seg);
                            listaAux2.Add(seg);
                        }

                    }
                    SegmentoFuAnualDadosTabela(gerarHtml, listaAux2, separaAnos, segmento);
                    listaAux2.Clear();
                    listaAux.Clear();
                }
            }

            gerarHtml.Append("<tr style='background-color:#D3D3D3; color:#000;'><td [B] colspan='3'>Total Geral</td>");
            int Valor = 0;
            int indiceI = 0;
            for (int i = 0; i < listaMarket.Count; i++)
            {
                Valor = 0;
                for (int d = 0; d < separaAnos.Length; d++)
                {
                    if (listaMarket[i].listaSub[d].Qtd != "0")
                        Valor++;
                    if (Valor.Equals(4))
                        indiceI = i;
                }
            }

            for (int i = 0; i < separaAnos.Length; i++)
            {
                gerarHtml.Append("<td [B]>" + listaMarket[indiceI].listaSub[i].TotalGeralAnual + "</td><td [B]></td><td [B]></td>");
            }

            gerarHtml.Append("</tr>");

            SegmentoFUAnualHtmlMarketShere(segmento, separaAnos, gerarHtml, listaMarket);
        }

        private void SegmentoFuAnualDadosTabela(StringBuilder gerarHtml, List<eSegmentoFU> lista, string[] separaAnos, eSegmentoFU segmento)
        {
            gerarHtml.Append("<tr style='color:#000; font-weight:normal;'><td style='background-color:#EFEFEF; [A]' rowspan='" + (lista.Count + 2) + "'><b>" + lista[0].Segmento + "</b></td>");
            for (int i = 0; i < lista.Count; i++)
            {
                gerarHtml.Append("<tr style='color:#000; font-weight:normal;'><td [B]>" + lista[i].Modelo + "</td><td [B]>" + lista[i].Marca + "</td>");
                for (int d = 0; d < separaAnos.Length; d++)
                {
                    if (lista[i].listaSub.Count < separaAnos.Length)
                    {
                        for (int f = 0; f < separaAnos.Length; f++)
                        {
                            if (f > (lista[i].listaSub.Count - 1))
                            {
                                lista[i].listaSub.Insert(f, new eSegmentoFU { Qtd = "0", PorcentSeg = "0,0", PorcentTotal = "0,0" });
                            }
                            else if (lista[i].listaSub[f].Ano.ToString() != separaAnos[f])
                            {
                                lista[i].listaSub.Insert(f, new eSegmentoFU { Qtd = "0", PorcentSeg = "0,0", PorcentTotal = "0,0" });
                            }
                        }
                    }

                    if (d.Equals(separaAnos.Length - 1))
                    {
                        if (lista[i].listaSub[d].Qtd.Equals("0"))
                        {
                            gerarHtml.Append("<td [B]>" + lista[i].listaSub[d].Qtd + "</td><td [B]>" + lista[i].listaSub[d].PorcentSeg + "%</td>");
                            if (segmento.Segmento == "-1")
                                gerarHtml.Append("<td [B]>" + lista[i].listaSub[d].PorcentTotal + "%</td></tr>");
                            else
                                gerarHtml.Append("</tr>");
                        }
                        else
                        {
                            gerarHtml.Append("<td [CB]>" + lista[i].listaSub[d].Qtd + "</td><td [CB]>" + lista[i].listaSub[d].PorcentSeg + "%</td>");
                            if (segmento.Segmento == "-1")
                                gerarHtml.Append("<td [CB]>" + lista[i].listaSub[d].PorcentTotal + "%</td></tr>");
                            else
                                gerarHtml.Append("</tr>");
                        }
                    }
                    else
                    {
                        gerarHtml.Append("<td [B]>" + lista[i].listaSub[d].Qtd + "</td><td [B]>" + lista[i].listaSub[d].PorcentSeg + "%</td>");
                        if (segmento.Segmento == "-1")
                            gerarHtml.Append("<td [B]>" + lista[i].listaSub[d].PorcentTotal + "%</td>");
                    }
                }
            }

            gerarHtml.Append("<tr style='background-color:#EFEFEF; color:#000;'><td [B] [AL] colspan='2'>Total Segmento " + lista[0].Segmento + "</td>");
            for (int i = 0; i < separaAnos.Length; i++)
            {
                gerarHtml.Append("<td [B]>" + lista[0].listaSub[i].TotalGeralSegmento + "</td><td [B]></td>");
                if (segmento.Segmento == "-1")
                    gerarHtml.Append("<td [B]>" + lista[0].listaSub[i].PorcentTotalGeralSeg + "%</td>");
            }
            gerarHtml.Append("</tr></tr>");
        }

        public string SegmentoFuAnualReplace(StringBuilder gerarHtml)
        {
            gerarHtml = gerarHtml.Replace("[EstiloTabelaBase]", EstiloTabelaBase);
            gerarHtml = gerarHtml.Replace("[SIZE]", "10");
            gerarHtml = gerarHtml.Replace("[NCelPadding]", "3");
            gerarHtml = gerarHtml.Replace("[CorBordaLinha]", "#000");
            gerarHtml = gerarHtml.Replace("[CorCabecalho]", "#" + eConfig.RelatorioCorCabecalho);
            gerarHtml = gerarHtml.Replace("[B]", "style='border: 1px solid #000;'");
            gerarHtml = gerarHtml.Replace("[A]", "border: 1px solid #000;");
            gerarHtml = gerarHtml.Replace("[AL]", "align='left'");
            gerarHtml = gerarHtml.Replace("[CA]", "style='border: 1px solid #000; background-color:#D3D3D3; color:#000;'");
            gerarHtml = gerarHtml.Replace("[CB]", "style='background-color:#DED9C5; color:#000; border: 1px solid #000;'");

            return gerarHtml.ToString();
        }

        public List<eSegmentoFU> GetUserRegistroSegmentoFUAnual(eSegmentoFU segmento)
        {
            try
            {
                dRelatorios db = new dRelatorios();
                return db.GetUserRegistroSegmentoFUAnual(segmento);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion

    }
}
