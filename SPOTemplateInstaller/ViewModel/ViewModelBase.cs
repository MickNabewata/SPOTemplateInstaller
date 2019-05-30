using System.Threading.Tasks;
using System.ComponentModel;

namespace SPOTemplateInstaller.ViewModel
{
    /// <summary>
    /// ビューモデル基底クラス
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged実装

        /// <summary>
        /// プロパティ変更イベント
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// プロパティ変更処理
        /// </summary>
        /// <param name="name"></param>
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion

        #region パブリックメソッド

        /// <summary>
        /// データの初期化
        /// </summary>
        public abstract Task InitializeDataAsync();

        #endregion
    }
}
