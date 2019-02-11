using System.Security;

namespace SPOTemplateInstaller.Model.ConnectionInfo.SharePoint
{
    /// <summary>
    /// テナント接続情報
    /// </summary>
    public class TenantConnectionInfo
    {
        #region プロパティ

        /// <summary>
        /// SharePoint管理センターの絶対URL(例：https://contoso-admin.sharepoint.com)
        /// </summary>
        public string SPOAdminCenterUrl { get; set; }

        /// <summary>
        /// SharePointサイトに接続するためのユーザーアカウント
        /// </summary>
        public string UserAccount { get; set; }

        /// <summary>
        /// SharePointサイトに接続するためのパスワード
        /// </summary>
        public string UserPassword { get; set; }

        /// <summary>
        /// SharePointサイトに接続するためのパスワード(セキュリティストリング)
        /// </summary>
        public SecureString UserSecurePassword { get { return StringToSecureString(UserPassword); } }

        #endregion

        #region コンストラクタ

        /// <summary>
        /// テナント接続情報 初期化
        /// </summary>
        /// <param name="spoAdminCenterUrl">SharePoint管理センターの絶対URL(例：https://contoso-admin.sharepoint.com)</param>
        /// <param name="userAccount">SharePointサイトに接続するためのユーザーアカウント</param>
        /// <param name="userPassword">SharePointサイトに接続するためのパスワード</param>
        public TenantConnectionInfo(string spoAdminCenterUrl, string userAccount, string userPassword)
        {
            SPOAdminCenterUrl = spoAdminCenterUrl;
            UserAccount = userAccount;
            UserPassword = userPassword;
        }

        #endregion

        #region プライベートメソッド

        /// <summary>
        /// 文字列をセキュアストリングに変換
        /// </summary>
        /// <param name="str">文字列</param>
        /// <returns>セキュアストリング</returns>
        private SecureString StringToSecureString(string str)
        {
            SecureString ret = new SecureString();
            foreach (char c in str.ToCharArray()) ret.AppendChar(c);
            return ret;
        }

        #endregion
    }
}
