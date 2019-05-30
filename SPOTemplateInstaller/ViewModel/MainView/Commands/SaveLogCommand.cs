using System;
using System.IO;
using System.Windows;
using Microsoft.Win32;

namespace SPOTemplateInstaller.ViewModel.MainView.Commands
{
    /// <summary>
    /// ログファイルに保存ボタン イベント
    /// </summary>
    public class SaveLogCommand : CommandBase<MainViewModel>
    {
        #region コンストラクタ

        /// <summary>
        /// ログファイルに保存ボタン イベント 初期化
        /// </summary>
        /// <param name="vm">親クラスインスタンス</param>
        public SaveLogCommand(MainViewModel vm)
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
            return (!_vm.IsSearching && !_vm.IsGetting && !_vm.IsApplying && !string.IsNullOrEmpty(_vm.Log));
        }

        /// <summary>
        /// イベント処理
        /// </summary>
        /// <param name="parameter">パラメータは送信されない</param>
        public override void Execute(object parameter)
        {
            // 保存先を選択
            var fileName = $"Log_{ DateTime.Now.ToString("yyyyMMddHHmmss") }.log";
            var dialog = new SaveFileDialog()
            {
                Title = "保存先の選択",
                FileName = fileName,
                OverwritePrompt = true
            };

            // 保存
            if (dialog.ShowDialog() == true)
            {
                File.WriteAllText(dialog.FileName, _vm.Log);
                MessageBox.Show("ログファイルに保存しました。");
            }
        }

        #endregion
    }
}
