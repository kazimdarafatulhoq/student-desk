using StudentManagement.Application.DTOs;
using StudentManagement.Application.Services;
using StudentManagement.Desktop.Helpers;
using StudentManagement.Desktop.Theme;
using StudentManagement.Domain.Enums;

namespace StudentManagement.Desktop.Forms;

public partial class frmAppUser : Form
{
    private readonly AuthService _auth;

    public frmAppUser(AuthService auth)
    {
        _auth = auth;
        InitializeComponent();
        UITheme.ApplyForm(this);
        cboRole.DataSource = Enum.GetValues(typeof(UserRole));
        Load += async (_, _) => await RefreshUsersAsync();
    }

    private async Task RefreshUsersAsync()
    {
        var users = await _auth.GetUsersAsync();
        dgvUsers.Rows.Clear();
        foreach (var u in users)
        {
            dgvUsers.Rows.Add(u.UserId, u.Username, u.FullName, u.Role.ToString(), u.IsActive ? "Active" : "Disabled",
                u.LastLoginAt?.ToString("dd-MMM-yyyy HH:mm") ?? "—");
        }
    }

    private async void btnCreate_Click(object? sender, EventArgs e)
    {
        try
        {
            await _auth.CreateUserAsync(new CreateUserRequest
            {
                Username = txtUsername.Text.Trim(),
                FullName = txtFullName.Text.Trim(),
                Password = txtPassword.Text,
                Email = txtEmail.Text.Trim(),
                Phone = txtPhone.Text.Trim(),
                Role = (UserRole)cboRole.SelectedItem!,
                CreatedBy = AppSession.Current?.Username
            });
            txtUsername.Clear();
            txtFullName.Clear();
            txtPassword.Clear();
            txtEmail.Clear();
            txtPhone.Clear();
            await RefreshUsersAsync();
            MessageBox.Show("User created successfully.", "User Management", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Create Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void btnToggle_Click(object? sender, EventArgs e)
    {
        if (dgvUsers.CurrentRow is null) return;
        var userId = Convert.ToInt32(dgvUsers.CurrentRow.Cells[0].Value);
        var active = dgvUsers.CurrentRow.Cells[4].Value?.ToString() == "Active";
        await _auth.SetActiveAsync(userId, !active);
        await RefreshUsersAsync();
    }
}
