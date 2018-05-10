using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace PayDebt
{
    class ContantPicker : Activity
    {
        protected override void OnCreate(Bundle state)
        {
            //contactNumber = FindViewById<TextView>(Resources.Id.contactnumber);

            //var buttonPickContact = FindViewById<Button>(Resource.Id.pickcontact);
            //buttonPickContact.Click += (sender, args) => TryGetContanct();
        }

        private TextView contactNumber;

        private void TryGetContanct()
        {
            if (CheckPermission())
                StartActivityForResult(new Intent(Intent.ActionPick, ContactsContract.Contacts.ContentUri), 1);
        }

        const string permission = Manifest.Permission.ReadContacts;
        const int RequestLocationId = 0;

        private bool CheckPermission()
        {
            if (CheckSelfPermission(permission) == Permission.Granted) return true;
            RequestPermissions(new[] {permission}, RequestLocationId);
            return CheckSelfPermission(permission) == Permission.Granted;
        }


        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            Log.Debug("PayDebt", "RequestCode: " + requestCode);
            if (resultCode == Result.Ok && requestCode == 1)
            {
                var contact = long.Parse(data.Data.LastPathSegment);
                // Sets the columns to retrieve for the user profile
                var projection = new String[]
                {
                    ContactsContract.Profile.InterfaceConsts.Id,
                    ContactsContract.Profile.InterfaceConsts.DisplayNamePrimary,
                    ContactsContract.Profile.InterfaceConsts.LookupKey,
                };

                // Retrieves the profile from the Contacts Provider
                var cursor =
                    ManagedQuery(
                        ContactsContract.Contacts.ContentUri,
                        projection,
                        null,
                        null,
                        null);

                var sb = new StringBuilder();
                if (cursor.MoveToFirst())
                {
                    do
                    {
                        var id = cursor.GetLong(cursor.GetColumnIndex(projection[0]));
                        var name = cursor.GetString(cursor.GetColumnIndex(projection[1]));
                        var photo = cursor.GetString(cursor.GetColumnIndex(projection[2]));

                        if (id == contact)
                        {
                            sb.AppendLine(string.Join(", ", id, name, photo));
                        }
                    } while (cursor.MoveToNext());

                    contactNumber.SetText(sb.ToString(), TextView.BufferType.Normal);
                }
            }
        }
    }
}