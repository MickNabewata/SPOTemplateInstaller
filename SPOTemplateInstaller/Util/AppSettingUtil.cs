using SPOTemplateInstaller.Model.AppSettings;

namespace SPOTemplateInstaller.Util
{
    public static class AppSettingUtil
    {
        #region プロパティ

        /// <summary>
        /// アカウント設定
        /// </summary>
        public static AccountSetting Account {
            get {
                return Properties.Settings.Default.AccountSetting;
            }
            set
            {
                Properties.Settings.Default.AccountSetting = value;
                Properties.Settings.Default.Save();
            }
        }

        #endregion
    }
}
