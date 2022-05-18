using System.ComponentModel;

namespace Xamariners.Mobile.Core.Common.Enums
{
    /// <summary>
    /// The log severity type.
    /// </summary>
    [DefaultValue(Debug)]
    public enum LogSeverity : int
    {
        /// <summary>
        /// Info
        /// </summary>
        Debug,

        /// <summary>
        /// Information
        /// </summary>
        Information,

        /// <summary>
        /// System
        /// </summary>
        System,

        /// <summary>
        /// Warning
        /// </summary>
        Warning,

        /// <summary>
        /// Error
        /// </summary>
        Error
    }
}
