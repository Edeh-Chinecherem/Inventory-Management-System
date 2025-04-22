public class AuditLog
{
    public int Id { get; set; }
    public string Action { get; set; } = string.Empty;
    public string User { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
}