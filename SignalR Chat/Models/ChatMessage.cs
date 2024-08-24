using System.ComponentModel.DataAnnotations;

namespace SignalR_Chat.Models
{
    public class ChatMessage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(256)]
        public string? Sender { get; set; }

        [MaxLength(256)]
        public string? Receiver { get; set; }

        [Required]
        public string Message { get; set; }

        public DateTime SentAt { get; set; }

        [MaxLength(50)]
        public string Sentiment { get; set; }
    }
}
