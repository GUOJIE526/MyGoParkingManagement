# 停車場後台管理系統

## 專案簡介
此專案是針對停車場後台管理的系統，具備車輛管理、用戶管理、優惠卷管理、出入管理、交易紀錄管理、月租管理、預訂管理、以及停車場與車位的管理功能。系統提供了即時查詢、資料編輯、刪除和新增等功能。

## 功能介紹
1. **車輛管理**：
    - 管理員可以新增、編輯及刪除車輛資料。
    - 支援車輛預訂與月租的狀態管理。
   
2. **用戶管理**：
    - 管理用戶的基本資料，聯繫資訊等。
   
3. **出入管理**：
    - 紀錄車輛的出入時間、車牌辨識、與停車場進出記錄。

4. **交易紀錄管理**：
    - 管理用戶的交易紀錄。

5. **月租管理**：
    - 管理車輛的月租訂單，包括租金金額、到期時間、付款狀態等。

6. **預訂管理**：
    - 允許用戶提前預訂停車位，並管理預訂記錄。

7. **停車場管理**：
    - 管理不同停車場的資料，包括停車場名稱、位置、以及車位的出租狀況。

8. **車位管理**：
    - 管理各停車場的車位資訊，顯示車位使用率等統計數據。

## 技術架構
- **前端**：使用 jQuery, DataTables 進行前端操作，透過 Ajax 技術實現即時的資料更新及互動。
- **後端**：ASP.NET Core MVC 框架，使用 C# 編寫，支援資料庫操作與多重資料驗證。
- **資料庫**：使用 Entity Framework Core 進行資料庫操作，並搭配 SQL Server。
