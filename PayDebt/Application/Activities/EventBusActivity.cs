using Android.App;
using Android.Content.PM;

namespace PayDebt.Application.Activities
{
    public abstract class EventBusActivity : Activity
    {
        public delegate void OnRequestPermissionsResultEvent(int requestCode, string[] permissions,
            Permission[] grantResults);

        public event OnRequestPermissionsResultEvent RequestPermissionsResult;
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            RequestPermissionsResult?.Invoke(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}