namespace signiel.Models.Responses;

/// <summary>
/// 투어 추천 아이템
/// </summary>
public class TripRecommendItemInfo {
    /// <summary>
    /// 투어 추천 아이템 텍스트
    /// </summary>
    public required string Text { get; set; }
    /// <summary>
    /// 투어 추천 아이템 이미지
    /// </summary>
    public required string? Image { get; set; }
    /// <summary>
    /// 다음 투어 추천 항목 ID (null이면 결과 화면으로 이동)
    /// </summary>
    public required ulong? Next { get; set; }
    /// <summary>
    /// 검색 쿼리
    /// </summary>
    public required string Query { get; set; }
}

/// <summary>
/// 투어 추천 상세
/// </summary>
public class TripRecommendDetail {
    /// <summary>
    /// 투어 추천 ID
    /// </summary>
    public required ulong Id { get; set; }
    /// <summary>
    /// 투어 추천 제목
    /// </summary>
    public required string Title { get; set; }
    /// <summary>
    /// 선택 아이템
    /// </summary>
    public required List<TripRecommendItemInfo> Items { get; set; }
    /// <summary>
    /// 이미지 여부
    /// </summary>
    public bool HasImage => Items.Count > 0 && Items.All(item => item.Image != null);
}

public class TripRecommendsDetail {
    /// <summary>
    /// 투어 추천 목록
    /// </summary>
    public required List<TripInfo> Trips { get; set; }
}