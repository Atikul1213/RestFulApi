namespace EmployeeAdminPortal.Models
{
    public class FailureLog
    {
        public int Id { get; set; }
        public int? OrderId { get; set; }
        public int? CustomerId { get; set; }
        public int? ProductId { get; set; }
        public int? PaymentId { get; set; }
        public string MethodName { get; set; } = string.Empty;
        public string ClassName { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
        public string StackTrace { get; set; } = string.Empty;
        public string? RequestPayload { get; set; }
        public string Servity { get; set; } = "Error";
        public string? CorrelationId { get; set; }
        public string Environment { get; set; } = "Production";
        public DateTime LoggedAt { get; set; } = DateTime.UtcNow;
    }
}
