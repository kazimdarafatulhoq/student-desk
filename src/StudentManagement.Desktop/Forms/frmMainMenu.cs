using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using StudentManagement.Application.Interfaces;
using StudentManagement.Application.Services;
using StudentManagement.Desktop.Helpers;
using StudentManagement.Desktop.Theme;
using StudentManagement.Domain.Enums;

namespace StudentManagement.Desktop.Forms
{
    public partial class frmMainMenu : Form
    {
        private IServiceScope _scope;
        private Form _activeChild;
        private bool _sidebarExpanded = true;
        private System.Windows.Forms.Timer _clockTimer;

        /// <summary>Parameterless constructor required by the WinForms designer.</summary>
        public frmMainMenu()
        {
            InitializeComponent();
        }

        public frmMainMenu(IServiceProvider services) : this()
        {
            _scope = services.CreateScope();
            UITheme.ApplyForm(this);
            _clockTimer = new System.Windows.Forms.Timer();
            _clockTimer.Interval = 1000;
            _clockTimer.Tick += ClockTimer_Tick;
            _clockTimer.Start();
            Load += frmMainMenu_Load;
            FormClosed += frmMainMenu_FormClosed;
        }

        private void ClockTimer_Tick(object sender, EventArgs e)
        {
            lblClock.Text = DateTime.Now.ToString("dd-MMM-yyyy  HH:mm:ss");
        }

        private void frmMainMenu_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_clockTimer != null)
            {
                _clockTimer.Stop();
                _clockTimer.Dispose();
            }
            if (_scope != null)
                _scope.Dispose();
        }

        private async void frmMainMenu_Load(object sender, EventArgs e)
        {
            if (AppSession.Current == null)
            {
                Close();
                return;
            }

            lblUserRole.Text = AppSession.Current.FullName + "  ·  " + AppSession.RoleDisplay;
            lblSession.Text = "Session: " + AppSession.Current.AcademicSession;
            lblProfileName.Text = AppSession.Current.FullName;
            lblProfileStatus.Text = "●  " + AppSession.RoleDisplay + " (Online)";
            lblProfileStatus.ForeColor = UITheme.Online;
            string initials = GetInitials(AppSession.Current.FullName);
            lblAvatar.Tag = initials;
            lblAvatar.Invalidate();
            ApplyRoleVisibility();
            await RefreshDbStatusAsync();
            OpenChild<frmDashboard>();
        }

        private static string GetInitials(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                return "AD";
            string[] parts = fullName.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 1)
                return parts[0].Substring(0, Math.Min(2, parts[0].Length)).ToUpperInvariant();
            return (parts[0][0].ToString() + parts[parts.Length - 1][0]).ToUpperInvariant();
        }

        private void ApplyRoleVisibility()
        {
            if (AppSession.Current == null)
                return;
            UserRole role = AppSession.Current.Role;
            btnAdmission.Visible = AuthService.CanAccess(role, "Admission");
            btnFeeCollection.Visible = AuthService.CanAccess(role, "FeeCollection");
            btnUsers.Visible = AuthService.CanAccess(role, "Users");
        }

        private async System.Threading.Tasks.Task RefreshDbStatusAsync()
        {
            try
            {
                IUnitOfWork uow = _scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                bool ok = await uow.TestConnectionAsync();
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
            if (_activeChild != null)
            {
                pnlContentContainer.Controls.Remove(_activeChild);
                _activeChild.Dispose();
                _activeChild = null;
            }

            Form form = _scope.ServiceProvider.GetRequiredService<T>();
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
            foreach (Control host in pnlSidebar.Controls)
            {
                foreach (Control c in GetAllButtons(host))
                {
                    Button b = c as Button;
                    if (b == null)
                        continue;
                    string tag = b.Tag != null ? b.Tag.ToString() : string.Empty;
                    if (tag == "nav" || tag == "nav-active")
                        UITheme.SetNavActive(b, false);
                }
            }

            Button active = null;
            if (formName == nameof(frmDashboard)) active = btnDashboard;
            else if (formName == nameof(frmStudentAdmission)) active = btnAdmission;
            else if (formName == nameof(frmStudentSearch)) active = btnSearch;
            else if (formName == nameof(frmFeeCollection)) active = btnFeeCollection;
            else if (formName == nameof(frmStudentLedger)) active = btnLedger;
            else if (formName == nameof(frmExamClearance)) active = btnExamClearance;
            else if (formName == nameof(frmAdmitCardPrint)) active = btnAdmitCard;
            else if (formName == nameof(frmAppUser)) active = btnUsers;

            if (active != null)
                UITheme.SetNavActive(active, true);
        }

        private System.Collections.Generic.IEnumerable<Control> GetAllButtons(Control root)
        {
            yield return root;
            foreach (Control child in root.Controls)
            {
                foreach (Control nested in GetAllButtons(child))
                    yield return nested;
            }
        }

        private void btnToggleSidebar_Click(object sender, EventArgs e)
        {
            _sidebarExpanded = !_sidebarExpanded;
            pnlSidebar.Width = _sidebarExpanded ? 260 : 72;
            pnlProfile.Visible = _sidebarExpanded;
            foreach (Control host in pnlSidebar.Controls)
            {
                foreach (Control c in GetAllButtons(host))
                {
                    Button b = c as Button;
                    if (b == null)
                        continue;
                    string tag = b.Tag != null ? b.Tag.ToString() : string.Empty;
                    if (b != btnToggleSidebar && (tag == "nav" || tag == "nav-active"))
                        b.Text = _sidebarExpanded ? (b.AccessibleName ?? b.Text) : string.Empty;
                }
                foreach (Control child in host.Controls)
                {
                    Label section = child as Label;
                    if (section != null && section.Tag != null && section.Tag.ToString() == "muted")
                        section.Visible = _sidebarExpanded;
                }
            }
            btnToggleSidebar.Text = _sidebarExpanded ? "☰  Collapse" : "☰";
        }

        private void btnDashboard_Click(object sender, EventArgs e) { OpenChild<frmDashboard>(); }
        private void btnAdmission_Click(object sender, EventArgs e) { OpenChild<frmStudentAdmission>(); }
        private void btnSearch_Click(object sender, EventArgs e) { OpenChild<frmStudentSearch>(); }
        private void btnFeeCollection_Click(object sender, EventArgs e) { OpenChild<frmFeeCollection>(); }
        private void btnLedger_Click(object sender, EventArgs e) { OpenChild<frmStudentLedger>(); }
        private void btnExamClearance_Click(object sender, EventArgs e) { OpenChild<frmExamClearance>(); }
        private void btnAdmitCard_Click(object sender, EventArgs e) { OpenChild<frmAdmitCardPrint>(); }
        private void btnUsers_Click(object sender, EventArgs e) { OpenChild<frmAppUser>(); }

        public void OpenFeeCollectionFromDashboard()
        {
            OpenChild<frmFeeCollection>();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            AppSession.Clear();
            System.Windows.Forms.Application.Restart();
        }
    }
}
