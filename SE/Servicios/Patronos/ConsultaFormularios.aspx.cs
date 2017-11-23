using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using org.igssgt.pruebas;

public partial class Servicios_Afiliados_ConsultaFormularios : System.Web.UI.Page
{
    private static string WS_ID = "usr_consulta_citas_ws";
    private static string WS_PASSWORD = "psw_consulta_citas_ws";

    #region Listeners

    /////
    /// <summary>
    /// Método llamado cuando se desea hacer la validación de formularios
    /// Se obtienen los siguientes datos de la vista: 
    /// @param numeroIdentificacion: DPI o No de Afiliado
    /// @param sedeSeleccionada: ID de sede seleccionada
    /// @param numeroFormulario: numero del formulario
    /// @param formularioSeleccionado: tipo de formulario SP-60 o SP-231
    /// @param fechaFormulario: fecha del formulario
    /// @return formulario válido o no válido
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnConsultar_Click(object sender, EventArgs e)
    {
        if (dtFecha.SelectedDate.ToString("dd/MM/yyyy") == "01/01/0001")
        {
            mostrarError("Debe ingresar la fecha del formulario.");
        }
        else
        {
            string sedeSeleccionada = codigoSede.Value;
            string formularioSeleccionado = ddlFormulario.SelectedValue;
            string numeroFormulario = txtNumFormulario.Text;
            string numeroIdentificacion = txtNumAfiliado.Text;
            string fechaFormulario = dtFecha.SelectedDate.ToString("dd/MM/yyyy");
            consultaFormulario(sedeSeleccionada, numeroFormulario, fechaFormulario, numeroIdentificacion, formularioSeleccionado);
        }
    }

    /// <summary>
    /// Método llamado cada vez que se desean limpiar los campos
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnLimpiar_Click(object sender, EventArgs e)
    {
        txtNumAfiliado.Text = "";
        txtNumFormulario.Text = "";
        txtSede.Text = "";
        ddlFormulario.SelectedIndex = 0;
        dtFecha.SelectedDate = DateTime.Now;
        pnlFormulario.Visible = false;
        gridFormulario.DataSource = null;
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }
    #endregion

    #region Funciones y métodos

    /// <summary>
    /// Métpdp que, en base a los parámetros ingresados, hace la consulta al WS
    /// y luego con el resultado obtenido, los muestra en datagrid y en label de nombre.
    /// </summary>
    /// <param name="sede">ID de sede seleccionada</param>
    /// <param name="tipoForm">ID de tipo de formulario seleccionado</param>
    /// <param name="numForm">ID de formulario</param>
    /// <param name="numID">Numero de identificación del usuario</param>
    /// <param name="fechaForm">Fecha del formulario</param>
    private void consultaFormulario(string sede, string numForm, string fechaForm, string numId, string tipoForm)
    {
        // lógica de llamada del WS
        WSMediConsulta ws = new WSMediConsulta();
        Boolean existe = ws.ExisteFormulario(numForm, sede, fechaForm, numId, tipoForm, WS_ID, WS_PASSWORD);
        DataSet resultado = null;

        if (existe)
        {
            resultado = formularioExitoso();
        }
        else
        {
            resultado = formularioNoExitoso();
        }

        // Actualizamos datagrid y mostramos panel
        gridFormulario.DataSource = resultado;
        gridFormulario.DataBind();
        pnlFormulario.Visible = true;
    }

    /// <summary>
    /// Método que retorna Dataset a mostrar cuando formulario es válido
    /// </summary>
    /// <returns></returns>
    private DataSet formularioExitoso()
    {
        DataSet dataDummie = new DataSet();
        DataTable dataTable = new DataTable();
        dataTable.Columns.Add("RESULTADO");
        dataTable.Rows.Add("Documento validado exitosamente, fue emitido por el IGSS");
        dataDummie.Tables.Add(dataTable);
        return dataDummie;
    }

    /// <summary>
    /// Método que retorna Dataset a mostrar cuando formulario es no válido
    /// </summary>
    /// <returns></returns>
    private DataSet formularioNoExitoso()
    {
        DataSet dataDummie = new DataSet();
        DataTable dataTable = new DataTable();
        dataTable.Columns.Add("RESULTADO");
        dataTable.Rows.Add("Documento falso, no fue emitido por el IGSS");
        dataDummie.Tables.Add(dataTable);
        return dataDummie;
    }

    /// <summary>
    /// Método que muestra error en caso de ser necesario
    /// </summary>
    /// <returns></returns>
    private void mostrarError(string error)
    {
        string script = "alert(\"" + error + "\");";
        ScriptManager.RegisterStartupScript(this, GetType(),
                              "ServerControlScript", script, true);
    }

    /// <summary>
    /// Función que convierte las filas "rows" en un formato JSON, para 
    /// luego poder retornar este a la vista (Ajax) y ser la fuente del autocomplete
    /// </summary>
    /// <param name="rows">Filas con unidades médicas</param>
    /// <returns></returns>
    public static string convertSedesToJson(DataRowCollection rows)
    {
        string jsonFinal = "";
        string jsonObjectBase = "{\"value\": \"{0}\", \"label\":\"{1}\"}";
        string jsonObjectActual;
        if (rows.Count > 0)
        {
            int a = 10;
            foreach (DataRow row in rows)
            {
                jsonObjectActual = jsonObjectBase.Replace("{0}", row[0].ToString()).Replace("{1}", row[1].ToString());
                jsonFinal += jsonObjectActual + ",";
            }

            jsonFinal = "[" + jsonFinal.Substring(0, jsonFinal.Length - 1) + "]";
        }

        return jsonFinal;
    }

    [WebMethod(EnableSession = true)]
    public static string GetSedes()
    {
        WSMediConsulta ws = new WSMediConsulta();
        DataSet datos = ws.DatosUnidades(WS_ID, WS_PASSWORD);
        string sedes = convertSedesToJson(datos.Tables[0].Rows);

        return sedes;
    }
    #endregion
}
