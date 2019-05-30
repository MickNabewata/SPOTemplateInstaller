using OfficeDevPnP.Core.Framework.Provisioning.Connectors;
using OfficeDevPnP.Core.Framework.Provisioning.Providers.Xml;

namespace SPOTemplateInstallLibrary.Model.ConnectionInfo.PnPTemplates
{
    /// <summary>
    /// テンプレート取得・保管先の接続情報インタフェース
    /// </summary>
    public interface IConnectionInfo
    {
        #region プロパティ

        /// <summary>
        /// テンプレートファイルのURI
        /// </summary>
        string Uri { get; }

        /// <summary>
        /// テンプレートファイルの一意キー(ファイル名など)
        /// </summary>
        string FileIdentifer { get; }

        #endregion

        #region メソッド

        /// <summary>
        /// テンプレートファイル保管場所へのコネクタ取得
        /// </summary>
        /// <param name="subFolderPath">サブフォルダパス(省略：ルートフォルダ)</param>
        /// <returns>コネクタ</returns>
        FileConnectorBase GetConnector(string subFolderPath = "");

        /// <summary>
        /// テンプレートファイル取得先へのコネクタ取得
        /// </summary>
        /// <param name="subFolderPath">サブフォルダパス(省略：ルートフォルダ)</param>
        /// <returns>コネクタ</returns>
        XMLTemplateProvider GetProvider(string subFolderPath = "");

        #endregion
    }
}
