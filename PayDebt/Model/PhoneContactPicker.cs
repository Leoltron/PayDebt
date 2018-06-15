using System;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content.PM;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using DebtModel;
using PayDebt.Application.Activities;

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
            if (ContextCompat.CheckSelfPermission(activity,
                    Manifest.Permission.ReadContacts) == Permission.Granted)
            {
                UpdateLoggedInStatus(activity);
                return;
            }

            if (!ActivityCompat.ShouldShowRequestPermissionRationale(activity,
                Manifest.Permission.ReadContacts))
            {
                ShowPermissionDiolog(activity);
                UpdateLoggedInStatus(activity);
                return;
            }

            new AlertDialog.Builder(activity)
                .SetPositiveButton(
                    Android.Resource.String.Ok,
                    (sender, args) =>
                    {
                        ShowPermissionDiolog(activity);
                        UpdateLoggedInStatus(activity);
                    }
                ).SetNegativeButton(Android.Resource.String.Cancel, (s, a) => { })
                .SetMessage(Resource.String.read_contact_permission_required)
                .Show();
        }

        private void UpdateLoggedInStatus(Activity activity)
        {
            isLoggedIn = ContextCompat.CheckSelfPermission(activity, Manifest.Permission.ReadContacts) == Permission.Granted;
            if (activity is RefreshableActivity refreshableActivity)
                refreshableActivity.Refresh();
        }

        private void ShowPermissionDiolog(Activity activity)
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