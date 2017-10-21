using System;
using System.Xml;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using WsMtService;

public partial class Servicios_Afiliados_ConsultaContribuciones : System.Web.UI.Page
{
    #region "Eventos"
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ddlDocumentos.Attributes.Add("onChange", "VerOrdenCed(this)");
        }

        lblMensajeError.Text = "";
    }
    protected void btnConsultar_Click(object sender, EventArgs e)
    {
        if (ddlDocumentos.SelectedValue != "CE")
            divOrdenCed.Style.Value = "display:none";
        pnlDetallePatrono.Visible = false;
        pnlPatronos.Visible = false;
        DatosAfiliado();
    }

    protected void gridPatronos_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Detalle")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            lblNombrePatrono.Text = gridPatronos.Rows[index].Cells[1].Text;
            DetallePatrono(gridPatronos.Rows[index].Cells[0].Text);
        }
    }

    protected void gridDetalle_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowIndex > 5)
        {
            e.Row.Visible = false;
        }
    }

    protected void btnLimpiar_Click(object sender, EventArgs e)
    {
        txtDocumento.Text = "";
        txtNumAfiliado.Text = "";
        lblNombreAfiliado.Text = "";
        pnlPatronos.Visible = false;
        pnlDetallePatrono.Visible = false;
        gridDetalle.DataSource = null;
        gridPatronos.DataSource = null;
        lblNombrePatrono.Text = "";
        divOrdenCed.Style.Value = "display:none";
        ddlDocumentos.SelectedIndex = 0;
    }

    #endregion

    // Obtiene el listado de patronos del afiliado
    void obtenerPatronos()
    {
        DataSet dsResultado = new DataSet();
        DataSet dsParametros = new DataSet();
        wsMiddleTier ws = new wsMiddleTier();

        dsParametros = AgregarParametros(dsParametros, "NUMERO_AFILIADO", txtNumAfiliado.Text, typeof(string));
        
        object oDatos = ws.GetDataByObject(dsParametros,
                            "Igss.RUAP.Afiliados.Logic.OperacionesSE",
                            "Igss.RUAP.Afiliados.Logic.OperacionesSE.OperacionesSE",
                            "PatronosXAfiliado", true, "DS", Page.User.Identity.Name, null, false, false, "PoolRUAP");

        dsResultado = RetornaDataset(oDatos);
        if (dsResultado.Tables.Count > 0)
        {
            gridPatronos.DataSource = dsResultado;
            gridPatronos.DataBind();
        }
        else
        {
            gridPatronos.DataSource = null;
            gridPatronos.DataBind();
        }
    }

    // Valida si los datos ingresados son correctos, y devuelve los datos generales del afiliado e invoca
    // a la función que devuelve el listado depatronos del afiliado
    void DatosAfiliado()
    {
        bool DocumentoCorrecto = true;
        DataSet ds = new DataSet();
        DataSet dsDatosAfiliado = new DataSet();
        Object result;

        ds = AgregarParametros(ds, "NUMAFI", txtNumAfiliado.Text, typeof(string));
        wsMiddleTier Ws = new wsMiddleTier();

        result = Ws.GetDataByObject(ds, "Igss.RUAP.Afiliados.Logic.uxBusca", "Igss.RUAP.Afiliados.Logic.uxBusca.InfoGeneral", "Afiliado", true, "DS", "ustr", "RAP_AFILIADOS", false, false, "PoolRUAP");
        dsDatosAfiliado = RetornaDataset(result);
        if (dsDatosAfiliado.Tables.Count > 0)
        {
            // Comparar el documento de identificación ingresado
            switch (ddlDocumentos.SelectedValue)
            {
                case "DPI":
                    {
                        if (dsDatosAfiliado.Tables[0].Rows[0]["DUI"].ToString() != txtDocumento.Text)
                            DocumentoCorrecto = false;
                    } break;
                case "CE":
                    {
                        divOrdenCed.Style.Value = "display:inline";
                        if (dsDatosAfiliado.Tables[0].Rows[0]["CEDULA_REGISTRO"].ToString() != txtDocumento.Text
                            || dsDatosAfiliado.Tables[0].Rows[0]["CEDULA_ORDEN"].ToString() != ddlOrdenCedula.SelectedValue)
                            DocumentoCorrecto = false;
                    } break;
                case "P":
                    {
                        if (dsDatosAfiliado.Tables[0].Rows[0]["PASAPORTE"].ToString() != txtDocumento.Text)
                            DocumentoCorrecto = false;
                    } break;
            }
            if (!DocumentoCorrecto)
                lblMensajeError.Text = "Los datos ingresados no corresponden a ningún afiliado registrado.";
            else
            {
                // Mostrar los datos del afiliado
                pnlPatronos.Visible = true;
                lblNombreAfiliado.Text = dsDatosAfiliado.Tables[0].Rows[0]["NOMA"].ToString();
                obtenerPatronos();
            }
        }
        else
        {
            lblMensajeError.Text = "Los datos ingresados no corresponden a ningún afiliado registrado.";
        }

    }

    // Para el patrono seleccionado, muestra las últimas 6 contribuciones, indicando si en ellas se hizo aporte
    // para el afiliado que consulta
    void DetallePatrono(string NumPatrono)
    {
        DataSet dsResultado = new DataSet();
        DataSet dsParametros = new DataSet();
        wsMiddleTier ws = new wsMiddleTier();

        dsParametros = AgregarParametros(dsParametros, "NUMERO_AFILIADO", txtNumAfiliado.Text, typeof(string));
        dsParametros = AgregarParametros(dsParametros, "NUMERO_PATRONO", NumPatrono, typeof(string));

        object oDatos = ws.GetDataByObject(dsParametros,
                            "Igss.RUAP.Afiliados.Logic.OperacionesSE",
                            "Igss.RUAP.Afiliados.Logic.OperacionesSE.OperacionesSE",
                            "AportesAfiliadoXPatrono", true, "DS", Page.User.Identity.Name, null, false, false, "PoolRUAP");

        dsResultado = RetornaDataset(oDatos);
        pnlDetallePatrono.Visible = true;

        if (dsResultado.Tables.Count > 0)
        {
            gridDetalle.DataSource = dsResultado;
            gridDetalle.DataBind();
            NombreMes();
        }
        else
        {
            gridDetalle.DataSource = null;
            gridDetalle.DataBind();
        }
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

    void NombreMes()
    {
        int intIndice;
        if (gridDetalle.Rows.Count > 0)
        {
            for (int i = 0; i < gridDetalle.Rows.Count; i++)
            {
                if (int.TryParse(gridDetalle.Rows[i].Cells[0].Text, out intIndice))
                    gridDetalle.Rows[i].Cells[0].Text = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.MonthNames[intIndice-1].ToString().ToUpper();
            }
        }
    }
}
