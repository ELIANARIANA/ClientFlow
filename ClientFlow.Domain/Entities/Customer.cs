namespace ClientFlow.Domain.Entities
{
    public class Customer
    {
        public Guid Id                   { get; set; }
        public string FirtName           { get; set; } = string.Empty;
        public string LastName           { get; set; } = string.Empty;
        public string Email              { get; set; } = string.Empty;
        public string? Phone             { get; set; }
        public string? CompanyName       { get; set; }
        public DateTimeOffset CreatedAt  { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
    }
}
