using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PagosPDF
{
    public partial class frmPagos : Form
    {
        Pagos CorePagos = new Pagos();
        DataTable Pagos = new DataTable();

        public frmPagos()
        {
            InitializeComponent();
        }

        private void IniciaPantalla()
        {
            cmbNombres.DataSource = CorePagos.ObtenerNombres(dtFecha.Text);
            cmbNombres.DisplayMember = "WizardName";
        }

        private void dtFecha_ValueChanged(object sender, EventArgs e)
        {
            cmbNombres.DataSource = CorePagos.ObtenerNombres(dtFecha.Text);
        }

        private void frmPagos_Load(object sender, EventArgs e)
        {
            IniciaPantalla();
        }

        private void btnGenerar_Click(object sender, EventArgs e)
        {
            Pagos = CorePagos.ObtenerPagos(cmbNombres.Text);
        }
    }
}
