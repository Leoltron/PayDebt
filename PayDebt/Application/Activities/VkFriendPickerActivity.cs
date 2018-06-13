using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;
using DebtModel;
using Infrastructure;
using Org.Json;
using PayDebt.AndroidInfrastructure;
using PayDebt.Model;
using VKontakte.API;
using System.Runtime.Serialization.Formatters.Binary;

namespace PayDebt.Application.Activities
{

    [Activity(Name = "ru.leoltron.PayDebt.VkFriendPickerActivity", Label = "", Theme = "@style/DesignTheme1")]
    public class VkFriendPickerActivity : FriendPickerActivity<ContactPicker<VKContact>, VKContact>
    {
    }
}