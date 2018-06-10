using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Android.App;
using Android.Content;
using DebtModel;

namespace PayDebt
{
    public static class SharedPrefExtensions
    {
        public static ISharedPreferences GetAppSharedPref(Activity activity)
        {
            return activity.GetSharedPreferences(activity.GetString(Resource.String.app_name),
                FileCreationMode.Private);
        }

        private const string DefaultCurrencyKey = "defaultCurrencyName";

        public static void PutDefaultCurrency(this ISharedPreferencesEditor editor, Currency currency)
        {
            editor.PutDefaultCurrency(currency.Name);
        }

        public static void PutDefaultCurrency(this ISharedPreferencesEditor editor, string currencyName)
        {
            editor.PutString(DefaultCurrencyKey, currencyName);
        }

        public static void PutDefaultCurrency(this ISharedPreferencesEditor editor)
        {
            editor.Remove(DefaultCurrencyKey);
        }

        public static Currency GetDefaultCurrency(this ISharedPreferences sharedPref)
        {
            var name = sharedPref.GetString(DefaultCurrencyKey, "NONE");
            return Currencies.All.FirstOrDefault(c => c.Name == name);
        }

        private const string MessageTemplateKey = "messageTemplate";

        public static void PutMessageTemplate(this ISharedPreferencesEditor editor, string messageTemplate)
        {
            editor.PutString(MessageTemplateKey, messageTemplate);
        }

        public static string GetMessageTemplate(this ISharedPreferences sharedPref)
        {
            return sharedPref.GetString(MessageTemplateKey, "PayDebt");
        }

        private const string IdSuffix = "_id";
        private const string ContactSuffix = "_contact";
        private const string MoneySuffix = "_money";
        private const string CreationDateSuffix = "_creationDate";
        private const string HasPaymentDateSuffix = "_hasPaymentDate";
        private const string PaymentDateSuffix = "_зaymentDate";
        private const string CommentSuffix = "_сomment";
        private const string IsPaidSuffix = "_isPaid";

        public static void PutDebt(this ISharedPreferencesEditor editor, string key, Debt debt)
        {
            editor.PutInt(key + IdSuffix, debt.Id);
            editor.PutContact(key + ContactSuffix, debt.AssosiatedContact);
            editor.PutMoney(key + MoneySuffix, debt.Money);
            editor.PutDate(key + CreationDateSuffix, debt.CreationDate);
            editor.PutBoolean(key + HasPaymentDateSuffix, debt.HasPaymentDate);
            if (debt.HasPaymentDate)
                editor.PutDate(key + PaymentDateSuffix, debt.PaymentDate);
            editor.PutString(key + CommentSuffix, debt.Comment);
            editor.PutBoolean(key + IsPaidSuffix, debt.IsPaid);
        }

        public static Debt GetDebt(this ISharedPreferences sharedPref, string key)
        {
            var id = sharedPref.GetIntOrThrow(key + IdSuffix);
            var contact = sharedPref.GetContact(key + ContactSuffix);
            var money = sharedPref.GetMoney(key + MoneySuffix);
            var creationDate = sharedPref.GetDate(key + CreationDateSuffix);

            sharedPref.CheckContains(key + CommentSuffix);
            var comment = sharedPref.GetString(key + CommentSuffix, "");

            sharedPref.CheckContains(key + IsPaidSuffix);
            var isPaid = sharedPref.GetBoolean(key + IsPaidSuffix, false);

            sharedPref.CheckContains(key + HasPaymentDateSuffix);
            var hasPaymentDate = sharedPref.GetBoolean(key + HasPaymentDateSuffix, false);

            var debt = hasPaymentDate
                ? new Debt(id, contact, money, comment, creationDate, sharedPref.GetDate(key + PaymentDateSuffix))
                : new Debt(id, contact, money, comment, creationDate);

            debt.IsPaid = isPaid;
            return debt;
        }

        private const string ContactNameSuffix = "_name";

        public static void PutContact(this ISharedPreferencesEditor editor, string key, Contact contact)
        {
            editor.PutString(key + ContactNameSuffix, contact.Name);
        }

        public static Contact GetContact(this ISharedPreferences sharedPref, string key)
        {
            sharedPref.CheckContains(key + ContactNameSuffix);
            return new Contact(sharedPref.GetString(key + ContactNameSuffix, ""));
        }

        private const string DaySuffix = "_day";
        private const string MonthSuffix = "_month";
        private const string YearSuffix = "_year";

        public static void PutDate(this ISharedPreferencesEditor editor, string key, DateTime dateTime)
        {
            var utcDateTime = dateTime.ToUniversalTime();
            editor.PutInt(key + DaySuffix, utcDateTime.Day);
            editor.PutInt(key + MonthSuffix, utcDateTime.Month);
            editor.PutInt(key + YearSuffix, utcDateTime.Year);
        }

        public static DateTime GetDate(this ISharedPreferences sharedPref, string key)
        {
            var day = sharedPref.GetIntOrThrow(key + DaySuffix);
            var month = sharedPref.GetIntOrThrow(key + MonthSuffix);
            var year = sharedPref.GetIntOrThrow(key + YearSuffix);
            return new DateTime(year, month, day).ToLocalTime();
        }

        private const string AmountSuffix = "_amount";
        private const string CurrencySuffix = "_currency";

        public static void PutMoney(this ISharedPreferencesEditor editor, string key, Money money)
        {
            editor.PutFloat(key + AmountSuffix, Convert.ToSingle(money.Amount));
            editor.PutCurrency(key + CurrencySuffix, money.Currency);
        }

        public static Money GetMoney(this ISharedPreferences sharedPref, string key)
        {
            sharedPref.CheckContains(key + AmountSuffix);
            var amount = Convert.ToDecimal(sharedPref.GetFloat(key + AmountSuffix, 0));
            var currency = sharedPref.GetCurrency(key + CurrencySuffix);
            return new Money(amount, currency);
        }

        private const string CultureInfoSuffix = "_cultureInfo";
        private const string NameSuffix = "_name";

        public static void PutCurrency(this ISharedPreferencesEditor editor, string key, Currency currency)
        {
            editor.PutCultureInfo(key + CultureInfoSuffix, currency.Culture);
            editor.PutString(key + NameSuffix, currency.Name);
        }

        public static Currency GetCurrency(this ISharedPreferences sharedPref, string key)
        {
            var cultureInfo = sharedPref.GetCultureInfo(key + CultureInfoSuffix);
            sharedPref.CheckContains(key + NameSuffix);
            var name = sharedPref.GetString(key + NameSuffix, "");

            return new Currency(name, cultureInfo);
        }

        public static void PutCultureInfo(this ISharedPreferencesEditor editor, string key, CultureInfo cultureInfo)
        {
            editor.PutString(key, cultureInfo.IetfLanguageTag);
        }

        public static CultureInfo GetCultureInfo(this ISharedPreferences sharedPref, string key)
        {
            sharedPref.CheckContains(key);
            return new CultureInfo(sharedPref.GetString(key, ""));
        }

        public static int GetIntOrThrow(this ISharedPreferences sharedPref, string key)
        {
            sharedPref.CheckContains(key);
            return sharedPref.GetInt(key, -1);
        }

        private static void CheckContains(this ISharedPreferences sharedPref, string key)
        {
            if (!sharedPref.Contains(key))
                throw new KeyNotFoundException($"Has not found key {key} in ISharedPreferences instance");
        }
    }
}