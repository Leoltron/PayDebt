using System;
using Android.App;
using DebtModel;

namespace PayDebt.Model
{
    public interface IContactPicker<out TContact> : IBaseContactPicker<TContact>, IAuthorizable
        where TContact : Contact
    {
        string Name { get; }
        int RequestCode { get; }
        bool CanSendMessage { get; }
        Type ActivityType { get; }
        void SendMessage(Contact contact, string message, Activity activity);
    }
}