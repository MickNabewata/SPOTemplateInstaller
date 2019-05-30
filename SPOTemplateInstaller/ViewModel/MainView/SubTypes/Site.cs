using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using SPOTemplateInstallLibrary;
using SPOTemplateInstallLibrary.Model.ConnectionInfo.SharePoint;

namespace SPOTemplateInstaller.ViewModel.MainView
{
    /// <summary>
    /// メインビュー用モデルクラス サイト情報
    /// </summary>
    public class Site : ViewModelBase
    {
        #region イベント
        

        #endregion

        #region コンストラクタ

        /// <summary>
        /// メインビュー用モデルクラス サイト情報 初期化
        /// </summary>
        public Site(MainViewModel vm)
        {
            // 値を保存
            VM = vm;
        }

        #endregion

        #region フィールド

        /// <summary>
        /// サブサイトが一度でも初期化されたか
        /// </summary>
        bool _isSubsitesInitialized = false;

        #endregion

        #region プロパティ

        /// <summary>
        /// メイン画面用ビューモデルクラス
        /// </summary>
        public MainViewModel VM { get; set; }

        /// <summary>
        /// ツリービューへの表示名
        /// </summary>
        public string DisplayTitle { get { var title = (string.IsNullOrEmpty(_title)) ? "(noTitle)" : _title; return $"{title} ({_url})"; } }

        /// <summary>
        /// タイトル
        /// </summary>
        public string Title { get { return _title; } set { _title = value; OnPropertyChanged("Title"); } }
        private string _title;

        /// <summary>
        /// URL
        /// </summary>
        public string URL { get { return _url; } set { _url = value; OnPropertyChanged("URL"); } }
        private string _url;
        
        /// <summary>
        /// サブサイト一覧
        /// </summary>
        public ObservableCollection<Site> SubSites { get { return _subSites; } set { _subSites = value; OnPropertyChanged("SubSites"); } }
        private ObservableCollection<Site> _subSites;
        
        /// <summary>
        /// ツリーが選択されているか否か
        /// </summary>
        public bool IsSelected { get { return _isSelected; } set { _isSelected = value; OnPropertyChanged("IsSelected"); if (_isSelected) TreeViewItemSelected(); } }
        private bool _isSelected;

        /// <summary>
        /// ツリーが展開されているか否か
        /// </summary>
        public bool IsExpanded { get { return _isExpanded; } set { _isExpanded = value; OnPropertyChanged("IsExpanded"); if (_isExpanded) TreeViewItemExpanded(); } }
        private bool _isExpanded;
        
        #endregion

        #region パブリックメソッド

        /// <summary>
        /// データの初期化
        /// </summary>
        public override async Task InitializeDataAsync()
        {
            // 展開状態とサブサイトを初期化する
            IsExpanded = false;
            SubSites = new ObservableCollection<Site>() {
                    new Site(VM){
                        Title = "データ取得中..."
                    }
                };
            _isSubsitesInitialized = false;
        }

        #endregion

        #region プライベートメソッド

        /// <summary>
        /// サブサイト情報を初期化
        /// </summary>
        private async Task InitializeSubSitesAsync()
        {
            // 他の構成ではSharePointから取得
            try
            {
                var infoList = await PnPUtility.GetSubSitesAsync(new SiteConnectionInfo(URL, VM?.Tenant?.UserAccount, VM?.Tenant?.UserPassword), false, null);
                var subSites = new ObservableCollection<Site>();
                foreach (var info in infoList)
                {
                    subSites.Add(new Site(VM)
                    {
                        Title = info.Title,
                        URL = info.Url
                    });
                }
                SubSites = subSites;
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// サブ-サブサイトのダミーデータを作成(子が無いと展開できないので)
        /// </summary>
        private void InitializeDammySubSubSites()
        {
            foreach (var site in SubSites)
            {
                site.SubSites = new ObservableCollection<Site>() {
                    new Site(VM){
                        Title = "データ取得中..."
                    }
                };
            }
        }

        /// <summary>
        /// ツリービューアイテムが開かれた時に実行するイベント
        /// </summary>
        private void TreeViewItemExpanded()
        {
            // 一度開かれている場合は処理しない
            if (_isSubsitesInitialized) return;

            _isSubsitesInitialized = true;

            Task.Run(async () => {
                // サブサイト情報を初期化
                await InitializeSubSitesAsync();

                // サブ-サブサイトのダミーデータを作成
                InitializeDammySubSubSites();
            });
        }

        /// <summary>
        /// ツリービューアイテムが選択された時に実行するイベント
        /// </summary>
        private void TreeViewItemSelected()
        {
            // このサイトを選択する
            VM.SelectedSite = this;
        }

        #endregion
    }
}
