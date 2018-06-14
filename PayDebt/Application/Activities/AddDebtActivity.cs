using System;
using System.Linq;
using System.Text;
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
    [Activity(Label = "AddDebtActivity", Name = "ru.leoltron.PayDebt.AddDebtActivity",
        ConfigurationChanges =
            Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
    public class AddDebtActivity : Activity, DatePickerDialog.IOnDateSetListener
    {
           
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

        private Contact lastContact = null;
        private IContactPicker<Contact> lastPicker = null;

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
            messageLinearLayout.Visibility = ContactPickers.HasAnyConnected ? ViewStates.Visible : ViewStates.Gone;
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
            lastPicker.SendMessage(lastContact, GetFormattedMessage(money), this);
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


        private void SwitchNameInputType()
        {
            if (lastContact != null)
            {
                nameEditText.Enabled = true;
                lastPicker = null;
                lastContact = null;
                UpdateButtons();
                return;
            }
            ShowChoiceDialog();

            UpdateButtons();
        }

        private void ShowChoiceDialog()
        {
            var items = ContactPickers.All
                .Where(x => x.IsLoggedIn)
                .Select(x => x.Name)
                .ToArray();
            new AlertDialog.Builder(this)
                .SetTitle(Resource.String.select)
                .SetItems(items, (sender, args) =>
                {
                    var picker = ContactPickers.All[args.Which];
                    var intent = new Intent(this, picker.PickerActivityType);
                    intent.PutExtra("picker", picker.SerializeToBytes());
                    StartActivityForResult(intent, picker.RequestCode);
                })
                .SetNegativeButton(Android.Resource.String.Cancel, (sender, args) => { })
                .Show();
        }

        private void UpdateButtons()
        {
            inputTypeSwitchButton.Visibility = ContactPickers.HasAnyConnected ? ViewStates.Visible : ViewStates.Gone;
            inputTypeSwitchButton.Text = 
                GetString(lastContact != null ? Resource.String.manually : Resource.String.choose_source);
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

            if (lastPicker != null 
                    && lastPicker.CanSendMessage 
                    && isBorrowingDebtSwitch.Checked 
                    && !string.IsNullOrWhiteSpace(messageEditText.Text))
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
            var picker = ContactPickers.All.SingleOrDefault(p => p.RequestCode == requestCode);
            if (picker == null)
            {
                base.OnActivityResult(requestCode, resultCode, data);
                lastPicker = null;
                lastContact = null;
                return;
            }
            if (resultCode == Result.Canceled)
                return;
            lastContact = data
                .GetByteArrayExtra(FriendPickerActivity.IntentExtraContactKey)
                .FromBinary<Contact>();
            lastPicker = picker;
            nameEditText.Text = lastContact.Name;
            nameEditText.Enabled = false;
            UpdateButtons();
        }
    }
}