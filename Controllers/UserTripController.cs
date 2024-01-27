using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using signiel.Contexts;
using signiel.Models;
using signiel.Models.Requests;
using signiel.Models.Responses;
using signiel.Services;

namespace signiel.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("api/users/@me")]
public class UserTripController : ControllerBase {
    private readonly ILogger<UserController> _logger;
    private readonly SignielContext _context;
    private readonly TripService _tripService;

    public UserTripController(ILogger<UserController> logger, SignielContext context, TripService tripService) {
        _logger = logger;
        _context = context;
        _tripService = tripService;
    }

    /// <summary>
    /// 내 투어 목록을 가져옵니다.
    /// </summary>
    /// <returns></returns>
    [HttpGet("trips")]
    public async Task<APIResponse<UserTripResponse>> GetTrips() {
        var userId = ulong.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        var tripInfos = await (
            from trip in _context.UserTrips
            where trip.User == userId
            select new {
                trip.Id,
                trip.Date,
                trip.Trip,
            }
        ).ToListAsync();

        var trips = await _tripService.GetDetailsAsync(
            from trip in _context.Trips
            where tripInfos.Select(tripInfo => tripInfo.Trip).Contains(trip.Id)
            select trip
        );

        return APIResponse<UserTripResponse>.FromData(new() {
            Trips = (
                from trip in trips
                join tripInfo in tripInfos on trip.Id equals tripInfo.Trip
                select new UserTripDetail {
                    Id = tripInfo.Id,
                    Date = tripInfo.Date.ToDateTime(TimeOnly.MinValue),
                    Trip = trip,
                }
            ).ToList(),
        });
    }

    /// <summary>
    /// 내 투어 목록에 추가합니다.
    /// </summary>
    [HttpPost("trips")]
    public async Task<APIResponse<ulong>> PostTrip([FromBody] UserTripRequest request) {
        var userId = ulong.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        var trip = await _context.Trips.FindAsync(request.Trip);

        if (trip == null) {
            HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
            return APIResponse<ulong>.FromError("Not Found");
        }

        if (_context.UserTrips.Any(tripInfo => tripInfo.User == userId && tripInfo.Trip == trip.Id)) {
            HttpContext.Response.StatusCode = (int)HttpStatusCode.Conflict;
            return APIResponse<ulong>.FromError("Already Exists");
        }

        var tripInfo = new UserTrip {
            User = userId,
            Trip = trip.Id,
            Date = DateOnly.FromDateTime(request.Date),
        };

        await _context.UserTrips.AddAsync(tripInfo);
        await _context.SaveChangesAsync();

        return APIResponse<ulong>.FromData(tripInfo.Id);
    }
}