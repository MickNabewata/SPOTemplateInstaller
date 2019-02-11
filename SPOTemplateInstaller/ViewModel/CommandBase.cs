using System;
using System.Windows;
using System.Windows.Input;

namespace SPOTemplateInstaller.ViewModel
{
    /// <summary>
    /// コマンド基底クラス
    /// </summary>
    public abstract class CommandBase<T> : ICommand
    {
        #region フィールド

        /// <summary>
        /// 親クラスインスタンス
        /// </summary>
        protected T _vm;

        #endregion

        #region イベント実装

        /// <summary>
        /// イベントを実行できる条件(falseが返るとコントロールが非活性になる)
        /// </summary>
        /// <param name="parameter">パラメータは送信されない</param>
        /// <returns>実行可否</returns>
        public abstract bool CanExecute(object parameter);

        /// <summary>
        /// イベント処理
        /// </summary>
        /// <param name="parameter">パラメータは送信されない</param>
        public abstract void Execute(object parameter);

        /// <summary>
        /// 実行可能条件の変更イベント
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        #endregion
    }
}
