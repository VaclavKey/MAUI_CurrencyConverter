using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;

namespace AndroidConverter.Droid
{
    [Activity(Theme = "@style/MainTheme", MainLauncher = true)]
    public class MainActivity : MauiAppCompatActivity
    {
        public override bool DispatchTouchEvent(MotionEvent? e)
        {
            if (e.PointerCount > 1)
            {
                return false;
            }

            return base.DispatchTouchEvent(e);
        }

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            RequestWindowFeature(Android.Views.WindowFeatures.NoTitle);

            base.OnCreate(savedInstanceState);

            Window.SetFlags(Android.Views.WindowManagerFlags.Fullscreen, Android.Views.WindowManagerFlags.Fullscreen);
        }
    }
}
