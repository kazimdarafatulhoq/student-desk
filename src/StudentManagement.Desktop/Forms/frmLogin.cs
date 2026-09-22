using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Forms;
using StudentManagement.Application.DTOs;
using StudentManagement.Application.Services;
using StudentManagement.Desktop.Helpers;
using StudentManagement.Desktop.Theme;
using StudentManagement.Domain.Enums;

namespace StudentManagement.Desktop.Forms
{
    [DesignerCategory("Form")]
    public partial class frmLogin : Form
    {
        private AuthService? _auth;

        /// <summary>Parameterless constructor required by the WinForms designer.</summary>
        public frmLogin()
        {
            InitializeComponent();
            if (!DesignTime.IsActive)
            {
                UITheme.ApplyForm(this);
                // Login card matches the form body (no elevated card fill).
                pnlCard.BackColor = UITheme.Canvas;
            }
        }

        public frmLogin(AuthService auth) : this()
        {
            _auth = auth;
            if (!DesignTime.IsActive)
            {
                UITheme.ApplyForm(this);
                pnlCard.BackColor = UITheme.Canvas;
            }
            AcceptButton = btnLogin;
            if (AppSession.OfflineMode)
            {
                lblHint.Text = "Design mode (no database) — admin / Admin@123";
                lblStatus.Text = "OfflineMode: SQL Server disconnected";
                lblStatus.ForeColor = UITheme.Warning;
            }
        }

        private async void btnLogin_Click(object? sender, EventArgs e)
        {
            try
            {
                btnLogin.Enabled = false;
                lblStatus.Text = "Authenticating...";
                lblStatus.ForeColor = UITheme.TextMuted;

                string user = txtUsername.Text.Trim();
                string pass = txtPassword.Text;

                AuthSession session;
                if (AppSession.OfflineMode)
                {
                    if (!string.Equals(user, "admin", StringComparison.OrdinalIgnoreCase) || pass != "Admin@123")
                        throw new UnauthorizedAccessException("Invalid username or password.");

                    session = new AuthSession
                    {
                        UserId = 1,
                        Username = "admin",
                        FullName = "System Super Admin",
                        Role = UserRole.SuperAdmin,
                        AcademicSession = "2025-2026",
                        LoginAt = DateTime.Now
                    };
                }
                else
                {
                    if (_auth == null)
                        throw new InvalidOperationException("Authentication service is not available.");

                    session = await _auth.LoginAsync(new LoginRequest
                    {
                        Username = user,
                        Password = pass
                    });
                }

                AppSession.Set(session);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
                lblStatus.ForeColor = UITheme.Danger;
                txtPassword.SelectAll();
                txtPassword.Focus();
            }
            finally
            {
                btnLogin.Enabled = true;
            }
        }
    }
}
