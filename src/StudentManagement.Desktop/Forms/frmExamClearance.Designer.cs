
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
namespace StudentManagement.Desktop.Forms
{
    partial class frmExamClearance
    {
        private Label lblTitle;
        private ComboBox cboStudent;
        private ComboBox cboTerm;
        private CheckBox chkOverride;
        private TextBox txtOverrideReason;
        private Button btnVerify;
        private Label lblResult;

        private void InitializeComponent()
        {
            SuspendLayout();
            Text = "Exam Clearance";
            BackColor = System.Drawing.Color.FromArgb(9, 12, 23);

            lblTitle = new Label { Text = "Exam Clearance Verification", Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold), ForeColor = System.Drawing.Color.White, Location = new Point(16, 12), AutoSize = true };

            var card = new Panel { Tag = "card", BackColor = System.Drawing.Color.FromArgb(13, 20, 48), Location = new Point(16, 55), Size = new Size(700, 280) };
            var lblS = new Label { Text = "Student", Location = new Point(20, 20), AutoSize = true, ForeColor = System.Drawing.Color.FromArgb(138, 147, 166) };
            cboStudent = new ComboBox { Location = new Point(20, 42), Width = 420, DropDownStyle = ComboBoxStyle.DropDownList, FlatStyle = FlatStyle.Flat };
            var lblT = new Label { Text = "Exam Term", Location = new Point(20, 85), AutoSize = true, ForeColor = System.Drawing.Color.FromArgb(138, 147, 166) };
            cboTerm = new ComboBox { Location = new Point(20, 107), Width = 420, DropDownStyle = ComboBoxStyle.DropDownList, FlatStyle = FlatStyle.Flat };
            chkOverride = new CheckBox { Text = "Admin override (allow admit card despite dues)", Location = new Point(20, 155), AutoSize = true, ForeColor = System.Drawing.Color.FromArgb(250, 204, 21) };
            txtOverrideReason = new TextBox { Location = new Point(20, 185), Width = 420, PlaceholderText = "Override reason (required when dues > 0)", BackColor = System.Drawing.Color.FromArgb(17, 24, 39), ForeColor = System.Drawing.Color.White, BorderStyle = BorderStyle.FixedSingle };
            btnVerify = new Button { Text = "Verify Clearance", Location = new Point(20, 230), Size = new Size(160, 36) };
            btnVerify.Click += btnVerify_Click;
            card.Controls.AddRange(new Control[] { lblS, cboStudent, lblT, cboTerm, chkOverride, txtOverrideReason, btnVerify });

            lblResult = new Label
            {
                Text = "Select a student and term, then verify clearance. Dues > ৳0 will block admit cards unless overridden.",
                Location = new Point(16, 355),
                Size = new Size(700, 60),
                ForeColor = System.Drawing.Color.FromArgb(138, 147, 166)
            };

            Controls.AddRange(new Control[] { lblTitle, card, lblResult });
            ResumeLayout(false);
        }
    }
}
