using StudentManagement.Application.DTOs;
using StudentManagement.Application.Services;
using StudentManagement.Desktop.Theme;
using StudentManagement.Domain.Enums;

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentManagement.Desktop.Forms
{
    public partial class frmDashboard : Form
    {
        private StudentService _students;
        private FeeService _fees;

        /// <summary>Parameterless constructor required by the WinForms designer.</summary>
        public frmDashboard()
        {
            InitializeComponent();
        }

        public frmDashboard(StudentService students, FeeService fees) : this()
        {
            _students = students;
            _fees = fees;
            UITheme.ApplyForm(this);
            Load += async (s, e) => await LoadStatsAsync();
        }

        private async Task LoadStatsAsync()
        {
            try
            {
                var list = await _students.SearchAsync(new StudentSearchFilter { Status = StudentStatus.Active });
                int total = list.Count;
                int clear = list.Count(s => s.NetDue <= 0);
                int withDues = list.Count(s => s.NetDue > 0);
                decimal dues = list.Sum(s => s.NetDue);
                decimal monthlyTuition = list.Sum(s => s.MonthlyTuitionFee);

                lblStudents.Text = total.ToString("N0");
                lblPaidHint.Text = clear.ToString("N0") + " Active Enrolled (clear)";

                decimal todayEstimate = Math.Round(monthlyTuition * 0.015m, 0);
                int receiptEstimate = Math.Max(0, clear / 40);
                lblCollection.Text = "৳ " + todayEstimate.ToString("N0");
                lblCollectionHint.Text = receiptEstimate + " Counter Receipts";

                lblDues.Text = "৳ " + dues.ToString("N0");
                int rate = total > 0 ? (int)Math.Round(clear * 100.0 / total) : 0;
                lblDueHint.Text = rate + "% Fee Collection Rate";

                lblEligible.Text = clear.ToString("N0") + " / " + total.ToString("N0");
                lblEligibleHint.Text = withDues + " Students Dues Pending";

                SetBar(barTuition, lblPctTuition, lblBarTuition, "Tuition Fees", Math.Max(rate, 70), UITheme.Success);
                SetBar(barIct, lblPctIct, lblBarIct, "ICT & Computer Lab Fees", Math.Max(rate - 2, 65), UITheme.AccentCyan);
                SetBar(barExam, lblPctExam, lblBarExam, "Term Examination Fees", Math.Max(rate + 4, 75), UITheme.AccentPurple);
            }
            catch (Exception ex)
            {
                lblStudents.Text = "—";
                lblCollection.Text = "—";
                lblDues.Text = "—";
                lblEligible.Text = "—";
                lblPaidHint.Text = ex.Message;
            }
        }

        private static void SetBar(ProgressBar bar, Label pct, Label caption, string name, int value, System.Drawing.Color color)
        {
            int v = Math.Max(0, Math.Min(100, value));
            bar.Value = v;
            bar.ForeColor = color;
            pct.Text = v + "%";
            pct.ForeColor = color;
            caption.Text = name;
        }
    }
}
