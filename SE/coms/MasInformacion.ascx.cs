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
using System.ComponentModel;

public partial class coms_MasInformacion : System.Web.UI.UserControl
{
    public enum Formato { UnorderedList, NumberedList, ImageAndText, Image };
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    private string _titulo;

    [Category("Appearance")]
    public string Titulo
    {
        get { return _titulo; }
        set { _titulo = value; }
    }

    private Formato _formato;

    [Category("Appearance")]
    public Formato Mostrar
    {
        get { return _formato; }
        set { _formato = value; }
    }

    private bool _mostrarTitulo = true;

    [Category("Appearance"), DefaultValue(true)]
    public bool MostrarTitulo
    {
        get { return _mostrarTitulo; }
        set { _mostrarTitulo = value; }
    }

    private string _descripcion;

    [Category("Appearance"), DefaultValue(null)]
    public string Descripcion
    {
        get { return _descripcion; }
        set { _descripcion = value; }
    }
    
    protected override void Render(HtmlTextWriter writer)
    {
        SiteMapProvider siteMap = SiteMap.Provider;
        if (siteMap != null)
        {
            SiteMapNode root = siteMap.CurrentNode;
            if (root != null && root.ChildNodes.Count>0)
            {
                writer.Write("<div class=\"masInfo\">");
                if (_mostrarTitulo)
                {
                    string titulo = string.IsNullOrEmpty(_titulo) ? "Más información" : _titulo;
                    writer.Write("<div class=\"title\">");
                    writer.Write(titulo);
                    writer.Write("</div>");
                }
                writer.Write("<div>");
                if (!string.IsNullOrEmpty(this._descripcion))
                {
                    writer.Write("<p>{0}</p>", this._descripcion);
                }
                switch (this.Mostrar)
                {
                    case Formato.UnorderedList:
                        writer.Write("<ul>");
                        break;
                    case Formato.NumberedList:
                        writer.Write("<ol>");
                        break;
                    default:
                        writer.Write("<div style\"display:block;position:relative;\">");
                        break;
                }
                foreach (SiteMapNode node in root.ChildNodes)
                {
                    bool visible;
                    string visibleAttribute = node["visible"];
                    if (!string.IsNullOrEmpty(visibleAttribute) && bool.TryParse(visibleAttribute, out visible) && !visible)
                        continue;
                    switch (this.Mostrar)
                    {
                        case Formato.UnorderedList:
                        case Formato.NumberedList:
                            writer.Write(string.Format("<li><a href=\"{1}\">{0}</a></li>", node.Title, node.Url));
                            break;
                        case Formato.Image:
                            writer.Write(string.Format("<div style=\"float:left; padding:5px; text-align:center;\"><a href=\"{1}\" ><img src=\"{0}\" alt=\"{2}\" /></a></div>", Page.ResolveUrl(node["image"]), node.Url, node.Title));
                            break;
                        case Formato.ImageAndText:
                            writer.Write(string.Format("<div style=\"float:left; padding:5px; text-align:center;\"\"><a href=\"{2}\"  ><img src=\"{0}\" /><br />{1}</a></div>", Page.ResolveUrl(node["image"]), node.Title, node.Url));
                            break;
                    }
                }
                switch (this.Mostrar)
                {
                    case Formato.UnorderedList:
                        writer.Write("</ul>");
                        break;
                    case Formato.NumberedList:
                        writer.Write("</ol>");
                        break;
                    default:
                        writer.Write("</div>");
                        break;
                }
                writer.Write("<div style=\"clear:both;\"></div></div></div>");
                //base.Render(writer);
            }
        }
    }
}
