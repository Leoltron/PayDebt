using System;
using Android.App;
using DebtModel;

namespace PayDebt.Model
{
    public class PhoneContactPicker : ContactPicker<PhoneContact>
    {
        public PhoneContactPicker(IContactProvider<PhoneContact> provider, string name, int requestCode, Type activityType) 
            : base(provider, name, requestCode, activityType)
        {
        }

        public override bool IsAuthorized => true;

        public override void LogIn(Activity activity)
        {
        }

        public override void LogOut()
        {
        }
    }
}