<%@ Page Language="C#" MasterPageFile="~/Page.master" AutoEventWireup="true" CodeFile="ConsultaCitas.aspx.cs"
    Inherits="Servicios_Afiliados_ConsultaCitas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="content" runat="Server">

    <script type="text/javascript">
    $(function () {
        // Cantidad de resultados mostrados como máximo en autocomplete
        const RESULTADOS_MOSTRADOS_AUTOCOMPLETE = 10;
        
        // Número de caracteres mínimos a escribir para empezar a mostrar resultados
        const MIN_LENGTH_AUTOCOMPLETE = 3;
    
        // AJAX que obtiene todas las sedes y se las asigna a autocomplete
        $.ajax({
            url: '<%=ResolveUrl("~/Servicios/Afiliados/ConsultaCitas.aspx/GetSedes") %>',
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
				        response(results.slice(0, RESULTADOS_MOSTRADOS_AUTOCOMPLETE));
			        },
                    minLength: MIN_LENGTH_AUTOCOMPLETE, // Mostrará recomendaciones hasta ingresar 3 caracteres
			        focus: function(event, ui) {
				        event.preventDefault();
				        $(this).val(ui.item.label);
			        },
			        select: function(event, ui) {
				        event.preventDefault();
				        $(this).val(ui.item.label);
				        
				        // Guardara el codigo de la sede en un campo oculto
				        $("[id$=codigoSede]").val(ui.item.value);
				        console.log('select');
			        }, 
			        change: function ( event, ui ){
			            // Si al salirse del autocomplete no ingresó una unidad medica correcta, valor será 0
			            $("[id$=codigoSede]").val(ui.item == null ? 0 : ui.item.value);
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
        Consulta de citas</h1>
    <div class="nota">
        Para consultar las citas programadas en los últimos meses, por favor ingresar
        su número de afiliación y la unidad médica en donde hizo la cita.
    </div>
    <div class="info">
        <p>
            <b>IMPORTANTE:</b><br />
            Los datos mostrados en esta consulta solamente incluyen las citas que aún no han
            pasado y las que hayan sido hechas en la sede seleccionada.
            <br />
            Por cualquier duda o consulta sobre la información presentada, por favor comunicarse
            a Oficinas Centrales del IGSS, al PBX 2412-1224 ext 83118.</p>
    </div>
    <asp:Panel ID="pnlBusqueda" GroupingText="Buscar citas" runat="server">
        <asp:Label ID="lblMensajeError" runat="server" CssClass="notarojo" ForeColor="red"></asp:Label>
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblNumIdentificacion" runat="server" Text="Número de afiliación: "></asp:Label></td>
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
                    <asp:Label ID="lblSedes" runat="server" Text="Unidad médica: "></asp:Label></td>
                <asp:HiddenField ID="codigoSede" runat="server" />
                <td>
                    <asp:TextBox ID="txtSede" runat="server" MaxLength="12"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="reqvSede" runat="server" ControlToValidate="txtSede"
                        ErrorMessage="Dato requerido" Display="Dynamic"></asp:RequiredFieldValidator>
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
    <asp:Panel ID="pnlCitas" runat="server" GroupingText="Citas del afiliado" Visible="false">
        <div style="padding-bottom: 7px">
            <asp:Label ID="lbltxtNomAfiliado" runat="server" Text="Nombre del afiliado:" Font-Size="Small"></asp:Label>
            <asp:Label ID="lblNombreAfiliado" runat="server" ForeColor="#003399" Font-Bold="true"
                Font-Size="Small"></asp:Label>
        </div>
        <asp:GridView ID="gridCitas" runat="server" AutoGenerateColumns="false" EmptyDataText="No se encontraron citas para los siguientes meses">
            <Columns>
                <asp:BoundField HeaderText="Fecha" DataField="FECHA" />
                <asp:BoundField HeaderText="Unidad" DataField="UNIDAD" />
                <asp:BoundField HeaderText="Paciente" DataField="PACIENTE" Visible="false" />
                <asp:BoundField HeaderText="Clínica" DataField="CLINICA" />
            </Columns>
        </asp:GridView>
        <asp:Label ID="lblSinResultados" runat="server" ForeColor="#003399" Font-Bold="true" Font-Size="Small" Visible="false">
        </asp:Label>
    </asp:Panel>
</asp:Content>
