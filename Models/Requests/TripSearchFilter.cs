
using System.ComponentModel;
using System.Web;

namespace signiel.Models.Requests;

public class TripSearchFilter {
    /// <summary>
    /// 검색 키워드 (제목, 내용)
    /// </summary>
    [StringLength(100, MinimumLength = 1), DefaultValue(null)]
    public string? Keyword { get; set; }
    /// <summary>
    /// 작성자 필터
    /// </summary>
    [Range(0, ulong.MaxValue), DefaultValue(null)]
    public ulong? AuthorId { get; set; }
    /// <summary>
    /// 테그 필터
    /// </summary>
    [MinLength(1), DefaultValue(null)]
    public Dictionary<string, string>? Tags { get; set; }
    /// <summary>
    /// 최소 여행일수
    /// </summary>
    [Range(1, 100), DefaultValue(null)]
    public int? MinDays { get; set; }
    /// <summary>
    /// 최대 여행일수
    /// </summary>
    [Range(1, 100), DefaultValue(null)]
    public int? MaxDays { get; set; }
    /// <summary>
    /// 최소 여행가격
    /// </summary>
    [Range(0, int.MaxValue), DefaultValue(null)]
    public int? MinPrice { get; set; }
    /// <summary>
    /// 최대 여행가격
    /// </summary>
    [Range(0, int.MaxValue), DefaultValue(null)]
    public int? MaxPrice { get; set; }

    public static TripSearchFilter operator +(TripSearchFilter a, TripSearchFilter b) {
        var tags = a.Tags != null && b.Tags != null ? new Dictionary<string, string>() : null;

        if (tags != null) {
            if (a.Tags != null) foreach (var (key, value) in a.Tags) {
                    tags.TryAdd(key, value);
                }

            if (b.Tags != null) foreach (var (key, value) in b.Tags) {
                    tags.TryAdd(key, value);
                }
        }

        return new() {
            Keyword = string.Join(",", a.Keyword, b.Keyword).Trim(),
            AuthorId = a.AuthorId ?? b.AuthorId,
            Tags = tags,
            MinDays = a.MinDays ?? b.MinDays,
            MaxDays = a.MaxDays ?? b.MaxDays,
            MinPrice = a.MinPrice ?? b.MinPrice,
            MaxPrice = a.MaxPrice ?? b.MaxPrice,
        };
    }

    public static TripSearchFilter? Parse(string query) {
        try {
            var filter = new TripSearchFilter();
            var queryDictionary = HttpUtility.ParseQueryString(query);

            if (queryDictionary.AllKeys.Contains("keyword")) {
                filter.Keyword = queryDictionary["keyword"];
            }
            if (queryDictionary.AllKeys.Contains("author")) {
                filter.AuthorId = ulong.Parse(queryDictionary["author"]!);
            }
            if (queryDictionary.AllKeys.Contains("tags")) {
                filter.Tags = queryDictionary["tags"]!.Split(',').Select(tag => tag.Split(':')).ToDictionary(tag => tag[0], tag => tag[1]);
            }
            if (queryDictionary.AllKeys.Contains("minDays")) {
                filter.MinDays = int.Parse(queryDictionary["minDays"]!);
            }
            if (queryDictionary.AllKeys.Contains("maxDays")) {
                filter.MaxDays = int.Parse(queryDictionary["maxDays"]!);
            }
            if (queryDictionary.AllKeys.Contains("minPrice")) {
                filter.MinPrice = int.Parse(queryDictionary["minPrice"]!);
            }
            if (queryDictionary.AllKeys.Contains("maxPrice")) {
                filter.MaxPrice = int.Parse(queryDictionary["maxPrice"]!);
            }

            return filter;
        } catch (Exception) {
            if (string.IsNullOrWhiteSpace(query)) {
                return null;
            }

            return new() {
                Keyword = query.Trim(),
            };
        }
    }

    public static TripSearchFilter CombinedParse(params string[] queries) {
        var filter = new TripSearchFilter();

        foreach (var query in queries) {
            var parsed = Parse(query);

            if (parsed == null) {
                continue;
            }

            filter += parsed;
        }

        return filter;
    }
}