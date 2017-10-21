<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Login.ascx.cs" Inherits="coms_Login" %>
<div class="loginHome" style="width:200px;"><div class="tituloSE">Servicios Electrónicos</div><div class="cSE">
<% if (!Page.User.Identity.IsAuthenticated)
   { %>
<script type="text/javascript">
function validate() 
{
            var user = document.getElementById("<%= this.un.ClientID %>");
            var val = user.value;
            if (val.toLowerCase() != val) {
                alert('No se permite el uso de Mayusculas para el Nombre de usuario');
                user.value = "";
		setTimeout(function() { document.getElementById("<%= this.un.ClientID %>").focus(); }, 10);
            }
        }
</script>
<!--Para ingresar al sistema de consultas en linea debe ingresar su usuario y clave y hacer click en Ingresar-->
<div class="text" style="width:50px;"><asp:Label ID="unl" runat="server" AssociatedControlID="un">Usuario:</asp:Label></div>
<asp:TextBox ID="un" runat="server" CssClass="tb"></asp:TextBox>
<asp:RequiredFieldValidator ID="unr" runat="server" ControlToValidate="un" ErrorMessage="Ingrese el usuario." ToolTip="Ingrese el usuario." ValidationGroup="Login">*</asp:RequiredFieldValidator><br />
<div class="text" style="width:50px;"><asp:Label ID="pl" runat="server" AssociatedControlID="p">Clave:</asp:Label></div>
<asp:TextBox CssClass="tb" ID="p" runat="server" TextMode="Password"></asp:TextBox>
<asp:RequiredFieldValidator ID="pr" runat="server" ControlToValidate="p" ErrorMessage="Ingrese la clave." ToolTip="Ingrese la clave." ValidationGroup="Login">*</asp:RequiredFieldValidator><br />
<asp:Literal ID="ft" runat="server" EnableViewState="False"></asp:Literal>
<asp:Button ID="lb" runat="server" CommandName="Login" Text="Ingresar" ValidationGroup="Login" OnClick="lb_Click" /> 
    <a href="<%= Page.ResolveClientUrl("~/NuevaClave.aspx") %>">Olvide mi clave</a><br />
<asp:CheckBox ID="rm" runat="server" Text="Recuerdeme" />
    <asp:Label ID="Msg" runat="server" ForeColor="red" EnableViewState="false"></asp:Label>
    <%} else { %> Usted ya esta autenticado como "<%= Page.User.Identity.Name %>" en el sistema, puede utilizar nuestros <a href="<%= Page.ResolveUrl(ConfigurationManager.AppSettings["appOnline"]) %>">Servicios Electrónicos</a>.<br /><br />También puede <a href="<%= Page.ResolveUrl(ConfigurationManager.AppSettings["appLogout"]) %>">cerrar su sesión</a>. <% } %>
    </div></div>