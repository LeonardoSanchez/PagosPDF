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
        bool Todos = false, Cargado = false;

        public frmPagos()
        {
            InitializeComponent();
        }

        private void IniciaPantalla()
        {
            
            if(Properties.Settings.Default.PrimeraEjecucion.Equals("SI"))
            {
                MessageBox.Show(this, "Favor de configurar los parámetros antes de usar la aplicación.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                frmDBConfig Pantalla = new frmDBConfig();
                Pantalla.ShowDialog();
            }
            Cargado = true;
        }

        private void dtFecha_ValueChanged(object sender, EventArgs e)
        {
            if (Cargado)
            {
                dgPagos.DataSource = null;
                cmbNombres.DataSource = CorePagos.ObtenerNombres(dtFecha.Text);
                cmbNombres.DisplayMember = "WizardName";
            }
            checkTodos.Checked = false;
        }

        private void frmPagos_Load(object sender, EventArgs e)
        {
            IniciaPantalla();
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
            int Correctos = 0, Fallidos = 0;
            if (todos)
            {
                foreach (DataGridViewRow Pago in dgPagos.Rows)
                {
                    ReportDocument report = new ReportDocument();
                    ConnectionInfo iConnectionInfo = new ConnectionInfo();

                    /// Obteniendo informacion de la conexión a utilizar
                    iConnectionInfo.DatabaseName = Properties.Settings.Default.BaseDatos;
                    iConnectionInfo.UserID = Properties.Settings.Default.UsuarioBD;
                    iConnectionInfo.Password = Properties.Settings.Default.PasswordBD;
                    iConnectionInfo.ServerName = Properties.Settings.Default.Servidor;

                    try
                    {
                        report.Load(System.IO.Directory.GetParent(Application.ExecutablePath).ToString() + @"\Reporte\PDF Pago.rpt");
                        report.Refresh();
                    }
                    
                    catch(Exception ex)
                    {
                        MessageBox.Show(this, "Error al cargar el reporte.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

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
                        Correctos++;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, "Error al exportar PDF.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Fallidos++;
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
                    iConnectionInfo.DatabaseName = Properties.Settings.Default.BaseDatos;
                    iConnectionInfo.UserID = Properties.Settings.Default.UsuarioBD;
                    iConnectionInfo.Password = Properties.Settings.Default.PasswordBD;
                    iConnectionInfo.ServerName = Properties.Settings.Default.Servidor;

                    try
                    {
                        report.Load(System.IO.Directory.GetParent(Application.ExecutablePath).ToString() + @"\Reporte\PDF Pago.rpt");
                        report.Refresh();
                    }

                    catch (Exception ex)
                    {
                        MessageBox.Show(this, "Error al cargar el reporte.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

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
                        Correctos++;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, "Error al exportar PDF.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Fallidos++;
                    }
                }
            }
            MessageBox.Show(this, "Exportación finalizada: Correctos: " + Correctos + " Fallidos: " + Fallidos, "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnGenerar_Click(object sender, EventArgs e)
        {
            string ruta;
            FolderBrowserDialog browser = new FolderBrowserDialog();
            if (checkTodos.Checked)
            {
                if (dgPagos.Rows.Count > 0)
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
                    MessageBox.Show(this, "No existen pagos en el asistente de pago seleccionado.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
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
        }

        private void configuraciónToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmDBConfig VentanaDB = new frmDBConfig();
            VentanaDB.ShowDialog();
        }

        private void cmbNombres_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgPagos.DataSource = CorePagos.ObtenerPagos(cmbNombres.Text);
            checkTodos.Checked = false;
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

