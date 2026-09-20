
using System;
using System.Drawing;
using System.Windows.Forms;

namespace StudentManagement.Desktop.Forms
{
    partial class frmMainMenu
    {
        private Panel pnlSidebar;
        private Panel pnlHeader;
        private Panel pnlContentContainer;
        private Panel pnlProfile;
        private StatusStrip statusBar;
        private ToolStripStatusLabel lblDbStatus;
        private ToolStripStatusLabel lblSync;
        private Label lblLogo;
        private Label lblUserRole;
        private Label lblSession;
        private Label lblClock;
        private Label lblAvatar;
        private Label lblProfileName;
        private Label lblProfileStatus;
        private Button btnToggleSidebar;
        private Button btnDashboard;
        private Button btnAdmission;
        private Button btnSearch;
        private Button btnFeeCollection;
        private Button btnLedger;
        private Button btnExamClearance;
        private Button btnAdmitCard;
        private Button btnUsers;
        private Button btnLogout;

        private void InitializeComponent()
        {
            SuspendLayout();
            Text = "Ideal High School & College — Student Management";
            WindowState = FormWindowState.Maximized;
            MinimumSize = new Size(1100, 700);
            BackColor = System.Drawing.Color.FromArgb(9, 12, 23);
            Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular);

            pnlSidebar = new Panel
            {
                Tag = "sidebar",
                Dock = DockStyle.Left,
                Width = 260,
                BackColor = System.Drawing.Color.FromArgb(7, 11, 24),
                Padding = new Padding(10, 12, 10, 12)
            };

            pnlProfile = new Panel
            {
                Dock = DockStyle.Top,
                Height = 72,
                BackColor = System.Drawing.Color.FromArgb(7, 11, 24),
                Padding = new Padding(8, 4, 8, 8)
            };
            lblAvatar = new Label
            {
                Text = string.Empty,
                Size = new Size(44, 44),
                Location = new Point(8, 10),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent,
                ForeColor = Color.White,
                Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold),
                Tag = "AD"
            };
            lblAvatar.Paint += Avatar_Paint;
            lblProfileName = new Label
            {
                Text = "Administrator",
                Location = new Point(60, 12),
                AutoSize = true,
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold)
            };
            lblProfileStatus = new Label
            {
                Text = "●  Administrator (Online)",
                Location = new Point(60, 36),
                AutoSize = true,
                ForeColor = System.Drawing.Color.FromArgb(34, 197, 94),
                Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular)
            };
            pnlProfile.Controls.AddRange(new Control[] { lblAvatar, lblProfileName, lblProfileStatus });

            btnToggleSidebar = NavButton("☰  Collapse", "toggle");
            btnToggleSidebar.Click += btnToggleSidebar_Click;
            btnDashboard = NavButton("Dashboard", "Dashboard");
            btnDashboard.Click += btnDashboard_Click;
            btnAdmission = NavButton("Student Registration", "Admission");
            btnAdmission.Click += btnAdmission_Click;
            btnSearch = NavButton("Student Directory", "Search");
            btnSearch.Click += btnSearch_Click;
            btnFeeCollection = NavButton("Fee Collection Counter", "Fee");
            btnFeeCollection.Click += btnFeeCollection_Click;
            btnLedger = NavButton("Payment History & Ledger", "Ledger");
            btnLedger.Click += btnLedger_Click;
            btnExamClearance = NavButton("4-Month Fee Eligibility", "Exam");
            btnExamClearance.Click += btnExamClearance_Click;
            btnAdmitCard = NavButton("Admit Card Printing", "Admit");
            btnAdmitCard.Click += btnAdmitCard_Click;
            btnUsers = NavButton("User Access & Security", "Users");
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
                Tag = "sidebar",
                BackColor = System.Drawing.Color.FromArgb(7, 11, 24),
                Padding = new Padding(2)
            };
            navFlow.Controls.Add(btnToggleSidebar);
            navFlow.Controls.Add(SectionHeader("OPERATIONAL CORE"));
            navFlow.Controls.Add(btnDashboard);
            navFlow.Controls.Add(btnAdmission);
            navFlow.Controls.Add(btnSearch);
            navFlow.Controls.Add(SectionHeader("ACCOUNTS & FEES"));
            navFlow.Controls.Add(btnFeeCollection);
            navFlow.Controls.Add(btnLedger);
            navFlow.Controls.Add(SectionHeader("EXAM & ADMIT CARDS"));
            navFlow.Controls.Add(btnExamClearance);
            navFlow.Controls.Add(btnAdmitCard);
            navFlow.Controls.Add(SectionHeader("SYSTEM"));
            navFlow.Controls.Add(btnUsers);
            navFlow.Controls.Add(btnLogout);

            pnlSidebar.Controls.Add(navFlow);
            pnlSidebar.Controls.Add(pnlProfile);

            pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 64,
                Tag = "header",
                BackColor = System.Drawing.Color.FromArgb(10, 16, 36),
                Padding = new Padding(16, 8, 16, 8)
            };
            lblLogo = new Label
            {
                Text = "IDEAL HIGH SCHOOL & COLLEGE",
                Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.White,
                AutoSize = true,
                Location = new Point(16, 18)
            };
            lblUserRole = new Label
            {
                Text = "User",
                Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.White,
                AutoSize = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(700, 10)
            };
            lblSession = new Label
            {
                Text = "Session",
                Tag = "muted",
                ForeColor = System.Drawing.Color.FromArgb(138, 147, 166),
                AutoSize = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(700, 34)
            };
            lblClock = new Label
            {
                Text = DateTime.Now.ToString("dd-MMM-yyyy  HH:mm:ss"),
                ForeColor = System.Drawing.Color.FromArgb(138, 147, 166),
                AutoSize = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(920, 22)
            };
            pnlHeader.Controls.AddRange(new Control[] { lblLogo, lblUserRole, lblSession, lblClock });
            pnlHeader.Resize += pnlHeader_Resize;

            pnlContentContainer = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = System.Drawing.Color.FromArgb(9, 12, 23),
                Padding = new Padding(8)
            };

            statusBar = new StatusStrip { BackColor = System.Drawing.Color.FromArgb(10, 16, 36), ForeColor = System.Drawing.Color.FromArgb(138, 147, 166) };
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

        private static Label SectionHeader(string text)
        {
            return new Label
            {
                Text = text,
                Tag = "muted",
                ForeColor = System.Drawing.Color.FromArgb(138, 147, 166),
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                AutoSize = true,
                Margin = new Padding(12, 16, 4, 6),
                Width = 220
            };
        }

        private static Button NavButton(string text, string accessible)
        {
            Button b = new Button();
            b.Text = text;
            b.AccessibleName = text;
            b.Tag = "nav";
            b.Width = 232;
            b.Height = 42;
            b.FlatStyle = FlatStyle.Flat;
            b.TextAlign = ContentAlignment.MiddleLeft;
            b.ForeColor = System.Drawing.Color.FromArgb(138, 147, 166);
            b.BackColor = Color.Transparent;
            b.Margin = new Padding(0, 2, 0, 2);
            b.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular);
            b.Cursor = Cursors.Hand;
            b.Padding = new Padding(12, 0, 8, 0);
            b.FlatAppearance.BorderSize = 0;
            b.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(13, 20, 48);
            return b;
        }

        private void Avatar_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            using (var brush = new SolidBrush(System.Drawing.Color.FromArgb(37, 99, 235)))
                e.Graphics.FillEllipse(brush, 0, 0, lblAvatar.Width - 1, lblAvatar.Height - 1);
            string initials = lblAvatar.Tag != null ? lblAvatar.Tag.ToString() : "AD";
            TextRenderer.DrawText(
                e.Graphics,
                initials,
                lblAvatar.Font,
                lblAvatar.ClientRectangle,
                Color.White,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        private void pnlHeader_Resize(object sender, EventArgs e)
        {
            lblClock.Left = pnlHeader.Width - lblClock.Width - 20;
            lblUserRole.Left = pnlHeader.Width - 360;
            lblSession.Left = pnlHeader.Width - 360;
        }
    }
}
