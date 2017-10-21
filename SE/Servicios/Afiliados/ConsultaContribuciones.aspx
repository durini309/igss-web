<%@ Page Language="C#" MasterPageFile="~/Page.master" AutoEventWireup="true" CodeFile="ConsultaContribuciones.aspx.cs" Inherits="Servicios_Afiliados_ConsultaContribuciones" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="content" Runat="Server">
<script type="text/javascript">
    function VerOrdenCed(obj)
    {
        var objOrdenCed = document.getElementById("ctl00_content_divOrdenCed");
        
        if (obj.value == "CE")
            objOrdenCed.style.display = "inline";
        else
            objOrdenCed.style.display = "none";
    }
</script>

<h1>Consulta de contribuciones</h1>
<div class="nota">
Para consultar las aportaciones realizadas por su patrono en los últimos seis meses, ingrese su número de afiliado y su documento de 
identificación registrado en el Instituto.
</div>
<div class="info">
    <p><b>IMPORTANTE:</b><br />
        Los datos mostrados en esta consulta solamente incluyen los provenientes de la Planilla Electrónica y de los 
        pagos realizados por el patrono en los últimos seis meses. <br />
        Por cualquier duda o consulta sobre la información presentada, por favor comunicarse a Oficinas Centrales del IGSS, 
        al PBX 2412-1111.</p>
</div>
<asp:Panel ID="pnlAfiliado" GroupingText="Buscar patronos" runat="server">
    <asp:Label ID="lblMensajeError" runat="server" CssClass="notarojo" ForeColor="red"></asp:Label>
    <table>
        <tr>
            <td><asp:Label ID="lblNumAfiliado" runat="server" Text="Número de afiliado: "></asp:Label></td>
            <td><asp:TextBox ID="txtNumAfiliado" runat="server" MaxLength="12"></asp:TextBox>
                <asp:RegularExpressionValidator ID="revNumAfiliado" runat="server" ControlToValidate="txtNumAfiliado" ValidationExpression="^[0-9]+$" ErrorMessage="Dato inválido" Display="Dynamic"></asp:RegularExpressionValidator>
                <asp:RequiredFieldValidator ID="reqvNumAfiliado" runat="server" ControlToValidate="txtNumAfiliado" ErrorMessage="Dato requerido" Display="Dynamic"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td><asp:Label ID="lblTipoDoc" runat="server" Text="Documento de identificación:"></asp:Label></td>
            <td>
                <asp:DropDownList ID="ddlDocumentos" runat="server">
                    <asp:ListItem Text="DPI" Value="DPI"></asp:ListItem>
                    <asp:ListItem Text="Cédula" Value="CE"></asp:ListItem>
                    <asp:ListItem Text="Pasaporte" Value="P"></asp:ListItem>
                </asp:DropDownList>
                <div id="divOrdenCed" runat="server" style="display:none">
                <asp:DropDownList ID="ddlOrdenCedula" runat="server">
                    <asp:ListItem Text="A01" Value="A01"></asp:ListItem>
                    <asp:ListItem Text="B02" Value="B02"></asp:ListItem>
                    <asp:ListItem Text="C03" Value="C03"></asp:ListItem>
                    <asp:ListItem Text="D04" Value="D04"></asp:ListItem>
                    <asp:ListItem Text="E05" Value="E05"></asp:ListItem>
                    <asp:ListItem Text="F06" Value="F06"></asp:ListItem>
                    <asp:ListItem Text="G07" Value="G07"></asp:ListItem>
                    <asp:ListItem Text="H08" Value="H08"></asp:ListItem>
                    <asp:ListItem Text="I09" Value="I09"></asp:ListItem>
                    <asp:ListItem Text="J10" Value="J10"></asp:ListItem>
                    <asp:ListItem Text="K11" Value="K11"></asp:ListItem>
                    <asp:ListItem Text="L12" Value="L12"></asp:ListItem>
                    <asp:ListItem Text="M13" Value="M13"></asp:ListItem>
                    <asp:ListItem Text="N14" Value="N14"></asp:ListItem>
                    <asp:ListItem Text="Ñ15" Value="Ñ15"></asp:ListItem>
                    <asp:ListItem Text="O16" Value="O16"></asp:ListItem>
                    <asp:ListItem Text="P17" Value="P17"></asp:ListItem>
                    <asp:ListItem Text="Q18" Value="Q18"></asp:ListItem>
                    <asp:ListItem Text="R19" Value="R19"></asp:ListItem>
                    <asp:ListItem Text="S20" Value="S20"></asp:ListItem>
                    <asp:ListItem Text="T21" Value="T21"></asp:ListItem>
                    <asp:ListItem Text="U22" Value="U22"></asp:ListItem>
                </asp:DropDownList>
                </div>
                <asp:TextBox ID="txtDocumento" runat="server" MaxLength="13"></asp:TextBox>
                <asp:RequiredFieldValidator ID="reqvDocumento" runat="server" ControlToValidate="txtDocumento" ErrorMessage="Dato requerido" Display="Dynamic"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td colspan="2"><asp:Button ID="btnConsultar" runat="server" Text="Consultar" OnClick="btnConsultar_Click"  />
                <asp:Button ID="btnLimpiar" runat="server" Text="Realizar otra consulta" OnClick="btnLimpiar_Click" CausesValidation="false" />
            </td>
        </tr>
    </table>
    <asp:ValidationSummary ID="vs" runat="server" ShowSummary="false" ShowMessageBox="false"  />
</asp:Panel>
<asp:Panel ID="pnlPatronos" runat="server" GroupingText="Patronos del afiliado" Visible="false">
    <div style="padding-bottom:7px">
        <asp:Label ID="lbltxtNomAfiliado" runat="server" Text="Nombre del afiliado:" Font-Size="Small"></asp:Label>
        <asp:Label ID="lblNombreAfiliado" runat="server" ForeColor="#003399" Font-Bold="true" Font-Size="Small"></asp:Label>
    </div>
    <asp:GridView ID="gridPatronos" runat="server" AutoGenerateColumns="false" OnRowCommand="gridPatronos_RowCommand"
        EmptyDataText="No se encontraron patronos con aportes mediante planilla electrónica en los últimos seis meses">
        <Columns>
            <asp:BoundField HeaderText="Número patronal" DataField="NUMERO_PATRONAL" />
            <asp:BoundField HeaderText="Patrono" DataField="PATRONO" />
            <asp:ButtonField Text="Detalle" ButtonType="Link" CommandName="Detalle" />
        </Columns>
    </asp:GridView>
</asp:Panel>
<asp:Panel ID="pnlDetallePatrono" runat="server" GroupingText="Detalle de aportes" Visible="false">
    <div style="padding-bottom:7px">
        <asp:Label ID="lblNombrePatrono" runat="server" ForeColor="#003399" Font-Bold="true" Font-Size="Small"></asp:Label>
    </div>
    <asp:GridView ID="gridDetalle" runat="server" AutoGenerateColumns="false" PageSize="6" AllowPaging="false" EmptyDataText="No se encontraron aportes del patrono en Planilla Electrónica" OnRowCreated="gridDetalle_RowCreated">
        <Columns>
            <asp:BoundField HeaderText="Mes" DataField="Mes_planilla" />
            <asp:BoundField HeaderText="Año" DataField="Ano_planilla" />
            <asp:BoundField HeaderText="Aporte afiliado" DataField="Aporte" />
        </Columns>
    </asp:GridView>
</asp:Panel>
</asp:Content>

