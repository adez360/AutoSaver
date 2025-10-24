using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System.Linq;

namespace AutoSaver
{

    public class AutoSaverWindow : EditorWindow
    {        
        // UI Elements
        private DropdownField _languageDropdown;
        private Label _titleLabel;
        private Label _settingsLabel;
        private Label _saveIntervalLabel;
        private Label _savePathLabel;
        private Label _maxFilesLabel;
        private Label _actionsLabel;
        private FloatField _intervalField;
        private TextField _pathField;
        private IntegerField _maxFilesField;
        private Button _manualSaveButton;
        private Button _toggleAutosaveButton;

        [MenuItem("Tools/adez360/AutoSaver")]
        public static void ShowWindow()
        {
            var window = GetWindow<AutoSaverWindow>("AutoSaver");
            window.minSize = new Vector2(400, 290);
            window.Show();
        }
        
        private void OnEnable()
        {
            if (rootVisualElement.childCount == 0)
            {
                CreateGUI();
            }
            UpdateUI();
            
            // Subscribe to language change events
            AutoSaverLocalization.OnLanguageChanged += UpdateLocalizedText;
        }

        private void OnDisable()
        {
            // Unsubscribe from language change events
            AutoSaverLocalization.OnLanguageChanged -= UpdateLocalizedText;
        }
        
        private void CreateGUI()
        {
            // Clear existing content
            rootVisualElement.Clear();
            
            // Load UXML
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Packages/com.adez360.autosaver/Editor/AutoSaverWindow.uxml");
            if (visualTree != null)
            {
                visualTree.CloneTree(rootVisualElement);
            }
            else
            {
                Debug.LogError("[AutoSaver] UXML file not found!");
                return;
            }
            
            // Load USS
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Packages/com.adez360.autosaver/Editor/AutoSaverWindow.uss");
            if (styleSheet != null)
            {
                rootVisualElement.styleSheets.Add(styleSheet);
            }
            else
            {
                Debug.LogError("[AutoSaver] USS file not found!");
            }
            
            // Bind UI elements
            BindUIElements();
            
            // Setup event handlers
            SetupEventHandlers();
        }
        
        private void BindUIElements()
        {
            // Find UI elements by name
            _languageDropdown = rootVisualElement.Q<DropdownField>("language-dropdown");
            _titleLabel = rootVisualElement.Q<Label>("title-label");
            _settingsLabel = rootVisualElement.Q<Label>("settings-label");
            _saveIntervalLabel = rootVisualElement.Q<Label>("save-interval-label");
            _savePathLabel = rootVisualElement.Q<Label>("save-path-label");
            _maxFilesLabel = rootVisualElement.Q<Label>("max-files-label");
            _actionsLabel = rootVisualElement.Q<Label>("actions-label");
            _intervalField = rootVisualElement.Q<FloatField>("interval-field");
            _pathField = rootVisualElement.Q<TextField>("path-field");
            _maxFilesField = rootVisualElement.Q<IntegerField>("max-files-field");
            _manualSaveButton = rootVisualElement.Q<Button>("manual-save-button");
            _toggleAutosaveButton = rootVisualElement.Q<Button>("toggle-autosave-button");
        }
        
        private void SetupEventHandlers()
        {
            if (_languageDropdown != null)
            {
                _languageDropdown.choices = AutoSaverLocalization.GetLanguageOptions().ToList();
                _languageDropdown.index = (int)AutoSaverLocalization.CurrentLanguage;
                _languageDropdown.RegisterValueChangedCallback(OnLanguageChanged);
            }
            
            if (_intervalField != null)
                _intervalField.RegisterValueChangedCallback(OnIntervalChanged);
            
            if (_pathField != null)
                _pathField.RegisterValueChangedCallback(OnPathChanged);
            
            if (_maxFilesField != null)
                _maxFilesField.RegisterValueChangedCallback(OnMaxFilesChanged);
            
            if (_manualSaveButton != null)
            {
                _manualSaveButton.clicked += PerformManualSave;
            }
            
            if (_toggleAutosaveButton != null)
            {
                _toggleAutosaveButton.clicked += ToggleAutoSave;
            }
        }    
    
        private void UpdateUI()
        {
            if (_manualSaveButton != null)
                _manualSaveButton.SetEnabled(true);

            // Update localized text
            UpdateLocalizedText();

            // Update settings UI from EditorPrefs
            if (_intervalField != null)
                _intervalField.SetValueWithoutNotify(AutoSaverEditor.SaveIntervalMinutes);

            if (_pathField != null)
                _pathField.SetValueWithoutNotify(AutoSaverEditor.SavePath);

            if (_maxFilesField != null)
                _maxFilesField.SetValueWithoutNotify(AutoSaverEditor.MaxSaveFiles);
        }
        
        #region Event Handlers
        
        private void OnLanguageChanged(ChangeEvent<string> evt)
        {
            AutoSaverLocalization.CurrentLanguage = (AutoSaverLocalization.Language)_languageDropdown.index;
        }

        private void OnIntervalChanged(ChangeEvent<float> evt)
        {
            AutoSaverEditor.SaveIntervalMinutes = evt.newValue;
        }

        private void OnPathChanged(ChangeEvent<string> evt)
        {
            AutoSaverEditor.SavePath = evt.newValue;
        }

        private void OnMaxFilesChanged(ChangeEvent<int> evt)
        {
            AutoSaverEditor.MaxSaveFiles = evt.newValue;
        }
        
        #endregion
        
        #region Button Actions
        
        private void PerformManualSave()
        {
            AutoSaverEditor.PerformManualSave();
        }

        private void ToggleAutoSave()
        {
            AutoSaverEditor.IsEnabled = !AutoSaverEditor.IsEnabled;
            UpdateToggleButton();
        }
        
        private void UpdateToggleButton()
        {
            if (_toggleAutosaveButton != null)
            {
                bool isEnabled = AutoSaverEditor.IsEnabled;
                string onText = AutoSaverLocalization.GetText("autosave_on");
                string offText = AutoSaverLocalization.GetText("autosave_off");
                _toggleAutosaveButton.text = isEnabled ? onText : offText;
                _toggleAutosaveButton.style.backgroundColor = isEnabled ? 
                    new Color(0.169f, 0.463f, 0.169f) : new Color(0.235f, 0.235f, 0.235f);
                
                // Set border colors for all sides
                var borderColor = isEnabled ? 
                    new Color(0.35f, 0.55f, 0.35f) : new Color(0.8f, 0.2f, 0.2f);
                _toggleAutosaveButton.style.borderTopColor = borderColor;
                _toggleAutosaveButton.style.borderRightColor = borderColor;
                _toggleAutosaveButton.style.borderBottomColor = borderColor;
                _toggleAutosaveButton.style.borderLeftColor = borderColor;
            }
        }

        private void UpdateLocalizedText()
        {
            if (_titleLabel != null)
                _titleLabel.text = AutoSaverLocalization.GetText("window_title");
            
            if (_settingsLabel != null)
                _settingsLabel.text = AutoSaverLocalization.GetText("settings");
            
            if (_saveIntervalLabel != null)
                _saveIntervalLabel.text = AutoSaverLocalization.GetText("save_interval");
            
            if (_savePathLabel != null)
                _savePathLabel.text = AutoSaverLocalization.GetText("save_path");
            
            if (_maxFilesLabel != null)
                _maxFilesLabel.text = AutoSaverLocalization.GetText("max_save_files");
            
            if (_actionsLabel != null)
                _actionsLabel.text = AutoSaverLocalization.GetText("actions");
            
            if (_manualSaveButton != null)
                _manualSaveButton.text = AutoSaverLocalization.GetText("manual_save");
            
            UpdateToggleButton();
        }
        

        #endregion
    }
}
