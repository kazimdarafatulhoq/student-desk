using StudentManagement.Desktop.Theme;

namespace StudentManagement.Desktop.Forms;

partial class frmDashboard
{
    private Label lblTitle = null!;
    private Label lblSubtitle = null!;
    private Panel cardStudents = null!;
    private Panel cardDues = null!;
    private Label lblStudents = null!;
    private Label lblDues = null!;
    private Label lblPaidHint = null!;
    private Label lblDueHint = null!;

    private void InitializeComponent()
    {
        SuspendLayout();
        Text = "Dashboard";
        Dock = DockStyle.Fill;
        BackColor = UITheme.Canvas;

        lblTitle = new Label
        {
            Text = "Operations Dashboard",
            Tag = "header",
            Font = UITheme.FontHeader,
            ForeColor = UITheme.TextPrimary,
            Location = new Point(16, 16),
            AutoSize = true
        };
        lblSubtitle = new Label
        {
            Text = "Live snapshot of admissions and receivables.",
            Tag = "muted",
            ForeColor = UITheme.TextMuted,
            Location = new Point(16, 48),
            AutoSize = true
        };

        cardStudents = MakeStatCard("Active Students", out lblStudents, out lblPaidHint, UITheme.Primary, new Point(16, 90));
        cardDues = MakeStatCard("Net Receivables", out lblDues, out lblDueHint, UITheme.Danger, new Point(300, 90));

        Controls.AddRange(new Control[] { lblTitle, lblSubtitle, cardStudents, cardDues });
        ResumeLayout(false);
    }

    private static Panel MakeStatCard(string title, out Label value, out Label hint, Color accent, Point location)
    {
        var panel = new Panel
        {
            Tag = "card",
            BackColor = UITheme.Card,
            Location = location,
            Size = new Size(260, 140)
        };
        panel.Paint += (_, e) =>
        {
            using var pen = new Pen(UITheme.Border);
            e.Graphics.DrawRectangle(pen, 0, 0, panel.Width - 1, panel.Height - 1);
            using var accentPen = new Pen(accent, 3);
            e.Graphics.DrawLine(accentPen, 0, 0, 0, panel.Height);
        };
        var t = new Label { Text = title, Location = new Point(20, 18), AutoSize = true, ForeColor = UITheme.TextMuted, Font = UITheme.FontSubtitle };
        value = new Label { Text = "…", Location = new Point(20, 50), AutoSize = true, ForeColor = accent, Font = UITheme.FontTitle };
        hint = new Label { Text = string.Empty, Location = new Point(20, 100), AutoSize = true, ForeColor = UITheme.TextMuted };
        panel.Controls.AddRange(new Control[] { t, value, hint });
        return panel;
    }
}
