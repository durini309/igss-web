﻿<%@ Page Language="C#" MasterPageFile="~/Page.master" AutoEventWireup="true" CodeFile="ConsultaFormularios.aspx.cs"
    Inherits="Servicios_Afiliados_ConsultaFormularios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="content" runat="Server">

    <script type="text/javascript">
    $(function () {
        // AJAX que obtiene todas las sedes y se las asigna a autocomplete
        $.ajax({
            url: '<%=ResolveUrl("~/Servicios/Patronos/ConsultaFormularios.aspx/GetSedes") %>',
            data: '',
            type: "POST",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var autocompleteSource = JSON.parse(data.d);
                
                // Inicializa autocomplete
                $("[id$=txtSede]").autocomplete({
                    source: function(request, response) {
				        var results = $.ui.autocomplete.filter(autocompleteSource, request.term);
				        // Retornamos las primeras 10 coincidencias
				        response(results.slice(0, 10));
			        },
                    minLength: 3, // Mostrará recomendaciones hasta ingresar 3 caracteres
			        focus: function(event, ui) {
				        event.preventDefault();
				        $(this).val(ui.item.label);
			        },
			        select: function(event, ui) {
				        event.preventDefault();
				        $(this).val(ui.item.label);
				        
				        // Guardara el codigo de la sede en un campo oculto
				        $("[id$=codigoSede]").val(ui.item.value);
			        }
                });
            },
            error: function (response) {
                console.log(response.responseText);
                alert('Error');
            },
            failure: function (response) {
                console.log(response.responseText);
                alert('Error');
            }
        });
   
    });  
    </script>

    <h1>
        Validación de Formularios</h1>
    <div class="nota">
        Para validar el formulario, por favor ingresar su número de formulario, la fecha,
        la unidad médica y seleccione el tipo de formulario a validar.
    </div>
    <div class="info">
        <p>
            <b>IMPORTANTE:</b><br />
            Los datos mostrados en esta consulta solamente validan formularios que hayan sido
            emitidos en la unidad médica seleccionada.
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
                <td style="width: 419px">
                    <asp:TextBox ID="txtNumFormulario" runat="server" MaxLength="12" Width="243px"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="revNumFormulario" runat="server" ControlToValidate="txtNumFormulario"
                        ValidationExpression="^[0-9]+$" ErrorMessage="Dato inválido" Display="Dynamic"></asp:RegularExpressionValidator>
                    <asp:RequiredFieldValidator ID="reqvNumFormulario" runat="server" ControlToValidate="txtNumFormulario"
                        ErrorMessage="Dato requerido" Display="Dynamic"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblFecha" runat="server" Text="Fecha del formulario: "></asp:Label></td>
                <td style="width: 419px">
                    <asp:Calendar ID="dtFecha" runat="server"></asp:Calendar>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblNumIdentificacion" runat="server" Text="Número de afiliación: "></asp:Label></td>
                <td style="width: 419px">
                    <asp:TextBox ID="txtNumAfiliado" runat="server" MaxLength="12" Width="241px"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="revNumAfiliado" runat="server" ControlToValidate="txtNumAfiliado"
                        ValidationExpression="^[0-9]+$" ErrorMessage="Dato inválido" Display="Dynamic"></asp:RegularExpressionValidator>
                    <asp:RequiredFieldValidator ID="reqvNumAfiliado" runat="server" ControlToValidate="txtNumAfiliado"
                        ErrorMessage="Dato requerido" Display="Dynamic"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblSedes" runat="server" Text="Unidad médica: "></asp:Label></td>
                <asp:HiddenField ID="codigoSede" runat="server" />
                <td style="width: 416px">
                    <asp:TextBox ID="txtSede" runat="server" MaxLength="255" Style="text-transform: uppercase;"
                        Width="243px" TextMode="MultiLine" Wrap="False" onmouseover='this.title=this.value'></asp:TextBox>
                    <asp:RequiredFieldValidator ID="reqvSede" runat="server" ControlToValidate="txtSede"
                        ErrorMessage="Dato requerido" Display="Dynamic"></asp:RequiredFieldValidator>
                </td>
                <%-- <td>
                    <asp:Label ID="lblSedes" runat="server" Text="Unidad Médica:"></asp:Label></td>
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
                <td style="width: 419px">
                    <asp:DropDownList ID="ddlFormulario" runat="server" Width="242px">
                        <asp:ListItem Text="SPS-60" Value="1"></asp:ListItem>
                        <asp:ListItem Text="SPS-231" Value="2"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button ID="btnConsultar" runat="server" Text="Consultar" OnClick="btnConsultar_Click" />
                    &nbsp; &nbsp;
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
