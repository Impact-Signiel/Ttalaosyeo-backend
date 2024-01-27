
using Microsoft.EntityFrameworkCore;
using signiel.Contexts;
using signiel.Models;
using signiel.Models.Requests;

namespace signiel.Services;

public class TripService {
    private readonly ILogger<TripService> _logger;
    private readonly SignielContext _context;

    public TripService(ILogger<TripService> logger, SignielContext context) {
        _logger = logger;
        _context = context;
    }

    public IQueryable<Trip> Query(TripSearchFilter filter, int? page = null, int? limit = null) {
        var query = _context.Trips.AsQueryable();

        if (filter.AuthorId != null) {
            query = query.Where(trip => trip.Author == filter.AuthorId);
        }

        if (filter.Keyword != null) {
            var keywords = filter.Keyword.Split(',');

            foreach (var keyword in keywords) {
                query = query.Where(trip => trip.Title.Contains(keyword));
            }
        }

        if (filter.MaxDays != null) {
            query = query.Where(trip => trip.Days <= filter.MaxDays);
        }

        if (filter.MinDays != null) {
            query = query.Where(trip => trip.Days >= filter.MinDays);
        }

        if (filter.Tags != null) {
            foreach (var (key, value) in filter.Tags) {
                query = query.Where(trip => trip.TripTags.Any(tag => tag.Key == key && tag.Value == value));
            }
        }

        if (filter.MinPrice != null) {
            query = query.Where(trip => trip.Price >= filter.MinPrice);
        }

        if (filter.MaxPrice != null) {
            query = query.Where(trip => trip.Price <= filter.MaxPrice);
        }

        if (page != null && limit != null) {
            query = query.Skip(page.Value * limit.Value).Take(limit.Value);
        }

        return query;
    }

    public IQueryable<Trip> Query(string filter, int? page = null, int? limit = null) {
        var parsed = TripSearchFilter.Parse(filter);

        if (parsed == null) {
            return _context.Trips.Where(trip => false);
        }

        return Query(parsed, page, limit);
    }

    public async Task<List<Trip>> SearchAsync(TripSearchFilter filter, int? page = null, int? limit = null) {
        return await Query(filter, page, limit).ToListAsync();
    }

    public async Task<List<Trip>> SearchAsync(string query, int? page = null, int? limit = null) {
        return await Query(query, page, limit).ToListAsync();
    }
}