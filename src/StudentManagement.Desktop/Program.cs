using System;
using System.Configuration;
using System.Windows.Forms;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StudentManagement.Application.Interfaces;
using StudentManagement.Application.Services;
using StudentManagement.Desktop.Forms;
using StudentManagement.Desktop.Helpers;
using StudentManagement.Desktop.Offline;
using StudentManagement.Infrastructure;
using StudentManagement.Infrastructure.Data;

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

            // OfflineMode = true → no SQL Server (UI / theme design).
            // Set to false later when you want the real backend.
            bool offline =
                string.Equals(configuration["App:OfflineMode"], "true", StringComparison.OrdinalIgnoreCase)
                || string.Equals(ConfigurationManager.AppSettings["OfflineMode"], "true", StringComparison.OrdinalIgnoreCase);

            AppSession.OfflineMode = offline;
            AppSession.InstitutionName = configuration["App:InstitutionName"] ?? AppSession.InstitutionName;

            var appConfigCs =
                ConfigurationManager.ConnectionStrings["StudentManagementDb"]?.ConnectionString
                ?? ConfigurationManager.ConnectionStrings["StudentManagementSDB"]?.ConnectionString;

            var services = new ServiceCollection();
            services.AddSingleton<IConfiguration>(configuration);

            if (offline)
            {
                services.AddSingleton<IUnitOfWork, OfflineUnitOfWork>();
                services.AddScoped<StudentService>();
                services.AddScoped<FeeService>();
                services.AddScoped<AuthService>();
                services.AddScoped<ExamService>();
            }
            else
            {
                services.AddInfrastructure(configuration, appConfigCs);
            }

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

            if (!offline)
            {
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
                        "\n\nOr set App:OfflineMode=true in appsettings.json to design the UI without SQL Server.",
                        "Database Setup Required",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
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
