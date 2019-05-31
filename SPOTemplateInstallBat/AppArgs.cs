using System;
using System.IO;
using System.Collections.Generic;

namespace SPOTemplateInstallBat
{
    /// <summary>
    /// アプリケーション引数保持・チェッククラス
    /// </summary>
    class AppArgs
    {
        #region 列挙

        /// <summary>
        /// 実行モード
        /// </summary>
        public enum MODE
        {
            /// <summary>
            /// 取得
            /// </summary>
            GET,

            /// <summary>
            /// 適用
            /// </summary>
            APPLY
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// エラーチェック成否
        /// </summary>
        public bool Retrived { get { return _retrived; } }
        private bool _retrived;

        /// <summary>
        /// 実行モード
        /// </summary>
        public MODE Mode { get { return _mode;  } }
        private MODE _mode;

        /// <summary>
        /// 接続先SharePointサイトURL
        /// </summary>
        public string SiteUrl { get { return _siteUrl; } }
        private string _siteUrl;

        /// <summary>
        /// 接続用アカウント
        /// </summary>
        public string Account { get { return _account; } }
        private string _account;

        /// <summary>
        /// 接続用パスワード
        /// </summary>
        public string Password { get { return _password; } }
        private string _password;

        /// <summary>
        /// テンプレートファイルフォルダパス
        /// </summary>
        public string FolderPath { get { return _folderPath; } }
        private string _folderPath;

        /// <summary>
        /// テンプレートファイル名
        /// </summary>
        public string FileName { get { return _fileName; } }
        private string _fileName;

        /// <summary>
        /// ログメッセージ一覧
        /// </summary>
        public List<string> LogMsg { get { return _logMsg; } }
        private List<string> _logMsg;

        #endregion

        #region コンストラクタ

        /// <summary>
        ///アプリケーション引数保持・チェッククラス
        /// </summary>
        /// <param name="args"></param>
        public AppArgs(string[] args)
        {
            // 結果
            bool result = true;

            // プロパティ初期化
            InitProps();

            // 引数解析
            if (args != null)
            {
                if (args.Length > 0) if (!Enum.TryParse(args[0], out _mode)) { result = false; };
                if (args.Length > 1) _siteUrl = args[1];
                if (args.Length > 2) _account = args[2];
                if (args.Length > 3) _password = args[3];
                if (args.Length > 4) _folderPath = args[4];
                if (args.Length > 5) _fileName = args[5];
            }

            // 引数の数チェック
            if (args == null || args.Length != 6)
            {
                LogMsg.Add("引数が不正です。");
                result = false;
            }

            // 結果
            _retrived = result;

            // プロパティ値をログメッセージに出力
            PushPropsToLog();
        }

        #endregion

        #region プライベートメソッド

        /// <summary>
        /// プロパティ初期化
        /// </summary>
        private void InitProps()
        {
            _retrived = false;
            _mode = MODE.GET;
            _siteUrl = string.Empty;
            _account = string.Empty;
            _password = string.Empty;
            _folderPath = string.Empty;
            _fileName = string.Empty;
            _logMsg = new List<string>();
        }

        /// <summary>
        /// プロパティ値をログメッセージに出力
        /// </summary>
        private void PushPropsToLog()
        {
            if(_logMsg == null) _logMsg = new List<string>();
            _logMsg.Add("----------------");
            _logMsg.Add($"実行モード                        ：{_mode}");
            _logMsg.Add($"接続先SharePointサイトURL         ：{_siteUrl}");
            _logMsg.Add($"接続用アカウント                  ：{_account}");
            _logMsg.Add($"接続用パスワード                  ：{_password}");
            _logMsg.Add($"テンプレートファイルフォルダパス  ：{_folderPath}");
            _logMsg.Add($"テンプレートファイル名            ：{_fileName}");
            _logMsg.Add("----------------");
        }

        #endregion
    }
}
