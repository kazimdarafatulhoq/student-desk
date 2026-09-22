using StudentManagement.Application.DTOs;
using StudentManagement.Application.Services;
using StudentManagement.Desktop.Helpers;
using StudentManagement.Desktop.Theme;
using StudentManagement.Domain.Entities;
using StudentManagement.Reporting.Generators;

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
    public partial class frmAdmitCardPrint : Form
    {
        private StudentService? _students;
        private ExamService? _exams;
        private List<StudentDto> _lookup = new();
        private List<ExamTerm> _terms = new();
        private List<ClassDto> _classes = new();

        /// <summary>Parameterless constructor required by the WinForms designer.</summary>
        public frmAdmitCardPrint()
        {
            InitializeComponent();
            if (!DesignTime.IsActive)
                UITheme.ApplyForm(this);
        }

        public frmAdmitCardPrint(StudentService students, ExamService exams) : this()
        {
            _students = students;
            _exams = exams;
            if (!DesignTime.IsActive)
                UITheme.ApplyForm(this);
            Load += async (s, e) => await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            if (_students == null || _exams == null)
                return;

            _lookup = (await _students.GetLookupAsync()).ToList();
            cboStudent.DisplayMember = "Display";
            cboStudent.ValueMember = "StudentId";
            cboStudent.DataSource = _lookup.Select(s => new { s.StudentId, Display = $"{s.RegistrationNo} — {s.FullName}" }).ToList();

            _terms = (await _exams.GetActiveTermsAsync()).ToList();
            cboTerm.DisplayMember = nameof(ExamTerm.TermName);
            cboTerm.ValueMember = nameof(ExamTerm.ExamTermId);
            cboTerm.DataSource = _terms;

            _classes = (await _students.GetClassesAsync()).ToList();
            _classes.Insert(0, new ClassDto { ClassId = 0, ClassName = "All Classes" });
            cboClass.DisplayMember = nameof(ClassDto.ClassName);
            cboClass.ValueMember = nameof(ClassDto.ClassId);
            cboClass.DataSource = _classes;
        }

        private async void btnIssue_Click(object? sender, EventArgs e)
        {
            if (_exams == null)
                return;

            try
            {
                if (cboStudent.SelectedValue is not int studentId || cboTerm.SelectedValue is not int termId)
                    return;

                var card = await _exams.IssueAdmitCardAsync(
                    studentId,
                    termId,
                    AppSession.Current?.UserId ?? 0,
                    chkOverride.Checked,
                    txtReason.Text.Trim(),
                    AppSession.Current?.Role);

                var path = AdmitCardPdfGenerator.GenerateBatch(new[] { card }, AppSession.InstitutionName, AppSession.ReportsPath);
                lblStatus.ForeColor = UITheme.Success;
                lblStatus.Text = $"Admit card {card.AdmitCardNo} issued. PDF: {path}";
            }
            catch (Exception ex)
            {
                lblStatus.ForeColor = UITheme.Danger;
                lblStatus.Text = ex.Message;
            }
        }

        private async void btnBatch_Click(object? sender, EventArgs e)
        {
            if (_students == null || _exams == null)
                return;

            try
            {
                if (cboTerm.SelectedValue is not int termId)
                    return;

                int? classId = cboClass.SelectedValue is int c && c > 0 ? c : null;
                var issued = new List<AdmitCardDto>();
                var students = await _students.SearchAsync(new StudentSearchFilter { ClassId = classId });

                foreach (var s in students)
                {
                    try
                    {
                        var card = await _exams.IssueAdmitCardAsync(
                            s.StudentId, termId, AppSession.Current?.UserId ?? 0,
                            chkOverride.Checked, txtReason.Text.Trim(), AppSession.Current?.Role);
                        issued.Add(card);
                    }
                    catch
                    {
                        // skip blocked students in batch
                    }
                }

                if (issued.Count == 0)
                {
                    lblStatus.ForeColor = UITheme.Warning;
                    lblStatus.Text = "No admit cards issued. Students may have outstanding dues.";
                    return;
                }

                var path = AdmitCardPdfGenerator.GenerateBatch(issued, AppSession.InstitutionName, AppSession.ReportsPath);
                lblStatus.ForeColor = UITheme.Success;
                lblStatus.Text = $"Batch printed: {issued.Count} card(s). PDF: {path}";
            }
            catch (Exception ex)
            {
                lblStatus.ForeColor = UITheme.Danger;
                lblStatus.Text = ex.Message;
            }
        }
    }
}
