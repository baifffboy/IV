namespace EcoCollectionService.Models
{
    public class EventSensor
    {
        public int Id { get; set; }
        public string SensorType { get; set; }  // "trash_level", "attendance", etc.
        public string SensorId { get; set; }
        public double CurrentValue { get; set; }
        public DateTime LastUpdated { get; set; }

        public int EventId { get; set; }
        public Event Event { get; set; }
    }
}