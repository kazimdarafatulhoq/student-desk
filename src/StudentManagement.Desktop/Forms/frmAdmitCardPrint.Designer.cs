using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace StudentManagement.Desktop.Forms
{
    partial class frmAdmitCardPrint
    {
        private IContainer components = null;
        private Label lblTitle;
        private Panel pnlCard;
        private Label lblTerm;
        private ComboBox cboTerm;
        private Label lblStudent;
        private ComboBox cboStudent;
        private Label lblClass;
        private ComboBox cboClass;
        private CheckBox chkOverride;
        private TextBox txtReason;
        private Button btnIssue;
        private Button btnBatch;
        private Label lblStatus;

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
            this.pnlCard = new System.Windows.Forms.Panel();
            this.lblTerm = new System.Windows.Forms.Label();
            this.cboTerm = new System.Windows.Forms.ComboBox();
            this.lblStudent = new System.Windows.Forms.Label();
            this.cboStudent = new System.Windows.Forms.ComboBox();
            this.lblClass = new System.Windows.Forms.Label();
            this.cboClass = new System.Windows.Forms.ComboBox();
            this.chkOverride = new System.Windows.Forms.CheckBox();
            this.txtReason = new System.Windows.Forms.TextBox();
            this.btnIssue = new System.Windows.Forms.Button();
            this.btnBatch = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.pnlCard.SuspendLayout();
            this.SuspendLayout();
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(16, 12);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(260, 25);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Admit Card Designer & Print";
            this.pnlCard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(14)))), ((int)(((byte)(20)))));
            this.pnlCard.Controls.Add(this.lblTerm);
            this.pnlCard.Controls.Add(this.cboTerm);
            this.pnlCard.Controls.Add(this.lblStudent);
            this.pnlCard.Controls.Add(this.cboStudent);
            this.pnlCard.Controls.Add(this.lblClass);
            this.pnlCard.Controls.Add(this.cboClass);
            this.pnlCard.Controls.Add(this.chkOverride);
            this.pnlCard.Controls.Add(this.txtReason);
            this.pnlCard.Controls.Add(this.btnIssue);
            this.pnlCard.Controls.Add(this.btnBatch);
            this.pnlCard.Location = new System.Drawing.Point(16, 55);
            this.pnlCard.Name = "pnlCard";
            this.pnlCard.Size = new System.Drawing.Size(760, 320);
            this.pnlCard.TabIndex = 1;
            this.pnlCard.Tag = "card";
            this.lblTerm.AutoSize = true;
            this.lblTerm.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(147)))), ((int)(((byte)(166)))));
            this.lblTerm.Location = new System.Drawing.Point(20, 20);
            this.lblTerm.Name = "lblTerm";
            this.lblTerm.Size = new System.Drawing.Size(70, 15);
            this.lblTerm.TabIndex = 0;
            this.lblTerm.Text = "Exam Term";
            this.cboTerm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTerm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboTerm.Location = new System.Drawing.Point(20, 42);
            this.cboTerm.Name = "cboTerm";
            this.cboTerm.Size = new System.Drawing.Size(320, 23);
            this.cboTerm.TabIndex = 1;
            this.lblStudent.AutoSize = true;
            this.lblStudent.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(147)))), ((int)(((byte)(166)))));
            this.lblStudent.Location = new System.Drawing.Point(20, 85);
            this.lblStudent.Name = "lblStudent";
            this.lblStudent.Size = new System.Drawing.Size(130, 15);
            this.lblStudent.TabIndex = 2;
            this.lblStudent.Text = "Student (single issue)";
            this.cboStudent.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStudent.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboStudent.Location = new System.Drawing.Point(20, 107);
            this.cboStudent.Name = "cboStudent";
            this.cboStudent.Size = new System.Drawing.Size(420, 23);
            this.cboStudent.TabIndex = 3;
            this.lblClass.AutoSize = true;
            this.lblClass.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(147)))), ((int)(((byte)(166)))));
            this.lblClass.Location = new System.Drawing.Point(460, 85);
            this.lblClass.Name = "lblClass";
            this.lblClass.Size = new System.Drawing.Size(110, 15);
            this.lblClass.TabIndex = 4;
            this.lblClass.Text = "Class (batch filter)";
            this.cboClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboClass.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboClass.Location = new System.Drawing.Point(460, 107);
            this.cboClass.Name = "cboClass";
            this.cboClass.Size = new System.Drawing.Size(220, 23);
            this.cboClass.TabIndex = 5;
            this.chkOverride.AutoSize = true;
            this.chkOverride.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(204)))), ((int)(((byte)(21)))));
            this.chkOverride.Location = new System.Drawing.Point(20, 155);
            this.chkOverride.Name = "chkOverride";
            this.chkOverride.Size = new System.Drawing.Size(220, 19);
            this.chkOverride.TabIndex = 6;
            this.chkOverride.Text = "Allow admin override for dues";
            this.txtReason.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(24)))), ((int)(((byte)(39)))));
            this.txtReason.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtReason.ForeColor = System.Drawing.Color.White;
            this.txtReason.Location = new System.Drawing.Point(20, 185);
            this.txtReason.Name = "txtReason";
            this.txtReason.Size = new System.Drawing.Size(420, 23);
            this.txtReason.TabIndex = 7;
            this.btnIssue.Location = new System.Drawing.Point(20, 235);
            this.btnIssue.Name = "btnIssue";
            this.btnIssue.Size = new System.Drawing.Size(180, 36);
            this.btnIssue.TabIndex = 8;
            this.btnIssue.Text = "Issue & Print Single";
            this.btnIssue.Click += new System.EventHandler(this.btnIssue_Click);
            this.btnBatch.Location = new System.Drawing.Point(220, 235);
            this.btnBatch.Name = "btnBatch";
            this.btnBatch.Size = new System.Drawing.Size(180, 36);
            this.btnBatch.TabIndex = 9;
            this.btnBatch.Tag = "success";
            this.btnBatch.Text = "Batch Admit Cards";
            this.btnBatch.Click += new System.EventHandler(this.btnBatch_Click);
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(147)))), ((int)(((byte)(166)))));
            this.lblStatus.Location = new System.Drawing.Point(16, 395);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(760, 50);
            this.lblStatus.TabIndex = 2;
            this.lblStatus.Text = "Institutional header, timetable, and barcode are embedded in the PDF admit card.";
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(14)))), ((int)(((byte)(20)))));
            this.ClientSize = new System.Drawing.Size(820, 480);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.pnlCard);
            this.Controls.Add(this.lblStatus);
            this.Name = "frmAdmitCardPrint";
            this.Text = "Admit Card Print";
            this.pnlCard.ResumeLayout(false);
            this.pnlCard.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
