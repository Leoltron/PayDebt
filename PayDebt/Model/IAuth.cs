using Android.App;

namespace PayDebt.Model
{
    public interface IAuth
    {
        bool IsLoggedIn { get; }
        void LogIn(Activity activity);
        void LogOut();
    }
}