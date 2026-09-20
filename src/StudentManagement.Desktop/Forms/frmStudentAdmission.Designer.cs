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
        private Label lblTitle;
        private GroupBox grpPersonal;
        private GroupBox grpAddress;
        private GroupBox grpAcademic;
        private TextBox txtFullName;
        private TextBox txtFatherName;
        private TextBox txtMotherName;
        private ComboBox cboBloodGroup;
        private ComboBox cboGender;
        private DateTimePicker dtpDateOfBirth;
        private TextBox txtAge;
        private TextBox txtGuardianPhone;
        private TextBox txtPresentAddress;
        private TextBox txtPermanentAddress;
        private CheckBox chkSameAddress;
        private ComboBox cboClass;
        private ComboBox cboSection;
        private TextBox txtRollNumber;
        private TextBox txtTuitionFee;
        private Button btnSave;
        private Button btnReset;

        private void InitializeComponent()
        {
            SuspendLayout();
            Text = "Student Admission";
            AutoScroll = true;
            BackColor = UITheme.Canvas;

            lblTitle = new Label
            {
                Text = "Student Registration & Admission Form",
                Font = UITheme.FontHeader,
                ForeColor = UITheme.TextPrimary,
                Location = new Point(16, 12),
                AutoSize = true
            };
            var lblCap = new Label
            {
                Text = "Capture student biodata, guardian contacts, and class placement.",
                Tag = "muted",
                ForeColor = UITheme.TextMuted,
                Location = new Point(16, 44),
                AutoSize = true
            };

            grpPersonal = Section("1. STUDENT INFORMATION", new Point(16, 72), new Size(920, 180));
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

            grpAddress = Section("2. ADDRESS DETAILS", new Point(16, 262), new Size(920, 170));
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

            grpAcademic = Section("3. ACADEMIC ENROLLMENT", new Point(16, 442), new Size(920, 130));
            cboClass = Combo(grpAcademic, "Admission Class", 20, 40, 180);
            cboSection = Combo(grpAcademic, "Section", 220, 40, 120);
            txtRollNumber = Field(grpAcademic, "Roll Number", 360, 40, 120);
            txtTuitionFee = Field(grpAcademic, "Monthly Tuition Fee (৳)", 500, 40, 160);
            txtTuitionFee.Text = "2500";

            btnSave = new Button { Text = "Save & Complete Admission", Location = new Point(16, 592), Size = new Size(220, 40) };
            btnSave.Click += btnSave_Click;
            btnReset = new Button { Text = "Reset Form", Location = new Point(250, 592), Size = new Size(140, 40), Tag = "ghost" };
            btnReset.Click += btnReset_Click;

            Controls.AddRange(new Control[] { lblTitle, lblCap, grpPersonal, grpAddress, grpAcademic, btnSave, btnReset });
            ResumeLayout(false);
        }

private static GroupBox Section(string title, Point location, Size size)
        {
            GroupBox group = new GroupBox();
            group.Text = title;
            group.Location = location;
            group.Size = size;
            group.BackColor = UITheme.Card;
            group.ForeColor = UITheme.Primary;
            group.Font = UITheme.FontSection;
            return group;
        }

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
