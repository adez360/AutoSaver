# AutoSaver Plugin

一個Unity Editor插件，可以定時自動保存場景到指定目錄，無需GameObject。

## 功能特色

- 🕒 定時自動保存場景（Editor-only，無需GameObject）
- ⚙️ 可自訂保存頻率（分鐘）
- 📁 可自訂保存位置
- 💾 手動保存功能
- 🗑️ 自動刪除過舊的存檔文件（包括.meta文件）
- 🎨 使用UIElement的現代化控制界面
- 🌍 多語言支持（英文/中文）
- 🔄 保存時不替換當前編輯的場景

## 文件結構

```
Assets/360/AutoSaver/
├── Editor/                          # Editor腳本目錄
│   ├── AutoSaverWindow.cs          # 主控制視窗
│   ├── AutoSaverWindow.uxml        # UIElement視窗佈局
│   ├── AutoSaverWindow.uss         # 視窗樣式
│   └── AutoSaverEditor.cs          # Editor-only自動保存邏輯
├── Runtime/                         # 運行時腳本目錄
│   └── AutoSaverLocalization.cs    # 本地化管理器
└── AutoSaver.asmdef                 # Assembly定義文件
```

## 使用方法

1. 在Unity選單中選擇 `Tools > AutoSaver` 打開控制視窗
2. 在頂部選擇語言（英文/中文）
3. 設定自動保存頻率（分鐘）
4. 設定保存路徑（預設：Assets/AutoSave/）
5. 設定最大保存文件數
6. 使用右側按鈕啟用/關閉自動保存
7. 使用手動保存按鈕立即保存

## 存檔命名格式

- 自動保存：`場景名_Auto_YYYYMMDD_HHmmss.unity`
- 手動保存：`場景名_Manual_YYYYMMDD_HHmmss.unity`

## 系統需求

- Unity 2022.3 LTS 或更高版本
- UIElement 支援

## 開發狀態

✅ 完成 - 功能完整，可正常使用
