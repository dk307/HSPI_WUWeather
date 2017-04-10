using System.ComponentModel;

namespace Hspi
{
    /// <summary>
    /// Units of measurement supported by the Forecast service.
    /// </summary>
    public enum Unit
    {
        /// <summary>
        /// US units of measurement.
        /// </summary>
        [Description("US Measurements")]
        US,

        /// <summary>
        /// SI units of measurement.
        /// </summary>
        [Description("SI Measurements")]
        SI,
    }
}