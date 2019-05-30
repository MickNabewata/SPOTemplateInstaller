using OfficeDevPnP.Core.Framework.Provisioning.Connectors;
using OfficeDevPnP.Core.Framework.Provisioning.Providers.Xml;

namespace SPOTemplateInstallLibrary.Model.ConnectionInfo.PnPTemplates
{
    /// <summary>
    /// テンプレート取得・保管先のファイルシステム接続情報
    /// </summary>
    public class FileSystemConnectionInfo : IConnectionInfo
    {
        #region プロパティ

        /// <summary>
        /// フォルダパス 例) c:\work\sample
        /// </summary>
        public string Uri { get; }

        /// <summary>
        /// テンプレートファイル名
        /// </summary>
        public string FileIdentifer { get; }

        #endregion

        #region コンストラクタ

        /// <summary>
        /// テンプレート取得・保管先のファイルシステム接続情報 初期化
        /// </summary>
        /// <param name="folderPath">フォルダパス 例) c:\work\sample</param>
        /// <param name="fileName">テンプレートファイル名 例) template.xml</param>
        public FileSystemConnectionInfo(string folderPath, string fileName)
        {
            Uri = folderPath;
            FileIdentifer = fileName;
        }

        #endregion

        #region パブリックメソッド

        /// <summary>
        /// テンプレートファイル保管場所へのコネクタ取得
        /// </summary>
        /// <param name="subFolderPath">サブフォルダパス(省略：ルートフォルダ)</param>
        /// <returns>コネクタ</returns>
        public FileConnectorBase GetConnector(string subFolderPath = "")
        {
            return new FileSystemConnector(Uri, subFolderPath);
        }

        /// <summary>
        /// テンプレートファイル取得先へのコネクタ取得
        /// </summary>
        /// <param name="subFolderPath">サブフォルダパス(省略：ルートフォルダ)</param>
        /// <returns>コネクタ</returns>
        public XMLTemplateProvider GetProvider(string subFolderPath = "")
        {
            return new XMLFileSystemTemplateProvider(Uri, subFolderPath);
        }

        #endregion
    }
}
