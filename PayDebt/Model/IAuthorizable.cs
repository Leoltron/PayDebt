using Android.App;

namespace PayDebt.Model
{
    public interface IAuthorizable
    {
        bool IsAuthorized { get; }
        void LogIn(Activity activity);
        void LogOut();
    }
}