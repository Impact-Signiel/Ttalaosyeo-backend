namespace Models.Requests;

/// <summary>
/// 여행 추천 요청
/// </summary>
public class TripRecommendRequest {
    /// <summary>
    /// 쿼리 목록
    /// </summary>
    [Required, MinLength(1)]
    public required string[] Queries { get; set; }
}