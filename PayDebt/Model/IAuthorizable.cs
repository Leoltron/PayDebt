using System;
using Android.App;

namespace PayDebt.Model
{
    public interface IAuthorizable
    {
        bool IsAuthorized { get; }
        Action<Activity> LogIn();
        void LogOut();
    }
}