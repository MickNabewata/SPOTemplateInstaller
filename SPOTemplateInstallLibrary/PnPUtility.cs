using Microsoft.SharePoint.Client;
using Microsoft.Online.SharePoint.TenantAdministration;
using OfficeDevPnP.Core.Framework.Provisioning.Model;
using OfficeDevPnP.Core.Framework.Provisioning.ObjectHandlers;
using OfficeDevPnP.Core.Entities;
using OfficeDevPnP.Core.Sites;
using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SPOTemplateInstallLibrary.Model.SPOObjects;
using SPOTemplateInstallLibrary.Model.ConnectionInfo.PnPTemplates;
using SPOTemplateInstallLibrary.Model.ConnectionInfo.SharePoint;

namespace SPOTemplateInstallLibrary
{
    /// <summary>
    /// PnPプロビジョニングエンジン操作ユーティリティ
    /// </summary>
    public static class PnPUtility
    {
        #region 固定値

        /// <summary>
        /// サブフォルダパス
        /// </summary>
        private const string persistFilesPath = "";

        #endregion

        #region プロパティ


        #endregion

        #region パブリックメソッド

        #region プロビジョニングテンプレート操作

        /// <summary>
        /// 既存サイトの定義をテンプレートとして取得
        /// </summary>
        /// <param name="siteConnectionInfo">SharePointサイト接続情報</param>
        /// <param name="templateSaveInfo">テンプレート保管場所情報</param>
        /// <param name="progress">進捗報告オブジェクト</param>
        /// <returns>サイト定義(XML)</returns>
        public static async Task<string> SaveSiteAsProvisioningTemplateAsync(SiteConnectionInfo siteConnectionInfo, IConnectionInfo templateSaveInfo, IProgress<string> progress = null)
        {
            // 戻り値
            string ret;

            progress?.Report("テンプレート取得開始");

            using (var ctx = new ClientContext(siteConnectionInfo.WebAbsoluteUrl))
            {
                #region SharePointサイトに接続

                progress?.Report($"1/3 SharePointサイトに接続");

                ctx.Credentials = new SharePointOnlineCredentials(siteConnectionInfo.UserAccount, siteConnectionInfo.UserSecurePassword);
                ctx.RequestTimeout = Timeout.Infinite;

                var web = ctx.Web;
                ctx.Load(web, w => w.Title);
                await ctx.ExecuteQueryRetryAsync().ConfigureAwait(false);

                #endregion

                #region テンプレートを取得

                // テンプレート取得オプション
                var creationInfo = new ProvisioningTemplateCreationInformation(ctx.Web)
                {
                    // 進捗報告
                    ProgressDelegate = delegate (String pnpMessage, Int32 pnpProgress, Int32 pnpTotal)
                    {
                        progress?.Report($"2/3 テンプレートを取得 - {pnpProgress}/{pnpTotal} {pnpMessage}");
                    },

                    // ロゴ等のファイル依存ファイル出力先
                    FileConnector = templateSaveInfo.GetConnector(persistFilesPath),

                    // ブランディング用ファイルを含める(テーマ/ロゴ/代替CSSなど)
                    PersistBrandingFiles = true,

                    // マスターページ/ページレイアウトを含む
                    IncludeNativePublishingFiles = true,

                    // 検索設定を含む
                    IncludeSearchConfiguration = true,

                    // 用語ストアを含む
                    IncludeSiteCollectionTermGroup = true,

                    // SharePointグループを含む
                    IncludeSiteGroups = true,

                    // 発行ファイル(マスターページ/ページレイアウトなど)を含む
                    PersistPublishingFiles = true
                };

                // テンプレート取得
                var template = ctx.Web.GetProvisioningTemplate(creationInfo);
                progress?.Report($"2/3 テンプレートを取得 - 完了");
                ret = template.ToXML();

                // テンプレート保存
                progress?.Report($"2/3 テンプレートを保存：{ templateSaveInfo.FileIdentifer }");
                templateSaveInfo.GetConnector().SaveFileStream(templateSaveInfo.FileIdentifer, new MemoryStream(Encoding.UTF8.GetBytes(ret)));

                #endregion
            }

            progress?.Report("テンプレート取得終了");

            // 返却
            return ret;
        }

        /// <summary>
        /// 保管済テンプレートを既存サイトに適用
        /// </summary>
        /// <param name="siteConnectionInfo">SharePointサイト接続情報</param>
        /// <param name="templateGetInfo">テンプレート保管場所情報</param>
        /// <param name="progress">進捗報告オブジェクト</param>
        /// <returns>無し</returns>
        public static async Task ApplyProvisioningTemplateAsync(SiteConnectionInfo siteConnectionInfo, IConnectionInfo templateGetInfo, IProgress<string> progress = null)
        {
            progress?.Report("テンプレート適用開始");

            using (var ctx = new ClientContext(siteConnectionInfo.WebAbsoluteUrl))
            {
                #region SharePointサイトに接続

                progress?.Report($"1/3 SharePointサイトに接続");

                ctx.Credentials = new SharePointOnlineCredentials(siteConnectionInfo.UserAccount, siteConnectionInfo.UserSecurePassword);
                ctx.RequestTimeout = Timeout.Infinite;

                var web = ctx.Web;
                ctx.Load(web, w => w.Title);
                await ctx.ExecuteQueryRetryAsync();

                #endregion

                #region テンプレートを取得

                ProvisioningTemplate template = GetProvisioningTemplate(templateGetInfo);
                progress?.Report($"2/3 テンプレートを取得：{ templateGetInfo.FileIdentifer }");

                #endregion

                #region テンプレートを適用

                // テンプレート適用オプション
                var applyingInfo = new ProvisioningTemplateApplyingInformation()
                {
                    // 進捗報告
                    ProgressDelegate = delegate (String pnpMessage, Int32 pnpProgress, Int32 pnpTotal)
                    {
                        progress?.Report($"3/3 テンプレートを適用 - {pnpProgress}/{pnpTotal} {pnpMessage}");
                    },

                    // 詳細な報告
                    MessagesDelegate = (string msg, ProvisioningMessageType type) =>
                    {
                        switch(type)
                        {
                            case ProvisioningMessageType.Warning:
                                progress?.Report($"[Warning] {msg}");
                                break;
                            case ProvisioningMessageType.Error:
                                progress?.Report($"[Error] {msg}");
                                throw new Exception(msg);
                            case ProvisioningMessageType.EasterEgg:
                                progress?.Report($"[EasterEgg] {msg}");
                                throw new Exception(msg);
                            case ProvisioningMessageType.Progress:
                                var msgs = msg?.Split('|');
                                if (msgs?.Length == 4)
                                {
                                    progress?.Report($"[Progress] {msgs[0]} {msgs[2]}/{msgs[3]} {msgs[1]}");
                                }
                                else
                                {
                                    progress?.Report($"[Progress] {msg}");
                                }
                                break;
                            case ProvisioningMessageType.Completed:
                                progress?.Report($"[Completed] {msg}");
                                break;
                            default:
                                break;
                        }
                    },

                    // 既存ナビゲーションをクリアする
                    ClearNavigation = true,

                    // テンプレート情報を維持する
                    PersistTemplateInfo = true
                };

                // テンプレート適用
                ctx.Web.ApplyProvisioningTemplate(
                    template,
                    applyingInfo);
                progress?.Report($"3/3 テンプレートを適用 - 完了");

                #endregion
            }

            progress?.Report("テンプレート適用終了");
        }

        /// <summary>
        /// 保管済テンプレートをXML形式の文字列として取得
        /// </summary>
        /// <param name="templateGetInfo">テンプレート保管場所情報</param>
        /// <returns>テンプレートXML</returns>
        public static string GetProvisioningTemplateByXml(IConnectionInfo templateGetInfo)
        {
            return GetProvisioningTemplate(templateGetInfo).ToXML();
        }

        #endregion

        #region サイトコレクション操作

        /// <summary>
        /// SharePoint管理センター接続
        /// </summary>
        /// <param name="tenantConnectionInfo">テナント接続情報</param>
        /// <param name="progress">進捗報告オブジェクト</param>
        public static async Task ConnectSpoAdminCenterAsync(TenantConnectionInfo tenantConnectionInfo, IProgress<string> progress = null)
        {
            List<SiteCollectionInfo> ret = new List<SiteCollectionInfo>();

            progress?.Report("テナント接続開始");

            using (var ctx = new ClientContext(tenantConnectionInfo.SPOAdminCenterUrl))
            {
                // 認証
                ctx.Credentials = new SharePointOnlineCredentials(tenantConnectionInfo.UserAccount, tenantConnectionInfo.UserSecurePassword);
                ctx.RequestTimeout = Timeout.Infinite;

                // テナント取得
                var tenant = new Tenant(ctx);
                ctx.Load(tenant);
                await ctx.ExecuteQueryRetryAsync().ConfigureAwait(false);
            }

            progress?.Report("テナント接続終了");
        }

        /// <summary>
        /// サイトコレクション一覧取得
        /// </summary>
        /// <param name="tenantConnectionInfo">テナント接続情報</param>
        /// <param name="msgProgress">進捗報告オブジェクト</param>
        /// <returns>サイトコレクション一覧</returns>
        public static async Task<List<SiteCollectionInfo>> GetSiteCollectionsAsync(TenantConnectionInfo tenantConnectionInfo, IProgress<string> msgProgress = null)
        {
            List<SiteCollectionInfo> ret = new List<SiteCollectionInfo>();

            msgProgress?.Report("テナント接続開始");

            using (var ctx = new ClientContext(tenantConnectionInfo.SPOAdminCenterUrl))
            {
                // 認証
                ctx.Credentials = new SharePointOnlineCredentials(tenantConnectionInfo.UserAccount, tenantConnectionInfo.UserSecurePassword);
                ctx.RequestTimeout = Timeout.Infinite;

                // テナント取得
                var tenant = new Tenant(ctx);
                ctx.Load(tenant);

                // サイトコレクション取得
                msgProgress?.Report("サイトコレクション情報取得");
                var siteCollections = tenant.GetSiteCollections(0, 0, false, false);
                await ctx.ExecuteQueryRetryAsync().ConfigureAwait(false);
                
                // 情報の詰め替え
                foreach (var siteCollection in siteCollections)
                {
                    msgProgress?.Report($"取得 - Title :{ siteCollection.Title } URL : { siteCollection.Url }");

                    var info = new SiteCollectionInfo()
                    {
                        Title = siteCollection.Title,
                        Url = siteCollection.Url
                    };
                    
                    ret.Add(info);
                }
            }

            msgProgress?.Report("テナント接続終了");

            return ret;
        }

        /// <summary>
        /// サイトコレクション作成
        /// </summary>
        /// <param name="tenantConnectionInfo">テナント接続情報</param>
        /// <param name="sitecollectionInfo">サイトコレクション作成情報</param>
        /// <param name="progress">進捗報告オブジェクト</param>
        public static async Task CreateSiteCollectionAsync(TenantConnectionInfo tenantConnectionInfo, SiteCollectionInfo sitecollectionInfo, IProgress<string> progress = null)
        {
            progress?.Report("テナント接続開始");

            using (var ctx = new ClientContext(tenantConnectionInfo.SPOAdminCenterUrl))
            {
                // 認証
                ctx.Credentials = new SharePointOnlineCredentials(tenantConnectionInfo.UserAccount, tenantConnectionInfo.UserSecurePassword);
                ctx.RequestTimeout = Timeout.Infinite;

                // テナント取得
                var tenant = new Tenant(ctx);

                // サイトコレクション作成情報
                progress?.Report("サイトコレクション作成開始");
                var creationInfo = new SiteCreationProperties()
                {
                    Title = sitecollectionInfo.Title,
                    Url = sitecollectionInfo.Url,
                    Owner = sitecollectionInfo.SiteOwnerLogin,
                    Template = sitecollectionInfo.Template,
                    StorageMaximumLevel = sitecollectionInfo.StorageMaximumLevel,
                    UserCodeMaximumLevel = 0,
                    Lcid = sitecollectionInfo.Lcid,
                    TimeZoneId = sitecollectionInfo.TimeZoneId,
                    CompatibilityLevel = (int)SiteCollectionInfo.CompatibilityLevels.SPS2013
                };
                
                // サイトコレクション作成
                var operation = tenant.CreateSite(creationInfo);
                ctx.Load(tenant);
                ctx.Load(operation, i => i.IsComplete);
                await ctx.ExecuteQueryRetryAsync();

                var siteCollections = tenant.GetSiteCollections(0, 0, true, true);
                ctx.Load(tenant);
                progress?.Report("サイトコレクション作成完了まで待機します・・・");
                await ctx.ExecuteQueryRetryAsync();

                // 作成完了まで待機
                while (!operation.IsComplete)
                {
                    progress?.Report("サイトコレクション作成完了まで待機します・・・");
                    Thread.Sleep(30000);
                    operation.RefreshLoad();
                    await ctx.ExecuteQueryRetryAsync();
                }
                progress?.Report("サイトコレクション作成完了");
            }

            progress?.Report("テナント接続終了");
        }

        /// <summary>
        /// サイトコレクション作成(コミュニケーションサイト)
        /// </summary>
        /// <param name="tenantConnectionInfo">テナント接続情報</param>
        /// <param name="sitecollectionInfo">サイトコレクション作成情報</param>
        /// <param name="progress">進捗報告オブジェクト</param>
        public static async Task CreateCommunicationSiteCollectionAsync(TenantConnectionInfo tenantConnectionInfo, SiteCollectionInfo siteCollectionInfo, SiteCollectionInfo.CommunicationSiteDesigns design, IProgress<string> progress = null)
        {
            progress?.Report("テナント接続開始");
            
            using (var ctx = new ClientContext(tenantConnectionInfo.SPOAdminCenterUrl))
            {
                // 認証
                ctx.Credentials = new SharePointOnlineCredentials(tenantConnectionInfo.UserAccount, tenantConnectionInfo.UserSecurePassword);
                ctx.RequestTimeout = Timeout.Infinite;
                
                // テナント取得
                var tenant = new Tenant(ctx);
                
                // サイトコレクション作成情報
                progress?.Report("サイトコレクション作成開始");
                var creationInfo = new CommunicationSiteCollectionCreationInformation()
                {
                    Title = siteCollectionInfo.Title,
                    Url = siteCollectionInfo.Url,
                    Owner = siteCollectionInfo.SiteOwnerLogin,
                    SiteDesign = (CommunicationSiteDesign)design,
                    Lcid = siteCollectionInfo.Lcid
                };

                // サイトコレクション作成
                progress?.Report("サイトコレクション作成完了まで待機します・・・");
                var operation = await ctx.CreateSiteAsync(creationInfo);
                ctx.Load(tenant);
                await ctx.ExecuteQueryRetryAsync();
            }

            progress?.Report("テナント接続終了");
        }

        /// <summary>
        /// サイトコレクション作成(コミュニケーションサイト)
        /// </summary>
        /// <param name="tenantConnectionInfo">テナント接続情報</param>
        /// <param name="sitecollectionInfo">サイトコレクション作成情報</param>
        /// <param name="progress">進捗報告オブジェクト</param>
        public static async Task CreateCommunicationSiteCollectionAsync(TenantConnectionInfo tenantConnectionInfo, SiteCollectionInfo siteCollectionInfo, Guid design, IProgress<string> progress = null)
        {
            progress?.Report("テナント接続開始");

            using (var ctx = new ClientContext(tenantConnectionInfo.SPOAdminCenterUrl))
            {
                // 認証
                ctx.Credentials = new SharePointOnlineCredentials(tenantConnectionInfo.UserAccount, tenantConnectionInfo.UserSecurePassword);
                ctx.RequestTimeout = Timeout.Infinite;

                // テナント取得
                var tenant = new Tenant(ctx);

                // サイトコレクション作成情報
                progress?.Report("サイトコレクション作成開始");
                var creationInfo = new CommunicationSiteCollectionCreationInformation()
                {
                    Title = siteCollectionInfo.Title,
                    Url = siteCollectionInfo.Url,
                    Owner = siteCollectionInfo.SiteOwnerLogin,
                    SiteDesignId = design,
                    Lcid = siteCollectionInfo.Lcid
                };

                // サイトコレクション作成
                progress?.Report("サイトコレクション作成完了まで待機します・・・");
                var operation = await ctx.CreateSiteAsync(creationInfo);
                ctx.Load(tenant);
                await ctx.ExecuteQueryRetryAsync();
            }

            progress?.Report("テナント接続終了");
        }
        
        /// <summary>
        /// サイトコレクション削除
        /// </summary>
        /// <param name="tenantConnectionInfo">テナント接続情報</param>
        /// <param name="siteFullUrl">サイト絶対URL(例：https://contoso.sharepoint.com/sites/del)</param>
        /// <param name="progress">進捗報告オブジェクト</param>
        public static async Task DeleteSiteCollectionAsync(TenantConnectionInfo tenantConnectionInfo, string siteFullUrl, IProgress<string> progress = null)
        {
            progress?.Report("テナント接続開始");

            using (var ctx = new ClientContext(tenantConnectionInfo.SPOAdminCenterUrl))
            {
                // 認証
                ctx.Credentials = new SharePointOnlineCredentials(tenantConnectionInfo.UserAccount, tenantConnectionInfo.UserSecurePassword);
                ctx.RequestTimeout = Timeout.Infinite;

                // テナント取得
                var tenant = new Tenant(ctx);

                // サイトコレクション削除
                progress?.Report("サイトコレクション削除");
                var result = tenant.DeleteSiteCollection(siteFullUrl, false);
                ctx.Load(tenant);
                await ctx.ExecuteQueryRetryAsync();

                // 結果通知
                if(!result)
                {
                    throw new Exception("サイトコレクションの削除が完了しませんでした。");
                }
            }

            progress?.Report("テナント接続終了");
        }

        /// <summary>
        /// サイトコレクションをゴミ箱から削除
        /// </summary>
        /// <param name="tenantConnectionInfo">テナント接続情報</param>
        /// <param name="siteFullUrl">サイト絶対URL(例：https://contoso.sharepoint.com/sites/del)</param>
        /// <param name="progress">進捗報告オブジェクト</param>
        public static async Task DeleteSiteCollectionFromRecyleBinAsync(TenantConnectionInfo tenantConnectionInfo, string siteFullUrl, IProgress<string> progress = null)
        {
            progress?.Report("テナント接続開始");

            using (var ctx = new ClientContext(tenantConnectionInfo.SPOAdminCenterUrl))
            {
                // 認証
                ctx.Credentials = new SharePointOnlineCredentials(tenantConnectionInfo.UserAccount, tenantConnectionInfo.UserSecurePassword);
                ctx.RequestTimeout = Timeout.Infinite;

                // テナント取得
                var tenant = new Tenant(ctx);
                
                // サイトコレクション削除
                progress?.Report("サイトコレクション削除");
                var result = tenant.DeleteSiteCollectionFromRecycleBin(siteFullUrl, true);
                ctx.Load(tenant);
                await ctx.ExecuteQueryRetryAsync();

                // 結果通知
                if (!result)
                {
                    throw new Exception("サイトコレクションの削除が完了しませんでした。");
                }
            }

            progress?.Report("テナント接続終了");
        }
        
        #endregion

        #region サイト操作

        /// <summary>
        /// サイトを取得
        /// </summary>
        /// <param name="siteConnectionInfo">サイト接続情報</param>
        /// <param name="recursive">サブサイトを再帰的に取得するか否か(既定値：false)</param>
        /// <param name="progress">進捗報告オブジェクト</param>
        /// <returns>サイト情報</returns>
        public static async Task<SiteInfo> GetSiteAsync(SiteConnectionInfo siteConnectionInfo, bool recursive = false, IProgress<string> progress = null)
        {
            SiteInfo ret;

            progress?.Report("サイト接続開始");

            using (var ctx = new ClientContext(siteConnectionInfo.WebAbsoluteUrl))
            {
                // 認証
                ctx.Credentials = new SharePointOnlineCredentials(siteConnectionInfo.UserAccount, siteConnectionInfo.UserSecurePassword);
                ctx.RequestTimeout = Timeout.Infinite;

                // サイト取得
                progress?.Report("サイト取得");
                Site site = ctx.Site;
                Web web = ctx.Web;
                ctx.Load(site);
                ctx.Load(site.Owner);
                ctx.Load(web);
                ctx.Load(web.RegionalSettings);
                ctx.Load(web.RegionalSettings.TimeZone);
                await ctx.ExecuteQueryRetryAsync().ConfigureAwait(false);

                // 詰め替え
                ret = new SiteInfo()
                {
                    Id = web.Id,
                    Title = web.Title,
                    Url = web.Url,
                    SiteOwnerLogin = site.Owner.Email,
                    Template = web.WebTemplate,
                    StorageMaximumLevel = 0,
                    Lcid = web.Language,
                    TimeZoneId = web.RegionalSettings.TimeZone.Id
                };

                // サブサイト取得
                if (recursive) ret.subSites = await GetSubSitesRecursiveAsync(ctx, web, recursive, progress).ConfigureAwait(false);
            }

            progress?.Report("サイト接続終了");

            return ret;
        }

        /// <summary>
        /// サブサイトを取得
        /// </summary>
        /// <param name="siteConnectionInfo">サイト接続情報</param>
        /// <param name="recursive">サブ-サブサイトを再帰的に取得するか否か(既定値：false)</param>
        /// <param name="progress">進捗報告オブジェクト</param>
        /// <returns>サイト情報一覧</returns>
        public static async Task<List<SiteInfo>> GetSubSitesAsync(SiteConnectionInfo siteConnectionInfo, bool recursive = false, IProgress<string> progress = null)
        {
            List<SiteInfo> ret;

            progress?.Report("サイト接続開始");

            using (var ctx = new ClientContext(siteConnectionInfo.WebAbsoluteUrl))
            {
                // 認証
                ctx.Credentials = new SharePointOnlineCredentials(siteConnectionInfo.UserAccount, siteConnectionInfo.UserSecurePassword);
                ctx.RequestTimeout = Timeout.Infinite;

                // サブサイト取得
                progress?.Report("サブサイト取得");
                Web web = ctx.Web;
                ctx.Load(web);
                ret = await GetSubSitesRecursiveAsync(ctx, web, recursive, progress);
            }

            progress?.Report("サイト接続終了");

            return ret;
        }

        /// <summary>
        /// サイトを作成
        /// </summary>
        /// <param name="siteConnectionInfo">サイト接続情報</param>
        /// <param name="siteInfo">サイト作成情報</param>
        /// <param name="progress">進捗報告オブジェクト</param>
        /// <returns>サイト情報</returns>
        public static async Task<SiteInfo> CreateSiteAsync(SiteConnectionInfo siteConnectionInfo, SiteInfo siteInfo, IProgress<string> progress = null)
        {
            SiteInfo ret;

            progress?.Report("サイト接続開始");

            using (var ctx = new ClientContext(siteConnectionInfo.WebAbsoluteUrl))
            {
                // 認証
                ctx.Credentials = new SharePointOnlineCredentials(siteConnectionInfo.UserAccount, siteConnectionInfo.UserSecurePassword);
                ctx.RequestTimeout = Timeout.Infinite;

                // サイト取得
                Web web = ctx.Web;

                // サイト作成
                progress?.Report("サイト作成");
                var createdWeb = web.CreateWeb(
                    new SiteEntity() {
                        Title = siteInfo.Title,
                        Url = siteInfo.Url,
                        SiteOwnerLogin = siteInfo.SiteOwnerLogin,
                        Template = siteInfo.Template,
                        StorageMaximumLevel = siteInfo.StorageMaximumLevel,
                        UserCodeMaximumLevel = 0,
                        Lcid = siteInfo.Lcid,
                        TimeZoneId = siteInfo.TimeZoneId
                    },
                    true,
                    true
                );
                ctx.Load(web);
                ctx.Load(createdWeb);
                ctx.Load(createdWeb.RegionalSettings);
                ctx.Load(createdWeb.RegionalSettings.TimeZone);
                await ctx.ExecuteQueryRetryAsync();

                // 詰め替え
                ret = new SiteInfo() {
                    Title = createdWeb.Title,
                    Url = createdWeb.Url,
                    Template = createdWeb.WebTemplate,
                    Lcid = createdWeb.RegionalSettings.CollationLCID,
                    TimeZoneId = createdWeb.RegionalSettings.TimeZone.Id
                };
            }

            progress?.Report("サイト接続終了");

            return ret;
        }

        /// <summary>
        /// サイトを削除
        /// </summary>
        /// <param name="siteConnectionInfo">サイト接続情報(削除対象の親を指定すること)</param>
        /// <param name="webUrl">サイトURL(削除対象を指定すること)</param>
        /// <param name="progress">進捗報告オブジェクト</param>
        public static async Task<bool> DeleteSiteAsync(SiteConnectionInfo siteConnectionInfo, string webUrl, IProgress<string> progress = null)
        {
            bool ret = false;

            progress?.Report("サイト接続開始");

            using (var ctx = new ClientContext(siteConnectionInfo.WebAbsoluteUrl))
            {
                // 認証
                ctx.Credentials = new SharePointOnlineCredentials(siteConnectionInfo.UserAccount, siteConnectionInfo.UserSecurePassword);
                ctx.RequestTimeout = Timeout.Infinite;

                // サイト取得
                Web web = ctx.Web;
                progress?.Report("サイト削除");

                // サイト削除
                ret = web.DeleteWeb(webUrl);
                ctx.Load(web);
                await ctx.ExecuteQueryRetryAsync();
            }

            progress?.Report("サイト接続終了");

            return ret;
        }

        #endregion

        #region サイトテンプレート操作

        /// <summary>
        /// サイトテンプレート一覧取得
        /// </summary>
        /// <param name="tenantConnectionInfo">テナント接続情報</param>
        /// <param name="lcid">ロケールID</param>
        /// <param name="compatibilityLevel">機能レベル</param>
        /// <param name="progress">進捗報告オブジェクト</param>
        /// <returns>サイトテンプレート一覧</returns>
        public static async Task<List<SiteTemplateInfo>> GetSiteTemplatesAsync(TenantConnectionInfo tenantConnectionInfo, uint? lcid = null, int? compatibilityLevel = null, IProgress<string> progress = null)
        {
            List<SiteTemplateInfo> ret = new List<SiteTemplateInfo>();

            progress?.Report("テナント接続開始");

            using (var ctx = new ClientContext(tenantConnectionInfo.SPOAdminCenterUrl))
            {
                // 認証
                ctx.Credentials = new SharePointOnlineCredentials(tenantConnectionInfo.UserAccount, tenantConnectionInfo.UserSecurePassword);
                ctx.RequestTimeout = Timeout.Infinite;

                // サイトテンプレート取得
                progress?.Report("サイトテンプレート取得");
                var tenant = new Tenant(ctx);
                SPOTenantWebTemplateCollection siteTemplates;
                if(lcid != null && compatibilityLevel != null)
                {
                    siteTemplates = tenant.GetSPOTenantWebTemplates(lcid.Value, compatibilityLevel.Value);
                }
                else
                {
                    siteTemplates = tenant.GetSPOTenantAllWebTemplates();
                }
                ctx.Load(tenant);
                ctx.Load(siteTemplates);
                await ctx.ExecuteQueryRetryAsync();

                // 情報の詰め替え
                progress?.Report("サイトテンプレート情報取得");
                foreach (var siteTemplate in siteTemplates)
                {
                    ret.Add(new SiteTemplateInfo()
                    {
                        Id = siteTemplate.Id,
                        Lcid = siteTemplate.Lcid,
                        Title = siteTemplate.Title,
                        Name = siteTemplate.Name,
                        Description = siteTemplate.Description,
                        DisplayCategory = siteTemplate.DisplayCategory,
                        CompatibilityLevel = siteTemplate.CompatibilityLevel,
                    });
                }
            }

            progress?.Report("テナント接続終了");

            return ret;
        }

        #endregion

        #region サイトデザイン操作

        /// <summary>
        /// サイトデザイン一覧取得
        /// </summary>
        /// <param name="tenantConnectionInfo">テナント接続情報</param>
        /// <param name="progress">進捗報告オブジェクト</param>
        /// <returns>サイトデザイン一覧</returns>
        public static async Task<List<SiteDesignInfo>> GetSiteDesignsAsync(TenantConnectionInfo tenantConnectionInfo, IProgress<string> progress = null)
        {
            List<SiteDesignInfo> ret = new List<SiteDesignInfo>();

            progress?.Report("テナント接続開始");

            using (var ctx = new ClientContext(tenantConnectionInfo.SPOAdminCenterUrl))
            {
                // 認証
                ctx.Credentials = new SharePointOnlineCredentials(tenantConnectionInfo.UserAccount, tenantConnectionInfo.UserSecurePassword);
                ctx.RequestTimeout = Timeout.Infinite;

                // テナント取得
                progress?.Report("サイトデザイン取得");
                var tenant = new Tenant(ctx);
                var siteDesigns = tenant.GetSiteDesigns();
                ctx.Load(tenant);
                ctx.Load(siteDesigns);
                await ctx.ExecuteQueryRetryAsync();

                // 情報の詰め替え
                progress?.Report("サイトデザイン情報取得");
                foreach (var siteDesign in siteDesigns)
                {
                    ret.Add(new SiteDesignInfo()
                    {
                        Id = siteDesign.Id,
                        IsDefault = siteDesign.IsDefault,
                        Description = siteDesign.Description,
                        PreviewImageUrl = siteDesign.PreviewImageUrl,
                        PreviewImageAltText = siteDesign.PreviewImageAltText,
                        SiteScriptIds = siteDesign.SiteScriptIds,
                        Title = siteDesign.Title,
                        Version = siteDesign.Version,
                        WebTemplate = siteDesign.WebTemplate
                    });
                }
            }

            progress?.Report("テナント接続終了");

            return ret;
        }

        #endregion

        #endregion

        #region プライベートメソッド

        /// <summary>
        /// 保管済テンプレートをProvisioningTemplateクラスとして取得
        /// </summary>
        /// <param name="templateGetInfo">テンプレート保管場所情報</param>
        /// <returns>テンプレートXML</returns>
        private static ProvisioningTemplate GetProvisioningTemplate(IConnectionInfo templateGetInfo)
        {
            var provider = templateGetInfo.GetProvider();
            return provider.GetTemplate(templateGetInfo.FileIdentifer);
        }
        
        /// <summary>
        /// サブサイトを再帰的に取得
        /// </summary>
        /// <param name="ctx">SharePointクライアントコンテキスト</param>
        /// <param name="web">SharePointサイト</param>
        /// <param name="progress">進捗報告オブジェクト</param>
        /// <returns>サブサイト情報</returns>
        private static async Task<List<SiteInfo>> GetSubSitesRecursiveAsync(ClientContext ctx, Web web, bool recursiveAll = false, IProgress<string> progress = null)
        {
            List<SiteInfo> ret = new List<SiteInfo>();

            // サブサイトを取得
            progress?.Report($"{ web.Url }のサブサイトを取得");
            var subWebs = web.GetSubwebsForCurrentUser(new SubwebQuery());
            ctx.Load(subWebs);
            await ctx.ExecuteQueryRetryAsync();
            foreach(var subWeb in subWebs)
            {
                var subSite = new SiteInfo()
                {
                    Title = subWeb.Title,
                    Url = subWeb.Url,
                    Template = subWeb.WebTemplate,
                    subSites = (recursiveAll == true)? await GetSubSitesRecursiveAsync(ctx, subWeb, recursiveAll, progress) : null
                };
                ret.Add(subSite);
            }

            return ret;
        }

        #endregion
    }


}
