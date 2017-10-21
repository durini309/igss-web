<%@ Page Language="C#" MasterPageFile="~/Page.master" AutoEventWireup="true" CodeFile="ConsultaCitas.aspx.cs" Inherits="Servicios_Afiliados_ConsultaCitas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="content" Runat="Server">
<script type="text/javascript">
$(document).ready(function() {
        $.ajax({  
            type: "POST",  
            url: "ConsultaCitas.aspx/obtieneSedes",   
            dataType: "json",  
            contentType: "application/json",  
            success: function(data) {  
                console.log(data);
            },  
            error: function(XMLHttpRequest, textStatus, errorThrown) {  
                console.log(XMLHttpRequest);  
            }  
        });
}); 
</script>

<h1>Consulta de citas</h1>
<div class="nota">
Para consultar las citas que ha realizado en los ùltimos meses, por favor ingresar su número 
de DPI o de Afiliado y la sede en donde hizo la cita.
</div>
<div class="info">
    <p><b>IMPORTANTE:</b><br />
        Los datos mostrados en esta consulta solamente incluyen las citas que aún no han pasado y las que hayan sido hechas
        en la sede seleccionada. <br />
        Por cualquier duda o consulta sobre la información presentada, por favor comunicarse a Oficinas Centrales del IGSS, 
        al PBX 2412-1111.</p>
</div>
<asp:Panel ID="pnlBusqueda" GroupingText="Buscar citas" runat="server">
    <asp:Label ID="lblMensajeError" runat="server" CssClass="notarojo" ForeColor="red"></asp:Label>
    <table>
        <tr>
            <td><asp:Label ID="lblNumIdentificacion" runat="server" Text="Número de identificación: "></asp:Label></td>
            <td><asp:TextBox ID="txtNumAfiliado" runat="server" MaxLength="12"></asp:TextBox>
                <asp:RegularExpressionValidator ID="revNumAfiliado" runat="server" ControlToValidate="txtNumAfiliado" ValidationExpression="^[0-9]+$" ErrorMessage="Dato inválido" Display="Dynamic"></asp:RegularExpressionValidator>
                <asp:RequiredFieldValidator ID="reqvNumAfiliado" runat="server" ControlToValidate="txtNumAfiliado" ErrorMessage="Dato requerido" Display="Dynamic"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td><asp:Label ID="lblSedes" runat="server" Text="Sede:"></asp:Label></td>
            <td>
                <asp:DropDownList ID="ddlSedes" runat="server" Width="173px">
                    <asp:ListItem Text="Zona 1" Value="z1"></asp:ListItem>
                    <asp:ListItem Text="Zona 5" Value="z5"></asp:ListItem>
                    <asp:ListItem Text="Zona 6" Value="z6"></asp:ListItem>
                </asp:DropDownList>
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
<asp:Panel ID="pnlCitas" runat="server" GroupingText="Citas del afiliado" Visible="false">
    <div style="padding-bottom:7px" >
        <asp:Label ID="lbltxtNomAfiliado" runat="server" Text="Nombre del afiliado:" Font-Size="Small"></asp:Label>
        <asp:Label ID="lblNombreAfiliado" runat="server" ForeColor="#003399" Font-Bold="true" Font-Size="Small"></asp:Label>
    </div>
    <asp:GridView ID="gridCitas" runat="server" AutoGenerateColumns="false"
        EmptyDataText="No se encontraron citas para los siguientes meses">
        <Columns>
            <asp:BoundField HeaderText="Fecha" DataField="FECHA" />
            <asp:BoundField HeaderText="Unidad" DataField="UNIDAD" />
            <asp:BoundField HeaderText="Paciente" DataField="PACIENTE" Visible="false"/>
            <asp:BoundField HeaderText="Clínica" DataField="CLINICA" />
        </Columns>
    </asp:GridView>
</asp:Panel>
</asp:Content>

