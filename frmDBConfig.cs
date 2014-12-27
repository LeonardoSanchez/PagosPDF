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
            txtServidor.Text = ConfigurationManager.AppSettings["Servidor"];
            txtBD.Text = ConfigurationManager.AppSettings["BaseDatos"];
            txtUsuario.Text = ConfigurationManager.AppSettings["UsuarioBD"];
            txtPassword.Text = ConfigurationManager.AppSettings["PasswordBD"];
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            ActualizarParametros();
            this.Dispose();
        }

        public void ActualizarParametros()
        {
            Configuration Config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            Config.AppSettings.Settings["Servidor"].Value = txtServidor.Text;
            Config.AppSettings.Settings["BaseDatos"].Value = txtBD.Text;
            Config.AppSettings.Settings["UsuarioBD"].Value = txtUsuario.Text;
            Config.AppSettings.Settings["PasswordBD"].Value = txtPassword.Text;
            Config.Save(ConfigurationSaveMode.Modified);
            MessageBox.Show(this, "Configuración actualizada con exito.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
