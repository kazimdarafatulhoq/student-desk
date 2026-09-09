using System.Windows.Forms;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StudentManagement.Desktop.Forms;
using StudentManagement.Infrastructure;
using StudentManagement.Infrastructure.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
namespace StudentManagement.Desktop
{
    internal static class Program
    {
        public static IServiceProvider Services { get; private set; } = null!;

        [STAThread]
        private static void Main()
        {
            System.Windows.Forms.Application.SetHighDpiMode(HighDpiMode.SystemAware);
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            var services = new ServiceCollection();
            services.AddSingleton<IConfiguration>(configuration);
            services.AddInfrastructure(configuration);
            services.AddTransient<frmLogin>();
            services.AddTransient<frmMainMenu>();
            services.AddTransient<frmStudentAdmission>();
            services.AddTransient<frmStudentSearch>();
            services.AddTransient<frmFeeCollection>();
            services.AddTransient<frmStudentLedger>();
            services.AddTransient<frmExamClearance>();
            services.AddTransient<frmAdmitCardPrint>();
            services.AddTransient<frmAppUser>();
            services.AddTransient<frmDashboard>();

            Services = services.BuildServiceProvider();

            try
            {
                using var scope = Services.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<StudentManagementDbContext>();
                DatabaseBootstrapper.InitializeAsync(db).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message +
                    "\n\nConnection string: appsettings.json / App.config" +
                    "\nSetup script: database\\StudentManagementDB.sql",
                    "Database Setup Required",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }

            using var login = Services.GetRequiredService<frmLogin>();
            if (login.ShowDialog() == DialogResult.OK)
            {
                var main = Services.GetRequiredService<frmMainMenu>();
                System.Windows.Forms.Application.Run(main);
            }
        }

        public static T GetService<T>() where T : notnull
            => Services.CreateScope().ServiceProvider.GetRequiredService<T>();
    }
}
