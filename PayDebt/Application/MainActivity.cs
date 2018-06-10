using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using DebtModel;
using Infrastructure;
using PayDebt.AndroidInfrastructure;
using VKontakte;
using Fragment = Android.Support.V4.App.Fragment;

namespace PayDebt.Application
{
    [Activity(Label = "PayDebt", MainLauncher = true, Name = "ru.leoltron.PayDebt.MainActivity",
        ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
    // ReSharper disable once UnusedMember.Global
    public class MainActivity : FragmentActivity
    {
        private const int AddDebtRequestCode = 512;
        private const int HistoryRequestCode = 874;

        public static IEntityStorageAccess<int, Debt> Storage;
        public static Debts Debts;

        private bool initialized;
        private TabLayout tabLayout;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.MainActivityLayout);

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


            VKSdk.Initialize(this);
            //VKSdk.Login(this);
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
            tabLayout.SetTabTextColors(Color.Aqua, Color.AntiqueWhite);
            var fragments = new Fragment[]
            {
                myDebtsListFragment = new DebtListFragment(Debts, debt => !debt.IsPaid && debt.IsMyDebt),
                theirDebtsListFragment = new DebtListFragment(Debts, debt => !debt.IsPaid && debt.IsTheirDebt)
            };
            var titles = CharSequence.ArrayFromStringArray(new[]
            {
                GetString(Resource.String.taken_debts),
                GetString(Resource.String.given_debts)
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

            if (item.ItemId == Resource.Id.menu_add)
            {
                StartActivityForResult(new Intent(this, typeof(AddDebtActivity)), AddDebtRequestCode);
                return true;
            }

            if (item.ItemId == Resource.Id.menu_settings)
            {
                StartActivity(new Intent(this, typeof(SettingsActivity)));
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