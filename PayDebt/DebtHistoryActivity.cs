using Android.App;
using Android.OS;
using Android.Widget;
using DebtModel;

namespace PayDebt
{
    [Activity(Name = "ru.leoltron.PayDebt.DebtHistoryActivity")]
    public class DebtHistoryActivity : Activity
    {
        private DebtInfoAdapter adapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.DebtHistoryActivityLayout);
            SetResult(Result.Canceled);

            FindViewById<Button>(Resource.Id.backButton).Click += (sender, args) => Finish();

            var listView = FindViewById<ListView>(Resource.Id.debtHistoryListView);
            adapter = new DebtInfoAdapter(this, MainActivity.Debts);

            listView.ItemLongClick += (sender, args) => OpenDebtDialog(adapter[args.Position]);
            listView.Adapter = adapter;
        }

        public void OpenDebtDialog(Debt debt)
        {
            var options = new[]
            {
                GetString(debt.IsPaid ? Resource.String.set_unpaid : Resource.String.set_paid)
            };

            var builder = new AlertDialog.Builder(this);
            builder.SetItems(options, (sender, args) =>
            {
                switch (args.Which)
                {
                    case 0:
                        debt.IsPaid = !debt.IsPaid;
                        MainActivity.Debts.Add(debt, MainActivity.Storage);
                        adapter.UpdateList();
                        adapter.NotifyDataSetInvalidated();
                        SetResult(Result.Ok);
                        break;
                }
            });
            builder.Show();
        }
    }
}