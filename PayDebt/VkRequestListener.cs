using System;
using Com.VK.Sdk.Api;

namespace PayDebt
{
    internal class VkRequestListener : VKRequest.VKRequestListener
    {
        private Action<VKRequest, int, int> OnAttemptFailed;
        private Action<VKResponse> OnRequestComplete;

        public VkRequestListener(Action<VKRequest, int, int> onAttemptFailed, Action<VKResponse> onRequestComplete)
        {
            OnAttemptFailed = onAttemptFailed;
            OnRequestComplete = onRequestComplete;
        }

        public override void AttemptFailed(VKRequest request, int attemptNumber, int totalAttempts)
        {
            OnAttemptFailed?.Invoke(request, attemptNumber, totalAttempts);
        }

        public override void OnComplete(VKResponse response)
        {
            OnRequestComplete?.Invoke(response);
        }
    }
}