using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPOTemplateInstaller.Util;
using SPOTemplateInstaller.Model.AppSettings;
using SPOTemplateInstaller.Model.ConnectionInfo.SharePoint;

namespace SPOTemplateInstaller.ViewModel.LoginView.Commands
{
    /// <summary>
    /// ログインボタン イベント
    /// </summary>
    public class LoginCommand : CommandBase<LoginViewModel>
    {
        #region コンストラクタ

        /// <summary>
        /// ログインボタン イベント 初期化
        /// </summary>
        /// <param name="vm">親クラスインスタンス</param>
        public LoginCommand(LoginViewModel vm)
        {
            _vm = vm;
        }

        #endregion

        #region イベント実装

        /// <summary>
        /// イベントを実行できる条件(falseが返るとコントロールが非活性になる)
        /// </summary>
        /// <param name="parameter">パラメータは送信されない</param>
        /// <returns>実行可否</returns>
        public override bool CanExecute(object parameter)
        {
            // ログイン情報をすべて入力すると実行可
            return ( !_vm.IsLoginTrying && !string.IsNullOrEmpty(_vm.Domain) && !string.IsNullOrEmpty(_vm.Account) && !string.IsNullOrEmpty(_vm.Password));
        }

        /// <summary>
        /// イベント処理
        /// </summary>
        /// <param name="parameter">パラメータは送信されない</param>
        public override void Execute(object parameter)
        {
            _vm.IsLoginTrying = true;
            DoLogin();
        }

        #endregion

        #region プライベートメソッド

        /// <summary>
        /// ログイン実行
        /// </summary>
        /// <returns></returns>
        private void DoLogin()
        {
            try
            {
                // テナント接続情報
                var tenantInfo = new TenantConnectionInfo(
                    $"https://{_vm.Domain}-admin.sharepoint.com",
                    _vm.Account,
                    _vm.Password);

                // サイトコレクション取得
                var task = PnPUtility.ConnectSpoAdminCenterAsync(tenantInfo);
                task.Wait();
           
                if (task.Exception == null)
                {
                    // アカウント保存
                    AppSettingUtil.Account = new AccountSetting()
                    {
                        Domain = _vm.Domain,
                        Account = _vm.Account,
                        Password = _vm.Password
                    };

                    // ログインボタン起動可能
                    _vm.IsLoginTrying = false;

                    // メイン画面起動
                    var view = new SPOTemplateInstaller.MainView(tenantInfo);
                    view.ShowDialog();
                }
                else
                {
                    // ログインボタン起動可能
                    _vm.IsLoginTrying = false;

                    MessageBox.Show($"ログインに失敗しました。\r\n{ task.Exception.InnerException?.Message?.ToString() }");
                }
            }
            catch (Exception ex)
            {
                // ログインボタン起動可能
                _vm.IsLoginTrying = false;

                var message = (ex.InnerException != null) ? ex.InnerException.Message : ex.Message;
                MessageBox.Show($"ログインに失敗しました。\r\n{ message }");
            }
        }

        #endregion
    }
}
