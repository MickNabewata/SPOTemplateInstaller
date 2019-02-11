using System;

namespace SPOTemplateInstaller.Model.SPOObjects
{
    /// <summary>
    /// サイトデザイン情報
    /// </summary>
    public class SiteDesignInfo
    {
        #region 列挙

        #endregion

        #region プロパティ

        /// <summary>
        /// ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 既定のテンプレートか否か
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// 説明
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// プレビュー画像URL
        /// </summary>
        public string PreviewImageUrl { get; set; }

        /// <summary>
        /// プレビュー画像代替テキスト
        /// </summary>
        public string PreviewImageAltText { get; set; }

        /// <summary>
        /// サイトスクリプトID一覧
        /// </summary>
        public Guid[] SiteScriptIds { get; set; }

        /// <summary>
        /// タイトル
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// バージョン
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// サイトテンプレート
        /// </summary>
        public string WebTemplate { get; set; }

        #endregion
    }
}
