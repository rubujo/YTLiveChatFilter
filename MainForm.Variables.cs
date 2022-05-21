using Google.Apis.YouTube.v3;

namespace YTLiveChatFilter;

// 阻擋設計工具。
partial class DesignerBlocker { };

public partial class MainForm
{
    /// <summary>
    /// 共享的 SharedHttpClientFactory
    /// </summary>
    private readonly IHttpClientFactory SharedHttpClientFactory;

    /// <summary>
    /// 共享的 liveChatId
    /// </summary>
    private string SharedLiveChatID = string.Empty;

    /// <summary>
    /// 共享的 NextPageToken
    /// </summary>
    private string SharedNextPageToken = string.Empty;

    /// <summary>
    /// 共享的輪詢間隔毫秒數
    /// </summary>
    private double SharedPollingIntervalMillis = 0.0;

    /// <summary>
    /// 封鎖的字詞
    /// </summary>
    private readonly List<string> BanWords = new();

    /// <summary>
    /// 可疑的頻道 ID
    /// </summary>
    private readonly List<string> SuspiciousChannelIds = new();

    /// <summary>
    /// 可疑的頻道
    /// </summary>
    private readonly Dictionary<string, string> SuspiciousChannels = new();

    /// <summary>
    /// 共享的 YouTubeService
    /// </summary>
    private YouTubeService? SharedYouTubeService = null;

    /// <summary>
    /// 共享的 System.Timers.Timer
    /// </summary>
    private readonly System.Timers.Timer SharedTimer = new();

    /// <summary>
    /// 共享的 CancellationTokenSource
    /// </summary>
    private CancellationTokenSource SharedCancellationTokenSource = new();

    /// <summary>
    /// 共享的 CancellationToken
    /// </summary>
    private CancellationToken? SharedCancellationToken = null;
}