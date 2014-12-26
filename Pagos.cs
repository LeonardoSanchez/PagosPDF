using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace PagosPDF
{
    class Pagos
    {
        string DatosConexion = "Data Source = localhost; Initial Catalog = LuxurySAPB1; Integrated Security = true;";

        public DataTable ObtenerNombres(string Fecha)
        {
            string Consulta = "SELECT WizardName FROM OPWZ WHERE PmntDate = @Fecha";
            DataTable DatosConsulta = new DataTable();
            
            using (SqlConnection Conexion = new SqlConnection(DatosConexion))
            {
                Conexion.Open();
                
                SqlCommand cmdConsulta = new SqlCommand(Consulta, Conexion);
                cmdConsulta.Parameters.Add("@Fecha", SqlDbType.Date);
                cmdConsulta.Parameters["@Fecha"].Value = Fecha;

                SqlDataAdapter Adapter = new SqlDataAdapter(cmdConsulta);
                Adapter.Fill(DatosConsulta);
                if (DatosConsulta.Rows.Count > 0)
                    Console.WriteLine("Hay datos");

                return DatosConsulta;
            }
        }

        public DataTable ObtenerPagos(string Nombre)
        {
            string Consulta = "SELECT DocEntry FROM OVPM WHERE Status = 'Y' AND JrnlMemo LIKE @Nombre";
            DataTable DatosConsulta = new DataTable();

            using (SqlConnection Conexion = new SqlConnection(DatosConexion))
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
