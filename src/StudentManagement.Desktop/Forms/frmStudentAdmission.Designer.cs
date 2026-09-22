using System;
using System.Drawing;
using System.Windows.Forms;

namespace StudentManagement.Desktop.Forms
{
    partial class frmStudentAdmission
    {
        private Label lblTitle;
        private Label lblCap;
        private GroupBox grpPersonal;
        private GroupBox grpAddress;
        private GroupBox grpAcademic;
        private GroupBox grpPhoto;
        private PictureBox picStudent;
        private Button btnBrowsePhoto;
        private Button btnClearPhoto;
        private Label lblPhotoHint;
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
            this.SuspendLayout();
            this.Text = "Student Admission";
            this.AutoScroll = true;
            this.BackColor = Color.FromArgb(9, 12, 23);
            this.ClientSize = new Size(1100, 720);
            this.Name = "frmStudentAdmission";

            this.lblTitle = new Label();
            this.lblTitle.Text = "Student Registration & Admission Form";
            this.lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            this.lblTitle.ForeColor = Color.White;
            this.lblTitle.Location = new Point(16, 12);
            this.lblTitle.AutoSize = true;

            this.lblCap = new Label();
            this.lblCap.Text = "Capture student photo, biodata, guardian contacts, and class placement.";
            this.lblCap.Tag = "muted";
            this.lblCap.ForeColor = Color.FromArgb(138, 147, 166);
            this.lblCap.Location = new Point(16, 44);
            this.lblCap.AutoSize = true;

            // Photo panel (right side)
            this.grpPhoto = Section("STUDENT PHOTO", new Point(960, 72), new Size(220, 280));
            this.picStudent = new PictureBox();
            this.picStudent.Location = new Point(20, 30);
            this.picStudent.Size = new Size(180, 180);
            this.picStudent.SizeMode = PictureBoxSizeMode.Zoom;
            this.picStudent.BackColor = Color.FromArgb(17, 24, 39);
            this.picStudent.BorderStyle = BorderStyle.FixedSingle;
            this.lblPhotoHint = new Label();
            this.lblPhotoHint.Text = "No photo selected";
            this.lblPhotoHint.Tag = "muted";
            this.lblPhotoHint.ForeColor = Color.FromArgb(138, 147, 166);
            this.lblPhotoHint.Location = new Point(20, 215);
            this.lblPhotoHint.AutoSize = true;
            this.btnBrowsePhoto = new Button();
            this.btnBrowsePhoto.Text = "Upload Photo";
            this.btnBrowsePhoto.Location = new Point(20, 238);
            this.btnBrowsePhoto.Size = new Size(110, 32);
            this.btnBrowsePhoto.Click += new EventHandler(this.btnBrowsePhoto_Click);
            this.btnClearPhoto = new Button();
            this.btnClearPhoto.Text = "Clear";
            this.btnClearPhoto.Tag = "ghost";
            this.btnClearPhoto.Location = new Point(136, 238);
            this.btnClearPhoto.Size = new Size(64, 32);
            this.btnClearPhoto.Click += new EventHandler(this.btnClearPhoto_Click);
            this.grpPhoto.Controls.Add(this.picStudent);
            this.grpPhoto.Controls.Add(this.lblPhotoHint);
            this.grpPhoto.Controls.Add(this.btnBrowsePhoto);
            this.grpPhoto.Controls.Add(this.btnClearPhoto);

            this.grpPersonal = Section("1. STUDENT INFORMATION", new Point(16, 72), new Size(920, 180));
            this.txtFullName = Field(this.grpPersonal, "Full Name", 20, 40, 260);
            this.txtFatherName = Field(this.grpPersonal, "Father's Name", 300, 40, 260);
            this.txtMotherName = Field(this.grpPersonal, "Mother's Name", 580, 40, 260);
            this.cboBloodGroup = Combo(this.grpPersonal, "Blood Group", 20, 100, 160);
            this.cboGender = Combo(this.grpPersonal, "Gender", 200, 100, 140);
            this.dtpDateOfBirth = new DateTimePicker();
            this.dtpDateOfBirth.Location = new Point(360, 120);
            this.dtpDateOfBirth.Size = new Size(160, 26);
            this.dtpDateOfBirth.Format = DateTimePickerFormat.Custom;
            this.dtpDateOfBirth.CustomFormat = "dd-MMM-yyyy";
            this.dtpDateOfBirth.Value = DateTime.Today.AddYears(-12);
            LabelAt(this.grpPersonal, "Date of Birth", 360, 100);
            this.grpPersonal.Controls.Add(this.dtpDateOfBirth);
            this.txtAge = Field(this.grpPersonal, "Age", 540, 100, 120);
            this.txtAge.ReadOnly = true;
            this.txtGuardianPhone = Field(this.grpPersonal, "Guardian Phone", 680, 100, 160);

            this.grpAddress = Section("2. ADDRESS DETAILS", new Point(16, 262), new Size(920, 170));
            this.txtPresentAddress = Multi(this.grpAddress, "Present Address", 20, 40, 420, 70);
            this.txtPermanentAddress = Multi(this.grpAddress, "Permanent Address", 460, 40, 420, 70);
            this.chkSameAddress = new CheckBox();
            this.chkSameAddress.Text = "Permanent same as Present";
            this.chkSameAddress.Location = new Point(20, 125);
            this.chkSameAddress.AutoSize = true;
            this.chkSameAddress.ForeColor = Color.White;
            this.grpAddress.Controls.Add(this.chkSameAddress);

            this.grpAcademic = Section("3. ACADEMIC ENROLLMENT", new Point(16, 442), new Size(920, 130));
            this.cboClass = Combo(this.grpAcademic, "Admission Class", 20, 40, 180);
            this.cboSection = Combo(this.grpAcademic, "Section", 220, 40, 120);
            this.txtRollNumber = Field(this.grpAcademic, "Roll Number", 360, 40, 120);
            this.txtTuitionFee = Field(this.grpAcademic, "Monthly Tuition Fee", 500, 40, 160);
            this.txtTuitionFee.Text = "2500";

            this.btnSave = new Button();
            this.btnSave.Text = "Save & Complete Admission";
            this.btnSave.Location = new Point(16, 592);
            this.btnSave.Size = new Size(220, 40);
            this.btnSave.Click += new EventHandler(this.btnSave_Click);

            this.btnReset = new Button();
            this.btnReset.Text = "Reset Form";
            this.btnReset.Location = new Point(250, 592);
            this.btnReset.Size = new Size(140, 40);
            this.btnReset.Tag = "ghost";
            this.btnReset.Click += new EventHandler(this.btnReset_Click);

            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblCap);
            this.Controls.Add(this.grpPersonal);
            this.Controls.Add(this.grpPhoto);
            this.Controls.Add(this.grpAddress);
            this.Controls.Add(this.grpAcademic);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnReset);
            this.ResumeLayout(false);
        }

        private static GroupBox Section(string title, Point location, Size size)
        {
            GroupBox group = new GroupBox();
            group.Text = title;
            group.Location = location;
            group.Size = size;
            group.BackColor = Color.FromArgb(13, 20, 48);
            group.ForeColor = Color.FromArgb(37, 99, 235);
            group.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            return group;
        }

        private static void LabelAt(Control parent, string text, int x, int y)
        {
            Label lbl = new Label();
            lbl.Text = text;
            lbl.Location = new Point(x, y);
            lbl.AutoSize = true;
            lbl.ForeColor = Color.FromArgb(138, 147, 166);
            parent.Controls.Add(lbl);
        }

        private static TextBox Field(Control parent, string label, int x, int y, int width)
        {
            LabelAt(parent, label, x, y);
            TextBox tb = new TextBox();
            tb.Location = new Point(x, y + 20);
            tb.Size = new Size(width, 26);
            tb.BackColor = Color.FromArgb(17, 24, 39);
            tb.ForeColor = Color.White;
            tb.BorderStyle = BorderStyle.FixedSingle;
            parent.Controls.Add(tb);
            return tb;
        }

        private static TextBox Multi(Control parent, string label, int x, int y, int width, int height)
        {
            LabelAt(parent, label, x, y);
            TextBox tb = new TextBox();
            tb.Location = new Point(x, y + 20);
            tb.Size = new Size(width, height);
            tb.Multiline = true;
            tb.ScrollBars = ScrollBars.Vertical;
            tb.BackColor = Color.FromArgb(17, 24, 39);
            tb.ForeColor = Color.White;
            tb.BorderStyle = BorderStyle.FixedSingle;
            parent.Controls.Add(tb);
            return tb;
        }

        private static ComboBox Combo(Control parent, string label, int x, int y, int width)
        {
            LabelAt(parent, label, x, y);
            ComboBox cb = new ComboBox();
            cb.Location = new Point(x, y + 20);
            cb.Size = new Size(width, 26);
            cb.DropDownStyle = ComboBoxStyle.DropDownList;
            cb.FlatStyle = FlatStyle.Flat;
            cb.BackColor = Color.FromArgb(17, 24, 39);
            cb.ForeColor = Color.White;
            parent.Controls.Add(cb);
            return cb;
        }
    }
}
