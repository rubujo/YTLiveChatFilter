# YouTube 聊天室過濾器

## 一、簡述

簡易的 YouTube 聊天室過濾器，透過 [YouTube Live Streaming API](https://developers.google.com/youtube/v3/live/getting-started?hl=zh-tw) 來過濾`作者名稱`和`訊息內容`是否包含封鎖的字詞，並針對偵測到的項目，進行`暫時停用使用者`、`隱藏使用者`或是`刪除訊息`等行為。

## 二、注意事項

1. 由於使用了 [YouTube Live Streaming API](https://developers.google.com/youtube/v3/live/getting-started?hl=zh-tw)，而其為 [YouTube Data API](https://developers.google.com/youtube/v3?hl=zh-tw) 的一部分，因此會有配額計算跟費用需要支付。
   - [配額計算機](https://developers.google.com/youtube/v3/determine_quota_cost?hl=zh-tw)
2. 需要在 [Google Cloud 控制台](https://console.cloud.google.com) 建立`專案`，在 `API 和服務`的`程式庫`啟用 `YouTube Data API v3` 後，於`憑證`建立 `API 金鑰`或是 `OAuth 2.0 用戶端 ID`（需要設定 `OAuth 同意畫面`）後才可以使用。
3. 使用 `API 金鑰`的話會有部分功能受限制無法使用，建議使用 `OAuth 2.0 用戶端 ID`。