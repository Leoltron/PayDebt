using System;
using Android.App;
using DebtModel;
using VKontakte;

namespace PayDebt.Model
{
    [Serializable]
    public class VkContactPicker : ContactPicker<VkContact>
    {
        public VkContactPicker(IContactProvider<VkContact> provider, string name) 
            : base(provider, name)
        {
        }

        public override bool IsAuthorized => VKSdk.IsLoggedIn;

        public override Action<Activity> LogIn()
        {
            if (IsAuthorized)
                return x => { };
            return activity => VKSdk.Login(activity, "friends", "messages");
        }

        public override void LogOut()
        {
            VKSdk.Logout();
        }
    }
}