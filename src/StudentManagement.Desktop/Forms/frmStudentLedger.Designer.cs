using System.Drawing;
using System.Windows.Forms;

namespace StudentManagement.Desktop.Forms
{
    partial class frmStudentLedger
    {
        private System.ComponentModel.IContainer components = null;
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
        private System.Windows.Forms.DataGridViewTextBoxColumn colDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVoucher;
        private System.Windows.Forms.DataGridViewTextBoxColumn colParticulars;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPeriod;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDebit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCredit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBalance;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLedStatus;
        private Button btnPrint;
        private Button btnExport;

        
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
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
            this.colDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVoucher = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colParticulars = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPeriod = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDebit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCredit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBalance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLedStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.dgvLedger.AllowUserToResizeRows = false;
            this.dgvLedger.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvLedger.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(20)))), ((int)(((byte)(48)))));
            this.dgvLedger.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.dgvLedger.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvLedger.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvLedger.ColumnHeadersHeight = 36;
            this.dgvLedger.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvLedger.EnableHeadersVisualStyles = false;
            this.dgvLedger.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(48)))), ((int)(((byte)(73)))));
            this.dgvLedger.MultiSelect = false;
            this.dgvLedger.ReadOnly = true;
            this.dgvLedger.RowHeadersVisible = false;
            this.dgvLedger.RowTemplate.Height = 28;
            this.dgvLedger.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLedger.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(16)))), ((int)(((byte)(36)))));
            this.dgvLedger.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(147)))), ((int)(((byte)(166)))));
            this.dgvLedger.ColumnHeadersDefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(16)))), ((int)(((byte)(36)))));
            this.dgvLedger.ColumnHeadersDefaultCellStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(147)))), ((int)(((byte)(166)))));
            this.dgvLedger.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(20)))), ((int)(((byte)(48)))));
            this.dgvLedger.DefaultCellStyle.ForeColor = System.Drawing.Color.White;
            this.dgvLedger.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(78)))), ((int)(((byte)(216)))));
            this.dgvLedger.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            this.dgvLedger.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(18)))), ((int)(((byte)(40)))));
            this.dgvLedger.AlternatingRowsDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            this.dgvLedger.Location = new System.Drawing.Point(16, 220);
            this.dgvLedger.Name = "dgvLedger";
            this.dgvLedger.Size = new System.Drawing.Size(960, 320);
            this.dgvLedger.TabIndex = 7;
            this.colDate.HeaderText = "Date";
            this.colDate.Name = "colDate";
            this.colDate.ReadOnly = true;
            this.colDate.FillWeight = 70F;
            this.colVoucher.HeaderText = "Voucher No";
            this.colVoucher.Name = "colVoucher";
            this.colVoucher.ReadOnly = true;
            this.colVoucher.FillWeight = 80F;
            this.colParticulars.HeaderText = "Particulars / Description";
            this.colParticulars.Name = "colParticulars";
            this.colParticulars.ReadOnly = true;
            this.colParticulars.FillWeight = 160F;
            this.colPeriod.HeaderText = "Fee Period";
            this.colPeriod.Name = "colPeriod";
            this.colPeriod.ReadOnly = true;
            this.colPeriod.FillWeight = 70F;
            this.colDebit.HeaderText = "Debit";
            this.colDebit.Name = "colDebit";
            this.colDebit.ReadOnly = true;
            this.colDebit.FillWeight = 60F;
            this.colCredit.HeaderText = "Credit";
            this.colCredit.Name = "colCredit";
            this.colCredit.ReadOnly = true;
            this.colCredit.FillWeight = 60F;
            this.colBalance.HeaderText = "Running Balance";
            this.colBalance.Name = "colBalance";
            this.colBalance.ReadOnly = true;
            this.colBalance.FillWeight = 80F;
            this.colLedStatus.HeaderText = "Status";
            this.colLedStatus.Name = "colLedStatus";
            this.colLedStatus.ReadOnly = true;
            this.colLedStatus.FillWeight = 60F;
            this.dgvLedger.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colDate,
            this.colVoucher,
            this.colParticulars,
            this.colPeriod,
            this.colDebit,
            this.colCredit,
            this.colBalance,
            this.colLedStatus});
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
    }
}
