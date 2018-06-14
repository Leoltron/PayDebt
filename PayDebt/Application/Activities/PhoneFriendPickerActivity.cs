using Android.App;
using DebtModel;

namespace PayDebt.Application.Activities
{
    [Activity(Name = "ru.leoltron.PayDebt.PhoneFriendPickerActivity", Label = "", Theme = "@style/DesignTheme1")]
    public class PhoneFriendPickerActivity : FriendPickerActivity<BaseContactPicker<PhoneContact>, PhoneContact>
    {
    }
}