using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EcoCollectionService.Data;
using EcoCollectionService.Models;

namespace EcoCollectionService.Controllers
{
	public class EventsController : Controller
	{
		private readonly ApplicationDbContext _context;

		public EventsController(ApplicationDbContext context)
		{
			_context = context;
		}

		// GET: Events
		public async Task<IActionResult> Index()
		{
			var events = await _context.Events
				.Include(e => e.Participants)
				.Include(e => e.Sensors)
				.ToListAsync();
			return View(events);
		}

		// GET: Events/Create
		[HttpGet]
		public IActionResult Create()
		{
			// Создаем мероприятие с датой по умолчанию
			var newEvent = new Event
			{
				EventDateTime = DateTime.Now.AddDays(1) // Завтрашняя дата по умолчанию
			};
			return View(newEvent);
		}

		// POST: Events/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(
	[Bind("Title,Description,EventDateTime,Location,MaxParticipants")] Event @event, [FromServices] EmailSender emailSender)
		{
			// Убедимся, что дата в UTC
			@event.EventDateTime = DateTime.SpecifyKind(@event.EventDateTime, DateTimeKind.Utc);

			if (ModelState.IsValid)
			{
				_context.Add(@event);
				await _context.SaveChangesAsync();

                await emailSender.SendNotificationAsync(
            "🎯 Новое мероприятие создано",
            $"""
            <h1 style="color: #2ecc71;">Новое мероприятие</h1>
            <p><strong>Название:</strong> {@event.Title}</p>
            <p><strong>Дата:</strong> {@event.EventDateTime:dd.MM.yyyy HH:mm}</p>
            <p><strong>Место:</strong> {@event.Location}</p>
            <p><a href="http://localhost:5152/Events">Перейти к мероприятию</a></p>
            """);

                return RedirectToAction(nameof(Index));
			}
			return View(@event);
		}

		// GET: Events/Register/5
		// GET: Events/Register/5
		public async Task<IActionResult> Register(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var @event = await _context.Events.FindAsync(id);
			if (@event == null)
			{
				return NotFound();
			}

			ViewData["EventTitle"] = @event.Title;
			return View(new Participant { EventId = id.Value });
		}

		// POST: Events/Register/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register(int id, [Bind("Name,Email,Phone,EventId")] Participant @participant, [FromServices] EmailSender emailSender)
		{
			if (id != @participant.EventId)
			{
				return NotFound();
			}



            if (ModelState.IsValid)
			{
				_context.Add(@participant);
				await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
			}

			var @event = await _context.Events.FindAsync(id);
			ViewData["EventTitle"] = @event?.Title;
			return View(@participant);
		}

        // GET: Events/CollectionStatus/5
        public async Task<IActionResult> CollectionStatus(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .Include(e => e.Sensors)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (@event == null)
            {
                return NotFound();
            }

            // Рассчитываем прогресс заполнения контейнеров
            var paperProgress = CalculateContainerProgress(@event, "paper");
            var glassProgress = CalculateContainerProgress(@event, "glass");
            var plasticProgress = CalculateContainerProgress(@event, "plastic");

            // Генерация случайного значения качества воздуха (20-40%)
            var random = new Random();
            var airQuality = random.Next(20, 41);
            var airQualityLastUpdated = DateTime.UtcNow.AddHours(3);

            var model = new CollectionStatusViewModel
            {
                Event = @event,
                PaperProgress = paperProgress,
                GlassProgress = glassProgress,
                PlasticProgress = plasticProgress,
                AirQuality = airQuality,
                AirQualityLastUpdated = airQualityLastUpdated
            };

            return View(model);
        }

        private int CalculateContainerProgress(Event @event, string containerType)
        {
            if (@event.Status == "Ожидается") return 0;

            var sensor = @event.Sensors.FirstOrDefault(s => s.SensorType == $"trash_level_{containerType}");
            if (sensor != null)
            {
                // Если есть реальные данные с датчика - используем их
                return (int)(sensor.CurrentValue * 100);
            }

            // Имитируем заполнение пропорционально времени мероприятия
            var timeElapsed = DateTime.UtcNow.AddHours(3) - @event.EventDateTime;
            var progressPercentage = (int)(timeElapsed.TotalMinutes / 60 * 100); // 60 минут = 1 час
            return Math.Min(progressPercentage, 100); // Не больше 100%
        }
    }
}