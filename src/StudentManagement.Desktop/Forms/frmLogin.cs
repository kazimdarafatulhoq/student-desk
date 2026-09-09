using StudentManagement.Application.DTOs;
using StudentManagement.Application.Services;
using StudentManagement.Desktop.Helpers;
using StudentManagement.Desktop.Theme;

namespace StudentManagement.Desktop.Forms;

public partial class frmLogin : Form
{
    private readonly AuthService _auth;

    public frmLogin(AuthService auth)
    {
        _auth = auth;
        InitializeComponent();
        UITheme.ApplyForm(this);
        AcceptButton = btnLogin;
    }

    private async void btnLogin_Click(object? sender, EventArgs e)
    {
        try
        {
            btnLogin.Enabled = false;
            lblStatus.Text = "Authenticating...";
            lblStatus.ForeColor = UITheme.TextMuted;

            var session = await _auth.LoginAsync(new LoginRequest
            {
                Username = txtUsername.Text.Trim(),
                Password = txtPassword.Text
            });

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
