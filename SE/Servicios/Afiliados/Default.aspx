<%@ Page Language="C#" MasterPageFile="~/Page.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Servicios_Afiliados_Default" Title="Untitled Page" %>
<%@ Register Src="../../coms/LinksSeguros.ascx" TagName="LinksSeguros" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="content" Runat="Server">
    <h1>
        Servicios Electrónicos para Afiliados</h1>
    <uc1:LinksSeguros ID="LinksSeguros1" runat="server" Description="Usted puede realizar las siguientes opciones:"
        NoLinksMessage="No existen opciones" />
</asp:Content>

