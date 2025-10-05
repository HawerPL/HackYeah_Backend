using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace HackYeah_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResourcesController : ControllerBase
    {
        private readonly string _basePath = Path.Combine("Resources", "Instructions");
        private readonly ILogger<ResourcesController> _logger;

        public ResourcesController(ILogger<ResourcesController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Pobiera listę plików dla danej kategorii
        /// </summary>
        /// <param name="category">Nazwa kategorii (np. Burza, Kryzys, Powódź, Pożar, Upał, Wichura)</param>
        /// <returns>Lista plików w kategorii</returns>
        [HttpGet("category/{category}")]
        public ActionResult<IEnumerable<FileInfoDto>> GetFilesByCategory(string category)
        {
            try
            {
                var categoryPath = Path.Combine(_basePath, category);

                if (!Directory.Exists(categoryPath))
                {
                    return NotFound($"Kategoria '{category}' nie została znaleziona");
                }

                var files = Directory.GetFiles(categoryPath)
                    .Select(file => new FileInfoDto
                    {
                        FileName = Path.GetFileName(file),
                        Category = category,
                        FileSize = new FileInfo(file).Length,
                        Extension = Path.GetExtension(file)
                    })
                    .ToList();

                return Ok(files);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd podczas pobierania plików dla kategorii {Category}", category);
                return StatusCode(500, "Wystąpił błąd podczas pobierania plików");
            }
        }

        /// <summary>
        /// Pobiera pojedynczy plik po nazwie i kategorii
        /// </summary>
        /// <param name="category">Nazwa kategorii</param>
        /// <param name="fileName">Nazwa pliku</param>
        /// <returns>Plik do pobrania</returns>
        [HttpGet("file/{category}/{fileName}")]
        public IActionResult GetFile(string category, string fileName)
        {
            try
            {
                var filePath = Path.Combine(_basePath, category, fileName);

                if (!System.IO.File.Exists(filePath))
                {
                    return NotFound($"Plik '{fileName}' w kategorii '{category}' nie został znaleziony");
                }

                // Zabezpieczenie przed path traversal
                var fullPath = Path.GetFullPath(filePath);
                var baseFullPath = Path.GetFullPath(_basePath);

                if (!fullPath.StartsWith(baseFullPath))
                {
                    return BadRequest("Nieprawidłowa ścieżka pliku");
                }

                var provider = new FileExtensionContentTypeProvider();
                if (!provider.TryGetContentType(fileName, out var contentType))
                {
                    contentType = "application/octet-stream";
                }

                var fileBytes = System.IO.File.ReadAllBytes(filePath);
                return File(fileBytes, contentType, fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd podczas pobierania pliku {FileName} z kategorii {Category}", fileName, category);
                return StatusCode(500, "Wystąpił błąd podczas pobierania pliku");
            }
        }

        /// <summary>
        /// Pobiera listę wszystkich dostępnych kategorii
        /// </summary>
        /// <returns>Lista kategorii</returns>
        [HttpGet("categories")]
        public ActionResult<IEnumerable<string>> GetCategories()
        {
            try
            {
                if (!Directory.Exists(_basePath))
                {
                    return NotFound("Katalog zasobów nie został znaleziony");
                }

                var categories = Directory.GetDirectories(_basePath)
                    .Select(dir => Path.GetFileName(dir))
                    .ToList();

                return Ok(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd podczas pobierania listy kategorii");
                return StatusCode(500, "Wystąpił błąd podczas pobierania kategorii");
            }
        }

        /// <summary>
        /// Pobiera wszystkie pliki ze wszystkich kategorii
        /// </summary>
        /// <returns>Lista wszystkich plików pogrupowanych według kategorii</returns>
        [HttpGet("all")]
        public ActionResult<Dictionary<string, List<FileInfoDto>>> GetAllFiles()
        {
            try
            {
                if (!Directory.Exists(_basePath))
                {
                    return NotFound("Katalog zasobów nie został znaleziony");
                }

                var result = new Dictionary<string, List<FileInfoDto>>();

                foreach (var categoryDir in Directory.GetDirectories(_basePath))
                {
                    var category = Path.GetFileName(categoryDir);
                    var files = Directory.GetFiles(categoryDir)
                        .Select(file => new FileInfoDto
                        {
                            FileName = Path.GetFileName(file),
                            Category = category,
                            FileSize = new FileInfo(file).Length,
                            Extension = Path.GetExtension(file)
                        })
                        .ToList();

                    result[category] = files;
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd podczas pobierania wszystkich plików");
                return StatusCode(500, "Wystąpił błąd podczas pobierania plików");
            }
        }
    }

    public class FileInfoDto
    {
        public string FileName { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public string Extension { get; set; } = string.Empty;
    }
}