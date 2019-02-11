using System;
using System.IO;
using System.Windows;
using Microsoft.Win32;
using System.Threading.Tasks;
using SPOTemplateInstaller.Util;
using SPOTemplateInstaller.Model.ConnectionInfo.SharePoint;
using SPOTemplateInstaller.Model.ConnectionInfo.PnPTemplates;

namespace SPOTemplateInstaller.ViewModel.MainView.Commands
{
    /// <summary>
    /// テンプレート適用ボタン イベント
    /// </summary>
    public class ApplyTemplateCommand : CommandBase<MainViewModel>
    {
        #region コンストラクタ

        /// <summary>
        /// テンプレート適用ボタン イベント 初期化
        /// </summary>
        /// <param name="vm">親クラスインスタンス</param>
        public ApplyTemplateCommand(MainViewModel vm)
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
            return (!_vm.IsSearching && !_vm.IsGetting && !_vm.IsApplying && _vm.SelectedSite != null);
        }

        /// <summary>
        /// イベント処理
        /// </summary>
        /// <param name="parameter">パラメータは送信されない</param>
        public override void Execute(object parameter)
        {
            // テンプレート適用開始
            _vm.IsApplying = true;
            var d = new Dispatcher();
            Task.Run(async () => { await DoSearch(d); });
        }

        #endregion

        #region プライベートメソッド

        /// <summary>
        /// 検索実行
        /// </summary>
        private async Task DoSearch(Dispatcher d)
        {
            try
            {
                // テンプレートを選択
                var dialog = new OpenFileDialog()
                {
                    Title = "テンプレートファイルの選択",
                    Multiselect = false,
                    Filter = $"XML(*.xml)|*.xml"
                };

                // 取得を実行
                if (dialog.ShowDialog() == true)
                {
                    // ログタブに切り替え
                    await d.Execute(() => { _vm.LogTabSelected = true; }, true).ConfigureAwait(false);

                    // サイト情報
                    var site = new SiteConnectionInfo(_vm.SelectedSite.URL, _vm.Tenant.UserAccount, _vm.Tenant.UserPassword);

                    // テンプレート
                    var template = new FileSystemConnectionInfo(Path.GetDirectoryName(dialog.FileName), Path.GetFileName(dialog.FileName));

                    // ログ
                    var progress = new Progress<string>(WriteLog);

                    // テンプレート適用
                    var task = PnPUtility.ApplyProvisioningTemplateAsync(site, template, progress);
                    await task;

                    if (task.Exception == null)
                    {
                        // テンプレート取得終了
                        await d.Execute(() => {
                            _vm.IsApplying = false;
                        }, true).ConfigureAwait(false);
                    }
                    else
                    {
                        // テンプレート取得終了
                        await d.Execute(() => { _vm.IsApplying = false; }, true).ConfigureAwait(false);
                        MessageBox.Show($"テンプレート取得に失敗しました。\r\n{ task.Exception.InnerException?.Message?.ToString() }");
                    }
                }
                else
                {
                    // テンプレート取得終了
                    await d.Execute(() => { _vm.IsApplying = false; }, true).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                // テンプレート取得終了
                await d.Execute(() => { _vm.IsApplying = false; }, true).ConfigureAwait(false);
                var message = (ex.InnerException != null) ? ex.InnerException.Message : ex.Message;
                MessageBox.Show($"テンプレート取得に失敗しました。\r\n{ message }");
            }
        }

        /// <summary>
        /// ログ出力
        /// </summary>
        /// <param name="message">ログメッセージ</param>
        private void WriteLog(string message)
        {
            if(!string.IsNullOrEmpty(_vm.Log)) _vm.Log += Environment.NewLine;
            _vm.Log += message;
        }

        #endregion
    }
}
