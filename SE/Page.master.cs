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

public partial class PageMaster : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (SiteMap.CurrentNode != null && !string.IsNullOrEmpty(SiteMap.CurrentNode.Title))
            this.Page.Title = string.Format("{0} - {1}", SiteMap.CurrentNode.Title, this.SiteName);
        else
            this.Page.Title = this.SiteName;
    }
    public string SiteName
    {
        get { 
            string siteName = ConfigurationManager.AppSettings["SiteName"];
            return string.IsNullOrEmpty(siteName) ? "IGSS" : siteName;
        }
    }
}
