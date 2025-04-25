using Ganss.Text;
using Google.Apis.YouTube.v3.Data;
using System.Diagnostics;
using System.Runtime.InteropServices;
using YTLiveChatFilter.Extensions;

namespace YTLiveChatFilter.Common;

/// <summary>
/// 自定義函式
/// </summary>
public class CustomFunction
{
    /// <summary>
    /// 記錄
    /// </summary>
    /// <param name="control">TextBox</param>
    /// <param name="message">字串，訊息</param>
    public static void WriteLog(TextBox control, string message)
    {
        control.InvokeIfRequired(action: () =>
        {
            string log = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] " +
                $"{message}{Environment.NewLine}";

            control.AppendText(text: log);
        });
    }

    /// <summary>
    /// 檢查封鎖的字詞
    /// </summary>
    /// <param name="control">TextBox</param>
    /// <param name="ahoCorasick">AhoCorasick</param>
    /// <param name="liveChatMessage">LiveChatMessage</param>
    /// <param name="suspiciousChannelIds">List&lt;string&gt;，可疑的頻道 ID</param>
    /// <param name="suspiciousChannels">Dictionary&lt;string, string&gt;，可疑的頻道</param>
    /// <param name="action">MethodInvoker</param>
    /// <returns>布林值</returns>
    public static bool CheckBanWords(
        TextBox control,
        AhoCorasick ahoCorasick,
        LiveChatMessage liveChatMessage,
        ref List<string> suspiciousChannelIds,
        ref Dictionary<string, string> suspiciousChannels,
        MethodInvoker action)
    {
        bool isDetected = false;

        string channelId = liveChatMessage.AuthorDetails.ChannelId,
            authorName = liveChatMessage.AuthorDetails.DisplayName,
            messageId = liveChatMessage.Id,
            message = liveChatMessage.Snippet.DisplayMessage,
            logContent = string.Format(
                format: Environment.NewLine +
                    $"---{Environment.NewLine}" +
                    $"頻道網址：{{0}}{Environment.NewLine}" +
                    $"頻道 ID：{{1}}{Environment.NewLine}" +
                    $"使用者：{{2}}{Environment.NewLine}" +
                    $"訊息 ID：{{3}}{Environment.NewLine}" +
                    $"訊息內容：{{4}}{Environment.NewLine}" +
                    "---",
                args:
                    [
                        $"https://www.youtube.com/channel/{channelId}",
                        channelId,
                        authorName,
                        messageId,
                        message
                    ]);

        // 檢查 "authorName" 是否含有封鎖的字詞。
        List<WordMatch> listWordMatch1 = [.. ahoCorasick.Search(text: authorName)];

        // 檢查 "message" 是否含有封鎖的字詞。
        List<WordMatch> listWordMatch2 = [.. ahoCorasick.Search(text: message)];

        if (listWordMatch1.Count > 0 ||
            listWordMatch2.Count > 0)
        {
            isDetected = true;

            if (!suspiciousChannels.Any(predicate: n => n.Key == channelId))
            {
                suspiciousChannels.Add(key: channelId, value: authorName);
            }

            if (!suspiciousChannelIds.Any(predicate: n => n == channelId))
            {
                suspiciousChannelIds.Add(item: channelId);

                action();

                WriteLog(
                    control: control,
                    message: $"頻道「{authorName}（{channelId}）」，已加入至可疑的頻道 ID 清單。");
            }

            WriteLog(control: control, message: logContent);
        }

        return isDetected;
    }

    /// <summary>
    /// 取得 ContextMenuStrip
    /// </summary>
    /// <param name="text">字串，應用程式的名稱</param>
    /// <param name="suspiciousChannels">Dictionary&lt;string, string&gt;，可疑的頻道</param>
    /// <param name="appIcon">Icon</param>
    /// <param name="TBSuspiciousChannelIds">TextBox</param>
    /// <param name="TBLog">TextBox</param>
    /// <returns>ContextMenuStrip</returns>
    public static ContextMenuStrip GetContextMenuStrip(
        string text,
        Dictionary<string, string> suspiciousChannels,
        Icon appIcon,
        TextBox TBSuspiciousChannelIds,
        TextBox TBLog)
    {
        ContextMenuStrip contextMenuStrip = new();

        ToolStripItem tsiCopySuspiciousChannelUrlToClipBoard = contextMenuStrip.Items
            .Add(text: "複製可疑的頻道網址至剪貼簿");

        ToolStripItem tsiViewSuspiciousChannelName = contextMenuStrip.Items
            .Add(text: "檢視可疑的頻道名稱");

        contextMenuStrip.ItemClicked += (object? sender, ToolStripItemClickedEventArgs e) =>
        {
            try
            {
                if (e.ClickedItem == tsiCopySuspiciousChannelUrlToClipBoard)
                {
                    TBSuspiciousChannelIds.InvokeIfRequired(action: () =>
                    {
                        string[] values = TBSuspiciousChannelIds.Text.Split(
                            separator: Environment.NewLine.ToCharArray(),
                            options: StringSplitOptions.RemoveEmptyEntries);

                        if (values.Length > 0)
                        {
                            string copiedContent = string.Empty;

                            foreach (string value in values)
                            {
                                copiedContent += $"https://www.youtube.com/channel/{value}{Environment.NewLine}";
                            }

                            Clipboard.SetText(text: copiedContent);

                            WriteLog(control: TBLog, message: "已將可疑的頻道網址複製至剪貼簿。");
                        }
                        else
                        {
                            MessageBox.Show(
                                text: "目前無可疑的頻道網址可供複製。",
                                caption: text,
                                buttons: MessageBoxButtons.OK,
                                icon: MessageBoxIcon.Warning);
                        }
                    });
                }
                else if (e.ClickedItem == tsiViewSuspiciousChannelName)
                {
                    if (suspiciousChannels.Count > 0)
                    {
                        Form PopupForm = new()
                        {
                            Text = "可疑的頻道名稱",
                            Width = 560,
                            Height = 440,
                            MaximizeBox = false,
                            FormBorderStyle = FormBorderStyle.FixedDialog,
                            Icon = appIcon
                        };

                        TextBox TBSuspiciousChannelName = new()
                        {
                            Location = new Point(x: 5, y: 5),
                            Width = 530,
                            Height = 390,
                            Multiline = true,
                            ScrollBars = ScrollBars.Both
                        };

                        string optputContent = string.Empty;

                        foreach (KeyValuePair<string, string> item in suspiciousChannels)
                        {
                            optputContent += $"{item.Value}（{item.Key}）{Environment.NewLine}";
                        }

                        TBSuspiciousChannelName.InvokeIfRequired(action: () =>
                        {
                            TBSuspiciousChannelName.Text = optputContent;
                        });

                        PopupForm.Controls.Add(value: TBSuspiciousChannelName);
                        PopupForm.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show(
                            text: "目前無可疑的頻道名稱可供檢視。",
                            caption: text,
                            buttons: MessageBoxButtons.OK,
                            icon: MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog(control: TBLog, message: ex.Message);
            }
        };

        return contextMenuStrip;
    }

    /// <summary>
    /// 開啟網頁瀏覽器
    /// <para>參考 1：https://github.com/dotnet/runtime/issues/17938#issuecomment-235502080 </para>
    /// <para>參考 2：https://github.com/dotnet/runtime/issues/17938#issuecomment-249383422 </para>
    /// </summary>
    /// <param name="url">字串，網址</param>
    public static void OpenBrowser(string url)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            // Process.Start(new ProcessStartInfo("cmd", $"/c start {url}"));
            url = url.Replace("&", "^&");

            Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            Process.Start("xdg-open", url);
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            Process.Start("open", url);
        }
        else
        {
            Debug.WriteLine("不支援的作業系統。");
        }
    }
}