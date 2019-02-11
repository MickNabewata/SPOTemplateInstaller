using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SPOTemplateInstaller.Util;
using SPOTemplateInstaller.ViewModel.LoginView.Commands;
using SPOTemplateInstaller.Model.ConnectionInfo.SharePoint;

namespace SPOTemplateInstaller.ViewModel.LoginView
{
    /// <summary>
    /// アカウント設定ビュー用モデルクラス
    /// </summary>
    public class LoginViewModel : ViewModelBase
    {
        #region コマンド

        /// <summary>
        /// ログイン
        /// </summary>
        public ICommand LoginCommand { get; set; }

        #endregion

        #region コンストラクタ

        /// <summary>
        /// アカウント設定ビュー用モデルクラス 初期化
        /// </summary>
        public LoginViewModel()
        {
            // イベント
            LoginCommand = new LoginCommand(this);
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// ドメイン
        /// </summary>
        public string Domain { get { return _domain; } set { _domain = value; OnPropertyChanged("Domain"); } }
        private string _domain;

        /// <summary>
        /// アカウント
        /// </summary>
        public string Account { get { return _account; } set { _account = value; OnPropertyChanged("Account"); } }
        private string _account;

        /// <summary>
        /// パスワード
        /// </summary>
        public string Password { get { return _password; } set { _password = value; OnPropertyChanged("Password"); } }
        private string _password;

        /// <summary>
        /// ログイン試行中か否か
        /// </summary>
        public bool IsLoginTrying { get { return _isLoginTrying; } set { _isLoginTrying = value; OnPropertyChanged("IsLoginTrying"); } }
        private bool _isLoginTrying;

        #endregion

        #region パブリックメソッド

        /// <summary>
        /// データの初期化
        /// </summary>
        public override async Task InitializeDataAsync()
        {
            var setting = AppSettingUtil.Account;
            _domain = setting?.Domain;
            _account = setting?.Account;
            _password = setting?.Password;
            _isLoginTrying = false;
        }

        #endregion

        #region プライベートメソッド

        #endregion
    }
}
