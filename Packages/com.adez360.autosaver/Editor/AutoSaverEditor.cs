using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System;
using System.IO;
using System.Linq;

namespace AutoSaver
{
    /// <summary>
    /// Editor-only AutoSaver - No GameObject required
    /// </summary>
    [InitializeOnLoad]
    public static class AutoSaverEditor
    {
        private static bool isEnabled = true;
        private static float saveIntervalMinutes = 5f;
        private static string savePath = "Assets/AutoSave/";
        private static int maxSaveFiles = 10;
        
        private static double lastSaveTime;
        private static bool isInitialized = false;
        
        static AutoSaverEditor()
        {
            EditorApplication.update += OnEditorUpdate;
            LoadSettings();
        }
        
        private static void OnEditorUpdate()
        {
            if (!isEnabled || !isInitialized) return;
            
            // Check if we're in play mode or if there's no active scene
            if (Application.isPlaying) return;
            
            var activeScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            if (string.IsNullOrEmpty(activeScene.path)) return;
            
            double currentTime = EditorApplication.timeSinceStartup;
            if (currentTime - lastSaveTime >= saveIntervalMinutes * 60.0)
            {
                PerformAutoSave();
            }
        }
        
        public static void PerformAutoSave()
        {
            if (!isEnabled) return;
            
            try
            {
                var activeScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
                if (string.IsNullOrEmpty(activeScene.name)) return;
                
                // Ensure save directory exists
                CreateSaveDirectory();
                
                string fileName = $"{activeScene.name}_Auto_{DateTime.Now:yyyyMMdd_HHmmss}.unity";
                string relativePath = savePath.Replace("Assets/", "").TrimEnd('/');
                string fullPath = Path.Combine(Application.dataPath, relativePath, fileName);
                
                
                // Mark scene as dirty to ensure it can be saved
                EditorSceneManager.MarkSceneDirty(activeScene);
                
                // Save as a copy without changing the current scene
                bool success = EditorSceneManager.SaveScene(activeScene, fullPath, true);
                if (success)
                {
                    Debug.Log($"[AutoSaver] Auto-saved: {fileName}");
                    lastSaveTime = EditorApplication.timeSinceStartup;
                    CleanupOldFiles();
                }
                else
                {
                    Debug.LogError("[AutoSaver] Auto-save failed!");
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"[AutoSaver] Auto-save failed: {e.Message}");
            }
        }
        
        public static void PerformManualSave()
        {
            try
            {
                var activeScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
                if (string.IsNullOrEmpty(activeScene.name)) return;
                
                // Ensure save directory exists
                CreateSaveDirectory();
                
                string fileName = $"{activeScene.name}_Manual_{DateTime.Now:yyyyMMdd_HHmmss}.unity";
                string relativePath = savePath.Replace("Assets/", "").TrimEnd('/');
                string fullPath = Path.Combine(Application.dataPath, relativePath, fileName);
                
                
                // Mark scene as dirty to ensure it can be saved
                EditorSceneManager.MarkSceneDirty(activeScene);
                
                // Save as a copy without changing the current scene
                bool success = EditorSceneManager.SaveScene(activeScene, fullPath, true);
                if (success)
                {
                    Debug.Log($"[AutoSaver] Manual save: {fileName}");
                    CleanupOldFiles();
                }
                else
                {
                    Debug.LogError("[AutoSaver] Manual save failed!");
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"[AutoSaver] Manual save failed: {e.Message}");
            }
        }
        
        private static void CleanupOldFiles()
        {
            try
            {
                string relativePath = savePath.Replace("Assets/", "").TrimEnd('/');
                string fullSavePath = Path.Combine(Application.dataPath, relativePath);
                if (!Directory.Exists(fullSavePath)) return;
                
                var files = Directory.GetFiles(fullSavePath, "*.unity")
                    .Select(f => new FileInfo(f))
                    .OrderByDescending(f => f.CreationTime)
                    .ToList();
                
                if (files.Count > maxSaveFiles)
                {
                    var filesToDelete = files.Skip(maxSaveFiles);
                    foreach (var file in filesToDelete)
                    {
                        // Delete the .unity file
                        File.Delete(file.FullName);
                        Debug.Log($"[AutoSaver] Deleted old file: {file.Name}");
                        
                        // delete the corresponding .meta file
                        string metaFilePath = file.FullName + ".meta";
                        if (File.Exists(metaFilePath))
                        {
                            File.Delete(metaFilePath);
                            Debug.Log($"[AutoSaver] Deleted meta file: {file.Name}.meta");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"[AutoSaver] Cleanup failed: {e.Message}");
            }
        }
        
        private static void CreateSaveDirectory()
        {
            string relativePath = savePath.Replace("Assets/", "").TrimEnd('/');
            string fullSavePath = Path.Combine(Application.dataPath, relativePath);
            if (!Directory.Exists(fullSavePath))
            {
                try
                {
                    Directory.CreateDirectory(fullSavePath);
                    Debug.Log($"[AutoSaver] Created save directory: {fullSavePath}");
                }
                catch (Exception e)
                {
                    Debug.LogError($"[AutoSaver] Failed to create save directory: {e.Message}");
                }
            }
        }
        
        private static void LoadSettings()
        {
            isEnabled = EditorPrefs.GetBool("AutoSaver.Enabled", true);
            saveIntervalMinutes = EditorPrefs.GetFloat("AutoSaver.Interval", 5f);
            savePath = EditorPrefs.GetString("AutoSaver.Path", "Assets/AutoSave/");
            maxSaveFiles = EditorPrefs.GetInt("AutoSaver.MaxFiles", 10);
            isInitialized = true;
        }
        
        public static void SaveSettings()
        {
            EditorPrefs.SetBool("AutoSaver.Enabled", isEnabled);
            EditorPrefs.SetFloat("AutoSaver.Interval", saveIntervalMinutes);
            EditorPrefs.SetString("AutoSaver.Path", savePath);
            EditorPrefs.SetInt("AutoSaver.MaxFiles", maxSaveFiles);
        }
        
        // Public properties for the window to access
        public static bool IsEnabled 
        { 
            get => isEnabled; 
            set { isEnabled = value; SaveSettings(); }
        }
        
        public static float SaveIntervalMinutes 
        { 
            get => saveIntervalMinutes; 
            set { saveIntervalMinutes = Mathf.Max(0.1f, value); SaveSettings(); }
        }
        
        public static string SavePath 
        { 
            get => savePath; 
            set { savePath = value; SaveSettings(); }
        }
        
        public static int MaxSaveFiles 
        { 
            get => maxSaveFiles; 
            set { maxSaveFiles = Mathf.Max(1, value); SaveSettings(); }
        }
    }
}
