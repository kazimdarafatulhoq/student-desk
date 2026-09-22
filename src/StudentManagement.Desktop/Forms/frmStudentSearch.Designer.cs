using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace StudentManagement.Desktop.Forms
{
    partial class frmStudentSearch
    {
        private IContainer components = null;
        private Label lblTitle;
        private Panel pnlFilter;
        private ComboBox cboClass;
        private ComboBox cboSection;
        private TextBox txtQuery;
        private Button btnSearch;
        private DataGridView dgvStudents;
        private Label lblCount;
        private Button btnEdit;
        private Button btnLedger;
        private Button btnCollect;
        private Button btnAdmit;

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
            this.pnlFilter = new System.Windows.Forms.Panel();
            this.cboClass = new System.Windows.Forms.ComboBox();
            this.cboSection = new System.Windows.Forms.ComboBox();
            this.txtQuery = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.dgvStudents = new System.Windows.Forms.DataGridView();
            this.lblCount = new System.Windows.Forms.Label();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnLedger = new System.Windows.Forms.Button();
            this.btnCollect = new System.Windows.Forms.Button();
            this.btnAdmit = new System.Windows.Forms.Button();
            this.pnlFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStudents)).BeginInit();
            this.SuspendLayout();
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(16, 12);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(160, 25);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Student Directory";
            this.pnlFilter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(20)))), ((int)(((byte)(48)))));
            this.pnlFilter.Controls.Add(this.cboClass);
            this.pnlFilter.Controls.Add(this.cboSection);
            this.pnlFilter.Controls.Add(this.txtQuery);
            this.pnlFilter.Controls.Add(this.btnSearch);
            this.pnlFilter.Location = new System.Drawing.Point(16, 50);
            this.pnlFilter.Name = "pnlFilter";
            this.pnlFilter.Size = new System.Drawing.Size(960, 70);
            this.pnlFilter.TabIndex = 1;
            this.pnlFilter.Tag = "card";
            this.cboClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboClass.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboClass.Location = new System.Drawing.Point(16, 22);
            this.cboClass.Name = "cboClass";
            this.cboClass.Size = new System.Drawing.Size(160, 23);
            this.cboClass.TabIndex = 0;
            this.cboSection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSection.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboSection.Location = new System.Drawing.Point(190, 22);
            this.cboSection.Name = "cboSection";
            this.cboSection.Size = new System.Drawing.Size(140, 23);
            this.cboSection.TabIndex = 1;
            this.txtQuery.Location = new System.Drawing.Point(350, 22);
            this.txtQuery.Name = "txtQuery";
            this.txtQuery.Size = new System.Drawing.Size(320, 23);
            this.txtQuery.TabIndex = 2;
            this.btnSearch.Location = new System.Drawing.Point(690, 18);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(110, 34);
            this.btnSearch.TabIndex = 3;
            this.btnSearch.Text = "Search";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            this.dgvStudents.AllowUserToAddRows = false;
            this.dgvStudents.AllowUserToDeleteRows = false;
            this.dgvStudents.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvStudents.Location = new System.Drawing.Point(16, 140);
            this.dgvStudents.Name = "dgvStudents";
            this.dgvStudents.ReadOnly = true;
            this.dgvStudents.Size = new System.Drawing.Size(960, 360);
            this.dgvStudents.TabIndex = 2;
            this.lblCount.AutoSize = true;
            this.lblCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(147)))), ((int)(((byte)(166)))));
            this.lblCount.Location = new System.Drawing.Point(16, 515);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(70, 15);
            this.lblCount.TabIndex = 3;
            this.lblCount.Text = "0 record(s)";
            this.btnEdit.Location = new System.Drawing.Point(16, 545);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(130, 36);
            this.btnEdit.TabIndex = 4;
            this.btnEdit.Tag = "ghost";
            this.btnEdit.Text = "Edit Profile";
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            this.btnLedger.Location = new System.Drawing.Point(160, 545);
            this.btnLedger.Name = "btnLedger";
            this.btnLedger.Size = new System.Drawing.Size(180, 36);
            this.btnLedger.TabIndex = 5;
            this.btnLedger.Tag = "ghost";
            this.btnLedger.Text = "View Financial Ledger";
            this.btnLedger.Click += new System.EventHandler(this.btnLedger_Click);
            this.btnCollect.Location = new System.Drawing.Point(350, 545);
            this.btnCollect.Name = "btnCollect";
            this.btnCollect.Size = new System.Drawing.Size(130, 36);
            this.btnCollect.TabIndex = 6;
            this.btnCollect.Text = "Collect Fee";
            this.btnCollect.Click += new System.EventHandler(this.btnCollect_Click);
            this.btnAdmit.Location = new System.Drawing.Point(495, 545);
            this.btnAdmit.Name = "btnAdmit";
            this.btnAdmit.Size = new System.Drawing.Size(150, 36);
            this.btnAdmit.TabIndex = 7;
            this.btnAdmit.Tag = "success";
            this.btnAdmit.Text = "Print Admit Card";
            this.btnAdmit.Click += new System.EventHandler(this.btnAdmit_Click);
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(12)))), ((int)(((byte)(23)))));
            this.ClientSize = new System.Drawing.Size(1000, 620);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.pnlFilter);
            this.Controls.Add(this.dgvStudents);
            this.Controls.Add(this.lblCount);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnLedger);
            this.Controls.Add(this.btnCollect);
            this.Controls.Add(this.btnAdmit);
            this.Name = "frmStudentSearch";
            this.Text = "Student Search";
            this.Load += new System.EventHandler(this.frmStudentSearch_LoadColumns);
            this.pnlFilter.ResumeLayout(false);
            this.pnlFilter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStudents)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void frmStudentSearch_LoadColumns(object sender, System.EventArgs e)
        {
            if (this.dgvStudents.Columns.Count > 0)
                return;
            this.dgvStudents.Columns.Add("Avatar", "");
            this.dgvStudents.Columns.Add("RegId", "Reg ID");
            this.dgvStudents.Columns.Add("Name", "Full Name");
            this.dgvStudents.Columns.Add("Class", "Class");
            this.dgvStudents.Columns.Add("Roll", "Roll");
            this.dgvStudents.Columns.Add("Mobile", "Guardian Mobile");
            this.dgvStudents.Columns.Add("AdmDate", "Admission Date");
            this.dgvStudents.Columns.Add("Status", "Status");
            this.dgvStudents.Columns[0].Width = 40;
        }
    }
}
