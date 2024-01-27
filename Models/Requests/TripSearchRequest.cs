
using System.ComponentModel;
using signiel.Models.Requests;

namespace Models.Requests;

/// <summary>
/// 여행 검색 
/// </summary>
public class TripSearchRequest {
    /// <summary>
    /// 페이지
    /// </summary>
    [Required, Range(1, int.MaxValue), DefaultValue(1)]
    public required int Page { get; set; }
    /// <summary>
    /// 쿼리
    /// </summary>
    [Required]
    public required TripSearchFilter Filter { get; set; }
}