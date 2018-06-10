using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;
using Infrastructure;
using Org.Json;
using PayDebt.AndroidInfrastructure;
using VKontakte.API;

namespace PayDebt
{

    [Activity(Name= "ru.leoltron.PayDebt.VkFriendPickerActivity", Label = "", Theme = "@style/DesignTheme1")]
    public class VkFriendPickerActivity : Activity
    {
        public const string IntentExtraNameKey = "VKFriendName";
        public const string IntentExtraIdKey = "VKFriendId";

        private List<string> names = new List<string>();
        private List<string> ids = new List<string>();

#pragma warning disable 414
        private bool isLoading = true;
#pragma warning restore 414

        private ProgressBar loadingProgressBar;

        private LinearLayout friendListLl;
        private ListView friendList;
        private EditText searchEditText;
        private ArrayAdapter<string> friendListAdapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.VkFriendPickerActivityLayout);
            SetResult(Result.Canceled);

            FindViewById<Button>(Resource.Id.update).Click += (sender, args) => UpdateFriendList();

            friendList = FindViewById<ListView>(Resource.Id.friendsListView);
            friendListAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, names);
            friendList.Adapter = friendListAdapter;
            friendList.ItemClick += FriendListOnItemClick;

            loadingProgressBar = FindViewById<ProgressBar>(Resource.Id.friendsProgressBar);
            friendListLl = FindViewById<LinearLayout>(Resource.Id.friendsListLL);

            searchEditText = FindViewById<EditText>(Resource.Id.searchEditText);
            searchEditText.TextChanged += (sender, args) => FilterFriendList();

            InitActionBar();
            UpdateFriendList();
        }

        private void FilterFriendList()
        {
            friendListAdapter.Clear();
            foreach (var name in names.GetAllStartsWith(searchEditText.Text))
                friendListAdapter.Add(name);
            friendListAdapter.NotifyDataSetInvalidated();
        }

        private void FriendListOnItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var intent = new Intent();
            intent.PutExtra(IntentExtraNameKey, names[e.Position]);
            intent.PutExtra(IntentExtraIdKey, ids[e.Position]);
            SetResult(Result.Ok, intent);
            Finish();
        }

        private void SwitchToLoading()
        {
            isLoading = true;
            friendListLl.Visibility = ViewStates.Gone;
            loadingProgressBar.Visibility = ViewStates.Visible;
        }

        private void SwitchToLoaded()
        {
            isLoading = false;
            friendListLl.Visibility = ViewStates.Visible;
            loadingProgressBar.Visibility = ViewStates.Gone;
        }

        private void UpdateFriendList()
        {
            SwitchToLoading();
            var parameters = new VKParameters();
            //parameters.Put(VKApiConst.UserId, VKApiConst.OwnerId);
            parameters.Put("order", "hints");
            parameters.Put(VKApiConst.Fields, "first_name,last_name");
            var request = VKApi.Friends.Get(parameters);
            request.Attempts = 5;
            request.ExecuteWithListener(new VkRequestListener(AttemptFailed, OnComplete));
        }

        private void AttemptFailed(VKRequest request, int attemptNumber, int totalAttempts)
        {
            if (totalAttempts == 5)
            {
                Toast.MakeText(this, GetString(Resource.String.friend_list_load_failed), ToastLength.Short).Show();
                SwitchToLoaded();
            }
        }

        private void OnComplete(VKResponse response)
        {
            var json = response.Json;
            if (!json.Has("response"))
            {
                Log.Error("PayDebt", "Invalid API response: " + response.Json);
                return;
            }

            json = json.GetJSONObject("response");

            var count = json.Has("count") ? json.GetInt("count") : 10;
            if (json.Has("items"))
            {
                names = new List<string>(count);
                ids = new List<string>(count);
                var jsonArray = json.GetJSONArray("items");
                jsonArray.Length();
                for (int i = 0; i < jsonArray.Length(); i++)
                    AddFriend(jsonArray.GetJSONObject(i));

                FilterFriendList();
            }


            SwitchToLoaded();
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

        private void AddFriend(JSONObject obj)
        {
            if (!obj.Has("id")) return;

            var firstName = obj.Has("first_name") ? obj.GetString("first_name") : "";
            var lastName = obj.Has("last_name") ? obj.GetString("last_name") : "";

            var name = string.Join(" ", new[] {firstName, lastName}.GetAllNonEmpty());

            names.Add(name);
            ids.Add(obj.GetInt("id").ToString());
        }

        private void InitActionBar()
        {
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);
        }
    }
}