using IHelperServices;

namespace HelperServices
{
    public class GeneralUtilities : _HelperService, IGeneralUtilities
    {
        public void SetHijriCuluture()
        {
            //CultureInfo HijriCI = new CultureInfo("ar-SA", true);
            //HijriCalendar Hijri = new HijriCalendar();
            //Thread.CurrentThread.CurrentCulture = HijriCI;
            //Thread.CurrentThread.CurrentUICulture = HijriCI;
            //CultureInfo.CurrentCulture.DateTimeFormat.Calendar = Hijri;
            //CultureInfo.CurrentUICulture.DateTimeFormat.Calendar = Hijri;
        }

        public void SetGregCuluture()
        {
            //CultureInfo GregCI = new CultureInfo("ar-EG", true);
            //GregorianCalendar Greg = new GregorianCalendar();
            //Thread.CurrentThread.CurrentCulture = GregCI;
            //Thread.CurrentThread.CurrentUICulture = GregCI;
            //CultureInfo.CurrentCulture.DateTimeFormat.Calendar = Greg;
            //CultureInfo.CurrentUICulture.DateTimeFormat.Calendar = Greg;
        }
    }
}
