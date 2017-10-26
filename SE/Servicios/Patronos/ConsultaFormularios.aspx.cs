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

public partial class Servicios_Afiliados_ConsultaFormularios : System.Web.UI.Page
{
    private static string WS_ID = "jdurini";
    private static string WS_PASSWORD = "lorem";

    #region Listeners

    /////
    /// <summary>
    /// Método llamado cuando se desea hacer la consulta de citas
    /// Se obtienen los siguientes datos de la vista: 
    /// @param numeroIdentificacion: DPI o No de Afiliado
    /// @param sedeSeleccionada: ID de sede seleccionada
    /// @return Datagrid con citas o mensaje de data vacía
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
            string sedeSeleccionada = txtSede.Text;
            string formularioSeleccionado = ddlFormulario.SelectedValue;
            string numeroFormulario = txtNumFormulario.Text;
            string numeroIdentificacion = txtNumAfiliado.Text;
            string fechaFormulario = dtFecha.SelectedDate.ToString("dd/MM/yyyy");
            consultaFormulario(sedeSeleccionada, formularioSeleccionado, numeroFormulario, numeroIdentificacion, fechaFormulario);
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
    private void consultaFormulario(string sede, string tipoForm, string numForm, string numID, string fechaForm)
    {
        // lógica de llamada del WS
        DataSet resultado = formularioDummie(
            sede,
            tipoForm,
            numForm,
            numID,
            fechaForm
        );

        // Actualizamos datagrid y mostramos panel
        gridFormulario.DataSource = resultado;
        gridFormulario.DataBind();
        pnlFormulario.Visible = true;
    }

    private DataSet formularioDummie(string sede, string tipoFormulario, string numeroFormulario, string numID, string fechaSel)
    {
        DataSet dataDummie = new DataSet();
        DataTable dataTable = new DataTable();
        dataTable.Columns.Add("RESULTADO");
        dataTable.Rows.Add("Documento validado exitosamente, fue emitido por el IGSS");
        dataDummie.Tables.Add(dataTable);
        return dataDummie;
    }

    private void mostrarError(string error)
    {
        string script = "alert(\"" + error + "\");";
        ScriptManager.RegisterStartupScript(this, GetType(),
                              "ServerControlScript", script, true);
    }

    [WebMethod(EnableSession = true)]
    public static string[] GetSedes(string keyword)
    {
        //Falta llamada para obtener sedes del servicio WS
        List<string> sede = new List<string>();
        sede.Add("Zona 2");
        sede.Add("Zona 3");
        return sede.ToArray();
    }
    #endregion
}
