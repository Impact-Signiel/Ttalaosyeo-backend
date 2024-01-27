namespace signiel.Models.Responses;

/// <summary>
/// 내 여행 정보
/// </summary>
public class UserTripDetail {
    /// <summary>
    /// 여행 ID
    /// </summary>
    public required ulong Id { get; set; }
    /// <summary>
    /// 여행 날짜
    /// </summary>
    public required DateTime Date { get; set; }
    /// <summary>
    /// 여행 정보
    /// </summary>
    public required TripInfoDetail Trip { get; set; }
}

public class UserTripResponse {
    /// <summary>
    /// 내 여행 목록
    /// </summary>
    public required List<UserTripDetail> Trips { get; set; }
}