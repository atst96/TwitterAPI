using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwitterAPI
{
	public abstract class LanguageTools
	{
		public static string APIErrorCodeToJapanese(long code, string sub = null)
		{
			switch (code)
			{
				case 32: return "ユーザーを認証できませんでした";
				case 34: return "申し訳ありませんが、そのページは存在しません";
				case 64: return "あなたのアカウントは一時停止されており、この機能にアクセスすることが許可されていません";
				case 68: return "Twitter REST API v1 は使用できません。APIバージョンを1.1に移行してください。\nhttps://dev.twitter.com/docs/api/1.1/overview";
				case 88: return "APIのアクセス制限に達しました";
				case 89: return "無効であるか有効期限が切れたトークンです";
				case 92: return "SSLが必要です";
				case 130: return "容量オーバーです";
				case 131: return "内部エラーが発生しました";
				case 135: return "ユーザーを認証できませんでした";
				case 161: return "これ以上多くの人をフォローできません";
				case 179: return "申し訳ありませんが、ツイートを確認する権利がありません";
				case 185: return "ユーザーは、日のツイート投稿限度を超えています";
				case 187: return "ツイートが多重しています";
				case 193: return "アップロードされたメディアの容量が大きすぎます";
				case 215: return "認証データが誤っています";
				case 231: return "ユーザーがログインを確認する必要があります";
				case 251: return "このエンドポイントは廃止されました。";
				case 261: return "アプリケーションは、書き込み動作を実行できません。";
				default: return sub;
			}
		}
	}
}
