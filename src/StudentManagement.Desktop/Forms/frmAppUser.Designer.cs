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
    partial class frmAppUser
    {
        private Label lblTitle;
        private Panel pnlCreate;
        private TextBox txtUsername;
        private TextBox txtFullName;
        private TextBox txtPassword;
        private TextBox txtEmail;
        private TextBox txtPhone;
        private ComboBox cboRole;
        private Button btnCreate;
        private DataGridView dgvUsers;
        private Button btnToggle;

        private void InitializeComponent()
        {
            SuspendLayout();
            Text = "User Management";
            BackColor = UITheme.Canvas;

            lblTitle = new Label { Text = "User Access & Security (RBAC)", Font = UITheme.FontHeader, ForeColor = UITheme.TextPrimary, Location = new Point(16, 12), AutoSize = true };

            pnlCreate = new Panel { Tag = "card", BackColor = UITheme.Card, Location = new Point(16, 55), Size = new Size(960, 140) };
            txtUsername = Field(pnlCreate, "Username", 16, 16, 150);
            txtFullName = Field(pnlCreate, "Full Name", 180, 16, 200);
            txtPassword = Field(pnlCreate, "Password", 400, 16, 160);
            txtPassword.UseSystemPasswordChar = true;
            txtEmail = Field(pnlCreate, "Email", 580, 16, 180);
            txtPhone = Field(pnlCreate, "Phone", 780, 16, 140);
            var lblRole = new Label { Text = "Role", Location = new Point(16, 75), AutoSize = true, ForeColor = UITheme.TextMuted };
            cboRole = new ComboBox { Location = new Point(16, 95), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList, FlatStyle = FlatStyle.Flat };
            btnCreate = new Button { Text = "Create User", Location = new Point(240, 92), Size = new Size(140, 34) };
            btnCreate.Click += btnCreate_Click;
            pnlCreate.Controls.AddRange(new Control[] { lblRole, cboRole, btnCreate });

            dgvUsers = new DataGridView { Location = new Point(16, 215), Size = new Size(960, 300), ReadOnly = true };
            dgvUsers.Columns.Add("Id", "ID");
            dgvUsers.Columns.Add("Username", "Username");
            dgvUsers.Columns.Add("Name", "Full Name");
            dgvUsers.Columns.Add("Role", "Role");
            dgvUsers.Columns.Add("Status", "Status");
            dgvUsers.Columns.Add("LastLogin", "Last Login");

            btnToggle = new Button { Text = "Enable / Disable Selected", Location = new Point(16, 530), Size = new Size(200, 36), Tag = "ghost" };
            btnToggle.Click += btnToggle_Click;

            Controls.AddRange(new Control[] { lblTitle, pnlCreate, dgvUsers, btnToggle });
            ResumeLayout(false);
        }

        private static TextBox Field(Control parent, string label, int x, int y, int width)
        {
            parent.Controls.Add(new Label { Text = label, Location = new Point(x, y), AutoSize = true, ForeColor = UITheme.TextMuted });
            var tb = new TextBox { Location = new Point(x, y + 20), Width = width, BackColor = UITheme.InputBack, ForeColor = UITheme.TextPrimary, BorderStyle = BorderStyle.FixedSingle };
            parent.Controls.Add(tb);
            return tb;
        }
    }
}
