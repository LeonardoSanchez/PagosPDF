using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace PagosPDF
{
    class Pagos
    {
        public string getConexionString()
        {
            string DatosConexion = "Data Source = " + Properties.Settings.Default.Servidor + "; Initial Catalog = " + Properties.Settings.Default.BaseDatos + "; Integrated Security = true; Connect Timeout = 5;";
            return DatosConexion;
        }
        public DataTable ObtenerNombres(string Fecha)
        {
            string Consulta = "SELECT WizardName FROM OPWZ WHERE PmntDate = @Fecha";
            DataTable DatosConsulta = new DataTable();
            
            using (SqlConnection Conexion = new SqlConnection(getConexionString()))
            {
                try
                {
                    Console.WriteLine(Conexion.ConnectionTimeout);
                    Conexion.Open();
                }

                catch(SqlException ex)
                {
                    MessageBox.Show("No se pudo conectar a la base de datos, favor de verificar los datos en Opciones -> Base de Datos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return DatosConsulta;
                }
                
                
                SqlCommand cmdConsulta = new SqlCommand(Consulta, Conexion);
                cmdConsulta.Parameters.Add("@Fecha", SqlDbType.Date);
                cmdConsulta.Parameters["@Fecha"].Value = Fecha;

                SqlDataAdapter Adapter = new SqlDataAdapter(cmdConsulta);
                Adapter.Fill(DatosConsulta);

                return DatosConsulta;
            }
        }

        public DataTable ObtenerPagos(string Nombre)
        {
            string Consulta = 
                @"SELECT DocEntry AS [Num de Pago], CardName AS [Proveedor],
                    TrsfrSum AS [Total] FROM OVPM WHERE Status = 'Y' AND JrnlMemo LIKE @Nombre";
            DataTable DatosConsulta = new DataTable();

            using (SqlConnection Conexion = new SqlConnection(getConexionString()))
            {
                Conexion.Open();

                SqlCommand cmdConsulta = new SqlCommand(Consulta, Conexion);
                cmdConsulta.Parameters.Add("@Nombre", SqlDbType.NVarChar);
                cmdConsulta.Parameters["@Nombre"].Value = "%" + Nombre + "%";

                SqlDataAdapter Adapter = new SqlDataAdapter(cmdConsulta);
                Adapter.Fill(DatosConsulta);

                return DatosConsulta;
            }
        }
    }
}
