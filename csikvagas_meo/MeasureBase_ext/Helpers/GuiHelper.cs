using System.ComponentModel;
using System.Windows;

namespace System.Windows.Controls
{
    public static class GuiHelper
    {
        public static void AutomaticRuntimeSize(this FrameworkElement fwe_in)
        {
            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                fwe_in.Width = double.NaN;
                fwe_in.Height = double.NaN;
            }
        }
    }
}

namespace System.ComponentModel
{
    public static class DesignMode
    {
        public static bool IsOn()
        {
#if DEBUG
            return LicenseManager.UsageMode == LicenseUsageMode.Designtime;
#else
			return false;
#endif
        }

        public static bool IsOn(DependencyObject obj_in)
        {
#if DEBUG
            return (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                || DesignerProperties.GetIsInDesignMode(obj_in);
#else
			return false;
#endif
        }
    }
}