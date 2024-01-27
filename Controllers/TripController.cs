using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Models.Requests;
using signiel.Contexts;
using signiel.Models;
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
                Items = recommend.TripRecommendItemRecommendNavigations.Select(item => new TripRecommendItemInfo {
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
    public async Task<APIResponse<TripInfoDetail>> GetTrip(
        [FromRoute, Range(1, ulong.MaxValue)] ulong id) {
        var trip = await _tripService.GetDetailAsync(id);

        if (trip == null) {
            HttpContext.Response.StatusCode = HttpStatusCode.NotFound.GetHashCode();
            return APIResponse<TripInfoDetail>.FromError("Not Found");
        }

        return APIResponse<TripInfoDetail>.FromData(trip);
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPost]
    public async Task<APIResponse<ulong>> PostTrip([FromBody] TripRequest request) {
        var trip = new Trip {
            Author = ulong.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!),
            Title = request.Title,
            Content = request.Content,
            Location = request.Location,
            Thumbnail = request.Thumbnail,
            Personnel = request.Personnel,
            Price = request.Price,
            Nights = request.Nights,
            Days = request.Days,
        };

        foreach (var tag in request.Tags) {
            trip.TripTags.Add(new TripTag {
                Key = tag.Key,
                Value = tag.Value,
            });
        }

        foreach (var image in request.Images) {
            trip.TripImages.Add(new TripImage {
                Image = image,
            });
        }

        foreach (var schedule in request.Schedules) {
            var tripSchedule = new TripSchedule {
                Title = schedule.Title,
                Description = schedule.Description,
            };

            foreach (var location in schedule.Locations) {
                var detail = new TripDetail() {
                    Location = location.Location,
                    Title = location.Title,
                    Description = location.Description,
                };

                foreach (var image in location.Images) {
                    detail.TripDetailImages.Add(new() {
                        Image = image,
                    });
                }

                tripSchedule.TripDetails.Add(detail);
            }

            trip.TripSchedules.Add(tripSchedule);
        }

        await _context.Trips.AddAsync(trip);
        await _context.SaveChangesAsync();

        return APIResponse<ulong>.FromData(trip.Id);
    }
}
