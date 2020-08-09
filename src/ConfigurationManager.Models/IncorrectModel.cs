namespace ConfigurationManager.Models
    {
    public class IncorrectModel
        {
        public int ordersPerHour { get; set; } //Number of Orders per Hour
        public int inboundStrategy { get; set; } //Number of Orders per Hour
        public int nonExistingProperty { get; set; }
        }
    }