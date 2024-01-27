
namespace signiel.Models.Requests;

public class TripRequest {
    /// <summary>
    /// 투어 제목
    /// </summary>
    [Required, StringLength(50)]
    public required string Title { get; set; }
    /// <summary>
    /// 투어 상세
    /// </summary>
    [Required, StringLength(1000)]
    public required string Content { get; set; }
    /// <summary>
    /// 투어 장소
    /// </summary>
    [Required, StringLength(50)]
    public required string Location { get; set; }
    /// <summary>
    /// 투어 인원
    /// </summary>
    [Required, Range(1, uint.MaxValue)]
    public required uint Personnel { get; set; }
    /// <summary>
    /// 투어 가격
    /// </summary>
    [Required, Range(1, long.MaxValue)]
    public required long Price { get; set; }
    /// <summary>
    /// N박
    /// </summary>
    [Required, Range(1, uint.MaxValue)]
    public required uint Nights { get; set; }
    /// <summary>
    /// N일
    /// </summary>
    [Required, Range(1, uint.MaxValue)]
    public required uint Days { get; set; }
    /// <summary>
    /// 투어 태그
    /// </summary>
    [Required]
    public required List<KeyValuePair<string, string>> Tags { get; set; }
    /// <summary>
    /// 투어 썸네일
    /// </summary>
    [Required, StringLength(500)]
    public required string Thumbnail { get; set; }
    /// <summary>
    /// 투어 일정
    /// </summary>
    [Required]
    public required List<TripScheduleDetailRequest> Schedules { get; set; }
    /// <summary>
    /// 투어 이미지
    /// </summary>
    [Required]
    public required List<string> Images { get; set; }
}

/// <summary>
/// 투어 일차 상세
/// </summary>
public class TripScheduleDetailRequest {
    /// <summary>
    /// 일차 제목
    /// </summary>
    [Required, StringLength(100)]
    public required string Title { get; set; }
    /// <summary>
    /// 일차 상세
    /// </summary>
    [Required, StringLength(1000)]
    public required string Description { get; set; }
    /// <summary>
    /// 일차 투어 장소
    /// </summary>
    [Required]
    public required List<TripScheduleLocationDetailRequest> Locations { get; set; }
}

/// <summary>
/// 투어 일차 장소 상세
/// </summary>
public class TripScheduleLocationDetailRequest {
    /// <summary>
    /// 장소
    /// </summary>
    [Required, StringLength(50)]
    public required string Location { get; set; }
    /// <summary>
    /// 장소 제목
    /// </summary>
    [Required, StringLength(100)]
    public required string Title { get; set; }
    /// <summary>
    /// 장소 설명
    /// </summary>
    [Required, StringLength(1000)]
    public required string Description { get; set; }
    /// <summary>
    /// 장소 이미지
    /// </summary>
    [Required]
    public required List<string> Images { get; set; }
}