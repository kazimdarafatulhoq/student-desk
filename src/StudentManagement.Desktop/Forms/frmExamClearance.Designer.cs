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
        private TextBox txtStudentSearch;
        private ComboBox cboStudent;
        private Label lblClass;
        private ComboBox cboClass;
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
            this.txtStudentSearch = new System.Windows.Forms.TextBox();
            this.cboStudent = new System.Windows.Forms.ComboBox();
            this.lblClass = new System.Windows.Forms.Label();
            this.cboClass = new System.Windows.Forms.ComboBox();
            this.lblTerm = new System.Windows.Forms.Label();
            this.cboTerm = new System.Windows.Forms.ComboBox();
            this.chkOverride = new System.Windows.Forms.CheckBox();
            this.txtOverrideReason = new System.Windows.Forms.TextBox();
            this.btnVerify = new System.Windows.Forms.Button();
            this.lblResult = new System.Windows.Forms.Label();
            this.pnlCard.SuspendLayout();
            this.SuspendLayout();
            //
            // lblTitle
            //
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(16, 12);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(250, 25);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Exam Clearance Verification";
            //
            // pnlCard
            //
            this.pnlCard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(20)))), ((int)(((byte)(48)))));
            this.pnlCard.Controls.Add(this.lblStudent);
            this.pnlCard.Controls.Add(this.txtStudentSearch);
            this.pnlCard.Controls.Add(this.cboStudent);
            this.pnlCard.Controls.Add(this.lblClass);
            this.pnlCard.Controls.Add(this.cboClass);
            this.pnlCard.Controls.Add(this.lblTerm);
            this.pnlCard.Controls.Add(this.cboTerm);
            this.pnlCard.Controls.Add(this.chkOverride);
            this.pnlCard.Controls.Add(this.txtOverrideReason);
            this.pnlCard.Controls.Add(this.btnVerify);
            this.pnlCard.Location = new System.Drawing.Point(16, 55);
            this.pnlCard.Name = "pnlCard";
            this.pnlCard.Size = new System.Drawing.Size(760, 330);
            this.pnlCard.TabIndex = 1;
            this.pnlCard.Tag = "card";
            //
            // lblStudent
            //
            this.lblStudent.AutoSize = true;
            this.lblStudent.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(147)))), ((int)(((byte)(166)))));
            this.lblStudent.Location = new System.Drawing.Point(20, 20);
            this.lblStudent.Name = "lblStudent";
            this.lblStudent.Size = new System.Drawing.Size(200, 15);
            this.lblStudent.TabIndex = 0;
            this.lblStudent.Text = "Student (search name / REG / phone)";
            //
            // txtStudentSearch
            //
            this.txtStudentSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(24)))), ((int)(((byte)(39)))));
            this.txtStudentSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtStudentSearch.ForeColor = System.Drawing.Color.White;
            this.txtStudentSearch.Location = new System.Drawing.Point(20, 42);
            this.txtStudentSearch.Name = "txtStudentSearch";
            this.txtStudentSearch.PlaceholderText = "Type to filter student list…";
            this.txtStudentSearch.Size = new System.Drawing.Size(420, 23);
            this.txtStudentSearch.TabIndex = 1;
            this.txtStudentSearch.TextChanged += new System.EventHandler(this.txtStudentSearch_TextChanged);
            //
            // cboStudent
            //
            this.cboStudent.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStudent.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboStudent.Location = new System.Drawing.Point(20, 72);
            this.cboStudent.Name = "cboStudent";
            this.cboStudent.Size = new System.Drawing.Size(420, 23);
            this.cboStudent.TabIndex = 2;
            //
            // lblClass
            //
            this.lblClass.AutoSize = true;
            this.lblClass.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(147)))), ((int)(((byte)(166)))));
            this.lblClass.Location = new System.Drawing.Point(460, 20);
            this.lblClass.Name = "lblClass";
            this.lblClass.Size = new System.Drawing.Size(90, 15);
            this.lblClass.TabIndex = 3;
            this.lblClass.Text = "Class filter";
            //
            // cboClass
            //
            this.cboClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboClass.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboClass.Location = new System.Drawing.Point(460, 42);
            this.cboClass.Name = "cboClass";
            this.cboClass.Size = new System.Drawing.Size(260, 23);
            this.cboClass.TabIndex = 4;
            this.cboClass.SelectedIndexChanged += new System.EventHandler(this.cboClass_SelectedIndexChanged);
            //
            // lblTerm
            //
            this.lblTerm.AutoSize = true;
            this.lblTerm.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(147)))), ((int)(((byte)(166)))));
            this.lblTerm.Location = new System.Drawing.Point(20, 115);
            this.lblTerm.Name = "lblTerm";
            this.lblTerm.Size = new System.Drawing.Size(70, 15);
            this.lblTerm.TabIndex = 5;
            this.lblTerm.Text = "Exam Term";
            //
            // cboTerm
            //
            this.cboTerm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTerm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboTerm.Location = new System.Drawing.Point(20, 137);
            this.cboTerm.Name = "cboTerm";
            this.cboTerm.Size = new System.Drawing.Size(420, 23);
            this.cboTerm.TabIndex = 6;
            //
            // chkOverride
            //
            this.chkOverride.AutoSize = true;
            this.chkOverride.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(204)))), ((int)(((byte)(21)))));
            this.chkOverride.Location = new System.Drawing.Point(20, 185);
            this.chkOverride.Name = "chkOverride";
            this.chkOverride.Size = new System.Drawing.Size(320, 19);
            this.chkOverride.TabIndex = 7;
            this.chkOverride.Text = "Admin override (allow admit card despite dues)";
            //
            // txtOverrideReason
            //
            this.txtOverrideReason.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(24)))), ((int)(((byte)(39)))));
            this.txtOverrideReason.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtOverrideReason.ForeColor = System.Drawing.Color.White;
            this.txtOverrideReason.Location = new System.Drawing.Point(20, 215);
            this.txtOverrideReason.Name = "txtOverrideReason";
            this.txtOverrideReason.PlaceholderText = "Override reason (required when override is checked)";
            this.txtOverrideReason.Size = new System.Drawing.Size(700, 23);
            this.txtOverrideReason.TabIndex = 8;
            //
            // btnVerify
            //
            this.btnVerify.Location = new System.Drawing.Point(20, 265);
            this.btnVerify.Name = "btnVerify";
            this.btnVerify.Size = new System.Drawing.Size(160, 36);
            this.btnVerify.TabIndex = 9;
            this.btnVerify.Text = "Verify Clearance";
            this.btnVerify.Click += new System.EventHandler(this.btnVerify_Click);
            //
            // lblResult
            //
            this.lblResult.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(147)))), ((int)(((byte)(166)))));
            this.lblResult.Location = new System.Drawing.Point(16, 400);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(760, 60);
            this.lblResult.TabIndex = 2;
            this.lblResult.Text = "Search or filter by class, pick a student and term, then verify clearance.";
            //
            // frmExamClearance
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(12)))), ((int)(((byte)(23)))));
            this.ClientSize = new System.Drawing.Size(820, 480);
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
