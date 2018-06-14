using System;
using Android.App;
using DebtModel;

namespace PayDebt.Model
{
    [Serializable]
    public abstract class ContactPicker<TContact> : BaseContactPicker<TContact>, IContactPicker<TContact>
        where TContact : Contact
    {
        public string Name { get; }
        public int RequestCode { get; }
        public virtual bool CanSendMessage { get; } = false;
        public Type PickerActivityType { get; }

        protected ContactPicker(IContactProvider<TContact> provider, string name, int requestCode, Type pickerActivityType)
            : base(provider)
        {
            Name = name;
            RequestCode = requestCode;
            PickerActivityType = pickerActivityType;
        }

        public virtual bool IsLoggedIn { get; private set; }
        public virtual void LogIn(Activity activity)
        {
            IsLoggedIn = true;
        }

        public virtual void SendMessage(Contact contact, string message, Activity activity)
        {
        }

        public virtual void LogOut()
        {
            IsLoggedIn = false;
        }
    }
}