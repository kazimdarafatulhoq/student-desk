using StudentManagement.Application.DTOs;
using StudentManagement.Application.Services;
using StudentManagement.Desktop.Helpers;
using StudentManagement.Desktop.Theme;
using StudentManagement.Domain.Enums;
using StudentManagement.Reporting.Generators;

namespace StudentManagement.Desktop.Forms;

public partial class frmFeeCollection : Form
{
    private readonly StudentService _students;
    private readonly FeeService _fees;
    private StudentDto? _student;
    private readonly List<CheckBox> _monthChecks = new();

    public frmFeeCollection(StudentService students, FeeService fees)
    {
        _students = students;
        _fees = fees;
        InitializeComponent();
        UITheme.ApplyForm(this);
        cboPaymentMethod.DataSource = Enum.GetValues(typeof(PaymentMethod));
        BuildMonthMatrix();
        txtFine.TextChanged += (_, _) => Recalc();
        txtWaiver.TextChanged += (_, _) => Recalc();
    }

    private void BuildMonthMatrix()
    {
        flpMonths.Controls.Clear();
        _monthChecks.Clear();
        foreach (var month in FeeService.BuildMonthMatrix(DateTime.Now.Year))
        {
            var chk = new CheckBox
            {
                Text = month,
                AutoSize = true,
                ForeColor = UITheme.TextPrimary,
                Margin = new Padding(8, 6, 8, 6)
            };
            chk.CheckedChanged += (_, _) => Recalc();
            _monthChecks.Add(chk);
            flpMonths.Controls.Add(chk);
        }
    }

    private async void btnFind_Click(object? sender, EventArgs e)
    {
        try
        {
            _student = await _students.FindQuickAsync(txtSearch.Text);
            if (_student is null)
            {
                ClearBio();
                MessageBox.Show("Student not found.", "Search", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            lblBioName.Text = _student.FullName;
            lblBioReg.Text = _student.RegistrationNo;
            lblBioClass.Text = $"{_student.ClassName} / {_student.SectionName}  ·  Roll {_student.RollNumber}";
            lblBioDue.Text = $"Net Due: ৳ {_student.NetDue:N2}";
            lblBioDue.ForeColor = _student.NetDue > 0 ? UITheme.Danger : UITheme.Success;
            lblBioPhone.Text = _student.GuardianPhone;
            txtTuitionUnit.Text = _student.MonthlyTuitionFee.ToString("0.##");
            Recalc();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Search Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ClearBio()
    {
        _student = null;
        lblBioName.Text = "—";
        lblBioReg.Text = "—";
        lblBioClass.Text = "—";
        lblBioDue.Text = "—";
        lblBioPhone.Text = "—";
    }

    private void Recalc()
    {
        if (_student is null)
        {
            lblNetPayable.Text = "৳ 0.00";
            return;
        }

        var months = _monthChecks.Count(c => c.Checked);
        _ = decimal.TryParse(txtTuitionUnit.Text, out var unit);
        _ = decimal.TryParse(txtFine.Text, out var fine);
        _ = decimal.TryParse(txtWaiver.Text, out var waiver);
        var tuition = unit * months;
        var net = FeeService.CalculateNetPayable(tuition, fine, waiver);
        lblTuitionTotal.Text = $"Tuition: ৳ {tuition:N2}";
        lblNetPayable.Text = $"৳ {net:N2}";
        lblNetPayable.ForeColor = net > 0 ? UITheme.Success : UITheme.TextMuted;
    }

    private async void btnCollect_Click(object? sender, EventArgs e)
    {
        if (_student is null)
        {
            MessageBox.Show("Find a student first.", "Fee Collection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            btnCollect.Enabled = false;
            var periods = _monthChecks.Where(c => c.Checked).Select(c => c.Text).ToList();
            _ = decimal.TryParse(txtTuitionUnit.Text, out var unit);
            _ = decimal.TryParse(txtFine.Text, out var fine);
            _ = decimal.TryParse(txtWaiver.Text, out var waiver);

            var result = await _fees.CollectAsync(new FeeCollectionRequest
            {
                StudentId = _student.StudentId,
                FeePeriods = periods,
                TuitionAmount = unit * periods.Count,
                FineAmount = fine,
                WaiverAmount = waiver,
                PaymentMethod = (PaymentMethod)cboPaymentMethod.SelectedItem!,
                TransactionRef = txtTxnRef.Text.Trim(),
                Remarks = txtRemarks.Text.Trim(),
                CollectedByUserId = AppSession.Current?.UserId ?? 0
            });

            var pdf = ReceiptSpooler.GeneratePdf(result, AppSession.InstitutionName, AppSession.ReportsPath);
            MessageBox.Show($"Payment recorded.\nReceipt: {result.ReceiptNo}\nPDF: {pdf}",
                "Collection Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            _student = await _students.GetByIdAsync(_student.StudentId);
            if (_student is not null)
            {
                lblBioDue.Text = $"Net Due: ৳ {_student.NetDue:N2}";
                lblBioDue.ForeColor = _student.NetDue > 0 ? UITheme.Danger : UITheme.Success;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Collection Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            btnCollect.Enabled = true;
        }
    }
}
