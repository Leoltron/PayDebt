using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Widget;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Util;
using Android.Views;
using PayDebt.Resources.layout;
using static Android.Graphics.Color;

namespace PayDebt
{
    [Activity(Label = "PayDebt", MainLauncher = true, Icon = "@mipmap/icon", Name = "PayDebt.MainActivity")]
    // ReSharper disable once UnusedMember.Global
    public class MainActivity : FragmentActivity
    {
        private const int AddDebtRequestCode = 512;
        private const int HistoryRequestCode = 874;

        public static IDebtsStorageAccess Storage;
        public static Debts Debts;

        private bool initialized = false;
        private TabLayout tabLayout;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar);
            toolbar.MenuItemClick += (sender, args) => OnOptionsItemSelected(args.Item);
            ActionBar.Title = GetString(Resource.String.app_name);

            tabLayout = FindViewById<TabLayout>(Resource.Id.sliding_tabs);

            if (!initialized)
            {
                Storage = new SharedPrefDebts(GetPreferences(FileCreationMode.Private));
                Debts = Debts.LoadFrom(Storage);
                InitTabLayout();
            }
        }

        private DebtListFragment myDebtsListFragment;
        private DebtListFragment theirDebtsListFragment;

        public void UpdateFragments()
        {
            myDebtsListFragment.UpdateAdapter();
            theirDebtsListFragment.UpdateAdapter();
        }

        private void InitTabLayout()
        {
            tabLayout.SetTabTextColors(Aqua, AntiqueWhite);
            var fragments = new Android.Support.V4.App.Fragment[]
            {
                myDebtsListFragment = new DebtListFragment(Debts, debt => !debt.IsPaid && debt.IsMyDebt),
                theirDebtsListFragment = new DebtListFragment(Debts, debt => !debt.IsPaid && debt.IsTheirDebt)
            };
            var titles = CharSequence.ArrayFromStringArray(new[]
            {
                GetString(Resource.String.taken_debts),
                GetString(Resource.String.given_debts),
            });

            var viewPager = FindViewById<ViewPager>(Resource.Id.viewpager);
            viewPager.Adapter = new TabsFragmentPagerAdapter(SupportFragmentManager, fragments, titles);
            tabLayout.SetupWithViewPager(viewPager);
            initialized = true;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Resource.Id.menu_debt_history)
            {
                StartActivityForResult(new Intent(this, typeof(DebtHistoryActivity)), HistoryRequestCode);
                return true;
            }
            else if (item.ItemId == Resource.Id.menu_add)
            {
                StartActivityForResult(new Intent(this, typeof(AddDebtActivity)), AddDebtRequestCode);
                return true;
            }

            return false;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Debts.SaveTo(Storage);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            switch (requestCode)
            {
                case AddDebtRequestCode:
                case HistoryRequestCode:
                    if (resultCode == Result.Ok)
                        UpdateFragments();
                    break;
                default:
                    base.OnActivityResult(requestCode, resultCode, data);
                    break;
            }
        }
    }
}