using System.Drawing;
using System.Windows.Forms;

namespace StudentManagement.Desktop.Forms
{
    partial class frmStudentLedger
    {
        private Label lblTitle;
        private Label lblSelect;
        private ComboBox cboStudent;
        private Panel cardInvoiced;
        private Panel cardPaid;
        private Panel cardAdvance;
        private Panel cardDue;
        private Label lblInvoicedTitle;
        private Label lblPaidTitle;
        private Label lblAdvanceTitle;
        private Label lblDueTitle;
        private Label lblInvoiced;
        private Label lblPaid;
        private Label lblAdvance;
        private Label lblDue;
        private DataGridView dgvLedger;
        private Button btnPrint;
        private Button btnExport;

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblSelect = new System.Windows.Forms.Label();
            this.cboStudent = new System.Windows.Forms.ComboBox();
            this.cardInvoiced = new System.Windows.Forms.Panel();
            this.lblInvoicedTitle = new System.Windows.Forms.Label();
            this.lblInvoiced = new System.Windows.Forms.Label();
            this.cardPaid = new System.Windows.Forms.Panel();
            this.lblPaidTitle = new System.Windows.Forms.Label();
            this.lblPaid = new System.Windows.Forms.Label();
            this.cardAdvance = new System.Windows.Forms.Panel();
            this.lblAdvanceTitle = new System.Windows.Forms.Label();
            this.lblAdvance = new System.Windows.Forms.Label();
            this.cardDue = new System.Windows.Forms.Panel();
            this.lblDueTitle = new System.Windows.Forms.Label();
            this.lblDue = new System.Windows.Forms.Label();
            this.dgvLedger = new System.Windows.Forms.DataGridView();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.cardInvoiced.SuspendLayout();
            this.cardPaid.SuspendLayout();
            this.cardAdvance.SuspendLayout();
            this.cardDue.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLedger)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(16, 12);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(220, 25);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Financial Ledger Statement";
            // 
            // lblSelect
            // 
            this.lblSelect.AutoSize = true;
            this.lblSelect.ForeColor = System.Drawing.Color.FromArgb(138, 147, 166);
            this.lblSelect.Location = new System.Drawing.Point(16, 55);
            this.lblSelect.Name = "lblSelect";
            this.lblSelect.Size = new System.Drawing.Size(48, 15);
            this.lblSelect.TabIndex = 1;
            this.lblSelect.Text = "Student";
            // 
            // cboStudent
            // 
            this.cboStudent.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStudent.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboStudent.Location = new System.Drawing.Point(80, 52);
            this.cboStudent.Name = "cboStudent";
            this.cboStudent.Size = new System.Drawing.Size(420, 23);
            this.cboStudent.TabIndex = 2;
            this.cboStudent.SelectedIndexChanged += new System.EventHandler(this.cboStudent_SelectedIndexChanged);
            // 
            // cardInvoiced
            // 
            this.cardInvoiced.BackColor = System.Drawing.Color.FromArgb(13, 20, 48);
            this.cardInvoiced.Controls.Add(this.lblInvoicedTitle);
            this.cardInvoiced.Controls.Add(this.lblInvoiced);
            this.cardInvoiced.Location = new System.Drawing.Point(16, 95);
            this.cardInvoiced.Name = "cardInvoiced";
            this.cardInvoiced.Size = new System.Drawing.Size(220, 100);
            this.cardInvoiced.TabIndex = 3;
            this.cardInvoiced.Paint += new System.Windows.Forms.PaintEventHandler(this.Card_Paint);
            // 
            // lblInvoicedTitle
            // 
            this.lblInvoicedTitle.AutoSize = true;
            this.lblInvoicedTitle.ForeColor = System.Drawing.Color.FromArgb(138, 147, 166);
            this.lblInvoicedTitle.Location = new System.Drawing.Point(12, 12);
            this.lblInvoicedTitle.Name = "lblInvoicedTitle";
            this.lblInvoicedTitle.Size = new System.Drawing.Size(130, 15);
            this.lblInvoicedTitle.TabIndex = 0;
            this.lblInvoicedTitle.Text = "Total Invoiced (Debits)";
            // 
            // lblInvoiced
            // 
            this.lblInvoiced.AutoSize = true;
            this.lblInvoiced.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblInvoiced.ForeColor = System.Drawing.Color.FromArgb(244, 63, 94);
            this.lblInvoiced.Location = new System.Drawing.Point(12, 45);
            this.lblInvoiced.Name = "lblInvoiced";
            this.lblInvoiced.Size = new System.Drawing.Size(60, 25);
            this.lblInvoiced.TabIndex = 1;
            this.lblInvoiced.Text = "৳ 0.00";
            // 
            // cardPaid
            // 
            this.cardPaid.BackColor = System.Drawing.Color.FromArgb(13, 20, 48);
            this.cardPaid.Controls.Add(this.lblPaidTitle);
            this.cardPaid.Controls.Add(this.lblPaid);
            this.cardPaid.Location = new System.Drawing.Point(250, 95);
            this.cardPaid.Name = "cardPaid";
            this.cardPaid.Size = new System.Drawing.Size(220, 100);
            this.cardPaid.TabIndex = 4;
            this.cardPaid.Paint += new System.Windows.Forms.PaintEventHandler(this.Card_Paint);
            // 
            // lblPaidTitle
            // 
            this.lblPaidTitle.AutoSize = true;
            this.lblPaidTitle.ForeColor = System.Drawing.Color.FromArgb(138, 147, 166);
            this.lblPaidTitle.Location = new System.Drawing.Point(12, 12);
            this.lblPaidTitle.Name = "lblPaidTitle";
            this.lblPaidTitle.Size = new System.Drawing.Size(110, 15);
            this.lblPaidTitle.TabIndex = 0;
            this.lblPaidTitle.Text = "Total Paid (Credits)";
            // 
            // lblPaid
            // 
            this.lblPaid.AutoSize = true;
            this.lblPaid.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblPaid.ForeColor = System.Drawing.Color.FromArgb(16, 185, 129);
            this.lblPaid.Location = new System.Drawing.Point(12, 45);
            this.lblPaid.Name = "lblPaid";
            this.lblPaid.Size = new System.Drawing.Size(60, 25);
            this.lblPaid.TabIndex = 1;
            this.lblPaid.Text = "৳ 0.00";
            // 
            // cardAdvance
            // 
            this.cardAdvance.BackColor = System.Drawing.Color.FromArgb(13, 20, 48);
            this.cardAdvance.Controls.Add(this.lblAdvanceTitle);
            this.cardAdvance.Controls.Add(this.lblAdvance);
            this.cardAdvance.Location = new System.Drawing.Point(484, 95);
            this.cardAdvance.Name = "cardAdvance";
            this.cardAdvance.Size = new System.Drawing.Size(220, 100);
            this.cardAdvance.TabIndex = 5;
            this.cardAdvance.Paint += new System.Windows.Forms.PaintEventHandler(this.Card_Paint);
            // 
            // lblAdvanceTitle
            // 
            this.lblAdvanceTitle.AutoSize = true;
            this.lblAdvanceTitle.ForeColor = System.Drawing.Color.FromArgb(138, 147, 166);
            this.lblAdvanceTitle.Location = new System.Drawing.Point(12, 12);
            this.lblAdvanceTitle.Name = "lblAdvanceTitle";
            this.lblAdvanceTitle.Size = new System.Drawing.Size(90, 15);
            this.lblAdvanceTitle.TabIndex = 0;
            this.lblAdvanceTitle.Text = "Advance Wallet";
            // 
            // lblAdvance
            // 
            this.lblAdvance.AutoSize = true;
            this.lblAdvance.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblAdvance.ForeColor = System.Drawing.Color.FromArgb(37, 99, 235);
            this.lblAdvance.Location = new System.Drawing.Point(12, 45);
            this.lblAdvance.Name = "lblAdvance";
            this.lblAdvance.Size = new System.Drawing.Size(60, 25);
            this.lblAdvance.TabIndex = 1;
            this.lblAdvance.Text = "৳ 0.00";
            // 
            // cardDue
            // 
            this.cardDue.BackColor = System.Drawing.Color.FromArgb(13, 20, 48);
            this.cardDue.Controls.Add(this.lblDueTitle);
            this.cardDue.Controls.Add(this.lblDue);
            this.cardDue.Location = new System.Drawing.Point(718, 95);
            this.cardDue.Name = "cardDue";
            this.cardDue.Size = new System.Drawing.Size(220, 100);
            this.cardDue.TabIndex = 6;
            this.cardDue.Paint += new System.Windows.Forms.PaintEventHandler(this.Card_Paint);
            // 
            // lblDueTitle
            // 
            this.lblDueTitle.AutoSize = true;
            this.lblDueTitle.ForeColor = System.Drawing.Color.FromArgb(138, 147, 166);
            this.lblDueTitle.Location = new System.Drawing.Point(12, 12);
            this.lblDueTitle.Name = "lblDueTitle";
            this.lblDueTitle.Size = new System.Drawing.Size(100, 15);
            this.lblDueTitle.TabIndex = 0;
            this.lblDueTitle.Text = "Net Due Balance";
            // 
            // lblDue
            // 
            this.lblDue.AutoSize = true;
            this.lblDue.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblDue.ForeColor = System.Drawing.Color.FromArgb(250, 204, 21);
            this.lblDue.Location = new System.Drawing.Point(12, 45);
            this.lblDue.Name = "lblDue";
            this.lblDue.Size = new System.Drawing.Size(60, 25);
            this.lblDue.TabIndex = 1;
            this.lblDue.Text = "৳ 0.00";
            // 
            // dgvLedger
            // 
            this.dgvLedger.AllowUserToAddRows = false;
            this.dgvLedger.AllowUserToDeleteRows = false;
            this.dgvLedger.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLedger.Location = new System.Drawing.Point(16, 220);
            this.dgvLedger.Name = "dgvLedger";
            this.dgvLedger.ReadOnly = true;
            this.dgvLedger.RowTemplate.Height = 25;
            this.dgvLedger.Size = new System.Drawing.Size(960, 320);
            this.dgvLedger.TabIndex = 7;
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(16, 555);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(150, 36);
            this.btnPrint.TabIndex = 8;
            this.btnPrint.Text = "Print Statement";
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(180, 555);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(170, 36);
            this.btnExport.TabIndex = 9;
            this.btnExport.Tag = "ghost";
            this.btnExport.Text = "Export CSV / Excel";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // frmStudentLedger
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(9, 12, 23);
            this.ClientSize = new System.Drawing.Size(1000, 620);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblSelect);
            this.Controls.Add(this.cboStudent);
            this.Controls.Add(this.cardInvoiced);
            this.Controls.Add(this.cardPaid);
            this.Controls.Add(this.cardAdvance);
            this.Controls.Add(this.cardDue);
            this.Controls.Add(this.dgvLedger);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.btnExport);
            this.Name = "frmStudentLedger";
            this.Text = "Student Ledger";
            this.Load += new System.EventHandler(this.frmStudentLedger_LoadColumns);
            this.cardInvoiced.ResumeLayout(false);
            this.cardInvoiced.PerformLayout();
            this.cardPaid.ResumeLayout(false);
            this.cardPaid.PerformLayout();
            this.cardAdvance.ResumeLayout(false);
            this.cardAdvance.PerformLayout();
            this.cardDue.ResumeLayout(false);
            this.cardDue.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLedger)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void Card_Paint(object sender, PaintEventArgs e)
        {
            Panel panel = sender as Panel;
            if (panel == null)
                return;
            using (Pen pen = new Pen(System.Drawing.Color.FromArgb(36, 48, 73)))
            {
                e.Graphics.DrawRectangle(pen, 0, 0, panel.Width - 1, panel.Height - 1);
            }
        }

        private void frmStudentLedger_LoadColumns(object sender, System.EventArgs e)
        {
            if (this.dgvLedger.Columns.Count > 0)
                return;
            this.dgvLedger.Columns.Add("Date", "Date");
            this.dgvLedger.Columns.Add("Voucher", "Voucher No");
            this.dgvLedger.Columns.Add("Particulars", "Particulars / Description");
            this.dgvLedger.Columns.Add("Period", "Fee Period");
            this.dgvLedger.Columns.Add("Debit", "Debit (৳)");
            this.dgvLedger.Columns.Add("Credit", "Credit (৳)");
            this.dgvLedger.Columns.Add("Balance", "Running Balance (৳)");
            this.dgvLedger.Columns.Add("Status", "Status");
        }
    }
}
