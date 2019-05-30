namespace SPOTemplateInstaller.ViewModel.MainView.Commands
{
    /// <summary>
    /// ログクリアボタン イベント
    /// </summary>
    public class ClearLogCommand : CommandBase<MainViewModel>
    {
        #region コンストラクタ

        /// <summary>
        /// ログクリアボタン イベント 初期化
        /// </summary>
        /// <param name="vm">親クラスインスタンス</param>
        public ClearLogCommand(MainViewModel vm)
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
            _vm.Log = string.Empty;
        }

        #endregion
    }
}
