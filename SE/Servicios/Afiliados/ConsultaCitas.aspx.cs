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

public partial class Servicios_Afiliados_ConsultaCitas : System.Web.UI.Page
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
        string sedeSeleccionada     = txtSede.Text;
        string numeroIdentificacion = txtNumAfiliado.Text;
        consultaCita(sedeSeleccionada, numeroIdentificacion);
    }

    /// <summary>
    /// Método llamado cada vez que se desean limpiar los campos
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnLimpiar_Click(object sender, EventArgs e)
    {
        txtNumAfiliado.Text = "";
        txtSede.Text = "";
        lblNombreAfiliado.Text = "";
        pnlCitas.Visible = false;
        gridCitas.DataSource = null;
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
    /// <param name="numID">Numero de identificación del usuario</param>
    private void consultaCita(string sede, string numID)
    {
        // lógica de llamada del WS
        DataSet resultado = citasDummie(
            sede,
            numID
        );
        //Si WS retorna datos, se muestra nombre. Si no, se oculta
        if (resultado.Tables[0].Rows.Count > 0)
        {
            lblNombreAfiliado.Text = resultado.Tables[0].Rows[0]["Paciente"].ToString();
            lbltxtNomAfiliado.Visible = true;
            lblNombreAfiliado.Visible = true;
        }
        else
        {
            lbltxtNomAfiliado.Visible = false;
            lblNombreAfiliado.Visible = false;
        }

        // Actualizamos datagrid y mostramos panel
        gridCitas.DataSource = resultado;
        gridCitas.DataBind();
        pnlCitas.Visible = true;
    }

    private DataSet citasDummie(string sede, string numID)
    {
        DataSet dataDummie = new DataSet();
        DataTable dataTable = new DataTable();
        dataTable.Columns.Add("Fecha");
        dataTable.Columns.Add("Unidad");
        dataTable.Columns.Add("Paciente");
        dataTable.Columns.Add("Clinica");
        dataTable.Rows.Add("2017-10-21", "IGSS zona 1", "Juan Carlos Durini", "Borja");
        dataTable.Rows.Add("2017-10-23", "IGSS zona 5", "Juan Carlos Durini", "Asuncion");
        dataTable.Rows.Add("2017-10-27", "IGSS zona 6", "Juan Carlos Durini", "MEga 6");
        dataDummie.Tables.Add(dataTable);
        return dataDummie;
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
