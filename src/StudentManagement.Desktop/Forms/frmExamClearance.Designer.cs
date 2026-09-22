using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace StudentManagement.Desktop.Forms
{
    partial class frmExamClearance
    {
        private IContainer components = null;
        private Label lblTitle;
        private Panel pnlCard;
        private Label lblStudent;
        private ComboBox cboStudent;
        private Label lblTerm;
        private ComboBox cboTerm;
        private CheckBox chkOverride;
        private TextBox txtOverrideReason;
        private Button btnVerify;
        private Label lblResult;

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
            this.lblStudent = new System.Windows.Forms.Label();
            this.cboStudent = new System.Windows.Forms.ComboBox();
            this.lblTerm = new System.Windows.Forms.Label();
            this.cboTerm = new System.Windows.Forms.ComboBox();
            this.chkOverride = new System.Windows.Forms.CheckBox();
            this.txtOverrideReason = new System.Windows.Forms.TextBox();
            this.btnVerify = new System.Windows.Forms.Button();
            this.lblResult = new System.Windows.Forms.Label();
            this.pnlCard.SuspendLayout();
            this.SuspendLayout();
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(16, 12);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(250, 25);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Exam Clearance Verification";
            this.pnlCard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(14)))), ((int)(((byte)(20)))));
            this.pnlCard.Controls.Add(this.lblStudent);
            this.pnlCard.Controls.Add(this.cboStudent);
            this.pnlCard.Controls.Add(this.lblTerm);
            this.pnlCard.Controls.Add(this.cboTerm);
            this.pnlCard.Controls.Add(this.chkOverride);
            this.pnlCard.Controls.Add(this.txtOverrideReason);
            this.pnlCard.Controls.Add(this.btnVerify);
            this.pnlCard.Location = new System.Drawing.Point(16, 55);
            this.pnlCard.Name = "pnlCard";
            this.pnlCard.Size = new System.Drawing.Size(700, 280);
            this.pnlCard.TabIndex = 1;
            this.pnlCard.Tag = "card";
            this.lblStudent.AutoSize = true;
            this.lblStudent.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(147)))), ((int)(((byte)(166)))));
            this.lblStudent.Location = new System.Drawing.Point(20, 20);
            this.lblStudent.Name = "lblStudent";
            this.lblStudent.Size = new System.Drawing.Size(48, 15);
            this.lblStudent.TabIndex = 0;
            this.lblStudent.Text = "Student";
            this.cboStudent.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStudent.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboStudent.Location = new System.Drawing.Point(20, 42);
            this.cboStudent.Name = "cboStudent";
            this.cboStudent.Size = new System.Drawing.Size(420, 23);
            this.cboStudent.TabIndex = 1;
            this.lblTerm.AutoSize = true;
            this.lblTerm.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(147)))), ((int)(((byte)(166)))));
            this.lblTerm.Location = new System.Drawing.Point(20, 85);
            this.lblTerm.Name = "lblTerm";
            this.lblTerm.Size = new System.Drawing.Size(70, 15);
            this.lblTerm.TabIndex = 2;
            this.lblTerm.Text = "Exam Term";
            this.cboTerm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTerm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboTerm.Location = new System.Drawing.Point(20, 107);
            this.cboTerm.Name = "cboTerm";
            this.cboTerm.Size = new System.Drawing.Size(420, 23);
            this.cboTerm.TabIndex = 3;
            this.chkOverride.AutoSize = true;
            this.chkOverride.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(204)))), ((int)(((byte)(21)))));
            this.chkOverride.Location = new System.Drawing.Point(20, 155);
            this.chkOverride.Name = "chkOverride";
            this.chkOverride.Size = new System.Drawing.Size(320, 19);
            this.chkOverride.TabIndex = 4;
            this.chkOverride.Text = "Admin override (allow admit card despite dues)";
            this.txtOverrideReason.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(24)))), ((int)(((byte)(39)))));
            this.txtOverrideReason.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtOverrideReason.ForeColor = System.Drawing.Color.White;
            this.txtOverrideReason.Location = new System.Drawing.Point(20, 185);
            this.txtOverrideReason.Name = "txtOverrideReason";
            this.txtOverrideReason.Size = new System.Drawing.Size(420, 23);
            this.txtOverrideReason.TabIndex = 5;
            this.btnVerify.Location = new System.Drawing.Point(20, 230);
            this.btnVerify.Name = "btnVerify";
            this.btnVerify.Size = new System.Drawing.Size(160, 36);
            this.btnVerify.TabIndex = 6;
            this.btnVerify.Text = "Verify Clearance";
            this.btnVerify.Click += new System.EventHandler(this.btnVerify_Click);
            this.lblResult.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(147)))), ((int)(((byte)(166)))));
            this.lblResult.Location = new System.Drawing.Point(16, 355);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(700, 60);
            this.lblResult.TabIndex = 2;
            this.lblResult.Text = "Select a student and term, then verify clearance. Dues will block admit cards unless overridden.";
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(14)))), ((int)(((byte)(20)))));
            this.ClientSize = new System.Drawing.Size(760, 450);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.pnlCard);
            this.Controls.Add(this.lblResult);
            this.Name = "frmExamClearance";
            this.Text = "Exam Clearance";
            this.pnlCard.ResumeLayout(false);
            this.pnlCard.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
