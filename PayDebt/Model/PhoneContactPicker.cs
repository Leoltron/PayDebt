using System;
using Android;
using Android.App;
using Android.Content.PM;
using Android.Support.V4.App;
using Android.Support.V4.Content;
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
                    Manifest.Permission.ReadContacts) != Permission.Granted)
            {
                if (ActivityCompat.ShouldShowRequestPermissionRationale(activity,
                    Manifest.Permission.ReadContacts))
                {
                    new AlertDialog.Builder(activity)
                        .SetPositiveButton(
                            Android.Resource.String.Ok,
                            (sender, args) =>
                            {
                                ActivityCompat.RequestPermissions(activity,
                                    new[] {Manifest.Permission.ReadContacts},
                                    MyPermissionsRequestReadContacts);
                                LogIn(activity);
                            }
                        ).SetNegativeButton(Android.Resource.String.Cancel, (s, a) => { })
                        .SetMessage(Resource.String.read_contact_permission_required).Show();
                }
                else
                {
                    ActivityCompat.RequestPermissions(activity,
                        new[] {Manifest.Permission.ReadContacts},
                        MyPermissionsRequestReadContacts);
                    LogIn(activity);
                }
            }
            else
            {
                isLoggedIn = true;
                if (activity is RefreshableActivity refreshableActivity)
                    refreshableActivity.Refresh();
            }
        }

        public override void LogOut()
        {
            isLoggedIn = false;
        }
    }
}