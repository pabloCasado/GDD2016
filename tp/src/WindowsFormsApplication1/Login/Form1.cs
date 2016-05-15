﻿using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using WindowsFormsApplication1.Coneccion;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void toggle_group_boxes()
        {
            this.groupBox1.Enabled = !this.groupBox1.Enabled;
            this.groupBox2.Enabled = !this.groupBox2.Enabled;
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
                    if(!this.comboBox1.Items.Contains(reader["nombre"].ToString()))
                    {
                        this.comboBox1.Items.Add(reader["nombre"].ToString());
                        this.comboBox1.DisplayMember = reader["nombre"].ToString();
                        this.comboBox1.ValueMember = reader["cod_rol"].ToString();
                    }
                }

            reader.Close();
            connection.Close();

            if (this.comboBox1.Items.Count > 1)
                this.toggle_group_boxes();
            else
                ;//Hay un sólo rol
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(this.comboBox1.SelectedIndex < 0)
            {
                MessageBox.Show("Debe seleccionar un rol");
            }
            //Hay varios roles pero ya eligieron uno
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.textBox1.Text = this.textBox2.Text = "";
            this.comboBox1.Items.Clear();
            this.toggle_group_boxes();
        }
    }
}
