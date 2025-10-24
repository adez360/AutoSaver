using System.Collections.Generic;
using UnityEngine;

namespace AutoSaver
{
    /// <summary>
    /// Localization manager for AutoSaver
    /// </summary>
    public static class AutoSaverLocalization
    {
        public enum Language
        {
            English,
            Chinese
        }

        private static Language currentLanguage = Language.English;
        private static Dictionary<string, Dictionary<Language, string>> translations;

        static AutoSaverLocalization()
        {
            InitializeTranslations();
        }

        private static void InitializeTranslations()
        {
            translations = new Dictionary<string, Dictionary<Language, string>>
            {
                ["window_title"] = new Dictionary<Language, string>
                {
                    [Language.English] = "AutoSaver Control Panel",
                    [Language.Chinese] = "AutoSaver 控制面板"
                },
                ["settings"] = new Dictionary<Language, string>
                {
                    [Language.English] = "Settings",
                    [Language.Chinese] = "設定"
                },
                ["save_interval"] = new Dictionary<Language, string>
                {
                    [Language.English] = "Save Interval (minutes)",
                    [Language.Chinese] = "保存間隔 (分鐘)"
                },
                ["save_path"] = new Dictionary<Language, string>
                {
                    [Language.English] = "Save Path",
                    [Language.Chinese] = "保存路徑"
                },
                ["max_save_files"] = new Dictionary<Language, string>
                {
                    [Language.English] = "Max Save Files",
                    [Language.Chinese] = "最大保存文件數"
                },
                ["actions"] = new Dictionary<Language, string>
                {
                    [Language.English] = "Actions",
                    [Language.Chinese] = "操作"
                },
                ["manual_save"] = new Dictionary<Language, string>
                {
                    [Language.English] = "Manual Save",
                    [Language.Chinese] = "手動保存"
                },
                ["autosave_on"] = new Dictionary<Language, string>
                {
                    [Language.English] = "AutoSave: ON",
                    [Language.Chinese] = "自動保存: 開啟"
                },
                ["autosave_off"] = new Dictionary<Language, string>
                {
                    [Language.English] = "AutoSave: OFF",
                    [Language.Chinese] = "自動保存: 關閉"
                },
                ["language"] = new Dictionary<Language, string>
                {
                    [Language.English] = "Language",
                    [Language.Chinese] = "語言"
                }
            };
        }

        public static Language CurrentLanguage
        {
            get => currentLanguage;
            set
            {
                currentLanguage = value;
                OnLanguageChanged?.Invoke();
            }
        }

        public static event System.Action OnLanguageChanged;

        public static string GetText(string key)
        {
            if (translations.TryGetValue(key, out var languageDict) &&
                languageDict.TryGetValue(currentLanguage, out var text))
            {
                return text;
            }
            return key; // Fallback to key if translation not found
        }

        public static string[] GetLanguageOptions()
        {
            return new string[] { "English", "中文" };
        }
    }
}
