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
using System.Text;
using System.Collections.Generic;

public partial class coms_Header : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }
    private StringBuilder sb = new StringBuilder();
    protected string LinksHTML
    {
        get { return this.sb.ToString(); }
    }
    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        List<SiteMapNode> nodes = new List<SiteMapNode>();
        SiteMapNode root = SiteMap.RootNode;
        nodes.Add(root);
        foreach (SiteMapNode smn in root.ChildNodes)
        {
            string top = smn["top"];
            bool esTop = string.IsNullOrEmpty(top) ? false : top.ToLower() == "true";
            if (esTop) 
                nodes.Add(smn);
        }
        bool first = true;
        foreach (SiteMapNode smn in nodes)
        {
            if (first)
                first = false;
            else
                sb.Append(" | ");
            sb.AppendFormat("<a href=\"{1}\">{0}</a>", smn.Title, Page.ResolveUrl(smn.Url));
        }
        if (Page.User.Identity.IsAuthenticated)
        {
            sb.AppendFormat(" | {1} <a href=\"{0}\">Cerrar Sesión</a>",Page.ResolveUrl("~/Logout.aspx"),Page.User.Identity.Name);
        }
        else
        {
            sb.AppendFormat(" | <a href=\"{0}\">Autenticarse</a>",FormsAuthentication.LoginUrl);
        }
    }
}
