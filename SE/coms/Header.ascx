<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Header.ascx.cs" Inherits="coms_Header" %>

<script language="javascript" type="text/javascript">
// <!CDATA[

// ]]>
</script>

<div id="header">
    <div id="date">
        <img src="<%= Page.ResolveUrl("~/img/newHeaderLeft.jpg") %>" alt="Instituto Guatemalteco de Seguridad Social" />
    </div>    
    <div id="logoMural">      
        <div id="headLinks">
            <%=LinksHTML %>
        </div>  
        <img src="<%= Page.ResolveUrl("~/img/newHeaderRigth.jpg") %>" alt="colita" id="IMG1"
            onclick="return IMG1_onclick()" />        
    </div>
    
</div>
