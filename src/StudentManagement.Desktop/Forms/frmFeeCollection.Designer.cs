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
    partial class frmFeeCollection
    {
        private Label lblTitle = null!;
        private Panel pnlSearch = null!;
        private TextBox txtSearch = null!;
        private Button btnFind = null!;
        private Panel pnlBio = null!;
        private Label lblBioName = null!;
        private Label lblBioReg = null!;
        private Label lblBioClass = null!;
        private Label lblBioDue = null!;
        private Label lblBioPhone = null!;
        private Panel pnlMatrix = null!;
        private FlowLayoutPanel flpMonths = null!;
        private TextBox txtTuitionUnit = null!;
        private TextBox txtFine = null!;
        private TextBox txtWaiver = null!;
        private ComboBox cboPaymentMethod = null!;
        private TextBox txtTxnRef = null!;
        private TextBox txtRemarks = null!;
        private Label lblTuitionTotal = null!;
        private Label lblNetPayable = null!;
        private Button btnCollect = null!;

        private void InitializeComponent()
        {
            SuspendLayout();
            Text = "Fee Collection";
            AutoScroll = true;
            BackColor = UITheme.Canvas;

            lblTitle = new Label { Text = "Fee POS Collection", Font = UITheme.FontHeader, ForeColor = UITheme.TextPrimary, Location = new Point(16, 12), AutoSize = true };

            pnlSearch = new Panel { Tag = "card", BackColor = UITheme.Card, Location = new Point(16, 50), Size = new Size(960, 60) };
            txtSearch = new TextBox { Location = new Point(16, 16), Width = 360, PlaceholderText = "Reg ID or Mobile" };
            btnFind = new Button { Text = "Quick Search", Location = new Point(400, 12), Size = new Size(130, 34) };
            btnFind.Click += btnFind_Click;
            pnlSearch.Controls.AddRange(new Control[] { txtSearch, btnFind });

            pnlBio = new Panel { Tag = "card", BackColor = UITheme.Card, Location = new Point(16, 124), Size = new Size(960, 100) };
            lblBioName = new Label { Text = "—", Font = UITheme.FontHeader, Location = new Point(16, 12), AutoSize = true, ForeColor = UITheme.TextPrimary };
            lblBioReg = new Label { Text = "—", Location = new Point(16, 48), AutoSize = true, ForeColor = UITheme.TextMuted };
            lblBioClass = new Label { Text = "—", Location = new Point(220, 48), AutoSize = true, ForeColor = UITheme.TextMuted };
            lblBioPhone = new Label { Text = "—", Location = new Point(520, 48), AutoSize = true, ForeColor = UITheme.TextMuted };
            lblBioDue = new Label { Text = "—", Location = new Point(720, 20), AutoSize = true, Font = UITheme.FontSubtitle, ForeColor = UITheme.Danger };
            pnlBio.Controls.AddRange(new Control[] { lblBioName, lblBioReg, lblBioClass, lblBioPhone, lblBioDue });

            pnlMatrix = new Panel { Tag = "card", BackColor = UITheme.Card, Location = new Point(16, 240), Size = new Size(960, 140) };
            var lblMonths = new Label { Text = "Tuition Fee Months", Font = UITheme.FontSubtitle, Location = new Point(16, 10), AutoSize = true, ForeColor = UITheme.TextMuted };
            flpMonths = new FlowLayoutPanel { Location = new Point(8, 36), Size = new Size(940, 90), BackColor = UITheme.Card };
            pnlMatrix.Controls.AddRange(new Control[] { lblMonths, flpMonths });

            var pnlPay = new Panel { Tag = "card", BackColor = UITheme.Card, Location = new Point(16, 396), Size = new Size(960, 180) };
            LabelAt(pnlPay, "Unit Tuition (৳)", 16, 16); txtTuitionUnit = Tb(pnlPay, 16, 36, 120); txtTuitionUnit.Text = "0";
            LabelAt(pnlPay, "Fine (৳)", 160, 16); txtFine = Tb(pnlPay, 160, 36, 100); txtFine.Text = "0";
            LabelAt(pnlPay, "Waiver (৳)", 280, 16); txtWaiver = Tb(pnlPay, 280, 36, 100); txtWaiver.Text = "0";
            LabelAt(pnlPay, "Payment Method", 400, 16);
            cboPaymentMethod = new ComboBox { Location = new Point(400, 36), Width = 160, DropDownStyle = ComboBoxStyle.DropDownList, FlatStyle = FlatStyle.Flat };
            pnlPay.Controls.Add(cboPaymentMethod);
            LabelAt(pnlPay, "Txn Ref", 580, 16); txtTxnRef = Tb(pnlPay, 580, 36, 160);
            LabelAt(pnlPay, "Remarks", 16, 80); txtRemarks = Tb(pnlPay, 16, 100, 400);
            lblTuitionTotal = new Label { Text = "Tuition: ৳ 0.00", Location = new Point(450, 100), AutoSize = true, ForeColor = UITheme.TextMuted };
            lblNetPayable = new Label { Text = "৳ 0.00", Location = new Point(650, 90), AutoSize = true, Font = UITheme.FontTitle, ForeColor = UITheme.Success };
            btnCollect = new Button { Text = "Collect & Print Receipt", Location = new Point(750, 130), Size = new Size(180, 36), Tag = "success" };
            btnCollect.Click += btnCollect_Click;
            pnlPay.Controls.AddRange(new Control[] { lblTuitionTotal, lblNetPayable, btnCollect });

            Controls.AddRange(new Control[] { lblTitle, pnlSearch, pnlBio, pnlMatrix, pnlPay });
            ResumeLayout(false);
        }

        private static void LabelAt(Control p, string t, int x, int y) =>
            p.Controls.Add(new Label { Text = t, Location = new Point(x, y), AutoSize = true, ForeColor = UITheme.TextMuted });

        private static TextBox Tb(Control p, int x, int y, int w)
        {
            var tb = new TextBox { Location = new Point(x, y), Width = w, BackColor = UITheme.InputBack, ForeColor = UITheme.TextPrimary, BorderStyle = BorderStyle.FixedSingle };
            p.Controls.Add(tb);
            return tb;
        }
    }
}
