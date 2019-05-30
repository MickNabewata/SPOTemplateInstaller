using System;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace SPOTemplateInstaller.ViewModel
{
    /// <summary>
    /// 別スレッドからUIスレッドへの変更を許可するためのヘルパクラス
    /// </summary>
    public class Dispatcher : DispatcherObject
    {
        /// <summary>
        /// 処理実行
        /// </summary>
        /// <param name="action">処理</param>
        /// <param name="requeryCanExequte">強制的にCanExecuteを再計算するか否か</param>
        public async Task Execute(Action action, bool requeryCanExequte = false)
        {
            await this.Dispatcher.InvokeAsync(() => {
                action.Invoke();
                if (requeryCanExequte == true) CommandManager.InvalidateRequerySuggested();
            });
        }
    }
}
