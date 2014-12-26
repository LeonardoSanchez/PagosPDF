namespace ReporteVentasExistencias
{
    partial class frmviewer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.crviewer = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.SuspendLayout();
            // 
            // crviewer
            // 
            this.crviewer.ActiveViewIndex = -1;
            this.crviewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.crviewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.crviewer.Location = new System.Drawing.Point(0, 0);
            this.crviewer.Name = "crviewer";
            this.crviewer.SelectionFormula = "";
            this.crviewer.Size = new System.Drawing.Size(396, 445);
            this.crviewer.TabIndex = 0;
            this.crviewer.ViewTimeSelectionFormula = "";
            // 
            // frmviewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(396, 445);
            this.Controls.Add(this.crviewer);
            this.Name = "frmviewer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmviewer";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.ResumeLayout(false);

        }

        #endregion

        public CrystalDecisions.Windows.Forms.CrystalReportViewer crviewer;

    }
}