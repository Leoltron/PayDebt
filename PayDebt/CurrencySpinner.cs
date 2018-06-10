using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Util;
using Android.Widget;

namespace PayDebt
{
    // ReSharper disable once ClassNeverInstantiated.Global
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class CurrencySpinner : Spinner
    {
        public CurrencySpinner(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
        }

        public CurrencySpinner(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
        }

        public void Initialize(Activity activity)
        {
            var currenciesNames = Currencies.All.Select(c => c.Name).ToList();
            var defaultCurrency = SharedPrefExtensions.GetAppSharedPref(activity).GetDefaultCurrency();
            var selected = currenciesNames.FindIndex(s => s == defaultCurrency?.Name);

            var adapter =
                new ArrayAdapter<string>(activity, Android.Resource.Layout.SimpleSpinnerItem, currenciesNames);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            Adapter = adapter;

            SetSelection(selected == -1 ? 0 : selected);
        }
    }
}