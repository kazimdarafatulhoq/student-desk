using StudentManagement.Application.DTOs;
using StudentManagement.Application.Services;
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
    public partial class frmDashboard : Form
    {
        private readonly StudentService _students;
        private readonly FeeService _fees;

        public frmDashboard(StudentService students, FeeService fees)
        {
            _students = students;
            _fees = fees;
            InitializeComponent();
            UITheme.ApplyForm(this);
            Load += async (_, _) => await LoadStatsAsync();
        }

        private async Task LoadStatsAsync()
        {
            try
            {
                var list = await _students.SearchAsync(new StudentSearchFilter { Status = StudentStatus.Active });
                lblStudents.Text = list.Count.ToString("N0");
                var dues = list.Sum(s => s.NetDue);
                lblDues.Text = $"৳ {dues:N2}";
                lblPaidHint.Text = $"{list.Count(s => s.NetDue <= 0)} students clear";
                lblDueHint.Text = $"{list.Count(s => s.NetDue > 0)} with outstanding dues";
            }
            catch (Exception ex)
            {
                lblStudents.Text = "—";
                lblDues.Text = "—";
                lblPaidHint.Text = ex.Message;
            }
        }
    }
}
