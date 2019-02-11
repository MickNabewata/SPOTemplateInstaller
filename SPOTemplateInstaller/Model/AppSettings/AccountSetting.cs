using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPOTemplateInstaller.Model.AppSettings    
{
    /// <summary>
    /// アプリケーション設定ファイル アカウント名設定
    /// </summary>
    public class AccountSetting
    {
        #region プロパティ

        /// <summary>
        /// ドメイン
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// アカウント
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// パスワード
        /// </summary>
        public string Password { get; set; }

        #endregion
    }
}
