using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace UpdateBDFromExcel
{
    public class SqlDatabase
    {
        // Se ha corregido 'Source' a 'Data Source' y se ha añadido un marcador de posición para la contraseña.
        // Asegúrate de reemplazar 'YOUR_PASSWORD' con la contraseña real de la base de datos.
        private string connectionString = "Data Source=172.190.120.3;Encrypt=False;Integrated Security=False;User ID=sa;Password=YOUR_PASSWORD";

        public SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        // Non-UI method that attempts to open the connection and returns success state and error message.
        public bool TryOpenConnection(out string errorMessage)
        {
            errorMessage = null;
            using (var connection = GetConnection())
            {
                try
                {
                    connection.Open();
                    return true;
                }
                catch (Exception ex)
                {
                    errorMessage = ex.Message;
                    return false;
                }
            }
        }

        // Existing UI-flavored method kept for compatibility; it delegates to TryOpenConnection.
        public void TestConnection()
        {
            if (TryOpenConnection(out var err))
            {
                MessageBox.Show("Conexión exitosa a la base de datos.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show($"Error al conectar a la base de datos: {err}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
