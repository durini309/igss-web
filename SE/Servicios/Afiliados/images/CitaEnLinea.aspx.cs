using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Xml;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using WsMtService;

public partial class Servicios_Afiliados_CitaEnLinea : System.Web.UI.Page
{
    #region Eventos

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            rvFecha.MinimumValue = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
            rvFecha.MaximumValue = DateTime.Now.AddDays(365).ToString("dd/MM/yyyy");
        }
    }

    protected void btnConsultar_Click(object sender, EventArgs e)
    {
        ValidaCTE();
    }

    protected void btnValidar_Click(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        DataSet dsDatosCita = new DataSet();
        Object result;

        try
        {
            ds = AgregarParametros(ds, "CODIGO_UNIDAD", ViewState["CODIGO_UNIDAD"].ToString(), typeof(string));
            ds = AgregarParametros(ds, "COD_ESPECIALIDAD", ddlEspecialidad.SelectedValue, typeof(string));
            ds = AgregarParametros(ds, "FECHA_CITA_REQUERIDA", DateTime.Parse(txtFechaSelec.Text), typeof(DateTime));
            wsMiddleTier Ws = new wsMiddleTier();

            result = Ws.GetDataByObject(ds, "Igss.Salud.CitaEnLinea.Control", "Igss.Salud.CitaEnLinea.Control.CitaEnLinea", "ObtenerFechaCita", true, "DS", "ustr", "SII_CITAS", false, false, "PoolSIGMCOEX");

            dsDatosCita = RetornaDataset(result);
            if (dsDatosCita.Tables.Count > 0 && dsDatosCita.Tables[0].Rows.Count > 0)
            {
                if (!EsError(result))
                {
                    ViewState["FECHA_HORA_CITA"] = dsDatosCita.Tables[0].Rows[0]["FECHA_HORA_CITA"];
                    ViewState["COD_CLINICA"] = dsDatosCita.Tables[0].Rows[0]["COD_CLINICA"].ToString();
                    ViewState["COD_JORNADA"] = dsDatosCita.Tables[0].Rows[0]["COD_JORNADA"].ToString();
                    ViewState["COD_ESPECIALIDAD"] = ddlEspecialidad.SelectedValue;
                    VentanaConfirmacion(this.Master.Page, "Confirmar fecha", "La cita será reservada en la siguiente fecha:<b>" + Convert.ToDateTime(dsDatosCita.Tables[0].Rows[0]["FECHA_HORA_CITA"]).ToString("dd/MM/yyyy HH:mm") + "</b>. ¿Confirma la reservación?", btnConfirmarCita);
                }
                else
                {
                    lblMensajeError.Text = dsDatosCita.Tables[0].Rows[0]["DESCRIPCION"].ToString();
                }
            }
            else
            {
                lblMensajeError.Text = "No se pudo obtener un espacio para reservar la cita.";
            }
        }
        catch (Exception Ex)
        {
            lblMensajeError.Text = Ex.Message;
        }
    }

    protected void btnConfirmarCita_Click(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        DataSet dsResultado = new DataSet();
        Object result;

        try
        {
            lblMensajeError.Text = "";
            btnValidar.Enabled = false;

            ds = AgregarParametros(ds, "USUARIO_ING", "USR_PUBLICO", typeof(string));
            ds = AgregarParametros(ds, "TIPO_DERECHOHABIENTE", ViewState["TIPO_DERECHOHABIENTE"].ToString(), typeof(string));
            ds = AgregarParametros(ds, "NUMERO_AFILIADO", ViewState["NUMERO_AFILIADO"].ToString(), typeof(string));
            ds = AgregarParametros(ds, "NUMERO_AFILIADO_PACIENTE", ViewState["NUMERO_AFILIADO_PACIENTE"].ToString(), typeof(string));
            ds = AgregarParametros(ds, "DIRECCION_PACIENTE", ViewState["DIRECCION_PACIENTE"].ToString(), typeof(string));
            ds = AgregarParametros(ds, "ZONA", Convert.ToDecimal(ViewState["ZONA"].ToString()), typeof(Decimal));
            ds = AgregarParametros(ds, "COD_DEPARTAMENTO", ViewState["COD_DEPARTAMENTO"], typeof(Decimal));
            ds = AgregarParametros(ds, "COD_MUNICIPIO", ViewState["COD_MUNICIPIO"], typeof(Decimal));
            ds = AgregarParametros(ds, "CODIGO_UNIDAD", ViewState["CODIGO_UNIDAD"].ToString(), typeof(string));
            ds = AgregarParametros(ds, "COD_JORNADA", ViewState["COD_JORNADA"].ToString(), typeof(string));
            ds = AgregarParametros(ds, "COD_CLINICA", ViewState["COD_CLINICA"].ToString(), typeof(string));
            ds = AgregarParametros(ds, "COD_ESPECIALIDAD", ViewState["COD_ESPECIALIDAD"].ToString(), typeof(string));
            ds = AgregarParametros(ds, "NUMERO_CERTIFICADO", ViewState["NUMERO_CERTIFICADO"].ToString(), typeof(string));
            ds = AgregarParametros(ds, "FECHA_HORA_CITA", ViewState["FECHA_HORA_CITA"], typeof(DateTime));

            wsMiddleTier Ws = new wsMiddleTier();

            result = Ws.GetDataByObject(ds, "Igss.Salud.CitaEnLinea.Control", "Igss.Salud.CitaEnLinea.Control.CitaEnLinea", "ReservarCita", true, "DS", "ustr", "SII_CITAS", false, false, "PoolSIGM");
            dsResultado = RetornaDataset(result);
            if (dsResultado.Tables.Count > 0 && dsResultado.Tables[0].Rows.Count > 0)
            {
                if (!EsError(result))
                {
                    DateTime FechaHoraCita = Convert.ToDateTime(ViewState["FECHA_HORA_CITA"].ToString());
                    txtClinicaJornada.Text = ViewState["COD_CLINICA"].ToString() + " - " + ViewState["COD_JORNADA"].ToString();
                    pnlConfirmada.Visible = true;
                    iframe_reportes.Attributes["src"] = "generaConstancia.aspx?af=" + ViewState["NUMERO_AFILIADO_PACIENTE"].ToString() + "&fhc=" + FechaHoraCita.ToString("dd/MM/yyyy HH:mm:ss");
                }
                else
                {
                    lblMensajeError.Text = "ERROR. " + dsResultado.Tables[0].Rows[0]["DESCRIPCION"].ToString();
                }
            }
        }
        catch (Exception Ex)
        {
            lblMensajeError.Text += "ERROR. " + Ex.Message;
        }
    }

    protected void btnNuevaConsulta_Click(object sender, EventArgs e)
    {
        Limpiar();
        lblMensajeError.Text = "";
    }

    protected void btnReimprimir_Click(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        DataSet dsCita = new DataSet();
        DateTime FechaHoraCita;
        Object result;

        ds = AgregarParametros(ds, "NUMERO_CTE", txtCTE.Text, typeof(string));
        ds = AgregarParametros(ds, "NUMERO_AFILIADO", txtNumAfiliado.Text, typeof(string));
        wsMiddleTier Ws = new wsMiddleTier();

        try
        {
            result = Ws.GetDataByObject(ds, "Igss.Salud.CitaEnLinea.Control", "Igss.Salud.CitaEnLinea.Control.CitaEnLinea", "ObtenerFechaCitaReservada", true, "DS", "ustr", "SII_CITAS", false, false, "PoolSIGMCOEX");
            dsCita = RetornaDataset(result);
            if (!EsError(result))
            {
                if (dsCita.Tables.Count > 0 && dsCita.Tables[0].Rows.Count > 0)
                {
                    FechaHoraCita = Convert.ToDateTime(dsCita.Tables[0].Rows[0]["FECHA_HORA_CITA"]);
                    iframe_reportes.Attributes["src"] = "generaConstancia.aspx?af=" + dsCita.Tables[0].Rows[0]["NUMERO_AFILIADO_PACIENTE"].ToString() + "&fhc=" + FechaHoraCita.ToString("dd/MM/yyyy HH:mm:ss");
                    VerConstancia();
                }
                else
                {
                    lblMensajeError.Text = "No se encontró ninguna cita reservada en línea con el número de afiliado y número de CTE ingresados.";
                }
            }
            else
            {
                lblMensajeError.Text = dsCita.Tables[0].Rows[0]["DESCRIPCION"].ToString();
            }
        }
        catch (Exception Ex)
        {
            lblMensajeError.Text = Ex.Message;
        }
    }

    #endregion

    // Obtiene los datos del afiliado y CTE
    void ValidaCTE()
    {
        DataSet ds = new DataSet();
        DataSet dsDatosAfiliado = new DataSet();
        DataSet dsDatosCTE = new DataSet();
        Object result;

        try
        {
            ds = AgregarParametros(ds, "NUMAFI", txtNumAfiliado.Text, typeof(string));
            wsMiddleTier Ws = new wsMiddleTier();

            result = Ws.GetDataByObject(ds, "Igss.RUAP.Afiliados.Logic.uxBusca", "Igss.RUAP.Afiliados.Logic.uxBusca.InfoGeneral", "Afiliado", true, "DS", "ustr", "RAP_AFILIADOS", false, false, "PoolRUAP");

            dsDatosAfiliado = RetornaDataset(result);
            if (dsDatosAfiliado.Tables.Count > 0)
            {
                // Datos del afiliado
                txtNombreAfiliado.Text = dsDatosAfiliado.Tables[0].Rows[0]["NOMA"].ToString();
                txtNumCTE.Text = txtCTE.Text;
                txtNumAfi.Text = txtNumAfiliado.Text;

                // Verificar si el CTE le pertenece al afiliado
                ds = new DataSet();
                ds = AgregarParametros(ds, "NUMERO_AFILIADO", txtNumAfiliado.Text, typeof(string));
                ds = AgregarParametros(ds, "CTE", txtCTE.Text, typeof(string));

                result = Ws.GetDataByObject(ds, "Igss.RUAP.Afiliados.Logic.OperacionesSE", "Igss.RUAP.Afiliados.Logic.OperacionesSE.OperacionesSE", "ValidaCTE", true, "DS", "ustr", "REC_CERT_TRABAJO_ELECTRONICO", false, false, "PoolRUAP");
                dsDatosCTE = RetornaDataset(result);
                if (dsDatosCTE.Tables.Count > 0)
                {
                    if (dsDatosCTE.Tables[0].Rows[0]["COD_RESULTADO"].ToString() == "0")
                    {
                        // Mostrar datos para creación de cita
                        pnlDatosGenerales.Visible = true;
                        if (dsDatosCTE.Tables[0].Rows[0]["USO_CERTIFICADO"].ToString() == "B")
                        {
                            ds = new DataSet();
                            ds = AgregarParametros(ds, "NUMAFI", dsDatosCTE.Tables[0].Rows[0]["NUMERO_AFILIADO_BENEFICIARIO"].ToString(), typeof(string));
                            result = Ws.GetDataByObject(ds, "Igss.RUAP.Afiliados.Logic.uxBusca", "Igss.RUAP.Afiliados.Logic.uxBusca.InfoGeneral", "Afiliado", true, "DS", "ustr", "RAP_AFILIADOS", false, false, "PoolRUAP");
                            dsDatosAfiliado = RetornaDataset(result);
                            if (dsDatosAfiliado.Tables.Count > 0)
                            {
                                txtNombrePaciente.Text = dsDatosAfiliado.Tables[0].Rows[0]["NOMA"].ToString();
                            }
                        }
                        else
                            txtNombrePaciente.Text = txtNombreAfiliado.Text;
                        txtUnidadAdscripcion.Text = dsDatosCTE.Tables[0].Rows[0]["NOMBRE_CENTRO"].ToString();
                        lblMensajeError.Text = "";
                        ParametrosEnMemoria(dsDatosCTE);
                        LlenarListaEspecialidades();
                        btnValidar.Enabled = true;
                        pnlAfiliado.Visible = false;
                    }
                    else
                    {
                        if (dsDatosCTE.Tables[0].Rows[0]["COD_RESULTADO"].ToString() == "-1")
                            lblMensajeError.Text = "ERROR. " + dsDatosCTE.Tables[0].Rows[0]["MSJ_ERROR"].ToString();
                        else
                            lblMensajeError.Text = dsDatosCTE.Tables[0].Rows[0]["MENSAJE"].ToString();
                    }

                }
                else
                {
                    lblMensajeError.Text = "El CTE es incorrecto o no pertenece al afiliado registrado.";
                }
            }
            else
            {
                lblMensajeError.Text = "El número de afiliación es incorrecto.";
            }
        }
        catch (Exception Ex)
        {
            Response.Write("ERROR, vuelva a cargar la página. " + Ex.Message);
        }
    }

    /// <summary>
    ///     Llena el listado de especialidades en las que se puede crear una cita
    /// </summary>
    void LlenarListaEspecialidades()
    { 
        DataSet ds = new DataSet();
        DataSet dsEspecialidades = new DataSet();
        Object result;

        ds = AgregarParametros(ds, "CODIGO_UNIDAD", ViewState["CODIGO_UNIDAD"].ToString(), typeof(string));
        ds = AgregarParametros(ds, "RIESGO", ViewState["RIESGO"].ToString(), typeof(string));
        ds = AgregarParametros(ds, "TIPO_DERECHOHABIENTE", ViewState["TIPO_DERECHOHABIENTE"].ToString(), typeof(string));
        wsMiddleTier Ws = new wsMiddleTier();

        try
        {
            result = Ws.GetDataByObject(ds, "Igss.Salud.CitaEnLinea.Control", "Igss.Salud.CitaEnLinea.Control.CitaEnLinea", "EspecialidadesDerechohabiente", true, "DS", "ustr", "SII_CONF_ESPECIALIDADES_CITAENL", false, false, "PoolSIGMCOEX");
            dsEspecialidades = RetornaDataset(result);
            if (!EsError(result))
            {

                if (dsEspecialidades.Tables.Count > 0 && dsEspecialidades.Tables[0].Rows.Count > 0)
                {
                    ddlEspecialidad.DataSource = dsEspecialidades;
                    ddlEspecialidad.DataBind();
                }
                else
                {
                    ddlEspecialidad.DataSource = null;
                    btnValidar.Enabled = false;
                    lblMensajeError.Text = "No hay clínicas habilitadas para la reservación de citas en línea en la unidad. Por ahora no es posible reservar cita en línea.";
                    pnlDatosGenerales.Visible = false;
                }
            }
            else
            {
                lblMensajeError.Text = dsEspecialidades.Tables[0].Rows[0]["DESCRIPCION"].ToString();
            }
        }
        catch (Exception Ex)
        {
            lblMensajeError.Text = Ex.Message;
        }
    }

    /// <summary>
    ///     A partir del DataSet, crea los parámetros requeridos como ViewState
    /// </summary>
    /// <param name="datos"></param>
    private void ParametrosEnMemoria(DataSet datos)
    { 
        ViewState["CODIGO_UNIDAD"] = datos.Tables[0].Rows[0]["COD_CENTRO"].ToString();
        ViewState["RIESGO"] = datos.Tables[0].Rows[0]["COD_SUBPROGRAMA"].ToString();
        ViewState["TIPO_DERECHOHABIENTE"] = datos.Tables[0].Rows[0]["TIPO_DERECHOHABIENTE"].ToString();
        ViewState["NUMERO_AFILIADO_PACIENTE"] = datos.Tables[0].Rows[0]["NUMERO_AFILIADO_PACIENTE"].ToString();
        ViewState["NUMERO_CERTIFICADO"] = datos.Tables[0].Rows[0]["NUMERO_CERTIFICADO"].ToString();
        ViewState["NUMERO_AFILIADO"] = datos.Tables[0].Rows[0]["NUMERO_AFILIADO"].ToString();
        ViewState["DIRECCION_PACIENTE"] = datos.Tables[0].Rows[0]["DIRECCION"].ToString();
        ViewState["ZONA"] = datos.Tables[0].Rows[0]["ZONA"].ToString();
        ViewState["COD_DEPARTAMENTO"] = datos.Tables[0].Rows[0]["DEPARTAMENTO"];
        ViewState["COD_MUNICIPIO"] = datos.Tables[0].Rows[0]["MUNICIPIO"];
    }

    /// <summary>
    ///     Colocar los controles en su estado inicial
    /// </summary>
    private void Limpiar()
    {
        pnlAfiliado.Visible = true;
        txtNombreAfiliado.Text = "";
        txtNombrePaciente.Text = "";
        txtUnidadAdscripcion.Text = "";
        txtClinicaJornada.Text = "";
        ddlEspecialidad.DataSource = null;
        pnlDatosGenerales.Visible = false;
        ViewState.Clear();
        btnValidar.Enabled = true;
        txtCTE.Text = "";
        txtNumAfiliado.Text = "";
        txtNumCTE.Text = "";
        txtNumAfi.Text = "";
        txtFechaSelec.Text = "";
        pnlDoc.Visible = false;
        pnlConfirmada.Visible = false;
    }

    // Función que facilita la creación del DataSet de parámetros
    DataSet AgregarParametros(DataSet dsParametros, string NombreParametro, object Valor, Type Tipo)
    {
        if (dsParametros.Tables.Count == 0)
        {
            dsParametros.Tables.Add("PARAMETROS");
        }
        if (!dsParametros.Tables[0].Columns.Contains(NombreParametro))
        {
            dsParametros.Tables[0].Columns.Add(NombreParametro, Tipo);
        }
        if (dsParametros.Tables[0].Rows.Count == 0)
        {
            DataRow dr = dsParametros.Tables[0].NewRow();
            dr[NombreParametro] = Valor;
            dsParametros.Tables[0].Rows.Add(dr);
        }
        else
        {
            dsParametros.Tables[0].Rows[0][NombreParametro] = Valor;
        }
        return dsParametros;
    }

    DataSet RetornaDataset(object v_xml)
    {
        DataSet datos = new DataSet();
        XmlDocument doc = new XmlDocument();
        System.IO.StringReader sr;

        doc.LoadXml(v_xml.ToString());
        doc.LoadXml(doc.DocumentElement.InnerXml);
        sr = new System.IO.StringReader(doc.InnerXml);
        datos.ReadXml(sr, XmlReadMode.Auto);
        return datos;
    }

    bool EsError(object v_xml)
    {
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(v_xml.ToString());
        if (doc.FirstChild.Name == "ERRORES")
            return true;
        else
        {
            if (doc.SelectNodes("//ERROR").Count > 0)
                return true;
            else
                return false;
        }
    }

    protected void VentanaConfirmacion(Page pagina, string titulo, string mensaje, Button botonAsociado_OK)
    {

        string codigoDelServer = string.Empty;

        if (botonAsociado_OK != null)
        {
            codigoDelServer = " document.getElementById(\"" + botonAsociado_OK.ClientID + "\").click(); ";
        }

        //script de la funcion para desplegar el messagebox
        string scriptMessageBox =
        "<script language=\"javascript\" type=\"text/javascript\"> \n" +
        "//<![CDATA[ \n" +
        "$(document).ready( function() { " +

             "var vdiv = " +
                "$('<div id=\"jdialog_message\"/>') " +
                        ".css({ " +
                        "display  : 'none' " +
                    "}); " +

                "$('form:first').append(vdiv); " +

                "$('#jdialog_message').append('<p>" + mensaje + "</p>'); " +

                "$('#jdialog_message').dialog( " +
                        "{ title	: '" + titulo + "' }, " +
                        "{ modal	: true}," +
                        "{ buttons: { \"Aceptar\" : function() { $(this).dialog(\"close\"); " + codigoDelServer + " }, " +
                                     "\"Cancelar\" : function() { $(this).dialog(\"close\"); }" +
                                   "} } " +
                "); " +
            "}); \n" +
        "//]]>" +
        "</script> ";

        //registramos el script, y usamos el objeto ScriptManager debido a que es posible que esta funcion pueda
        //ser llamada desde un objeto que es un trigger para algun Updatepanel
        pagina.ClientScript.RegisterStartupScript(typeof(Page), "messagebox", scriptMessageBox, false);
    }

    protected void VerConstancia()
    {
        string scriptMessageBox =
        "<script language=\"javascript\" type=\"text/javascript\"> \n" +
        "//<![CDATA[ \n" +
        "$(document).ready( function() { " +

                "$('#ctl00_content_pnlDoc').dialog( " +
                        "{ title	: 'Constancia de cita reservada' }, " +
                        "{ modal	: true}," +
                        "{ width    : 700}," +
                        "{ height   : 550}," +
                        "{ buttons: { \"Cerrar\" : function() { $(this).dialog(\"close\"); }" +

                                   "} } " +
                "); " +



            "}); \n" +
        "//]]>" +
        "</script> ";

        this.Master.Page.ClientScript.RegisterStartupScript(typeof(Page), "Constancia", scriptMessageBox, false);    
    }


    protected void btnConstancia_Click(object sender, EventArgs e)
    {
        VerConstancia();
    }
}
