using YTLiveChatFilter.Common;
using YTLiveChatFilter.Extensions;
using YTLiveChatFilter.Properties;

namespace YTLiveChatFilter;

public partial class MainForm : Form
{
    public MainForm(IHttpClientFactory httpClientFactory)
    {
        InitializeComponent();

        SharedHttpClientFactory = httpClientFactory;
    }

    private void MainForm_Load(object sender, EventArgs e)
    {
        CustomInit();
        CheckAppVersion();
    }

    private void BtnSelectClientSecretFile_Click(object sender, EventArgs e)
    {
        try
        {
            OpenFileDialog openFileDialog = new()
            {
                Title = "選擇 OAuth 用戶端檔案",
                Filter = "JSON|*.json",
                FilterIndex = 0,
                FileName = "client_secret.json"
            };

            DialogResult dialogResult = openFileDialog.ShowDialog();

            if (dialogResult == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;

                if (!string.IsNullOrEmpty(value: filePath) && File.Exists(path: filePath))
                {
                    TBClientSecretFilePath.InvokeIfRequired(action: () =>
                    {
                        TBClientSecretFilePath.Text = filePath;
                    });
                }
                else
                {
                    MessageBox.Show(
                        text: "請選擇實際存在的檔案。",
                        caption: Text,
                        buttons: MessageBoxButtons.OK,
                        icon: MessageBoxIcon.Error);
                }
            }
        }
        catch (Exception ex)
        {
            CustomFunction.WriteLog(
                control: TBLog,
                message: ex.Message);
        }
    }

    private void TBClientSecretFilePath_TextChanged(object sender, EventArgs e)
    {
        TBClientSecretFilePath.InvokeIfRequired(action: () =>
        {
            string value = TBClientSecretFilePath.Text;

            if (Settings.Default.ClientSecretPath != value)
            {
                Settings.Default.ClientSecretPath = value;
                Settings.Default.Save();

                CustomFunction.WriteLog(
                    control: TBLog,
                    message: "已更新 OAuth 用戶端檔案的路徑。");
            }
        });
    }

    private void CBUseOAuth_CheckedChanged(object sender, EventArgs e)
    {
        CBUseOAuth20.InvokeIfRequired(action: () =>
        {
            bool value = CBUseOAuth20.Checked;

            if (Settings.Default.UseOAuth20 != value)
            {
                Settings.Default.UseOAuth20 = value;
                Properties.Settings.Default.Save();

                string status = value ? "勾選" : "取消勾選";

                CustomFunction.WriteLog(
                    control: TBLog,
                    message: $"已{status}使用 OAuth 2.0 用戶端 ID。");
            }

            TBAPIKey.InvokeIfRequired(action: () =>
            {
                TBAPIKey.Enabled = !value;
            });

            TBClientSecretFilePath.InvokeIfRequired(action: () =>
            {
                TBClientSecretFilePath.Enabled = value;
            });

            BtnSelectClientSecretFile.InvokeIfRequired(action: () =>
            {
                BtnSelectClientSecretFile.Enabled = value;
            });

            CBBanTemporary.InvokeIfRequired(action: () =>
            {
                CBBanTemporary.Enabled = value;
            });

            CBBanPermanent.InvokeIfRequired(action: () =>
            {
                CBBanPermanent.Enabled = value;
            });

            CBDeleteMessage.InvokeIfRequired(action: () =>
            {
                CBDeleteMessage.Enabled = value;
            });

            NUPBanDuration.InvokeIfRequired(action: () =>
            {
                NUPBanDuration.Enabled = value;
            });

            if (!value)
            {
                string message = "若不勾選「使用 OAuth 2.0 用戶端 ID」選項，" +
                    "將無法使用「暫時停用使用者」、「隱藏使用者」以及「刪除訊息」等功能。";

                MessageBox.Show(
                    text: message,
                    caption: Text,
                    buttons: MessageBoxButtons.OK,
                    icon: MessageBoxIcon.Information);
            }
        });
    }

    private void TBAPIKey_TextChanged(object sender, EventArgs e)
    {
        TBAPIKey.InvokeIfRequired(action: () =>
        {
            string value = TBAPIKey.Text;

            if (Settings.Default.APIKey != value)
            {
                Settings.Default.APIKey = value;
                Settings.Default.Save();

                CustomFunction.WriteLog(
                    control: TBLog,
                    message: $"已更新 API 金鑰。");
            }
        });
    }

    private void CBBanTemporary_CheckedChanged(object sender, EventArgs e)
    {
        CBBanTemporary.InvokeIfRequired(action: () =>
        {
            bool value = CBBanTemporary.Checked;

            if (Settings.Default.BanTemporary != value)
            {
                Settings.Default.BanTemporary = value;
                Settings.Default.Save();

                string status = value ? "啟用" : "停用";

                CustomFunction.WriteLog(
                    control: TBLog,
                    message: $"已{status}暫時停用使用者。");
            }
        });
    }

    private void CBBanPermanent_CheckedChanged(object sender, EventArgs e)
    {
        CBBanPermanent.InvokeIfRequired(action: () =>
        {
            bool value = CBBanPermanent.Checked;

            if (Settings.Default.BanPermanent != value)
            {
                Settings.Default.BanPermanent = value;
                Settings.Default.Save();

                string status = value ? "啟用" : "停用";

                CustomFunction.WriteLog(
                    control: TBLog,
                    message: $"已{status}隱藏使用者。");
            }
        });
    }

    private void CBDeleteMessage_CheckedChanged(object sender, EventArgs e)
    {
        CBDeleteMessage.InvokeIfRequired(action: () =>
        {
            bool value = CBDeleteMessage.Checked;

            if (Settings.Default.DeleteMessage != value)
            {
                Settings.Default.DeleteMessage = value;
                Settings.Default.Save();

                string status = value ? "啟用" : "停用";

                CustomFunction.WriteLog(
                    control: TBLog,
                    message: $"已{status}刪除訊息。");
            }
        });
    }

    private void NUPBanDuration_ValueChanged(object sender, EventArgs e)
    {
        NUPBanDuration.InvokeIfRequired(action: () =>
        {
            int value = Convert.ToInt32(value: NUPBanDuration.Value);

            if (Settings.Default.BanDuration != value)
            {
                Settings.Default.BanDuration = value;
                Settings.Default.Save();

                CustomFunction.WriteLog(
                    control: TBLog,
                    message: $"已更新暫時停用秒數，更新後為暫時停用 {value} 秒。");
            }
        });
    }

    private async void BtnStart_Click(object? sender, EventArgs? e)
    {
        try
        {
            SetControlState(enable: false);

            SharedCancellationTokenSource = new();
            SharedCancellationToken = SharedCancellationTokenSource.Token;

            if (SharedCancellationToken.HasValue)
            {
                SharedCancellationToken.Value.ThrowIfCancellationRequested();
            }

            bool banTemporary = false,
                banPermanent = false,
                deleteMessage = false,
                useOAuth20 = false;

            int banDuration = 300;

            string apiKey = string.Empty,
                clientSecretFilePath = string.Empty,
                videoID = string.Empty;

            #region 取值

            CBUseOAuth20.InvokeIfRequired(action: () =>
            {
                useOAuth20 = CBUseOAuth20.Checked;
            });

            TBAPIKey.InvokeIfRequired(action: () =>
            {
                apiKey = TBAPIKey.Text;
            });

            TBClientSecretFilePath.InvokeIfRequired(action: () =>
            {
                clientSecretFilePath = TBClientSecretFilePath.Text;
            });

            TBVideoID.InvokeIfRequired(action: () =>
            {
                videoID = TBVideoID.Text;
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

            #endregion

            #region 檢查值

            if (useOAuth20)
            {
                if (string.IsNullOrEmpty(value: clientSecretFilePath))
                {
                    MessageBox.Show(
                        text: "請選擇 client_secret.json 檔案的路徑。",
                        caption: Text,
                        buttons: MessageBoxButtons.OK,
                        icon: MessageBoxIcon.Warning);

                    BtnStop_Click(sender: null, e: null);

                    return;
                }
                else
                {
                    if (!File.Exists(path: clientSecretFilePath))
                    {
                        MessageBox.Show(
                            text: "請確認選擇的 client_secret.json 檔案是否存在。",
                            caption: Text,
                            buttons: MessageBoxButtons.OK,
                            icon: MessageBoxIcon.Warning);

                        BtnStop_Click(sender: null, e: null);

                        return;
                    }
                }
            }
            else
            {
                if (string.IsNullOrEmpty(value: apiKey))
                {
                    MessageBox.Show(
                        text: "請輸入 API 金鑰。",
                        caption: Text,
                        buttons: MessageBoxButtons.OK,
                        icon: MessageBoxIcon.Warning);

                    BtnStop_Click(sender: null, e: null);

                    return;
                }
            }

            if (string.IsNullOrEmpty(value: videoID))
            {
                MessageBox.Show(
                    text: "請輸入影片 ID。",
                    caption: Text,
                    buttons: MessageBoxButtons.OK,
                    icon: MessageBoxIcon.Warning);

                BtnStop_Click(sender: null, e: null);

                return;
            }

            if (!useOAuth20)
            {
                CustomFunction.WriteLog(
                    control: TBLog,
                    message: "因為您使用「API 金鑰」，故只能蒐集可疑的頻道 ID。");
            }

            #endregion

            CustomFunction.WriteLog(
                control: TBLog,
                message: "開始作業。");

            await Task.Run(function: async () =>
            {
                await GetFirstLiveChatMessagesResp(
                    apiKey: apiKey,
                    clientSecretFilePath: clientSecretFilePath,
                    videoId: videoID,
                    deleteMessage: deleteMessage,
                    banTemporary: banTemporary,
                    banPermanent: banPermanent,
                    banDuration: banDuration,
                    useOAuth20: useOAuth20);

            }, cancellationToken: SharedCancellationToken.Value);
        }
        catch (Exception ex)
        {
            CustomFunction.WriteLog(
                control: TBLog,
                message: ex.Message);

            BtnStop_Click(sender: null, e: null);
        }
    }

    private void BtnStop_Click(object? sender, EventArgs? e)
    {
        try
        {
            if (!SharedCancellationTokenSource.IsCancellationRequested)
            {
                SharedCancellationTokenSource.Cancel();
            }

            SharedTimer.Stop();

            ResetVariables();

            CustomFunction.WriteLog(
                control: TBLog,
                message: "已結束作業。");
        }
        catch (Exception ex)
        {
            CustomFunction.WriteLog(
                control: TBLog,
                message: ex.Message);
        }
        finally
        {
            SetControlState(enable: true);
        }
    }

    private void BtnClear_Click(object sender, EventArgs e)
    {
        try
        {
            TBVideoID.InvokeIfRequired(action: () =>
            {
                TBVideoID.Clear();
            });

            TBSuspiciousChannelIds.InvokeIfRequired(action: () =>
            {
                SuspiciousChannelIds.Clear();

                TBSuspiciousChannelIds.Clear();

                SuspiciousChannels.Clear();
            });

            TBLog.InvokeIfRequired(action: () =>
            {
                TBLog.Clear();
            });
        }
        catch (Exception ex)
        {
            CustomFunction.WriteLog(
                control: TBLog,
                message: ex.Message);
        }
    }

    private void TBBanWords_TextChanged(object sender, EventArgs e)
    {
        TBBanWords.InvokeIfRequired(action: () =>
        {
            string value = TBBanWords.Text;

            BanWords.Clear();

            CustomFunction.WriteLog(control: TBLog, message: "已清除封鎖的字詞。");

            if (!string.IsNullOrEmpty(value: value))
            {
                string[] banWords = value.Split(
                    separator: Environment.NewLine.ToCharArray(),
                    options: StringSplitOptions.RemoveEmptyEntries);

                BanWords.AddRange(collection: banWords);
            }

            if (Settings.Default.BanWords != value)
            {
                Settings.Default.BanWords = value;
                Settings.Default.Save();

                CustomFunction.WriteLog(
                    control: TBLog,
                    message: "已更新封鎖的字詞。");
            }
        });
    }
}