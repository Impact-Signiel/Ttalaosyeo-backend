
namespace signiel.Models.Responses;

/// <summary>
/// 작성자 정보
/// </summary>
public class AuthorInfo {
    /// <summary>
    /// 작성자 ID
    /// </summary>
    public required ulong Id { get; set; }
    /// <summary>
    /// 작성자 닉네임
    /// </summary>
    public required string Nickname { get; set; }
}