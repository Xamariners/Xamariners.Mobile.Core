using System.ComponentModel;

namespace Xamariners.Mobile.Core.Common.Enums
{
    /// <summary>
    /// The App Live Status. [Default] AppLiveStatus.Unknown
    /// </summary>
    [DefaultValue(Unknown)]
    public enum AppLiveStatus : int
    {
        /// <summary>
        /// AppLiveStatus.Unknown: Unknown app status
        /// </summary>
        [Description("Unknown App Status")] Unknown,

        /// <summary>
        /// AppLiveStatus.Running: The app is currently running.
        /// </summary>
        [Description("The App is Currently Running")] Running,

        /// <summary>
        /// AppLiveStatus.JustStarted: The app is just started.
        /// </summary>
        [Description("The App was Just Started")] JustStarted,

        /// <summary>
        /// AppLiveStatus.JustWokeUp: The app is just woke up.
        /// </summary>
        [Description("The App Just Woke Up")] JustWokeUp,

        /// <summary>
        /// AppLiveStatus.GoingToSleep: The app is going to sleep.
        /// </summary>
        [Description("The App is Going to Sleep")] GoingToSleep,

        /// <summary>
        /// AppLiveStatus.Sleeping: The app is sleeping.
        /// </summary>
        [Description("The App is Sleeping")] Sleeping,
    }
}
