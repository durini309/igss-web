<%@ Page Language="C#" Theme="CoExSC" MasterPageFile="~/Servicios/Afiliados/MasterCita.master" AutoEventWireup="true" CodeFile="CitaEnLinea.aspx.cs" Inherits="Servicios_Afiliados_CitaEnLinea" Title="Cita en l�nea" %>
<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="content" Runat="Server">
<script type="text/javascript">

$(function () {
$.datepicker.setDefaults($.datepicker.regional["es"]);
$("#ctl00_content_txtFechaSelec").datepicker({
firstDay: 0,
minDate: 1,
maxDate: 365,
dateFormat: "dd/mm/yy",
dayNamesMin: [ "Do", "Lu", "Ma", "Mi", "Ju", "Vi", "Sa" ],
monthNames: [ "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" ],
});
});

</script>
<h1>Reservaci�n de cita m�dica en l�nea</h1>
<div class="nota">
    Esta herramienta permite a los afiliados activos reservar una cita para atenci�n m�dica en su unidad de adscripci�n.
    Para ello debe contar con un Certificado de Trabajo Electr�nico vigente.
<br />    IMPORTANTE:
<br />    - La reservaci�n de citas no est� habilitada para todas las unidades m�dicas del Instituto.
Al ingresar sus datos se le indicar� si su unidad de adscripci�n est� habilitada.
<br />    - Puede seleccionar una cita para un beneficiario hijo y esposa, si as� est� indicado en el Certificado de Trabajo Electr�nico.
</div>
<asp:Panel ID="pnlAfiliado" GroupingText="Validar acreditaci�n" runat="server" CssClass="grupoDatos"> 
    Ingrese el N�mero de afiliado y Certificado de Trabajo Electr�nico
    <table width="100%">
        <tr>
            <td style="width:400px">
                <span class="etiqueta2">N�mero de afiliado: </span>

                    <asp:TextBox ID="txtNumAfiliado" runat="server" MaxLength="12"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="revNumAfiliado" runat="server" ControlToValidate="txtNumAfiliado" ValidationExpression="^[0-9]+$" ErrorMessage="Dato inv�lido" Display="Dynamic"></asp:RegularExpressionValidator>
                        <asp:RequiredFieldValidator ID="reqvNumAfiliado" runat="server" ControlToValidate="txtNumAfiliado" ErrorMessage="Dato requerido" Display="Dynamic"></asp:RequiredFieldValidator>
                    <br />
                    <span class="etiqueta2">N�mero de Certificado Electr�nico: </span>
                    
                        <asp:TextBox ID="txtCTE" runat="server" MaxLength="12"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="revCTE" runat="server" ControlToValidate="txtCTE" ValidationExpression="^[0-9]+$" ErrorMessage="Dato inv�lido" Display="Dynamic"></asp:RegularExpressionValidator>
                        <asp:RequiredFieldValidator ID="rvCTE" runat="server" ControlToValidate="txtCTE" ErrorMessage="Dato requerido" Display="Dynamic"></asp:RequiredFieldValidator>
            
            </td>
            <td align="left" style="vertical-align:middle;">
                <asp:Button ID="btnConsultar" runat="server" Text="Validar datos" OnClick="btnConsultar_Click"/>&nbsp;&nbsp;
                <asp:Button ID="btnReimprimir" runat="server" Text="Reimprimir constancia" OnClick="btnReimprimir_Click" />
            </td>
        </tr>
    </table>
    <asp:ValidationSummary ID="vs" runat="server" ShowSummary="false" ShowMessageBox="false"  />
    <p>Si desea volver a imprimir su constancia de reservaci�n de cita, ingrese el n�mero de afiliaci�n y n�mero de CTE, y presione el bot�n Reimprimir constancia</p>
</asp:Panel>
<div style="padding-left:5px; padding-bottom:5px;">
    <asp:Label ID="lblMensajeError" runat="server" CssClass="notarojo" ForeColor="red"></asp:Label><br />
    <asp:Button ID="btnNuevaConsulta" runat="server" Text="Regresar" CausesValidation="false" OnClick="btnNuevaConsulta_Click" />
</div>
<asp:Panel ID="pnlDatosGenerales" runat="server" Visible="false" GroupingText="Datos de la cita" CssClass="grupoDatos">
    <table class="tablaDatos" >
        <tr>
            <td class="celdaNombreCampo2" style="width:130px">N�mero de CTE</td>
            <td><asp:Label ID="txtNumCTE" runat="server" Font-Bold="true"></asp:Label></td>
            <td class="celdaNombreCampo2" style="width:140px">Unidad de adscripci�n:</td>
            <td><asp:Label ID="txtUnidadAdscripcion" runat="server" Font-Bold="true"></asp:Label></td>                        
        </tr>
        <tr>
            <td class="celdaNombreCampo2">N�mero de afiliado</td>
            <td><asp:Label ID="txtNumAfi" runat="server" Font-Bold="true"></asp:Label></td>
            <td class="celdaNombreCampo2">Especialidad</td>
            <td><asp:DropDownList ID="ddlEspecialidad" runat="server" DataTextField="nombre_especialidad" DataValueField="cod_especialidad" >
                </asp:DropDownList>
            </td>                        
        </tr>
        <tr>
            <td class="celdaNombreCampo2">Nombre del afiliado:</td>
            <td><asp:Label ID="txtNombreAfiliado" runat="server" Font-Bold="true"></asp:Label></td>
            <td class="celdaNombreCampo2">Fecha en que desea ser atendido</td>
            <td>
                <asp:TextBox ID="txtFechaSelec" runat="server"></asp:TextBox>
                
                <asp:RangeValidator ID="rvFecha" runat="server" ErrorMessage="Fecha inv�lida" ControlToValidate="txtFechaSelec" Type="Date" MaximumValue="31/12/2100"></asp:RangeValidator>
                <asp:Button ID="btnValidar" runat="server" Text="Validar fecha" OnClick="btnValidar_Click"  />
            </td>                        
        </tr>
        <tr>
            <td class="celdaNombreCampo2">Nombre del paciente:</td>
            <td><asp:Label ID="txtNombrePaciente" runat="server" Font-Bold="true"></asp:Label></td>
            <td class="celdaNombreCampo2">Cl�nica y jornada asignadas</td>
            <td><asp:Label ID="txtClinicaJornada" runat="server" Font-Bold="true"></asp:Label></td>
        </tr>
    </table>
    <div id="divValidar" style="display:none">
                <asp:Label ID="txtFechaCitaSeleccionada" runat="server" CssClass="titulo"></asp:Label>
            <asp:Button ID="btnConfirmarCita" runat="server" Text="Confirmar" OnClick="btnConfirmarCita_Click" CausesValidation="false" />
    </div>

</asp:Panel>

    <asp:Panel ID="pnlConfirmada" runat="server" Visible="false" CssClass="info">
        <p>La cita ha sido confirmada. Puede descargar e imprimir la constancia con los datos de la cita y presentarla en la unidad el d�a de la cita.</p>
            <asp:Button ID="btnConstancia" runat="server" Text="Imprimir constancia" OnClick="btnConstancia_Click" />
    </asp:Panel>    

<div id="pnlDoc" runat="server" style="display:none">
    <fieldset title="Constancia " class="grupoDatos">
        <iframe style="WIDTH: 650px; HEIGHT: 420px;" id="iframe_reportes" runat="server" frameborder="0" scrolling="no"></iframe>
    </fieldset>
</div>
<div class="loader"></div>
</asp:Content>

