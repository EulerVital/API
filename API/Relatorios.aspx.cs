using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using API.Controllers;
using API.Models;

namespace RelatoriosHTML
{
    public partial class Relatorios : System.Web.UI.Page
    {
        RelatorioController relatorio;

        bool ehGrupo = false;
        int NumMarcas = 6;
        string chassi = "";
        string Valor = null;


        protected void Page_Load(object sender, EventArgs e)
        {
            relatorio = new RelatorioController();
            relatorio.InstanciarObjeto("ABRARE");
            EscolhaRelatorio("EmplacamentoCidade");
        }

        private void EscolhaRelatorio(string Relatorio)
        {
            Valor = Request.QueryString["DetalheVeiculoChassi"];
            if(!string.IsNullOrEmpty(Valor))
                Relatorio = Valor;

            switch (Relatorio)
            {
                case "Detalhamento":

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

                    Tabela.Text = relatorio.Detalhamento_1HtmlTabela(detalhe);
                    Titulo.Text = relatorio.Detalhamanto_1HtmlTitulo(detalhe);
                    break;
                case "DetalheVeiculoChassi":

                    eDetalheVeiculoChassi detaV = new eDetalheVeiculoChassi();

                    detaV.IdEmpresa = 36580;
                    detaV.Mes = 02;
                    detaV.Ano = 2016;
                    detaV.Grupo = false;
                    detaV.IdLocalizacao = 8450;

                    Tabela.Text = relatorio.DetalhesVeiculoChassiHtmlTabela(detaV);
                    Titulo.Text = relatorio.DetalhesVeiculoChassiHtmlTitulo(detaV);

                    break;

                case "DetalhamentoAnual":

                    eDetalhamentoAnual detaAnual = new eDetalhamentoAnual();

                    detaAnual.Ano = 2016;
                    detaAnual.Categoria = 3;
                    detaAnual.TipoVenda = 2;
                    detaAnual.Segmento = '*';
                    detaAnual.RegiaoOperacional = -1;
                    detaAnual.RegiaoGeografico = -1;
                    detaAnual.RegiaoMetropolitana = -1;
                    detaAnual.Estado = -1;
                    detaAnual.Cidade = -1;
                    detaAnual.AreaOperacional = -1;
                    detaAnual.Concessionaria = 36580;
                    detaAnual.ByGroup = 1;

                    Titulo.Text = relatorio.Relatorios(relatorio.DetalhamentoAnualHtmlTabela(detaAnual), relatorio.DetalhamentoAnualHtmlTitulo(detaAnual));
                    break;

                case "EmplacamentoCidade":

                    eEmplacamentoCidade emplaCity = new eEmplacamentoCidade();

                    emplaCity.AteDia = 30;
                    emplaCity.AteMes = 11;
                    emplaCity.AteAno = 2016;
                    emplaCity.Categoria = 3;
                    emplaCity.TipoVenda = 2;
                    emplaCity.Seguimento = "*";
                    emplaCity.Concessionaria = 36580;
                    emplaCity.ByGroup = true;
                    emplaCity.Anual = false;
                    emplaCity.Ranking2 = false;

                    if(emplaCity.ByGroup)
                        Titulo.Text = relatorio.Relatorios(relatorio.EmplacamentoCidadeGrupoTabela(emplaCity), relatorio.EmplacamentoCidadeHtmlTitulo(emplaCity));
                    else
                        Titulo.Text = relatorio.Relatorios(relatorio.EmplacamentoCidadeHtmlTabela(emplaCity), relatorio.EmplacamentoCidadeHtmlTitulo(emplaCity));
                    
                    break;
                case "DiarioMarcas":
                    Titulo.Text = relatorio.Relatorios(relatorio.DiarioMarcasTabela(), relatorio.DiarioMarcasTitulo()); 
                    break;
                case "Ultimo12Meses":

                    ltRelatorio.Text = relatorio.Relatorios(relatorio.Ultimo12MesesTabela(), relatorio.Ultimo12MesesHtmlTitulo()); 
                    break;
                case "AreaDeInfluencia":

                    ltRelatorio.Text = relatorio.Relatorios(relatorio.AreaDeInfluenciaHtmlTabela(NumMarcas), relatorio.AreaDeInfluenciaHtmlTitulo()); 
                    break;
                case "Localidade":

                    ltRelatorio.Text = relatorio.Relatorios(relatorio.LocalidadeHtmlTabela(NumMarcas), relatorio.LocalidadeHtmlTitulo()); 

                    break;
                case "ModeloAno":

                    ltRelatorio.Text = relatorio.Relatorios(relatorio.ModeloAnoHtmlTabela(NumMarcas), relatorio.ModeloAnoHtmlTitulo()); 
                    break;
                case "MarcaAno":

                    ltRelatorio.Text = relatorio.Relatorios(relatorio.MarcaAnoHtmlTabela(NumMarcas), relatorio.MarcaAnoTitulo()); 
                    break;
                case "RankingGrupoModelos":

                    ltRelatorio.Text = relatorio.Relatorios(relatorio.RankingGrupoModelosHtmlTabela(NumMarcas), relatorio.RankingGrupoModelosHtmlTitulo()); 
                    break;
                case "SegmentoMesAno":

                    ltRelatorio.Text = relatorio.Relatorios(relatorio.SegmentoMesAnoHtmlTabela(5, 3, 7), relatorio.SegmentoMesAnoHtmlTitulo()); 
                    break;
                case "EvolucaoMercado":

                    ltRelatorio.Text = relatorio.Relatorios(relatorio.EvolucaoMercadoHtmlTabela(5, 3, 7), relatorio.EvolulcaoMercadoHtmlTitulo()); 
                    break;
                case "RankingConcessionariaGrupo":

                    ltRelatorio.Text = relatorio.Relatorios(relatorio.RankingConcessionariaGrupoHtmlTitulo(), relatorio.RankingConcessionariaGrupoHtmlTabela(ehGrupo, 6)); 
                    break;
                case "VeiculosInfo":

                    chassi = "8AGCN48P0AR154014,9BD17164G95355025,9BD17164G95355571";
                    ltRelatorio.Text = relatorio.Relatorios(relatorio.VeiculosInfoHtmlTabela(chassi), relatorio.VeiculosInfoHtmlTitulo()); 

                    break;
                case "AcessoUsuario":

                    eAcessos acessoUser = new eAcessos();

                    acessoUser.Dia = 10;
                    acessoUser.Mes = 10;
                    acessoUser.Ano = 2016;
                    acessoUser.DiaDe = 10;
                    acessoUser.MesDe = 10;
                    acessoUser.AnoDe = 2016;
                    acessoUser.Grupo = null;
                    acessoUser.Empresa = null;
                    acessoUser.Usuario = null;
                    acessoUser.Relatorio = null;
                    acessoUser.Log = "1";

                    ltRelatorio.Text = relatorio.Relatorios(relatorio.AcessoUsuarioHtmlTabela(acessoUser), relatorio.AcessoUsuarioHtmlTitulo(acessoUser)); 

                    break;
                case "AcessosGruposEmpresa":

                    eAcessos acessoGrupo = new eAcessos();

                    acessoGrupo.Dia = 10;
                    acessoGrupo.Mes = 10;
                    acessoGrupo.Ano = 2016;
                    acessoGrupo.DiaDe = 10;
                    acessoGrupo.MesDe = 10;
                    acessoGrupo.AnoDe = 2016;
                    acessoGrupo.Grupo = null;
                    acessoGrupo.Empresa = null;
                    acessoGrupo.Usuario = null;
                    acessoGrupo.Relatorio = null;
                    acessoGrupo.Log = "1";

                    ltRelatorio.Text = relatorio.Relatorios(relatorio.AcessoGrupoEmpresaHtmlTabela(acessoGrupo), relatorio.AcessoGrupoEmpresaHtmlTitulo(acessoGrupo)); 
                    break;

                case "AcessosRelatorios":

                    eAcessos acessoRelatorio = new eAcessos();

                    acessoRelatorio.Dia = 10;
                    acessoRelatorio.Mes = 10;
                    acessoRelatorio.Ano = 2016;
                    acessoRelatorio.DiaDe = 10;
                    acessoRelatorio.MesDe = 10;
                    acessoRelatorio.AnoDe = 2016;
                    acessoRelatorio.Grupo = null;
                    acessoRelatorio.Empresa = null;
                    acessoRelatorio.Usuario = null;
                    acessoRelatorio.Relatorio = null;
                    acessoRelatorio.Log = "1";

                    ltRelatorio.Text = relatorio.Relatorios(relatorio.AcessosRelatoriosHtmlTabela(acessoRelatorio), relatorio.AcessosRelatoriosHtmlTitulo(acessoRelatorio)); 
                    break;
                case "EstadosCidades":

                    ltRelatorio.Text = relatorio.Relatorios(relatorio.EstadosCidadesHtmlTabela(), relatorio.EstadosCidadesHtmlTitulo()); 
                    break;

                case "RegioesAreasMunicipios":

                    ltRelatorio.Text = relatorio.Relatorios(relatorio.RegioesAreasMunicipiosHtmlTabela(36197, 1), relatorio.RegioesAreasMunicipiosHtmlTitulo(36197, 1)); 

                    break;

                case "DuploEmplacamento":

                    eDuploEmplacamento duplo = new eDuploEmplacamento();

                    duplo.Dia = 11;
                    duplo.Mes = 10;
                    duplo.Ano = 2016;
                    duplo.RegiaoOperacional = -1;
                    duplo.Estado = -1;
                    duplo.Concessionaria = -1;
                    duplo.DiaDe = 1;
                    duplo.MesDe = 9;
                    duplo.AnoDe = 2016;
                    duplo.ModalidadeVenda = null;
                    duplo.Sigla = null;
                    duplo.Bygroup = 0;

                    ltRelatorio.Text = relatorio.Relatorios(relatorio.DuploEmplacamentoHtmlTabela(duplo), relatorio.DuploEmplacamentoHtmlTitulo(duplo)); 

                    break;

                case "FaturamentoDVE":

                    eFaturamentoDve faturamento = new eFaturamentoDve();

                    faturamento.Dia = 24;
                    faturamento.Mes = 10;
                    faturamento.Ano = 2016;
                    faturamento.ModalidadeVenda = "AGENCIA DE VIAGENS";
                    faturamento.Sigla = "AGVIA";

                    ltRelatorio.Text = relatorio.Relatorios(relatorio.FaturamentoDVEHtmlTabela(faturamento), relatorio.FaturamentoDVEHtmlTitulo(faturamento)); 

                    break;

                case "FaturamentoDVR":

                    int ano = 2016;
                    int mes = 10;
                    int dia = 24;

                    ltRelatorio.Text = relatorio.Relatorios(relatorio.FaturamentoDVRHtmlTabela(ano, mes, dia), relatorio.FaturamentoDVRHtmlTitulo(ano, mes, dia)); 
                    break;
                case "FaturamentoDveCliente":

                    int anoFdc = 2016;
                    int mesFdc = 10;
                    int diaFdc = 25;

                    ltRelatorio.Text = relatorio.Relatorios(relatorio.FaturamentoDveClienteHtmlTabela(diaFdc, mesFdc, anoFdc), relatorio.FaturamentoDveClienteHtmlTitulo(diaFdc, mesFdc, anoFdc)); 

                    break;
                case "FaturamentoDVEConcessionaria":

                    int anoF = 2016;
                    int mesF = 10;
                    int diaF = 25;

                    ltRelatorio.Text = relatorio.Relatorios(relatorio.FaturamentoDveConcessionariaHtmlTabela(diaF, mesF, anoF), relatorio.FaturamentoDVEConcessionariaHtmlTitulo(diaF, mesF, anoF)); 
                    break;

                case "Invasao":

                    eInvasao invasao = new eInvasao();

                    invasao.AteAno = 2016;
                    invasao.AteMes = 10;
                    invasao.AteDia = 27;
                    invasao.DeDia = 31;
                    invasao.DeMes = 12;
                    invasao.DeAno = 2015;
                    invasao.AreaOperacional = 2897;

                    ltRelatorio.Text = relatorio.Relatorios(relatorio.InvasaoHtmlTabela(invasao), relatorio.InvasaoHtmlTitulo(invasao)); 

                    break;
                case "InvasaoArea":

                    eInvasao invasaoArea = new eInvasao();

                    invasaoArea.AteAno = 2016;
                    invasaoArea.AteMes = 11;
                    invasaoArea.AteDia = 1;
                    invasaoArea.DeDia = 3;
                    invasaoArea.DeMes = 12;
                    invasaoArea.DeAno = 2001;
                    invasaoArea.AreaOperacional = 2897;

                    ltRelatorio.Text = relatorio.Relatorios(relatorio.InvasaoAreaHtmlTabela(invasaoArea), relatorio.InvasaoAreaHtmlTitulo(invasaoArea)); 

                    break;

                case "BasketMes":

                    eBasket bask = new eBasket();

                    bask.Ano = 2016;
                    bask.Mes = 10;
                    bask.Dia = 31;
                    bask.DiaDe = 31;
                    bask.MesDe = 12;
                    bask.AnoDe = 2014;
                    bask.AreaOperacional = null;
                    bask.BasketId = "5,6";
                    bask.RegiaoOperacional = null;
                    bask.RegiaoGeografico = null;
                    bask.RegiaoMetropolitana = null;
                    bask.Estado = null;
                    bask.Cidade = null;
                    bask.AreaOperacional = null;

                    ltRelatorio.Text = relatorio.Relatorios(relatorio.BasketMesHtmlTabela(bask), relatorio.BasketMesHtmlTitulo(bask)); 

                    break;

                case "BasketAno":

                    eBasket bask1 = new eBasket();

                    bask1.Ano = 2016;
                    bask1.Mes = 11;
                    bask1.Dia = 1;
                    bask1.DiaDe = 31;
                    bask1.MesDe = 12;
                    bask1.AnoDe = 2014;
                    bask1.AreaOperacional = null;
                    bask1.BasketId = "5,6";
                    bask1.RegiaoOperacional = 76;
                    bask1.RegiaoGeografico = null;
                    bask1.RegiaoMetropolitana = null;
                    bask1.Estado = null;
                    bask1.Cidade = null;
                    bask1.AreaOperacional = null;

                    ltRelatorio.Text = relatorio.Relatorios(relatorio.BasketAnoHtmlTabela(bask1), relatorio.BasketAnoHtmlTitulo(bask1)); 

                    break;

                case "BasketPeriodo":

                    eBasketPeriodo baskPer = new eBasketPeriodo();

                    baskPer.Ano = 2016;
                    baskPer.Mes = 11;
                    baskPer.Dia = 8;
                    baskPer.DiaDe = 8;
                    baskPer.MesDe = 11;
                    baskPer.AnoDe = 2016;
                    baskPer.SegAno = 2016;
                    baskPer.SegMes = 11;
                    baskPer.SegDia = 8;
                    baskPer.SegDiaDe = 8;
                    baskPer.SegMesDe = 11;
                    baskPer.SegAnoDe = 2016;
                    baskPer.AreaOperacional = null;
                    baskPer.BasketId = "5,6";
                    baskPer.RegiaoOperacional = null;
                    baskPer.RegiaoGeografico = null;
                    baskPer.RegiaoMetropolitana = null;
                    baskPer.Estado = null;
                    baskPer.Cidade = null;
                    baskPer.AreaOperacional = null;

                    ltRelatorio.Text = relatorio.Relatorios(relatorio.BasketPeriodoHtmlTabela(baskPer), relatorio.BasketPeriodoHtmlTitulo(baskPer)); 

                    break;

                case "SegmentoFUModalidade":

                    eSegmentoFU fuMod = new eSegmentoFU();

                    fuMod.Dia = 3;
                    fuMod.Mes = 11;
                    fuMod.Ano = 2016;
                    fuMod.DiaDe = 3;
                    fuMod.MesDe = 11;
                    fuMod.AnoDe = 2016;
                    fuMod.TipoVenda = 2;
                    fuMod.RegiaoOperacional = -1;
                    fuMod.RegiaoGeografico = -1;
                    fuMod.RegiaoMetropolitana = -1;
                    fuMod.Estado = -1;
                    fuMod.Cidade = -1;
                    fuMod.AreaOperacional = -1;
                    fuMod.Segmento = "-1";
                    fuMod.Marca = "-1";
                    fuMod.Modelo = "-1";

                    //Parâmetro do tipo int, serve para filtrar entre venda direta ou Varejo
                    // 0 = Traz os dois
                    // 1 = Traz Venda direta
                    // >1 = Traz Varejo

                    ltRelatorio.Text = relatorio.Relatorios(relatorio.SegmentoFUModalidadeHtmlTabela(fuMod, 0), relatorio.SegmentoFUModalidadeHtmlTitulo(fuMod)); 
                    break;

                case "SegmentoFUPeriodo":

                    eSegmentoFUPeriodo segPer = new eSegmentoFUPeriodo();

                    segPer.AnoAte = 2016;
                    segPer.MesAte = 11;
                    segPer.DiaAte = 8;
                    segPer.DiaDe = 31;
                    segPer.MesDe = 12;
                    segPer.AnoDe = 2015;
                    segPer.SegAnoAte = 2014;
                    segPer.SegMesAte = 12;
                    segPer.SegDiaAte = 31;
                    segPer.SegDiaDe = 31;
                    segPer.SegMesDe = 12;
                    segPer.SegAnoDe = 2013;
                    segPer.AreaOperacional = null;
                    segPer.TipoVenda = 2;
                    segPer.RegiaoOperacional = null;
                    segPer.RegiaoGeografico = null;
                    segPer.RegiaoMetropolitana = null;
                    segPer.Estado = null;
                    segPer.Cidade = null;
                    segPer.AreaOperacional = null;
                    segPer.Segmento = "-1";
                    segPer.Marca = "-1";
                    segPer.Modelo = "-1";

                    ltRelatorio.Text = relatorio.Relatorios(relatorio.SegmentoFUPeriodoHtmlTabela(segPer), relatorio.SegmentoFUPeriodoHtmlTitulo(segPer)); 

                    break;

                case "SegmentoFULocalidade":

                    eSegmentoFU fuLoc = new eSegmentoFU();

                    fuLoc.Dia = 18;
                    fuLoc.Mes = 11;
                    fuLoc.Ano = 2016;
                    fuLoc.DiaDe = 18;
                    fuLoc.MesDe = 11;
                    fuLoc.AnoDe = 2016;
                    fuLoc.TipoVenda = 2;
                    fuLoc.RegiaoOperacional = -1;
                    fuLoc.RegiaoGeografico = -1;
                    fuLoc.RegiaoMetropolitana = -1;
                    fuLoc.Estado = -1;
                    fuLoc.Cidade = -1;
                    fuLoc.AreaOperacional = -1;
                    fuLoc.Segmento = "-1";
                    fuLoc.Marca = "-1";
                    fuLoc.Modelo = "-1";

                    ltRelatorio.Text = relatorio.Relatorios(relatorio.SegmentoFULocalidadeHtmlTabela(fuLoc, 2), relatorio.SegmentoFULocalidadeHtmlTitulo(fuLoc, 2)); 
                    
                    break;

                case "SegmentoFUAnual":

                    eSegmentoFU fuAnu = new eSegmentoFU();

                    fuAnu.Dia = 25;
                    fuAnu.Mes = 11;
                    fuAnu.Ano = 2016;
                    fuAnu.DiaDe = 25;
                    fuAnu.MesDe = 11;
                    fuAnu.AnoDe = 2016;
                    fuAnu.TipoVenda = 1;
                    fuAnu.RegiaoOperacional = -1;
                    fuAnu.RegiaoGeografico = -1;
                    fuAnu.RegiaoMetropolitana = -1;
                    fuAnu.Estado = -1;
                    fuAnu.Cidade = -1;
                    fuAnu.AreaOperacional = -1;
                    fuAnu.Segmento = "-1";
                    fuAnu.Marca = "-1";
                    fuAnu.Modelo = "-1";
                    fuAnu.ListaAnos = "2013,2014,2015,2016";

                    ltRelatorio.Text = relatorio.Relatorios(relatorio.SegmentoFUAnualHtmlTabela(fuAnu, 2), relatorio.SegmentoFUAnualHtmlTitulo(fuAnu, 2)); 

                    break;

            }
        }
    }
}