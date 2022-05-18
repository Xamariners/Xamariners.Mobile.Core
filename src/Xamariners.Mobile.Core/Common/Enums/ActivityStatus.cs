using System.ComponentModel;

namespace Xamariners.Mobile.Core.Common.Enums
{
    /// <summary>
    /// The activity status.
    /// </summary>
    [DefaultValue(Idle)]
    public enum ActivityStatus : int
    {
        /// <summary>
        /// ActivityStatus.Idle.
        /// </summary>
        Idle = 0,

        /// <summary>
        /// ActivityStatus.Initiated.
        /// </summary>
        Initiated = 1,

        /// <summary>
        /// ActivityStatus.Processing.
        /// </summary>
        Processing = 2,

        /// <summary>
        /// ActivityStatus.Completed.
        /// </summary>
        Completed = 3,

        /// <summary>
        /// ActivityStatus.Failed.
        /// </summary>
        Failed = 4,

        /// <summary>
        /// ActivityStatus.Pending.
        /// </summary>
        Pending = 5,

        /// <summary>
        /// ActivityStatus.Cancelled.
        /// </summary>
        Cancelled = 6
    }
}
