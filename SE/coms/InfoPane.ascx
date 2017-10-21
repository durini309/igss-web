<%@ Control Language="C#" AutoEventWireup="true" CodeFile="InfoPane.ascx.cs" Inherits="coms_InfoPane" %>
<%--<div id="Div1" class="logo"><img alt="Logo IGSS" src="<%=LogoUrl %>" /></div>--%><div class="search" id="sarea" runat="server">Buscar: <input type="text" /><input type="button" class="goButton" value="Go" /></div>
        <div class="bannerarea" runat="server" id="barea"><div class="banner">
        <object classid="clsid:d27cdb6e-ae6d-11cf-96b8-444553540000" codebase="http://fpdownload.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=7,0,0,0" width="510" height="227" id="Banner" align="middle">
        <param name="allowScriptAccess" value="sameDomain" />
        <param name="movie" value="<%=BannerUrl %>" />
        <param name="quality" value="high" />
        <param name="bgcolor" value="#D97904" />
        <param name="wmode" value="transparent" />
        <embed src="<%=BannerUrl %>" quality="high" bgcolor="#d3e0ff" width="510" height="227" name="Banner" align="middle" allowScriptAccess="sameDomain" type="application/x-shockwave-flash" pluginspage="http://www.macromedia.com/go/getflashplayer" />
        </object>
        </div></div>
