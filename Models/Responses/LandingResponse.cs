
namespace signiel.Models.Responses;

/// <summary>
/// 메인페이지 응답
/// </summary>
public class LandingResponse {
    /// <summary>
    /// 배너
    /// </summary>
    public required List<LandingBannerInfo> Banners { get; set; }
    /// <summary>
    /// 섹션
    /// </summary>
    public required List<LandingSectionInfo> Sections { get; set; }
}

public class LandingBannerInfo {
    /// <summary>
    /// 배너 제목
    /// </summary>
    public required string Title { get; set; }
    /// <summary>
    /// 배너 설명
    /// </summary>
    public required string Description { get; set; }
    /// <summary>
    /// 배너 이미지
    /// </summary>
    public required string Image { get; set; }
    /// <summary>
    /// 배너 쿼리
    /// </summary>
    public required string Query { get; set; }
}

public class LandingSectionInfo {
    /// <summary>
    /// 섹션 레이블
    /// </summary>
    public required string Label { get; set; }
    /// <summary>
    /// 섹션 이미지
    /// </summary>
    public required string Image { get; set; }
    /// <summary>
    /// 섹션 쿼리
    /// </summary>
    public required string Query { get; set; }
    /// <summary>
    /// 섹션 카테고리 투어
    /// </summary>
    public required List<TripInfo> Trips { get; set; }
}