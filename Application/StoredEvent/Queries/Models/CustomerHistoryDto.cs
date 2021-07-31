namespace Application.StoredEvent.Queries.Models
{
    public class CustomerHistoryDto
    {
        public string Action { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string BirthDate { get; set; }
        public string Timestamp { get; set; } 
    }
}
