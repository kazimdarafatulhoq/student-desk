using StudentManagement.Application.DTOs;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
namespace StudentManagement.Desktop.Helpers
{
    public static class AppSession
    {
        public static AuthSession? Current { get; private set; }
        public static string InstitutionName { get; set; } = "Horizon Academy";
        public static string ReportsPath { get; set; } =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "StudentManagement", "Reports");

        public static bool IsAuthenticated => Current is not null;

        public static void Set(AuthSession session) => Current = session;

        public static void Clear() => Current = null;

        public static string RoleDisplay => Current?.Role.ToString() ?? "Guest";
    }
}
