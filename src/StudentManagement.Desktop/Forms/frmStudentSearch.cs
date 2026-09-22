using StudentManagement.Application.DTOs;
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
    public partial class frmStudentSearch : Form
    {
        private StudentService? _students;
        private List<StudentDto> _rows = new();

        public event Action<StudentDto>? OpenLedgerRequested;
        public event Action<StudentDto>? CollectFeeRequested;
        public event Action<StudentDto>? PrintAdmitRequested;

        /// <summary>Parameterless constructor required by the WinForms designer.</summary>
        public frmStudentSearch()
        {
            InitializeComponent();
            if (!DesignTime.IsActive)
                UITheme.ApplyForm(this);
        }

        public frmStudentSearch(StudentService students) : this()
        {
            _students = students;
            if (!DesignTime.IsActive)
                UITheme.ApplyForm(this);
            Load += async (s, e) =>
            {
                await LoadLookupsAsync();
                await SearchAsync();
            };
        }

        private async Task LoadLookupsAsync()
        {
            if (_students == null)
                return;

            var classes = (await _students.GetClassesAsync()).ToList();
            classes.Insert(0, new ClassDto { ClassId = 0, ClassName = "All Classes" });
            cboClass.DisplayMember = nameof(ClassDto.ClassName);
            cboClass.ValueMember = nameof(ClassDto.ClassId);
            cboClass.DataSource = classes;
            cboClass.SelectedIndexChanged += async (_, _) => await LoadSectionsAsync();
            await LoadSectionsAsync();
        }

        private async Task LoadSectionsAsync()
        {
            if (_students == null)
                return;

            var classId = cboClass.SelectedValue is int id ? id : 0;
            var sections = classId > 0
                ? (await _students.GetSectionsAsync(classId)).ToList()
                : new List<SectionDto>();
            sections.Insert(0, new SectionDto { SectionId = 0, SectionName = "All Sections" });
            cboSection.DisplayMember = nameof(SectionDto.SectionName);
            cboSection.ValueMember = nameof(SectionDto.SectionId);
            cboSection.DataSource = sections;
        }

        private async void btnSearch_Click(object? sender, EventArgs e) => await SearchAsync();

        private async Task SearchAsync()
        {
            if (_students == null)
                return;

            var filter = new StudentSearchFilter
            {
                Query = txtQuery.Text.Trim(),
                ClassId = cboClass.SelectedValue is int c && c > 0 ? c : null,
                SectionId = cboSection.SelectedValue is int s && s > 0 ? s : null
            };

            _rows = (await _students.SearchAsync(filter)).ToList();
            dgvStudents.Rows.Clear();
            foreach (var row in _rows)
            {
                dgvStudents.Rows.Add("●", row.RegistrationNo, row.FullName, row.ClassName, row.RollNumber,
                    row.GuardianPhone, row.AdmissionDate.ToString("dd-MMM-yyyy"), row.Status.ToString());
            }
            lblCount.Text = $"{_rows.Count} record(s)";
        }

        private StudentDto? Selected()
        {
            if (dgvStudents.CurrentRow is null || dgvStudents.CurrentRow.Index < 0 || dgvStudents.CurrentRow.Index >= _rows.Count)
                return null;
            return _rows[dgvStudents.CurrentRow.Index];
        }

        private void btnEdit_Click(object? sender, EventArgs e)
        {
            var s = Selected();
            if (s is null) return;
            MessageBox.Show($"Open editor for {s.RegistrationNo} — {s.FullName}\nUse Admission module values to update via service.",
                "Edit Profile", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnLedger_Click(object? sender, EventArgs e)
        {
            var s = Selected();
            if (s is null) return;
            OpenLedgerRequested?.Invoke(s);
            MessageBox.Show($"Navigate to Student Ledger for {s.RegistrationNo}", "View Ledger", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnCollect_Click(object? sender, EventArgs e)
        {
            var s = Selected();
            if (s is null) return;
            CollectFeeRequested?.Invoke(s);
            MessageBox.Show($"Navigate to Fee Collection for {s.RegistrationNo}", "Collect Fee", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnAdmit_Click(object? sender, EventArgs e)
        {
            var s = Selected();
            if (s is null) return;
            PrintAdmitRequested?.Invoke(s);
            MessageBox.Show($"Navigate to Admit Card for {s.RegistrationNo}", "Admit Card", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
