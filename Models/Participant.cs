using System.ComponentModel.DataAnnotations;

namespace EcoCollectionService.Models
{
    public class Participant
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Имя обязательно")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email обязателен")]
        [EmailAddress(ErrorMessage = "Некорректный формат email")]
        public string Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Некорректный формат телефона")]
        public string? Phone { get; set; }

        [Required]
        public int EventId { get; set; }

        public Event? Event { get; set; }
    }
}