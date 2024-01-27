
namespace signiel.Models.Requests;

/// <summary>
/// 투어 결?제
/// </summary>
public class UserTripRequest {
    /// <summary>
    /// 투어 Id
    /// </summary>
    [Required, Range(1, ulong.MaxValue)]
    public ulong Trip { get; set; }
    /// <summary>
    /// 투어 날짜
    /// </summary>
    [Required]
    public DateTime Date { get; set; }
}