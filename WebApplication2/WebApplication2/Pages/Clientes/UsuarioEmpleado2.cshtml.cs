using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Data;
using System.Data.SqlTypes;

namespace WebApplication2.Pages.Clientes
{
    public class UsuarioEmpleadoModel2 : PageModel
    {
        public bool mostrarBoton = Global.mostrarBotonVA;
        public List<MesPlanilla> listaMesPlanilla = new List<MesPlanilla>();
        public String nombreUsuarioEmpleado = Global.nombreUsuarioEmpleado;
        public String docIdentificacionEmpleado = Global.docIdentificacionEmpleado;
        public String errorMessage = "";
        public Empleado empleado = new Empleado();
        public void OnGet()
        {
            try
            {
                String SPNombre = "dbo.BuscarEmpleadoPorId";
                String connectionString = "Data Source=pruebajose2312.database.windows.net;Initial Catalog=prueba2312;Persist Security Info=True;User ID=adminjose;Password=Bases1234";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();                                      //Se abre la coneccion con la BD.
                    using (SqlCommand command = new SqlCommand(SPNombre, connection))
                    {
                        //Variables para obtener el DataSet mandados de la BD.
                        SqlDataAdapter adapter = new SqlDataAdapter();
                        DataTable table = new DataTable();

                        command.CommandType = CommandType.StoredProcedure;  //Indicar que el comando sera un SP.
                        command.Parameters.AddWithValue("@inIdTipoEvento", 12);
                        command.Parameters.AddWithValue("@inIdEmpleado", int.Parse("" + Global.idUsuarioEmpleado));
                        command.Parameters.AddWithValue("@inUsuario", Global.sesion);
                        command.Parameters.AddWithValue("@inIP", Global.IP);

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
                                empleado.Nombre = "" + row[0];
                                empleado.ValorDocIdentidad = "" + row[2];
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
                SPNombre = "dbo.PlanMensual";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();                                      //Se abre la coneccion con la BD.
                    using (SqlCommand command = new SqlCommand(SPNombre, connection))
                    {
                        //Variables para obtener el DataSet mandados de la BD.
                        SqlDataAdapter adapter = new SqlDataAdapter();
                        DataTable table = new DataTable();

                        command.CommandType = CommandType.StoredProcedure;  //Indicar que el comando sera un SP.
                        command.Parameters.AddWithValue("@inIdEmpleado", int.Parse("" + Global.idUsuarioEmpleado));
                        command.Parameters.AddWithValue("@inUsuario", Global.sesion);
                        command.Parameters.AddWithValue("@inIP", Global.IP);

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
                                MesPlanilla PlanillaM = new MesPlanilla();
                                PlanillaM.IdPlanillaM = "" + row[0];
                                PlanillaM.Bruto = "" + SqlMoney.Parse(row[1].ToString());
                                PlanillaM.TotalDeduc = "" + SqlMoney.Parse(row[2].ToString());
                                PlanillaM.Neto = "" + SqlMoney.Parse(row[3].ToString());

                                listaMesPlanilla.Add(PlanillaM);
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
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }

        }
        public void OnPostAccion2(string action, string idMesP)
        {
            Global.idMesPlanilla = idMesP;

            Console.WriteLine("Boton Detalles Deduccion");
            Console.WriteLine("ID Mes:" + idMesP);
            Response.Redirect("/Clientes/DetalleMesDeducciones");


            // Puedes acceder a la fila correspondiente y sus datos utilizando el valor de `id`.

        }

        public void OnPostVolverAdmin()
        {
            
            try
            {
                String SPNombre = "dbo.VolverAAdministrador";
                String connectionString = "Data Source=pruebajose2312.database.windows.net;Initial Catalog=prueba2312;Persist Security Info=True;User ID=adminjose;Password=Bases1234";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();                                      //Se abre la coneccion con la BD.
                    using (SqlCommand command = new SqlCommand(SPNombre, connection))
                    {
                        //Variables para obtener el DataSet mandados de la BD.
                        SqlDataAdapter adapter = new SqlDataAdapter();
                        DataTable table = new DataTable();

                        command.CommandType = CommandType.StoredProcedure;  //Indicar que el comando sera un SP.
                        command.Parameters.AddWithValue("@inUsuario", Global.sesion);
                        command.Parameters.AddWithValue("@inIP", Global.IP);

                        //Codigo para que detecte el output del SP.
                        SqlParameter resultCodeParam = new SqlParameter("@outResultCode", SqlDbType.Int);
                        resultCodeParam.Direction = ParameterDirection.Output;
                        command.Parameters.Add(resultCodeParam);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }

            Response.Redirect("/Clientes/UsuarioAdmin");
        }

    }
}
