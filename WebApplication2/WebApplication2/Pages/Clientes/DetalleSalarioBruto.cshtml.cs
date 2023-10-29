using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Data;
using System.Data.SqlTypes;

namespace WebApplication2.Pages.Clientes
{
    public class DetalleSalarioBrutoModel : PageModel
    {
        public List<MarcasSemanaPlanilla> listaMarcasSemanaPlanilla = new List<MarcasSemanaPlanilla>();
        public String idPlanilla = Global.idSemanaPlanilla.ToString();
        public String nombreUsuarioEmpleado = Global.nombreUsuarioEmpleado;
        public String docIdentificacionEmpleado = Global.docIdentificacionEmpleado;
        public String errorMessage = "";
        public void OnGet()
        {
            try
            {
                String SPNombre = "dbo.DetallesSemanaPlanilla";
                String connectionString = "Data Source=pruebajose2312.database.windows.net;Initial Catalog=prueba2312;Persist Security Info=True;User ID=adminjose;Password=Bases1234";

                using SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();                                      //Se abre la coneccion con la BD.
                using (SqlCommand command = new SqlCommand(SPNombre, connection))
                {
                    //Variables para obtener el DataSet mandados de la BD.
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    DataTable table = new DataTable();

                    command.CommandType = CommandType.StoredProcedure;  //Indicar que el comando sera un SP.
                    command.Parameters.AddWithValue("@inIdSemanaPlanilla", int.Parse(idPlanilla));
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
                            MarcasSemanaPlanilla MarcasPlanillaS = new MarcasSemanaPlanilla();
                            DateTime fechaHora = (DateTime)row[0];
                            DateTime horaInicio = (DateTime)row[1];
                            DateTime horaFin = (DateTime)row[2]; 
                            
                            MarcasPlanillaS.Fecha = fechaHora.ToString("yyyy-MM-dd");
                            MarcasPlanillaS.Inicio = "" + row[1];
                            MarcasPlanillaS.Fin = "" + row[2];
                            MarcasPlanillaS.QOrdinarias = "" + row[3];
                            MarcasPlanillaS.MQOrdinarias = "" + SqlMoney.Parse(row[4].ToString());
                            MarcasPlanillaS.QExtra = "" + row[5];
                            MarcasPlanillaS.MQExtra = "" + SqlMoney.Parse(row[6].ToString());
                            MarcasPlanillaS.QDoble = "" + row[7];
                            MarcasPlanillaS.MQDoble = "" + SqlMoney.Parse(row[8].ToString());

                            listaMarcasSemanaPlanilla.Add(MarcasPlanillaS);
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
