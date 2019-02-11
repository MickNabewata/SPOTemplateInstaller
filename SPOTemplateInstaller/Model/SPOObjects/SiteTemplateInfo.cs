namespace SPOTemplateInstaller.Model.SPOObjects
{
    /// <summary>
    /// サイトテンプレート情報
    /// </summary>
    public class SiteTemplateInfo
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

        #endregion

        #region プロパティ

        public int Id { get; set; }

        /// <summary>
        /// ロケールID(日本は1041)
        /// </summary>
        public uint Lcid { get; set; }

        /// <summary>
        /// タイトル
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 名前
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 説明
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// カテゴリ
        /// </summary>
        public string DisplayCategory { get; set; }
        
        /// <summary>
        /// 機能レベル
        /// </summary>
        public int CompatibilityLevel { get; set; }

        #endregion
    }
}
