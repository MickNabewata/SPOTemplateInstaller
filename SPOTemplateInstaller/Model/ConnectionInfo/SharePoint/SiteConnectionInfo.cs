using System.Security;

namespace SPOTemplateInstaller.Model.ConnectionInfo.SharePoint
{
    /// <summary>
    /// サイト接続情報
    /// </summary>
    public class SiteConnectionInfo
    {
        #region プロパティ

        /// <summary>
        /// SharePointサイトの絶対URL(例：https://contoso.sharepoint.com/sites/sample)
        /// </summary>
        public string WebAbsoluteUrl { get; set; }

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
        /// サイト接続情報 初期化
        /// </summary>
        /// <param name="webAbsoluteUrl">SharePointサイトの絶対URL(例：https://contoso.sharepoint.com/sites/sample)</param>
        /// <param name="userAccount">SharePointサイトに接続するためのユーザーアカウント</param>
        /// <param name="userPassword">SharePointサイトに接続するためのパスワード</param>
        public SiteConnectionInfo(string webAbsoluteUrl, string userAccount, string userPassword)
        {
            WebAbsoluteUrl = webAbsoluteUrl;
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
