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
using System.Xml;
using org.igssgt.pruebas;

public partial class Servicios_Afiliados_ConsultaCitas : System.Web.UI.Page
{
    private static string WS_ID = "usr_consulta_citas_ws";
    private static string WS_PASSWORD = "psw_consulta_citas_ws";

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
        string sedeSeleccionada = codigoSede.Value;
        string numeroIdentificacion = txtNumAfiliado.Text;
        if (sedeSeleccionada.Equals("0"))
        {
            lblSinResultados.Text = "Unidad médica ingresada no existe";
            lblSinResultados.Visible = true;
            pnlCitas.Visible = true;
        } else 
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

    #region Métodos para consulta de citas

    /// <summary>
    /// Función que retorna datatable con columnas a desplegar
    /// </summary>
    /// <returns></returns>
    private DataTable creaDatatableInicial()
    {
        DataTable dataTable = new DataTable();
        dataTable.Columns.Add("Fecha");
        dataTable.Columns.Add("Unidad");
        dataTable.Columns.Add("Paciente"); //Está oculta porque no se está seguro si este siempre será el mismo paciente o puede cambiar
        dataTable.Columns.Add("Clinica");
        return dataTable;
    }

    /// <summary>
    /// Método que, en base a los parámetros ingresados, hace la consulta al WS
    /// y luego con el resultado obtenido, los muestra en datagrid y en label de nombre.
    /// El resultado del WS pueden ser 2:
    /// 1. Sin registros: retornará un rows con 2 columnas
    /// 2. Con registros: retornará uno o más rows con la información de la cita (6 columnas)
    /// </summary>
    /// <param name="sede">ID de sede seleccionada</param>
    /// <param name="numID">Numero de identificación del usuario</param>
    private void consultaCita(string sede, string numID)
    {
        DataSet dataToReturn = new DataSet();
        WSMediConsulta ws = new WSMediConsulta();
        DataSet datos = ws.DatosCitas(numID, sede, WS_ID, WS_PASSWORD);
        int columnCount = datos.Tables[0].Columns.Count;
        // Si hay más de 2 columnas, quiere decir que sí hay info de la cita
        if (columnCount > 2)
        {
            // Obtenemos datos ratornados por WS
            DataRowCollection drc = datos.Tables[0].Rows;
            DataTable dataTable = creaDatatableInicial();
            foreach (DataRow dr in drc)
            {
                // Los parámetros son: Fecha, Unidad, Paciente y clínica
                dataTable.Rows.Add(dr[1], dr[0], dr[5], " - ");
            }
            // Agregamos información obtenida en dataSet y luego en dataGrid
            dataToReturn.Tables.Add(dataTable);
            gridCitas.DataSource = dataToReturn;
            gridCitas.DataBind();
            gridCitas.Visible = true;

            // Mostramos nombre y labels
            lblNombreAfiliado.Text = drc[0][5].ToString();
            lbltxtNomAfiliado.Visible = true;
            lblNombreAfiliado.Visible = true;
            lblSinResultados.Visible = false;
        }
        else
        {
            lbltxtNomAfiliado.Visible = false;
            lblNombreAfiliado.Visible = false;
            lblSinResultados.Visible = true;
            gridCitas.Visible = false;
            lblSinResultados.Text = "No existen registros para los datos ingresados.";
        }

        // Actualizamos datagrid y mostramos panel
        pnlCitas.Visible = true;
    }
    #endregion

    #region Métodos para obtención de unidades médicas
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
            // Recorremos cada fila para mapearla a JSON
            foreach (DataRow row in rows)
            {
                // Sustituimos {0} por el ID de la unidad médica
                jsonObjectActual = jsonObjectBase.Replace("{0}", row[0].ToString());
                
                // Sustituimos {1} por nombre de la unidad médica
                jsonObjectActual = jsonObjectActual.Replace("{1}", row[1].ToString());
                jsonFinal += jsonObjectActual + ",";
            }

            // Concatenamos [] para convertirlo a Array
            jsonFinal = "[" + jsonFinal.Substring(0, jsonFinal.Length - 1) + "]";
        }

        return jsonFinal;
    }

    /// <summary>
    /// Método llamado via AJAX para obtener las unidades médicas
    /// </summary>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public static string GetSedes()
    {
        // Obtenemos unidades médicas desde WS
        WSMediConsulta ws = new WSMediConsulta();
        DataSet datos = ws.DatosUnidades(WS_ID, WS_PASSWORD);
        
        // Convertimos a JSON las unidades
        string sedes = convertSedesToJson(datos.Tables[0].Rows);

        return sedes;
    }
    #endregion
}
