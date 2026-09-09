using StudentManagement.Desktop.Theme;

namespace StudentManagement.Desktop.Forms;

partial class frmExamClearance
{
    private Label lblTitle = null!;
    private ComboBox cboStudent = null!;
    private ComboBox cboTerm = null!;
    private CheckBox chkOverride = null!;
    private TextBox txtOverrideReason = null!;
    private Button btnVerify = null!;
    private Label lblResult = null!;

    private void InitializeComponent()
    {
        SuspendLayout();
        Text = "Exam Clearance";
        BackColor = UITheme.Canvas;

        lblTitle = new Label { Text = "Exam Clearance Verification", Font = UITheme.FontHeader, ForeColor = UITheme.TextPrimary, Location = new Point(16, 12), AutoSize = true };

        var card = new Panel { Tag = "card", BackColor = UITheme.Card, Location = new Point(16, 55), Size = new Size(700, 280) };
        var lblS = new Label { Text = "Student", Location = new Point(20, 20), AutoSize = true, ForeColor = UITheme.TextMuted };
        cboStudent = new ComboBox { Location = new Point(20, 42), Width = 420, DropDownStyle = ComboBoxStyle.DropDownList, FlatStyle = FlatStyle.Flat };
        var lblT = new Label { Text = "Exam Term", Location = new Point(20, 85), AutoSize = true, ForeColor = UITheme.TextMuted };
        cboTerm = new ComboBox { Location = new Point(20, 107), Width = 420, DropDownStyle = ComboBoxStyle.DropDownList, FlatStyle = FlatStyle.Flat };
        chkOverride = new CheckBox { Text = "Admin override (allow admit card despite dues)", Location = new Point(20, 155), AutoSize = true, ForeColor = UITheme.Warning };
        txtOverrideReason = new TextBox { Location = new Point(20, 185), Width = 420, PlaceholderText = "Override reason (required when dues > 0)", BackColor = UITheme.InputBack, ForeColor = UITheme.TextPrimary, BorderStyle = BorderStyle.FixedSingle };
        btnVerify = new Button { Text = "Verify Clearance", Location = new Point(20, 230), Size = new Size(160, 36) };
        btnVerify.Click += btnVerify_Click;
        card.Controls.AddRange(new Control[] { lblS, cboStudent, lblT, cboTerm, chkOverride, txtOverrideReason, btnVerify });

        lblResult = new Label
        {
            Text = "Select a student and term, then verify clearance. Dues > ৳0 will block admit cards unless overridden.",
            Location = new Point(16, 355),
            Size = new Size(700, 60),
            ForeColor = UITheme.TextMuted
        };

        Controls.AddRange(new Control[] { lblTitle, card, lblResult });
        ResumeLayout(false);
    }
}
