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
            Pagos = CorePagos.ObtenerPagos(cmbNombres.SelectedItem.ToString());


            frmviewer viewer = new frmviewer();
            ReportDocument report = new ReportDocument();
            ConnectionInfo iConnectionInfo = new ConnectionInfo();

            /// Obteniendo informacion de la conexión a utilizar
            iConnectionInfo.DatabaseName = "LuxurySAPB1";
            iConnectionInfo.UserID = "sa";
            iConnectionInfo.Password = "12";
            iConnectionInfo.ServerName = "GERARDO-PC";

            /*report.Load(System.IO.Directory.GetParent(Application.ExecutablePath).ToString() + @"\" +
                (_tipodoc.Trim() == "13" ? "Ticket.rpt" : "TicketEntrega.rpt"));*/

            //reasignando datos de conexión a reporte 
            SetDBLogonForReport(iConnectionInfo, report);

            //Asignando parametros de reporte
            report.ParameterFields["DocEntry"].CurrentValues.AddValue("HOLA");

            this.Cursor = Cursors.Default;
            //mostrando reporte

            viewer.crviewer.ReportSource = report;
            try
            {
                ExportOptions CrExportOptions;
                DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
                PdfRtfWordFormatOptions CrFormatTypeOptions = new PdfRtfWordFormatOptions();
                CrDiskFileDestinationOptions.DiskFileName = "c:\\REPORTE.pdf";
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
                MessageBox.Show(ex.ToString());
            }

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


    }
}

