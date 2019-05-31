using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using SPOTemplateInstallLibrary;
using SPOTemplateInstallLibrary.Model.ConnectionInfo.SharePoint;
using SPOTemplateInstallLibrary.Model.ConnectionInfo.PnPTemplates;

namespace SPOTemplateInstallBat
{
    /// <summary>
    /// SharePoint PnPバッチ スタートアップクラス
    /// </summary>
    class Program
    {
        /// <summary>
        /// エンドポイント
        /// </summary>
        /// <param name="args">引数</param>
        static void Main(string[] args)
        {
            // ログ出力用メソッド名
            string methodName = MethodBase.GetCurrentMethod().Name;

            // 開始ログ
            Log.Write(methodName, "PnPバッチ開始");

            try
            {
                // 引数解析
                AppArgs appArgs = new AppArgs(args);
                Log.Write(methodName, appArgs.LogMsg.ToArray());

                // 処理
                if (appArgs.Retrived)
                {
                    // サイト情報
                    var site = new SiteConnectionInfo(appArgs.SiteUrl, appArgs.Account, appArgs.Password);

                    // 保存先
                    var template = new FileSystemConnectionInfo(appArgs.FolderPath, appArgs.FileName);

                    // 進捗報告
                    var progress = new Progress<string>((log) => { Log.Write(methodName, log); });

                    // 実行
                    var task = (appArgs.Mode == AppArgs.MODE.GET)? PnPUtility.SaveSiteAsProvisioningTemplateAsync(site, template, progress) : PnPUtility.ApplyProvisioningTemplateAsync(site, template, progress);
                    task.ConfigureAwait(false);
                    task.Wait();
                }
            }
            catch(Exception ex)
            {
                var message = (ex.InnerException != null) ? ex.InnerException : ex;
                Log.Write(methodName, message);
            }

            // 終了ログ
            Log.Write(methodName, "PnPバッチ終了");
        }
    }
}
