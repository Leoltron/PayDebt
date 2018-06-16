using Android.App;
using DebtModel;
using PayDebt.Model;

namespace PayDebt.Application.Activities
{
    [Activity(Name = "ru.leoltron.PayDebt.VkFriendPickerActivity", Label = "", Theme = "@style/DesignTheme1")]
    public class VkFriendPickerActivity : FriendPickerActivity<VkContactPicker, VkContact>
    {
    }
}