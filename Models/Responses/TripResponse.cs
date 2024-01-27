
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
    /// 투어 작성자
    /// </summary>
    public required AuthorInfo Author { get; set; }
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
    /// <summary>
    /// 투어 태그
    /// </summary>
    public required List<KeyValuePair<string, string>> Tags { get; set; }
    /// <summary>
    /// 투어 썸네일
    /// </summary>
    public required string Thumbnail { get; set; }
}

/// <summary>
/// 투어 검색 응답
/// </summary>
public class TripSearchResponse : APIPagenationResponse {
    /// <summary>
    /// 투어 목록
    /// </summary>
    public required List<TripInfo> Trips { get; set; }
}

/// <summary>
/// 투어 상세 응답
/// </summary>
public class TripInfoDetail {
    /// <summary>
    /// 투어 ID
    /// </summary>
    public required ulong Id { get; set; }
    /// <summary>
    /// 투어 작성자
    /// </summary>
    public required AuthorInfo Author { get; set; }
    /// <summary>
    /// 투어 제목
    /// </summary>
    public required string Title { get; set; }
    /// <summary>
    /// 투어 상세
    /// </summary>
    public required string Content { get; set; }
    /// <summary>
    /// 투어 장소
    /// </summary>
    public required string Location { get; set; }
    /// <summary>
    /// 투어 인원
    /// </summary>
    public required uint Personnel { get; set; }
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
    /// <summary>
    /// 투어 작성일
    /// </summary>
    public required DateTime CreatedAt { get; set; }
    /// <summary>
    /// 투어 태그
    /// </summary>
    public required List<KeyValuePair<string, string>> Tags { get; set; }
    /// <summary>
    /// 투어 썸네일
    /// </summary>
    public required string Thumbnail { get; set; }
    /// <summary>
    /// 투어 일정
    /// </summary>
    public required List<TripScheduleDetail> Schedules { get; set; }
}

/// <summary>
/// 투어 일차 상세
/// </summary>
public class TripScheduleDetail {
    /// <summary>
    /// 일차
    /// </summary>
    public required int Day { get; set; }
    /// <summary>
    /// 일차 제목
    /// </summary>
    public required string Title { get; set; }
    /// <summary>
    /// 일차 상세
    /// </summary>
    public required string Description { get; set; }
    /// <summary>
    /// 일차 투어 장소
    /// </summary>
    public required List<TripScheduleLocationDetail> Locations { get; set; }
}

/// <summary>
/// 투어 일차 장소 상세
/// </summary>
public class TripScheduleLocationDetail {
    /// <summary>
    /// 장소
    /// </summary>
    public required string Location { get; set; }
    /// <summary>
    /// 장소 제목
    /// </summary>
    public required string Title { get; set; }
    /// <summary>
    /// 장소 설명
    /// </summary>
    public required string Description { get; set; }
    /// <summary>
    /// 장소 이미지
    /// </summary>
    public required List<string> Images { get; set; }
}