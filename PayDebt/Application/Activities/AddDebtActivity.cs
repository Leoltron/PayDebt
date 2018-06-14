﻿using System;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;
using DebtModel;
using Infrastructure;
using PayDebt.AndroidInfrastructure;
using PayDebt.Model;
using VKontakte;
using VKontakte.API;

namespace PayDebt.Application.Activities
{
    [Activity(Label = "AddDebtActivity", Name = "ru.leoltron.PayDebt.AddDebtActivity",
        ConfigurationChanges =
            Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
    public class AddDebtActivity : Activity, DatePickerDialog.IOnDateSetListener
    {
        private const int FindVkFriendRequestCode = 339000236;
           
        private Button finishButton;

        private EditText nameEditText;
        private EditText amountEditText;

        private LinearLayout messageLinearLayout;
        private EditText messageEditText;

        private Button inputTypeSwitchButton;

        private Button dateButton;
        private Switch dateSwitch;
        private DatePickerDialog datePickerDialog;
        private DateTime lastPaymentDateChoosed;

        private CurrencySpinner currencySpinner;

        private Switch isBorrowingDebtSwitch;

        private EditText commentEditText;

        private bool usingVkFriendAsName = false;
        private string lastVkFriendName = "";
        private string lastVkFriendId = "";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            SetContentView(Resource.Layout.AddDebtActivityLayout);
            base.OnCreate(savedInstanceState);
            SetResult(Result.Canceled);
            InitViews();
        }

        private void InitViews()
        {
            InitControlButtons();
            nameEditText = FindViewById<EditText>(Resource.Id.personNameEditText);
            isBorrowingDebtSwitch = FindViewById<Switch>(Resource.Id.selectDebtTypeSwitch);
            isBorrowingDebtSwitch.Checked = Intent.GetBooleanExtra(Constants.IsBorrowingintentExtraKey, false);
            InitPaymentInputArea();
            InitPaymentDateChoosingArea();
            commentEditText = FindViewById<EditText>(Resource.Id.commentEditText);
        }

        private void InitPaymentInputArea()
        {
            InitAmountEditText();
            InitCurrencySpinner();
            InitMessageViews();
        }

        private void InitMessageViews()
        {
            messageLinearLayout = FindViewById<LinearLayout>(Resource.Id.messageLinearLayout);
            messageLinearLayout.Visibility = VKSdk.IsLoggedIn ? ViewStates.Visible : ViewStates.Gone;
            messageEditText = FindViewById<EditText>(Resource.Id.messageEditText);
            messageEditText.Text = SharedPrefExtensions.GetAppSharedPref(this).GetMessageTemplate();
        }

        private void InitAmountEditText()
        {
            amountEditText = FindViewById<EditText>(Resource.Id.amountEditText);
            amountEditText.TextChanged += (sender, args) => finishButton.Enabled = GetAmount() != 0;
        }

        private void InitCurrencySpinner()
        {
            currencySpinner = FindViewById<CurrencySpinner>(Resource.Id.currencySpinner);
            currencySpinner.Initialize(this);
        }

        private void InitPaymentDateChoosingArea()
        {
            InitPaymentDateSwitch();
            InitDatePickerDialog();
            InitPaymentDateButton();
        }

        private void InitPaymentDateButton()
        {
            dateButton = FindViewById<Button>(Resource.Id.dateButton);
            dateButton.Click += (sender, args) => datePickerDialog.Show();
        }

        private void InitPaymentDateSwitch()
        {
            dateSwitch = FindViewById<Switch>(Resource.Id.hasPaymentDateSwitch);
            dateSwitch.CheckedChange += (sender, args) => UpdateDate();
        }

        private void InitDatePickerDialog()
        {
            var today = DateTime.Today;
            datePickerDialog = new DatePickerDialog(this, this, today.Year, today.Month, today.Day);
            datePickerDialog.DatePicker.MinDate = today.Millisecond;
            datePickerDialog.CancelEvent += (sender, args) => dateSwitch.Checked = false;
        }

        private void InitControlButtons()
        {
            FindViewById<Button>(Resource.Id.cancelDebtAddition).Click += (sender, args) => Finish();
            finishButton = FindViewById<Button>(Resource.Id.finishDebtAddition);
            finishButton.Enabled = false;
            finishButton.Click += (sender, args) => AddDebtAndFinish();

            inputTypeSwitchButton = FindViewById<Button>(Resource.Id.vkFriendsButton);
            inputTypeSwitchButton.Click += (sender, args) => SwitchNameInputType();
            UpdateButtons();
        }

        private void SendMessageToLastChoosedFriend(Money money)
        {
            if (string.IsNullOrWhiteSpace(lastVkFriendId)) return;
            var vkParams = new VKParameters();
            vkParams.Put("user_id", lastVkFriendId);
            vkParams.Put("message", GetFormattedMessage(money));
            new VKRequest("messages.send", vkParams).ExecuteWithListener(new VkRequestListener(OnAttemptFailed,
                OnRequestComplete));
        }

        private string GetFormattedMessage(Money money)
        {
            var moneyString = money.ToString();
            var message = messageEditText.Text;
            var sb = new StringBuilder();
            var screened = false;
            foreach (var c in message)
            {
                switch (c)
                {
                    case Constants.MessageTemplateAmountOfDebtSymbol:
                        sb.Append(screened ? c.ToString() : moneyString);
                        screened = false;
                        break;
                    case Constants.ScreenSymbol:
                        if (screened)
                            sb.Append(c.ToString());
                        screened = !screened;
                        break;
                    default:
                        sb.Append(c.ToString());
                        screened = false;
                        break;
                }
            }
            return sb.ToString();
        }

        private void OnRequestComplete(VKResponse response)
        {
            //Toast.MakeText(this, Resource.String.msg_sent_sucessfully, ToastLength.Short);
        }

        private void OnAttemptFailed(VKRequest request, int arg2, int arg3)
        {
            //Toast.MakeText(this, Resource.String.msg_sent_failed, ToastLength.Short);
        }

        private void SwitchNameInputType()
        {
            if (usingVkFriendAsName)
            {
                nameEditText.Enabled = true;
                usingVkFriendAsName = false;
            }
            else
            {
                var intent = new Intent(this, typeof(VkFriendPickerActivity));
                var contactPicker = ContactPickers.All[0];
                intent.PutExtra("picker", contactPicker.SerializeToBytes());
                StartActivityForResult(intent, contactPicker.RequestCode);
            }

            UpdateButtons();
        }

        private void UpdateButtons()
        {
            inputTypeSwitchButton.Visibility = VKSdk.IsLoggedIn ? ViewStates.Visible : ViewStates.Gone;
            inputTypeSwitchButton.Text =
                GetString(usingVkFriendAsName ? Resource.String.manually : Resource.String.vk_friends);
        }

        private void AddDebtAndFinish()
        {
            var amount = GetAmount();
            if (isBorrowingDebtSwitch.Checked)
                amount *= -1;
            var money = new Money(amount, GetSelectedCurrency());

            var assosiatedContact = new Contact(nameEditText.Text);
            var comment = commentEditText.Text;
            var id = MainActivity.Debts.GetNextId();

            var debt = dateSwitch.Checked
                ? new Debt(id, assosiatedContact, money, comment, DateTime.Now, lastPaymentDateChoosed)
                : new Debt(id, assosiatedContact, money, comment, DateTime.Now);
            MainActivity.Debts.Add(debt, MainActivity.Storage);
            SetResult(Result.Ok);

            if (usingVkFriendAsName && isBorrowingDebtSwitch.Checked && !string.IsNullOrWhiteSpace(messageEditText.Text))
            {
                var builder = new AlertDialog.Builder(this);
                builder.SetMessage(Resource.String.send_debt_ask_msq);
                builder.SetNegativeButton(Android.Resource.String.No, (sender, args) => { Finish(); });
                builder.SetPositiveButton(Android.Resource.String.Yes, (sender, args) =>
                {
                    SendMessageToLastChoosedFriend(money);
                    Finish();
                });
                builder.Show();
            }
            else
                Finish();
        }

        private decimal GetAmount()
        {
            var amountStr = amountEditText.Text;
            if (amountStr.Length == 0 || amountStr[0] == '.')
                amountStr = '0' + amountStr;
            if (amountStr[amountStr.Length - 1] == '.')
                amountStr += '0';
            return decimal.Parse(amountStr);
        }

        private Currency GetSelectedCurrency()
        {
            var selectedCurrencyName = (string) currencySpinner.SelectedItem;
            return Currencies.All.FirstOrDefault(c => c.Name == selectedCurrencyName);
        }

        private void UpdateDate()
        {
            if (dateSwitch.Checked)
                datePickerDialog.Show();
            else
                HideDateButton();
        }

        private void HideDateButton()
        {
            dateButton.Enabled = false;
            dateButton.Text = "";
            dateButton.Visibility = ViewStates.Gone;
        }

        private void ShowDateButton(string text = "")
        {
            dateButton.Enabled = true;
            dateButton.Text = text;
            dateButton.Visibility = ViewStates.Visible;
        }

        public void OnDateSet(DatePicker view, int year, int month, int dayOfMonth)
        {
            lastPaymentDateChoosed = new DateTime(year, month, dayOfMonth);
            ShowDateButton(lastPaymentDateChoosed.ToShortDateString());
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (requestCode == ContactPickers.All[0].RequestCode)
            {
                if (resultCode == Result.Canceled)
                    return;
                var contact = data
                    .GetByteArrayExtra(FriendPickerActivity.IntentExtraContactKey)
                    .FromBinary<VkContact>();
                lastVkFriendName = contact.Name;
                lastVkFriendId = contact.Id.ToString();
                usingVkFriendAsName = true;
                nameEditText.Text = lastVkFriendName;
                nameEditText.Enabled = false;
                UpdateButtons();
            }
            else
                base.OnActivityResult(requestCode, resultCode, data);
        }
    }
}