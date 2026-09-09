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
    partial class frmStudentAdmission
    {
        private Label lblTitle = null!;
        private GroupBox grpPersonal = null!;
        private GroupBox grpAddress = null!;
        private GroupBox grpAcademic = null!;
        private TextBox txtFullName = null!;
        private TextBox txtFatherName = null!;
        private TextBox txtMotherName = null!;
        private ComboBox cboBloodGroup = null!;
        private ComboBox cboGender = null!;
        private DateTimePicker dtpDateOfBirth = null!;
        private TextBox txtAge = null!;
        private TextBox txtGuardianPhone = null!;
        private TextBox txtPresentAddress = null!;
        private TextBox txtPermanentAddress = null!;
        private CheckBox chkSameAddress = null!;
        private ComboBox cboClass = null!;
        private ComboBox cboSection = null!;
        private TextBox txtRollNumber = null!;
        private TextBox txtTuitionFee = null!;
        private Button btnSave = null!;
        private Button btnReset = null!;

        private void InitializeComponent()
        {
            SuspendLayout();
            Text = "Student Admission";
            AutoScroll = true;
            BackColor = UITheme.Canvas;

            lblTitle = new Label
            {
                Text = "Student Admission",
                Font = UITheme.FontHeader,
                ForeColor = UITheme.TextPrimary,
                Location = new Point(16, 12),
                AutoSize = true
            };

            grpPersonal = Section("1. Student & Guardian Information", new Point(16, 50), new Size(920, 180));
            txtFullName = Field(grpPersonal, "Full Name", 20, 40, 260);
            txtFatherName = Field(grpPersonal, "Father's Name", 300, 40, 260);
            txtMotherName = Field(grpPersonal, "Mother's Name", 580, 40, 260);
            cboBloodGroup = Combo(grpPersonal, "Blood Group", 20, 100, 160);
            cboGender = Combo(grpPersonal, "Gender", 200, 100, 140);
            dtpDateOfBirth = new DateTimePicker
            {
                Location = new Point(360, 120),
                Size = new Size(160, 26),
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd-MMM-yyyy",
                Value = DateTime.Today.AddYears(-12)
            };
            LabelAt(grpPersonal, "Date of Birth", 360, 100);
            grpPersonal.Controls.Add(dtpDateOfBirth);
            txtAge = Field(grpPersonal, "Age", 540, 100, 120);
            txtAge.ReadOnly = true;
            txtGuardianPhone = Field(grpPersonal, "Guardian Phone", 680, 100, 160);

            grpAddress = Section("2. Address & Residence Details", new Point(16, 240), new Size(920, 170));
            txtPresentAddress = Multi(grpAddress, "Present Address", 20, 40, 420, 70);
            txtPermanentAddress = Multi(grpAddress, "Permanent Address", 460, 40, 420, 70);
            chkSameAddress = new CheckBox
            {
                Text = "Permanent same as Present",
                Location = new Point(20, 125),
                AutoSize = true,
                ForeColor = UITheme.TextPrimary
            };
            grpAddress.Controls.Add(chkSameAddress);

            grpAcademic = Section("3. Academic Enrollment & Fees", new Point(16, 420), new Size(920, 130));
            cboClass = Combo(grpAcademic, "Admission Class", 20, 40, 180);
            cboSection = Combo(grpAcademic, "Section", 220, 40, 120);
            txtRollNumber = Field(grpAcademic, "Roll Number", 360, 40, 120);
            txtTuitionFee = Field(grpAcademic, "Monthly Tuition Fee (৳)", 500, 40, 160);
            txtTuitionFee.Text = "2500";

            btnSave = new Button { Text = "Save to SQL Server", Location = new Point(16, 570), Size = new Size(180, 40) };
            btnSave.Click += btnSave_Click;
            btnReset = new Button { Text = "Reset Form", Location = new Point(210, 570), Size = new Size(140, 40), Tag = "ghost" };
            btnReset.Click += btnReset_Click;

            Controls.AddRange(new Control[] { lblTitle, grpPersonal, grpAddress, grpAcademic, btnSave, btnReset });
            ResumeLayout(false);
        }

        private static GroupBox Section(string title, Point location, Size size) =>
            new()
            {
                Text = title,
                Location = location,
                Size = size,
                BackColor = UITheme.Card,
                ForeColor = UITheme.TextPrimary,
                Font = UITheme.FontSubtitle
            };

        private static void LabelAt(Control parent, string text, int x, int y)
        {
            parent.Controls.Add(new Label { Text = text, Location = new Point(x, y), AutoSize = true, ForeColor = UITheme.TextMuted });
        }

        private static TextBox Field(Control parent, string label, int x, int y, int width)
        {
            LabelAt(parent, label, x, y);
            var tb = new TextBox { Location = new Point(x, y + 20), Size = new Size(width, 26), BackColor = UITheme.InputBack, ForeColor = UITheme.TextPrimary, BorderStyle = BorderStyle.FixedSingle };
            parent.Controls.Add(tb);
            return tb;
        }

        private static TextBox Multi(Control parent, string label, int x, int y, int width, int height)
        {
            LabelAt(parent, label, x, y);
            var tb = new TextBox
            {
                Location = new Point(x, y + 20),
                Size = new Size(width, height),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                BackColor = UITheme.InputBack,
                ForeColor = UITheme.TextPrimary,
                BorderStyle = BorderStyle.FixedSingle
            };
            parent.Controls.Add(tb);
            return tb;
        }

        private static ComboBox Combo(Control parent, string label, int x, int y, int width)
        {
            LabelAt(parent, label, x, y);
            var cb = new ComboBox
            {
                Location = new Point(x, y + 20),
                Size = new Size(width, 26),
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat,
                BackColor = UITheme.InputBack,
                ForeColor = UITheme.TextPrimary
            };
            parent.Controls.Add(cb);
            return cb;
        }
    }
}
