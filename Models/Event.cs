using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcoCollectionService.Models
{
	public class Event
	{
		public int Id { get; set; }

		[Required]
		public string Title { get; set; } = string.Empty;

		[Required]
		public string Description { get; set; } = string.Empty;


		[Required]
		[Display(Name = "Event Date")]
		public DateTime EventDateTime { get; set; } = DateTime.UtcNow.AddDays(1); // Устанавливаем UTC по умолчанию

		[Required]
		public string Location { get; set; } = string.Empty;

		public int? MaxParticipants { get; set; }

		public ICollection<Participant> Participants { get; set; } = new List<Participant>();
		public ICollection<EventSensor> Sensors { get; set; } = new List<EventSensor>();

        [NotMapped]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string Status => DateTime.UtcNow.AddHours(3) >= EventDateTime ? "Началось" : "Ожидается";
    }
}