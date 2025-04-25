using Google.Apis.Auth.OAuth2;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using YTLiveChatFilter.Models;

namespace YTLiveChatFilter.Common;

/// <summary>
/// YouTube 聊天室工具
/// </summary>
public class YTLiveChatUtil
{
    /// <summary>
    /// 取得 YouTubeService
    /// </summary>
    /// <param name="appName">字串，應用程式的名稱</param>
    /// <param name="apiKey">字串，API 金鑰</param>
    /// <param name="clientSecretFilePath">字串，client_secret.json 檔案的路徑</param>
    /// <param name="useOAuth20">布林值，是否使用 OAuth 2.0</param>
    /// <returns>Task&lt;YouTubeService&gt;</returns>
    public static async Task<YouTubeService> GetYouTubeService(
        string appName,
        string apiKey,
        string clientSecretFilePath,
        bool useOAuth20)
    {
        UserCredential? userCredential = null;

        using (FileStream fileStream = new(
            path: clientSecretFilePath,
            mode: FileMode.Open,
            access: FileAccess.Read))
        {
            if (useOAuth20)
            {
                GoogleClientSecrets googleClientSecrets = await GoogleClientSecrets
                    .FromStreamAsync(stream: fileStream);

                ClientSecrets clientSecrets = googleClientSecrets.Secrets;

                string foldaerPath = Path.Combine(
                    path1: AppDomain.CurrentDomain.BaseDirectory,
                    path2: Properties.Settings.Default.AuthStoreFolderName);

                // 自定義存放的路徑。
                GoogleWebAuthorizationBroker.Folder = foldaerPath;

                CancellationTokenSource cancellationTokenSource = new();

                CancellationToken cancellationToken = cancellationTokenSource.Token;

                // 在指定秒數後自動取消。
                cancellationTokenSource.CancelAfter(delay: TimeSpan.FromSeconds(
                    value: Properties.Settings.Default.DefaultTimeoutSeconds));

                userCredential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    clientSecrets: clientSecrets,
                    scopes:
                    [
                        YouTubeService.Scope.Youtube,
                        YouTubeService.Scope.YoutubeForceSsl
                    ],
                    user: Environment.UserName,
                    taskCancellationToken: cancellationToken,
                    dataStore: null,
                    codeReceiver: null);
            }
        }

        return new(initializer: new()
        {
            ApiKey = apiKey,
            ApplicationName = appName,
            HttpClientInitializer = useOAuth20 ? userCredential : null
        });
    }

    /// <summary>
    /// 取得 liveChatId
    /// </summary>
    /// <param name="youTubeService">YouTubeService</param>
    /// <param name="videoId">字串，影片的 ID 值</param>
    /// <param name="control">TextBox</param>
    /// <returns>Task&lt;string&gt;</returns>
    public static async Task<string> GetLiveChatID(
        YouTubeService? youTubeService,
        string videoId,
        TextBox control)
    {
        string liveChatId = string.Empty;

        if (youTubeService != null)
        {
            VideosResource.ListRequest request = youTubeService.Videos
                .List(part: "liveStreamingDetails");

            request.Id = videoId;
            request.Hl = "zh_TW";

            VideoListResponse response = await request.ExecuteAsync();

            Video? video = response.Items.FirstOrDefault();

            if (video != null)
            {
                VideoLiveStreamingDetails liveStreamingDetails = video
                    .LiveStreamingDetails;

                if (liveStreamingDetails != null)
                {
                    liveChatId = liveStreamingDetails.ActiveLiveChatId;
                }
            }
        }
        else
        {
            CustomFunction.WriteLog(
                control: control,
                message: "youTubeService 是 null。");
        }

        return liveChatId;
    }

    /// <summary>
    /// 取得 LiveChatMessagesResp
    /// </summary>
    /// <param name="youTubeService">YouTubeService</param>
    /// <param name="liveChatId">字串，liveChatId</param>
    /// <param name="pageToken">字串，PageToken</param>
    /// <param name="control">TextBox</param>
    /// <returns>Task&lt;LiveChatMessagesResp&gt;</returns>
    public static async Task<LiveChatMessagesResp> GetLiveChatMessagesResp(
        YouTubeService? youTubeService,
        string liveChatId,
        string pageToken,
        TextBox control)
    {
        LiveChatMessagesResp liveChatMessagesResp = new();

        if (youTubeService != null)
        {
            LiveChatMessagesResource.ListRequest request = youTubeService
                .LiveChatMessages
                .List(
                    liveChatId: liveChatId,
                    part: "id,snippet,authorDetails");

            request.MaxResults = 2000;
            request.Hl = "zh-TW";

            if (!string.IsNullOrEmpty(value: pageToken))
            {
                request.PageToken = pageToken;
            }

            LiveChatMessageListResponse response = await request.ExecuteAsync();

            string log = $"每頁資料 {response.PageInfo.ResultsPerPage} 筆，" +
                $"資料共 {response.PageInfo.TotalResults} 筆。";

            CustomFunction.WriteLog(control: control, message: log);

            liveChatMessagesResp.NextPageToken = response.NextPageToken;
            liveChatMessagesResp.PollingIntervalMillis = response.PollingIntervalMillis ?? 0;
            liveChatMessagesResp.Items = response.Items;
        }
        else
        {
            CustomFunction.WriteLog(
                control: control,
                message: "youTubeService 是 null。");
        }

        return liveChatMessagesResp;
    }

    /// <summary>
    /// 暫時停用／隱藏使用者
    /// </summary>
    /// <param name="youTubeService">YouTubeService</param>
    /// <param name="liveChatBan">LiveChatBan</param>
    /// <param name="control">TextBox</param>
    /// <returns>Task&lt;LiveChatBan?&gt;</returns>
    public static async Task<LiveChatBan?> BanUser(
        YouTubeService? youTubeService,
        LiveChatBan liveChatBan,
        TextBox control)
    {
        if (youTubeService != null)
        {
            LiveChatBansResource.InsertRequest request = youTubeService
                .LiveChatBans
                .Insert(
                    body: liveChatBan,
                    part: "snippet");

            return await request.ExecuteAsync();
        }
        else
        {
            CustomFunction.WriteLog(
                control: control,
                message: "youTubeService 是 null。");
        }

        return null;
    }

    /// <summary>
    /// 建立 LiveChatBan
    /// </summary>
    /// <param name="liveChatId">字串，liveChatId</param>
    /// <param name="channelId">字串，頻道的 ID 值</param>
    /// <param name="banPermanent">布林值，判斷是否隱藏使用者</param>
    /// <param name="banDuration">數值，暫時停用秒數</param>
    /// <returns>LiveChatBan</returns>
    public static LiveChatBan CreatLiveChatBan(
        string liveChatId,
        string channelId,
        bool banPermanent,
        int banDuration)
    {
        return new()
        {
            Snippet = new()
            {
                LiveChatId = liveChatId,
                Type = banPermanent ? "permanent" : "temporary",
                BanDurationSeconds = banPermanent ? null : Convert.ToUInt64(value: banDuration),
                BannedUserDetails = new()
                {
                    ChannelId = channelId
                }
            }
        }; ;
    }

    /// <summary>
    /// 刪除訊息
    /// </summary>
    /// <param name="youTubeService">YouTubeService</param>
    /// <param name="messageId">字串，訊息的 ID</param>
    /// <param name="control">TextBox</param>
    /// <returns>Task&lt;string&gt;</returns>
    public static async Task<string> DeleteMessage(
        YouTubeService? youTubeService,
        string messageId,
        TextBox control)
    {
        if (youTubeService != null)
        {
            LiveChatMessagesResource.DeleteRequest request = youTubeService
                .LiveChatMessages
                .Delete(id: messageId);

            return await request.ExecuteAsync();
        }
        else
        {
            CustomFunction.WriteLog(
                control: control,
                message: "youTubeService 是 null。");
        }

        return string.Empty;
    }
}