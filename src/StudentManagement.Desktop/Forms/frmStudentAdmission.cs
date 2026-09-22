using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using StudentManagement.Application.DTOs;
using StudentManagement.Application.Security;
using StudentManagement.Application.Services;
using StudentManagement.Desktop.Helpers;
using StudentManagement.Desktop.Theme;
using StudentManagement.Domain.Enums;

namespace StudentManagement.Desktop.Forms
{
    public partial class frmStudentAdmission : Form
    {
        private StudentService? _students;
        private string? _selectedPhotoPath;

        /// <summary>Parameterless constructor required by the WinForms designer.</summary>
        public frmStudentAdmission()
        {
            InitializeComponent();
            if (!DesignTime.IsActive)
                UITheme.ApplyForm(this);
        }

        public frmStudentAdmission(StudentService students) : this()
        {
            _students = students;
            if (!DesignTime.IsActive)
                UITheme.ApplyForm(this);
            Load += async (s, e) => await LoadLookupsAsync();
            dtpDateOfBirth.ValueChanged += (s, e) => UpdateAge();
        }

        private async Task LoadLookupsAsync()
        {
            if (_students == null)
                return;

            cboBloodGroup.DataSource = Enum.GetValues(typeof(BloodGroup));
            cboGender.DataSource = Enum.GetValues(typeof(Gender));
            IReadOnlyList<ClassDto> classes = await _students.GetClassesAsync();
            cboClass.DisplayMember = nameof(ClassDto.ClassName);
            cboClass.ValueMember = nameof(ClassDto.ClassId);
            cboClass.DataSource = classes.ToList();
            cboClass.SelectedIndexChanged += async (s, e) => await LoadSectionsAsync();
            if (classes.Count > 0)
                await LoadSectionsAsync();
            UpdateAge();
        }

        private async Task LoadSectionsAsync()
        {
            if (_students == null)
                return;
            if (!(cboClass.SelectedValue is int classId))
                return;
            IReadOnlyList<SectionDto> sections = await _students.GetSectionsAsync(classId);
            cboSection.DisplayMember = nameof(SectionDto.SectionName);
            cboSection.ValueMember = nameof(SectionDto.SectionId);
            cboSection.DataSource = sections.ToList();
        }

        private void UpdateAge()
        {
            txtAge.Text = AgeCalculator.Format(dtpDateOfBirth.Value.Date);
        }

        private void chkSameAddress_CheckedChanged(object? sender, EventArgs e)
        {
            if (chkSameAddress.Checked)
            {
                txtPermanentAddress.Text = txtPresentAddress.Text;
                txtPermanentAddress.Enabled = false;
            }
            else
            {
                txtPermanentAddress.Enabled = true;
            }
        }

        private void btnBrowsePhoto_Click(object? sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Select Student Photo";
                dlg.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif|All Files|*.*";
                dlg.CheckFileExists = true;
                if (dlg.ShowDialog(this) != DialogResult.OK)
                    return;

                try
                {
                    if (picStudent.Image != null)
                    {
                        Image old = picStudent.Image;
                        picStudent.Image = null;
                        old.Dispose();
                    }

                    // Load a copy so the source file is not locked.
                    using (Image src = Image.FromFile(dlg.FileName))
                    {
                        picStudent.Image = new Bitmap(src);
                    }

                    _selectedPhotoPath = dlg.FileName;
                    lblPhotoHint.Text = Path.GetFileName(dlg.FileName);
                    lblPhotoHint.ForeColor = UITheme.Success;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not load image.\n" + ex.Message, "Photo Upload",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void btnClearPhoto_Click(object? sender, EventArgs e)
        {
            ClearPhoto();
        }

        private void ClearPhoto()
        {
            if (picStudent.Image != null)
            {
                Image old = picStudent.Image;
                picStudent.Image = null;
                old.Dispose();
            }
            _selectedPhotoPath = null;
            lblPhotoHint.Text = "No photo selected";
            lblPhotoHint.ForeColor = UITheme.TextMuted;
        }

        private string? SavePhotoForStudent(string registrationNo)
        {
            if (string.IsNullOrWhiteSpace(_selectedPhotoPath) || !File.Exists(_selectedPhotoPath))
                return null;

            string photosRoot = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "StudentManagement",
                "Photos");
            Directory.CreateDirectory(photosRoot);

            string ext = Path.GetExtension(_selectedPhotoPath);
            if (string.IsNullOrWhiteSpace(ext))
                ext = ".jpg";

            string dest = Path.Combine(photosRoot, registrationNo + ext);
            File.Copy(_selectedPhotoPath, dest, true);
            return dest;
        }

        private async void btnSave_Click(object? sender, EventArgs e)
        {
            if (_students == null)
                return;

            try
            {
                btnSave.Enabled = false;
                if (chkSameAddress.Checked)
                    txtPermanentAddress.Text = txtPresentAddress.Text;

                StudentDto result = await _students.AdmitAsync(new StudentAdmissionRequest
                {
                    FullName = txtFullName.Text,
                    FatherName = txtFatherName.Text,
                    MotherName = txtMotherName.Text,
                    BloodGroup = (BloodGroup)cboBloodGroup.SelectedItem!,
                    Gender = (Gender)cboGender.SelectedItem!,
                    DateOfBirth = dtpDateOfBirth.Value.Date,
                    GuardianPhone = txtGuardianPhone.Text,
                    PresentAddress = txtPresentAddress.Text,
                    PermanentAddress = txtPermanentAddress.Text,
                    ClassId = (int)cboClass.SelectedValue!,
                    SectionId = (int)cboSection.SelectedValue!,
                    RollNumber = txtRollNumber.Text,
                    MonthlyTuitionFee = decimal.Parse(txtTuitionFee.Text.Trim()),
                    AcademicSession = AppSession.Current != null ? AppSession.Current.AcademicSession : "2025-2026",
                    CreatedBy = AppSession.Current != null ? AppSession.Current.Username : null,
                    PhotoPath = _selectedPhotoPath
                });

                // Copy selected image into app Photos folder using registration ID.
                string? savedPhoto = SavePhotoForStudent(result.RegistrationNo);
                if (!string.IsNullOrEmpty(savedPhoto))
                {
                    result.PhotoPath = savedPhoto;
                    // Persist renamed path when backend is online.
                    try
                    {
                        result.PhotoPath = savedPhoto;
                        await _students.UpdateAsync(result);
                    }
                    catch
                    {
                        // Offline demo store may not persist photo path updates; UI still works.
                    }
                }

                MessageBox.Show(
                    "Student admitted successfully.\nRegistration ID: " + result.RegistrationNo +
                    (string.IsNullOrEmpty(savedPhoto) ? string.Empty : "\nPhoto saved."),
                    "Admission Saved",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                btnReset_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Admission Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnSave.Enabled = true;
            }
        }

        private void btnReset_Click(object? sender, EventArgs e)
        {
            txtFullName.Clear();
            txtFatherName.Clear();
            txtMotherName.Clear();
            txtGuardianPhone.Clear();
            txtPresentAddress.Clear();
            txtPermanentAddress.Clear();
            txtPermanentAddress.Enabled = true;
            chkSameAddress.Checked = false;
            txtRollNumber.Clear();
            txtTuitionFee.Text = "2500";
            dtpDateOfBirth.Value = DateTime.Today.AddYears(-12);
            ClearPhoto();
            UpdateAge();
            txtFullName.Focus();
        }
    }
}
