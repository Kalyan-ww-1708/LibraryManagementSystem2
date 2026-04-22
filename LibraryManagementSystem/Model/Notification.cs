namespace LibraryManagementSystem.Model
{
    public class Notification
    {
        public Guid NotificationId { get; set; }
        public Guid UserId { get; set; }

        public required string Title { get; set; }    
        public string? Message { get; set; }   

        public bool IsRead { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Guid? BorrowId { get; set; }
    }
}
