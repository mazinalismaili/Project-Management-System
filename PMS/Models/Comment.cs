namespace PMS.Models
{
    public class Comment
    {
        public string Id { get; set; }
        public string CommentContent { get; set; }
        public DateTime CreatedAt { get; set; }

        public string? TaskId { get; set; }
        public Task? Task { get; set; }

        public string? EmployeeId { get; set; }
        public Employee? Employee { get; set; }

        

    }
}
