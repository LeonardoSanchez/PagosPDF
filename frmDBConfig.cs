using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;

namespace PagosPDF
{
    public partial class frmDBConfig : Form
    {
        public frmDBConfig()
        {
            InitializeComponent();
        }

        private void frmDBConfig_Load(object sender, EventArgs e)
        {
            IniciaPantalla();
        }

        public void IniciaPantalla()
        {
            CargaParametros();
        }

        public void CargaParametros()
        {
            txtServidor.Text = Properties.Settings.Default.Servidor;
            txtBD.Text = Properties.Settings.Default.BaseDatos;
            txtUsuario.Text = Properties.Settings.Default.UsuarioBD;
            txtPassword.Text = Properties.Settings.Default.PasswordBD;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        public bool VerificarVacio(Form Formulario)
        {
            foreach(Control Objeto in Formulario.Controls)
            {
                if(Objeto is TextBox)
                {
                    TextBox txt = Objeto as TextBox;
                    if(txt.Text == string.Empty)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if(VerificarVacio(this))
            {
                MessageBox.Show(this, "Favor de no dejar campos vacíos.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                ActualizarParametros();
                this.Dispose();
            }
        }

        public void ActualizarParametros()
        {
            Properties.Settings.Default.Servidor = txtServidor.Text;
            Properties.Settings.Default.BaseDatos = txtBD.Text;
            Properties.Settings.Default.UsuarioBD = txtUsuario.Text;
            Properties.Settings.Default.PasswordBD = txtPassword.Text;
            if (Properties.Settings.Default.PrimeraEjecucion.Equals("SI"))
            {
                Properties.Settings.Default.PrimeraEjecucion = "NO";
            }
            Properties.Settings.Default.Save();
            MessageBox.Show(this, "Configuración actualizada con exito.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
