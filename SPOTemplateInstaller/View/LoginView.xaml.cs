using System.Threading.Tasks;
using System.Windows;
using SPOTemplateInstaller.ViewModel.LoginView;

namespace SPOTemplateInstaller
{
    /// <summary>
    /// LoginView.xaml の相互作用ロジック
    /// </summary>
    public partial class LoginView : Window
    {
        #region コンストラクタ

        /// <summary>
        /// ログイン画面 初期化
        /// </summary>
        public LoginView()
        {
            InitializeComponent();

            // データバインド
            var vm = new LoginViewModel();
            var task = Task.Run(async () => { await vm.InitializeDataAsync(); });
            task.ConfigureAwait(false);
            task.Wait();
            this.DataContext = vm;
        }

        #endregion
    }
}
