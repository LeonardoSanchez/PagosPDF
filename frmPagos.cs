using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
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
    public partial class frmPagos : Form
    {
        Pagos CorePagos = new Pagos();
        DataTable Pagos = new DataTable();
        bool Todos = false;

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
            dgPagos.DataSource = null;
            cmbNombres.DataSource = CorePagos.ObtenerNombres(dtFecha.Text);
        }

        public void PrimeraEjecucion()
        {
            string Ejecucion = ConfigurationManager.AppSettings["FirstTimeRunning"];
            if(Ejecucion.Equals("Si"))
            {
                MessageBox.Show(this, "Favor de configurar los parámetros antes de usar la aplicación.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                frmDBConfig Pantalla = new frmDBConfig();
                Pantalla.ShowDialog();
            }
        }

        private void frmPagos_Load(object sender, EventArgs e)
        {
            IniciaPantalla();
            PrimeraEjecucion();
        }

        private void SetDBLogonForReport(ConnectionInfo myConnectionInfo, ReportDocument myReportDocument)
        {
            //funcion para reasignar datos de conexión a un reporte seleccionado

            Tables myTables = myReportDocument.Database.Tables;

            foreach (CrystalDecisions.CrystalReports.Engine.Table myTable in myTables)
            {
                TableLogOnInfo myTableLogonInfo = myTable.LogOnInfo;
                myTableLogonInfo.ConnectionInfo = myConnectionInfo;
                myTable.ApplyLogOnInfo(myTableLogonInfo);
            }

            if (myReportDocument.Subreports.Count > 0)
            {
                CrystalDecisions.CrystalReports.Engine.ReportDocument subRptDoc;
                CrystalDecisions.CrystalReports.Engine.Subreports subReports;
                Tables CrTables;
                TableLogOnInfo crtableLogoninfo;

                subReports = myReportDocument.Subreports;

                for (int iCount = 0; iCount < subReports.Count; iCount++)
                {
                    subRptDoc = subReports[iCount];
                    CrTables = subRptDoc.Database.Tables;

                    foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables)
                    {
                        crtableLogoninfo = CrTable.LogOnInfo;
                        crtableLogoninfo.ConnectionInfo = myConnectionInfo;
                        CrTable.ApplyLogOnInfo(crtableLogoninfo);
                    }
                }
            }
        }

        public void GeneraPDF(string ruta, bool todos)
        {
            if (todos)
            {
                foreach (DataGridViewRow Pago in dgPagos.Rows)
                {
                    ReportDocument report = new ReportDocument();
                    ConnectionInfo iConnectionInfo = new ConnectionInfo();

                    /// Obteniendo informacion de la conexión a utilizar
                    iConnectionInfo.DatabaseName = ConfigurationManager.AppSettings["BaseDatos"];
                    iConnectionInfo.UserID = ConfigurationManager.AppSettings["UsuarioBD"];
                    iConnectionInfo.Password = ConfigurationManager.AppSettings["PasswordBD"];
                    iConnectionInfo.ServerName = ConfigurationManager.AppSettings["Servidor"];

                    report.Load(System.IO.Directory.GetParent(Application.ExecutablePath).ToString() + @"\" +
                        ("PDF Pago.rpt"));
                    report.Refresh();

                    //reasignando datos de conexión a reporte 
                    SetDBLogonForReport(iConnectionInfo, report);

                    //Asignando parametros de reporte
                    int numPago = Convert.ToInt32(Pago.Cells["Num de Pago"].Value);
                    report.ParameterFields["npago"].CurrentValues.AddValue(numPago);


                    this.Cursor = Cursors.Default;
                    try
                    {
                        ExportOptions CrExportOptions;
                        DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
                        PdfRtfWordFormatOptions CrFormatTypeOptions = new PdfRtfWordFormatOptions();
                        CrDiskFileDestinationOptions.DiskFileName = ruta + "/" + numPago.ToString() + ".pdf";
                        CrExportOptions = report.ExportOptions;
                        {
                            CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                            CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                            CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                            CrExportOptions.FormatOptions = CrFormatTypeOptions;
                        }
                        report.Export();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Fallo conexion con la base de datos", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                foreach (DataGridViewRow Pago in dgPagos.SelectedRows)
                {
                    ReportDocument report = new ReportDocument();
                    ConnectionInfo iConnectionInfo = new ConnectionInfo();

                    /// Obteniendo informacion de la conexión a utilizar
                    iConnectionInfo.DatabaseName = ConfigurationManager.AppSettings["BaseDatos"];
                    iConnectionInfo.UserID = ConfigurationManager.AppSettings["UsuarioBD"];
                    iConnectionInfo.Password = ConfigurationManager.AppSettings["PasswordBD"];
                    iConnectionInfo.ServerName = ConfigurationManager.AppSettings["Servidor"];

                    report.Load(System.IO.Directory.GetParent(Application.ExecutablePath).ToString() + @"\" +
                        ("PDF Pago.rpt"));
                    report.Refresh();

                    //reasignando datos de conexión a reporte 
                    SetDBLogonForReport(iConnectionInfo, report);

                    //Asignando parametros de reporte
                    int numPago = Convert.ToInt32(Pago.Cells["Num de Pago"].Value);
                    report.ParameterFields["npago"].CurrentValues.AddValue(numPago);


                    this.Cursor = Cursors.Default;
                    try
                    {
                        ExportOptions CrExportOptions;
                        DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
                        PdfRtfWordFormatOptions CrFormatTypeOptions = new PdfRtfWordFormatOptions();
                        CrDiskFileDestinationOptions.DiskFileName = ruta + "/" + numPago.ToString() + ".pdf";
                        CrExportOptions = report.ExportOptions;
                        {
                            CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                            CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                            CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                            CrExportOptions.FormatOptions = CrFormatTypeOptions;
                        }
                        report.Export();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Fallo conexion con la base de datos", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnGenerar_Click(object sender, EventArgs e)
        {
            string ruta;
            FolderBrowserDialog browser = new FolderBrowserDialog();
            if (dgPagos.SelectedRows.Count > 0)
            {
                if (browser.ShowDialog() == DialogResult.OK)
                {
                    ruta = browser.SelectedPath;

                    Pagos = CorePagos.ObtenerPagos(cmbNombres.Text);

                    GeneraPDF(ruta, Todos);
                }
            }
            else
            {
                MessageBox.Show(this, "Favor de seleccionar al menos un pago.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void configuraciónToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmDBConfig VentanaDB = new frmDBConfig();
            VentanaDB.ShowDialog();
        }

        private void cmbNombres_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgPagos.DataSource = CorePagos.ObtenerPagos(cmbNombres.Text);
        }

        private void checkTodos_CheckedChanged(object sender, EventArgs e)
        {
            if(checkTodos.Checked)
            {
                Todos = true;
            }
            else
            {
                Todos = false;
            }
        }
    }
}

