# Pleasanter API 面の版差

<!-- このファイルは tools/VehicleVision.PleasanterTools.DotNetClient.ApiSurface が生成する。手で編集しない。 -->

手順・読み方・限界は [API 面の抽出](../_documents/API面の抽出.md) にある。機械可読の版は `history.json`、最新の版の全体は `latest.json`。

- 対象: 66 版（`Pleasanter_1.4.0.0` 〜 `Pleasanter_1.5.9.0`）
- **Since** が `1.4.0.0` のものは「その版以前から在る」の意味。**Until** は「その版から無い」の意味
- Pleasanter のソースからの機械抽出であり、**実機の Pleasanter では確かめていない**

## 版ごとの件数

| 版 | エンドポイント | 型 | メンバー・列挙値 |
| --- | --- | --- | --- |
| `1.4.0.0` | 52 | 112 | 2037 |
| `1.4.0.1` | 52 | 112 | 2037 |
| `1.4.1.0` | 52 | 114 | 2065 |
| `1.4.1.1` | 52 | 114 | 2065 |
| `1.4.2.0` | 52 | 114 | 2067 |
| `1.4.2.1` | 52 | 114 | 2067 |
| `1.4.2.2` | 52 | 114 | 2067 |
| `1.4.2.3` | 52 | 114 | 2067 |
| `1.4.3.0` | 52 | 114 | 2067 |
| `1.4.3.1` | 52 | 114 | 2067 |
| `1.4.4.0` | 52 | 114 | 2075 |
| `1.4.5.0` | 53 | 114 | 2076 |
| `1.4.6.0` | 54 | 116 | 2084 |
| `1.4.6.1` | 54 | 116 | 2084 |
| `1.4.6.2` | 54 | 116 | 2084 |
| `1.4.6.3` | 54 | 116 | 2084 |
| `1.4.6.4` | 54 | 116 | 2084 |
| `1.4.7.0` | 54 | 116 | 2084 |
| `1.4.7.1` | 54 | 116 | 2084 |
| `1.4.7.2` | 54 | 116 | 2084 |
| `1.4.8.0` | 54 | 116 | 2086 |
| `1.4.8.1` | 54 | 116 | 2086 |
| `1.4.9.0` | 54 | 116 | 2086 |
| `1.4.9.1` | 54 | 116 | 2086 |
| `1.4.9.2` | 54 | 116 | 2086 |
| `1.4.10.0` | 54 | 116 | 2097 |
| `1.4.10.1` | 54 | 116 | 2097 |
| `1.4.10.2` | 54 | 116 | 2097 |
| `1.4.10.3` | 54 | 116 | 2097 |
| `1.4.11.0` | 54 | 116 | 2101 |
| `1.4.12.0` | 54 | 116 | 2108 |
| `1.4.13.0` | 60 | 117 | 2128 |
| `1.4.14.0` | 60 | 117 | 2133 |
| `1.4.15.0` | 60 | 122 | 2212 |
| `1.4.16.0` | 60 | 122 | 2224 |
| `1.4.16.1` | 60 | 122 | 2224 |
| `1.4.16.2` | 60 | 122 | 2224 |
| `1.4.16.3` | 60 | 122 | 2224 |
| `1.4.17.0` | 60 | 122 | 2224 |
| `1.4.17.1` | 60 | 122 | 2224 |
| `1.4.18.0` | 60 | 122 | 2227 |
| `1.4.18.1` | 60 | 122 | 2227 |
| `1.4.19.0` | 60 | 122 | 2227 |
| `1.4.19.1` | 60 | 122 | 2227 |
| `1.4.19.2` | 60 | 122 | 2227 |
| `1.4.20.0` | 60 | 122 | 2229 |
| `1.4.21.0` | 60 | 123 | 2230 |
| `1.4.21.1` | 60 | 123 | 2230 |
| `1.4.22.0` | 60 | 123 | 2231 |
| `1.4.23.0` | 60 | 126 | 2249 |
| `1.4.23.1` | 60 | 126 | 2249 |
| `1.4.23.2` | 60 | 126 | 2249 |
| `1.4.23.3` | 60 | 126 | 2249 |
| `1.5.0.0` | 60 | 126 | 2252 |
| `1.5.1.0` | 60 | 126 | 2261 |
| `1.5.2.0` | 60 | 127 | 2265 |
| `1.5.3.0` | 60 | 127 | 2266 |
| `1.5.4.0` | 60 | 127 | 2266 |
| `1.5.5.0` | 60 | 127 | 2266 |
| `1.5.6.0` | 60 | 133 | 2295 |
| `1.5.6.1` | 60 | 133 | 2295 |
| `1.5.7.0` | 65 | 135 | 2316 |
| `1.5.7.1` | 65 | 135 | 2316 |
| `1.5.8.0` | 82 | 136 | 2335 |
| `1.5.8.1` | 82 | 136 | 2335 |
| `1.5.9.0` | 83 | 136 | 2335 |

## エンドポイント

変更の列は、変わった版と、変わった要素（`history.json` の revisions で値を確かめる）。

| エンドポイント | 領域 | Since | Until | 変更 |
| --- | --- | --- | --- | --- |
| `DELETE scim/v2/groups/{id}` | scim | `1.5.8.0` | | |
| `DELETE scim/v2/users/{id}` | scim | `1.5.8.0` | | |
| `GET scim/v2/groups` | scim | `1.5.8.0` | | |
| `GET scim/v2/groups/{id}` | scim | `1.5.8.0` | | |
| `GET scim/v2/resourcetypes` | scim | `1.5.8.0` | | |
| `GET scim/v2/resourcetypes/{id}` | scim | `1.5.8.0` | | |
| `GET scim/v2/schemas` | scim | `1.5.8.0` | | |
| `GET scim/v2/schemas/{id}` | scim | `1.5.8.0` | | |
| `GET scim/v2/serviceproviderconfig` | scim | `1.5.8.0` | | |
| `GET scim/v2/users` | scim | `1.5.8.0` | | |
| `GET scim/v2/users/{id}` | scim | `1.5.8.0` | | |
| `PATCH scim/v2/groups/{id}` | scim | `1.5.8.0` | | |
| `PATCH scim/v2/users/{id}` | scim | `1.5.8.0` | | |
| `POST api/backgroundtasks/rebuildsearchindexes` | api | `1.4.0.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST api/backgroundtasks/{id}/rebuildsearchindexes` | api | `1.4.0.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST api/binaries/upload` | api | `1.4.0.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST api/binaries/{guid}/get` | api | `1.4.0.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST api/binaries/{guid}/getstream` | api | `1.4.0.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST api/binaries/{guid}/upload` | api | `1.4.0.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST api/demo/register` | api | `1.4.0.0` | | `1.5.6.0`（attributes, rateLimiting）; `1.5.7.0`（gates） |
| `POST api/depts/create` | api | `1.4.0.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST api/depts/get` | api | `1.4.0.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST api/depts/import` | api | `1.4.0.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST api/depts/{id}/delete` | api | `1.4.0.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST api/depts/{id}/get` | api | `1.4.0.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST api/depts/{id}/update` | api | `1.4.0.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST api/extended/sql` | api | `1.4.0.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST api/extensions/create` | api | `1.4.13.0` | | `1.4.16.0`（auth, response）; `1.5.6.0`（attributes, rateLimiting） |
| `POST api/extensions/get` | api | `1.4.13.0` | | `1.4.16.0`（auth, response）; `1.5.6.0`（attributes, rateLimiting） |
| `POST api/extensions/{id}/delete` | api | `1.4.13.0` | | `1.4.16.0`（auth, response）; `1.5.6.0`（attributes, rateLimiting） |
| `POST api/extensions/{id}/get` | api | `1.4.13.0` | | `1.4.16.0`（auth, response）; `1.5.6.0`（attributes, rateLimiting） |
| `POST api/extensions/{id}/update` | api | `1.4.13.0` | | `1.4.16.0`（auth, response）; `1.5.6.0`（attributes, rateLimiting） |
| `POST api/groups/create` | api | `1.4.0.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST api/groups/get` | api | `1.4.0.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST api/groups/import` | api | `1.4.0.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST api/groups/{id}/delete` | api | `1.4.0.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST api/groups/{id}/get` | api | `1.4.0.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST api/groups/{id}/update` | api | `1.4.0.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST api/items/{id}/bulkdelete` | api | `1.4.0.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST api/items/{id}/bulkupsert` | api | `1.4.6.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST api/items/{id}/copysitepackage` | api | `1.4.0.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST api/items/{id}/create` | api | `1.4.0.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST api/items/{id}/createsite` | api | `1.4.0.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST api/items/{id}/delete` | api | `1.4.0.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST api/items/{id}/deletesite` | api | `1.4.0.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST api/items/{id}/export` | api | `1.4.0.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST api/items/{id}/get` | api | `1.4.0.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST api/items/{id}/getclosestsiteid` | api | `1.4.5.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST api/items/{id}/getsite` | api | `1.4.0.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST api/items/{id}/import` | api | `1.4.0.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST api/items/{id}/movesite` | api | `1.5.9.0` | | |
| `POST api/items/{id}/synchronizesummaries` | api | `1.4.0.0` | | `1.4.12.0`（response）; `1.5.6.0`（attributes, rateLimiting） |
| `POST api/items/{id}/update` | api | `1.4.0.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST api/items/{id}/updatesite` | api | `1.4.0.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST api/items/{id}/updatesitesettings` | api | `1.4.0.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST api/items/{id}/upsert` | api | `1.4.0.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST api/sessions/delete` | api | `1.4.0.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST api/sessions/get` | api | `1.4.0.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST api/sessions/set` | api | `1.4.0.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST api/tenants/create` | api | `1.5.7.0` | | |
| `POST api/tenants/get` | api | `1.5.7.0` | | |
| `POST api/tenants/{id}/delete` | api | `1.5.7.0` | | |
| `POST api/tenants/{id}/resume` | api | `1.5.7.0` | | |
| `POST api/tenants/{id}/suspend` | api | `1.5.7.0` | | |
| `POST api/users/create` | api | `1.4.0.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST api/users/get` | api | `1.4.0.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST api/users/import` | api | `1.4.0.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST api/users/{id}/delete` | api | `1.4.0.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST api/users/{id}/get` | api | `1.4.0.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST api/users/{id}/update` | api | `1.4.0.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST api/utility/getlicenseinfo` | api | `1.4.13.0` | | `1.4.21.0`（gates, requestTypes）; `1.5.6.0`（attributes, rateLimiting） |
| `POST api/{reference}/{id}/{controller}/send` | api | `1.4.0.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST legacy:Api_Depts/Get` | legacy | `1.4.0.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST legacy:Api_Groups/Get` | legacy | `1.4.0.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST legacy:Api_Items/Create` | legacy | `1.4.0.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST legacy:Api_Items/Delete` | legacy | `1.4.0.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST legacy:Api_Items/Get` | legacy | `1.4.0.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST legacy:Api_Items/Update` | legacy | `1.4.0.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST legacy:Api_Users/Get` | legacy | `1.4.0.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST scim/v2/groups` | scim | `1.5.8.0` | | |
| `POST scim/v2/users` | scim | `1.5.8.0` | | |
| `PUT scim/v2/groups/{id}` | scim | `1.5.8.0` | | |
| `PUT scim/v2/users/{id}` | scim | `1.5.8.0` | | |

## 途中の版で増減・変化した項目

全版（`1.4.0.0` 〜 `1.5.9.0`）で在り、中身も変わらない項目は省き、件数だけを末尾に書く。

### 型

| 項目 | Since | Until | 変更 |
| --- | --- | --- | --- |
| `Implem.Pleasanter.Libraries.License.LicenseUtilities.LicenseInfoApi` | `1.4.21.0` | | |
| `Implem.Pleasanter.Libraries.Models.Versions.AfterCreateActionTypes` | `1.4.23.0` | | |
| `Implem.Pleasanter.Libraries.Models.Versions.AfterUpdateActionTypes` | `1.4.23.0` | | |
| `Implem.Pleasanter.Libraries.ServerScripts.ServerScriptElements` | `1.5.6.0` | | |
| `Implem.Pleasanter.Libraries.ServerScripts.ServerScriptModel.ServerScriptModelColumn` | `1.5.6.0` | | |
| `Implem.Pleasanter.Libraries.ServerScripts.ServerScriptModel.ServerScriptModelRow` | `1.5.6.0` | | |
| `Implem.Pleasanter.Libraries.ServerScripts.ServerScriptModelResponse` | `1.5.6.0` | | |
| `Implem.Pleasanter.Libraries.ServerScripts.ServerScriptModelResponse.Types` | `1.5.6.0` | | |
| `Implem.Pleasanter.Libraries.ServerScripts.ServerScriptModelResponses` | `1.5.6.0` | | |
| `Implem.Pleasanter.Libraries.Settings.AiProvider` | `1.5.8.0` | | |
| `Implem.Pleasanter.Libraries.Settings.AnalyPartSetting` | `1.4.1.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DashboardPartLayout` | `1.4.1.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Process.AfterProcessStatusChangeActionTypes` | `1.4.23.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings.StandardExportTypes` | `1.5.2.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ColumnApiSettingModel` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.DragParamsApiSettingModel` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.DragStateApiSettingModel` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.LinkApiSettingModel` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.SectionApiSettingModel` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.CreateTenantApiModel` | `1.5.7.0` | | |
| `Implem.Pleasanter.Models.ExtensionApiModel` | `1.4.13.0` | | |
| `Implem.Pleasanter.Models.Issues.IssueBulkUpsertApiModel` | `1.4.6.0` | | |
| `Implem.Pleasanter.Models.Results.ResultBulkUpsertApiModel` | `1.4.6.0` | | |
| `Implem.Pleasanter.Models.SuspendTenantApiModel` | `1.5.7.0` | | |

### メンバー（プロパティ・フィールド）

| 項目 | Since | Until | 変更 |
| --- | --- | --- | --- |
| `Implem.Pleasanter.Libraries.DataTypes.Attachment#Extension` | `1.4.7.0` | | |
| `Implem.Pleasanter.Libraries.DataTypes.Attachment#Extention` | `1.4.0.0` | `1.4.7.0` | |
| `Implem.Pleasanter.Libraries.License.LicenseUtilities.LicenseInfoApi#AppTypes` | `1.4.21.0` | | |
| `Implem.Pleasanter.Libraries.Requests.Api#PageSize` | `1.4.13.0` | | |
| `Implem.Pleasanter.Libraries.Requests.Api#SsCache` | `1.5.6.0` | | |
| `Implem.Pleasanter.Libraries.Requests.ImportApi#MigrationMode` | `1.4.16.0` | | |
| `Implem.Pleasanter.Libraries.Requests.ImportApi#RejectNullImport` | `1.4.15.0` | | |
| `Implem.Pleasanter.Libraries.ServerScripts.ServerScriptElements#DisplayTypeHash` | `1.5.6.0` | | |
| `Implem.Pleasanter.Libraries.ServerScripts.ServerScriptElements#LabelTextHash` | `1.5.6.0` | | |
| `Implem.Pleasanter.Libraries.ServerScripts.ServerScriptModel.ServerScriptModelColumn#ChoiceHash` | `1.5.6.0` | | |
| `Implem.Pleasanter.Libraries.ServerScripts.ServerScriptModel.ServerScriptModelColumn#ExtendedCellCss` | `1.5.6.0` | | |
| `Implem.Pleasanter.Libraries.ServerScripts.ServerScriptModel.ServerScriptModelColumn#ExtendedControlCss` | `1.5.6.0` | | |
| `Implem.Pleasanter.Libraries.ServerScripts.ServerScriptModel.ServerScriptModelColumn#ExtendedFieldCss` | `1.5.6.0` | | |
| `Implem.Pleasanter.Libraries.ServerScripts.ServerScriptModel.ServerScriptModelColumn#ExtendedHtmlAfterControl` | `1.5.6.0` | | |
| `Implem.Pleasanter.Libraries.ServerScripts.ServerScriptModel.ServerScriptModelColumn#ExtendedHtmlAfterField` | `1.5.6.0` | | |
| `Implem.Pleasanter.Libraries.ServerScripts.ServerScriptModel.ServerScriptModelColumn#ExtendedHtmlBeforeField` | `1.5.6.0` | | |
| `Implem.Pleasanter.Libraries.ServerScripts.ServerScriptModel.ServerScriptModelColumn#ExtendedHtmlBeforeLabel` | `1.5.6.0` | | |
| `Implem.Pleasanter.Libraries.ServerScripts.ServerScriptModel.ServerScriptModelColumn#ExtendedHtmlBetweenLabelAndControl` | `1.5.6.0` | | |
| `Implem.Pleasanter.Libraries.ServerScripts.ServerScriptModel.ServerScriptModelColumn#Hide` | `1.5.6.0` | | |
| `Implem.Pleasanter.Libraries.ServerScripts.ServerScriptModel.ServerScriptModelColumn#HideChanged` | `1.5.6.0` | | |
| `Implem.Pleasanter.Libraries.ServerScripts.ServerScriptModel.ServerScriptModelColumn#LabelRaw` | `1.5.6.0` | | |
| `Implem.Pleasanter.Libraries.ServerScripts.ServerScriptModel.ServerScriptModelColumn#LabelText` | `1.5.6.0` | | |
| `Implem.Pleasanter.Libraries.ServerScripts.ServerScriptModel.ServerScriptModelColumn#RawText` | `1.5.6.0` | | |
| `Implem.Pleasanter.Libraries.ServerScripts.ServerScriptModel.ServerScriptModelColumn#ReadOnly` | `1.5.6.0` | | |
| `Implem.Pleasanter.Libraries.ServerScripts.ServerScriptModel.ServerScriptModelColumn#ValidateRequired` | `1.5.6.0` | | |
| `Implem.Pleasanter.Libraries.ServerScripts.ServerScriptModel.ServerScriptModelRow#Columns` | `1.5.6.0` | | |
| `Implem.Pleasanter.Libraries.ServerScripts.ServerScriptModel.ServerScriptModelRow#Elements` | `1.5.6.0` | | |
| `Implem.Pleasanter.Libraries.ServerScripts.ServerScriptModel.ServerScriptModelRow#ExtendedRowCss` | `1.5.6.0` | | |
| `Implem.Pleasanter.Libraries.ServerScripts.ServerScriptModel.ServerScriptModelRow#ExtendedRowData` | `1.5.6.0` | | |
| `Implem.Pleasanter.Libraries.ServerScripts.ServerScriptModel.ServerScriptModelRow#Hidden` | `1.5.6.0` | | |
| `Implem.Pleasanter.Libraries.ServerScripts.ServerScriptModel.ServerScriptModelRow#Responses` | `1.5.6.0` | | |
| `Implem.Pleasanter.Libraries.ServerScripts.ServerScriptModelResponse#ColumnName` | `1.5.6.0` | | |
| `Implem.Pleasanter.Libraries.ServerScripts.ServerScriptModelResponse#Type` | `1.5.6.0` | | |
| `Implem.Pleasanter.Libraries.Settings.AiProvider#ConnectionSetting` | `1.5.8.0` | | |
| `Implem.Pleasanter.Libraries.Settings.AiProvider#Disabled` | `1.5.8.0` | | |
| `Implem.Pleasanter.Libraries.Settings.AiProvider#Format` | `1.5.8.0` | | |
| `Implem.Pleasanter.Libraries.Settings.AiProvider#Id` | `1.5.8.0` | | |
| `Implem.Pleasanter.Libraries.Settings.AiProvider#ProviderType` | `1.5.8.0` | | |
| `Implem.Pleasanter.Libraries.Settings.AiProvider#Title` | `1.5.8.0` | | |
| `Implem.Pleasanter.Libraries.Settings.AnalyPartSetting#AggregationTarget` | `1.4.1.0` | | |
| `Implem.Pleasanter.Libraries.Settings.AnalyPartSetting#AggregationType` | `1.4.1.0` | | |
| `Implem.Pleasanter.Libraries.Settings.AnalyPartSetting#GroupBy` | `1.4.1.0` | | |
| `Implem.Pleasanter.Libraries.Settings.AnalyPartSetting#Id` | `1.4.1.0` | | |
| `Implem.Pleasanter.Libraries.Settings.AnalyPartSetting#TimePeriod` | `1.4.1.0` | | |
| `Implem.Pleasanter.Libraries.Settings.AnalyPartSetting#TimePeriodValue` | `1.4.1.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#CellSticky` | `1.4.20.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#CellWidth` | `1.4.20.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#CellWordWrap` | `1.4.22.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#MultilingualLabelText` | `1.4.23.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#RecordingFormat` | `1.4.16.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DashboardPart#IndexSites` | `1.4.1.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DashboardPart#IndexSitesData` | `1.4.1.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DashboardPartLayout#Content` | `1.4.1.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DashboardPartLayout#H` | `1.4.1.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DashboardPartLayout#Id` | `1.4.1.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DashboardPartLayout#W` | `1.4.1.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DashboardPartLayout#X` | `1.4.1.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DashboardPartLayout#Y` | `1.4.1.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DataChange#ValueFormulaIsDisplayError` | `1.4.10.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DataChange#ValueFormulaNotUseDisplayName` | `1.4.10.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Export#ExportCommentsJsonFormat` | `1.4.18.0` | | |
| `Implem.Pleasanter.Libraries.Settings.ExportColumn#ExportJsonFormat` | `1.4.18.0` | | |
| `Implem.Pleasanter.Libraries.Settings.FormulaSet#Disabled` | `1.5.0.0` | | |
| `Implem.Pleasanter.Libraries.Settings.FormulaSet#FormulaScriptOutOfCondition` | `1.4.11.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Html#Analy` | `1.4.1.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Html#DraftKey` | `1.5.7.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Html#DraftOutputMode` | `1.5.7.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Link#NotReturnParentRecord` | `1.4.10.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Notification#BccAddress` | `1.4.10.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Notification#CcAddress` | `1.4.10.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Process#AfterProcessStatusChangeActionType` | `1.4.23.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Process#Disabled` | `1.5.1.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Process#Icon` | `1.4.11.0` | | |
| `Implem.Pleasanter.Libraries.Settings.QuickAccessSite#OpenInNewTab` | `1.4.10.0` | | |
| `Implem.Pleasanter.Libraries.Settings.QuickAccessSite#Title` | `1.4.10.0` | | |
| `Implem.Pleasanter.Libraries.Settings.QuickAccessSite#Url` | `1.4.10.0` | | |
| `Implem.Pleasanter.Libraries.Settings.QuickAccessSite#ViewMode` | `1.4.10.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Script#Analy` | `1.4.1.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Script#DraftKey` | `1.5.7.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Script#DraftOutputMode` | `1.5.7.0` | | |
| `Implem.Pleasanter.Libraries.Settings.ServerScript#AfterBulkDelete` | `1.4.16.0` | | |
| `Implem.Pleasanter.Libraries.Settings.ServerScript#BeforeBulkDelete` | `1.4.16.0` | | |
| `Implem.Pleasanter.Libraries.Settings.ServerScript#Disabled` | `1.4.12.0` | | |
| `Implem.Pleasanter.Libraries.Settings.ServerScript#Functionalize` | `1.4.12.0` | | |
| `Implem.Pleasanter.Libraries.Settings.ServerScript#TryCatch` | `1.4.12.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#AfterCreateActionType` | `1.4.23.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#AfterUpdateActionType` | `1.4.23.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#AiProviders` | `1.5.8.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#AiProvidersAllDisabled` | `1.5.8.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#AllowMigrationMode` | `1.4.16.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#AnalyGuide` | `1.4.1.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#DisableSiteConditions` | `1.4.14.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#EnableAnaly` | `1.4.1.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#EnableExpandLinkPath` | `1.4.12.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#FormEndDateTime` | `1.4.23.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#FormStartDateTime` | `1.4.23.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#FormThanksMessage` | `1.4.23.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#FormUnavailableMessage` | `1.4.23.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#FormulasGetErrorDetails` | `1.5.1.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#GuideAllowExpand` | `1.4.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#GuideExpand` | `1.4.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#HtmlsAllDisabled` | `1.4.14.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#ItemPermissionType` | `1.4.0.0` | `1.5.6.0` | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#LinkPageSize` | `1.4.18.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#NotInheritPermissionsWhenCreatingSite` | `1.4.13.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#OpenEditInNewTab` | `1.4.8.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#PermissionType` | `1.4.0.0` | `1.5.6.0` | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#PermissionTypeCache` | `1.4.0.0` | `1.5.6.0` | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#ProcessOutputFormulaLogs` | `1.4.10.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#RejectNullImport` | `1.4.15.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#ScriptsAllDisabled` | `1.4.14.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#ServerScriptModelRowCache1` | `1.5.6.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#ServerScriptsAllDisabled` | `1.4.14.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#ServerScriptsGetErrorDetails` | `1.5.1.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#StandardExportType` | `1.5.2.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#StylesAllDisabled` | `1.4.14.0` | | |
| `Implem.Pleasanter.Libraries.Settings.StatusControl#Disabled` | `1.5.0.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Style#Analy` | `1.4.1.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Style#DraftKey` | `1.5.7.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Style#DraftOutputMode` | `1.5.7.0` | | |
| `Implem.Pleasanter.Libraries.Settings.View#AnalyPartSettings` | `1.4.1.0` | | |
| `Implem.Pleasanter.Libraries.Settings.View#DashboardPartLayoutHash` | `1.4.1.0` | | |
| `Implem.Pleasanter.Libraries.Settings.View#GuidesReduced` | `1.4.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.View#IndexSuffix` | `1.4.1.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ColumnApiSettingModel#AllowDeleteAttachments` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ColumnApiSettingModel#AllowImage` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ColumnApiSettingModel#CellSticky` | `1.5.1.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ColumnApiSettingModel#CellWidth` | `1.5.1.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ColumnApiSettingModel#CellWordWrap` | `1.5.1.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ColumnApiSettingModel#ChoicesControlType` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ColumnApiSettingModel#ChoicesText` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ColumnApiSettingModel#ColumnName` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ColumnApiSettingModel#ControlType` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ColumnApiSettingModel#DateTimeStep` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ColumnApiSettingModel#DecimalPlaces` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ColumnApiSettingModel#DefaultInput` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ColumnApiSettingModel#Description` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ColumnApiSettingModel#DragParams` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ColumnApiSettingModel#EditorFormat` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ColumnApiSettingModel#EditorReadOnly` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ColumnApiSettingModel#FieldCss` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ColumnApiSettingModel#Format` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ColumnApiSettingModel#GridFormat` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ColumnApiSettingModel#GridLabelText` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ColumnApiSettingModel#Id` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ColumnApiSettingModel#InputGuide` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ColumnApiSettingModel#IsNewEnable` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ColumnApiSettingModel#LabelText` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ColumnApiSettingModel#LimitQuantity` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ColumnApiSettingModel#LimitSize` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ColumnApiSettingModel#Link` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ColumnApiSettingModel#Max` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ColumnApiSettingModel#MaxLength` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ColumnApiSettingModel#Min` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ColumnApiSettingModel#NoDuplication` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ColumnApiSettingModel#NoWrap` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ColumnApiSettingModel#Nullable` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ColumnApiSettingModel#RoundingType` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ColumnApiSettingModel#Step` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ColumnApiSettingModel#TextAlign` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ColumnApiSettingModel#ThumbnailLimitSize` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ColumnApiSettingModel#TotalLimitSize` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ColumnApiSettingModel#Unit` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ColumnApiSettingModel#ValidateRequired` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ColumnApiSettingModel#ViewerSwitchingType` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.DragParamsApiSettingModel#Category` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.DragParamsApiSettingModel#LinkName` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.DragParamsApiSettingModel#State` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.DragParamsApiSettingModel#Type` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.DragStateApiSettingModel#Edit` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.DragStateApiSettingModel#Filter` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.DragStateApiSettingModel#Grid` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.HtmlApiSettingModel#DraftKey` | `1.5.7.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.HtmlApiSettingModel#DraftOutputMode` | `1.5.7.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.HtmlApiSettingModel#HtmlAnaly` | `1.4.1.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.LinkApiSettingModel#AddSource` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.LinkApiSettingModel#ColumnName` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.LinkApiSettingModel#ExcludeMe` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.LinkApiSettingModel#JsonFormat` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.LinkApiSettingModel#LinkActions` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.LinkApiSettingModel#Lookups` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.LinkApiSettingModel#MembersOnly` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.LinkApiSettingModel#NoAddButton` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.LinkApiSettingModel#NotReturnParentRecord` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.LinkApiSettingModel#Priority` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.LinkApiSettingModel#SearchFormat` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.LinkApiSettingModel#SelectNewLink` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.LinkApiSettingModel#SiteId` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.LinkApiSettingModel#SiteTitle` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.LinkApiSettingModel#SourceId` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.LinkApiSettingModel#TableName` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.LinkApiSettingModel#View` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ProcessApiSettingModel#AfterProcessStatusChangeActionType` | `1.4.23.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ProcessApiSettingModel#Disabled` | `1.5.1.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ProcessApiSettingModel#Icon` | `1.4.11.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ScriptApiSettingModel#DraftKey` | `1.5.7.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ScriptApiSettingModel#DraftOutputMode` | `1.5.7.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ScriptApiSettingModel#ScriptAnaly` | `1.4.1.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.SectionApiSettingModel#AllowExpand` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.SectionApiSettingModel#Expand` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.SectionApiSettingModel#Hide` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.SectionApiSettingModel#Id` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.SectionApiSettingModel#LabelText` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ServerScriptApiSettingModel#Disabled` | `1.4.12.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ServerScriptApiSettingModel#Functionalize` | `1.4.12.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ServerScriptApiSettingModel#ServerScriptAfterBulkDelete` | `1.4.16.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ServerScriptApiSettingModel#ServerScriptBeforeBulkDelete` | `1.4.16.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ServerScriptApiSettingModel#TryCatch` | `1.4.12.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.SiteSettingsApiModel#Columns` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.SiteSettingsApiModel#EditorColumnHash` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.SiteSettingsApiModel#FilterColumns` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.SiteSettingsApiModel#GeneralTabLabelText` | `1.5.3.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.SiteSettingsApiModel#GridColumns` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.SiteSettingsApiModel#Links` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.SiteSettingsApiModel#SectionLatestId` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.SiteSettingsApiModel#Sections` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.SiteSettingsApiModel#TabLatestId` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.SiteSettingsApiModel#Tabs` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.SiteSettingsApiModel#Timestamp` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.SiteSettingsApiModel#VerUp` | `1.5.6.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.StatusControlApiSettingModel#Disabled` | `1.5.0.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.StyleApiSettingModel#DraftKey` | `1.5.7.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.StyleApiSettingModel#DraftOutputMode` | `1.5.7.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.StyleApiSettingModel#StyleAnaly` | `1.4.1.0` | | |
| `Implem.Pleasanter.Models.CreateTenantApiModel#ApiKey` | `1.5.7.0` | | |
| `Implem.Pleasanter.Models.CreateTenantApiModel#Language` | `1.5.7.0` | | |
| `Implem.Pleasanter.Models.CreateTenantApiModel#LoginId` | `1.5.7.0` | | |
| `Implem.Pleasanter.Models.CreateTenantApiModel#Name` | `1.5.7.0` | | |
| `Implem.Pleasanter.Models.CreateTenantApiModel#NotifyMailAddress` | `1.5.7.0` | | |
| `Implem.Pleasanter.Models.CreateTenantApiModel#TenantName` | `1.5.7.0` | | |
| `Implem.Pleasanter.Models.CreateTenantApiModel#Title` | `1.5.7.0` | | |
| `Implem.Pleasanter.Models.ExtensionApiModel#ApiVersion` | `1.4.13.0` | | |
| `Implem.Pleasanter.Models.ExtensionApiModel#Body` | `1.4.13.0` | | |
| `Implem.Pleasanter.Models.ExtensionApiModel#Comments` | `1.4.13.0` | | |
| `Implem.Pleasanter.Models.ExtensionApiModel#CreatedTime` | `1.4.13.0` | | |
| `Implem.Pleasanter.Models.ExtensionApiModel#Creator` | `1.4.13.0` | | |
| `Implem.Pleasanter.Models.ExtensionApiModel#Description` | `1.4.13.0` | | |
| `Implem.Pleasanter.Models.ExtensionApiModel#Disabled` | `1.4.13.0` | | |
| `Implem.Pleasanter.Models.ExtensionApiModel#ExtensionId` | `1.4.13.0` | | |
| `Implem.Pleasanter.Models.ExtensionApiModel#ExtensionName` | `1.4.13.0` | | |
| `Implem.Pleasanter.Models.ExtensionApiModel#ExtensionSettings` | `1.4.13.0` | | |
| `Implem.Pleasanter.Models.ExtensionApiModel#ExtensionType` | `1.4.13.0` | | |
| `Implem.Pleasanter.Models.ExtensionApiModel#TenantId` | `1.4.13.0` | | |
| `Implem.Pleasanter.Models.ExtensionApiModel#UpdatedTime` | `1.4.13.0` | | |
| `Implem.Pleasanter.Models.ExtensionApiModel#Updator` | `1.4.13.0` | | |
| `Implem.Pleasanter.Models.ExtensionApiModel#Ver` | `1.4.13.0` | | |
| `Implem.Pleasanter.Models.GroupApiModel#GroupChildren` | `1.4.4.0` | | |
| `Implem.Pleasanter.Models.GroupApiModel#LdapGuid` | `1.4.4.0` | | |
| `Implem.Pleasanter.Models.GroupApiModel#LdapSearchRoot` | `1.4.4.0` | | |
| `Implem.Pleasanter.Models.GroupApiModel#LdapSync` | `1.4.4.0` | | |
| `Implem.Pleasanter.Models.GroupApiModel#ScimExternalId` | `1.5.8.0` | | |
| `Implem.Pleasanter.Models.GroupApiModel#ScimId` | `1.5.8.0` | | |
| `Implem.Pleasanter.Models.GroupApiModel#ScimSync` | `1.5.8.0` | | |
| `Implem.Pleasanter.Models.GroupApiModel#SynchronizedTime` | `1.4.4.0` | | |
| `Implem.Pleasanter.Models.IssueApiModel#MigrationMode` | `1.4.16.0` | | |
| `Implem.Pleasanter.Models.IssueApiModel#ProcessIds` | `1.4.16.0` | | |
| `Implem.Pleasanter.Models.Issues.IssueBulkUpsertApiModel#Data` | `1.4.6.0` | | `1.4.7.0`（type） |
| `Implem.Pleasanter.Models.Issues.IssueBulkUpsertApiModel#KeyNotFoundCreate` | `1.4.6.0` | | |
| `Implem.Pleasanter.Models.Issues.IssueBulkUpsertApiModel#Keys` | `1.4.6.0` | | `1.4.7.0`（type） |
| `Implem.Pleasanter.Models.Issues.IssueBulkUpsertApiModel#SiteId` | `1.4.6.0` | | |
| `Implem.Pleasanter.Models.ResultApiModel#MigrationMode` | `1.4.16.0` | | |
| `Implem.Pleasanter.Models.ResultApiModel#ProcessIds` | `1.4.16.0` | | |
| `Implem.Pleasanter.Models.Results.ResultBulkUpsertApiModel#Data` | `1.4.6.0` | | `1.4.7.0`（type） |
| `Implem.Pleasanter.Models.Results.ResultBulkUpsertApiModel#KeyNotFoundCreate` | `1.4.6.0` | | |
| `Implem.Pleasanter.Models.Results.ResultBulkUpsertApiModel#Keys` | `1.4.6.0` | | `1.4.7.0`（type） |
| `Implem.Pleasanter.Models.Results.ResultBulkUpsertApiModel#SiteId` | `1.4.6.0` | | |
| `Implem.Pleasanter.Models.Shared._BaseApiModel#SsCache` | `1.5.6.0` | | |
| `Implem.Pleasanter.Models.SiteApiModel#AnalyGuide` | `1.4.1.0` | | |
| `Implem.Pleasanter.Models.SiteApiModel#EnableSsCache` | `1.5.6.0` | | |
| `Implem.Pleasanter.Models.SiteApiModel#FindSiteNames` | `1.4.5.0` | | |
| `Implem.Pleasanter.Models.SiteApiModel#Form` | `1.4.23.0` | | |
| `Implem.Pleasanter.Models.SuspendTenantApiModel#ApiKey` | `1.5.7.0` | | |
| `Implem.Pleasanter.Models.SuspendTenantApiModel#SuspendDate` | `1.5.7.0` | | |
| `Implem.Pleasanter.Models.UserApiModel#AllowMovingFromTopSite` | `1.4.8.0` | | |
| `Implem.Pleasanter.Models.UserApiModel#EnableSecretKey` | `1.4.2.0` | | |
| `Implem.Pleasanter.Models.UserApiModel#LoginExpirationLimit` | `1.4.13.0` | | |
| `Implem.Pleasanter.Models.UserApiModel#LoginExpirationPeriod` | `1.4.13.0` | | |
| `Implem.Pleasanter.Models.UserApiModel#Manager` | `1.4.13.0` | | |
| `Implem.Pleasanter.Models.UserApiModel#ScimExternalId` | `1.5.8.0` | | |
| `Implem.Pleasanter.Models.UserApiModel#ScimId` | `1.5.8.0` | | |
| `Implem.Pleasanter.Models.UserApiModel#ScimSync` | `1.5.8.0` | | |
| `Implem.Pleasanter.Models.UserApiModel#SecretKey` | `1.4.2.0` | | |
| `Implem.Pleasanter.Models.UserApiModel#UiBackgroundColor` | `1.5.8.0` | | |
| `Implem.Pleasanter.Models.UserApiModel#UiColorScheme` | `1.5.8.0` | | |
| `Implem.Pleasanter.Models.UserApiModel#UiMainColor` | `1.5.8.0` | | |
| `Implem.Pleasanter.Models.UserApiModel#UiSubColor` | `1.5.8.0` | | |
| `Implem.Pleasanter.Models.UserApiModel#UiType` | `1.5.8.0` | | |
| `Implem.Pleasanter.Models.WikiApiModel#MigrationMode` | `1.4.16.0` | | |

### 列挙値

| 項目 | Since | Until | 変更 |
| --- | --- | --- | --- |
| `Implem.Pleasanter.Libraries.Models.Versions.AfterCreateActionTypes#Default` | `1.4.23.0` | | |
| `Implem.Pleasanter.Libraries.Models.Versions.AfterCreateActionTypes#OpenNewEditor` | `1.4.23.0` | | |
| `Implem.Pleasanter.Libraries.Models.Versions.AfterCreateActionTypes#ReturnToList` | `1.4.23.0` | | |
| `Implem.Pleasanter.Libraries.Models.Versions.AfterUpdateActionTypes#Default` | `1.4.23.0` | | |
| `Implem.Pleasanter.Libraries.Models.Versions.AfterUpdateActionTypes#MoveToNextRecord` | `1.4.23.0` | | |
| `Implem.Pleasanter.Libraries.Models.Versions.AfterUpdateActionTypes#ReturnToList` | `1.4.23.0` | | |
| `Implem.Pleasanter.Libraries.ServerScripts.ServerScriptModelResponse.Types#Filter` | `1.5.6.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DashboardPartType#Index` | `1.4.1.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DataChange.Types#InputValueFormula` | `1.4.10.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Notification.Types#LineWorks` | `1.5.1.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Process.AfterProcessStatusChangeActionTypes#Default` | `1.4.23.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Process.AfterProcessStatusChangeActionTypes#ReturnToList` | `1.4.23.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Process.ExecutionTypes#AddedButtonOrCreateOrUpdate` | `1.4.11.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Reminder.ReminderTypes#LineWorks` | `1.5.1.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings.StandardExportTypes#Default` | `1.5.2.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings.StandardExportTypes#Editor` | `1.5.2.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings.StandardExportTypes#Grid` | `1.5.2.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings.TextAlignTypes#Center` | `1.4.1.0` | | |

### API の既定値（Parameters/Api.json）

なし。

### 規約ルーティング（Startup の MapControllerRoute）

| 項目 | Since | Until | 変更 |
| --- | --- | --- | --- |
| `FormBinaries` | `1.4.23.0` | | |

## 全版で変わらない項目の数

| 分類 | 件数 |
| --- | --- |
| enum | 165 |
| member | 1868 |
| parameter | 5 |
| route | 5 |
| type | 112 |
