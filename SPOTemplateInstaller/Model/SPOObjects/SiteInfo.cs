using System;
using System.Collections.Generic;

namespace SPOTemplateInstaller.Model.SPOObjects
{
    /// <summary>
    /// サイト情報
    /// </summary>
    public class SiteInfo
    {
        #region 列挙

        /// <summary>
        /// ロケールID
        /// </summary>
        public enum LocalIds
        {
            /// <summary>
            /// 日本語
            /// </summary>
            JA = 1041,

            /// <summary>
            /// 英語(U.S.)
            /// </summary>
            ENUS = 1033
        }

        /// <summary>
        /// タイムゾーンID
        /// </summary>
        public enum TimeZones
        {
            /// <summary>
            /// UTC + 09:00 : 東京, 大阪, 札幌
            /// </summary>
            TokyoOsakaSapporo = 20
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// タイトル
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// URL
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// テンプレート
        /// </summary>
        public string Template { get; set; }

        /// <summary>
        /// ロケールID(日本は1041)
        /// </summary>
        public uint Lcid { get; set; }

        /// <summary>
        /// タイムゾーンID(日本は20)
        /// </summary>
        public int TimeZoneId { get; set; }

        /// <summary>
        /// サイトコレクション管理者
        /// </summary>
        public string SiteOwnerLogin { get; set; }

        /// <summary>
        /// クォータ
        /// </summary>
        public long StorageMaximumLevel { get; set; }

        /// <summary>
        /// サブサイト
        /// </summary>
        public List<SiteInfo> subSites { get; set; }

        #endregion
    }
}
