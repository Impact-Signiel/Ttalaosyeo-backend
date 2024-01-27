
using signiel.Helpers;

namespace signiel.Models.Responses;

/// <summary>
/// 투어 응답
/// </summary>
public class TripInfo {
    /// <summary>
    /// 투어 ID
    /// </summary>
    public required ulong Id { get; set; }
    /// <summary>
    /// 투어 작성자 ID
    /// </summary>
    public required ulong AuthorId { get; set; }
    /// <summary>
    /// 투어 작성자 닉네임
    /// </summary>
    public required string Author { get; set; }
    /// <summary>
    /// 투어 제목
    /// </summary>
    public required string Title { get; set; }
    /// <summary>
    /// 투어 가격
    /// </summary>
    public required long Price { get; set; }
    /// <summary>
    /// N박
    /// </summary>
    public required uint Nights { get; set; }
    /// <summary>
    /// N일
    /// </summary>
    public required uint Days { get; set; }
    /// <summary>
    /// N박 M일 포메팅
    /// </summary>
    public string DayNights => DayNightFormatter.Format(Days, Nights);
    public required List<KeyValuePair<string, string>> Tags { get; set; }
}

public class TripSearchResponse : APIPagenationResponse {
    public required List<TripInfo> Trips { get; set; }
}