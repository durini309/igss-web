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

public partial class coms_InfoPaneCompact : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        _logUrl = Page.ResolveUrl("~/img/igss-mid.jpg");
    }
    private string _logUrl;
    public string LogoUrl
    {
        get { return _logUrl; }
    }
}
