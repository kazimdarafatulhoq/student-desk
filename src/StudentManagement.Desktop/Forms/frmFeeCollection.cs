using StudentManagement.Application.DTOs;
using StudentManagement.Application.Services;
using StudentManagement.Desktop.Helpers;
using StudentManagement.Desktop.Theme;
using StudentManagement.Domain.Enums;
using StudentManagement.Reporting.Generators;

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace StudentManagement.Desktop.Forms
{
    [DesignerCategory("Form")]
    public partial class frmFeeCollection : Form
    {
        private static readonly (string Key, string Label)[] OptionalFeeDefinitions =
        {
            ("RegistrationFee", "Registration Fee"),
            ("AdmissionFee", "New Admission / Re-admission"),
            ("TransportFee", "Monthly Transport Fee"),
            ("ExaminationFee", "Examination Fee (1st / 2nd Term / Annual / Test)"),
            ("TranscriptFee", "Transcript / Testimonial / Certificate Fee"),
            ("TransferCertificateFee", "Transfer Certificate / Certification Letter"),
            ("HostelFoodCharges", "Hostel Food Charges"),
            ("Miscellaneous", "Miscellaneous")
        };

        private StudentService? _students;
        private FeeService? _fees;
        private StudentDto? _student;
        private List<CheckBox> _monthChecks = new();
        private readonly Dictionary<string, TextBox> _optionalFeeBoxes = new();

        /// <summary>Parameterless constructor required by the WinForms designer.</summary>
        public frmFeeCollection()
        {
            InitializeComponent();
            if (!DesignTime.IsActive)
                UITheme.ApplyForm(this);
        }

        public frmFeeCollection(StudentService students, FeeService fees) : this()
        {
            _students = students;
            _fees = fees;
            if (!DesignTime.IsActive)
                UITheme.ApplyForm(this);
            cboPaymentMethod.DataSource = System.Enum.GetValues(typeof(StudentManagement.Domain.Enums.PaymentMethod));
            BuildMonthMatrix();
            BuildOptionalFeeFields();
            txtFine.TextChanged += (s, e) => Recalc();
            txtWaiver.TextChanged += (s, e) => Recalc();
            txtTuitionUnit.TextChanged += (s, e) => Recalc();
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

        private void BuildOptionalFeeFields()
        {
            flpOtherFees.Controls.Clear();
            _optionalFeeBoxes.Clear();
            flpOtherFees.WrapContents = true;
            flpOtherFees.FlowDirection = FlowDirection.LeftToRight;

            foreach (var def in OptionalFeeDefinitions)
            {
                var cell = new Panel
                {
                    Width = 228,
                    Height = 58,
                    Margin = new Padding(4, 4, 4, 4),
                    BackColor = Color.Transparent
                };

                var lbl = new Label
                {
                    AutoSize = false,
                    Text = def.Label,
                    ForeColor = UITheme.TextMuted,
                    Location = new Point(0, 0),
                    Size = new Size(220, 28),
                    Font = UITheme.FontBody
                };

                var txt = new TextBox
                {
                    Name = "txt" + def.Key,
                    Text = "0",
                    Location = new Point(0, 30),
                    Size = new Size(220, 23),
                    BorderStyle = BorderStyle.FixedSingle,
                    BackColor = UITheme.InputBack,
                    ForeColor = UITheme.TextPrimary
                };
                txt.TextChanged += (_, _) => Recalc();

                cell.Controls.Add(lbl);
                cell.Controls.Add(txt);
                flpOtherFees.Controls.Add(cell);
                _optionalFeeBoxes[def.Key] = txt;
            }
        }

        private List<NamedFeeAmount> ReadOptionalFees()
        {
            var list = new List<NamedFeeAmount>();
            foreach (var def in OptionalFeeDefinitions)
            {
                if (!_optionalFeeBoxes.TryGetValue(def.Key, out var box))
                    continue;
                if (!decimal.TryParse(box.Text, out var amount) || amount <= 0)
                    continue;
                list.Add(new NamedFeeAmount { Name = def.Label, Amount = amount });
            }
            return list;
        }

        private async void btnFind_Click(object? sender, EventArgs e)
        {
            if (_students == null)
                return;

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
                lblTuitionTotal.Text = "Tuition: ৳ 0.00";
                return;
            }

            var months = _monthChecks.Count(c => c.Checked);
            _ = decimal.TryParse(txtTuitionUnit.Text, out var unit);
            _ = decimal.TryParse(txtFine.Text, out var fine);
            _ = decimal.TryParse(txtWaiver.Text, out var waiver);
            var otherFees = ReadOptionalFees();
            var otherTotal = otherFees.Sum(f => f.Amount);
            var tuition = unit * months;
            var net = FeeService.CalculateNetPayable(tuition, fine, waiver, otherTotal);
            lblTuitionTotal.Text = otherTotal > 0
                ? $"Tuition: ৳ {tuition:N2}  ·  Other: ৳ {otherTotal:N2}"
                : $"Tuition: ৳ {tuition:N2}";
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

            if (_fees == null || _students == null)
                return;

            try
            {
                btnCollect.Enabled = false;
                var periods = _monthChecks.Where(c => c.Checked).Select(c => c.Text).ToList();
                _ = decimal.TryParse(txtTuitionUnit.Text, out var unit);
                _ = decimal.TryParse(txtFine.Text, out var fine);
                _ = decimal.TryParse(txtWaiver.Text, out var waiver);
                var otherFees = ReadOptionalFees();

                var result = await _fees.CollectAsync(new FeeCollectionRequest
                {
                    StudentId = _student.StudentId,
                    FeePeriods = periods,
                    TuitionAmount = unit * periods.Count,
                    FineAmount = fine,
                    WaiverAmount = waiver,
                    OtherFees = otherFees,
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
}
