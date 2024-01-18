using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Monitor_Producao
{
    public partial class Monitor_Linhas : System.Web.UI.Page
    {
        SqlConnection conexaobd = new SqlConnection("server=10.251.24.11;database=SGM_ONE;uid=sa;pwd=P@ssw0rd");
        SqlCommand comandoSQL = new SqlCommand();
        SqlDataReader objDataReader;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Module.Ndata = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                Module.DataAtual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                // Preencher linhas
                conexaobd.Open();
                cbolinha.Items.Clear();
                cbolinha.Items.Add("LINHA");
                cbolinha.Items.Add("RIVIAN LINHA 1");
                cbolinha.Items.Add("RIVIAN LINHA 2");
                cbolinha.Items.Add("EOL RIVIAN 1");
                cbolinha.Items.Add("EOL RIVIAN 2");
                cbolinha.Items.Add("EOL RIVIAN 3");
                cbolinha.Items.Add("EOL RIVIAN 4");
                comandoSQL.Connection = conexaobd;
                comandoSQL.CommandText = "SELECT LIN_CT, LIN_MAQUINA, LIN_NOME_LINHA FROM LINHA WHERE LIN_STATUS = 0 AND LIN_CT <> 4010 ORDER BY LIN_CT, LIN_MAQUINA ASC;";
                objDataReader = comandoSQL.ExecuteReader();
                while (objDataReader.Read())
                {
                    if (objDataReader.HasRows)
                    {
                        cbolinha.Items.Add(objDataReader["LIN_CT"].ToString() + objDataReader["LIN_MAQUINA"].ToString() + " - " + objDataReader["LIN_NOME_LINHA"].ToString());
                    }
                }
                objDataReader.Close();
                conexaobd.Close();
                Turno();
                Calculos();
                Dias();
                DiaAtual();
            }
        }

        // Cálculos
        protected void Calculos()
        {
            TARGET();
            Reset();

            // Cálculo do absenteísmo
            conexaobd.Open();
            comandoSQL.Connection = conexaobd;
            comandoSQL.CommandText = "SELECT COUNT(*) AS FALTAS FROM TAB_CONTROLE_FALTA where CONVERT(DATE, DATA_APONTAMENTO) = '" + Module.Ndata + "'  AND TURNO = '" + turno.Text.Replace("° TURNO", "") + "';";
            objDataReader = comandoSQL.ExecuteReader();
            if (objDataReader.Read())
            {
                calcAbs.Text = objDataReader["FALTAS"].ToString();
            }
            if (int.Parse(calcAbs.Text) > int.Parse(tgtAbs.Text))
            {
                PanelAbs.BackColor = System.Drawing.ColorTranslator.FromHtml("#FF5757");
            }
            else
            {
                PanelAbs.BackColor = System.Drawing.ColorTranslator.FromHtml("#00BF63");
            }
            objDataReader.Close();
            conexaobd.Close();

            // Cálculo dos Dias sem acidentes 
            int diferencaEmDias = 0;
            conexaobd.Open();
            comandoSQL.Connection = conexaobd;
            comandoSQL.CommandText = "SELECT DATA FROM TAB_ACIDENTES WHERE TIPO = 'COM AFASTAMENTO' ORDER BY DATA DESC";
            objDataReader = comandoSQL.ExecuteReader();
            if (objDataReader.Read())
            {
                DateTime dataDoBanco = objDataReader.GetDateTime(0);
                diferencaEmDias = (int)(Module.Ndata - dataDoBanco).TotalDays;
                calcAci.Text = diferencaEmDias.ToString();
            }
            else
            {
                calcAci.Text = "N/A";
            }
            objDataReader.Close();
            conexaobd.Close();

            if (diferencaEmDias < int.Parse(tgtAci.Text))
            {
                PanelAci.BackColor = System.Drawing.ColorTranslator.FromHtml("#FF5757");
            }
            else
            {
                PanelAci.BackColor = System.Drawing.ColorTranslator.FromHtml("#00BF63");
            }

            // Cálculo das reclamações
            conexaobd.Open();
            comandoSQL.Connection = conexaobd;
            comandoSQL.CommandText = "SELECT COUNT(*) AS 'RECLAMACOES' FROM TAB_RECLAMACOES WHERE DATA <= '02/10/2023 00:00:00' AND MONTH(DATA) = MONTH(GETDATE());";
            objDataReader = comandoSQL.ExecuteReader();
            if (objDataReader.Read())
            {
                calcRec.Text = objDataReader["RECLAMACOES"].ToString();
            }
            if (int.Parse(calcRec.Text) > int.Parse(tgtRec.Text))
            {
                PanelRec.BackColor = System.Drawing.ColorTranslator.FromHtml("#FF5757");
            }
            else
            {
                PanelRec.BackColor = System.Drawing.ColorTranslator.FromHtml("#00BF63");
            }
            objDataReader.Close();
            conexaobd.Close();

            if (cbolinha.Text != "LINHA")
            {
                //calculo do FTQ
                conexaobd.Open();
                comandoSQL.Connection = conexaobd;
                comandoSQL.CommandText = "SELECT RFTQ_PART_NUMBER AS PARTNUMBER, LIN_CT AS CT, LIN_MAQUINA AS MAQUINA, RFTQ_QUANTIDADE AS QTD, RFTQ_TURNO AS TURNO, RFTQ_DATA AS DATA FROM RELATORIO_FTQ ";
                comandoSQL.CommandText += "INNER JOIN LINHA ON LIN_CODIGO_LINHA = RFTQ_CODIGO_LINHA LEFT JOIN MOTIVO_FTQ ON MF_CODIGO_FTQ = RFTQ_CODIGO_FTQ WHERE LIN_CT = '" + Module.ct + "' AND LIN_MAQUINA LIKE '" + Module.maquina + "' ";
                comandoSQL.CommandText += "AND CONVERT(DATE, RFTQ_DATA) = '" + Module.Ndata + "' AND RFTQ_TURNO = '" + turno.Text.Replace("° TURNO", "") + "' ORDER BY RFTQ_DATA";
                objDataReader = comandoSQL.ExecuteReader();
                int totalQtd = 0;
                while (objDataReader.Read())
                {
                    if (objDataReader.HasRows)
                    {
                        int qtd = Convert.ToInt32(objDataReader["QTD"]);
                        totalQtd += qtd;
                    }
                }
                objDataReader.Close();
                conexaobd.Close();
                if (totalQtd > 0)
                {
                    calcFTQ.Text = totalQtd.ToString();
                }
                if (totalQtd > int.Parse(tgtFTQ.Text))
                {
                    PanelFTQ.BackColor = System.Drawing.ColorTranslator.FromHtml("#FF5757");
                }
                else
                {
                    PanelFTQ.BackColor = System.Drawing.ColorTranslator.FromHtml("#00BF63");
                }

                // Cálculo do scrap e do valor do scrap
                conexaobd.Open();
                comandoSQL.Connection = conexaobd;
                comandoSQL.CommandText = "select RSCP_QUANTIDADE,SCV_VALOR,SCV_VALOR * RSCP_QUANTIDADE as 'VALOR_TOTAL' from relatorio_scrap left join SCRAP_VALORES on RSCP_PART_NUMBER = SCV_PN and year(rscp_data) = SCV_ANO and MONTH(rscp_data) = SCV_MES ";
                comandoSQL.CommandText += "where CONVERT(DATE, RSCP_DATA) = '" + Module.Ndata + "' AND RSCP_CT = '" + Module.ct + "' AND RSCP_MAQUINA LIKE '" + Module.maquina + "' AND RSCP_APROVACAO = 1 AND RSCP_TURNO = '" + turno.Text.Replace("° TURNO", "") + "' AND RSCP_MEDIDA = 'PC'";
                objDataReader = comandoSQL.ExecuteReader();
                int somaScrap = 0;
                double somaValorScrap = 0;
                while (objDataReader.Read())
                {
                    int Scrap;
                    double ValorScrap;
                    if (int.TryParse(objDataReader["RSCP_QUANTIDADE"].ToString(), out Scrap))
                    {
                        somaScrap += Scrap;
                    }
                    if (double.TryParse(objDataReader["VALOR_TOTAL"].ToString(), out ValorScrap))
                    {
                        somaValorScrap += ValorScrap;
                    }
                }
                calcScrap.Text = somaScrap.ToString();
                calcValorScrap.Text = "R$" + somaValorScrap.ToString("F0");
                objDataReader.Close();
                conexaobd.Close();
                if (somaScrap > int.Parse(tgtScrap.Text))
                {
                    PanelScrap.BackColor = System.Drawing.ColorTranslator.FromHtml("#FF5757");
                }
                else
                {
                    PanelScrap.BackColor = System.Drawing.ColorTranslator.FromHtml("#00BF63");
                }
                if (somaValorScrap > double.Parse(tgtValorScrap.Text))
                {
                    PanelValorScrap.BackColor = System.Drawing.ColorTranslator.FromHtml("#FF5757");
                }
                else
                {
                    PanelValorScrap.BackColor = System.Drawing.ColorTranslator.FromHtml("#00BF63");
                }

                // Cálculo das peças produzidas
                conexaobd.Open();
                comandoSQL.Connection = conexaobd;
                comandoSQL.CommandText = "SELECT ETQ_QUANTIDADE ";
                comandoSQL.CommandText += "FROM ETIQUETA inner join linha on LIN_CODIGO_LINHA = ETQ_COD_LINHA ";
                comandoSQL.CommandText += "WHERE LIN_CT ='" + Module.ct + "' AND LIN_MAQUINA LIKE '" + Module.maquina + "' AND CONVERT(DATE, ETQ_DATA) = '" + Module.Ndata + "' ";
                comandoSQL.CommandText += "AND((CONVERT(VARCHAR(8), ETQ_DATA, 114) BETWEEN '06:55:01' AND '16:43:00' AND 1 = '" + turno.Text.Replace("° TURNO", "") + "') OR ";
                comandoSQL.CommandText += "(CONVERT(VARCHAR(8), ETQ_DATA, 114) BETWEEN '16:43:01' AND '23:59:59' AND 2 = '" + turno.Text.Replace("° TURNO", "") + "') OR ";
                comandoSQL.CommandText += "(CONVERT(VARCHAR(8), ETQ_DATA, 114) BETWEEN '00:00:00' AND '01:51:00' AND 2 = '" + turno.Text.Replace("° TURNO", "") + "') OR ";
                comandoSQL.CommandText += "(CONVERT(VARCHAR(8), ETQ_DATA, 114) BETWEEN '01:51:01' AND '06:55:00' AND 3 = '" + turno.Text.Replace("° TURNO", "") + "')) ";
                comandoSQL.CommandText += "AND ETQ_VERIFICACAO = 0 AND ETQ_STATUS = 'OK' order by etq_data";
                objDataReader = comandoSQL.ExecuteReader();
                int somaPecas = 0;

                while (objDataReader.Read())
                {
                    int pecas;
                    if (int.TryParse(objDataReader["ETQ_QUANTIDADE"].ToString(), out pecas))
                    {
                        somaPecas += pecas;
                    }
                }
                calcPecas.Text = somaPecas.ToString();

                objDataReader.Close();
                conexaobd.Close();

                if (somaPecas < int.Parse(tgtPecas.Text))
                {
                    PanelPecas.BackColor = System.Drawing.ColorTranslator.FromHtml("#FF5757");
                }
                else
                {
                    PanelPecas.BackColor = System.Drawing.ColorTranslator.FromHtml("#00BF63");
                }

                // Cálculo da eficiência
                int eficiencia = 0;
                if (Module.ct == "1010" || Module.ct == "3010")
                {
                    eficiencia = 0;
                    //conexaobd.Open();
                    //comandoSQL.Connection = conexaobd;
                    //comandoSQL.CommandText = "SELECT LIN_CT, LIN_MAQUINA, PRO_OPE5 ";
                    //comandoSQL.CommandText += "FROM PRODUTO RIGHT JOIN LINHA_PRODUTO ON LP_PART_NUMBER = PRO_PART_NUMBER ";
                    //comandoSQL.CommandText += "INNER JOIN LINHA ON LP_CODIGO_LINHA = LIN_CODIGO_LINHA WHERE LIN_CT = '" + Module.ct + "' AND LIN_MAQUINA LIKE '" + Module.maquina + "'";
                    //objDataReader = comandoSQL.ExecuteReader();
                    //double somaOP5 = 0;
                    //double OP5 = 0;
                    //while (objDataReader.Read())
                    //{

                    //    if (double.TryParse(objDataReader["PRO_OPE5"].ToString(), out OP5))
                    //    {
                    //        somaOP5 += OP5;
                    //    }
                    //    double resultado = somaPecas * somaOP5;
                    //    calcEfi.Text = resultado.ToString("F0") + "%";
                    //}
                    //objDataReader.Close();
                    //conexaobd.Close();
                }
                else
                {
                    conexaobd.Open();
                    comandoSQL.Connection = conexaobd;
                    comandoSQL.CommandText = "SELECT LIN_CT, LIN_MAQUINA, ";
                    comandoSQL.CommandText += "CONVERT(VARCHAR(10), RPRO_DATA_INICIO, 103) AS 'DATA', RPRO_TURNO, RPRO_CODIGO_ORDEM, RPRO_PLANEJADO, ";
                    comandoSQL.CommandText += "RPRO_PRODUZIDO FROM RELATORIO_PRODUCAO ";
                    comandoSQL.CommandText += "INNER JOIN LINHA ON RPRO_CODIGO_LINHA = LIN_CODIGO_LINHA ";
                    comandoSQL.CommandText += "INNER JOIN MOTIVO_SITUACAO_LINHA ON RPRO_SIT_LINHA = MSL_CODIGO ";
                    comandoSQL.CommandText += "WHERE RPRO_DATA_INICIO = '" + Module.Ndata + "' AND LIN_STATUS <> 1 AND RPRO_CODIGO_PLANTA = '10007' ";
                    comandoSQL.CommandText += "AND LIN_CT = '" + Module.ct + "' AND LIN_MAQUINA LIKE '" + Module.maquina + "'";
                    objDataReader = comandoSQL.ExecuteReader();

                    if (objDataReader.Read())
                    {
                        int produzido = Convert.ToInt32(objDataReader["RPRO_PRODUZIDO"]);
                        int planejado = Convert.ToInt32(objDataReader["RPRO_PLANEJADO"]);

                        if (planejado != 0)
                        {
                            eficiencia = (produzido * 100) / planejado;
                            calcEfi.Text = eficiencia.ToString() + "%";
                        }
                        else
                        {
                            calcEfi.Text = "0%";
                        }
                    }
                    objDataReader.Close();
                    conexaobd.Close();
                }
                if (eficiencia < int.Parse(tgtEfi.Text))
                {
                    PanelEfi.BackColor = System.Drawing.ColorTranslator.FromHtml("#FF5757");
                }
                else
                {
                    PanelEfi.BackColor = System.Drawing.ColorTranslator.FromHtml("#00BF63");
                }

                // Cálculo da produtividade
                // select horas geradas
                double somaHG = 0;
                double HG;
                double somaMOV = 0;
                double MOV;
                double produtividade = 0;

                conexaobd.Open();
                comandoSQL.Connection = conexaobd;
                comandoSQL.CommandText = "SELECT EHG_HORAS_GERADAS, EHG_HORAS_GERADAS_OPE5 ";
                comandoSQL.CommandText += "FROM ETIQUETA inner join linha on LIN_CODIGO_LINHA = ETQ_COD_LINHA inner join ETIQUETA_HORAS_GERADAS on EHG_CODIGO = ETQ_CODIGO ";
                comandoSQL.CommandText += "WHERE LIN_CT ='" + Module.ct + "' AND LIN_MAQUINA LIKE '" + Module.maquina + "' AND CONVERT(DATE, ETQ_DATA) = '" + Module.Ndata + "' ";
                comandoSQL.CommandText += "AND((CONVERT(VARCHAR(8), ETQ_DATA, 114) BETWEEN '06:55:01' AND '16:43:00' AND 1 = '" + turno.Text.Replace("° TURNO", "") + "') OR ";
                comandoSQL.CommandText += "(CONVERT(VARCHAR(8), ETQ_DATA, 114) BETWEEN '16:43:01' AND '23:59:59' AND 2 = '" + turno.Text.Replace("° TURNO", "") + "') OR ";
                comandoSQL.CommandText += "(CONVERT(VARCHAR(8), ETQ_DATA, 114) BETWEEN '00:00:00' AND '01:51:00' AND 2 = '" + turno.Text.Replace("° TURNO", "") + "') OR ";
                comandoSQL.CommandText += "(CONVERT(VARCHAR(8), ETQ_DATA, 114) BETWEEN '01:51:01' AND '06:55:00' AND 3 = '" + turno.Text.Replace("° TURNO", "") + "')) ";
                comandoSQL.CommandText += "AND ETQ_VERIFICACAO = 0 AND ETQ_STATUS = 'OK' order by etq_data";
                objDataReader = comandoSQL.ExecuteReader();
                if (Module.ct == "1010" || Module.ct == "3010")
                {
                    while (objDataReader.Read())
                    {
                        if (double.TryParse(objDataReader["EHG_HORAS_GERADAS_OPE5"].ToString(), out HG))
                        {
                            somaHG += HG;
                        }
                    }
                }
                else
                {
                    while (objDataReader.Read())
                    {
                        if (double.TryParse(objDataReader["EHG_HORAS_GERADAS"].ToString(), out HG))
                        {
                            somaHG += HG;
                        }
                    }
                }
                objDataReader.Close();
                conexaobd.Close();

                calcHG.Text = somaHG.ToString("F1");
                if (somaHG < int.Parse(tgtHG.Text))
                {
                    PanelHG.BackColor = System.Drawing.ColorTranslator.FromHtml("#FF5757");
                }
                else
                {
                    PanelHG.BackColor = System.Drawing.ColorTranslator.FromHtml("#00BF63");
                }

                // select duração movimentação
                conexaobd.Open();
                comandoSQL.Connection = conexaobd;
                comandoSQL.CommandText = "SELECT MOV_DATA_INICIO,MOV_DATA_FIM, CASE WHEN MOV_DATA_FIM IS NULL THEN CONVERT(VARCHAR(23), GETDATE(), 121) ";
                comandoSQL.CommandText += "ELSE CONVERT(VARCHAR(23), MOV_DATA_FIM, 121) END AS DATA_FIM_OU_ATUAL, CASE WHEN MOV_DATA_FIM IS NULL THEN DATEDIFF(SECOND, MOV_DATA_INICIO, GETDATE()) / 3600.0 ";
                comandoSQL.CommandText += "ELSE DATEDIFF(SECOND, MOV_DATA_INICIO, MOV_DATA_FIM) / 3600.0 END AS DURACAO ";
                comandoSQL.CommandText += "FROM MOVIMENTACAO INNER JOIN linha ON LIN_CODIGO_LINHA = MOV_LINHA_DESTINO WHERE LIN_CT = '" + Module.ct + "' ";
                comandoSQL.CommandText += "AND LIN_MAQUINA LIKE '" + Module.maquina + "' AND CONVERT(DATE, MOV_DATA_INICIO) = '" + Module.Ndata + "' ";
                comandoSQL.CommandText += "AND((CONVERT(VARCHAR(8), MOV_DATA_INICIO, 114) BETWEEN '06:55:01' AND '16:43:00' AND 1 = '" + turno.Text.Replace("° TURNO", "") + "') OR ";
                comandoSQL.CommandText += "(CONVERT(VARCHAR(8), MOV_DATA_INICIO, 114) BETWEEN '16:43:01' AND '23:59:59' AND 2 = '" + turno.Text.Replace("° TURNO", "") + "') OR ";
                comandoSQL.CommandText += "(CONVERT(VARCHAR(8), MOV_DATA_INICIO, 114) BETWEEN '00:00:00' AND '01:51:00' AND 2 = '" + turno.Text.Replace("° TURNO", "") + "') OR ";
                comandoSQL.CommandText += "(CONVERT(VARCHAR(8), MOV_DATA_INICIO, 114) BETWEEN '01:51:01' AND '06:55:00' AND 3 = '" + turno.Text.Replace("° TURNO", "") + "')) order by MOV_DATA_INICIO; ";
                objDataReader = comandoSQL.ExecuteReader();
                while (objDataReader.Read())
                {
                    if (double.TryParse(objDataReader["DURACAO"].ToString(), out MOV))
                    {
                        somaMOV += MOV;
                    }
                }
                objDataReader.Close();
                conexaobd.Close();


                calcMOV.Text = somaMOV.ToString("F1");
                if (somaMOV < int.Parse(tgtMOV.Text))
                {
                    PanelMOV.BackColor = System.Drawing.ColorTranslator.FromHtml("#FF5757");
                }
                else
                {
                    PanelMOV.BackColor = System.Drawing.ColorTranslator.FromHtml("#00BF63");
                }

                // Cálculo da % de produtividade
                if (somaHG != 0 & somaMOV != 0)
                {
                    produtividade = (somaHG * 100) / somaMOV;
                    calcProd.Text = produtividade.ToString("F0") + "%";
                }
                else
                {
                    calcProd.Text = "0%";
                }

                if (produtividade < int.Parse(tgtProd.Text))
                {
                    PanelProd.BackColor = System.Drawing.ColorTranslator.FromHtml("#FF5757");
                }
                else
                {
                    PanelProd.BackColor = System.Drawing.ColorTranslator.FromHtml("#00BF63");
                }

                // Cálculo das paradas
                double somaParadas = 0;
                double Paradas = 0;
                conexaobd.Open();
                comandoSQL.Connection = conexaobd;
                comandoSQL.CommandText = "SELECT RPAR_DATA_INICIO, RPAR_DATA_FIM, CASE WHEN RPAR_DATA_FIM IS NULL THEN CONVERT(VARCHAR(23), GETDATE(), 121) ";
                comandoSQL.CommandText += "ELSE CONVERT(VARCHAR(23), RPAR_DATA_FIM, 121) END AS DATA_FIM_OU_ATUAL, CASE WHEN RPAR_DATA_FIM IS NULL THEN DATEDIFF(SECOND, RPAR_DATA_INICIO, GETDATE()) / 3600.0 ";
                comandoSQL.CommandText += "ELSE DATEDIFF(SECOND, RPAR_DATA_INICIO, RPAR_DATA_FIM) / 3600.0 END AS DURACAO FROM RELATORIO_PARADA INNER JOIN linha ON LIN_CODIGO_LINHA = RPAR_CODIGO_LINHA WHERE LIN_CT = '" + Module.ct + "' ";
                comandoSQL.CommandText += "AND LIN_MAQUINA LIKE '" + Module.maquina + "' AND CONVERT(DATE, RPAR_DATA_INICIO) = '" + Module.Ndata + "' AND RPAR_TURNO = '" + turno.Text.Replace("° TURNO", "") + "'";
                objDataReader = comandoSQL.ExecuteReader();
                while (objDataReader.Read())
                {
                    if (double.TryParse(objDataReader["DURACAO"].ToString(), out Paradas))
                    {
                        somaParadas += Paradas;
                    }
                }
                calcParada.Text = somaParadas.ToString("F1");
                objDataReader.Close();
                conexaobd.Close();

                if (somaParadas > int.Parse(tgtParada.Text))
                {
                    PanelParada.BackColor = System.Drawing.ColorTranslator.FromHtml("#FF5757");
                }
                else
                {
                    PanelParada.BackColor = System.Drawing.ColorTranslator.FromHtml("#00BF63");
                }

            }
            else
            {
                calcFTQ.Text = "0";
                PanelFTQ.BackColor = System.Drawing.ColorTranslator.FromHtml("#737373");
                calcScrap.Text = "0";
                PanelScrap.BackColor = System.Drawing.ColorTranslator.FromHtml("#737373");
                calcValorScrap.Text = "R$0";
                PanelValorScrap.BackColor = System.Drawing.ColorTranslator.FromHtml("#737373");
                calcPecas.Text = "0";
                PanelPecas.BackColor = System.Drawing.ColorTranslator.FromHtml("#737373");
                calcEfi.Text = "0%";
                PanelEfi.BackColor = System.Drawing.ColorTranslator.FromHtml("#737373");
                calcProd.Text = "0%";
                PanelProd.BackColor = System.Drawing.ColorTranslator.FromHtml("#737373");
                calcHG.Text = "0";
                PanelHG.BackColor = System.Drawing.ColorTranslator.FromHtml("#737373");
                calcMOV.Text = "0";
                PanelMOV.BackColor = System.Drawing.ColorTranslator.FromHtml("#737373");
                calcParada.Text = "0";
                PanelParada.BackColor = System.Drawing.ColorTranslator.FromHtml("#737373");
            }
            UpdatePanelS.Update();
            UpdatePanelQ.Update();
            UpdatePanelD.Update();
            UpdatePanelP.Update();
        }

        //protected void Timer1_Tick(object sender, EventArgs e)//timer 1s em 1s
        //{
        //    //data.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //}
        protected void Timer2_Tick(object sender, EventArgs e)//timer 1min em 1min
        {
            if (Module.DataAtual != new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day))
            {
                DiaAtual();
                Module.DataAtual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            }
            Turno();
            Dias();
            Calculos();
            UpdatePanelTurno.Update();
        }

        //protected void TimerLinha_Tick(object sender, EventArgs e)//timer 5min em 5min
        //{
        //    string[] filaCircular = { "RIVIAN LINHA 1", "RIVIAN LINHA 2", "EOL RIVIAN 1", "EOL RIVIAN 2", "EOL RIVIAN 3", "EOL RIVIAN 4" };
        //    string[] maquinas = { "30", "40", "50", "60", "70", "80" };

        //    // Encontre a posição atual na fila
        //    int posicaoAtual = Array.IndexOf(filaCircular, cbolinha.Text);

        //    if (posicaoAtual >= 0)
        //    {
        //        // Avance para a próxima posição na fila (circular)
        //        posicaoAtual = (posicaoAtual + 1) % filaCircular.Length;

        //        // Atualize os valores com base na nova posição
        //        cbolinha.Text = filaCircular[posicaoAtual];
        //        Module.maquina = maquinas[posicaoAtual];
        //    }
        //    UpdatePanelLinha.Update();
        //    Calculos();
        //}

        // Turno
        protected void Turno()
        {

            DateTime currentTime = DateTime.Now;
            TimeSpan time = currentTime.TimeOfDay;
            if (time >= new TimeSpan(16, 44, 59) && time <= new TimeSpan(23, 59, 59))
            {
                turno.Text = "2° TURNO";
            }
            else if (time >= new TimeSpan(0, 0, 0) && time <= new TimeSpan(6, 44, 59))
            {
                turno.Text = "3° TURNO";
            }
            else
            {
                turno.Text = "1° TURNO";
            }
        }

        protected void DiaButton_Click(object sender, EventArgs e)//dias
        {
            Button diaButton = (Button)sender;
            if (int.Parse(diaButton.Text) <= DateTime.Now.Day)
            {
                diaButton.CssClass = "btndia-selecionado";
                foreach (Control control in Panel17.Controls)
                {
                    if (control is Button && control != diaButton)
                    {
                        ((Button)control).CssClass = "btndia";
                    }
                }
                Module.Ndata = new DateTime(DateTime.Now.Year, DateTime.Now.Month, int.Parse(diaButton.Text));
                Calculos();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ExibirModalScript", Module.Modal(), false);
                titulo_modal.Text = "Erro";
                texto_modal.Text = "Não temos dados para a data selecionada";
                return;
            }
        }

        protected void Dias()
        {
            // Dias
            if (DateTime.Now.Month == 2)
            {
                if (DateTime.Now.Year == 2024 || DateTime.Now.Year == 2028 || DateTime.Now.Year == 2032 || DateTime.Now.Year == 2036)
                {
                    dia29.Visible = true;
                    dia30.Visible = false;
                    dia31.Visible = false;
                }
                else
                {
                    dia29.Visible = false;
                    dia30.Visible = false;
                    dia31.Visible = false;
                }
            }
            else if (DateTime.Now.Month == 4 || DateTime.Now.Month == 6 || DateTime.Now.Month == 9 || DateTime.Now.Month == 11)
            {
                dia29.Visible = true;
                dia30.Visible = true;
                dia31.Visible = false;
            }
            else
            {
                dia29.Visible = true;
                dia30.Visible = true;
                dia31.Visible = true;
            }

            string[] nomesMeses = { "JAN", "FEV", "MAR", "ABR", "MAI", "JUN", "JUL", "AGO", "SET", "OUT", "NOV", "DEZ" };

            int mesAtual = DateTime.Now.Month;

            if (mesAtual >= 1 && mesAtual <= 12)
            {
                lblmes.Text = nomesMeses[mesAtual - 1];
            }
            else
            {
                lblmes.Text = "MÊS";
            }
        }

        protected void DiaAtual()
        {
            Module.Ndata = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            string buttonId = "dia" + Module.Ndata.Day;
            Button dia = Panel17.FindControl(buttonId) as Button;
            foreach (Control control in Panel17.Controls)
            {
                if (control is Button && control != dia)
                {
                    ((Button)control).CssClass = "btndia";
                }
            }
            if (dia != null)
            {
                dia.CssClass = "btndia-selecionado";
            }
        }

        protected void cbolinha_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbolinha.Text == "LINHA")
            {
                Reset();
                Module.ct = null;
                Module.maquina = null;
            }
            else if (cbolinha.Text == "RIVIAN LINHA 1")
            {
                Module.ct = "4010";
                Module.maquina = "30%";
            }
            else if (cbolinha.Text == "RIVIAN LINHA 2")
            {
                Module.ct = "4010";
                Module.maquina = "40%";
            }
            else if (cbolinha.Text == "EOL RIVIAN 1")
            {
                Module.ct = "4010";
                Module.maquina = "50%";
            }
            else if (cbolinha.Text == "EOL RIVIAN 2")
            {
                Module.ct = "4010";
                Module.maquina = "60%";
            }
            else if (cbolinha.Text == "EOL RIVIAN 3")
            {
                Module.ct = "4010";
                Module.maquina = "70%";
            }
            else if (cbolinha.Text == "EOL RIVIAN 4")
            {
                Module.ct = "4010";
                Module.maquina = "80%";
            }
            else
            {
                Module.ct = cbolinha.SelectedItem.Value.Substring(0, 4);
                string selectedValue = cbolinha.SelectedItem.ToString();
                int indexOfDash = selectedValue.IndexOf(" - ");
                if (indexOfDash != -1 && indexOfDash >= 4)
                {
                    Module.maquina = selectedValue.Substring(4, indexOfDash - 4);
                }
            }

            Calculos();

            //if (cbolinha.Text == "RIVIAN LINHA 1" || cbolinha.Text == "RIVIAN LINHA 2" || cbolinha.Text == "EOL RIVIAN 1" || cbolinha.Text == "EOL RIVIAN 2" || cbolinha.Text == "EOL RIVIAN 3" || cbolinha.Text == "EOL RIVIAN 4")
            //{
            //    TimerLinha.Enabled = true;
            //}
            //else
            //{
            //    TimerLinha.Enabled = false;
            //}
        }

        protected void Reset()
        {
            calcAbs.Text = "0";
            calcAci.Text = "0";
            calcRec.Text = "0";

            calcFTQ.Text = "0";
            calcScrap.Text = "0";
            calcValorScrap.Text = "R$0";
            calcPecas.Text = "0";
            calcEfi.Text = "0%";
            calcHG.Text = "0";
            calcMOV.Text = "0";
            calcProd.Text = "0%";
            calcParada.Text = "0";
        }

        //TARGET
        protected void TARGET()
        {
            if (cbolinha.Text != "LINHA")
            {
                conexaobd.Open();
                if (cbolinha.Text == "RIVIAN LINHA 1" || cbolinha.Text == "RIVIAN LINHA 2" || cbolinha.Text == "EOL RIVIAN 1" || cbolinha.Text == "EOL RIVIAN 2" || cbolinha.Text == "EOL RIVIAN 3" || cbolinha.Text == "EOL RIVIAN 4")
                {
                    comandoSQL.Connection = conexaobd;
                    comandoSQL.CommandText = "SELECT * FROM TAB_TARGET WHERE TARGET_LINHA LIKE '4010"+Module.maquina+"' ";
                    comandoSQL.CommandText += "AND TARGET_TURNO = '" + turno.Text.Replace("° TURNO", "") + "'";
                    objDataReader = comandoSQL.ExecuteReader();
                    if (objDataReader.Read())
                    {
                        tgtFTQ.Text = objDataReader["TARGET_FTQ"].ToString();
                        tgtScrap.Text = objDataReader["TARGET_SCRAP"].ToString();
                        tgtValorScrap.Text = objDataReader["TARGET_VALOR_SCRAP"].ToString();
                        tgtPecas.Text = objDataReader["TARGET_PECAS"].ToString();
                        tgtEfi.Text = objDataReader["TARGET_EFICIENCIA"].ToString();
                        tgtHG.Text = objDataReader["TARGET_HORAS_PRODUZIDAS"].ToString();
                        tgtMOV.Text = objDataReader["TARGET_HORAS_MOVIMENTADAS"].ToString();
                        tgtProd.Text = objDataReader["TARGET_PRODUTIVIDADE"].ToString();
                        tgtParada.Text = objDataReader["TARGET_HORAS_PARADAS"].ToString();
                    }
                }
                else
                {
                    comandoSQL.Connection = conexaobd;
                    comandoSQL.CommandText = "SELECT * FROM TAB_TARGET INNER JOIN LINHA ON LIN_CODIGO_LINHA = TARGET_LINHA WHERE LIN_CT = '" + Module.ct + "' ";
                    comandoSQL.CommandText += "AND LIN_MAQUINA LIKE '" + Module.maquina + "' AND TARGET_TURNO = '" + turno.Text.Replace("° TURNO", "") + "'";
                    objDataReader = comandoSQL.ExecuteReader();
                    if (objDataReader.Read())
                    {

                        tgtFTQ.Text = objDataReader["TARGET_FTQ"].ToString();
                        tgtScrap.Text = objDataReader["TARGET_SCRAP"].ToString();
                        tgtValorScrap.Text = objDataReader["TARGET_VALOR_SCRAP"].ToString();
                        tgtPecas.Text = objDataReader["TARGET_PECAS"].ToString();
                        tgtEfi.Text = objDataReader["TARGET_EFICIENCIA"].ToString();
                        tgtHG.Text = objDataReader["TARGET_HORAS_PRODUZIDAS"].ToString();
                        tgtMOV.Text = objDataReader["TARGET_HORAS_MOVIMENTADAS"].ToString();
                        tgtProd.Text = objDataReader["TARGET_PRODUTIVIDADE"].ToString();
                        tgtParada.Text = objDataReader["TARGET_HORAS_PARADAS"].ToString();
                    }
                }
                objDataReader.Close();
                conexaobd.Close();
            }
        }

        // Modal UP SQDP
        protected void btns_Click(object sender, ImageClickEventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ExibirModalScript", Module.Modal(), false);
            titulo_modal.Text = "Segurança";
            //texto_modal.Text = "<strong>Mão de obra FCST</strong></br> Texto</br>";
            texto_modal.Text = "<strong>Absenteísmo</strong></br> Faltas de funcionários da manufatura (por turno)</br>";
            texto_modal.Text += "<strong>Dias sem Acidentes</strong></br> Período de dias sem acidentes da fábrica (total)";
        }
        protected void btnq_Click(object sender, ImageClickEventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ExibirModalScript", Module.Modal(), false);
            titulo_modal.Text = "Qualidade";
            texto_modal.Text = "<strong>FTQ</strong> First Time Quality</br>Falhas em peças (por linha e turno)</br>";
            texto_modal.Text += "<strong>Scrap</strong></br> Peças jogadas fora (por linha e turno)</br>";
            texto_modal.Text += "<strong>Valor Scrap</strong></br> Preço das preças jogadas fora (por linha e turno)</br>";
            texto_modal.Text += "<strong>Reclamações</strong></br> Reclamações da fábrica (por mês)";

        }
        protected void btnd_Click(object sender, ImageClickEventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ExibirModalScript", Module.Modal(), false);
            titulo_modal.Text = "Delivery";
            texto_modal.Text = "<strong>Peças Produzidas</strong></br> Peças produzidas (por linha e turno)</br>";
            texto_modal.Text += "<strong>Eficiência</strong></br> Produzido / Planejado (por linha e turno)";
        }
        protected void btnp_Click(object sender, ImageClickEventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ExibirModalScript", Module.Modal(), false);
            titulo_modal.Text = "Produção";
            texto_modal.Text = "<strong>Horas produzidas </strong></br> (por linha e turno)</br>";
            texto_modal.Text += "<strong>Horas movimentadas</strong></br> (por linha e turno)</br>";
            texto_modal.Text += "<strong>Produtividade</strong></br> Horas geradas / Horas movimentadas (por linha e turno)</br>";
            texto_modal.Text += "<strong>Horas paradas</strong></br> (por linha e turno)";
        }
    }
}