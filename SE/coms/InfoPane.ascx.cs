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

public partial class coms_InfoPane : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        _logUrl = Page.ResolveUrl("~/img/igss-big.jpg");
        _bannerUrl = Page.ResolveUrl("~/img/Anuncio1.swf");
        //_bannerUrl = Page.ResolveUrl("~/img/bannerIgss.swf");
    }
    private string _logUrl;
    private string _bannerUrl;
    public string BannerUrl
    {
        get { return _bannerUrl; }
    }
    public string LogoUrl
    {
        get { return _logUrl; }
    }
    private bool _showBanner = false;

    public bool ShowBanner
    {
        get { return _showBanner; }
        set { _showBanner = value; }
    }

    private bool _showSearch = false;

    public bool ShowSearch
    {
        get { return _showSearch; }
        set { _showSearch = value; }
    }

    protected override void OnPreRender(EventArgs e)
    {
        this.barea.Visible = _showBanner;
        this.sarea.Visible = _showSearch;
        base.OnPreRender(e);
    }
}
