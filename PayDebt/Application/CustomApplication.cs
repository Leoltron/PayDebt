using System;
using System.Globalization;
using Android.App;
using Android.Runtime;
using DebtModel;
using PayDebt.Application.Activities;
using PayDebt.Model;
using VKontakte;

namespace PayDebt.Application
{
#if DEBUG
    [Application(Debuggable = true, Name = "ru.leoltron.PayDebt.CustomApplication", AllowBackup =true, Label = "@string/app_name", Theme = "@style/DesignTheme", Icon = "@mipmap/ic_launcher")]
#else
    [Application(Debuggable = false, Name = "ru.leoltron.PayDebt.CustomApplication", AllowBackup =true, Label = "@string/app_name", Theme = "@style/DesignTheme", Icon = "@mipmap/ic_launcher")]
#endif
    // ReSharper disable once UnusedMember.Global
    public class CustomApplication : Android.App.Application
    {
        public CustomApplication(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public CustomApplication()
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();
            Currencies.Add(new Currency("USD", CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")));
            Currencies.Add(new Currency("EUR", CultureInfo.GetCultureInfoByIetfLanguageTag("fr-FR")));
            Currencies.Add(new Currency("RUB", CultureInfo.GetCultureInfoByIetfLanguageTag("ru-RU")));
            ContactPickers.Add(new VkContactPicker(new VkContactProvider(),
                                                   GetString(Resource.String.vk),
                                                   0,
                                                   typeof(VkFriendPickerActivity)));
            VKSdk.Initialize(this);
        }
    }
}