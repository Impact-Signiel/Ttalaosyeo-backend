
using System.Net;
using System.Net.Http.Headers;
using System.Net.Mime;
using Microsoft.AspNetCore.StaticFiles;
using signiel.Models.Responses;
using SkiaSharp;

namespace signiel.Controllers;

[ApiController]
[Route("api/media")]
public class MediaController : ControllerBase {
    private readonly ILogger<MediaController> _logger;
    private readonly IConfiguration _configuration;

    public MediaController(ILogger<MediaController> logger, IConfiguration configuration) {
        _logger = logger;
        _configuration = configuration;

        if (!Directory.Exists(_configuration["Media:Path"]!)) {
            Directory.CreateDirectory(_configuration["Media:Path"]!);
        }
    }

    [HttpGet("images/{id}")]
    public async Task<IActionResult> GetImage([FromRoute] string id) {
        if (!Guid.TryParse(id, out var guid)) {
            HttpContext.Response.StatusCode = HttpStatusCode.BadRequest.GetHashCode();
            return new EmptyResult();
        }

        var path = Path.Combine(_configuration["Media:Path"]!, guid.ToString());
        var file = new FileInfo(path);

        if (!file.Exists) {
            HttpContext.Response.StatusCode = HttpStatusCode.NotFound.GetHashCode();
            return new EmptyResult();
        }

        var buffer = new byte[file.Length];
        await using var fs = file.OpenRead();
        await fs.ReadAsync(buffer);

        HttpContext.Response.Headers.ContentDisposition = new ContentDispositionHeaderValue("inline") {
            FileName = file.Name,
        }.ToString();

        return File(buffer, "image/jpeg", file.Name);
    }

    [HttpPost("images")]
    public async Task<APIResponse<string>> PostImage(IFormFile file) {
        try {
            var guid = Guid.NewGuid();
            var path = Path.Combine(_configuration["Media:Path"]!, guid.ToString());

            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);

            await System.IO.File.WriteAllBytesAsync(path, ScaleImage(ms.ToArray(), 1080, 1080));

            var url = new UriBuilder(HttpContext.Request.Scheme, HttpContext.Request.Host.Host, 443, $"api/media/images/{guid}");

            return APIResponse<string>.FromData(url.Uri.ToString());
        } catch (Exception e) {
            HttpContext.Response.StatusCode = HttpStatusCode.BadRequest.GetHashCode();
            return APIResponse<string>.FromError(e.Message);
        }
    }

    public static byte[] ScaleImage(byte[] imageBytes, int maxWidth, int maxHeight) {
        SKBitmap image = SKBitmap.Decode(imageBytes);

        var ratioX = (double)maxWidth / image.Width;
        var ratioY = (double)maxHeight / image.Height;
        var ratio = Math.Min(ratioX, ratioY);

        var newWidth = (int)(image.Width * ratio);
        var newHeight = (int)(image.Height * ratio);

        var info = new SKImageInfo(newWidth, newHeight);
        image = image.Resize(info, SKFilterQuality.High);

        using var ms = new MemoryStream();
        image.Encode(ms, SKEncodedImageFormat.Jpeg, 70);
        return ms.ToArray();
    }
}