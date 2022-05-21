namespace YTLiveChatFilter
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.LAPIKey = new System.Windows.Forms.Label();
            this.TBAPIKey = new System.Windows.Forms.TextBox();
            this.LClientSecretPath = new System.Windows.Forms.Label();
            this.TBClientSecretFilePath = new System.Windows.Forms.TextBox();
            this.BtnSelectClientSecretFile = new System.Windows.Forms.Button();
            this.CBUseOAuth20 = new System.Windows.Forms.CheckBox();
            this.LLog = new System.Windows.Forms.Label();
            this.TBLog = new System.Windows.Forms.TextBox();
            this.BtnStart = new System.Windows.Forms.Button();
            this.BtnStop = new System.Windows.Forms.Button();
            this.LVideoID = new System.Windows.Forms.Label();
            this.TBVideoID = new System.Windows.Forms.TextBox();
            this.LDeniedWords = new System.Windows.Forms.Label();
            this.TBBanWords = new System.Windows.Forms.TextBox();
            this.LDeniedChannelIDs = new System.Windows.Forms.Label();
            this.TBSuspiciousChannelIds = new System.Windows.Forms.TextBox();
            this.LVersion = new System.Windows.Forms.Label();
            this.BtnClear = new System.Windows.Forms.Button();
            this.CBBanPermanent = new System.Windows.Forms.CheckBox();
            this.LBanDuration = new System.Windows.Forms.Label();
            this.NUPBanDuration = new System.Windows.Forms.NumericUpDown();
            this.CBDeleteMessage = new System.Windows.Forms.CheckBox();
            this.CBBanTemporary = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.NUPBanDuration)).BeginInit();
            this.SuspendLayout();
            // 
            // LAPIKey
            // 
            this.LAPIKey.AutoSize = true;
            this.LAPIKey.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.LAPIKey.Location = new System.Drawing.Point(9, 65);
            this.LAPIKey.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LAPIKey.Name = "LAPIKey";
            this.LAPIKey.Size = new System.Drawing.Size(55, 15);
            this.LAPIKey.TabIndex = 4;
            this.LAPIKey.Text = "API 金鑰";
            // 
            // TBAPIKey
            // 
            this.TBAPIKey.Location = new System.Drawing.Point(65, 60);
            this.TBAPIKey.Margin = new System.Windows.Forms.Padding(2);
            this.TBAPIKey.Name = "TBAPIKey";
            this.TBAPIKey.Size = new System.Drawing.Size(422, 23);
            this.TBAPIKey.TabIndex = 5;
            this.TBAPIKey.TextChanged += new System.EventHandler(this.TBAPIKey_TextChanged);
            // 
            // LClientSecretPath
            // 
            this.LClientSecretPath.AutoSize = true;
            this.LClientSecretPath.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.LClientSecretPath.Location = new System.Drawing.Point(9, 9);
            this.LClientSecretPath.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LClientSecretPath.Name = "LClientSecretPath";
            this.LClientSecretPath.Size = new System.Drawing.Size(146, 15);
            this.LClientSecretPath.TabIndex = 0;
            this.LClientSecretPath.Text = "OAuth 用戶端檔案的路徑";
            // 
            // TBClientSecretFilePath
            // 
            this.TBClientSecretFilePath.Location = new System.Drawing.Point(9, 32);
            this.TBClientSecretFilePath.Margin = new System.Windows.Forms.Padding(2);
            this.TBClientSecretFilePath.Name = "TBClientSecretFilePath";
            this.TBClientSecretFilePath.Size = new System.Drawing.Size(478, 23);
            this.TBClientSecretFilePath.TabIndex = 3;
            this.TBClientSecretFilePath.TextChanged += new System.EventHandler(this.TBClientSecretFilePath_TextChanged);
            // 
            // BtnSelectClientSecretFile
            // 
            this.BtnSelectClientSecretFile.Location = new System.Drawing.Point(338, 4);
            this.BtnSelectClientSecretFile.Margin = new System.Windows.Forms.Padding(2);
            this.BtnSelectClientSecretFile.Name = "BtnSelectClientSecretFile";
            this.BtnSelectClientSecretFile.Size = new System.Drawing.Size(149, 23);
            this.BtnSelectClientSecretFile.TabIndex = 1;
            this.BtnSelectClientSecretFile.Text = "選擇 OAuth 用戶端檔案";
            this.BtnSelectClientSecretFile.UseVisualStyleBackColor = true;
            this.BtnSelectClientSecretFile.Click += new System.EventHandler(this.BtnSelectClientSecretFile_Click);
            // 
            // CBUseOAuth20
            // 
            this.CBUseOAuth20.AutoSize = true;
            this.CBUseOAuth20.Location = new System.Drawing.Point(491, 34);
            this.CBUseOAuth20.Margin = new System.Windows.Forms.Padding(2);
            this.CBUseOAuth20.Name = "CBUseOAuth20";
            this.CBUseOAuth20.Size = new System.Drawing.Size(163, 19);
            this.CBUseOAuth20.TabIndex = 2;
            this.CBUseOAuth20.Text = "使用 OAuth 2.0 用戶端 ID";
            this.CBUseOAuth20.UseVisualStyleBackColor = true;
            this.CBUseOAuth20.CheckedChanged += new System.EventHandler(this.CBUseOAuth_CheckedChanged);
            // 
            // LLog
            // 
            this.LLog.AutoSize = true;
            this.LLog.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.LLog.Location = new System.Drawing.Point(9, 249);
            this.LLog.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LLog.Name = "LLog";
            this.LLog.Size = new System.Drawing.Size(31, 15);
            this.LLog.TabIndex = 19;
            this.LLog.Text = "紀錄";
            // 
            // TBLog
            // 
            this.TBLog.Location = new System.Drawing.Point(9, 267);
            this.TBLog.Margin = new System.Windows.Forms.Padding(2);
            this.TBLog.Multiline = true;
            this.TBLog.Name = "TBLog";
            this.TBLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.TBLog.Size = new System.Drawing.Size(761, 152);
            this.TBLog.TabIndex = 20;
            // 
            // BtnStart
            // 
            this.BtnStart.Location = new System.Drawing.Point(620, 5);
            this.BtnStart.Margin = new System.Windows.Forms.Padding(2);
            this.BtnStart.Name = "BtnStart";
            this.BtnStart.Size = new System.Drawing.Size(73, 23);
            this.BtnStart.TabIndex = 8;
            this.BtnStart.Text = "開始";
            this.BtnStart.UseVisualStyleBackColor = true;
            this.BtnStart.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // BtnStop
            // 
            this.BtnStop.Location = new System.Drawing.Point(697, 5);
            this.BtnStop.Margin = new System.Windows.Forms.Padding(2);
            this.BtnStop.Name = "BtnStop";
            this.BtnStop.Size = new System.Drawing.Size(73, 23);
            this.BtnStop.TabIndex = 9;
            this.BtnStop.Text = "停止";
            this.BtnStop.UseVisualStyleBackColor = true;
            this.BtnStop.Click += new System.EventHandler(this.BtnStop_Click);
            // 
            // LVideoID
            // 
            this.LVideoID.AutoSize = true;
            this.LVideoID.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.LVideoID.Location = new System.Drawing.Point(9, 88);
            this.LVideoID.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LVideoID.Name = "LVideoID";
            this.LVideoID.Size = new System.Drawing.Size(47, 15);
            this.LVideoID.TabIndex = 6;
            this.LVideoID.Text = "影片 ID";
            // 
            // TBVideoID
            // 
            this.TBVideoID.Location = new System.Drawing.Point(65, 86);
            this.TBVideoID.Margin = new System.Windows.Forms.Padding(2);
            this.TBVideoID.Name = "TBVideoID";
            this.TBVideoID.Size = new System.Drawing.Size(422, 23);
            this.TBVideoID.TabIndex = 7;
            // 
            // LDeniedWords
            // 
            this.LDeniedWords.AutoSize = true;
            this.LDeniedWords.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.LDeniedWords.Location = new System.Drawing.Point(9, 116);
            this.LDeniedWords.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LDeniedWords.Name = "LDeniedWords";
            this.LDeniedWords.Size = new System.Drawing.Size(293, 15);
            this.LDeniedWords.TabIndex = 15;
            this.LDeniedWords.Text = "封鎖的字詞（※一行一個字詞，英文字詞請使用小寫）";
            // 
            // TBBanWords
            // 
            this.TBBanWords.Location = new System.Drawing.Point(9, 133);
            this.TBBanWords.Margin = new System.Windows.Forms.Padding(2);
            this.TBBanWords.Multiline = true;
            this.TBBanWords.Name = "TBBanWords";
            this.TBBanWords.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.TBBanWords.Size = new System.Drawing.Size(478, 115);
            this.TBBanWords.TabIndex = 16;
            this.TBBanWords.TextChanged += new System.EventHandler(this.TBBanWords_TextChanged);
            // 
            // LDeniedChannelIDs
            // 
            this.LDeniedChannelIDs.AutoSize = true;
            this.LDeniedChannelIDs.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.LDeniedChannelIDs.Location = new System.Drawing.Point(491, 116);
            this.LDeniedChannelIDs.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LDeniedChannelIDs.Name = "LDeniedChannelIDs";
            this.LDeniedChannelIDs.Size = new System.Drawing.Size(110, 15);
            this.LDeniedChannelIDs.TabIndex = 17;
            this.LDeniedChannelIDs.Text = "可疑的頻道 ID 清單";
            // 
            // TBSuspiciousChannelIds
            // 
            this.TBSuspiciousChannelIds.Location = new System.Drawing.Point(491, 133);
            this.TBSuspiciousChannelIds.Margin = new System.Windows.Forms.Padding(2);
            this.TBSuspiciousChannelIds.Multiline = true;
            this.TBSuspiciousChannelIds.Name = "TBSuspiciousChannelIds";
            this.TBSuspiciousChannelIds.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.TBSuspiciousChannelIds.Size = new System.Drawing.Size(279, 115);
            this.TBSuspiciousChannelIds.TabIndex = 18;
            // 
            // LVersion
            // 
            this.LVersion.AutoSize = true;
            this.LVersion.Location = new System.Drawing.Point(9, 421);
            this.LVersion.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LVersion.Name = "LVersion";
            this.LVersion.Size = new System.Drawing.Size(43, 15);
            this.LVersion.TabIndex = 21;
            this.LVersion.Text = "版本：";
            // 
            // BtnClear
            // 
            this.BtnClear.Location = new System.Drawing.Point(697, 32);
            this.BtnClear.Margin = new System.Windows.Forms.Padding(2);
            this.BtnClear.Name = "BtnClear";
            this.BtnClear.Size = new System.Drawing.Size(73, 23);
            this.BtnClear.TabIndex = 10;
            this.BtnClear.Text = "清除";
            this.BtnClear.UseVisualStyleBackColor = true;
            this.BtnClear.Click += new System.EventHandler(this.BtnClear_Click);
            // 
            // CBBanPermanent
            // 
            this.CBBanPermanent.AutoSize = true;
            this.CBBanPermanent.Location = new System.Drawing.Point(607, 62);
            this.CBBanPermanent.Margin = new System.Windows.Forms.Padding(2);
            this.CBBanPermanent.Name = "CBBanPermanent";
            this.CBBanPermanent.Size = new System.Drawing.Size(86, 19);
            this.CBBanPermanent.TabIndex = 12;
            this.CBBanPermanent.Text = "隱藏使用者";
            this.CBBanPermanent.UseVisualStyleBackColor = true;
            this.CBBanPermanent.CheckedChanged += new System.EventHandler(this.CBBanPermanent_CheckedChanged);
            // 
            // LBanDuration
            // 
            this.LBanDuration.AutoSize = true;
            this.LBanDuration.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.LBanDuration.Location = new System.Drawing.Point(491, 88);
            this.LBanDuration.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LBanDuration.Name = "LBanDuration";
            this.LBanDuration.Size = new System.Drawing.Size(79, 15);
            this.LBanDuration.TabIndex = 14;
            this.LBanDuration.Text = "暫時停用秒數";
            // 
            // NUPBanDuration
            // 
            this.NUPBanDuration.Location = new System.Drawing.Point(574, 86);
            this.NUPBanDuration.Margin = new System.Windows.Forms.Padding(2);
            this.NUPBanDuration.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.NUPBanDuration.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NUPBanDuration.Name = "NUPBanDuration";
            this.NUPBanDuration.Size = new System.Drawing.Size(196, 23);
            this.NUPBanDuration.TabIndex = 15;
            this.NUPBanDuration.Value = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.NUPBanDuration.ValueChanged += new System.EventHandler(this.NUPBanDuration_ValueChanged);
            // 
            // CBDeleteMessage
            // 
            this.CBDeleteMessage.AutoSize = true;
            this.CBDeleteMessage.Location = new System.Drawing.Point(698, 62);
            this.CBDeleteMessage.Name = "CBDeleteMessage";
            this.CBDeleteMessage.Size = new System.Drawing.Size(74, 19);
            this.CBDeleteMessage.TabIndex = 13;
            this.CBDeleteMessage.Text = "刪除訊息";
            this.CBDeleteMessage.UseVisualStyleBackColor = true;
            this.CBDeleteMessage.CheckedChanged += new System.EventHandler(this.CBDeleteMessage_CheckedChanged);
            // 
            // CBBanTemporary
            // 
            this.CBBanTemporary.AutoSize = true;
            this.CBBanTemporary.Location = new System.Drawing.Point(492, 62);
            this.CBBanTemporary.Name = "CBBanTemporary";
            this.CBBanTemporary.Size = new System.Drawing.Size(110, 19);
            this.CBBanTemporary.TabIndex = 11;
            this.CBBanTemporary.Text = "暫時停用使用者";
            this.CBBanTemporary.UseVisualStyleBackColor = true;
            this.CBBanTemporary.CheckedChanged += new System.EventHandler(this.CBBanTemporary_CheckedChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 441);
            this.Controls.Add(this.CBBanTemporary);
            this.Controls.Add(this.CBDeleteMessage);
            this.Controls.Add(this.NUPBanDuration);
            this.Controls.Add(this.LBanDuration);
            this.Controls.Add(this.CBBanPermanent);
            this.Controls.Add(this.BtnClear);
            this.Controls.Add(this.LVersion);
            this.Controls.Add(this.TBSuspiciousChannelIds);
            this.Controls.Add(this.LDeniedChannelIDs);
            this.Controls.Add(this.TBBanWords);
            this.Controls.Add(this.LDeniedWords);
            this.Controls.Add(this.TBVideoID);
            this.Controls.Add(this.LVideoID);
            this.Controls.Add(this.BtnStop);
            this.Controls.Add(this.BtnStart);
            this.Controls.Add(this.TBLog);
            this.Controls.Add(this.LLog);
            this.Controls.Add(this.CBUseOAuth20);
            this.Controls.Add(this.BtnSelectClientSecretFile);
            this.Controls.Add(this.TBClientSecretFilePath);
            this.Controls.Add(this.LClientSecretPath);
            this.Controls.Add(this.TBAPIKey);
            this.Controls.Add(this.LAPIKey);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "YouTube 聊天室過濾器";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.NUPBanDuration)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label LAPIKey;
        private TextBox TBAPIKey;
        private Label LClientSecretPath;
        private TextBox TBClientSecretFilePath;
        private Button BtnSelectClientSecretFile;
        private CheckBox CBUseOAuth20;
        private Label LLog;
        private TextBox TBLog;
        private Button BtnStart;
        private Button BtnStop;
        private Label LVideoID;
        private TextBox TBVideoID;
        private Label LDeniedWords;
        private TextBox TBBanWords;
        private Label LDeniedChannelIDs;
        private TextBox TBSuspiciousChannelIds;
        private Label LVersion;
        private Button BtnClear;
        private CheckBox CBBanPermanent;
        private Label LBanDuration;
        private NumericUpDown NUPBanDuration;
        private CheckBox CBDeleteMessage;
        private CheckBox CBBanTemporary;
    }
}