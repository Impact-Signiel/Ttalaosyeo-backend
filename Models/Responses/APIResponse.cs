
using System.Text.Json.Serialization;

namespace signiel.Models.Responses;

/// <summary>
/// API응답
/// </summary>
/// <typeparam name="TData">데이터타입</typeparam>
public class APIResponse<TData> {
    /// <summary>
    /// 응답코드
    /// </summary>
    [JsonPropertyName("status")]
    public required string Code { get; set; }
    /// <summary>
    /// 응답데이터
    /// </summary>
    public TData? Data { get; set; } = default;

    public static APIResponse<TData> FromData(TData data) {
        return new APIResponse<TData> {
            Code = "ok",
            Data = data
        };
    }

    public static APIResponse<TData> FromError(string code) {
        return new APIResponse<TData> {
            Code = code
        };
    }
}

/// <summary>
/// API페이징응답
/// </summary>
public class APIPagenationResponse {
    /// <summary>
    /// 페이지
    /// </summary>
    public required int Page { get; set; }
    /// <summary>
    /// 페이지당 항목수
    /// </summary>
    public required int Limit { get; set; }
    /// <summary>
    /// 전체 항목수
    /// </summary>
    public required int Total { get; set; }
    /// <summary>
    /// 전체 페이지수
    /// </summary>
    public int TotalPage => (int)Math.Ceiling((double)Total / Limit);
}