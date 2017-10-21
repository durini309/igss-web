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

public partial class coms_Documento : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.Literal1.Text = "No se encontro el contenido "+GetFileName();
    }
    public string GetFileName()
    {
        string url = Page.Request.Url.LocalPath;
        int index = url.LastIndexOf("/");
        string path = url.Substring(0,index-1);
        string filename= url.Substring(index+1,url.Length-index-6); 
        string id = this.ID;
        return string.Format("{0}/{1}.{2}.xdoc",path,filename,id);
    }
    //public string 
}
