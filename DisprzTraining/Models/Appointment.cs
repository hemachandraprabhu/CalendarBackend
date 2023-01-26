namespace DisprzTraining.Models
{
    public class Appointment
    {
        public Guid id { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public string appointment { get; set; } = string.Empty;
    }
}
