<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Monitor_Linhas.aspx.cs" Inherits="Monitor_Producao.Monitor_Linhas" %>

<!DOCTYPE html>

<head>
    <title>Monitor de Linhas</title>
    <link href="linhas.css" rel="stylesheet" />
    <link rel="shortcut icon" href="../Images/icon-prod.ico" type="image/x-icon">
    <script>
        function exibirModal() {
            var modal = document.querySelector('.modal');
            modal.style.display = 'block';
        }

        function fecharModal() {
            var modal = document.querySelector('.modal');
            modal.style.display = 'none';
        }
    </script>
</head>

<body>
    <form id="form1" runat="server">

        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <asp:Panel ID="Panel16" runat="server" CssClass="navbar">
            <%--            <asp:Panel ID="Panel21" Style="display: inline-block;" runat="server" Width="216px">
                <img src="../Images/APTIV_LOGO.png" alt="Logo" class="logo-container" />
            </asp:Panel>--%>
            <%--                    <asp:Panel ID="Panel20" Style="display: inline-block;" runat="server">
                        <asp:Label ID="data" runat="server" Text="00/00/0000 00:00:00" Font-Size="24px"></asp:Label>
                    </asp:Panel>
                    &nbsp--%>

            <asp:Label ID="Label25" runat="server" Text="•" CssClass="navbar-text-ponto"></asp:Label>
            &nbsp&nbsp
                        <b>
                            <asp:Label ID="Label23" runat="server" Text="APTIV" Style="letter-spacing: 3px;" CssClass="navbar-text"></asp:Label></b>
            &nbsp
                <asp:Label ID="Label2" runat="server" Text="/ DASHBOARD" CssClass="navbar-text"></asp:Label>
            &nbsp &nbsp
            <asp:UpdatePanel ID="UpdatePanelLinha" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Panel ID="Panel22" Style="display: inline-block;" runat="server">
                        <asp:DropDownList ID="cbolinha" runat="server" CssClass="cbolinha" AutoPostBack="True" OnSelectedIndexChanged="cbolinha_SelectedIndexChanged"></asp:DropDownList>
                    </asp:Panel>
                </ContentTemplate>
                <Triggers>
<%--                    <asp:AsyncPostBackTrigger ControlID="TimerLinha" EventName="Tick" />--%>
                    <asp:AsyncPostBackTrigger ControlID="cbolinha" EventName="SelectedIndexChanged" />
                </Triggers>
            </asp:UpdatePanel>
            &nbsp &nbsp
                                    <asp:UpdatePanel ID="UpdatePanelTurno" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Panel ID="Panel24" Style="display: inline-block;" runat="server">
                                                <asp:Label ID="turno" runat="server" Text="0° TURNO" CssClass="navbar-text"></asp:Label>
                                            </asp:Panel>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="Timer2" EventName="Tick" />
                                        </Triggers>
                                    </asp:UpdatePanel>
            &nbsp&nbsp
                <asp:Label ID="Label26" runat="server" Text="•" CssClass="navbar-text-ponto"></asp:Label>
        </asp:Panel>
        <%--        <asp:Timer ID="Timer1" runat="server" Interval="1000" OnTick="Timer1_Tick"></asp:Timer>--%>
        <div class="container">
            <asp:Panel ID="Panel14" Class="linha" runat="server">
                <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="Panel1" Class="titulo-linha" runat="server">
                            <asp:Label ID="Label1" Class="SQDPK" runat="server" Text="S"></asp:Label>
                            <asp:ImageButton ID="btns" src="../Images/ICON_DOC.png" runat="server" Style="height: 34px; width: 34px; position: absolute; top: 0; right: 0; margin-top: 10px; margin-right: 10px;" OnClick="btns_Click" />
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdatePanel ID="UpdatePanelS" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel ID="PanelFCST" Class="box" runat="server" Visible="false">
                            <asp:Label ID="Label15" Class="titulo" runat="server" Text="MÃO DE OBRA FCST"></asp:Label>
                            <asp:Label ID="calcFCST" Class="texto" runat="server" Text="0"></asp:Label>
                            <div>
                                <asp:Label ID="Label16" Class="target" runat="server" Text="TARGET"></asp:Label>&nbsp;<asp:Label ID="tgtFCST" Class="target" runat="server" Text="0" Font-Bold="True"></asp:Label></div>
                        </asp:Panel>
                        <asp:Panel ID="PanelAbs" Class="box" runat="server" Visible="true">
                            <asp:Label ID="Label19" Class="titulo" runat="server" Text="ABSENTEÍSMO"></asp:Label>
                            <asp:Label ID="calcAbs" Class="texto" runat="server" Text="0"></asp:Label>
                            <div>
                                <asp:Label ID="Label33" Class="target" runat="server" Text="TARGET"></asp:Label>&nbsp;<asp:Label ID="tgtAbs" Class="target" runat="server" Text="0" Font-Bold="True"></asp:Label></div>
                        </asp:Panel>
                        <asp:Panel ID="PanelAci" Class="box" runat="server">
                            <asp:Label ID="Label4" Class="titulo" runat="server" Text="DIAS S/ ACIDENTE"></asp:Label>
                            <asp:Label ID="calcAci" Class="texto" runat="server" Text="0"></asp:Label>
                            <div>
                                <asp:Label ID="Label35" Class="target" runat="server" Text="TARGET"></asp:Label>&nbsp;<asp:Label ID="tgtAci" Class="target" runat="server" Text="500" Font-Bold="True"></asp:Label></div>
                        </asp:Panel>

                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="Timer2" EventName="Tick" />
<%--                        <asp:AsyncPostBackTrigger ControlID="TimerLinha" EventName="Tick" />--%>
                    </Triggers>
                </asp:UpdatePanel>

            </asp:Panel>
            <br />

            <asp:Panel ID="Panel2" Class="linha" runat="server">
                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="Panel5" Class="titulo-linha" runat="server">
                            <asp:Label ID="Label5" Class="SQDPK" runat="server" Text="Q"></asp:Label>
                            <asp:ImageButton ID="btnq" src="../Images/ICON_DOC.png" runat="server" Style="height: 34px; width: 34px; position: absolute; top: 0; right: 0; margin-top: 10px; margin-right: 10px;" OnClick="btnq_Click" />
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdatePanel ID="UpdatePanelQ" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel ID="PanelFTQ" Class="box" runat="server">
                            <asp:Label ID="Label6" Class="titulo" runat="server" Text="FTQ"></asp:Label>
                            <asp:Label ID="calcFTQ" Class="texto" runat="server" Text="0"></asp:Label>
                            <div>
                                <asp:Label ID="Label20" Class="target" runat="server" Text="TARGET"></asp:Label>&nbsp;<asp:Label ID="tgtFTQ" Class="target" runat="server" Text="0" Font-Bold="True"></asp:Label></div>
                        </asp:Panel>
                        <asp:Panel ID="PanelScrap" Class="box" runat="server">
                            <asp:Label ID="Label7" Class="titulo" runat="server" Text="SCRAP"></asp:Label>
                            <asp:Label ID="calcScrap" Class="texto" runat="server" Text="0"></asp:Label>
                            <div>
                                <asp:Label ID="Label22" Class="target" runat="server" Text="TARGET"></asp:Label>&nbsp;<asp:Label ID="tgtScrap" Class="target" runat="server" Text="0" Font-Bold="True"></asp:Label></div>
                        </asp:Panel>
                        <asp:Panel ID="PanelValorScrap" Class="box" runat="server">
                            <asp:Label ID="Label3" Class="titulo" runat="server" Text="VALOR SCRAP"></asp:Label>
                            <asp:Label ID="calcValorScrap" Class="texto" runat="server" Text="0"></asp:Label>
                            <div>
                                <asp:Label ID="Label17" Class="target" runat="server" Text="TARGET"></asp:Label>&nbsp;<asp:Label ID="tgtValorScrap" Class="target" runat="server" Text="0" Font-Bold="True"></asp:Label><asp:Label ID="Label37" Class="target" runat="server" Text="$" Font-Bold="True"></asp:Label></div>
                        </asp:Panel>
                        <asp:Panel ID="PanelRec" Class="box" runat="server">
                            <asp:Label ID="Label13" Class="titulo" runat="server" Text="RECLAMAÇÕES"></asp:Label>
                            <asp:Label ID="calcRec" Class="texto" runat="server" Text="0"></asp:Label>
                            <div>
                                <asp:Label ID="Label24" Class="target" runat="server" Text="TARGET"></asp:Label>&nbsp;<asp:Label ID="tgtRec" Class="target" runat="server" Text="0" Font-Bold="True"></asp:Label></div>
                        </asp:Panel>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="Timer2" EventName="Tick" />
<%--                        <asp:AsyncPostBackTrigger ControlID="TimerLinha" EventName="Tick" />--%>
                    </Triggers>
                </asp:UpdatePanel>

            </asp:Panel>
            <br />

            <asp:Panel ID="Panel3" Class="linha" runat="server">
                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="Panel10" Class="titulo-linha" runat="server">
                            <asp:Label ID="Label8" Class="SQDPK" runat="server" Text="D"></asp:Label>
                            <asp:ImageButton ID="btnd" src="../Images/ICON_DOC.png" runat="server" Style="height: 34px; width: 34px; position: absolute; top: 0; right: 0; margin-top: 10px; margin-right: 10px;" OnClick="btnd_Click" />
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdatePanel ID="UpdatePanelD" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel ID="PanelPecas" Class="box" runat="server">
                            <asp:Label ID="Label9" Class="titulo" runat="server" Text="PEÇAS PRODUZIDAS"></asp:Label>
                            <asp:Label ID="calcPecas" Class="texto" runat="server" Text="0"></asp:Label>
                            <div>
                                <asp:Label ID="Label28" Class="target" runat="server" Text="TARGET"></asp:Label>&nbsp;<asp:Label ID="tgtPecas" Class="target" runat="server" Text="0" Font-Bold="True"></asp:Label></div>
                        </asp:Panel>
                        <asp:Panel ID="PanelEfi" Class="box" runat="server">
                            <asp:Label ID="Label10" Class="titulo" runat="server" Text="EFICIÊNCIA"></asp:Label>
                            <asp:Label ID="calcEfi" Class="texto" runat="server" Text="0%"></asp:Label>
                            <div>
                                <asp:Label ID="Label30" Class="target" runat="server" Text="TARGET"></asp:Label>&nbsp;<asp:Label ID="tgtEfi" Class="target" runat="server" Text="0" Font-Bold="True"></asp:Label><asp:Label ID="Label27" Class="target" runat="server" Text="%" Font-Bold="True"></asp:Label></div>
                        </asp:Panel>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="Timer2" EventName="Tick" />
<%--                        <asp:AsyncPostBackTrigger ControlID="TimerLinha" EventName="Tick" />--%>
                    </Triggers>
                </asp:UpdatePanel>

            </asp:Panel>
            <br />

            <asp:Panel ID="Panel4" Class="linha" runat="server">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="Panel13" Class="titulo-linha" runat="server">
                            <asp:Label ID="Label11" Class="SQDPK" runat="server" Text="P"></asp:Label>
                            <asp:ImageButton ID="btnp" src="../Images/ICON_DOC.png" runat="server" Style="height: 34px; width: 34px; position: absolute; top: 0; right: 0; margin-top: 10px; margin-right: 10px;" OnClick="btnp_Click" />
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdatePanel ID="UpdatePanelP" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel ID="PanelProd" Class="box" runat="server">
                            <asp:Label ID="Label12" Class="titulo" runat="server" Text="PRODUTIVIDADE"></asp:Label>
                            <asp:Label ID="calcProd" Class="texto" runat="server" Text="0%"></asp:Label>
                            <div>
                                <asp:Label ID="Label32" Class="target" runat="server" Text="TARGET"></asp:Label>&nbsp;<asp:Label ID="tgtProd" Class="target" runat="server" Text="0" Font-Bold="True"></asp:Label><asp:Label ID="Label36" Class="target" runat="server" Text="%" Font-Bold="True"></asp:Label></div>
                        </asp:Panel>
                        <asp:Panel ID="PanelHG" Class="box" runat="server">
                            <asp:Label ID="Label18" Class="titulo" runat="server" Text="HORAS PRODUZIDAS"></asp:Label>
                            <asp:Label ID="calcHG" Class="texto" runat="server" Text="0"></asp:Label>
                            <div>
                                <asp:Label ID="Label29" Class="target" runat="server" Text="TARGET"></asp:Label>&nbsp;<asp:Label ID="tgtHG" Class="target" runat="server" Text="0" Font-Bold="True"></asp:Label></div>
                        </asp:Panel>
                        <asp:Panel ID="PanelMOV" Class="box" runat="server">
                            <asp:Label ID="Label31" Class="titulo" runat="server" Text="HORAS MOVIMENTADAS"></asp:Label>
                            <asp:Label ID="calcMOV" Class="texto" runat="server" Text="0"></asp:Label>
                            <div>
                                <asp:Label ID="Label34" Class="target" runat="server" Text="TARGET"></asp:Label>&nbsp;<asp:Label ID="tgtMOV" Class="target" runat="server" Text="0" Font-Bold="True"></asp:Label></div>
                        </asp:Panel>
                        <asp:Panel ID="PanelParada" Class="box" runat="server">
                            <asp:Label ID="Label14" Class="titulo" runat="server" Text="HORAS PARADAS"></asp:Label>
                            <asp:Label ID="calcParada" Class="texto" runat="server" Text="0"></asp:Label>
                            <div>
                                <asp:Label ID="Label21" Class="target" runat="server" Text="TARGET"></asp:Label>&nbsp;<asp:Label ID="tgtParada" Class="target" runat="server" Text="0" Font-Bold="True"></asp:Label></div>
                        </asp:Panel>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="Timer2" EventName="Tick" />
<%--                        <asp:AsyncPostBackTrigger ControlID="TimerLinha" EventName="Tick" />--%>
                    </Triggers>
                </asp:UpdatePanel>
            </asp:Panel>

        </div>
<%--        <asp:Timer ID="TimerLinha" runat="server" OnTick="TimerLinha_Tick" Interval="300000" Enabled="False"></asp:Timer>--%>
        <asp:Timer ID="Timer2" runat="server" OnTick="Timer2_Tick" Interval="51000"></asp:Timer>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Panel ID="Panel17" Class="dias" runat="server" Style="margin-bottom: 0px">
                    <asp:Panel ID="mes" Class="mes" runat="server">
                        <asp:Label ID="lblmes" Style="font-size: 14px; font-weight: 600;" runat="server" Text="MÊS"></asp:Label>
                    </asp:Panel>
                    <asp:Button ID="dia1" Class="btndia" Text="1" runat="server" OnClick="DiaButton_Click" />
                    <asp:Button ID="dia2" Class="btndia" Text="2" runat="server" OnClick="DiaButton_Click" />
                    <asp:Button ID="dia3" Class="btndia" Text="3" runat="server" OnClick="DiaButton_Click" />
                    <asp:Button ID="dia4" Class="btndia" Text="4" runat="server" OnClick="DiaButton_Click" />
                    <asp:Button ID="dia5" Class="btndia" Text="5" runat="server" OnClick="DiaButton_Click" />
                    <asp:Button ID="dia6" Class="btndia" Text="6" runat="server" OnClick="DiaButton_Click" />
                    <asp:Button ID="dia7" Class="btndia" Text="7" runat="server" OnClick="DiaButton_Click" />
                    <asp:Button ID="dia8" Class="btndia" Text="8" runat="server" OnClick="DiaButton_Click" />
                    <asp:Button ID="dia9" Class="btndia" Text="9" runat="server" OnClick="DiaButton_Click" />
                    <asp:Button ID="dia10" Class="btndia" Text="10" runat="server" OnClick="DiaButton_Click" />
                    <asp:Button ID="dia11" Class="btndia" Text="11" runat="server" OnClick="DiaButton_Click" />
                    <asp:Button ID="dia12" Class="btndia" Text="12" runat="server" OnClick="DiaButton_Click" />
                    <asp:Button ID="dia13" Class="btndia" Text="13" runat="server" OnClick="DiaButton_Click" />
                    <asp:Button ID="dia14" Class="btndia" Text="14" runat="server" OnClick="DiaButton_Click" />
                    <asp:Button ID="dia15" Class="btndia" Text="15" runat="server" OnClick="DiaButton_Click" />
                    <asp:Button ID="dia16" Class="btndia" Text="16" runat="server" OnClick="DiaButton_Click" />
                    <asp:Button ID="dia17" Class="btndia" Text="17" runat="server" OnClick="DiaButton_Click" />
                    <asp:Button ID="dia18" Class="btndia" Text="18" runat="server" OnClick="DiaButton_Click" />
                    <asp:Button ID="dia19" Class="btndia" Text="19" runat="server" OnClick="DiaButton_Click" />
                    <asp:Button ID="dia20" Class="btndia" Text="20" runat="server" OnClick="DiaButton_Click" />
                    <asp:Button ID="dia21" Class="btndia" Text="21" runat="server" OnClick="DiaButton_Click" />
                    <asp:Button ID="dia22" Class="btndia" Text="22" runat="server" OnClick="DiaButton_Click" />
                    <asp:Button ID="dia23" Class="btndia" Text="23" runat="server" OnClick="DiaButton_Click" />
                    <asp:Button ID="dia24" Class="btndia" Text="24" runat="server" OnClick="DiaButton_Click" />
                    <asp:Button ID="dia25" Class="btndia" Text="25" runat="server" OnClick="DiaButton_Click" />
                    <asp:Button ID="dia26" Class="btndia" Text="26" runat="server" OnClick="DiaButton_Click" />
                    <asp:Button ID="dia27" Class="btndia" Text="27" runat="server" OnClick="DiaButton_Click" />
                    <asp:Button ID="dia28" Class="btndia" Text="28" runat="server" OnClick="DiaButton_Click" />
                    <asp:Button ID="dia29" Class="btndia" Text="29" runat="server" OnClick="DiaButton_Click" />
                    <asp:Button ID="dia30" Class="btndia" Text="30" runat="server" OnClick="DiaButton_Click" />
                    <asp:Button ID="dia31" Class="btndia" Text="31" runat="server" OnClick="DiaButton_Click" />
                </asp:Panel>
                <%-- modal UP--%>
                <div class="modal">
                    <label class="x" onclick="fecharModal()">x</label>
                    <asp:Label ID="titulo_modal" class="titulo_modal" runat="server" Text="titulo_modal"></asp:Label><br />
                    <asp:Label ID="texto_modal" class="texto_modal" runat="server" Text="texto_modal"></asp:Label>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>

