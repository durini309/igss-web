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
using System.Collections.Generic;

public partial class coms_LeftMenu : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected override void Render(HtmlTextWriter writer)
    {
        writer.Write("<div id=\"menu\"><h6>Secciones del Sitio</h6><div id=\"options\">");
        List<SiteMapNode> nodes = new List<SiteMapNode>();
        SiteMapNode root = SiteMap.RootNode;
        nodes.Add(root);
        foreach (SiteMapNode smn in root.ChildNodes)
        {
            string visible = smn["visible"];
            bool esVisible = string.IsNullOrEmpty(visible) ? true : visible.ToLower() == "true";
            if (!esVisible)
                continue;
            string seccion = smn["seccion"];
            bool esSeccion = string.IsNullOrEmpty(seccion) ? false : seccion.ToLower() == "true";
            if (!esSeccion)
                continue;
            nodes.Add(smn);
        }
        SiteMapNode last = nodes.Count == 0 ? null : nodes[nodes.Count - 1];
        foreach (SiteMapNode smn in nodes)
        {
            bool bold = false;
            string sbold = smn["bold"];
            if (!string.IsNullOrEmpty(sbold) && sbold.ToLower().Equals("true"))
                bold = true;
            //if (smn == last)
            //    writer.Write(string.Format("<div class=\"last\"><a href=\"{1}\">{0}</a></div>", smn.Title, Page.ResolveUrl(smn.Url)));
            //else
            //    writer.Write(string.Format("<div><a href=\"{1}\">{0}</a></div>", smn.Title, Page.ResolveUrl(smn.Url)));
            writer.Write("<div");
            if (smn == last)
                writer.Write(" class=\"last\"");
            writer.Write(">");
            if (bold)
                writer.Write("<b>");
            writer.Write(string.Format("<a href=\"{1}\">{0}</a>", smn.Title, Page.ResolveUrl(smn.Url)));
            if (bold)
                writer.Write("</b>");
            writer.Write("</div>");
        }
        writer.Write("</div></div>");
    }
}
