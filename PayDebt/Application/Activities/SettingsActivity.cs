using System;
using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using DebtModel;
using PayDebt.AndroidInfrastructure;
using PayDebt.Model;

namespace PayDebt.Application.Activities
{
    [Activity(Name = "ru.leoltron.PayDebt.SettingsActivity", Label = "SettingsActivity", Theme = "@style/DesignTheme1")]
    public class SettingsActivity : Activity
    {
        private bool defaultCurrencyChanged = false;
        private CurrencySpinner defaultCurrencySpinner;

        private Button connectButton;
        private Button disconnectButton;

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
            InitConnectButton();
            InitDisconnectButton();
        }

        private void InitDisconnectButton()
        {
            disconnectButton = FindViewById<Button>(Resource.Id.disconnectButton);
            disconnectButton.Click += (sender, args) => ShowDisconnectDialog();
            UpdateDisconnectButton();
        }

        private void InitConnectButton()
        {
            connectButton = FindViewById<Button>(Resource.Id.connectButton);
            connectButton.Click += (sender, args) => ShowConnectDialog();
            UpdateConnectButton();
        }

        private void UpdateDisconnectButton()
        {
            disconnectButton.Enabled = ContactPickers.HasAnyConnected;
        }

        private void ShowDisconnectDialog()
        {
            ShowChoosePickerDialog(
                ContactPickers.All.Where(p => p.IsLoggedIn).ToArray(),
                picker =>
                {
                    picker.LogOut();
                    UpdateConnectionButtons();
                },
                GetString(Resource.String.disconnect_source)
                );
        }

        private void UpdateConnectButton()
        {
            connectButton.Enabled = ContactPickers.HasAnyNotConnected;
        }

        private void ShowConnectDialog()
        {
            ShowChoosePickerDialog(
                ContactPickers.All.Where(p => !p.IsLoggedIn).ToArray(),
                picker =>
                {
                    picker.LogIn(this);
                    UpdateConnectionButtons();
                },
                GetString(Resource.String.connect_source)
            );
        }

        private void ShowChoosePickerDialog(IContactPicker<Contact>[] pickers, Action<IContactPicker<Contact>> action,
            string title)
        {
            new AlertDialog.Builder(this)
                .SetTitle(title)
                .SetItems(pickers.Select(p => p.Name).ToArray(), (sender, args) => action(pickers[args.Which]))
                .SetNegativeButton(Android.Resource.String.Cancel, (sender, args) => { })
                .Show();
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


        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            UpdateConnectionButtons();
            base.OnActivityResult(requestCode, resultCode, data);
        }

        private void UpdateConnectionButtons()
        {
            UpdateConnectButton();
            UpdateDisconnectButton();
        }
    }
}