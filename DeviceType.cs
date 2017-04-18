using System;

namespace Hspi
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = true)]
    internal sealed class UnitTypeDescription : Attribute
    {
        public UnitTypeDescription(Unit unit, string description)
        {
            Unit = unit;
            Description = description;
        }

        public Unit Unit { get; private set; }
        public string Description { get; private set; }
    }

    internal enum DeviceUnitType
    {
        [UnitTypeDescription(Unit.US, "inches")]
        [UnitTypeDescription(Unit.SI, "mm")]
        Precipitation,

        [UnitTypeDescription(Unit.US, "Hg")]
        [UnitTypeDescription(Unit.SI, "millibars")]
        Pressure,

        [UnitTypeDescription(Unit.US, "°F")]
        [UnitTypeDescription(Unit.SI, "°C")]
        Temperature,

        [UnitTypeDescription(Unit.US, "miles")]
        [UnitTypeDescription(Unit.SI, "km")]
        Visibility,

        [UnitTypeDescription(Unit.US, "miles/hr")]
        [UnitTypeDescription(Unit.SI, "km/hr")]
        WinSpeed,

        WeatherType,
    };
}