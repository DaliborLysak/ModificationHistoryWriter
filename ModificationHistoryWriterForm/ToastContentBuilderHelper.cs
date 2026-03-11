using Microsoft.Toolkit.Uwp.Notifications;

namespace ModificationHistoryWriterForm
{
    /// <summary>
    /// Provides helper methods for displaying and cleaning up Windows toast notifications
    /// via the <c>Microsoft.Toolkit.Uwp.Notifications</c> library.
    /// </summary>
    public static class ToastContentBuilderHelper
    {
        /// <summary>
        /// Displays a Windows toast notification with the given <paramref name="title"/>
        /// and <paramref name="message"/>.
        /// <para>
        /// The notification expires automatically 20 minutes after it is shown.
        /// </para>
        /// </summary>
        /// <param name="title">The bold heading displayed at the top of the notification.</param>
        /// <param name="message">The body text displayed below the title.</param>
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

        /// <summary>
        /// Clears all toast notifications previously shown by this application from the
        /// Windows Action Center and unregisters the application's notification COM activator.
        /// <para>
        /// Call this method when the application is closing to ensure no stale notifications
        /// remain visible and all COM resources are released.
        /// </para>
        /// </summary>
        public static void ClearAndClose()
        {
            ToastNotificationManagerCompat.History.Clear();
            ToastNotificationManagerCompat.Uninstall();
        }
    }
}
