namespace SPOTemplateInstaller.Model.SPOObjects
{
    /// <summary>
    /// サイトコレクション情報
    /// </summary>
    public class SiteCollectionInfo
    {
        #region 列挙

        /// <summary>
        /// 機能レベル
        /// </summary>
        public enum CompatibilityLevels
        {
            /// <summary>
            /// SharePoint 2013 (2018/10/12時点最新)
            /// </summary>
            SPS2013 = 15
        }

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

        /// <summary>
        /// サイトコレクションのステータス
        /// </summary>
        public enum Status
        {
            /// <summary>
            /// 存在しない
            /// </summary>
            NotExists,

            /// <summary>
            /// 稼働中
            /// </summary>
            Active,

            /// <summary>
            /// 作成中
            /// </summary>
            Creating,

            /// <summary>
            /// 削除済である
            /// </summary>
            Recycled
        }

        /// <summary>
        /// コミュニケーションサイトデザイン
        /// </summary>
        public enum CommunicationSiteDesigns
        {
            /// <summary>
            /// トピック
            /// </summary>
            Topic = 0,

            /// <summary>
            /// ショーケース
            /// </summary>
            ShowCase = 1,

            /// <summary>
            /// 空白
            /// </summary>
            Blank = 2            
        }

        #endregion

        #region プロパティ

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
        /// クォータ(MB)
        /// </summary>
        public long StorageMaximumLevel { get; set; }

        /// <summary>
        /// 警告クォータ
        /// </summary>
        public long StorageWarningLevel { get; set; }

        /// <summary>
        /// ストレージ利用率
        /// </summary>
        public long StorageUsage { get; set; }

        /// <summary>
        /// 機能レベル
        /// </summary>
        public int CompatibilityLevel { get; set; }

        /// <summary>
        /// 現在の状態(稼働中, 作成中, 削除済, 存在しない)
        /// </summary>
        public Status CurrentStatus { get; set; }

        #endregion
    }
}
