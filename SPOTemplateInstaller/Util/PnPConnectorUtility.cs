using OfficeDevPnP.Core.Framework.Provisioning.Connectors;

namespace SPOTemplateInstaller.Util
{
    /// <summary>
    /// PnPプロビジョニングエンジン コネクタ操作ユーティリティ
    /// </summary>
    public static class PnPConnectorUtility
    {
        #region パブリックメソッド
        
        /// <summary>
        /// ファイルシステムコネクタ
        /// </summary>
        /// <param name="folderPath">フォルダパス</param>
        /// <returns>コネクタ</returns>
        public static FileSystemConnector GetFileSystemConnector(string folderPath)
        {
            return new FileSystemConnector(folderPath, "");
        }

        #endregion
    }
}
