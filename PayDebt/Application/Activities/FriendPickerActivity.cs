﻿using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using DebtModel;
using Infrastructure;
using PayDebt.Model;

namespace PayDebt.Application.Activities
{

    [Activity(Name = "ru.leoltron.PayDebt.FriendPickerActivity", Label = "", Theme = "@style/DesignTheme1")]
    public class FriendPickerActivity<TPicker, TContact> : Activity
        where TPicker : ContactPicker<TContact>
        where TContact : Contact
    {
#pragma warning disable 414
        private bool isLoading = true;
#pragma warning restore 414

        private ProgressBar loadingProgressBar;

        private LinearLayout friendListLl;
        private ListView friendList;
        private EditText searchEditText;
        private ArrayAdapter<string> friendListAdapter;
        private TPicker picker;
        private const string IntentExtraContactKey = "Contact";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            picker = Intent.GetByteArrayExtra("picker").FromBinary<TPicker>();
            SetContentView(Resource.Layout.FriendPickerActivityLayout);
            SetResult(Result.Canceled);

            FindViewById<Button>(Resource.Id.update).Click += (sender, args) => UpdateContacts();

            friendList = FindViewById<ListView>(Resource.Id.friendsListView);
            friendListAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, picker.Names);
            friendList.Adapter = friendListAdapter;
            friendList.ItemClick += FriendListOnItemClick;

            loadingProgressBar = FindViewById<ProgressBar>(Resource.Id.friendsProgressBar);
            friendListLl = FindViewById<LinearLayout>(Resource.Id.friendsListLL);

            InitSearchEditText();

            InitActionBar();
            UpdateContacts();
        }

        private void InitSearchEditText()
        {
            searchEditText = FindViewById<EditText>(Resource.Id.searchEditText);
            searchEditText.TextChanged += (sender, args) => FilterContacts();
        }

        private void FriendListOnItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var intent = new Intent();
            intent.PutExtra(IntentExtraContactKey, picker.DisplayedContacts[e.Position].SerializeToBytes());

            SetResult(Result.Ok, intent);
            Finish();
        }

        public override bool OnMenuItemSelected(int featureId, IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home)
            {
                Finish();
                return true;
            }

            return base.OnMenuItemSelected(featureId, item);
        }

        // ReSharper disable once MemberCanBePrivate.Global
        protected void FilterContacts()
        {
            friendListAdapter.Clear();
            picker.FilterContacts(searchEditText.Text);
            foreach (var name in picker.Names)
                friendListAdapter.Add(name);

            friendListAdapter.NotifyDataSetInvalidated();
        }

        // ReSharper disable once MemberCanBePrivate.Global
        protected void SwitchToLoading()
        {
            isLoading = true;
            friendListLl.Visibility = ViewStates.Gone;
            loadingProgressBar.Visibility = ViewStates.Visible;
        }

        // ReSharper disable once MemberCanBePrivate.Global
        protected void SwitchToLoaded()
        {
            isLoading = false;
            friendListLl.Visibility = ViewStates.Visible;
            loadingProgressBar.Visibility = ViewStates.Gone;
        }

        // ReSharper disable once MemberCanBePrivate.Global
        protected async void UpdateContacts()
        {
            SwitchToLoading();
            var updateSuccesfull = true;
            try
            {
                await picker.UpdateContactsAsync();
            }
            catch (SystemException)
            {
                updateSuccesfull = false;
                Toast.MakeText(this, GetString(Resource.String.friend_list_load_failed), ToastLength.Short).Show();
            }

            RunOnUiThread(() =>
            {
                if (updateSuccesfull)
                    FilterContacts();
                SwitchToLoaded();
            });
        }

        private void InitActionBar()
        {
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);
        }
    }

    public static class FriendPickerActivity
    {
        public const string IntentExtraContactKey = "Contact";
    }
}