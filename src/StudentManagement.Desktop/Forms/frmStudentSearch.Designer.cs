using StudentManagement.Desktop.Theme;

namespace StudentManagement.Desktop.Forms;

partial class frmStudentSearch
{
    private Label lblTitle = null!;
    private Panel pnlFilter = null!;
    private ComboBox cboClass = null!;
    private ComboBox cboSection = null!;
    private TextBox txtQuery = null!;
    private Button btnSearch = null!;
    private DataGridView dgvStudents = null!;
    private Label lblCount = null!;
    private Button btnEdit = null!;
    private Button btnLedger = null!;
    private Button btnCollect = null!;
    private Button btnAdmit = null!;

    private void InitializeComponent()
    {
        SuspendLayout();
        Text = "Student Search";
        BackColor = UITheme.Canvas;

        lblTitle = new Label
        {
            Text = "Student Search & Records",
            Font = UITheme.FontHeader,
            ForeColor = UITheme.TextPrimary,
            Location = new Point(16, 12),
            AutoSize = true
        };

        pnlFilter = new Panel
        {
            Tag = "card",
            BackColor = UITheme.Card,
            Location = new Point(16, 50),
            Size = new Size(960, 70)
        };
        cboClass = new ComboBox { Location = new Point(16, 22), Width = 160, DropDownStyle = ComboBoxStyle.DropDownList, FlatStyle = FlatStyle.Flat };
        cboSection = new ComboBox { Location = new Point(190, 22), Width = 140, DropDownStyle = ComboBoxStyle.DropDownList, FlatStyle = FlatStyle.Flat };
        txtQuery = new TextBox { Location = new Point(350, 22), Width = 320, PlaceholderText = "Name / Roll / Mobile / Reg ID" };
        btnSearch = new Button { Text = "Search", Location = new Point(690, 18), Size = new Size(110, 34) };
        btnSearch.Click += btnSearch_Click;
        pnlFilter.Controls.AddRange(new Control[] { cboClass, cboSection, txtQuery, btnSearch });

        dgvStudents = new DataGridView
        {
            Location = new Point(16, 140),
            Size = new Size(960, 360),
            ReadOnly = true
        };
        dgvStudents.Columns.Add("Avatar", "");
        dgvStudents.Columns.Add("RegId", "Reg ID");
        dgvStudents.Columns.Add("Name", "Full Name");
        dgvStudents.Columns.Add("Class", "Class");
        dgvStudents.Columns.Add("Roll", "Roll");
        dgvStudents.Columns.Add("Mobile", "Guardian Mobile");
        dgvStudents.Columns.Add("AdmDate", "Admission Date");
        dgvStudents.Columns.Add("Status", "Status");
        dgvStudents.Columns[0].Width = 40;

        lblCount = new Label { Text = "0 record(s)", Location = new Point(16, 515), AutoSize = true, ForeColor = UITheme.TextMuted };
        btnEdit = new Button { Text = "Edit Profile", Location = new Point(16, 545), Size = new Size(130, 36), Tag = "ghost" };
        btnEdit.Click += btnEdit_Click;
        btnLedger = new Button { Text = "View Financial Ledger", Location = new Point(160, 545), Size = new Size(180, 36), Tag = "ghost" };
        btnLedger.Click += btnLedger_Click;
        btnCollect = new Button { Text = "Collect Fee", Location = new Point(350, 545), Size = new Size(130, 36) };
        btnCollect.Click += btnCollect_Click;
        btnAdmit = new Button { Text = "Print Admit Card", Location = new Point(495, 545), Size = new Size(150, 36), Tag = "success" };
        btnAdmit.Click += btnAdmit_Click;

        Controls.AddRange(new Control[] { lblTitle, pnlFilter, dgvStudents, lblCount, btnEdit, btnLedger, btnCollect, btnAdmit });
        ResumeLayout(false);
    }
}
