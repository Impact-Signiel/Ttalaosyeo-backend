

using Microsoft.EntityFrameworkCore;
using signiel.Contexts;
using signiel.Models;
using signiel.Models.Responses;
using signiel.Services;

[ApiController]
[Route("api/landing")]
public class LandingController : ControllerBase {
    private readonly ILogger<LandingController> _logger;
    private readonly SignielContext _context;
    private readonly TripService _tripService;

    public LandingController(ILogger<LandingController> logger, SignielContext context, TripService tripSearchService) {
        _logger = logger;
        _context = context;
        _tripService = tripSearchService;
    }

    /// <summary>
    /// 랜딩 페이지 데이터를 가져옵니다.
    /// </summary>
    [HttpGet]
    public async Task<APIResponse<LandingResponse>> GetLanding() {
        var sections = await (
                from section in _context.LandingSections
                select new LandingSectionInfo {
                    Label = section.Label,
                    Image = section.Image,
                    Query = section.Query,
                    Trips = null!,
                }
            ).ToListAsync();

        foreach (var section in sections) {
            section.Trips = await _tripService.Query(section.Query, 0, 4)
                .Select(trip => new TripInfo {
                    Id = trip.Id,
                    AuthorId = trip.Author,
                    Author = trip.AuthorNavigation.Nickname,
                    Title = trip.Title,
                    Price = trip.Price,
                    Nights = trip.Nights,
                    Days = trip.Days,
                    Tags = trip.TripTags.Select(tag => new KeyValuePair<string, string>(tag.Key, tag.Value)).ToList()
                }).ToListAsync();
        }

        return APIResponse<LandingResponse>.FromData(new() {
            Banners = await (
                from banner in _context.LandingBanners
                select new LandingBannerInfo {
                    Title = banner.Title,
                    Description = banner.Description,
                    Image = banner.Image,
                    Query = banner.Query,
                }
            ).ToListAsync(),
            Sections = sections,
        });
    }
}
