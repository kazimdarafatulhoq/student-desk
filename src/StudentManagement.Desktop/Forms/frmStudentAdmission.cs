using StudentManagement.Application.DTOs;
using StudentManagement.Application.Security;
using StudentManagement.Application.Services;
using StudentManagement.Desktop.Helpers;
using StudentManagement.Desktop.Theme;
using StudentManagement.Domain.Enums;

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
    public partial class frmStudentAdmission : Form
    {
        private StudentService _students;

        /// <summary>Parameterless constructor required by the WinForms designer.</summary>
        public frmStudentAdmission()
        {
            InitializeComponent();
        }

        public frmStudentAdmission(StudentService students) : this()
        {
            _students = students;
            UITheme.ApplyForm(this);
            Load += async (s, e) => await LoadLookupsAsync();
            dtpDateOfBirth.ValueChanged += (s, e) => UpdateAge();
            chkSameAddress.CheckedChanged += chkSameAddress_CheckedChanged;
        }

        private async Task LoadLookupsAsync()
        {
            cboBloodGroup.DataSource = Enum.GetValues(typeof(BloodGroup));
            cboGender.DataSource = Enum.GetValues(typeof(Gender));
            var classes = await _students.GetClassesAsync();
            cboClass.DisplayMember = nameof(ClassDto.ClassName);
            cboClass.ValueMember = nameof(ClassDto.ClassId);
            cboClass.DataSource = classes.ToList();
            cboClass.SelectedIndexChanged += async (_, _) => await LoadSectionsAsync();
            if (classes.Count > 0)
                await LoadSectionsAsync();
            UpdateAge();
        }

        private async Task LoadSectionsAsync()
        {
            if (cboClass.SelectedValue is not int classId)
                return;
            var sections = await _students.GetSectionsAsync(classId);
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

        private async void btnSave_Click(object? sender, EventArgs e)
        {
            try
            {
                btnSave.Enabled = false;
                if (chkSameAddress.Checked)
                    txtPermanentAddress.Text = txtPresentAddress.Text;

                var result = await _students.AdmitAsync(new StudentAdmissionRequest
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
                    AcademicSession = AppSession.Current?.AcademicSession ?? "2025-2026",
                    CreatedBy = AppSession.Current?.Username
                });

                MessageBox.Show($"Student admitted successfully.\nRegistration ID: {result.RegistrationNo}",
                    "Admission Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            UpdateAge();
            txtFullName.Focus();
        }
    }
}
