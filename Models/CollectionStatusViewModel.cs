// CollectionStatusViewModel.cs
namespace EcoCollectionService.Models
{
	public class CollectionStatusViewModel
	{
		public Event Event { get; set; }
		public int PaperProgress { get; set; }
		public int GlassProgress { get; set; }
		public int PlasticProgress { get; set; }
		public int AirQuality { get; set; }
		public DateTime AirQualityLastUpdated { get; set; }
	}
}