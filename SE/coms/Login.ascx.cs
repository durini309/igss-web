using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class coms_Login : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
	this.un.Attributes.Add("onchange", "validate();");
    }
    protected void lb_Click(object sender, EventArgs e)
    {
        bool passwordExpire = false;
        int passwordExpireDays = 30;
        Page.Validate("Login");
        if (Membership.ValidateUser(this.un.Text, this.p.Text))
        {
            Session["CAut"] = this.p.Text;
            MembershipUser mu = Membership.GetUser(this.un.Text, false);
            if (mu != null)
            {
                if (mu.Email == null || mu.PasswordQuestion == null)
                {
                    Session["completarInscripcion"] = mu.UserName;
                    Response.Redirect("~/CompletarInscripcion.aspx", true);
                    return;
                }
                bool mustChangePassword = false;
                if (!mustChangePassword && passwordExpire)
                {
                    TimeSpan ts = DateTime.Now - mu.LastPasswordChangedDate;
                    if (passwordExpireDays <= ts.Days)
                        mustChangePassword = true;
                }
                if (mustChangePassword)
                {
                    Session["cambioRequerido"] = mu.UserName;
                    Response.Redirect("~/CambiarClave.aspx", true);
                    return;
                }
            }
            FormsAuthentication.SetAuthCookie(this.un.Text, this.rm.Checked);
            string appOnlineUrl = ConfigurationManager.AppSettings["appOnline"];
            string url = null;
            if (string.IsNullOrEmpty(_urlServicios))
                url = appOnlineUrl;
            else
                url = _urlServicios;
            if (!string.IsNullOrEmpty(url))
                Response.Redirect(url,true);
        }
        else
            Msg.Text = "Usuario o clave incorrecta";
    }
    private string _urlServicios;

    public string UrlServicios
    {
        get { return _urlServicios; }
        set { _urlServicios = value; }
    }

}
