
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
        private Label lblTitle;
        private Panel pnlSearch;
        private TextBox txtSearch;
        private Button btnFind;
        private Panel pnlBio;
        private Label lblBioName;
        private Label lblBioReg;
        private Label lblBioClass;
        private Label lblBioDue;
        private Label lblBioPhone;
        private Panel pnlMatrix;
        private FlowLayoutPanel flpMonths;
        private TextBox txtTuitionUnit;
        private TextBox txtFine;
        private TextBox txtWaiver;
        private ComboBox cboPaymentMethod;
        private TextBox txtTxnRef;
        private TextBox txtRemarks;
        private Label lblTuitionTotal;
        private Label lblNetPayable;
        private Button btnCollect;

        private void InitializeComponent()
        {
            SuspendLayout();
            Text = "Fee Collection";
            AutoScroll = true;
            BackColor = System.Drawing.Color.FromArgb(9, 12, 23);

            lblTitle = new Label { Text = "Fee Collection Counter", Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold), ForeColor = System.Drawing.Color.White, Location = new Point(16, 12), AutoSize = true };

            pnlSearch = new Panel { Tag = "card", BackColor = System.Drawing.Color.FromArgb(13, 20, 48), Location = new Point(16, 50), Size = new Size(960, 60) };
            txtSearch = new TextBox { Location = new Point(16, 16), Width = 360, PlaceholderText = "Reg ID or Mobile" };
            btnFind = new Button { Text = "Quick Search", Location = new Point(400, 12), Size = new Size(130, 34) };
            btnFind.Click += btnFind_Click;
            pnlSearch.Controls.AddRange(new Control[] { txtSearch, btnFind });

            pnlBio = new Panel { Tag = "card", BackColor = System.Drawing.Color.FromArgb(13, 20, 48), Location = new Point(16, 124), Size = new Size(960, 100) };
            lblBioName = new Label { Text = "—", Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold), Location = new Point(16, 12), AutoSize = true, ForeColor = System.Drawing.Color.White };
            lblBioReg = new Label { Text = "—", Location = new Point(16, 48), AutoSize = true, ForeColor = System.Drawing.Color.FromArgb(138, 147, 166) };
            lblBioClass = new Label { Text = "—", Location = new Point(220, 48), AutoSize = true, ForeColor = System.Drawing.Color.FromArgb(138, 147, 166) };
            lblBioPhone = new Label { Text = "—", Location = new Point(520, 48), AutoSize = true, ForeColor = System.Drawing.Color.FromArgb(138, 147, 166) };
            lblBioDue = new Label { Text = "—", Location = new Point(720, 20), AutoSize = true, Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold), ForeColor = System.Drawing.Color.FromArgb(244, 63, 94) };
            pnlBio.Controls.AddRange(new Control[] { lblBioName, lblBioReg, lblBioClass, lblBioPhone, lblBioDue });

            pnlMatrix = new Panel { Tag = "card", BackColor = System.Drawing.Color.FromArgb(13, 20, 48), Location = new Point(16, 240), Size = new Size(960, 140) };
            var lblMonths = new Label { Text = "Tuition Fee Months", Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold), Location = new Point(16, 10), AutoSize = true, ForeColor = System.Drawing.Color.FromArgb(138, 147, 166) };
            flpMonths = new FlowLayoutPanel { Location = new Point(8, 36), Size = new Size(940, 90), BackColor = System.Drawing.Color.FromArgb(13, 20, 48) };
            pnlMatrix.Controls.AddRange(new Control[] { lblMonths, flpMonths });

            var pnlPay = new Panel { Tag = "card", BackColor = System.Drawing.Color.FromArgb(13, 20, 48), Location = new Point(16, 396), Size = new Size(960, 180) };
            LabelAt(pnlPay, "Unit Tuition (৳)", 16, 16); txtTuitionUnit = Tb(pnlPay, 16, 36, 120); txtTuitionUnit.Text = "0";
            LabelAt(pnlPay, "Fine (৳)", 160, 16); txtFine = Tb(pnlPay, 160, 36, 100); txtFine.Text = "0";
            LabelAt(pnlPay, "Waiver (৳)", 280, 16); txtWaiver = Tb(pnlPay, 280, 36, 100); txtWaiver.Text = "0";
            LabelAt(pnlPay, "Payment Method", 400, 16);
            cboPaymentMethod = new ComboBox { Location = new Point(400, 36), Width = 160, DropDownStyle = ComboBoxStyle.DropDownList, FlatStyle = FlatStyle.Flat };
            pnlPay.Controls.Add(cboPaymentMethod);
            LabelAt(pnlPay, "Txn Ref", 580, 16); txtTxnRef = Tb(pnlPay, 580, 36, 160);
            LabelAt(pnlPay, "Remarks", 16, 80); txtRemarks = Tb(pnlPay, 16, 100, 400);
            lblTuitionTotal = new Label { Text = "Tuition: ৳ 0.00", Location = new Point(450, 100), AutoSize = true, ForeColor = System.Drawing.Color.FromArgb(138, 147, 166) };
            lblNetPayable = new Label { Text = "৳ 0.00", Location = new Point(650, 90), AutoSize = true, Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold), ForeColor = System.Drawing.Color.FromArgb(16, 185, 129) };
            btnCollect = new Button { Text = "Collect & Print Receipt", Location = new Point(750, 130), Size = new Size(180, 36), Tag = "success" };
            btnCollect.Click += btnCollect_Click;
            pnlPay.Controls.AddRange(new Control[] { lblTuitionTotal, lblNetPayable, btnCollect });

            Controls.AddRange(new Control[] { lblTitle, pnlSearch, pnlBio, pnlMatrix, pnlPay });
            ResumeLayout(false);
        }

        private static void LabelAt(Control p, string t, int x, int y)
        {
            Label lbl = new Label();
            lbl.Text = t;
            lbl.Location = new Point(x, y);
            lbl.AutoSize = true;
            lbl.ForeColor = System.Drawing.Color.FromArgb(138, 147, 166);
            p.Controls.Add(lbl);
        }

        private static TextBox Tb(Control p, int x, int y, int w)
        {
            var tb = new TextBox { Location = new Point(x, y), Width = w, BackColor = System.Drawing.Color.FromArgb(17, 24, 39), ForeColor = System.Drawing.Color.White, BorderStyle = BorderStyle.FixedSingle };
            p.Controls.Add(tb);
            return tb;
        }
    }
}
