using System.ComponentModel.DataAnnotations;

namespace SignalR_Chat.Models
{
    public class ChatRoom
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
