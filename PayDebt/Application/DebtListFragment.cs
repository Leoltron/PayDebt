using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using DebtModel;
using PayDebt.Application.Activities;
using Fragment = Android.Support.V4.App.Fragment;

namespace PayDebt.Application
{
    class DebtListFragment : Fragment
    {
        private IEnumerable<Debt> DebtEnumerable { get; set; }
        private Func<Debt, bool> Filter { get; set; }
        private ListView debtList;
        private DebtInfoAdapter adapter;

        public DebtListFragment()
        {
        }

        public DebtListFragment(IEnumerable<Debt> debtEnumerable, Func<Debt, bool> filter)
        {
            this.DebtEnumerable = debtEnumerable;
            this.Filter = filter;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.DebtListLayout, container, false);
            debtList = view.FindViewById<ListView>(Resource.Id.debtListView);
            debtList.ItemLongClick += (sender, args) => OpenDebtDialog(adapter[args.Position]);
            adapter = new DebtInfoAdapter(Activity, DebtEnumerable, Filter);
            debtList.Adapter = adapter;
            return view;
        }

        public void OpenDebtDialog(Debt debt)
        {
            var options = new[]
            {
                GetString(debt.IsPaid ? Resource.String.set_unpaid : Resource.String.set_paid)
            };

            var builder = new AlertDialog.Builder(Context);
            builder.SetItems(options, (sender, args) =>
            {
                switch (args.Which)
                {
                    case 0:
                        debt.IsPaid = !debt.IsPaid;
                        MainActivity.Debts.Add(debt, MainActivity.Storage);
                        UpdateAdapter();
                        break;
                }
            });
            builder.Show();
        }

        public void UpdateAdapter()
        {
            adapter.UpdateList();
        }
    }
}