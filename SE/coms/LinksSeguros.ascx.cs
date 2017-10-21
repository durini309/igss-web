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
using System.Text;

public partial class coms_LinksSeguros : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    private string _siteMapUrl;
    [Category("Behavior"), Description("Url from the site map, where the links for the child options will be generated"), DefaultValue(null)]
    public string SiteMapUrl
    {
        get { return _siteMapUrl; }
        set { _siteMapUrl = value; }
    }
    private string _title;
    [Category("Appearance"), Description("Url from the site map, where the links for the child options will be generated"), DefaultValue(null)]
    public string Title
    {
        get { return _title; }
        set { _title = value; }
    }
    private string _description;
    [Category("Appearance"), Description("Description no show to users"), DefaultValue(null)]
    public string Description
    {
        get { return _description; }
        set { _description = value; }
    }
    private string _noLinksMessage;
    [Category("Appearance"), Description("Default message to show to user if no options are available"), DefaultValue(null)]
    public string NoLinksMessage
    {
        get { return _noLinksMessage; }
        set { _noLinksMessage = value; }
    }
    protected override void OnPreRender(EventArgs e)
    {
        if (!string.IsNullOrEmpty(_title))
            this.linksTitle.InnerText = _title;
        SiteMapProvider map = SiteMap.Provider;
        string html = null;
        if (map != null)
        {
            SiteMapNode root = map.CurrentNode;
            if (root != null)
            {
                StringBuilder sb = new StringBuilder();
                int i = 0;
                foreach (SiteMapNode node in root.ChildNodes)
                {
                    if (i++ > 0)
                        sb.Append("<br />");
                    sb.AppendFormat("<a href=\"{0}\">", node.Url);
                    sb.Append(node.Title);
                    sb.Append("</a>");
                    if (!string.IsNullOrEmpty(node.Description))
                    {
                        sb.AppendFormat(" - {0}", node.Description);
                    }
                }
                html = sb.ToString();
            }
        }
        this.descriptionLiteral.Text = _description;
        if (!string.IsNullOrEmpty(html))
            this.links.Text = html;
        else
            this.links.Text = _noLinksMessage;
        base.OnPreRender(e);
    }
}
