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
        public Type ActivityType { get; }

        protected ContactPicker(IContactProvider<TContact> provider, string name, int requestCode, Type activityType) : base(provider)
        {
            Name = name;
            RequestCode = requestCode;
            ActivityType = activityType;
        }

        public virtual bool IsAuthorized { get; private set; }
        public virtual void LogIn(Activity activity)
        {
            IsAuthorized = true;
        }

        public virtual void SendMessage(Contact contact, string message, Activity activity)
        {
        }

        public virtual void LogOut()
        {
            IsAuthorized = false;
        }
    }
}