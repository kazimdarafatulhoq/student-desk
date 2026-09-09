using StudentManagement.Application.DTOs;
using StudentManagement.Application.Services;
using StudentManagement.Desktop.Helpers;
using StudentManagement.Desktop.Theme;
using StudentManagement.Domain.Entities;

namespace StudentManagement.Desktop.Forms;

public partial class frmExamClearance : Form
{
    private readonly StudentService _students;
    private readonly ExamService _exams;
    private List<StudentDto> _lookup = new();
    private List<ExamTerm> _terms = new();

    public frmExamClearance(StudentService students, ExamService exams)
    {
        _students = students;
        _exams = exams;
        InitializeComponent();
        UITheme.ApplyForm(this);
        Load += async (_, _) => await LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        _lookup = (await _students.GetLookupAsync()).ToList();
        cboStudent.DisplayMember = "Display";
        cboStudent.ValueMember = "StudentId";
        cboStudent.DataSource = _lookup.Select(s => new { s.StudentId, Display = $"{s.RegistrationNo} — {s.FullName}" }).ToList();

        _terms = (await _exams.GetActiveTermsAsync()).ToList();
        cboTerm.DisplayMember = nameof(ExamTerm.TermName);
        cboTerm.ValueMember = nameof(ExamTerm.ExamTermId);
        cboTerm.DataSource = _terms;
    }

    private async void btnVerify_Click(object? sender, EventArgs e)
    {
        try
        {
            if (cboStudent.SelectedValue is not int studentId || cboTerm.SelectedValue is not int termId)
                return;

            var result = await _exams.VerifyClearanceAsync(
                studentId,
                termId,
                chkOverride.Checked,
                txtOverrideReason.Text.Trim(),
                AppSession.Current?.UserId,
                AppSession.Current?.Role);

            if (!result.IsCleared)
            {
                lblResult.ForeColor = UITheme.Danger;
                lblResult.Text = $"BLOCKED — Outstanding dues ৳ {result.DuesAtClearance:N2}. Admit card cannot be issued unless an admin overrides.";
            }
            else
            {
                lblResult.ForeColor = UITheme.Success;
                lblResult.Text = result.AdminOverride
                    ? $"CLEARED with admin override (dues were ৳ {result.DuesAtClearance:N2})."
                    : "CLEARED — No outstanding dues. Eligible for admit card.";
            }
        }
        catch (Exception ex)
        {
            lblResult.ForeColor = UITheme.Danger;
            lblResult.Text = ex.Message;
        }
    }
}
