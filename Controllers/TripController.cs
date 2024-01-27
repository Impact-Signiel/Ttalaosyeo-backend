using System.Net;
using Microsoft.EntityFrameworkCore;
using Models.Requests;
using signiel.Contexts;
using signiel.Models.Requests;
using signiel.Models.Responses;
using signiel.Services;

namespace signiel.Controllers;

[ApiController]
[Route("api/trips")]
public class TripController : ControllerBase {
    private readonly ILogger<LandingController> _logger;
    private readonly SignielContext _context;
    private readonly TripService _tripService;

    public TripController(ILogger<LandingController> logger, SignielContext context, TripService tripSearchService) {
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
                    Author = new() {
                        Id = trip.Author,
                        Nickname = trip.AuthorNavigation.Nickname,
                    },
                    Title = trip.Title,
                    Price = trip.Price,
                    Nights = trip.Nights,
                    Days = trip.Days,
                    Tags = trip.TripTags.Select(tag => new KeyValuePair<string, string>(tag.Key, tag.Value)).ToList(),
                    Thumbnail = trip.Thumbnail,
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
                    Author = new() {
                        Id = trip.Author,
                        Nickname = trip.AuthorNavigation.Nickname,
                    },
                    Title = trip.Title,
                    Price = trip.Price,
                    Nights = trip.Nights,
                    Days = trip.Days,
                    Tags = trip.TripTags.Select(tag => new KeyValuePair<string, string>(tag.Key, tag.Value)).ToList(),
                    Thumbnail = trip.Thumbnail,
                }).ToListAsync()
        });
    }

    /// <summary>
    /// 투어 상세 정보를 가져옵니다.
    /// </summary>
    /// <param name="id">투어 ID</param>
    [HttpGet("{id}")]
    public async Task<APIResponse<TripDetail>> GetTrip(
        [FromRoute, Range(1, ulong.MaxValue)] ulong id) {
        var trip = await (
            from t
            in _context.Trips
            where t.Id == id
            select new TripDetail {
                Id = t.Id,
                Author = new() {
                    Id = t.Author,
                    Nickname = t.AuthorNavigation.Nickname,
                },
                Title = t.Title,
                Content = t.Content,
                Location = t.Location,
                Personnel = t.Personnel,
                Price = t.Price,
                Nights = t.Nights,
                Days = t.Days,
                CreatedAt = t.CreatedAt,
                Tags = t.TripTags.Select(tag => new KeyValuePair<string, string>(tag.Key, tag.Value)).ToList(),
                Thumbnail = t.Thumbnail,
                Schedules = t.TripSchedules.Select(schedule => new TripScheduleDetail {
                    Day = 0,
                    Title = schedule.Title,
                    Description = schedule.Description,
                    Locations = schedule.TripDetails.Select(location => new TripScheduleLocationDetail {
                        Location = location.Location,
                        Title = location.Title,
                        Description = location.Description,
                        Images = location.TripDetailImages.Select(image => image.Image).ToList(),
                    }).ToList(),
                }).ToList(),
            }
        ).FirstOrDefaultAsync();

        if (trip == null) {
            HttpContext.Response.StatusCode = HttpStatusCode.NotFound.GetHashCode();
            return APIResponse<TripDetail>.FromError("Not Found");
        }


        foreach (var schedule in trip.Schedules) {
            schedule.Day = trip.Schedules.IndexOf(schedule) + 1;
        }

        return APIResponse<TripDetail>.FromData(trip);
    }
}
