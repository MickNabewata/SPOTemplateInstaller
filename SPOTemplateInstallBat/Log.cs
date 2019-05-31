using System;

namespace SPOTemplateInstallBat
{
    class Log
    {
        #region パブリックメソッド

        /// <summary>
        /// 例外をコンソールに出力
        /// </summary>
        /// <param name="methodName">現在のメソッド名</param>
        /// <param name="ex">例外</param>
        public static void Write(string methodName, Exception ex)
        {
            string log = $"{ DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") }\t{methodName}\t例外が発生しました。{ex?.GetType().ToString()}\t{ex?.Message}\r\n{ex?.StackTrace}";
            Console.WriteLine(log);
        }

        /// <summary>
        /// メッセージをコンソールに出力
        /// </summary>
        /// <param name="methodName">現在のメソッド名</param>
        /// <param name="message">メッセージ</param>
        public static void Write(string methodName, string message)
        {
            string log = $"{ DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") }\t{methodName}\t{message}";
            Console.WriteLine(log);
        }

        /// <summary>
        /// メッセージ一覧をコンソールに出力
        /// </summary>
        /// <param name="methodName">現在のメソッド名</param>
        /// <param name="messages">メッセージ一覧</param>
        public static void Write(string methodName, string[] messages)
        {
            if(messages != null)
            {
                foreach(string msg in messages)
                {
                    Write(methodName, msg);
                }
            }
        }

        /// <summary>
        /// メッセージをコンソールに出力(日付無し)
        /// </summary>
        /// <param name="message">メッセージ</param>
        public static void Write(string message)
        {
            string log = $"{message}";
            Console.WriteLine(log);
        }

        /// <summary>
        /// メッセージ一覧をコンソールに出力日付無し)
        /// </summary>
        /// <param name="messages">メッセージ一覧</param>
        public static void Write(string[] messages)
        {
            if (messages != null)
            {
                foreach (string msg in messages)
                {
                    Write(msg);
                }
            }
        }

        #endregion
    }
}
