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

using WsReportes;
using WsMtService;

public partial class Servicios_Afiliados_generaConstancia : System.Web.UI.Page
{
    string sNumeroAfiliado = "";
    string sFechaHoraCita = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        string nombreReporte = "Constancia Cita en Linea";
        ParameterValue[] parametros;
        wsMiddleTier ws = new wsMiddleTier();
        object oDatos;

        if (Request.QueryString["af"] != null)
        {
            if (Request.QueryString["af"].ToString().Length > 0)
                sNumeroAfiliado = Request.QueryString["af"].ToString();
        }

        if (Request.QueryString["fhc"] != null)
        {
            if (Request.QueryString["fhc"].ToString().Length > 0)
                sFechaHoraCita = Request.QueryString["fhc"].ToString();
        }

        parametros = new ParameterValue[2];

        parametros[0] = new ParameterValue();
        parametros[0].Name = "NumeroAfiliadoPaciente";
        parametros[0].Value = sNumeroAfiliado;

        parametros[1] = new ParameterValue();
        parametros[1].Name = "FechaHoraCita";
        parametros[1].Value = sFechaHoraCita;

        ejecutaReporte(nombreReporte, "V", parametros);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="nombreReporte"></param>
    /// <param name="orientacion">V:Vertical H:Horizontal</param>
    /// <param name="parameters"></param>
    private void ejecutaReporte(string nombreReporte, string orientacion, ParameterValue[] parameters)
    {

        //instanciamos el servico de Reporting Services para ejecutar un reporte
        ReportExecutionService rs1 = new ReportExecutionService();
        
        //configuramos las credenciales de red para acceder al servidor de reportes
        string usuarioReportes = System.Configuration.ConfigurationSettings.AppSettings["Reports"];
        int finUsuario = usuarioReportes.IndexOf(":");
        string Usuario = usuarioReportes.Substring(0, finUsuario);
        int finClave = usuarioReportes.IndexOf("@");
        string Clave = usuarioReportes.Substring(finUsuario+1, finClave-finUsuario-1);
        string Dominio = usuarioReportes.Substring(finClave + 1, usuarioReportes.Length - finClave-1); 
        rs1.Credentials = new System.Net.NetworkCredential(Usuario, Clave, Dominio);
//        rs1.Credentials = new System.Net.NetworkCredential("USR_REPORTES", "whfa1600", "SRVIGSSDESA01");
        rs1.Timeout = 300000;

        byte[] result;

        string ambiente = System.Configuration.ConfigurationSettings.AppSettings["AmbienteReportes"];

        string reportPath;
		System.Net.NetworkCredential LogonCredentials;        
		if(ambiente.Equals("DESARROLLO"))
            reportPath = "/Desarrollo/SIGM/" + nombreReporte; //Ruta y nombre del Reporte que se quiere visualizar
        else
            reportPath = "/Produccion/SIGM/" + nombreReporte; //Ruta y nombre del Reporte que se quiere visualizar
        string format = "PDF"; //Formato en que vamos a desplegar el reporte
        string historyID = null;
        
        string devInfo = "";
        if (orientacion == "V")
            devInfo = "<DeviceInfo><PageWidth>8.5in</PageWidth><PageHeight>11.0in</PageHeight><MarginLeft>0.3in</MarginLeft><MarginRight>0.2in</MarginRight><MarginTop>0.3in</MarginTop><MarginBottom>0.3in</MarginBottom></DeviceInfo>";
        else
            devInfo = "<DeviceInfo><PageWidth>11.0in</PageWidth><PageHeight>8.5in</PageHeight><MarginLeft>0.3in</MarginLeft><MarginRight>0.3in</MarginRight><MarginTop>0.3in</MarginTop><MarginBottom>0.3in</MarginBottom></DeviceInfo>";

        //Ejecutamos el reporte
        DataSourceCredentials[] credentials = new DataSourceCredentials[0];
        ExecutionInfo execInfo;
        ExecutionHeader execHeader = null;
        String encoding = "";
        String mimeType = "";
        Warning[] warnings;
        String[] streamIDs;
        String SessionId = null;
        string extension = "";

        try
        {
            rs1.ExecutionHeaderValue = execHeader;
            execInfo = rs1.LoadReport(reportPath, historyID);
            SessionId = rs1.ExecutionHeaderValue.ExecutionID;
            if (parameters != null)
            {
                rs1.SetExecutionParameters(parameters, "en-us");
            }
            result = rs1.Render(format, devInfo, out extension, out encoding, out mimeType, out warnings, out streamIDs);
            execInfo = rs1.GetExecutionInfo();

            //Escribimos la salida en la pagina web
            Response.ContentType = mimeType;
            Response.OutputStream.Write(result, 0, result.Length);
            Response.End();

            /*
            //Escribimos la salida en la pagina web
            Response.ContentType = mimeType;
            //Response.AppendHeader("content-disposition", "attachment; filename=Constancia.pdf");
            Response.OutputStream.Write(result, 0, result.Length);
            Response.End();*/
        }
        catch (Exception ex)
        {
            Response.Write("<html><body><p>No se pudo cargar el reporte, por favor vuelva a intentarlo.</p> <input type=\"button\" value=\"Reintentar\" onClick=\"window.location.href=window.location.href;\">");
            Response.Write("<p>&nbsp;</p>");
            Response.Write("<p>&nbsp;</p>");
            Response.Write("<p>Información para soporte</p>");
            Response.Write("<p>Mensaje:" + ex.Message + "</p>");
            Response.Write("<p>Origen:" + ex.Source + "</p></body></html>");
            Response.End();
        }
    }
}

