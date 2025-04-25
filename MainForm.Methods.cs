using Ganss.Text;
using Google.Apis.YouTube.v3.Data;
using System.Reflection;
using System.Timers;
using YTLiveChatFilter.Common;
using YTLiveChatFilter.Extensions;
using YTLiveChatFilter.Models;
using YTLiveChatFilter.Properties;

namespace YTLiveChatFilter;

// 阻擋設計工具。
partial class DesignerBlocker { };

public partial class MainForm
{
    /// <summary>
    /// 自定義初始化
    /// </summary>
    private void CustomInit()
    {
        InitBanWords();
        InitTimer();
        InitControls();
    }

    /// <summary>
    /// 初始化控制項
    /// </summary>
    private void InitControls()
    {
        LVersion.InvokeIfRequired(action: () =>
        {
            Version? version = Assembly.GetExecutingAssembly().GetName().Version;

            string verText = version != null ? $"v{version}" : "無";

            // 設定版本號顯示。
            LVersion.Text = $"版本號：{verText}";
        });

        CBUseOAuth20.InvokeIfRequired(action: () =>
        {
            CBUseOAuth20.Checked = Settings.Default.UseOAuth20;
        });

        TBAPIKey.InvokeIfRequired(action: () =>
        {
            TBAPIKey.Text = Settings.Default.APIKey;
        });

        TBClientSecretFilePath.InvokeIfRequired(action: () =>
        {
            TBClientSecretFilePath.Text = Settings.Default.ClientSecretPath;
        });

        TBVideoID.InvokeIfRequired(action: TBVideoID.Select);

        TBBanWords.InvokeIfRequired(action: () =>
        {
            TBBanWords.Text = Settings.Default.BanWords;
        });

        CBBanTemporary.InvokeIfRequired(action: () =>
        {
            CBBanTemporary.Checked = Settings.Default.BanTemporary;
        });

        CBBanPermanent.InvokeIfRequired(action: () =>
        {
            CBBanPermanent.Checked = Settings.Default.BanPermanent;
        });

        CBDeleteMessage.InvokeIfRequired(action: () =>
        {
            CBDeleteMessage.Checked = Settings.Default.DeleteMessage;
        });

        NUPBanDuration.InvokeIfRequired(action: () =>
        {
            NUPBanDuration.Value = Settings.Default.BanDuration;
        });

        TBSuspiciousChannelIds.InvokeIfRequired(action: () =>
        {
            TBSuspiciousChannelIds.ContextMenuStrip = CustomFunction.GetContextMenuStrip(
                text: Text,
                suspiciousChannels: SuspiciousChannels,
                appIcon: Resources.app_icon,
                TBSuspiciousChannelIds: TBSuspiciousChannelIds,
                TBLog: TBLog);
        });

        SetControlState(enable: true);
    }

    /// <summary>
    /// 初始化 banWords
    /// </summary>
    private void InitBanWords()
    {
        TBBanWords.InvokeIfRequired(action: () =>
        {
            if (string.IsNullOrEmpty(value: Settings.Default.BanWords))
            {
                string value = Settings.Default.DefaultBanWords;

                string[] defaultDeniedWords = value.Split(
                    separator: [','],
                    options: StringSplitOptions.RemoveEmptyEntries);

                TBBanWords.Text = string.Join(
                    separator: Environment.NewLine,
                    value: defaultDeniedWords);

                CustomFunction.WriteLog(
                    control: TBLog,
                    message: "已從設定檔案內載入預設的封鎖的字詞。");
            }
        });
    }

    /// <summary>
    /// 初始化 SharedTimer
    /// </summary>
    private void InitTimer()
    {
        SharedTimer.Elapsed += async (object? sender, ElapsedEventArgs e) =>
        {
            try
            {
                if (SharedYouTubeService == null)
                {
                    CustomFunction.WriteLog(
                        control: TBLog,
                        message: "SharedYTSvc 為 null，停止作業。");

                    BtnStop_Click(sender: null, e: null);

                    return;
                }

                bool banTemporary = false,
                    banPermanent = false,
                    deleteMessage = false,
                    useOAuth20 = false;

                int banDuration = 300;

                CBUseOAuth20.InvokeIfRequired(action: () =>
                {
                    useOAuth20 = CBUseOAuth20.Checked;
                });

                CBBanTemporary.InvokeIfRequired(action: () =>
                {
                    banTemporary = CBBanTemporary.Checked;
                });

                CBBanPermanent.InvokeIfRequired(action: () =>
                {
                    banPermanent = CBBanPermanent.Checked;
                });

                CBDeleteMessage.InvokeIfRequired(action: () =>
                {
                    deleteMessage = CBDeleteMessage.Checked;
                });

                NUPBanDuration.InvokeIfRequired(action: () =>
                {
                    banDuration = Convert.ToInt32(value: NUPBanDuration.Value);
                });

                LiveChatMessagesResp response = await YTLiveChatUtil.GetLiveChatMessagesResp(
                    youTubeService: SharedYouTubeService,
                    liveChatId: SharedLiveChatID,
                    pageToken: SharedNextPageToken,
                    control: TBLog);

                SharedNextPageToken = response.NextPageToken;
                SharedPollingIntervalMillis = Convert.ToDouble(value: response.PollingIntervalMillis);

                CustomFunction.WriteLog(
                    control: TBLog,
                    message: $"在 {SharedPollingIntervalMillis} 毫秒後進行下一次資料抓取。");

                FilterMessages(
                    items: response.Items,
                    deleteMessage: deleteMessage,
                    banTemporary: banTemporary,
                    banPermanent: banPermanent,
                    banDuration: banDuration,
                    useOAuth20: useOAuth20);

                if (string.IsNullOrEmpty(value: SharedNextPageToken))
                {
                    CustomFunction.WriteLog(
                        control: TBLog,
                        message: "SharedNextPageToken 為空值，停止作業。");

                    BtnStop_Click(sender: null, e: null);

                    return;
                }

                SharedTimer.Interval = Convert.ToDouble(value: response.PollingIntervalMillis);
            }
            catch (Exception ex)
            {
                CustomFunction.WriteLog(
                    control: TBLog,
                    message: ex.Message);

                BtnStop_Click(sender: null, e: null);
            }
        };
    }

    /// <summary>
    /// 取得第一個 
    /// </summary>
    /// <param name="apiKey">字串，金鑰</param>
    /// <param name="clientSecretFilePath">字串，client_secret.json 檔案的路徑</param>
    /// <param name="videoId">字串，影片的 ID</param>
    /// <param name="deleteMessage">布林值，是否刪除訊息</param>
    /// <param name="banTemporary">布林值，是否暫時停用</param>
    /// <param name="banPermanent">布林值，是否永久封鎖</param>
    /// <param name="banDuration">數值，封鎖秒數</param>
    /// <param name="useOAuth20">布林值，是否使用 OAuth 2.0 用戶端 ID</param>
    /// <returns>Task</returns>
    private async Task GetFirstLiveChatMessagesResp(
        string apiKey,
        string clientSecretFilePath,
        string videoId,
        bool deleteMessage,
        bool banTemporary,
        bool banPermanent,
        int banDuration,
        bool useOAuth20)
    {
        SharedYouTubeService = await YTLiveChatUtil.GetYouTubeService(
            appName: Settings.Default.AppName,
            apiKey: apiKey,
            clientSecretFilePath: clientSecretFilePath,
            useOAuth20: useOAuth20);

        SharedLiveChatID = await YTLiveChatUtil.GetLiveChatID(
            youTubeService: SharedYouTubeService,
            videoId: videoId,
            control: TBLog);

        if (!string.IsNullOrEmpty(value: SharedLiveChatID))
        {
            LiveChatMessagesResp response = await YTLiveChatUtil.GetLiveChatMessagesResp(
                youTubeService: SharedYouTubeService,
                liveChatId: SharedLiveChatID,
                pageToken: string.Empty,
                control: TBLog);

            SharedNextPageToken = response.NextPageToken;
            SharedPollingIntervalMillis = Convert.ToDouble(value: response.PollingIntervalMillis);

            CustomFunction.WriteLog(
                control: TBLog,
                message: $"在 {SharedPollingIntervalMillis} 毫秒後進行下一次資料抓取。");

            FilterMessages(
                 items: response.Items,
                 deleteMessage: deleteMessage,
                 banTemporary: banTemporary,
                 banPermanent: banPermanent,
                 banDuration: banDuration,
                 useOAuth20: useOAuth20);

            if (!string.IsNullOrEmpty(value: SharedNextPageToken))
            {
                SharedTimer.Interval = SharedPollingIntervalMillis;
                SharedTimer.Start();
            }
            else
            {
                CustomFunction.WriteLog(
                    control: TBLog,
                    message: "SharedNextPageToken 為空值，停止作業。");

                BtnStop_Click(sender: null, e: null);
            }
        }
        else
        {
            CustomFunction.WriteLog(
                control: TBLog,
                message: "該直播已結束。");

            BtnStop_Click(sender: null, e: null);
        }
    }

    /// <summary>
    /// 過濾訊息
    /// </summary>
    /// <param name="items">IList&lt;LiveChatMessage&gt;?</param>
    /// <param name="deleteMessage">布林值，是否刪除訊息</param>
    /// <param name="banTemporary">布林值，是否暫時停用</param>
    /// <param name="banPermanent">布林值，是否永久封鎖</param>
    /// <param name="banDuration">數值，封鎖秒數</param>
    /// <param name="useOAuth20">布林值，是否使用 OAuth 2.0 用戶端 ID</param>
    private async void FilterMessages(
        IList<LiveChatMessage>? items,
        bool deleteMessage,
        bool banTemporary,
        bool banPermanent,
        int banDuration,
        bool useOAuth20)
    {
        if (items == null)
        {
            CustomFunction.WriteLog(
                control: TBLog,
                message: "傳入 FilterMessages() 的 items 是 null。");

            return;
        }

        // 使用頻道 ID 進行 Grouping。
        IEnumerable<IGrouping<string, LiveChatMessage>> groupedDataSet =
             items.GroupBy(keySelector: n => n.AuthorDetails.ChannelId);

        // 逐個組合處理。
        foreach (IGrouping<string, LiveChatMessage> dataSet in groupedDataSet)
        {
            // 逐筆訊息處理。
            foreach (LiveChatMessage liveChatMessage in dataSet)
            {
                bool isDetected = false;

                // 初始化 AhoCorasick。
                AhoCorasick ahoCorasick = new(
                       comparer: CharComparer.CurrentCultureIgnoreCase,
                       words: BanWords);

                isDetected = CustomFunction.CheckBanWords(
                    control: TBLog,
                    ahoCorasick: ahoCorasick,
                    liveChatMessage: liveChatMessage,
                    suspiciousChannelIds: ref SuspiciousChannelIds,
                    suspiciousChannels: ref SuspiciousChannels,
                    action: UpdateTBSuspiciousChannelIds);

                if (!isDetected)
                {
                    continue;
                }

                if (!useOAuth20)
                {
                    CustomFunction.WriteLog(
                       control: TBLog,
                       message: "因為您使用「API 金鑰」，" +
                           "所以只會將頻道的 ID 加入至可疑的頻道 ID 清單內。");

                    continue;
                }

                try
                {
                    string channelId = liveChatMessage.AuthorDetails.ChannelId,
                        authorName = liveChatMessage.AuthorDetails.DisplayName,
                        messageId = liveChatMessage.Id;

                    bool isChatOwner = liveChatMessage.AuthorDetails.IsChatOwner ?? false,
                        isChatModerator = liveChatMessage.AuthorDetails.IsChatModerator ?? false,
                        _isChatSponsor = liveChatMessage.AuthorDetails.IsChatSponsor ?? false,
                        _isVerified = liveChatMessage.AuthorDetails.IsVerified ?? false;

                    // 針對「擁有者」以及「管理員」不進行過濾。
                    if (isChatOwner || isChatModerator)
                    {
                        string role = "無關人員";

                        if (isChatModerator)
                        {
                            role = "管理員";
                        }

                        if (isChatOwner)
                        {
                            role = "擁有者";
                        }

                        string msgHasSpecialRole = $"頻道「{authorName}（{channelId}）」" +
                            $"是此頻道的「{role}」，故忽略不進行後續的處理。";

                        CustomFunction.WriteLog(
                            control: TBLog,
                            message: msgHasSpecialRole);

                        continue;
                    }

                    // 刪除訊息。
                    if (deleteMessage)
                    {
                        // 會回傳空值。
                        _ = await YTLiveChatUtil.DeleteMessage(
                            youTubeService: SharedYouTubeService,
                            messageId: messageId,
                            control: TBLog);

                        CustomFunction.WriteLog(
                            control: TBLog,
                            message: $"已刪除含有封鎖的字詞的訊息。（訊息 ID：{messageId}）");
                    }

                    // 封鎖使用者。
                    if (banTemporary || banPermanent)
                    {
                        LiveChatBan liveChatBan = YTLiveChatUtil.CreatLiveChatBan(
                            liveChatId: SharedLiveChatID,
                            channelId: channelId,
                            banPermanent: banPermanent,
                            banDuration: banDuration);

                        LiveChatBan? banResult = await YTLiveChatUtil.BanUser(
                            youTubeService: SharedYouTubeService,
                            liveChatBan: liveChatBan,
                            control: TBLog);

                        if (banResult == null)
                        {
                            CustomFunction.WriteLog(
                                control: TBLog,
                                message: "YTLiveChatUtil.BanUser() 回傳的 banResult 是 null。");

                            continue;
                        }

                        // 不會含有 "BanDurationSeconds"。
                        string banType = banPermanent ? "加入至隱藏的使用者" : "暫時停用",
                            banDurationStr = banPermanent ? string.Empty : $" {banDuration} 秒",
                            channel = $"{banResult.Snippet.BannedUserDetails.DisplayName}" +
                                $"（{banResult.Snippet.BannedUserDetails.ChannelId}）",
                                resultLog = $"已將使用者 {channel} {banType}{banDurationStr}";

                        CustomFunction.WriteLog(
                            control: TBLog,
                            message: resultLog);
                    }
                }
                catch (Exception ex)
                {
                    CustomFunction.WriteLog(
                        control: TBLog,
                        message: ex.Message);
                }
            }
        }
    }

    /// <summary>
    /// 更新 TBSuspiciousChannelIds
    /// </summary>
    private void UpdateTBSuspiciousChannelIds()
    {
        TBSuspiciousChannelIds.InvokeIfRequired(action: () =>
        {
            TBSuspiciousChannelIds.Text = string.Join(
                separator: Environment.NewLine,
                values: SuspiciousChannelIds);
        });
    }

    /// <summary>
    /// 設定控制項的狀態
    /// </summary>
    /// <param name="enable">布林值，啟用，預設值為 true</param>
    private void SetControlState(bool enable = true)
    {
        TBClientSecretFilePath.InvokeIfRequired(action: () =>
        {
            TBClientSecretFilePath.Enabled = enable;
        });

        bool useOAuth20 = false;

        CBUseOAuth20.InvokeIfRequired(action: () =>
        {
            CBUseOAuth20.Enabled = enable;

            useOAuth20 = CBUseOAuth20.Checked;
        });

        TBAPIKey.InvokeIfRequired(action: () =>
        {
            TBAPIKey.Enabled = !useOAuth20;
        });

        TBClientSecretFilePath.InvokeIfRequired(action: () =>
        {
            TBClientSecretFilePath.Enabled = enable && useOAuth20;
        });

        BtnSelectClientSecretFile.InvokeIfRequired(action: () =>
        {
            BtnSelectClientSecretFile.Enabled = enable && useOAuth20;
        });

        CBBanTemporary.InvokeIfRequired(action: () =>
        {
            CBBanTemporary.Enabled = useOAuth20;
        });

        CBBanPermanent.InvokeIfRequired(action: () =>
        {
            CBBanPermanent.Enabled = useOAuth20;
        });

        CBDeleteMessage.InvokeIfRequired(action: () =>
        {
            CBDeleteMessage.Enabled = useOAuth20;
        });

        NUPBanDuration.InvokeIfRequired(action: () =>
        {
            NUPBanDuration.Enabled = useOAuth20;
        });

        TBVideoID.InvokeIfRequired(action: () =>
        {
            TBVideoID.Enabled = enable;
        });

        BtnStart.InvokeIfRequired(action: () =>
        {
            BtnStart.Enabled = enable;
        });

        BtnStop.InvokeIfRequired(action: () =>
        {
            BtnStop.Enabled = !enable;
        });

        BtnClear.InvokeIfRequired(action: () =>
        {
            BtnClear.Enabled = enable;
        });
    }

    /// <summary>
    /// 清除變數
    /// </summary>
    private void ResetVariables()
    {
        SharedLiveChatID = string.Empty;
        SharedNextPageToken = string.Empty;
        SharedPollingIntervalMillis = 0.0;
        SharedYouTubeService = null;
        SharedCancellationToken = null;
    }

    /// <summary>
    /// 檢查應用程式的版本
    /// </summary>
    private async void CheckAppVersion()
    {
        using HttpClient httpClient = SharedHttpClientFactory.CreateClient();

        UpdateNotifier.CheckResult checkResult = await UpdateNotifier.CheckVersion(httpClient);

        if (!string.IsNullOrEmpty(checkResult.MessageText))
        {
            CustomFunction.WriteLog(TBLog, checkResult.MessageText);
        }

        if (checkResult.HasNewVersion &&
            !string.IsNullOrEmpty(checkResult.DownloadUrl))
        {
            DialogResult dialogResult = MessageBox.Show($"您是否要下載新版本 v{checkResult.VersionText}？",
                Text,
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question);

            if (dialogResult == DialogResult.OK)
            {
                CustomFunction.OpenBrowser(checkResult.DownloadUrl);
            }
        }

        if (checkResult.NetVersionIsOdler &&
            !string.IsNullOrEmpty(checkResult.DownloadUrl))
        {
            DialogResult dialogResult = MessageBox.Show($"您是否要下載舊版本 v{checkResult.VersionText}？",
                Text,
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question);

            if (dialogResult == DialogResult.OK)
            {
                CustomFunction.OpenBrowser(checkResult.DownloadUrl);
            }
        }
    }
}