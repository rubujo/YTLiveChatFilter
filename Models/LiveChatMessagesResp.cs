using Google.Apis.YouTube.v3.Data;

namespace YTLiveChatFilter.Models;

/// <summary>
/// 類別 LiveChatMessagesResp
/// </summary>
public class LiveChatMessagesResp
{
	/// <summary>
	/// 下一個 PageToken
	/// </summary>
	public string NextPageToken { get; set; } = string.Empty;

	/// <summary>
	/// 輪循的間隔毫秒
	/// </summary>
	public long PollingIntervalMillis { get; set; } = 0;

	/// <summary>
	/// LiveChatMessage 列表
	/// </summary>
	public IList<LiveChatMessage>? Items { get; set; }
}