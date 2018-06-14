using System;
using Android.App;
using DebtModel;

namespace PayDebt.Model
{
    [Serializable]
    public class ContactPicker<TContact> : BaseContactPicker<TContact>, IContactPicker<TContact>
        where TContact : Contact
    {
        public string Name { get; }
        public int RequestCode { get; }
        private static int nextRequestCode;


        public ContactPicker(IContactProvider<TContact> provider, string name) : base(provider)
        {
            Name = name;
            RequestCode = nextRequestCode++;
        }

        public virtual bool IsAuthorized { get; private set; } = true;
        public virtual Action<Activity> LogIn()
        {
            return activity => IsAuthorized = true;
        }

        public virtual void LogOut()
        {
            IsAuthorized = false;
        }
    }
}