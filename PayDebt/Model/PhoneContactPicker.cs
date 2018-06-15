using System;
using Android;
using Android.App;
using Android.Content.PM;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using DebtModel;
using PayDebt.AndroidInfrastructure;

namespace PayDebt.Model
{
    [Serializable]
    public class PhoneContactPicker : ContactPicker<PhoneContact>
    {
        private volatile bool isLoggedIn = false;
        private const int MyPermissionsRequestReadContacts = 575418974;

        public PhoneContactPicker(IContactProvider<PhoneContact> provider, string name, int requestCode,
            Type activityType)
            : base(provider, name, requestCode, activityType)
        {
        }

        public override bool IsLoggedIn => isLoggedIn;

        public override void LogIn(Activity activity)
        {
            if (activity is EventBusActivity eventBusActivity)
                eventBusActivity.RequestPermissionsResult += (code, permissions, results) =>
                {
                    if (code == MyPermissionsRequestReadContacts)
                        UpdateLoggedInStatus(activity);
                };

            if (ContextCompat.CheckSelfPermission(activity,
                    Manifest.Permission.ReadContacts) == Permission.Granted)
            {
                UpdateLoggedInStatus(activity);
                return;
            }

            if (!ActivityCompat.ShouldShowRequestPermissionRationale(activity,
                Manifest.Permission.ReadContacts))
            {
                ShowPermissionDialog(activity);
                UpdateLoggedInStatus(activity);
                return;
            }

            new AlertDialog.Builder(activity)
                .SetPositiveButton(
                    Android.Resource.String.Ok,
                    (sender, args) =>
                    {
                        ShowPermissionDialog(activity);
                        UpdateLoggedInStatus(activity);
                    }
                ).SetNegativeButton(Android.Resource.String.Cancel, (s, a) => { })
                .SetMessage(Resource.String.read_contact_permission_required)
                .Show();
        }

        private void UpdateLoggedInStatus(Activity activity)
        {
            isLoggedIn = ContextCompat.CheckSelfPermission(activity, Manifest.Permission.ReadContacts) == Permission.Granted;
            if (activity is IRefreshable refreshable)
                refreshable.Refresh();
        }

        private void ShowPermissionDialog(Activity activity)
        {
            ActivityCompat.RequestPermissions(activity,
                new[] { Manifest.Permission.ReadContacts },
                MyPermissionsRequestReadContacts);
        }

        public override void LogOut()
        {
            isLoggedIn = false;
        }
    }
}