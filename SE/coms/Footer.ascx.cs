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
using System.Security.Cryptography;
using System.Text;

public partial class coms_Footer : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {        
        contactenos.NavigateUrl = String.Format("{0}{1}", Page.ResolveUrl("~/Contactenos.aspx"), String.IsNullOrEmpty(ConfigurationManager.AppSettings["EmailSoporte"]) ? "" : String.Format("?EmailSoporte={0}", EncryptAndHash(ConfigurationManager.AppSettings["EmailSoporte"])));
    }
    private const string KEY = "igsssoporte";

    public string EncryptAndHash(string value)
    {
        MACTripleDES des = new MACTripleDES();
        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        des.Key = md5.ComputeHash(Encoding.UTF8.GetBytes(KEY));
        string encrypted = Convert.ToBase64String(des.ComputeHash(Encoding.UTF8.GetBytes(value))) + '-' + Convert.ToBase64String(Encoding.UTF8.GetBytes(value));
        return HttpUtility.UrlEncode(encrypted);
    }   
    //protected override void Render(HtmlTextWriter writer)
    //{
    //    writer.Write("<div id=\"footer\">");
    //    List<SiteMapNode> nodes = new List<SiteMapNode>();
    //    SiteMapNode root = SiteMap.RootNode;
    //    nodes.Add(root);
    //    foreach (SiteMapNode smn in root.ChildNodes)
    //    {
    //        string visible = smn["visible"];
    //        bool esVisible = string.IsNullOrEmpty(visible) ? true : visible.ToLower() == "true";
    //        if (!esVisible)
    //            continue;
    //        string seccion = smn["seccion"];
    //        string pie = smn["pie"];
    //        bool esSeccion = string.IsNullOrEmpty(seccion) ? false : seccion.ToLower() == "true";
    //        bool esPie = string.IsNullOrEmpty(pie) ? false : pie.ToLower() == "true";
    //        if (esSeccion && !esPie) 
    //            continue;
    //        nodes.Add(smn);
    //    }
    //    bool first = true;
    //    foreach (SiteMapNode smn in nodes)
    //    {
    //        if (first)
    //            first = false;
    //        else
    //            writer.Write(" | ");
    //        writer.Write(string.Format("<a href=\"{1}\">{0}</a>", smn.Title, Page.ResolveUrl(smn.Url)));
    //    }
    //    writer.Write("</div>");
    //}
    
}
