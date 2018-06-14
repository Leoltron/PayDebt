using Android.App;
using DebtModel;

namespace PayDebt.Application.Activities
{

    [Activity(Name = "ru.leoltron.PayDebt.VkFriendPickerActivity", Label = "", Theme = "@style/DesignTheme1")]
    public class VkFriendPickerActivity : FriendPickerActivity<BaseContactPicker<VkContact>, VkContact>
    {
    }
}