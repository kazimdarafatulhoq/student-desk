using System.ComponentModel;

namespace StudentManagement.Desktop.Helpers
{
    /// <summary>
    /// Detects Visual Studio WinForms design-time so constructors can skip runtime-only setup.
    /// </summary>
    public static class DesignTime
    {
        public static bool IsActive
        {
            get { return LicenseManager.UsageMode == LicenseUsageMode.Designtime; }
        }
    }
}
