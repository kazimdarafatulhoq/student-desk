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
    partial class frmLogin
    {
        private Panel pnlCard = null!;
        private Label lblBrand = null!;
        private Label lblTitle = null!;
        private Label lblUser = null!;
        private Label lblPass = null!;
        private TextBox txtUsername = null!;
        private TextBox txtPassword = null!;
        private Button btnLogin = null!;
        private Label lblStatus = null!;
        private Label lblHint = null!;

        private void InitializeComponent()
        {
            SuspendLayout();
            Text = "Sign In — Student Management";
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ClientSize = new Size(460, 420);
            BackColor = UITheme.Canvas;
            Font = UITheme.FontBody;

            pnlCard = new Panel
            {
                Tag = "card",
                BackColor = UITheme.Card,
                Location = new Point(40, 40),
                Size = new Size(380, 330)
            };
            pnlCard.Paint += (_, e) =>
            {
                using var pen = new Pen(UITheme.Border);
                e.Graphics.DrawRectangle(pen, 0, 0, pnlCard.Width - 1, pnlCard.Height - 1);
            };

            lblBrand = new Label
            {
                Text = "HORIZON ACADEMY",
                Font = UITheme.FontHeader,
                ForeColor = UITheme.Primary,
                Location = new Point(24, 24),
                AutoSize = true
            };
            lblTitle = new Label
            {
                Text = "Enterprise Portal Login",
                Font = UITheme.FontSubtitle,
                ForeColor = UITheme.TextMuted,
                Location = new Point(24, 56),
                AutoSize = true
            };
            lblUser = new Label { Text = "Username", Location = new Point(24, 100), AutoSize = true, ForeColor = UITheme.TextPrimary };
            txtUsername = new TextBox { Location = new Point(24, 122), Size = new Size(330, 28), Text = "admin" };
            lblPass = new Label { Text = "Password", Location = new Point(24, 165), AutoSize = true, ForeColor = UITheme.TextPrimary };
            txtPassword = new TextBox { Location = new Point(24, 187), Size = new Size(330, 28), UseSystemPasswordChar = true, Text = "Admin@123" };
            btnLogin = new Button { Text = "Sign In", Location = new Point(24, 240), Size = new Size(330, 40), Tag = "primary" };
            btnLogin.Click += btnLogin_Click;
            lblStatus = new Label { Text = string.Empty, Location = new Point(24, 290), Size = new Size(330, 20), ForeColor = UITheme.TextMuted };
            lblHint = new Label
            {
                Text = "Default: admin / Admin@123",
                Location = new Point(40, 380),
                AutoSize = true,
                Tag = "muted",
                ForeColor = UITheme.TextMuted
            };

            pnlCard.Controls.AddRange(new Control[] { lblBrand, lblTitle, lblUser, txtUsername, lblPass, txtPassword, btnLogin, lblStatus });
            Controls.Add(pnlCard);
            Controls.Add(lblHint);
            ResumeLayout(false);
        }
    }
}
