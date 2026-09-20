
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
    partial class frmAdmitCardPrint
    {
        private Label lblTitle;
        private ComboBox cboStudent;
        private ComboBox cboTerm;
        private ComboBox cboClass;
        private CheckBox chkOverride;
        private TextBox txtReason;
        private Button btnIssue;
        private Button btnBatch;
        private Label lblStatus;

        private void InitializeComponent()
        {
            SuspendLayout();
            Text = "Admit Card Print";
            BackColor = System.Drawing.Color.FromArgb(9, 12, 23);

            lblTitle = new Label { Text = "Admit Card Designer & Print", Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold), ForeColor = System.Drawing.Color.White, Location = new Point(16, 12), AutoSize = true };

            var card = new Panel { Tag = "card", BackColor = System.Drawing.Color.FromArgb(13, 20, 48), Location = new Point(16, 55), Size = new Size(760, 320) };
            AddLabel(card, "Exam Term", 20, 20);
            cboTerm = new ComboBox { Location = new Point(20, 42), Width = 320, DropDownStyle = ComboBoxStyle.DropDownList, FlatStyle = FlatStyle.Flat };
            AddLabel(card, "Student (single issue)", 20, 85);
            cboStudent = new ComboBox { Location = new Point(20, 107), Width = 420, DropDownStyle = ComboBoxStyle.DropDownList, FlatStyle = FlatStyle.Flat };
            AddLabel(card, "Class (batch filter)", 460, 85);
            cboClass = new ComboBox { Location = new Point(460, 107), Width = 220, DropDownStyle = ComboBoxStyle.DropDownList, FlatStyle = FlatStyle.Flat };
            chkOverride = new CheckBox { Text = "Allow admin override for dues", Location = new Point(20, 155), AutoSize = true, ForeColor = System.Drawing.Color.FromArgb(250, 204, 21) };
            txtReason = new TextBox { Location = new Point(20, 185), Width = 420, PlaceholderText = "Override reason", BackColor = System.Drawing.Color.FromArgb(17, 24, 39), ForeColor = System.Drawing.Color.White, BorderStyle = BorderStyle.FixedSingle };
            btnIssue = new Button { Text = "Issue & Print Single", Location = new Point(20, 235), Size = new Size(180, 36) };
            btnIssue.Click += btnIssue_Click;
            btnBatch = new Button { Text = "Batch Admit Cards", Location = new Point(220, 235), Size = new Size(180, 36), Tag = "success" };
            btnBatch.Click += btnBatch_Click;
            card.Controls.AddRange(new Control[] { cboTerm, cboStudent, cboClass, chkOverride, txtReason, btnIssue, btnBatch });

            lblStatus = new Label { Text = "Institutional header, timetable, and barcode are embedded in the PDF admit card.", Location = new Point(16, 395), Size = new Size(760, 50), ForeColor = System.Drawing.Color.FromArgb(138, 147, 166) };

            Controls.AddRange(new Control[] { lblTitle, card, lblStatus });
            ResumeLayout(false);
        }

        private static void AddLabel(Control parent, string text, int x, int y)
        {
            Label lbl = new Label();
            lbl.Text = text;
            lbl.Location = new Point(x, y);
            lbl.AutoSize = true;
            lbl.ForeColor = System.Drawing.Color.FromArgb(138, 147, 166);
            parent.Controls.Add(lbl);
        }
    }
}
