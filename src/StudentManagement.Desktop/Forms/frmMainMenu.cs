using Microsoft.Extensions.DependencyInjection;
using StudentManagement.Application.Interfaces;
using StudentManagement.Application.Services;
using StudentManagement.Desktop.Helpers;
using StudentManagement.Desktop.Theme;
using StudentManagement.Domain.Enums;

namespace StudentManagement.Desktop.Forms;

public partial class frmMainMenu : Form
{
    private readonly IServiceScope _scope;
    private Form? _activeChild;
    private bool _sidebarExpanded = true;
    private readonly System.Windows.Forms.Timer _clockTimer;

    public frmMainMenu(IServiceProvider sp)
    {
        _scope = sp.CreateScope();
        InitializeComponent();
        UITheme.ApplyForm(this);
        _clockTimer = new System.Windows.Forms.Timer { Interval = 1000 };
        _clockTimer.Tick += (_, _) => lblClock.Text = DateTime.Now.ToString("dd-MMM-yyyy  HH:mm:ss");
        _clockTimer.Start();
        Load += frmMainMenu_Load;
        FormClosed += (_, _) =>
        {
            _clockTimer.Stop();
            _scope.Dispose();
        };
    }

    private async void frmMainMenu_Load(object? sender, EventArgs e)
    {
        if (AppSession.Current is null)
        {
            Close();
            return;
        }

        lblUserRole.Text = $"{AppSession.Current.FullName}  ·  {AppSession.RoleDisplay}";
        lblSession.Text = $"Session: {AppSession.Current.AcademicSession}";
        ApplyRoleVisibility();
        await RefreshDbStatusAsync();
        OpenChild<frmDashboard>();
    }

    private void ApplyRoleVisibility()
    {
        var role = AppSession.Current!.Role;
        btnAdmission.Visible = AuthService.CanAccess(role, "Admission");
        btnFeeCollection.Visible = AuthService.CanAccess(role, "FeeCollection");
        btnUsers.Visible = AuthService.CanAccess(role, "Users");
    }

    private async Task RefreshDbStatusAsync()
    {
        try
        {
            var uow = _scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var ok = await uow.TestConnectionAsync();
            lblDbStatus.Text = ok ? "Database: Connected" : "Database: Offline";
            lblDbStatus.ForeColor = ok ? UITheme.Success : UITheme.Danger;
            lblSync.Text = ok ? "Ledger sync: Live" : "Ledger sync: Paused";
        }
        catch
        {
            lblDbStatus.Text = "Database: Error";
            lblDbStatus.ForeColor = UITheme.Danger;
        }
    }

    private void OpenChild<T>() where T : Form
    {
        if (_activeChild is not null)
        {
            pnlContentContainer.Controls.Remove(_activeChild);
            _activeChild.Dispose();
            _activeChild = null;
        }

        var form = _scope.ServiceProvider.GetRequiredService<T>();
        form.TopLevel = false;
        form.FormBorderStyle = FormBorderStyle.None;
        form.Dock = DockStyle.Fill;
        form.BackColor = UITheme.Canvas;
        pnlContentContainer.Controls.Add(form);
        form.Show();
        _activeChild = form;
        HighlightNav(typeof(T).Name);
    }

    private void HighlightNav(string formName)
    {
        foreach (Control c in pnlSidebar.Controls)
        {
            if (c is Button b && b.Tag?.ToString() == "nav")
            {
                b.BackColor = Color.Transparent;
                b.ForeColor = UITheme.TextMuted;
            }
        }

        Button? active = formName switch
        {
            nameof(frmDashboard) => btnDashboard,
            nameof(frmStudentAdmission) => btnAdmission,
            nameof(frmStudentSearch) => btnSearch,
            nameof(frmFeeCollection) => btnFeeCollection,
            nameof(frmStudentLedger) => btnLedger,
            nameof(frmExamClearance) => btnExamClearance,
            nameof(frmAdmitCardPrint) => btnAdmitCard,
            nameof(frmAppUser) => btnUsers,
            _ => null
        };

        if (active is not null)
        {
            active.BackColor = UITheme.Card;
            active.ForeColor = UITheme.TextPrimary;
        }
    }

    private void btnToggleSidebar_Click(object? sender, EventArgs e)
    {
        _sidebarExpanded = !_sidebarExpanded;
        pnlSidebar.Width = _sidebarExpanded ? 220 : 64;
        foreach (Control c in pnlSidebar.Controls)
        {
            if (c is Button b && b != btnToggleSidebar)
                b.Text = _sidebarExpanded ? (b.AccessibleName ?? b.Text) : string.Empty;
        }
        btnToggleSidebar.Text = _sidebarExpanded ? "☰  Collapse" : "☰";
    }

    private void btnDashboard_Click(object? sender, EventArgs e) => OpenChild<frmDashboard>();
    private void btnAdmission_Click(object? sender, EventArgs e) => OpenChild<frmStudentAdmission>();
    private void btnSearch_Click(object? sender, EventArgs e) => OpenChild<frmStudentSearch>();
    private void btnFeeCollection_Click(object? sender, EventArgs e) => OpenChild<frmFeeCollection>();
    private void btnLedger_Click(object? sender, EventArgs e) => OpenChild<frmStudentLedger>();
    private void btnExamClearance_Click(object? sender, EventArgs e) => OpenChild<frmExamClearance>();
    private void btnAdmitCard_Click(object? sender, EventArgs e) => OpenChild<frmAdmitCardPrint>();
    private void btnUsers_Click(object? sender, EventArgs e) => OpenChild<frmAppUser>();

    private void btnLogout_Click(object? sender, EventArgs e)
    {
        AppSession.Clear();
        System.Windows.Forms.Application.Restart();
    }
}
