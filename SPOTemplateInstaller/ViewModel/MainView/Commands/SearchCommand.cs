using System;
using System.Linq;
using System.Windows;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using SPOTemplateInstaller.Util;

namespace SPOTemplateInstaller.ViewModel.MainView.Commands
{
    /// <summary>
    /// 検索ボタン イベント
    /// </summary>
    public class SearchCommand : CommandBase<MainViewModel>
    {
        #region コンストラクタ

        /// <summary>
        /// 検索ボタン イベント 初期化
        /// </summary>
        /// <param name="vm">親クラスインスタンス</param>
        public SearchCommand(MainViewModel vm)
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
            return (!_vm.IsSearching && !_vm.IsGetting && !_vm.IsApplying);
        }

        /// <summary>
        /// イベント処理
        /// </summary>
        /// <param name="parameter">パラメータは送信されない</param>
        public override void Execute(object parameter)
        {
            // 検索開始
            _vm.SelectedSite = null;
            _vm.Sites = new ObservableCollection<Site>();
            _vm.IsSearching = true;
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
                // サイトコレクション取得
                var task = PnPUtility.GetSiteCollectionsAsync(_vm.Tenant, null);
                await task;

                if (task.Exception == null)
                {
                    // 成功したらViewModelへ追加
                    ObservableCollection<Site> sites = new ObservableCollection<Site>();
                    var result = task.Result;

                    // 絞り込み
                    if (!string.IsNullOrEmpty(_vm.SiteName))
                    {
                        result = (from s in result where s.Title.ToLower().Contains(_vm.SiteName.ToLower()) select s).ToList();
                    }

                    if (!string.IsNullOrEmpty(_vm.SiteURL))
                    {
                        result = (from s in result where s.Url.ToLower().Contains(_vm.SiteURL.ToLower()) select s).ToList();
                    }

                    // 追加
                    foreach (var siteCollection in result)
                    {
                        var site = new Site(_vm) { Title = siteCollection.Title, URL = siteCollection.Url };
                        await site.InitializeDataAsync().ConfigureAwait(false);
                        sites.Add(site);
                    }

                    // 検索終了
                    await d.Execute(() => {
                        _vm.Sites = sites;
                        _vm.IsSearching = false;
                    }, true).ConfigureAwait(false);
                }
                else
                {
                    // 検索終了
                    await d.Execute(() => { _vm.IsSearching = false; }, true).ConfigureAwait(false);
                    MessageBox.Show($"検索に失敗しました。\r\n{ task.Exception.InnerException?.Message?.ToString() }");
                }
            }
            catch (Exception ex)
            {
                // 検索終了
                await d.Execute(() => { _vm.IsSearching = false; }, true).ConfigureAwait(false);
                var message = (ex.InnerException != null) ? ex.InnerException.Message : ex.Message;
                MessageBox.Show($"検索に失敗しました。\r\n{ message }");
            }
        }

        #endregion
    }
}
