<%@ Page Language="C#" MasterPageFile="~/Page.master" AutoEventWireup="true" CodeFile="ConsultaFormularios.aspx.cs"
    Inherits="Servicios_Afiliados_ConsultaFormularios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="content" runat="Server">

    <script type="text/javascript">
$(function () {
        $("[id$=txtSede]").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: '<%=ResolveUrl("~/Servicios/Patronos/ConsultaFormularios.aspx/GetSedes") %>',
                    data: "{ 'keyword': '" + request.term + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        response($.map(data.d, function (item) {
                            return {
                                label: item.split('-')[0],
                                val: item.split('-')[1]
                            }
                        }))
                    },
                    error: function (response) {
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });
            },
            select: function (e, i) {
                $("[id$=hfCustomerId]").val(i.item.val);
            },
            minLength: 1
        });
    });  
    </script>

    <h1>
        Validación de Formularios</h1>
    <div class="nota">
        Para validar el formulario, por favor ingresar su número de formulario, el número
        de DPI o de Afiliado, la fecha, la sede o unidad y seleccione el formulario a validar.
    </div>
    <div class="info">
        <p>
            <b>IMPORTANTE:</b><br />
            Los datos mostrados en esta consulta solamente incluyen las citas que aún no han
            pasado y las que hayan sido hechas en la sede seleccionada.
            <br />
            Por cualquier duda o consulta sobre la información presentada, por favor comunicarse
            a Oficinas Centrales del IGSS, al PBX 2412-1111.</p>
    </div>
    <asp:Panel ID="pnlBusqueda" GroupingText="Información a validar" runat="server">
        <asp:Label ID="lblMensajeError" runat="server" CssClass="notarojo" ForeColor="red"></asp:Label>
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblNumFormulario" runat="server" Text="Número de formulario: "></asp:Label></td>
                <td>
                    <asp:TextBox ID="txtNumFormulario" runat="server" MaxLength="12"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="revNumFormulario" runat="server" ControlToValidate="txtNumFormulario"
                        ValidationExpression="^[0-9]+$" ErrorMessage="Dato inválido" Display="Dynamic"></asp:RegularExpressionValidator>
                    <asp:RequiredFieldValidator ID="reqvNumFormulario" runat="server" ControlToValidate="txtNumFormulario"
                        ErrorMessage="Dato requerido" Display="Dynamic"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblFecha" runat="server" Text="Fecha del formulario: "></asp:Label></td>
                <td>
                    <asp:Calendar ID="dtFecha" runat="server"></asp:Calendar>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblNumIdentificacion" runat="server" Text="Número de identificación del afiliado: "></asp:Label></td>
                <td>
                    <asp:TextBox ID="txtNumAfiliado" runat="server" MaxLength="12"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="revNumAfiliado" runat="server" ControlToValidate="txtNumAfiliado"
                        ValidationExpression="^[0-9]+$" ErrorMessage="Dato inválido" Display="Dynamic"></asp:RegularExpressionValidator>
                    <asp:RequiredFieldValidator ID="reqvNumAfiliado" runat="server" ControlToValidate="txtNumAfiliado"
                        ErrorMessage="Dato requerido" Display="Dynamic"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblSedes" runat="server" Text="Sede: "></asp:Label></td>
                <asp:HiddenField ID="hfCustomerId" runat="server" />
                <td>
                    <asp:TextBox ID="txtSede" runat="server" MaxLength="12"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="revSede" runat="server" ControlToValidate="txtNumAfiliado"
                        ValidationExpression="^[0-9]+$" ErrorMessage="Dato inválido" Display="Dynamic"></asp:RegularExpressionValidator>
                    <asp:RequiredFieldValidator ID="reqvSede" runat="server" ControlToValidate="txtNumAfiliado"
                        ErrorMessage="Dato requerido" Display="Dynamic"></asp:RequiredFieldValidator>
                </td>
                <%-- <td>
                    <asp:Label ID="lblSedes" runat="server" Text="Sede:"></asp:Label></td>
                <td>
                    <asp:DropDownList ID="ddlSedes" runat="server" Width="173px">
                        <asp:ListItem Text="Zona 1" Value="z1"></asp:ListItem>
                        <asp:ListItem Text="Zona 5" Value="z5"></asp:ListItem>
                        <asp:ListItem Text="Zona 6" Value="z6"></asp:ListItem>
                    </asp:DropDownList>
                </td>--%>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblFormularios" runat="server" Text="Tipo de formulario:"></asp:Label></td>
                <td>
                    <asp:DropDownList ID="ddlFormulario" runat="server" Width="173px">
                        <asp:ListItem Text="SPS-60" Value="60"></asp:ListItem>
                        <asp:ListItem Text="SPS-231" Value="231"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button ID="btnConsultar" runat="server" Text="Consultar" OnClick="btnConsultar_Click" />
                    <asp:Button ID="btnLimpiar" runat="server" Text="Realizar otra consulta" OnClick="btnLimpiar_Click"
                        CausesValidation="false" />
                </td>
            </tr>
        </table>
        <asp:ValidationSummary ID="vs" runat="server" ShowSummary="false" ShowMessageBox="false" />
    </asp:Panel>
    <asp:Panel ID="pnlFormulario" runat="server" GroupingText="Resultado validación del formulario del afiliado"
        Visible="false">
        <asp:GridView ID="gridFormulario" runat="server" AutoGenerateColumns="false" EmptyDataText="No se se obtuvo respuesta para validar el formulario del afiliado.">
            <Columns>
                <asp:BoundField HeaderText="Resultado validación" DataField="RESULTADO" />
            </Columns>
        </asp:GridView>
    </asp:Panel>
</asp:Content>
