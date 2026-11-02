# Pleasanter API 面の版差

<!-- このファイルは tools/VehicleVision.PleasanterTools.DotNetClient.ApiSurface が生成する。手で編集しない。 -->

手順・読み方・限界は [API 面の抽出](../../_documents/API面の抽出.md) にある。機械可読の版は `history.json`、最新の版の全体は `latest.json`。

- 対象: 159 版（`Pleasanter_1.3.0.0` 〜 `Pleasanter_1.5.9.0`）
- **Since** が `1.3.0.0` のものは「その版以前から在る」の意味。**Until** は「その版から無い」の意味
- Pleasanter のソースからの機械抽出であり、**実機の Pleasanter では確かめていない**

## 版ごとの件数

| 版 | エンドポイント | 型 | メンバー・列挙値 |
| --- | --- | --- | --- |
| `1.3.0.0` | 35 | 43 | 1105 |
| `1.3.0.1` | 35 | 43 | 1105 |
| `1.3.1.0` | 35 | 43 | 1108 |
| `1.3.1.1` | 35 | 43 | 1108 |
| `1.3.1.2` | 35 | 43 | 1108 |
| `1.3.2.0` | 35 | 43 | 1111 |
| `1.3.2.1` | 35 | 43 | 1111 |
| `1.3.3.0` | 35 | 43 | 1113 |
| `1.3.3.1` | 35 | 43 | 1113 |
| `1.3.3.2` | 35 | 43 | 1113 |
| `1.3.3.3` | 35 | 43 | 1113 |
| `1.3.4.0` | 39 | 81 | 1673 |
| `1.3.4.1` | 39 | 81 | 1673 |
| `1.3.4.2` | 39 | 81 | 1673 |
| `1.3.5.0` | 39 | 82 | 1681 |
| `1.3.5.1` | 39 | 82 | 1681 |
| `1.3.6.0` | 39 | 82 | 1682 |
| `1.3.7.0` | 39 | 82 | 1682 |
| `1.3.7.1` | 39 | 82 | 1682 |
| `1.3.8.0` | 39 | 82 | 1683 |
| `1.3.9.0` | 39 | 82 | 1683 |
| `1.3.9.1` | 39 | 82 | 1683 |
| `1.3.10.0` | 40 | 83 | 1694 |
| `1.3.11.0` | 40 | 83 | 1699 |
| `1.3.11.1` | 40 | 83 | 1699 |
| `1.3.11.2` | 40 | 83 | 1699 |
| `1.3.12.0` | 40 | 87 | 1726 |
| `1.3.13.0` | 40 | 89 | 1738 |
| `1.3.14.0` | 40 | 90 | 1741 |
| `1.3.15.0` | 40 | 90 | 1743 |
| `1.3.15.1` | 40 | 90 | 1743 |
| `1.3.16.0` | 40 | 90 | 1747 |
| `1.3.16.1` | 40 | 90 | 1747 |
| `1.3.16.2` | 40 | 90 | 1747 |
| `1.3.16.3` | 40 | 90 | 1747 |
| `1.3.17.0` | 40 | 91 | 1757 |
| `1.3.17.1` | 40 | 91 | 1757 |
| `1.3.18.0` | 40 | 91 | 1758 |
| `1.3.19.0` | 40 | 91 | 1758 |
| `1.3.20.0` | 40 | 91 | 1758 |
| `1.3.21.0` | 40 | 91 | 1759 |
| `1.3.22.0` | 40 | 91 | 1760 |
| `1.3.23.0` | 40 | 91 | 1760 |
| `1.3.24.0` | 40 | 91 | 1760 |
| `1.3.25.0` | 43 | 91 | 1762 |
| `1.3.25.1` | 43 | 91 | 1762 |
| `1.3.25.2` | 43 | 91 | 1762 |
| `1.3.25.3` | 43 | 91 | 1762 |
| `1.3.26.0` | 43 | 92 | 1769 |
| `1.3.26.1` | 43 | 92 | 1769 |
| `1.3.27.0` | 43 | 92 | 1769 |
| `1.3.27.1` | 43 | 92 | 1770 |
| `1.3.28.0` | 43 | 92 | 1771 |
| `1.3.29.0` | 43 | 92 | 1774 |
| `1.3.29.1` | 43 | 92 | 1774 |
| `1.3.29.2` | 43 | 92 | 1774 |
| `1.3.30.0` | 44 | 92 | 1775 |
| `1.3.30.1` | 44 | 92 | 1775 |
| `1.3.31.0` | 44 | 92 | 1775 |
| `1.3.32.0` | 46 | 93 | 1785 |
| `1.3.33.0` | 47 | 93 | 1786 |
| `1.3.34.0` | 47 | 93 | 1787 |
| `1.3.35.0` | 47 | 93 | 1787 |
| `1.3.35.1` | 47 | 93 | 1787 |
| `1.3.36.0` | 47 | 93 | 1789 |
| `1.3.37.0` | 47 | 93 | 1791 |
| `1.3.37.1` | 47 | 93 | 1791 |
| `1.3.37.2` | 47 | 93 | 1791 |
| `1.3.37.3` | 47 | 93 | 1791 |
| `1.3.38.0` | 47 | 94 | 1803 |
| `1.3.38.1` | 47 | 94 | 1803 |
| `1.3.38.2` | 47 | 94 | 1803 |
| `1.3.38.3` | 47 | 94 | 1803 |
| `1.3.39.0` | 47 | 94 | 1805 |
| `1.3.39.1` | 47 | 94 | 1805 |
| `1.3.39.2` | 47 | 94 | 1805 |
| `1.3.40.0` | 47 | 94 | 1806 |
| `1.3.40.1` | 47 | 94 | 1806 |
| `1.3.40.2` | 47 | 94 | 1806 |
| `1.3.41.0` | 47 | 94 | 1806 |
| `1.3.42.0` | 47 | 94 | 1807 |
| `1.3.42.1` | 47 | 94 | 1807 |
| `1.3.43.0` | 47 | 94 | 1807 |
| `1.3.44.0` | 47 | 99 | 1847 |
| `1.3.45.0` | 47 | 101 | 1868 |
| `1.3.46.0` | 47 | 102 | 1874 |
| `1.3.46.1` | 47 | 102 | 1874 |
| `1.3.47.0` | 48 | 103 | 1877 |
| `1.3.48.0` | 51 | 103 | 1878 |
| `1.3.49.0` | 51 | 103 | 1878 |
| `1.3.50.0` | 52 | 109 | 1981 |
| `1.3.50.1` | 52 | 109 | 1981 |
| `1.3.50.2` | 52 | 109 | 1981 |
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
| `POST api/backgroundtasks/rebuildsearchindexes` | api | `1.3.32.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST api/backgroundtasks/{id}/rebuildsearchindexes` | api | `1.3.32.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST api/binaries/upload` | api | `1.3.0.0` | | `1.3.4.0`（requestTypes）; `1.3.6.0`（input）; `1.3.39.0`（requestTypes, response）; `1.5.6.0`（attributes, rateLimiting） |
| `POST api/binaries/{guid}/get` | api | `1.3.0.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST api/binaries/{guid}/getstream` | api | `1.3.0.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST api/binaries/{guid}/upload` | api | `1.3.0.0` | | `1.3.4.0`（requestTypes）; `1.3.6.0`（input）; `1.3.39.0`（requestTypes, response）; `1.5.6.0`（attributes, rateLimiting） |
| `POST api/demo/register` | api | `1.3.30.0` | | `1.5.6.0`（attributes, rateLimiting）; `1.5.7.0`（gates） |
| `POST api/depts/create` | api | `1.3.25.0` | | `1.3.39.0`（requestTypes）; `1.5.6.0`（attributes, rateLimiting） |
| `POST api/depts/get` | api | `1.3.0.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST api/depts/import` | api | `1.3.48.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST api/depts/{id}/delete` | api | `1.3.25.0` | | `1.3.39.0`（requestTypes）; `1.5.6.0`（attributes, rateLimiting） |
| `POST api/depts/{id}/get` | api | `1.3.0.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST api/depts/{id}/update` | api | `1.3.25.0` | | `1.3.39.0`（requestTypes）; `1.5.6.0`（attributes, rateLimiting） |
| `POST api/extended/sql` | api | `1.3.0.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST api/extensions/create` | api | `1.4.13.0` | | `1.4.16.0`（auth, response）; `1.5.6.0`（attributes, rateLimiting） |
| `POST api/extensions/get` | api | `1.4.13.0` | | `1.4.16.0`（auth, response）; `1.5.6.0`（attributes, rateLimiting） |
| `POST api/extensions/{id}/delete` | api | `1.4.13.0` | | `1.4.16.0`（auth, response）; `1.5.6.0`（attributes, rateLimiting） |
| `POST api/extensions/{id}/get` | api | `1.4.13.0` | | `1.4.16.0`（auth, response）; `1.5.6.0`（attributes, rateLimiting） |
| `POST api/extensions/{id}/update` | api | `1.4.13.0` | | `1.4.16.0`（auth, response）; `1.5.6.0`（attributes, rateLimiting） |
| `POST api/groups/create` | api | `1.3.0.0` | | `1.3.39.0`（requestTypes）; `1.5.6.0`（attributes, rateLimiting） |
| `POST api/groups/get` | api | `1.3.0.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST api/groups/import` | api | `1.3.48.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST api/groups/{id}/delete` | api | `1.3.0.0` | | `1.3.39.0`（requestTypes）; `1.5.6.0`（attributes, rateLimiting） |
| `POST api/groups/{id}/get` | api | `1.3.0.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST api/groups/{id}/update` | api | `1.3.0.0` | | `1.3.39.0`（requestTypes）; `1.5.6.0`（attributes, rateLimiting） |
| `POST api/items/{id}/bulkdelete` | api | `1.3.0.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST api/items/{id}/bulkupsert` | api | `1.4.6.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST api/items/{id}/copysitepackage` | api | `1.3.0.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST api/items/{id}/create` | api | `1.3.0.0` | | `1.3.39.0`（requestTypes, response）; `1.5.6.0`（attributes, rateLimiting） |
| `POST api/items/{id}/createsite` | api | `1.3.4.0` | | `1.3.39.0`（requestTypes, response）; `1.5.6.0`（attributes, rateLimiting） |
| `POST api/items/{id}/delete` | api | `1.3.0.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST api/items/{id}/deletesite` | api | `1.3.4.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST api/items/{id}/export` | api | `1.3.0.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST api/items/{id}/get` | api | `1.3.0.0` | | `1.3.4.0`（response）; `1.3.38.0`（response）; `1.5.6.0`（attributes, rateLimiting） |
| `POST api/items/{id}/getclosestsiteid` | api | `1.4.5.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST api/items/{id}/getsite` | api | `1.3.4.0` | | `1.3.38.0`（response）; `1.5.6.0`（attributes, rateLimiting） |
| `POST api/items/{id}/import` | api | `1.3.47.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST api/items/{id}/movesite` | api | `1.5.9.0` | | |
| `POST api/items/{id}/synchronizesummaries` | api | `1.3.33.0` | | `1.4.12.0`（response）; `1.5.6.0`（attributes, rateLimiting） |
| `POST api/items/{id}/update` | api | `1.3.0.0` | | `1.3.4.0`（requestTypes）; `1.3.39.0`（requestTypes, response）; `1.5.6.0`（attributes, rateLimiting） |
| `POST api/items/{id}/updatesite` | api | `1.3.4.0` | | `1.3.39.0`（requestTypes, response）; `1.5.6.0`（attributes, rateLimiting） |
| `POST api/items/{id}/updatesitesettings` | api | `1.3.50.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST api/items/{id}/upsert` | api | `1.3.10.0` | | `1.3.14.0`（requestTypes）; `1.3.39.0`（response）; `1.5.6.0`（attributes, rateLimiting） |
| `POST api/sessions/delete` | api | `1.3.0.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST api/sessions/get` | api | `1.3.0.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST api/sessions/set` | api | `1.3.0.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST api/tenants/create` | api | `1.5.7.0` | | |
| `POST api/tenants/get` | api | `1.5.7.0` | | |
| `POST api/tenants/{id}/delete` | api | `1.5.7.0` | | |
| `POST api/tenants/{id}/resume` | api | `1.5.7.0` | | |
| `POST api/tenants/{id}/suspend` | api | `1.5.7.0` | | |
| `POST api/users/create` | api | `1.3.0.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST api/users/get` | api | `1.3.0.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST api/users/import` | api | `1.3.48.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST api/users/{id}/delete` | api | `1.3.0.0` | | `1.3.39.0`（requestTypes）; `1.5.6.0`（attributes, rateLimiting） |
| `POST api/users/{id}/get` | api | `1.3.0.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST api/users/{id}/update` | api | `1.3.0.0` | | `1.3.39.0`（requestTypes）; `1.5.6.0`（attributes, rateLimiting） |
| `POST api/utility/getlicenseinfo` | api | `1.4.13.0` | | `1.4.21.0`（gates, requestTypes）; `1.5.6.0`（attributes, rateLimiting） |
| `POST api/{reference}/{id}/{controller}/send` | api | `1.3.0.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST legacy:Api_Depts/Get` | legacy | `1.3.0.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST legacy:Api_Groups/Get` | legacy | `1.3.0.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST legacy:Api_Items/Create` | legacy | `1.3.0.0` | | `1.3.39.0`（requestTypes, response）; `1.5.6.0`（attributes, rateLimiting） |
| `POST legacy:Api_Items/Delete` | legacy | `1.3.0.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST legacy:Api_Items/Get` | legacy | `1.3.0.0` | | `1.3.4.0`（response）; `1.3.38.0`（response）; `1.5.6.0`（attributes, rateLimiting） |
| `POST legacy:Api_Items/Update` | legacy | `1.3.0.0` | | `1.3.4.0`（requestTypes）; `1.3.39.0`（requestTypes, response）; `1.5.6.0`（attributes, rateLimiting） |
| `POST legacy:Api_Users/Get` | legacy | `1.3.0.0` | | `1.5.6.0`（attributes, rateLimiting） |
| `POST scim/v2/groups` | scim | `1.5.8.0` | | |
| `POST scim/v2/users` | scim | `1.5.8.0` | | |
| `PUT scim/v2/groups/{id}` | scim | `1.5.8.0` | | |
| `PUT scim/v2/users/{id}` | scim | `1.5.8.0` | | |

## 途中の版で増減・変化した項目

全版（`1.3.0.0` 〜 `1.5.9.0`）で在り、中身も変わらない項目は省き、件数だけを末尾に書く。

### 型

| 項目 | Since | Until | 変更 |
| --- | --- | --- | --- |
| `Implem.Pleasanter.Libraries.License.LicenseUtilities.LicenseInfoApi` | `1.4.21.0` | | |
| `Implem.Pleasanter.Libraries.Models.Versions.AfterCreateActionTypes` | `1.4.23.0` | | |
| `Implem.Pleasanter.Libraries.Models.Versions.AfterUpdateActionTypes` | `1.4.23.0` | | |
| `Implem.Pleasanter.Libraries.Models.Versions.AutoVerUpTypes` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Requests.ImportApi` | `1.3.47.0` | | |
| `Implem.Pleasanter.Libraries.Security.Permissions.Types` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Server.Times.RepeatTypes` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.ServerScripts.ServerScriptElements` | `1.5.6.0` | | |
| `Implem.Pleasanter.Libraries.ServerScripts.ServerScriptModel.ServerScriptModelColumn` | `1.5.6.0` | | |
| `Implem.Pleasanter.Libraries.ServerScripts.ServerScriptModel.ServerScriptModelRow` | `1.5.6.0` | | |
| `Implem.Pleasanter.Libraries.ServerScripts.ServerScriptModelResponse` | `1.5.6.0` | | |
| `Implem.Pleasanter.Libraries.ServerScripts.ServerScriptModelResponse.Types` | `1.5.6.0` | | |
| `Implem.Pleasanter.Libraries.ServerScripts.ServerScriptModelResponses` | `1.5.6.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Aggregation` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Aggregation.Types` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.AiProvider` | `1.5.8.0` | | |
| `Implem.Pleasanter.Libraries.Settings.AnalyPartSetting` | `1.4.1.0` | | |
| `Implem.Pleasanter.Libraries.Settings.AutoNumbering` | `1.3.32.0` | | |
| `Implem.Pleasanter.Libraries.Settings.BulkUpdateColumn` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.BulkUpdateColumnDetail` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column.AutoNumberingResetTypes` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column.FullTextTypes` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column.ViewerSwitchingTypes` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.ColumnAccessControl` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.ColumnUtilities.CheckFilterControlTypes` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.ColumnUtilities.DateFilterSetMode` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DashboardPart` | `1.3.44.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DashboardPartLayout` | `1.4.1.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DashboardPartType` | `1.3.44.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DataChange` | `1.3.13.0` | | `1.4.0.0`（attributes） |
| `Implem.Pleasanter.Libraries.Settings.DataChange.Types` | `1.3.13.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Formula` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Formula.OperatorTypes` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.FormulaSet` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Html` | `1.3.45.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Html.PositionTypes` | `1.3.45.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Notification` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Notification.Expressions` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Notification.MethodTypes` | `1.3.17.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Notification.Types` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Process` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Process.ActionTypes` | `1.3.12.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Process.AfterProcessStatusChangeActionTypes` | `1.4.23.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Process.ExecutionTypes` | `1.3.14.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Process.ScreenTypes` | `1.3.5.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Process.ValidationTypes` | `1.3.12.0` | | |
| `Implem.Pleasanter.Libraries.Settings.QuickAccessLayout` | `1.3.44.0` | | |
| `Implem.Pleasanter.Libraries.Settings.QuickAccessSite` | `1.3.44.0` | | |
| `Implem.Pleasanter.Libraries.Settings.RelatingColumn` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Reminder` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Reminder.ReminderTypes` | `1.3.10.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Script` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Section` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.ServerScript` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SettingList` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings.CalendarTypes` | `1.3.46.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings.GridEditorTypes` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings.RoundingTypes` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings.SaveViewTypes` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings.SearchTypes` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings.StandardExportTypes` | `1.5.2.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings.TextAlignTypes` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.StatusControl` | `1.3.12.0` | | |
| `Implem.Pleasanter.Libraries.Settings.StatusControl.ControlConstraintsTypes` | `1.3.12.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Style` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Summary` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Tab` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.TimeLineDisplayType` | `1.3.44.0` | | |
| `Implem.Pleasanter.Libraries.Settings.ValidateInput` | `1.3.4.0` | | `1.4.0.0`（attributes） |
| `Implem.Pleasanter.Models.ApiSiteSettingPermission` | `1.4.0.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ColumnApiSettingModel` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.DragParamsApiSettingModel` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.DragStateApiSettingModel` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.HtmlApiSettingModel` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ISiteSettingBaseProperties` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.LinkApiSettingModel` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ProcessApiSettingModel` | `1.4.0.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ScriptApiSettingModel` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.SectionApiSettingModel` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ServerScriptApiSettingModel` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.SiteSettingsApiModel` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.StatusControlApiSettingModel` | `1.4.0.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.StyleApiSettingModel` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.CreateTenantApiModel` | `1.5.7.0` | | |
| `Implem.Pleasanter.Models.ExtensionApiModel` | `1.4.13.0` | | |
| `Implem.Pleasanter.Models.Issues.IssueBulkUpsertApiModel` | `1.4.6.0` | | |
| `Implem.Pleasanter.Models.Results.ResultBulkUpsertApiModel` | `1.4.6.0` | | |
| `Implem.Pleasanter.Models.Shared._ImageApiModel` | `1.3.26.0` | | |
| `Implem.Pleasanter.Models.SiteApiModel` | `1.3.4.0` | | |
| `Implem.Pleasanter.Models.SuspendTenantApiModel` | `1.5.7.0` | | |
| `Implem.Pleasanter.Models.WikiApiModel` | `1.3.38.0` | | |

### メンバー（プロパティ・フィールド）

| 項目 | Since | Until | 変更 |
| --- | --- | --- | --- |
| `Implem.Pleasanter.Libraries.DataTypes.Attachment#Extension` | `1.4.7.0` | | |
| `Implem.Pleasanter.Libraries.DataTypes.Attachment#Extention` | `1.3.0.0` | `1.4.7.0` | |
| `Implem.Pleasanter.Libraries.License.LicenseUtilities.LicenseInfoApi#AppTypes` | `1.4.21.0` | | |
| `Implem.Pleasanter.Libraries.Requests.Api#ApiVersion` | `1.3.0.0` | | `1.3.13.0`（default） |
| `Implem.Pleasanter.Libraries.Requests.Api#Keys` | `1.3.10.0` | | |
| `Implem.Pleasanter.Libraries.Requests.Api#PageSize` | `1.4.13.0` | | |
| `Implem.Pleasanter.Libraries.Requests.Api#SsCache` | `1.5.6.0` | | |
| `Implem.Pleasanter.Libraries.Requests.ImportApi#Encoding` | `1.3.47.0` | | |
| `Implem.Pleasanter.Libraries.Requests.ImportApi#Key` | `1.3.47.0` | | |
| `Implem.Pleasanter.Libraries.Requests.ImportApi#MigrationMode` | `1.4.16.0` | | |
| `Implem.Pleasanter.Libraries.Requests.ImportApi#RejectNullImport` | `1.4.15.0` | | |
| `Implem.Pleasanter.Libraries.Requests.ImportApi#ReplaceAllGroupMembers` | `1.3.48.0` | | |
| `Implem.Pleasanter.Libraries.Requests.ImportApi#UpdatableImport` | `1.3.47.0` | | |
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
| `Implem.Pleasanter.Libraries.Settings.Aggregation#Data` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Aggregation#GroupBy` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Aggregation#Id` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Aggregation#Target` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Aggregation#Type` | `1.3.4.0` | | |
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
| `Implem.Pleasanter.Libraries.Settings.AutoNumbering#ColumnName` | `1.3.32.0` | | |
| `Implem.Pleasanter.Libraries.Settings.AutoNumbering#Default` | `1.3.32.0` | | |
| `Implem.Pleasanter.Libraries.Settings.AutoNumbering#Format` | `1.3.32.0` | | |
| `Implem.Pleasanter.Libraries.Settings.AutoNumbering#ResetType` | `1.3.32.0` | | |
| `Implem.Pleasanter.Libraries.Settings.AutoNumbering#Step` | `1.3.32.0` | | |
| `Implem.Pleasanter.Libraries.Settings.BulkUpdateColumn#Columns` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.BulkUpdateColumn#Details` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.BulkUpdateColumn#Id` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.BulkUpdateColumn#Title` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.BulkUpdateColumnDetail#DefaultInput` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.BulkUpdateColumnDetail#EditorReadOnly` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.BulkUpdateColumnDetail#ValidateRequired` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#AddChoiceHashByServerScript` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#AddSource` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#After` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#Aggregatable` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#AllowBulkUpdate` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#AllowDeleteAttachments` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#AllowImage` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#Anchor` | `1.3.12.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#AnchorFormat` | `1.3.12.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#AutoNumberingDefault` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#AutoNumberingFormat` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#AutoNumberingResetType` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#AutoNumberingStep` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#AutoPostBack` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#BinaryStorageProvider` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#CanCreateCache` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#CanReadCache` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#CanUpdateCache` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#CellSticky` | `1.4.20.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#CellWidth` | `1.4.20.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#CellWordWrap` | `1.4.22.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#CheckFilterControlType` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#ChoiceHash` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#ChoiceValueHash` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#ChoicesControlType` | `1.3.28.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#ChoicesText` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#ClientRegexValidation` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#ColumnName` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#ColumnsReturnedWhenAutomaticPostback` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#Computable` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#ControlCss` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#ControlDateTime` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#ControlFormat` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#ControlType` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#CopyByDefault` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#DateFilterFy` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#DateFilterHalf` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#DateFilterMaxSpan` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#DateFilterMinSpan` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#DateFilterMonth` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#DateFilterQuarter` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#DateFilterSetMode` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#DateTimeStep` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#DbNullable` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#DecimalPlaces` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#DefaultInput` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#DefaultMaxValue` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#DefaultMinValue` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#DefaultNotNull` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#Description` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#EditSelf` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#EditorColumn` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#EditorFormat` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#EditorReadOnly` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#EditorVisible` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#ExcludeMe` | `1.3.37.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#Export` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#ExportFormat` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#ExtendedCellCss` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#ExtendedControlCss` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#ExtendedFieldCss` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#ExtendedHtmlAfterControl` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#ExtendedHtmlAfterField` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#ExtendedHtmlBeforeField` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#ExtendedHtmlBeforeLabel` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#ExtendedHtmlBetweenLabelAndControl` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#FieldCss` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#FilterColumn` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#FilterVisible` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#FloatClear` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#Format` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#FullTextType` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#GridColumn` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#GridDateTime` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#GridDesign` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#GridFormat` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#GridLabelText` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#GridStyle` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#GridVisible` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#Hash` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#Hide` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#HistoryColumn` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#HistoryVisible` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#Id` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#Id_Ver` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#ImportKey` | `1.3.11.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#InputGuide` | `1.3.36.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#JoinTableName` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#Joined` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#LabelText` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#LabelTextDefault` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#LimitQuantity` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#LimitSize` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#Link` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#LinkColumn` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#LinkVisible` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#LinkedTitleHash` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#Linking` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#LocalFolderLimitSize` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#LocalFolderTotalLimitSize` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#LowSchemaVersion` | `1.3.42.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#Max` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#MaxLength` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#MessageWhenDuplicated` | `1.3.8.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#Min` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#MultilingualLabelText` | `1.4.23.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#MultipleSelections` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#Name` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#No` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#NoDuplication` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#NoWrap` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#NotDeleteExistHistory` | `1.3.37.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#NotEditorSettings` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#NotInsertBlankChoice` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#NotSelect` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#NotUpdate` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#Nullable` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#NumFilterMax` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#NumFilterMin` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#NumFilterStep` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#OpenAnchorNewTab` | `1.3.32.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#OutputType` | `1.3.16.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#OverwriteSameFileName` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#RecordedTime` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#RecordingFormat` | `1.4.16.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#RegexValidationMessage` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#Required` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#RoundingType` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#SearchType` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#Section` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#ServerRegexValidation` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#ServerScriptModelColumn` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#SiteId` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#SiteSettings` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#Size` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#SqlParam` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#StatusReadOnly` | `1.3.32.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#Step` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#StringFormat` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#TableAlias` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#TextAlign` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#ThumbnailLimitSize` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#TitleColumn` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#TitleVisible` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#TotalCount` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#TotalLimitSize` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#Type` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#TypeCs` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#TypeName` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#Unit` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#UnitDefault` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#UseSearch` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#ValidateDate` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#ValidateEmail` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#ValidateEqualTo` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#ValidateMaxLength` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#ValidateNumber` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#ValidateRequired` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#ViewerSwitchingType` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column#Width` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.ColumnAccessControl#AllowedUsers` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.ColumnAccessControl#ColumnName` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.ColumnAccessControl#Depts` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.ColumnAccessControl#Groups` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.ColumnAccessControl#No` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.ColumnAccessControl#RecordUsers` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.ColumnAccessControl#Type` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.ColumnAccessControl#Users` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DashboardPart#CalendarFromTo` | `1.3.50.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DashboardPart#CalendarGroupBy` | `1.3.50.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DashboardPart#CalendarShowStatus` | `1.3.50.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DashboardPart#CalendarSites` | `1.3.50.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DashboardPart#CalendarSitesData` | `1.3.50.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DashboardPart#CalendarTimePeriod` | `1.3.50.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DashboardPart#CalendarType` | `1.3.50.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DashboardPart#Content` | `1.3.44.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DashboardPart#Depts` | `1.3.44.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DashboardPart#DisableAsynchronousLoading` | `1.3.50.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DashboardPart#ExtendedCss` | `1.3.44.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DashboardPart#Groups` | `1.3.44.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DashboardPart#Height` | `1.3.44.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DashboardPart#HtmlContent` | `1.3.44.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DashboardPart#Id` | `1.3.44.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DashboardPart#IndexSites` | `1.4.1.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DashboardPart#IndexSitesData` | `1.4.1.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DashboardPart#KambanAggregateType` | `1.4.0.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DashboardPart#KambanAggregationView` | `1.4.0.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DashboardPart#KambanColumns` | `1.4.0.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DashboardPart#KambanGroupByX` | `1.4.0.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DashboardPart#KambanGroupByY` | `1.4.0.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DashboardPart#KambanShowStatus` | `1.4.0.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DashboardPart#KambanSites` | `1.4.0.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DashboardPart#KambanSitesData` | `1.4.0.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DashboardPart#KambanValue` | `1.4.0.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DashboardPart#QuickAccessLayout` | `1.3.44.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DashboardPart#QuickAccessSites` | `1.3.44.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DashboardPart#QuickAccessSitesData` | `1.3.44.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DashboardPart#ShowTitle` | `1.3.44.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DashboardPart#SiteId` | `1.3.44.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DashboardPart#TimeLineBody` | `1.3.44.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DashboardPart#TimeLineDisplayType` | `1.3.44.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DashboardPart#TimeLineItemCount` | `1.3.44.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DashboardPart#TimeLineSites` | `1.3.44.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DashboardPart#TimeLineSitesData` | `1.3.44.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DashboardPart#TimeLineTitle` | `1.3.44.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DashboardPart#Title` | `1.3.44.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DashboardPart#Type` | `1.3.44.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DashboardPart#Users` | `1.3.44.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DashboardPart#View` | `1.3.44.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DashboardPart#Width` | `1.3.44.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DashboardPart#X` | `1.3.44.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DashboardPart#Y` | `1.3.44.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DashboardPartLayout#Content` | `1.4.1.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DashboardPartLayout#H` | `1.4.1.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DashboardPartLayout#Id` | `1.4.1.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DashboardPartLayout#W` | `1.4.1.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DashboardPartLayout#X` | `1.4.1.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DashboardPartLayout#Y` | `1.4.1.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DataChange#BaseDateTime` | `1.3.15.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DataChange#ColumnName` | `1.3.13.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DataChange#Delete` | `1.4.0.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DataChange#Id` | `1.3.13.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DataChange#Type` | `1.3.13.0` | | `1.4.0.0`（nullable, type） |
| `Implem.Pleasanter.Libraries.Settings.DataChange#Value` | `1.3.13.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DataChange#ValueFormulaIsDisplayError` | `1.4.10.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DataChange#ValueFormulaNotUseDisplayName` | `1.4.10.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Export#Depts` | `1.3.1.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Export#ExportCommentsJsonFormat` | `1.4.18.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Export#Groups` | `1.3.1.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Export#Users` | `1.3.1.0` | | |
| `Implem.Pleasanter.Libraries.Settings.ExportColumn#ExportJsonFormat` | `1.4.18.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Formula#Children` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Formula#ColumnName` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Formula#OperatorType` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Formula#RawValue` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.FormulaSet#CalculationMethod` | `1.3.50.0` | | `1.3.50.1`（default） |
| `Implem.Pleasanter.Libraries.Settings.FormulaSet#Condition` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.FormulaSet#Disabled` | `1.5.0.0` | | |
| `Implem.Pleasanter.Libraries.Settings.FormulaSet#Formula` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.FormulaSet#FormulaMapping` | `1.3.50.0` | | |
| `Implem.Pleasanter.Libraries.Settings.FormulaSet#FormulaScript` | `1.3.50.0` | | |
| `Implem.Pleasanter.Libraries.Settings.FormulaSet#FormulaScriptOutOfCondition` | `1.4.11.0` | | |
| `Implem.Pleasanter.Libraries.Settings.FormulaSet#Id` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.FormulaSet#IsDisplayError` | `1.3.50.0` | | |
| `Implem.Pleasanter.Libraries.Settings.FormulaSet#NotUseDisplayName` | `1.3.50.0` | | |
| `Implem.Pleasanter.Libraries.Settings.FormulaSet#OutOfCondition` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.FormulaSet#Target` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Html#All` | `1.3.45.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Html#Analy` | `1.4.1.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Html#Body` | `1.3.45.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Html#BurnDown` | `1.3.45.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Html#Calendar` | `1.3.45.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Html#Crosstab` | `1.3.45.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Html#Disabled` | `1.3.45.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Html#DraftKey` | `1.5.7.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Html#DraftOutputMode` | `1.5.7.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Html#Edit` | `1.3.45.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Html#Gantt` | `1.3.45.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Html#Id` | `1.3.45.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Html#ImageLib` | `1.3.45.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Html#Index` | `1.3.45.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Html#Kamban` | `1.3.45.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Html#New` | `1.3.45.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Html#PositionType` | `1.3.45.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Html#TimeSeries` | `1.3.45.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Html#Title` | `1.3.45.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Link#ExcludeMe` | `1.3.30.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Link#NotReturnParentRecord` | `1.4.10.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Link#Priority` | `1.3.40.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Link#SelectNewLink` | `1.3.36.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Lookup#Overwrite` | `1.3.10.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Lookup#OverwriteForm` | `1.3.15.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Notification#Address` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Notification#AfterBulkDelete` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Notification#AfterBulkUpdate` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Notification#AfterCondition` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Notification#AfterCopy` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Notification#AfterCreate` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Notification#AfterDelete` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Notification#AfterImport` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Notification#AfterUpdate` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Notification#BccAddress` | `1.4.10.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Notification#BeforeCondition` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Notification#Body` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Notification#CcAddress` | `1.4.10.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Notification#Delete` | `1.4.0.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Notification#Disabled` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Notification#Encoding` | `1.3.17.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Notification#Expression` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Notification#Format` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Notification#Headers` | `1.3.17.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Notification#Id` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Notification#Index` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Notification#MediaType` | `1.3.17.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Notification#MethodType` | `1.3.17.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Notification#MonitorChangesColumns` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Notification#Prefix` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Notification#Subject` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Notification#Token` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Notification#Type` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Notification#UseCustomFormat` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Process#ActionType` | `1.3.12.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Process#AfterProcessStatusChangeActionType` | `1.4.23.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Process#AllowBulkProcessing` | `1.3.22.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Process#AutoNumbering` | `1.3.32.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Process#ChangedStatus` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Process#ConfirmationMessage` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Process#CurrentStatus` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Process#DataChanges` | `1.3.13.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Process#Depts` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Process#Description` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Process#Disabled` | `1.5.1.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Process#DisplayName` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Process#ErrorMessage` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Process#ExecutionType` | `1.3.14.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Process#Groups` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Process#Icon` | `1.4.11.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Process#Id` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Process#MatchConditions` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Process#Name` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Process#Notifications` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Process#OnClick` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Process#ScreenType` | `1.3.5.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Process#SuccessMessage` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Process#Tooltip` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Process#Users` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Process#ValidateInputs` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Process#ValidationType` | `1.3.12.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Process#View` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.QuickAccessSite#Css` | `1.3.44.0` | | |
| `Implem.Pleasanter.Libraries.Settings.QuickAccessSite#Icon` | `1.3.44.0` | | |
| `Implem.Pleasanter.Libraries.Settings.QuickAccessSite#Id` | `1.3.44.0` | | |
| `Implem.Pleasanter.Libraries.Settings.QuickAccessSite#OpenInNewTab` | `1.4.10.0` | | |
| `Implem.Pleasanter.Libraries.Settings.QuickAccessSite#Title` | `1.4.10.0` | | |
| `Implem.Pleasanter.Libraries.Settings.QuickAccessSite#Url` | `1.4.10.0` | | |
| `Implem.Pleasanter.Libraries.Settings.QuickAccessSite#ViewMode` | `1.4.10.0` | | |
| `Implem.Pleasanter.Libraries.Settings.RelatingColumn#Columns` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.RelatingColumn#ColumnsLinkedClass` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.RelatingColumn#Id` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.RelatingColumn#Title` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Reminder#Body` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Reminder#Column` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Reminder#Condition` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Reminder#Disabled` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Reminder#ExcludeOverdue` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Reminder#From` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Reminder#Id` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Reminder#Line` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Reminder#NotSendHyperLink` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Reminder#NotSendIfNotApplicable` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Reminder#Range` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Reminder#ReminderType` | `1.3.10.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Reminder#SendCompletedInPast` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Reminder#StartDateTime` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Reminder#Subject` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Reminder#To` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Reminder#Token` | `1.3.10.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Reminder#Type` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Script#All` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Script#Analy` | `1.4.1.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Script#Body` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Script#BurnDown` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Script#Calendar` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Script#Crosstab` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Script#Disabled` | `1.3.44.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Script#DraftKey` | `1.5.7.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Script#DraftOutputMode` | `1.5.7.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Script#Edit` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Script#Gantt` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Script#Id` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Script#ImageLib` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Script#Index` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Script#Kamban` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Script#New` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Script#TimeSeries` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Script#Title` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Section#AllowExpand` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Section#Expand` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Section#Hide` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Section#Id` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Section#LabelText` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.ServerScript#AfterBulkDelete` | `1.4.16.0` | | |
| `Implem.Pleasanter.Libraries.Settings.ServerScript#AfterCreate` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.ServerScript#AfterDelete` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.ServerScript#AfterFormula` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.ServerScript#AfterUpdate` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.ServerScript#Background` | `1.3.50.0` | | |
| `Implem.Pleasanter.Libraries.Settings.ServerScript#BeforeBulkDelete` | `1.4.16.0` | | |
| `Implem.Pleasanter.Libraries.Settings.ServerScript#BeforeCreate` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.ServerScript#BeforeDelete` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.ServerScript#BeforeFormula` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.ServerScript#BeforeOpeningPage` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.ServerScript#BeforeOpeningRow` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.ServerScript#BeforeUpdate` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.ServerScript#Body` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.ServerScript#Debug` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.ServerScript#Disabled` | `1.4.12.0` | | |
| `Implem.Pleasanter.Libraries.Settings.ServerScript#Functionalize` | `1.4.12.0` | | |
| `Implem.Pleasanter.Libraries.Settings.ServerScript#Id` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.ServerScript#Name` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.ServerScript#Shared` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.ServerScript#TimeOut` | `1.3.32.0` | | |
| `Implem.Pleasanter.Libraries.Settings.ServerScript#Title` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.ServerScript#TryCatch` | `1.4.12.0` | | |
| `Implem.Pleasanter.Libraries.Settings.ServerScript#WhenViewProcessing` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.ServerScript#WhenloadingRecord` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.ServerScript#WhenloadingSiteSettings` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#AccessStatus` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#AddressBook` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#AfterCreateActionType` | `1.4.23.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#AfterUpdateActionType` | `1.4.23.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#AggregationCollection` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#Aggregations` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#AiProviders` | `1.5.8.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#AiProvidersAllDisabled` | `1.5.8.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#AllowCopy` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#AllowEditingComments` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#AllowLockTable` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#AllowMigrationMode` | `1.4.16.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#AllowPhysicalDeleteHistories` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#AllowReferenceCopy` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#AllowRestoreHistories` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#AllowSeparate` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#AllowStandardExport` | `1.3.11.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#AllowViewReset` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#AllowedIntegratedSites` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#AlwaysRequestSearchCondition` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#AnalyGuide` | `1.4.1.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#ApiCount` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#ApiCountDate` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#AutoVerUpType` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#Body` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#BulkUpdateColumns` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#BurnDownGuide` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#CalendarGuide` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#CalendarType` | `1.3.46.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#CharToAddWhenCopying` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#ChoiceHashCache` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#ColumnAccessControlCaches` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#ColumnCollection` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#ColumnDefinitionHash` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#ColumnHash` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#Columns` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#CreateColumnAccessControls` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#CrosstabGuide` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#DashboardParts` | `1.3.44.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#DashboardPartsAsynchronousLoading` | `1.3.50.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#DefaultImportKey` | `1.3.11.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#DeleteImageWhenDeleting` | `1.3.11.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#Destinations` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#DisableLinkToEdit` | `1.3.34.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#DisableSiteConditions` | `1.4.14.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#EditInDialog` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#EditScript` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#EditStyle` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#EditorColumnHash` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#EditorColumns` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#EditorColumnsOrder` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#EditorGuide` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#EnableAnaly` | `1.4.1.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#EnableBurnDown` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#EnableCalendar` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#EnableCrosstab` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#EnableExpandLinkPath` | `1.4.12.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#EnableGantt` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#EnableImageLib` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#EnableKamban` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#EnableTimeSeries` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#Exports` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#ExtendedHeader` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#FilterColumns` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#FilterColumnsOrder` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#FirstDayOfWeek` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#FirstMonth` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#FormEndDateTime` | `1.4.23.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#FormStartDateTime` | `1.4.23.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#FormThanksMessage` | `1.4.23.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#FormUnavailableMessage` | `1.4.23.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#FormulaHash` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#Formulas` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#FormulasGetErrorDetails` | `1.5.1.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#FullTextIncludeBreadcrumb` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#FullTextIncludeSiteId` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#FullTextIncludeSiteTitle` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#FullTextNumberOfMails` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#GanttGuide` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#GeneralTabLabelText` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#GridColumns` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#GridColumnsOrder` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#GridEditorType` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#GridGuide` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#GridPageSize` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#GridScript` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#GridStyle` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#GridView` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#GuideAllowExpand` | `1.4.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#GuideExpand` | `1.4.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#HideLink` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#HistoryColumns` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#HistoryColumnsOrder` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#HistoryOnGrid` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#Htmls` | `1.3.45.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#HtmlsAllDisabled` | `1.4.14.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#ImageLibGuide` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#ImageLibPageSize` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#ImportEncoding` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#InheritPermission` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#IntegratedSites` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#ItemPermissionType` | `1.3.4.0` | `1.5.6.0` | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#JoinStacks` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#JoinedSsHash` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#KambanGuide` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#LinkCollection` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#LinkColumnSiteIdHash` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#LinkColumns` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#LinkColumnsOrder` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#LinkPageSize` | `1.4.18.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#LinkTableView` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#Linked` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#LinkedSsDataSetHash` | `1.3.4.0` | `1.3.10.0` | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#Links` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#LockedRecordTime` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#LockedRecordUser` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#LockedTableTime` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#LockedTableUser` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#MailBccDefault` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#MailCcDefault` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#MailToDefault` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#Migrated` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#MoveTargets` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#NearCompletionTimeAfterDays` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#NearCompletionTimeBeforeDays` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#NewScript` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#NewStyle` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#NoDisplayCrosstabGraph` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#NoDisplayIfReadOnly` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#NotInheritPermissionsWhenCreatingSite` | `1.4.13.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#Notifications` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#OpenEditInNewTab` | `1.4.8.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#OutputFormulaLogs` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#ParentId` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#PermissionForCreating` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#PermissionForUpdating` | `1.3.17.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#PermissionType` | `1.3.4.0` | `1.5.6.0` | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#PermissionTypeCache` | `1.3.4.0` | `1.5.6.0` | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#ProcessOutputFormulaLogs` | `1.4.10.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#Processes` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#Publish` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#ReadColumnAccessControls` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#ReferenceId` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#ReferenceType` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#RejectNullImport` | `1.4.15.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#RelatingColumns` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#Reminders` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#Responsive` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#SaveViewType` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#Scripts` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#ScriptsAllDisabled` | `1.4.14.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#SearchType` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#SectionLatestId` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#Sections` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#ServerScriptModelRowCache1` | `1.5.6.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#ServerScripts` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#ServerScriptsAllDisabled` | `1.4.14.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#ServerScriptsAndExtended` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#ServerScriptsGetErrorDetails` | `1.5.1.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#SetAllChoices` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#ShowGanttProgressRate` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#SiteId` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#Sources` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#StandardExportType` | `1.5.2.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#StatusControls` | `1.3.12.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#Styles` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#StylesAllDisabled` | `1.4.14.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#Summaries` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#SummaryCollection` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#SwitchCommandButtonsAutoPostBack` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#SwitchRecordWithAjax` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#TabLatestId` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#TableType` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#Tabs` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#TimeSeriesGuide` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#Title` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#TitleColumns` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#TitleColumnsOrder` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#TitleSeparator` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#UpdatableImport` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#UpdateColumnAccessControls` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#UseDelayFilter` | `1.3.6.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#UseFilterButton` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#UseFiltersArea` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#UseGridHeaderFilters` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#UseIncompleteFilter` | `1.3.5.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#UseNearCompletionTimeFilter` | `1.3.5.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#UseNegativeFilters` | `1.3.21.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#UseOverdueFilter` | `1.3.5.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#UseOwnFilter` | `1.3.5.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#UseRelatingColumnsOnFilter` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#UseSearchFilter` | `1.3.5.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#Version` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#ViewLatestId` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings#Views` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.StatusControl#ColumnHash` | `1.3.12.0` | | |
| `Implem.Pleasanter.Libraries.Settings.StatusControl#Delete` | `1.4.0.0` | | |
| `Implem.Pleasanter.Libraries.Settings.StatusControl#Depts` | `1.3.12.0` | | |
| `Implem.Pleasanter.Libraries.Settings.StatusControl#Description` | `1.3.12.0` | | |
| `Implem.Pleasanter.Libraries.Settings.StatusControl#Disabled` | `1.5.0.0` | | |
| `Implem.Pleasanter.Libraries.Settings.StatusControl#Groups` | `1.3.12.0` | | |
| `Implem.Pleasanter.Libraries.Settings.StatusControl#Id` | `1.3.12.0` | | |
| `Implem.Pleasanter.Libraries.Settings.StatusControl#Name` | `1.3.12.0` | | |
| `Implem.Pleasanter.Libraries.Settings.StatusControl#ReadOnly` | `1.3.12.0` | | |
| `Implem.Pleasanter.Libraries.Settings.StatusControl#Status` | `1.3.12.0` | | `1.4.0.0`（nullable, type） |
| `Implem.Pleasanter.Libraries.Settings.StatusControl#Users` | `1.3.12.0` | | |
| `Implem.Pleasanter.Libraries.Settings.StatusControl#View` | `1.3.12.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Style#All` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Style#Analy` | `1.4.1.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Style#Body` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Style#BurnDown` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Style#Calendar` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Style#Crosstab` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Style#Disabled` | `1.3.44.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Style#DraftKey` | `1.5.7.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Style#DraftOutputMode` | `1.5.7.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Style#Edit` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Style#Gantt` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Style#Id` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Style#ImageLib` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Style#Index` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Style#Kamban` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Style#New` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Style#TimeSeries` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Style#Title` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Summary#DestinationColumn` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Summary#DestinationCondition` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Summary#DestinationReferenceType` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Summary#Id` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Summary#LinkColumn` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Summary#SetZeroWhenOutOfCondition` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Summary#SiteId` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Summary#SourceColumn` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Summary#SourceCondition` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Summary#Type` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Tab#Id` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Tab#LabelText` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.ValidateInput#ClientRegexValidation` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.ValidateInput#ColumnName` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.ValidateInput#Delete` | `1.4.0.0` | | |
| `Implem.Pleasanter.Libraries.Settings.ValidateInput#Id` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.ValidateInput#Max` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.ValidateInput#Min` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.ValidateInput#RegexValidationMessage` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.ValidateInput#Required` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.ValidateInput#ServerRegexValidation` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.View#AnalyPartSettings` | `1.4.1.0` | | |
| `Implem.Pleasanter.Libraries.Settings.View#ApiGetMailAddresses` | `1.3.16.0` | | |
| `Implem.Pleasanter.Libraries.Settings.View#CalendarDateHash` | `1.3.50.0` | | |
| `Implem.Pleasanter.Libraries.Settings.View#CalendarEnd` | `1.3.46.0` | | |
| `Implem.Pleasanter.Libraries.Settings.View#CalendarEndHash` | `1.3.50.0` | | |
| `Implem.Pleasanter.Libraries.Settings.View#CalendarShowStatus` | `1.3.29.0` | | |
| `Implem.Pleasanter.Libraries.Settings.View#CalendarSiteId` | `1.3.50.0` | | |
| `Implem.Pleasanter.Libraries.Settings.View#CalendarStart` | `1.3.46.0` | | |
| `Implem.Pleasanter.Libraries.Settings.View#CalendarStartHash` | `1.3.50.0` | | |
| `Implem.Pleasanter.Libraries.Settings.View#CalendarSuffix` | `1.3.50.0` | | |
| `Implem.Pleasanter.Libraries.Settings.View#CalendarType` | `1.3.50.0` | | |
| `Implem.Pleasanter.Libraries.Settings.View#CalendarViewType` | `1.3.46.0` | | |
| `Implem.Pleasanter.Libraries.Settings.View#CalendarViewTypeHash` | `1.3.50.0` | | |
| `Implem.Pleasanter.Libraries.Settings.View#ColumnFilterExpressions` | `1.3.16.0` | | |
| `Implem.Pleasanter.Libraries.Settings.View#ColumnFilterNegatives` | `1.3.18.0` | | |
| `Implem.Pleasanter.Libraries.Settings.View#CreateCommand` | `1.3.32.0` | | |
| `Implem.Pleasanter.Libraries.Settings.View#CrosstabNotShowZeroRows` | `1.3.29.0` | | |
| `Implem.Pleasanter.Libraries.Settings.View#DashboardPartLayoutHash` | `1.4.1.0` | | |
| `Implem.Pleasanter.Libraries.Settings.View#DefaultMode` | `1.3.2.0` | | |
| `Implem.Pleasanter.Libraries.Settings.View#FilterColumns` | `1.3.11.0` | | |
| `Implem.Pleasanter.Libraries.Settings.View#GuidesReduced` | `1.4.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.View#IndexSuffix` | `1.4.1.0` | | |
| `Implem.Pleasanter.Libraries.Settings.View#KambanShowStatus` | `1.3.29.0` | | |
| `Implem.Pleasanter.Libraries.Settings.View#KambanSuffix` | `1.4.0.0` | | |
| `Implem.Pleasanter.Libraries.Settings.View#KeepFilterState` | `1.3.12.0` | | |
| `Implem.Pleasanter.Libraries.Settings.View#KeepSorterState` | `1.3.12.0` | | |
| `Implem.Pleasanter.Libraries.Settings.View#MergeSessionViewFilters` | `1.3.3.0` | | |
| `Implem.Pleasanter.Libraries.Settings.View#MergeSessionViewSorters` | `1.3.3.0` | | |
| `Implem.Pleasanter.Libraries.Settings.View#TimeSeriesChartType` | `1.3.25.0` | | |
| `Implem.Pleasanter.Libraries.Settings.View#TimeSeriesHorizontalAxis` | `1.3.25.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettingPermission#Depts` | `1.4.0.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettingPermission#Groups` | `1.4.0.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettingPermission#Users` | `1.4.0.0` | | |
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
| `Implem.Pleasanter.Models.ApiSiteSettings.HtmlApiSettingModel#Body` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.HtmlApiSettingModel#Delete` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.HtmlApiSettingModel#Disabled` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.HtmlApiSettingModel#DraftKey` | `1.5.7.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.HtmlApiSettingModel#DraftOutputMode` | `1.5.7.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.HtmlApiSettingModel#HtmlAll` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.HtmlApiSettingModel#HtmlAnaly` | `1.4.1.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.HtmlApiSettingModel#HtmlBurnDown` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.HtmlApiSettingModel#HtmlCalendar` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.HtmlApiSettingModel#HtmlCrosstab` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.HtmlApiSettingModel#HtmlEdit` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.HtmlApiSettingModel#HtmlGantt` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.HtmlApiSettingModel#HtmlImageLib` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.HtmlApiSettingModel#HtmlIndex` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.HtmlApiSettingModel#HtmlKamban` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.HtmlApiSettingModel#HtmlNew` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.HtmlApiSettingModel#HtmlPositionType` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.HtmlApiSettingModel#HtmlTimeSeries` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.HtmlApiSettingModel#Id` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.HtmlApiSettingModel#Title` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ISiteSettingBaseProperties#Delete` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ISiteSettingBaseProperties#HtmlPositionType` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ISiteSettingBaseProperties#Id` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ISiteSettingBaseProperties#Title` | `1.3.50.0` | | |
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
| `Implem.Pleasanter.Models.ApiSiteSettings.ProcessApiSettingModel#ActionType` | `1.4.0.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ProcessApiSettingModel#AfterProcessStatusChangeActionType` | `1.4.23.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ProcessApiSettingModel#AllowBulkProcessing` | `1.4.0.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ProcessApiSettingModel#AutoNumbering` | `1.4.0.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ProcessApiSettingModel#ChangedStatus` | `1.4.0.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ProcessApiSettingModel#ConfirmationMessage` | `1.4.0.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ProcessApiSettingModel#CurrentStatus` | `1.4.0.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ProcessApiSettingModel#DataChanges` | `1.4.0.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ProcessApiSettingModel#Delete` | `1.4.0.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ProcessApiSettingModel#Depts` | `1.4.0.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ProcessApiSettingModel#Description` | `1.4.0.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ProcessApiSettingModel#Disabled` | `1.5.1.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ProcessApiSettingModel#DisplayName` | `1.4.0.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ProcessApiSettingModel#ErrorMessage` | `1.4.0.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ProcessApiSettingModel#ExecutionType` | `1.4.0.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ProcessApiSettingModel#Groups` | `1.4.0.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ProcessApiSettingModel#Icon` | `1.4.11.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ProcessApiSettingModel#Id` | `1.4.0.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ProcessApiSettingModel#MatchConditions` | `1.4.0.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ProcessApiSettingModel#Name` | `1.4.0.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ProcessApiSettingModel#Notifications` | `1.4.0.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ProcessApiSettingModel#OnClick` | `1.4.0.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ProcessApiSettingModel#Permission` | `1.4.0.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ProcessApiSettingModel#ScreenType` | `1.4.0.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ProcessApiSettingModel#SuccessMessage` | `1.4.0.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ProcessApiSettingModel#Tooltip` | `1.4.0.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ProcessApiSettingModel#Users` | `1.4.0.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ProcessApiSettingModel#ValidateInputs` | `1.4.0.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ProcessApiSettingModel#ValidationType` | `1.4.0.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ProcessApiSettingModel#View` | `1.4.0.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ScriptApiSettingModel#Body` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ScriptApiSettingModel#Delete` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ScriptApiSettingModel#Disabled` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ScriptApiSettingModel#DraftKey` | `1.5.7.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ScriptApiSettingModel#DraftOutputMode` | `1.5.7.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ScriptApiSettingModel#HtmlPositionType` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ScriptApiSettingModel#Id` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ScriptApiSettingModel#ScriptAll` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ScriptApiSettingModel#ScriptAnaly` | `1.4.1.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ScriptApiSettingModel#ScriptBurnDown` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ScriptApiSettingModel#ScriptCalendar` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ScriptApiSettingModel#ScriptCrosstab` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ScriptApiSettingModel#ScriptEdit` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ScriptApiSettingModel#ScriptGantt` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ScriptApiSettingModel#ScriptImageLib` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ScriptApiSettingModel#ScriptIndex` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ScriptApiSettingModel#ScriptKamban` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ScriptApiSettingModel#ScriptNew` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ScriptApiSettingModel#ScriptTimeSeries` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ScriptApiSettingModel#Title` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.SectionApiSettingModel#AllowExpand` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.SectionApiSettingModel#Expand` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.SectionApiSettingModel#Hide` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.SectionApiSettingModel#Id` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.SectionApiSettingModel#LabelText` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ServerScriptApiSettingModel#Body` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ServerScriptApiSettingModel#CalenServerScriptBeforeFormuladarGuide` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ServerScriptApiSettingModel#Delete` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ServerScriptApiSettingModel#Disabled` | `1.4.12.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ServerScriptApiSettingModel#Functionalize` | `1.4.12.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ServerScriptApiSettingModel#HtmlPositionType` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ServerScriptApiSettingModel#Id` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ServerScriptApiSettingModel#Name` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ServerScriptApiSettingModel#ServerScriptAfterBulkDelete` | `1.4.16.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ServerScriptApiSettingModel#ServerScriptAfterCreate` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ServerScriptApiSettingModel#ServerScriptAfterDelete` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ServerScriptApiSettingModel#ServerScriptAfterFormula` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ServerScriptApiSettingModel#ServerScriptAfterUpdate` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ServerScriptApiSettingModel#ServerScriptBeforeBulkDelete` | `1.4.16.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ServerScriptApiSettingModel#ServerScriptBeforeCreate` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ServerScriptApiSettingModel#ServerScriptBeforeDelete` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ServerScriptApiSettingModel#ServerScriptBeforeFormula` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ServerScriptApiSettingModel#ServerScriptBeforeOpeningPage` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ServerScriptApiSettingModel#ServerScriptBeforeOpeningRow` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ServerScriptApiSettingModel#ServerScriptBeforeUpdate` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ServerScriptApiSettingModel#ServerScriptShared` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ServerScriptApiSettingModel#ServerScriptWhenViewProcessing` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ServerScriptApiSettingModel#ServerScriptWhenloadingRecord` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ServerScriptApiSettingModel#ServerScriptWhenloadingSiteSettings` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ServerScriptApiSettingModel#Title` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.ServerScriptApiSettingModel#TryCatch` | `1.4.12.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.SiteSettingsApiModel#Columns` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.SiteSettingsApiModel#EditorColumnHash` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.SiteSettingsApiModel#FilterColumns` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.SiteSettingsApiModel#GeneralTabLabelText` | `1.5.3.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.SiteSettingsApiModel#GridColumns` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.SiteSettingsApiModel#Htmls` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.SiteSettingsApiModel#Links` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.SiteSettingsApiModel#Processes` | `1.4.0.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.SiteSettingsApiModel#Scripts` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.SiteSettingsApiModel#SectionLatestId` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.SiteSettingsApiModel#Sections` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.SiteSettingsApiModel#ServerScripts` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.SiteSettingsApiModel#StatusControls` | `1.4.0.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.SiteSettingsApiModel#Styles` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.SiteSettingsApiModel#TabLatestId` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.SiteSettingsApiModel#Tabs` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.SiteSettingsApiModel#Timestamp` | `1.4.15.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.SiteSettingsApiModel#VerUp` | `1.5.6.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.StatusControlApiSettingModel#ColumnHash` | `1.4.0.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.StatusControlApiSettingModel#Delete` | `1.4.0.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.StatusControlApiSettingModel#Description` | `1.4.0.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.StatusControlApiSettingModel#Disabled` | `1.5.0.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.StatusControlApiSettingModel#Id` | `1.4.0.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.StatusControlApiSettingModel#Name` | `1.4.0.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.StatusControlApiSettingModel#Permission` | `1.4.0.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.StatusControlApiSettingModel#ReadOnly` | `1.4.0.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.StatusControlApiSettingModel#Status` | `1.4.0.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.StatusControlApiSettingModel#View` | `1.4.0.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.StyleApiSettingModel#Body` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.StyleApiSettingModel#Delete` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.StyleApiSettingModel#Disabled` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.StyleApiSettingModel#DraftKey` | `1.5.7.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.StyleApiSettingModel#DraftOutputMode` | `1.5.7.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.StyleApiSettingModel#HtmlPositionType` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.StyleApiSettingModel#Id` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.StyleApiSettingModel#StyleAll` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.StyleApiSettingModel#StyleAnaly` | `1.4.1.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.StyleApiSettingModel#StyleBurnDown` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.StyleApiSettingModel#StyleCalendar` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.StyleApiSettingModel#StyleCrosstab` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.StyleApiSettingModel#StyleEdit` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.StyleApiSettingModel#StyleGantt` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.StyleApiSettingModel#StyleImageLib` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.StyleApiSettingModel#StyleIndex` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.StyleApiSettingModel#StyleKamban` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.StyleApiSettingModel#StyleNew` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.StyleApiSettingModel#StyleTimeSeries` | `1.3.50.0` | | |
| `Implem.Pleasanter.Models.ApiSiteSettings.StyleApiSettingModel#Title` | `1.3.50.0` | | |
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
| `Implem.Pleasanter.Models.GroupApiModel#GroupMembers` | `1.3.4.0` | | |
| `Implem.Pleasanter.Models.GroupApiModel#LdapGuid` | `1.4.4.0` | | |
| `Implem.Pleasanter.Models.GroupApiModel#LdapSearchRoot` | `1.4.4.0` | | |
| `Implem.Pleasanter.Models.GroupApiModel#LdapSync` | `1.4.4.0` | | |
| `Implem.Pleasanter.Models.GroupApiModel#ScimExternalId` | `1.5.8.0` | | |
| `Implem.Pleasanter.Models.GroupApiModel#ScimId` | `1.5.8.0` | | |
| `Implem.Pleasanter.Models.GroupApiModel#ScimSync` | `1.5.8.0` | | |
| `Implem.Pleasanter.Models.GroupApiModel#SynchronizedTime` | `1.4.4.0` | | |
| `Implem.Pleasanter.Models.IssueApiModel#MigrationMode` | `1.4.16.0` | | |
| `Implem.Pleasanter.Models.IssueApiModel#ProcessId` | `1.3.39.0` | | |
| `Implem.Pleasanter.Models.IssueApiModel#ProcessIds` | `1.4.16.0` | | |
| `Implem.Pleasanter.Models.Issues.IssueBulkUpsertApiModel#Data` | `1.4.6.0` | | `1.4.7.0`（type） |
| `Implem.Pleasanter.Models.Issues.IssueBulkUpsertApiModel#KeyNotFoundCreate` | `1.4.6.0` | | |
| `Implem.Pleasanter.Models.Issues.IssueBulkUpsertApiModel#Keys` | `1.4.6.0` | | `1.4.7.0`（type） |
| `Implem.Pleasanter.Models.Issues.IssueBulkUpsertApiModel#SiteId` | `1.4.6.0` | | |
| `Implem.Pleasanter.Models.ResultApiModel#MigrationMode` | `1.4.16.0` | | |
| `Implem.Pleasanter.Models.ResultApiModel#ProcessId` | `1.3.39.0` | | |
| `Implem.Pleasanter.Models.ResultApiModel#ProcessIds` | `1.4.16.0` | | |
| `Implem.Pleasanter.Models.Results.ResultBulkUpsertApiModel#Data` | `1.4.6.0` | | `1.4.7.0`（type） |
| `Implem.Pleasanter.Models.Results.ResultBulkUpsertApiModel#KeyNotFoundCreate` | `1.4.6.0` | | |
| `Implem.Pleasanter.Models.Results.ResultBulkUpsertApiModel#Keys` | `1.4.6.0` | | `1.4.7.0`（type） |
| `Implem.Pleasanter.Models.Results.ResultBulkUpsertApiModel#SiteId` | `1.4.6.0` | | |
| `Implem.Pleasanter.Models.Shared._BaseApiModel#ApiVersion` | `1.3.0.0` | | `1.3.13.0`（default） |
| `Implem.Pleasanter.Models.Shared._BaseApiModel#ImageHash` | `1.3.26.0` | | |
| `Implem.Pleasanter.Models.Shared._BaseApiModel#SsCache` | `1.5.6.0` | | |
| `Implem.Pleasanter.Models.Shared._ImageApiModel#Alt` | `1.3.26.0` | | |
| `Implem.Pleasanter.Models.Shared._ImageApiModel#Base64` | `1.3.26.0` | | |
| `Implem.Pleasanter.Models.Shared._ImageApiModel#EndNewLine` | `1.3.26.0` | | |
| `Implem.Pleasanter.Models.Shared._ImageApiModel#Extension` | `1.3.26.0` | | |
| `Implem.Pleasanter.Models.Shared._ImageApiModel#HeadNewLine` | `1.3.26.0` | | |
| `Implem.Pleasanter.Models.Shared._ImageApiModel#Position` | `1.3.26.0` | | |
| `Implem.Pleasanter.Models.SiteApiModel#AnalyGuide` | `1.4.1.0` | | |
| `Implem.Pleasanter.Models.SiteApiModel#ApiCount` | `1.3.4.0` | | |
| `Implem.Pleasanter.Models.SiteApiModel#ApiCountDate` | `1.3.4.0` | | |
| `Implem.Pleasanter.Models.SiteApiModel#Body` | `1.3.4.0` | | |
| `Implem.Pleasanter.Models.SiteApiModel#BurnDownGuide` | `1.3.4.0` | | |
| `Implem.Pleasanter.Models.SiteApiModel#CalendarGuide` | `1.3.4.0` | | |
| `Implem.Pleasanter.Models.SiteApiModel#Comments` | `1.3.4.0` | | |
| `Implem.Pleasanter.Models.SiteApiModel#CreatedTime` | `1.3.4.0` | | |
| `Implem.Pleasanter.Models.SiteApiModel#Creator` | `1.3.4.0` | | |
| `Implem.Pleasanter.Models.SiteApiModel#CrosstabGuide` | `1.3.4.0` | | |
| `Implem.Pleasanter.Models.SiteApiModel#DisableCrossSearch` | `1.3.4.0` | | |
| `Implem.Pleasanter.Models.SiteApiModel#DisableSiteCreatorPermission` | `1.3.31.0` | | |
| `Implem.Pleasanter.Models.SiteApiModel#EditorGuide` | `1.3.4.0` | | |
| `Implem.Pleasanter.Models.SiteApiModel#EnableSsCache` | `1.5.6.0` | | |
| `Implem.Pleasanter.Models.SiteApiModel#FindSiteNames` | `1.4.5.0` | | |
| `Implem.Pleasanter.Models.SiteApiModel#Form` | `1.4.23.0` | | |
| `Implem.Pleasanter.Models.SiteApiModel#GanttGuide` | `1.3.4.0` | | |
| `Implem.Pleasanter.Models.SiteApiModel#GridGuide` | `1.3.4.0` | | |
| `Implem.Pleasanter.Models.SiteApiModel#ImageLibGuide` | `1.3.4.0` | | |
| `Implem.Pleasanter.Models.SiteApiModel#InheritPermission` | `1.3.4.0` | | |
| `Implem.Pleasanter.Models.SiteApiModel#ItemTitle` | `1.3.4.0` | | |
| `Implem.Pleasanter.Models.SiteApiModel#KambanGuide` | `1.3.4.0` | | |
| `Implem.Pleasanter.Models.SiteApiModel#LockedTime` | `1.3.4.0` | | |
| `Implem.Pleasanter.Models.SiteApiModel#LockedUser` | `1.3.4.0` | | |
| `Implem.Pleasanter.Models.SiteApiModel#ParentId` | `1.3.4.0` | | |
| `Implem.Pleasanter.Models.SiteApiModel#Permissions` | `1.3.4.0` | | |
| `Implem.Pleasanter.Models.SiteApiModel#Publish` | `1.3.4.0` | | |
| `Implem.Pleasanter.Models.SiteApiModel#ReferenceType` | `1.3.4.0` | | |
| `Implem.Pleasanter.Models.SiteApiModel#SiteGroupName` | `1.3.4.0` | | |
| `Implem.Pleasanter.Models.SiteApiModel#SiteId` | `1.3.4.0` | | |
| `Implem.Pleasanter.Models.SiteApiModel#SiteName` | `1.3.4.0` | | |
| `Implem.Pleasanter.Models.SiteApiModel#SiteSettings` | `1.3.4.0` | | |
| `Implem.Pleasanter.Models.SiteApiModel#SummaryId` | `1.3.33.0` | | |
| `Implem.Pleasanter.Models.SiteApiModel#TenantId` | `1.3.4.0` | | |
| `Implem.Pleasanter.Models.SiteApiModel#TimeSeriesGuide` | `1.3.4.0` | | |
| `Implem.Pleasanter.Models.SiteApiModel#Title` | `1.3.4.0` | | |
| `Implem.Pleasanter.Models.SiteApiModel#UpdatedTime` | `1.3.4.0` | | |
| `Implem.Pleasanter.Models.SiteApiModel#Updator` | `1.3.4.0` | | |
| `Implem.Pleasanter.Models.SiteApiModel#Ver` | `1.3.4.0` | | |
| `Implem.Pleasanter.Models.SuspendTenantApiModel#ApiKey` | `1.5.7.0` | | |
| `Implem.Pleasanter.Models.SuspendTenantApiModel#SuspendDate` | `1.5.7.0` | | |
| `Implem.Pleasanter.Models.UserApiModel#AllowMovingFromTopSite` | `1.4.8.0` | | |
| `Implem.Pleasanter.Models.UserApiModel#DisableSecondaryAuthentication` | `1.3.2.0` | | |
| `Implem.Pleasanter.Models.UserApiModel#EnableSecondaryAuthentication` | `1.3.2.0` | | |
| `Implem.Pleasanter.Models.UserApiModel#EnableSecretKey` | `1.4.2.0` | | |
| `Implem.Pleasanter.Models.UserApiModel#LoginExpirationLimit` | `1.4.13.0` | | |
| `Implem.Pleasanter.Models.UserApiModel#LoginExpirationPeriod` | `1.4.13.0` | | |
| `Implem.Pleasanter.Models.UserApiModel#MailAddresses` | `1.3.16.0` | | |
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
| `Implem.Pleasanter.Models.WikiApiModel#Body` | `1.3.38.0` | | |
| `Implem.Pleasanter.Models.WikiApiModel#Comments` | `1.3.38.0` | | |
| `Implem.Pleasanter.Models.WikiApiModel#CreatedTime` | `1.3.38.0` | | |
| `Implem.Pleasanter.Models.WikiApiModel#Creator` | `1.3.38.0` | | |
| `Implem.Pleasanter.Models.WikiApiModel#ItemTitle` | `1.3.38.0` | | |
| `Implem.Pleasanter.Models.WikiApiModel#Locked` | `1.3.38.0` | | |
| `Implem.Pleasanter.Models.WikiApiModel#MigrationMode` | `1.4.16.0` | | |
| `Implem.Pleasanter.Models.WikiApiModel#SiteId` | `1.3.38.0` | | |
| `Implem.Pleasanter.Models.WikiApiModel#Title` | `1.3.38.0` | | |
| `Implem.Pleasanter.Models.WikiApiModel#UpdatedTime` | `1.3.38.0` | | |
| `Implem.Pleasanter.Models.WikiApiModel#Updator` | `1.3.38.0` | | |
| `Implem.Pleasanter.Models.WikiApiModel#Ver` | `1.3.38.0` | | |
| `Implem.Pleasanter.Models.WikiApiModel#WikiId` | `1.3.38.0` | | |

### 列挙値

| 項目 | Since | Until | 変更 |
| --- | --- | --- | --- |
| `Implem.Libraries.DataSources.SqlServer.Sqls.TableTypes#All` | `1.3.0.0` | | `1.3.27.1`（value）; `1.3.31.0`（value） |
| `Implem.Libraries.DataSources.SqlServer.Sqls.TableTypes#Match` | `1.3.27.1` | `1.3.31.0` | |
| `Implem.Pleasanter.Libraries.Models.Versions.AfterCreateActionTypes#Default` | `1.4.23.0` | | |
| `Implem.Pleasanter.Libraries.Models.Versions.AfterCreateActionTypes#OpenNewEditor` | `1.4.23.0` | | |
| `Implem.Pleasanter.Libraries.Models.Versions.AfterCreateActionTypes#ReturnToList` | `1.4.23.0` | | |
| `Implem.Pleasanter.Libraries.Models.Versions.AfterUpdateActionTypes#Default` | `1.4.23.0` | | |
| `Implem.Pleasanter.Libraries.Models.Versions.AfterUpdateActionTypes#MoveToNextRecord` | `1.4.23.0` | | |
| `Implem.Pleasanter.Libraries.Models.Versions.AfterUpdateActionTypes#ReturnToList` | `1.4.23.0` | | |
| `Implem.Pleasanter.Libraries.Models.Versions.AutoVerUpTypes#Always` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Models.Versions.AutoVerUpTypes#Default` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Models.Versions.AutoVerUpTypes#Disabled` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Security.Permissions.Types#Create` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Security.Permissions.Types#Delete` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Security.Permissions.Types#Export` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Security.Permissions.Types#Import` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Security.Permissions.Types#ManagePermission` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Security.Permissions.Types#ManageService` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Security.Permissions.Types#ManageSite` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Security.Permissions.Types#ManageTenant` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Security.Permissions.Types#NotSet` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Security.Permissions.Types#Read` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Security.Permissions.Types#SendMail` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Security.Permissions.Types#Update` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Server.Times.RepeatTypes#Daily` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Server.Times.RepeatTypes#EndOfMonth` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Server.Times.RepeatTypes#Monthly` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Server.Times.RepeatTypes#NumberWeekly` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Server.Times.RepeatTypes#Weekly` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Server.Times.RepeatTypes#Yearly` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.ServerScripts.ServerScriptModelResponse.Types#Filter` | `1.5.6.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Aggregation.Types#Average` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Aggregation.Types#Count` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Aggregation.Types#Total` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column.AutoNumberingResetTypes#Day` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column.AutoNumberingResetTypes#Month` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column.AutoNumberingResetTypes#None` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column.AutoNumberingResetTypes#String` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column.AutoNumberingResetTypes#Year` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column.FullTextTypes#DisplayName` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column.FullTextTypes#None` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column.FullTextTypes#Value` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column.FullTextTypes#ValueAndDisplayName` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column.ViewerSwitchingTypes#Auto` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column.ViewerSwitchingTypes#Disabled` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Column.ViewerSwitchingTypes#Manual` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.ColumnUtilities.CheckFilterControlTypes#OnAndOff` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.ColumnUtilities.CheckFilterControlTypes#OnOnly` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.ColumnUtilities.DateFilterSetMode#Default` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.ColumnUtilities.DateFilterSetMode#Range` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DashboardPartType#Calendar` | `1.3.50.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DashboardPartType#Custom` | `1.3.44.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DashboardPartType#CustomHtml` | `1.3.44.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DashboardPartType#Index` | `1.4.1.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DashboardPartType#Kamban` | `1.4.0.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DashboardPartType#QuickAccess` | `1.3.44.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DashboardPartType#TimeLine` | `1.3.44.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DataChange.Types#CopyDisplayValue` | `1.3.13.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DataChange.Types#CopyValue` | `1.3.13.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DataChange.Types#InputDate` | `1.3.13.0` | | `1.4.10.0`（value） |
| `Implem.Pleasanter.Libraries.Settings.DataChange.Types#InputDateTime` | `1.3.13.0` | | `1.4.10.0`（value） |
| `Implem.Pleasanter.Libraries.Settings.DataChange.Types#InputDept` | `1.3.13.0` | | `1.4.10.0`（value） |
| `Implem.Pleasanter.Libraries.Settings.DataChange.Types#InputUser` | `1.3.13.0` | | `1.4.10.0`（value） |
| `Implem.Pleasanter.Libraries.Settings.DataChange.Types#InputValue` | `1.3.13.0` | | |
| `Implem.Pleasanter.Libraries.Settings.DataChange.Types#InputValueFormula` | `1.4.10.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Formula.OperatorTypes#Addition` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Formula.OperatorTypes#Division` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Formula.OperatorTypes#Multiplication` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Formula.OperatorTypes#NotSet` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Formula.OperatorTypes#Subtraction` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Html.PositionTypes#BodyScriptBottom` | `1.3.45.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Html.PositionTypes#BodyScriptTop` | `1.3.45.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Html.PositionTypes#HeadBottom` | `1.3.45.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Html.PositionTypes#HeadTop` | `1.3.45.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Notification.Expressions#And` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Notification.Expressions#Or` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Notification.MethodTypes#Delete` | `1.3.17.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Notification.MethodTypes#Get` | `1.3.17.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Notification.MethodTypes#Post` | `1.3.17.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Notification.MethodTypes#Put` | `1.3.17.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Notification.Types#ChatWork` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Notification.Types#HttpClient` | `1.3.17.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Notification.Types#InCircle` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Notification.Types#Line` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Notification.Types#LineGroup` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Notification.Types#LineWorks` | `1.5.1.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Notification.Types#Mail` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Notification.Types#RocketChat` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Notification.Types#Slack` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Notification.Types#Teams` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Process.ActionTypes#None` | `1.3.12.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Process.ActionTypes#PostBack` | `1.3.12.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Process.ActionTypes#Save` | `1.3.12.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Process.AfterProcessStatusChangeActionTypes#Default` | `1.4.23.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Process.AfterProcessStatusChangeActionTypes#ReturnToList` | `1.4.23.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Process.ExecutionTypes#AddedButton` | `1.3.14.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Process.ExecutionTypes#AddedButtonOrCreateOrUpdate` | `1.4.11.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Process.ExecutionTypes#CreateOrUpdate` | `1.3.14.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Process.ScreenTypes#Edit` | `1.3.5.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Process.ScreenTypes#New` | `1.3.5.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Process.ValidationTypes#Merge` | `1.3.12.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Process.ValidationTypes#None` | `1.3.12.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Process.ValidationTypes#Replacement` | `1.3.12.0` | | |
| `Implem.Pleasanter.Libraries.Settings.QuickAccessLayout#Horizontal` | `1.3.44.0` | | |
| `Implem.Pleasanter.Libraries.Settings.QuickAccessLayout#Vertical` | `1.3.44.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Reminder.ReminderTypes#ChatWork` | `1.3.10.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Reminder.ReminderTypes#InCircle` | `1.3.10.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Reminder.ReminderTypes#Line` | `1.3.10.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Reminder.ReminderTypes#LineGroup` | `1.3.10.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Reminder.ReminderTypes#LineWorks` | `1.5.1.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Reminder.ReminderTypes#Mail` | `1.3.10.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Reminder.ReminderTypes#RocketChat` | `1.3.10.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Reminder.ReminderTypes#Slack` | `1.3.10.0` | | |
| `Implem.Pleasanter.Libraries.Settings.Reminder.ReminderTypes#Teams` | `1.3.10.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings.CalendarTypes#FullCalendar` | `1.3.46.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings.CalendarTypes#Standard` | `1.3.46.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings.GridEditorTypes#Dialog` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings.GridEditorTypes#Grid` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings.GridEditorTypes#None` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings.RoundingTypes#AwayFromZero` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings.RoundingTypes#Ceiling` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings.RoundingTypes#Floor` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings.RoundingTypes#ToEven` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings.RoundingTypes#Truncate` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings.SaveViewTypes#None` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings.SaveViewTypes#Session` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings.SaveViewTypes#User` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings.SearchTypes#BroadMatchOfTitle` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings.SearchTypes#FullText` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings.SearchTypes#MatchInFrontOfTitle` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings.SearchTypes#PartialMatch` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings.StandardExportTypes#Default` | `1.5.2.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings.StandardExportTypes#Editor` | `1.5.2.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings.StandardExportTypes#Grid` | `1.5.2.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings.TextAlignTypes#Center` | `1.4.1.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings.TextAlignTypes#Left` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.SiteSettings.TextAlignTypes#Right` | `1.3.4.0` | | |
| `Implem.Pleasanter.Libraries.Settings.StatusControl.ControlConstraintsTypes#Hidden` | `1.3.12.0` | | |
| `Implem.Pleasanter.Libraries.Settings.StatusControl.ControlConstraintsTypes#None` | `1.3.12.0` | | |
| `Implem.Pleasanter.Libraries.Settings.StatusControl.ControlConstraintsTypes#ReadOnly` | `1.3.12.0` | | |
| `Implem.Pleasanter.Libraries.Settings.StatusControl.ControlConstraintsTypes#Required` | `1.3.12.0` | | |
| `Implem.Pleasanter.Libraries.Settings.TimeLineDisplayType#Detailed` | `1.3.44.0` | | |
| `Implem.Pleasanter.Libraries.Settings.TimeLineDisplayType#Simple` | `1.3.44.0` | | |
| `Implem.Pleasanter.Libraries.Settings.TimeLineDisplayType#Standard` | `1.3.44.0` | | |

### API の既定値（Parameters/Api.json）

| 項目 | Since | Until | 変更 |
| --- | --- | --- | --- |
| `Api.Compatibility_1_3_12` | `1.3.13.0` | | |
| `Api.Version` | `1.3.13.0` | | |

### 規約ルーティング（Startup の MapControllerRoute）

| 項目 | Since | Until | 変更 |
| --- | --- | --- | --- |
| `FormBinaries` | `1.4.23.0` | | |
| `Others` | `1.3.0.0` | | `1.3.20.0`（constraints） |

## 全版で変わらない項目の数

| 分類 | 件数 |
| --- | --- |
| enum | 41 |
| member | 1060 |
| parameter | 3 |
| route | 4 |
| type | 43 |
