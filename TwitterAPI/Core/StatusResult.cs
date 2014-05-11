using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwitterAPI
{
    public enum StatusResult
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success,

        /// <summary>
        /// URLが見つかりません
        /// </summary>
        FileNotFound,

        /// <summary>
        /// リクエストが不正です
        /// </summary>
        BadRequest,

        /// <summary>
        /// 認証されていません
        /// </summary>
        Unauthorized,

        NotAcceptable,

        /// <summary>
        /// レートリミットが上限に達しました
        /// </summary>
        RateLimited,

        /// <summary>
        /// サーバーがダウンしています
        /// </summary>
        ServerDown,

        /// <summary>
        /// タイムアウト
        /// </summary>
        RequestTimeout,

        /// <summary>
        /// 接続に失敗しました
        /// </summary>
        ConnectionFailure,

        /// <summary>
        /// 不明のエラー
        /// </summary>
        Unknown,

        ProxyAuthenticationRequired,

        /// <summary>
        /// Jsonからパースできませんでした
        /// </summary>
        ParseError
    }
}
