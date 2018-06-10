using System;
using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using VKontakte;
using VKontakte.API;

namespace PayDebt
{
    [Activity(Label = "AddDebtActivity", Name = "ru.leoltron.PayDebt.AddDebtActivity")]
    public class AddDebtActivity : Activity, DatePickerDialog.IOnDateSetListener
    {
        private Random rand = new Random();

        private const int FindVkFriendRequestCode = 339000236;
        private Button finishButton;

        private EditText nameEditText;
        private EditText amountEditText;
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
            InitPaymentInputArea();
            InitPaymentDateChoosingArea();
            commentEditText = FindViewById<EditText>(Resource.Id.commentEditText);
        }

        private void InitPaymentInputArea()
        {
            InitAmountEditText();
            InitCurrencySpinner();
            InitMessageEditText();
        }

        private void InitMessageEditText()
        {
            messageEditText = FindViewById<EditText>(Resource.Id.messageEditText);
            messageEditText.Enabled = VKSdk.IsLoggedIn;
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
            vkParams.Put("message", messageEditText.Text);
            new VKRequest("messages.send", vkParams).ExecuteWithListener(new VkRequestListener(OnAttemptFailed, OnRequestComplete));
        }

        private void OnRequestComplete(VKResponse obj)
        {
            //Toast.MakeText(this, Resource.String.msg_sent_sucessfully, ToastLength.Short);
        }

        private void OnAttemptFailed(VKRequest arg1, int arg2, int arg3)
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
                StartActivityForResult(new Intent(this, typeof(VkFriendPickerActivity)), FindVkFriendRequestCode);
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

            if (usingVkFriendAsName && isBorrowingDebtSwitch.Checked)
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
            if (requestCode == FindVkFriendRequestCode)
            {
                if (resultCode == Result.Canceled)
                    return;
                lastVkFriendName = data.GetStringExtra(VkFriendPickerActivity.IntentExtraNameKey);
                lastVkFriendId = data.GetStringExtra(VkFriendPickerActivity.IntentExtraIdKey);
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