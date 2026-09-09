using StudentManagement.Desktop.Theme;

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
    partial class frmStudentLedger
    {
        private Label lblTitle = null!;
        private ComboBox cboStudent = null!;
        private Panel cardInvoiced = null!;
        private Panel cardPaid = null!;
        private Panel cardAdvance = null!;
        private Panel cardDue = null!;
        private Label lblInvoiced = null!;
        private Label lblPaid = null!;
        private Label lblAdvance = null!;
        private Label lblDue = null!;
        private DataGridView dgvLedger = null!;
        private Button btnPrint = null!;
        private Button btnExport = null!;

        private void InitializeComponent()
        {
            SuspendLayout();
            Text = "Student Ledger";
            BackColor = UITheme.Canvas;

            lblTitle = new Label { Text = "Financial Ledger Statement", Font = UITheme.FontHeader, ForeColor = UITheme.TextPrimary, Location = new Point(16, 12), AutoSize = true };
            var lblSelect = new Label { Text = "Student", Location = new Point(16, 55), AutoSize = true, ForeColor = UITheme.TextMuted };
            cboStudent = new ComboBox { Location = new Point(80, 52), Width = 420, DropDownStyle = ComboBoxStyle.DropDownList, FlatStyle = FlatStyle.Flat };
            cboStudent.SelectedIndexChanged += cboStudent_SelectedIndexChanged;

            cardInvoiced = Stat("Total Invoiced (Debits)", out lblInvoiced, UITheme.Danger, new Point(16, 95));
            cardPaid = Stat("Total Paid (Credits)", out lblPaid, UITheme.Success, new Point(250, 95));
            cardAdvance = Stat("Advance Wallet", out lblAdvance, UITheme.Primary, new Point(484, 95));
            cardDue = Stat("Net Due Balance", out lblDue, UITheme.Warning, new Point(718, 95));

            dgvLedger = new DataGridView { Location = new Point(16, 220), Size = new Size(960, 320), ReadOnly = true };
            dgvLedger.Columns.Add("Date", "Date");
            dgvLedger.Columns.Add("Voucher", "Voucher No");
            dgvLedger.Columns.Add("Particulars", "Particulars / Description");
            dgvLedger.Columns.Add("Period", "Fee Period");
            dgvLedger.Columns.Add("Debit", "Debit (৳)");
            dgvLedger.Columns.Add("Credit", "Credit (৳)");
            dgvLedger.Columns.Add("Balance", "Running Balance (৳)");
            dgvLedger.Columns.Add("Status", "Status");

            btnPrint = new Button { Text = "Print Statement", Location = new Point(16, 555), Size = new Size(150, 36) };
            btnPrint.Click += btnPrint_Click;
            btnExport = new Button { Text = "Export CSV / Excel", Location = new Point(180, 555), Size = new Size(170, 36), Tag = "ghost" };
            btnExport.Click += btnExport_Click;

            Controls.AddRange(new Control[] { lblTitle, lblSelect, cboStudent, cardInvoiced, cardPaid, cardAdvance, cardDue, dgvLedger, btnPrint, btnExport });
            ResumeLayout(false);
        }

        private static Panel Stat(string title, out Label value, Color color, Point loc)
        {
            var p = new Panel { Tag = "card", BackColor = UITheme.Card, Location = loc, Size = new Size(220, 100) };
            p.Paint += (_, e) =>
            {
                using var pen = new Pen(UITheme.Border);
                e.Graphics.DrawRectangle(pen, 0, 0, p.Width - 1, p.Height - 1);
            };
            var t = new Label { Text = title, Location = new Point(12, 12), AutoSize = true, ForeColor = UITheme.TextMuted };
            value = new Label { Text = "৳ 0.00", Location = new Point(12, 45), AutoSize = true, Font = UITheme.FontHeader, ForeColor = color };
            p.Controls.AddRange(new Control[] { t, value });
            return p;
        }
    }
}
