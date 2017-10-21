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

public partial class coms_Links : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    private static readonly string[] cssClasses = new string[]{ "", "left", "right", "left right"};
    protected override void Render(HtmlTextWriter writer)
    {
        List<SiteMapNode> nodes = new List<SiteMapNode>();
        SiteMapNode root = SiteMap.RootNode;
        SiteMapNode currentNode = SiteMap.CurrentNode;
        while (currentNode!=null)
        {
            if (currentNode!=null)
            {
                string subroot = SiteMap.CurrentNode["subroot"];
                if (!string.IsNullOrEmpty(subroot))
                {
                    bool isSubRoot = subroot.ToLower() == "true";
                    if (isSubRoot)
                    {
                        root = SiteMap.CurrentNode;
                        break;
                    }
                }
            }
            if (currentNode.ParentNode != root)
            {
                currentNode = currentNode.ParentNode;
            }
            else
                break;
        } 
        nodes.Add(root);
        foreach (SiteMapNode smn in root.ChildNodes)
        {
            string visible = smn["visible"];
            bool esVisible = string.IsNullOrEmpty(visible) ? true : visible.ToLower() == "true";
            if (!esVisible)
                continue;
            string seccion = smn["seccion"];
            string pie = smn["pie"];
            string ilink = smn["ilink"];
            bool esSeccion = string.IsNullOrEmpty(seccion) ? false : seccion.ToLower() == "true";
            bool esPie = string.IsNullOrEmpty(pie) ? false : pie.ToLower() == "true";
            bool iLink = string.IsNullOrEmpty(ilink) ? false : ilink.ToLower() == "true";
            if (!iLink && ( esSeccion || esPie))
                continue;
            nodes.Add(smn);
        }
        int c = 0;
        writer.Write("<div id=\"ilinks\">");
        foreach (SiteMapNode smn in nodes)
        {
            c++;
            bool right = c < nodes.Count;
            bool left = c > 1;
            string cssClass = cssClasses[(left ? 1 : 0) + (right ? 2 : 0)];
            writer.Write(string.Format("<div class=\"{2}\"><a href=\"{1}\">{0}</a></div>", smn.Title, Page.ResolveUrl(smn.Url), cssClass));
        }
        writer.Write("</div>");
    }
}
