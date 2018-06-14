using System;
using Android.App;
using DebtModel;
using PayDebt.AndroidInfrastructure;
using VKontakte;
using VKontakte.API;

namespace PayDebt.Model
{
    [Serializable]
    public class VkContactPicker : ContactPicker<VkContact>
    {
        public VkContactPicker(IContactProvider<VkContact> provider, string name, int requestCode, Type type) 
            : base(provider, name, requestCode, type)
        {
        }

        public override bool CanSendMessage => true;
        public override bool IsAuthorized => VKSdk.IsLoggedIn;

        public override void LogIn(Activity activity)
        {
            if (IsAuthorized)
                return;
            VKSdk.Login(activity, "friends", "messages");
        }

        public override void LogOut()
        {
            VKSdk.Logout();
        }

        public override void SendMessage(Contact contact, string message, Activity activity)
        {
            if (!(contact is VkContact vkContact)) return;
            var vkParams = new VKParameters();
            vkParams.Put("user_id", vkContact.Id);
            vkParams.Put("message", message);
            new VKRequest("messages.send", vkParams)
                .ExecuteWithListener(new VkRequestListener(OnAttemptFailed(activity), OnRequestComplete(activity)));
        }

        private Action<VKResponse> OnRequestComplete(Activity activity)
        {
            return response => { };
        }

        private Action<VKRequest, int, int> OnAttemptFailed(Activity activity)
        {
            return (request, x, y) => { };
        }
    }
}