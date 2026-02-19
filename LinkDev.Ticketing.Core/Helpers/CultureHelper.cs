using System.Globalization;

namespace LinkDev.Ticketing.Core.Helpers
{
    public class CultureHelper
    {
        public static CultureInfo GetCulture()
        {
            return Thread.CurrentThread.CurrentUICulture;
        }

        public static void SetCulture(CultureInfo culture)
        {
            //Thread.CurrentThread.CurrentUICulture = culture;
            //Thread.CurrentThread.CurrentCulture = culture;
        }
    }
}
