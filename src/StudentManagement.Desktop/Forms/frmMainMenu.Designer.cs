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
    partial class frmMainMenu
    {
        private Panel pnlSidebar = null!;
        private Panel pnlHeader = null!;
        private Panel pnlContentContainer = null!;
        private StatusStrip statusBar = null!;
        private ToolStripStatusLabel lblDbStatus = null!;
        private ToolStripStatusLabel lblSync = null!;
        private Label lblLogo = null!;
        private Label lblUserRole = null!;
        private Label lblSession = null!;
        private Label lblClock = null!;
        private Button btnToggleSidebar = null!;
        private Button btnDashboard = null!;
        private Button btnAdmission = null!;
        private Button btnSearch = null!;
        private Button btnFeeCollection = null!;
        private Button btnLedger = null!;
        private Button btnExamClearance = null!;
        private Button btnAdmitCard = null!;
        private Button btnUsers = null!;
        private Button btnLogout = null!;

        private void InitializeComponent()
        {
            SuspendLayout();
            Text = "Student Management & Financial Accounting";
            WindowState = FormWindowState.Maximized;
            MinimumSize = new Size(1100, 700);
            BackColor = UITheme.Canvas;
            Font = UITheme.FontBody;

            pnlSidebar = new Panel
            {
                Tag = "sidebar",
                Dock = DockStyle.Left,
                Width = 220,
                BackColor = UITheme.Sidebar,
                Padding = new Padding(8)
            };

            btnToggleSidebar = NavButton("☰  Collapse", "toggle");
            btnToggleSidebar.Click += btnToggleSidebar_Click;
            btnDashboard = NavButton("Dashboard", "Dashboard");
            btnDashboard.Click += btnDashboard_Click;
            btnAdmission = NavButton("Student Admission", "Admission");
            btnAdmission.Click += btnAdmission_Click;
            btnSearch = NavButton("Student Search", "Search");
            btnSearch.Click += btnSearch_Click;
            btnFeeCollection = NavButton("Fee Collection", "Fee");
            btnFeeCollection.Click += btnFeeCollection_Click;
            btnLedger = NavButton("Student Ledger", "Ledger");
            btnLedger.Click += btnLedger_Click;
            btnExamClearance = NavButton("Exam Clearance", "Exam");
            btnExamClearance.Click += btnExamClearance_Click;
            btnAdmitCard = NavButton("Admit Card Print", "Admit");
            btnAdmitCard.Click += btnAdmitCard_Click;
            btnUsers = NavButton("User Management", "Users");
            btnUsers.Click += btnUsers_Click;
            btnLogout = NavButton("Sign Out", "Logout");
            btnLogout.Tag = "danger";
            btnLogout.Click += btnLogout_Click;

            var navFlow = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                BackColor = UITheme.Sidebar,
                Padding = new Padding(4)
            };
            navFlow.Controls.AddRange(new Control[]
            {
                btnToggleSidebar, btnDashboard, btnAdmission, btnSearch, btnFeeCollection,
                btnLedger, btnExamClearance, btnAdmitCard, btnUsers, btnLogout
            });
            pnlSidebar.Controls.Add(navFlow);

            pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 64,
                BackColor = UITheme.Card,
                Padding = new Padding(16, 8, 16, 8)
            };
            lblLogo = new Label
            {
                Text = "HORIZON ACADEMY",
                Font = UITheme.FontHeader,
                ForeColor = UITheme.Primary,
                AutoSize = true,
                Location = new Point(16, 18)
            };
            lblUserRole = new Label
            {
                Text = "User",
                Font = UITheme.FontSubtitle,
                ForeColor = UITheme.TextPrimary,
                AutoSize = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(700, 10)
            };
            lblSession = new Label
            {
                Text = "Session",
                Tag = "muted",
                ForeColor = UITheme.TextMuted,
                AutoSize = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(700, 34)
            };
            lblClock = new Label
            {
                Text = DateTime.Now.ToString("dd-MMM-yyyy  HH:mm:ss"),
                ForeColor = UITheme.TextMuted,
                AutoSize = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(920, 22)
            };
            pnlHeader.Controls.AddRange(new Control[] { lblLogo, lblUserRole, lblSession, lblClock });
            pnlHeader.Resize += (_, _) =>
            {
                lblClock.Left = pnlHeader.Width - lblClock.Width - 20;
                lblUserRole.Left = pnlHeader.Width - 360;
                lblSession.Left = pnlHeader.Width - 360;
            };

            pnlContentContainer = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = UITheme.Canvas,
                Padding = new Padding(8)
            };

            statusBar = new StatusStrip { BackColor = UITheme.Card, ForeColor = UITheme.TextMuted };
            lblDbStatus = new ToolStripStatusLabel("Database: Checking...");
            lblSync = new ToolStripStatusLabel("Ledger sync: —") { Spring = true, TextAlign = ContentAlignment.MiddleRight };
            statusBar.Items.AddRange(new ToolStripItem[] { lblDbStatus, lblSync });

            Controls.Add(pnlContentContainer);
            Controls.Add(pnlHeader);
            Controls.Add(pnlSidebar);
            Controls.Add(statusBar);
            ResumeLayout(false);
            PerformLayout();
        }

        private static Button NavButton(string text, string accessible)
        {
            var b = new Button
            {
                Text = text,
                AccessibleName = text,
                Tag = "nav",
                Width = 200,
                Height = 44,
                FlatStyle = FlatStyle.Flat,
                TextAlign = ContentAlignment.MiddleLeft,
                ForeColor = UITheme.TextMuted,
                BackColor = Color.Transparent,
                Margin = new Padding(0, 2, 0, 2),
                Font = UITheme.FontNav,
                Cursor = Cursors.Hand
            };
            b.FlatAppearance.BorderSize = 0;
            b.FlatAppearance.MouseOverBackColor = UITheme.Card;
            return b;
        }
    }
}
