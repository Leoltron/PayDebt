using Android.App;

namespace PayDebt.Application.Activities
{
    public abstract class RefreshableActivity : Activity
    {
        public abstract void Refresh();
    }
}