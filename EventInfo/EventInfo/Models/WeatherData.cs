namespace EventInfo.Models
{
    public class WeatherData
    {
        public string MeasurementId { get; set; }
        public string StationName { get; set; }
        public DateTime MeasurementTimestamp { get; set; }
        public float AirTemperature { get; set; }
        public float WetBulbTemperature { get; set; }
        public int Humidity { get; set; }
        public float RainIntensity { get; set; }
        public float IntervalRain { get; set; }
        public float TotalRain { get; set; }
        public int PrecipitationType { get; set; }
        public float WindDirection { get; set; }
        public float WindSpeed { get; set; }
        public float MaximumWindSpeed { get; set; }
        public float BiometricPressure { get; set; }
        public int SolarRadiation { get; set; }
        public int Heading { get; set; }
        public string BatteryLife { get; set; }
        public string MeasurementTimestampLabel { get; set; }
        public string Timestamp { get; set; }
        public string EventType { get; set; }
    }
}
