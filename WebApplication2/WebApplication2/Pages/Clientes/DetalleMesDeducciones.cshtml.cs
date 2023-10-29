using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Data;
using System.Data.SqlTypes;

namespace WebApplication2.Pages.Clientes
{
    public class DetalleDeduccionesModel2 : PageModel
    {
        public List<DeducSemanaPlanilla> listaDeducSemanaPlanilla = new List<DeducSemanaPlanilla>();
        public String idMesPlanilla = Global.idMesPlanilla.ToString();
        public String nombreUsuarioEmpleado = Global.nombreUsuarioEmpleado;
        public String docIdentificacionEmpleado = Global.docIdentificacionEmpleado;
        public String errorMessage = "";
        public String inter = "";
        public float conver;
        public void OnGet()
        {
            try
            {
                String SPNombre = "dbo.DetallesMesPlanillaDeduc";
                String connectionString = "Data Source=pruebajose2312.database.windows.net;Initial Catalog=prueba2312;Persist Security Info=True;User ID=adminjose;Password=Bases1234";

                using SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();                                      //Se abre la coneccion con la BD.
                using (SqlCommand command = new SqlCommand(SPNombre, connection))
                {
                    //Variables para obtener el DataSet mandados de la BD.
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    DataTable table = new DataTable();

                    command.CommandType = CommandType.StoredProcedure;  //Indicar que el comando sera un SP.
                    command.Parameters.AddWithValue("@inIdMesPlanilla", int.Parse(idMesPlanilla));
                    //command.Parameters.AddWithValue("@inUsuario", Global.sesion);
                    //command.Parameters.AddWithValue("@inIP", Global.IP);

                    //Codigo para que detecte el output del SP.
                    SqlParameter resultCodeParam = new SqlParameter("@outResultCode", SqlDbType.Int);
                    resultCodeParam.Direction = ParameterDirection.Output;
                    command.Parameters.Add(resultCodeParam);
                    //command.ExecuteNonQuery();


                    //Porceso de obtener el DataSet.
                    adapter.SelectCommand = command;
                    DataSet dataSet = new DataSet();
                    adapter.Fill(dataSet);

                    //Si el output de errores por DataSet es 0 (No hay problemas).
                    if (dataSet.Tables[0].Rows[0][0].ToString().Equals("0"))
                    {

                        foreach (DataRow row in dataSet.Tables[1].Rows) //Recorra cada fila de la tabla con los datos y estraigala en el tipo ClienteInfo.
                        {
                            DeducSemanaPlanilla DeducPlanillaS = new DeducSemanaPlanilla();
                            DeducPlanillaS.Nombre = "" + row[0];
                            if (row[1]+"" == "0") {
                                DeducPlanillaS.Porcentaje = "N/A";
                            }
                            else
                            {
                                inter = "" + row[1];
                                conver = float.Parse(inter);
                                conver = conver * 100;
                                DeducPlanillaS.Porcentaje = conver + "%";
                            }
                            DeducPlanillaS.Monto = "" + SqlMoney.Parse(row[2].ToString());

                            listaDeducSemanaPlanilla.Add(DeducPlanillaS);
                        }
                    }
                    else
                    {
                        //En caso de que haya algun error al cargar la tabla de articulos.
                        errorMessage = "Error al cargar la lista de articulos.";
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }
        }
    }
}
