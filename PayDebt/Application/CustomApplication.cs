using System;
using Android.App;
using Android.Runtime;
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
            VKSdk.Initialize(this);
        }
    }
}