using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using SPOTemplateInstaller.Model.SPOObjects;
using SPOTemplateInstaller.ViewModel.MainView;
using SPOTemplateInstaller.Model.ConnectionInfo.SharePoint;

namespace SPOTemplateInstaller
{
    /// <summary>
    /// MainView.xaml の相互作用ロジック
    /// </summary>
    public partial class MainView : Window
    {
        #region コンストラクタ

        /// <summary>
        /// ログイン画面 初期化
        /// </summary>
        public MainView(TenantConnectionInfo tenant)
        {
            InitializeComponent();

            // データバインド
            var vm = new MainViewModel(tenant, () => { this.Focus(); });
            Task.Run(async () => { await vm.InitializeDataAsync(); });
            this.DataContext = vm;
        }

        #endregion
    }
}
