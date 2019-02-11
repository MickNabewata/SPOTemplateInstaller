using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SPOTemplateInstaller.Model.SPOObjects;
using SPOTemplateInstaller.ViewModel;
using SPOTemplateInstaller.Model.ConnectionInfo.SharePoint;
using SPOTemplateInstaller.ViewModel.MainView.Commands;

namespace SPOTemplateInstaller.ViewModel.MainView
{
    /// <summary>
    /// アカウント設定ビュー用モデルクラス
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        #region コマンド

        /// <summary>
        /// 検索
        /// </summary>
        public ICommand SearchCommand { get; set; }

        /// <summary>
        /// テンプレート取得
        /// </summary>
        public ICommand GetTemplateCommand { get; set; }

        /// <summary>
        /// テンプレート適用
        /// </summary>
        public ICommand ApplyTemplateCommand { get; set; }

        /// <summary>
        /// ログファイルに保存
        /// </summary>
        public ICommand SaveLogCommand { get; set; }

        /// <summary>
        /// ログクリア
        /// </summary>
        public ICommand ClearLogCommand { get; set; }

        #endregion

        #region コンストラクタ

        /// <summary>
        /// アカウント設定ビュー用モデルクラス 初期化
        /// </summary>
        /// <param name="tenant">テナント接続情報</param>
        public MainViewModel(TenantConnectionInfo tenant, Action action)
        {
            // 引数を保管
            Tenant = tenant;
            _action = action;

            // イベント
            SearchCommand = new SearchCommand(this);
            GetTemplateCommand = new GetTemplateCommand(this);
            ApplyTemplateCommand = new ApplyTemplateCommand(this);
            SaveLogCommand = new SaveLogCommand(this);
            ClearLogCommand = new ClearLogCommand(this);
        }

        #endregion

        #region プロパティ

        public Action _action;

        /// <summary>
        /// テナント接続情報
        /// </summary>
        public TenantConnectionInfo Tenant { get; }

        /// <summary>
        /// サイト名(検索条件)
        /// </summary>
        public string SiteName { get { return _siteName; } set { _siteName = value; OnPropertyChanged("SiteName"); } }
        private string _siteName;

        /// <summary>
        /// サイトURL(検索条件)
        /// </summary>
        public string SiteURL { get { return _siteURL; } set { _siteURL = value; OnPropertyChanged("SiteURL"); } }
        private string _siteURL;

        /// <summary>
        /// サイト一覧
        /// </summary>
        public ObservableCollection<Site> Sites { get { return _sites; } set { _sites = value; OnPropertyChanged("Sites"); } }
        private ObservableCollection<Site> _sites = new ObservableCollection<Site>();

        /// <summary>
        /// 選択中のサイト
        /// </summary>
        public Site SelectedSite { get { return _selectedSite; } set { _selectedSite = value; OnPropertyChanged("SelectedSite"); } }
        private Site _selectedSite;

        /// <summary>
        /// 検索を実行中か否か
        /// </summary>
        public bool IsSearching { get { return _isSearching; } set { _isSearching = value; OnPropertyChanged("IsSearching"); } }
        private bool _isSearching;

        /// <summary>
        /// 取得を実行中か否か
        /// </summary>
        public bool IsGetting { get { return _isGetting; } set { _isGetting = value; OnPropertyChanged("IsGetting"); } }
        private bool _isGetting;

        /// <summary>
        /// 適用を実行中か否か
        /// </summary>
        public bool IsApplying { get { return _isApplying; } set { _isApplying = value; OnPropertyChanged("IsApplying"); } }
        private bool _isApplying;

        /// <summary>
        /// 「ログ」タブが選択されているか否か
        /// </summary>
        public bool LogTabSelected { get { return _logTabSelected; } set { _logTabSelected = value; OnPropertyChanged("LogTabSelected"); } }
        private bool _logTabSelected;

        /// <summary>
        /// ログ
        /// </summary>
        public string Log { get { return _log; } set { _log = value; OnPropertyChanged("Log"); } }
        private string _log;

        #endregion

        #region パブリックメソッド

        /// <summary>
        /// データの初期化
        /// </summary>
        public override async Task InitializeDataAsync()
        {
            _siteName = "";
            _siteURL = "";
            _isSearching = false;
            _isGetting = false;
            _isApplying = false;
            _logTabSelected = false;
        }

        #endregion

        #region プライベートメソッド
        
        #endregion
    }
}
