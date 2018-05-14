using System;
using System.Linq;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace PayDebt
{
    [Activity(Label = "AddDebtActivity")]
    public class AddDebtActivity : Activity, DatePickerDialog.IOnDateSetListener
    {
        private Button finishButton;

        private EditText nameEditText;
        private EditText amountEditText;

        private Button dateButton;
        private Switch dateSwitch;
        private DatePickerDialog datePickerDialog;
        private DateTime lastPaymentDateChoosed;

        private CurrencySpinner currencySpinner;

        private Switch isBorrowingDebtSwitch;

        private EditText commentEditText;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            SetContentView(Resource.Layout.AddDebtLayout);
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
            return Currency.Currencies.FirstOrDefault(c => c.Name == selectedCurrencyName);
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
    }
}