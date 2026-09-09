using StudentManagement.Desktop.Theme;

namespace StudentManagement.Desktop.Forms;

partial class frmAdmitCardPrint
{
    private Label lblTitle = null!;
    private ComboBox cboStudent = null!;
    private ComboBox cboTerm = null!;
    private ComboBox cboClass = null!;
    private CheckBox chkOverride = null!;
    private TextBox txtReason = null!;
    private Button btnIssue = null!;
    private Button btnBatch = null!;
    private Label lblStatus = null!;

    private void InitializeComponent()
    {
        SuspendLayout();
        Text = "Admit Card Print";
        BackColor = UITheme.Canvas;

        lblTitle = new Label { Text = "Admit Card Designer & Print", Font = UITheme.FontHeader, ForeColor = UITheme.TextPrimary, Location = new Point(16, 12), AutoSize = true };

        var card = new Panel { Tag = "card", BackColor = UITheme.Card, Location = new Point(16, 55), Size = new Size(760, 320) };
        AddLabel(card, "Exam Term", 20, 20);
        cboTerm = new ComboBox { Location = new Point(20, 42), Width = 320, DropDownStyle = ComboBoxStyle.DropDownList, FlatStyle = FlatStyle.Flat };
        AddLabel(card, "Student (single issue)", 20, 85);
        cboStudent = new ComboBox { Location = new Point(20, 107), Width = 420, DropDownStyle = ComboBoxStyle.DropDownList, FlatStyle = FlatStyle.Flat };
        AddLabel(card, "Class (batch filter)", 460, 85);
        cboClass = new ComboBox { Location = new Point(460, 107), Width = 220, DropDownStyle = ComboBoxStyle.DropDownList, FlatStyle = FlatStyle.Flat };
        chkOverride = new CheckBox { Text = "Allow admin override for dues", Location = new Point(20, 155), AutoSize = true, ForeColor = UITheme.Warning };
        txtReason = new TextBox { Location = new Point(20, 185), Width = 420, PlaceholderText = "Override reason", BackColor = UITheme.InputBack, ForeColor = UITheme.TextPrimary, BorderStyle = BorderStyle.FixedSingle };
        btnIssue = new Button { Text = "Issue & Print Single", Location = new Point(20, 235), Size = new Size(180, 36) };
        btnIssue.Click += btnIssue_Click;
        btnBatch = new Button { Text = "Batch Admit Cards", Location = new Point(220, 235), Size = new Size(180, 36), Tag = "success" };
        btnBatch.Click += btnBatch_Click;
        card.Controls.AddRange(new Control[] { cboTerm, cboStudent, cboClass, chkOverride, txtReason, btnIssue, btnBatch });

        lblStatus = new Label { Text = "Institutional header, timetable, and barcode are embedded in the PDF admit card.", Location = new Point(16, 395), Size = new Size(760, 50), ForeColor = UITheme.TextMuted };

        Controls.AddRange(new Control[] { lblTitle, card, lblStatus });
        ResumeLayout(false);
    }

    private static void AddLabel(Control parent, string text, int x, int y) =>
        parent.Controls.Add(new Label { Text = text, Location = new Point(x, y), AutoSize = true, ForeColor = UITheme.TextMuted });
}
