using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using PayDebt.AndroidInfrastructure;
using PayDebt.Model;
using VKontakte;

namespace PayDebt.Application.Activities
{
    [Activity(Name = "ru.leoltron.PayDebt.SettingsActivity", Label = "SettingsActivity", Theme = "@style/DesignTheme1")]
    public class SettingsActivity : Activity
    {
        private bool defaultCurrencyChanged = false;
        private CurrencySpinner defaultCurrencySpinner;

        private Button switchVkUsingButton;

        private bool messageTemplateChanged = false;
        private EditText messageTemplateEditText;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SettingsActivityLayout);
            InitActionBar();
            
            FindViewById<TextView>(Resource.Id.messageTemplateTextView)
                .FormatText(Constants.MessageTemplateAmountOfDebtSymbol, Constants.ScreenSymbol);

            InitCurrencySpinner();
            InitMessageTemplateEditText();
            InitVkLoginButton();
        }

        private void InitVkLoginButton()
        {
            switchVkUsingButton = FindViewById<Button>(Resource.Id.vkConnectButton);
            switchVkUsingButton.Click += (sender, args) => SwitchVkLoggedInState();
            UpdateVkButton();
        }

        private void SwitchVkLoggedInState()
        {
            if (VKSdk.IsLoggedIn)
                VKSdk.Logout();
            else
                VKSdk.Login(this, "friends", "messages");
            UpdateVkButton();
        }

        private void InitActionBar()
        {
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);
            SetTitle(Resource.String.settings);
        }

        private void InitMessageTemplateEditText()
        {
            messageTemplateEditText = FindViewById<EditText>(Resource.Id.messageTemplateEditText);
            var sharedPref = SharedPrefExtensions.GetAppSharedPref(this);
            messageTemplateEditText.Text = sharedPref.GetMessageTemplate();
            messageTemplateEditText.TextChanged += (sender, args) => messageTemplateChanged = true;
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

        private void InitCurrencySpinner()
        {
            defaultCurrencySpinner = FindViewById<CurrencySpinner>(Resource.Id.defaultCurrencySpinner);
            defaultCurrencySpinner.ItemSelected += (sender, args) => defaultCurrencyChanged = true;
            defaultCurrencySpinner.Initialize(this);
        }

        protected override void OnDestroy()
        {
            using (var editor = SharedPrefExtensions.GetAppSharedPref(this).Edit())
            {
                if (defaultCurrencyChanged)
                    editor.PutDefaultCurrency((string) defaultCurrencySpinner.SelectedItem);

                if (messageTemplateChanged)
                    editor.PutMessageTemplate(messageTemplateEditText.Text);

                editor.Commit();
            }

            base.OnDestroy();
        }

        private void UpdateVkButton()
        {
            switchVkUsingButton.Text =
                GetString(VKSdk.IsLoggedIn ? Resource.String.vk_logout : Resource.String.vk_login);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            UpdateVkButton();
            base.OnActivityResult(requestCode, resultCode, data);
        }
    }
}