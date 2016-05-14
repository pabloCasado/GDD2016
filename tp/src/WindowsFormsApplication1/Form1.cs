using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApplication1;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var connection = new DBConnection().openConnection();

            SqlCommand command = new SqlCommand("HARDCOR.login", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@username", this.textBox1.Text));
            command.Parameters.Add(new SqlParameter("@password", this.textBox2.Text));
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            if (!reader.HasRows)  // El usuario no existe
                MessageBox.Show("El usuario " + this.textBox1.Text + " no está registrado en el sistema",
                    "Error al iniciar sesión", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
                while (reader.Read())
                {
                    if (!(bool)reader["login_valido"])
                    {
                        string message;
                        if ((bool)reader["habilitado"])
                            message = "La contraseña es incorrecta. Tiene " + (3 - (Int32.Parse(reader["intentos"].ToString()))) + " intentos disponibles";
                        else
                            message = "Su usuario ha sido bloqueado";

                        MessageBox.Show(message, "Error al iniciar sesión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
                    //Código a ejecutar si el login es válido <inserte aquí>
                }
            reader.Close();
            connection.Close();
        }
    }
}
