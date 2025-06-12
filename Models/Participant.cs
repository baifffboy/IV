using System.ComponentModel.DataAnnotations;

namespace EcoCollectionService.Models
{
    public class Participant
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "��� �����������")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email ����������")]
        [EmailAddress(ErrorMessage = "������������ ������ email")]
        public string Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "������������ ������ ��������")]
        public string? Phone { get; set; }

        [Required]
        public int EventId { get; set; }

        public Event? Event { get; set; }
    }
}