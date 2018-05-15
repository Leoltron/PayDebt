using System;
using Android.App;
using Android.Runtime;
using Com.VK.Sdk;

namespace PayDebt
{
#if DEBUG
    [Application(Debuggable = true)]
#else
    [Application(Debuggable = false)]
#endif
    // ReSharper disable once UnusedMember.Global
    public class CustomApplication : Application
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