<%@ Page Language="C#" MasterPageFile="~/Page.master" AutoEventWireup="true" CodeFile="Default.aspx.cs"
    Inherits="Servicios_Default" Title="Untitled Page" %>

<%@ Register Src="../coms/LinksSeguros.ascx" TagName="LinksSeguros" TagPrefix="uc1" %>
<%@ Register Src="../coms/MasInformacion.ascx" TagName="MasInformacion" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="content" runat="Server">
    <h1>
        Servicios Electrónicos</h1>
    <uc1:LinksSeguros ID="LinksSeguros1" runat="server" Description="Usted puede realizar las siguientes categorias de servicios:"
        NoLinksMessage="No existen opciones" />
</asp:Content>
