

using System.Net;
using Microsoft.EntityFrameworkCore;
using Models.Requests;
using signiel.Contexts;
using signiel.Models.Requests;
using signiel.Models.Responses;
using signiel.Services;

[ApiController]
[Route("api/trips")]
public class TripsController : ControllerBase {
    private readonly ILogger<LandingController> _logger;
    private readonly SignielContext _context;
    private readonly TripService _tripService;

    public TripsController(ILogger<LandingController> logger, SignielContext context, TripService tripSearchService) {
        _logger = logger;
        _context = context;
        _tripService = tripSearchService;
    }

    /// <summary>
    /// 투어 추천 항목을 가져옵니다.
    /// </summary>
    /// <param name="id">추천 항목. 기본값: 1</param>
    [HttpGet("recommends/{id}")]
    public async Task<APIResponse<TripRecommendDetail>> GetRecommend(
        [FromRoute, Range(1, ulong.MaxValue)] ulong id = 1) {
        var result = await (
            from recommend
            in _context.TripRecommends
            where recommend.Id == id
            select new TripRecommendDetail {
                Id = recommend.Id,
                Title = recommend.Title,
                Items = recommend.TripRecommendItemRecommendNavigations.Select(item => new TripRecommendItem {
                    Text = item.Text,
                    Image = item.Image,
                    Next = item.Next,
                    Query = item.Query,
                }).ToList(),
            }
        ).FirstOrDefaultAsync();

        if (result == null) {
            HttpContext.Response.StatusCode = HttpStatusCode.NotFound.GetHashCode();
            return APIResponse<TripRecommendDetail>.FromError("Not Found");
        }

        return APIResponse<TripRecommendDetail>.FromData(result);
    }

    /// <summary>
    /// 투어 추천 결과를 가져옵니다.
    /// </summary>
    /// <param name="request">쿼리</param>
    [HttpPost("recommends")]
    public async Task<APIResponse<TripRecommendsDetail>> SearchRecommends(
        [FromBody] TripRecommendRequest request) {
        return APIResponse<TripRecommendsDetail>.FromData(new() {
            Trips = await _tripService.Query(TripSearchFilter.CombinedParse(request.Queries), 0, 15)
                .Select(trip => new TripInfo {
                    Id = trip.Id,
                    AuthorId = trip.Author,
                    Author = trip.AuthorNavigation.Nickname,
                    Title = trip.Title,
                    Price = trip.Price,
                    Nights = trip.Nights,
                    Days = trip.Days,
                    Tags = trip.TripTags.Select(tag => new KeyValuePair<string, string>(tag.Key, tag.Value)).ToList()
                }).ToListAsync()
        });
    }

    /// <summary>
    /// 투어를 검색합니다.
    /// </summary>
    /// <param name="request">쿼리</param>
    [HttpPost("search")]
    public async Task<APIResponse<TripSearchResponse>> PostSearch(
        [FromBody] TripSearchRequest request) {
        const int limit = 20;

        var query = _tripService.Query(request.Filter, request.Page - 1, limit);

        return APIResponse<TripSearchResponse>.FromData(new() {
            Page = request.Page,
            Limit = limit,
            Total = await query.CountAsync(),
            Trips = await query
                .Select(trip => new TripInfo {
                    Id = trip.Id,
                    AuthorId = trip.Author,
                    Author = trip.AuthorNavigation.Nickname,
                    Title = trip.Title,
                    Price = trip.Price,
                    Nights = trip.Nights,
                    Days = trip.Days,
                    Tags = trip.TripTags.Select(tag => new KeyValuePair<string, string>(tag.Key, tag.Value)).ToList()
                }).ToListAsync()
        });
    }
}
