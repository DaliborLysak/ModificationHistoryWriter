using Microsoft.Toolkit.Uwp.Notifications;

namespace ModificationHistoryWriterForm
{
    public static class ToastContentBuilderHelper
    {
        public static void ShowMessage(string title, string message)
        {
            new ToastContentBuilder()
                .AddArgument("action", "viewConversation")
                .AddArgument("conversationId", 6611)
                .AddText(title)
                .AddText(message)
                .Show(toast =>
                {
                    toast.ExpirationTime = DateTime.Now.AddMinutes(20);
                });
        }
        public static void ClearAndClose()
        {
            ToastNotificationManagerCompat.History.Clear();
            ToastNotificationManagerCompat.Uninstall();
        }
    }
}
