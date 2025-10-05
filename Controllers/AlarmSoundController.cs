using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace HackYeah_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlarmSoundController : ControllerBase
    {
        private readonly string _alarmsPath;
        private readonly FileExtensionContentTypeProvider _contentTypeProvider;

        public AlarmSoundController(IWebHostEnvironment environment)
        {
            _alarmsPath = Path.Combine(environment.ContentRootPath, "Resources", "Alarms");
            _contentTypeProvider = new FileExtensionContentTypeProvider();
        }

        [HttpGet("{fileName}")]
        public IActionResult GetAlarmSound(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName) ||
                fileName.Contains("..") ||
                fileName.Contains("/") ||
                fileName.Contains("\\"))
            {
                return BadRequest("Nieprawidłowa nazwa pliku");
            }

            var filePath = Path.Combine(_alarmsPath, fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound($"Plik alarmu '{fileName}' nie został znaleziony");
            }

            if (!_contentTypeProvider.TryGetContentType(filePath, out string contentType))
            {
                contentType = "application/octet-stream";
            }

            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return File(fileStream, contentType, fileName);
        }

        [HttpGet]
        public IActionResult GetAvailableAlarms()
        {
            if (!Directory.Exists(_alarmsPath))
            {
                return NotFound("Folder z alarmami nie istnieje");
            }

            var files = Directory.GetFiles(_alarmsPath)
                .Select(Path.GetFileName)
                .Where(f => f != null)
                .ToList();

            return Ok(new { alarms = files, count = files.Count });
        }
    }
}
