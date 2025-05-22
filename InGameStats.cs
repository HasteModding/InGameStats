using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization;

namespace InGameStats;

/// <summary>
/// Stats display plugin object.
/// </summary>
[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(CanvasScaler))]
[RequireComponent(typeof(GraphicRaycaster))]
public class InGameStats : MonoBehaviour {
    /// <summary>
    /// Category of the plugin for settings.
    /// </summary>
    public static LocalizedString Category = new ("InGameStats", "Setting_Category");
    /// <summary>
    /// Singleton instance of the InGameStats class.
    /// This is used to access the instance of the class from other scripts.
    /// </summary>
    public static InGameStats Instance { get; private set; } = null!;

    /// <summary>
    /// Indicates if the stats should be displayed only in runs.
    /// </summary>
    public DisplayMode displayMode = DisplayMode.Always;
    /// <summary>
    /// The base Y offset for the stats text.
    /// </summary>
    public float yBaseOffset = 10f;
    /// <summary>
    /// The base Y offset for the stats text.
    /// </summary>
    public float xBaseOffset = 10f;
    /// <summary>
    /// The font size of the stats text.
    /// </summary>
    public int fontSize = 12;
    /// <summary>
    /// What alignment to use for the stats text.
    /// </summary>
    public AlignmentMode alignmentMode = AlignmentMode.Left;
    /// <summary>
    /// Indicates if the colors should be used for the stats text.
    /// </summary>
    public ColorizedMode colorizedMode = ColorizedMode.Colorized;
    /// <summary>
    /// Indicates if the game font should be used for the stats text.
    /// </summary>
    public FontMode fontMode = FontMode.GameFont;
    /// <summary>
    /// Indicates if the stats should be outlined.
    /// </summary>
    public OutlineMode outlineMode = OutlineMode.Outline;
    private TMP_FontAsset _fontAsset = null!;
    /// <summary>
    /// Dictionary of types (heriting from Stat) to their TextMeshProUGUI objects.
    /// </summary>
    public Dictionary<Type, TextMeshProUGUI> StatTextObjects { get; } = new();
    /// <summary>
    /// Dictionary of types (heriting from Stat) to their instance.
    /// </summary>
    public static Dictionary<Type, IG_Stat> StatInstances { get; } = new();
    /// <summary>
    /// Set of types (heriting from Stat) handled by the plugin.
    /// </summary>
    public static HashSet<Type> StatTypes { get; } = new();

    private Canvas? _canvas;

    private void Awake() {
        Instance = this;
        GM_API.StartNewRun += CallOnStartNewRun;
        GM_API.SpawnedInHub += CallOnSpawnedInHub;
        GM_API.NewLevel += CallOnNewLevel;
        GM_API.MainMenuPlayButton += CallOnMainMenuPlayButton;
        On.PlayerMovement.GetLanding += (orig, self, hit) => {
            object landing = orig(self, hit);
            if (landing != null) {
                CallOnHitLandingObjectComputed(landing);
                float landingScore = Utils.ComputeLandingScore(landing);
                if (landingScore >= 0) {
                    CallOnLandingScoreComputed(landingScore);
                }
            }
            return landing;
        };

        // Set up the Canvas
        _canvas = GetComponent<Canvas>();
        _canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        _canvas.sortingOrder = 1500;

        CanvasScaler canvasScaler = GetComponent<CanvasScaler>();
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2(Screen.width, Screen.height);
    }

    private void Update() {
        bool needRecreateUI = false;
        foreach (Type statType in StatTypes) {
            if (StatInstances.TryGetValue(statType, out IG_Stat? instance)) {
                bool isEnabled = instance.Enabled;
                bool hasText = StatTextObjects.TryGetValue(statType, out TextMeshProUGUI? text);
                instance.OnUpdate(text, colorizedMode);
                needRecreateUI |= isEnabled != hasText;
            }
        }

        if (needRecreateUI) {
            RecreateUI();
        }

        if (displayMode == DisplayMode.None || displayMode == DisplayMode.InRun && !RunHandler.InRun) {
            // Hide all stats if not in run
            foreach (TextMeshProUGUI text in StatTextObjects.Values) {
                text.gameObject.SetActive(false);
            }
        } else {
            // Show all stats
            foreach (TextMeshProUGUI text in StatTextObjects.Values) {
                text.gameObject.SetActive(true);
            }
        }
    }
    
    /// <summary>
    /// Creates the empty UI elements for the enabled stats.
    /// This method is called when the mod is loaded or when the settings are changed.
    /// </summary>
    public void RecreateUI() {
        if (fontMode == FontMode.GameFont && _fontAsset == null) {
            _fontAsset = Resources.FindObjectsOfTypeAll<TMP_FontAsset>()
                .FirstOrDefault(font => font.name == "AkzidenzGroteskPro-Bold SDF");

            Debug.Log(_fontAsset != null 
                ? $"Font asset found: {_fontAsset.name}" 
                : "Font asset not found. Using default font.");
        }

        // Destroy all existing stat text objects
        foreach (KeyValuePair<Type, TextMeshProUGUI> record in StatTextObjects) {
            TextMeshProUGUI textObject = record.Value;
            if (StatInstances.TryGetValue(record.Key, out IG_Stat? instance)) {
                instance.OnTextDestroy(textObject);
            }
            Destroy(textObject.gameObject);
        }
        StatTextObjects.Clear();

        // Create new stat text objects for each registered stat type
        float yOffset = yBaseOffset;
        foreach (Type statType in StatTypes) {
            if (StatInstances.TryGetValue(statType, out IG_Stat? instance)) {
                if (!instance.Enabled)
                    continue;

                TextMeshProUGUI text = new GameObject(statType.Name).AddComponent<TextMeshProUGUI>();
                text.transform.SetParent(_canvas!.transform);

                // Set all positioning properties
                RectTransform rectTransform = text.GetComponent<RectTransform>();
                switch (alignmentMode) {
                    case AlignmentMode.Left:
                        rectTransform.anchorMin = new Vector2(0, 1);
                        rectTransform.anchorMax = new Vector2(0, 1);
                        rectTransform.pivot = new Vector2(0, 1);
                        rectTransform.anchoredPosition = new Vector2(xBaseOffset, -yOffset);
                        text.alignment = TextAlignmentOptions.Left;
                        break;
                    case AlignmentMode.Right:
                        rectTransform.anchorMin = new Vector2(1, 1);
                        rectTransform.anchorMax = new Vector2(1, 1);
                        rectTransform.pivot = new Vector2(1, 1);
                        rectTransform.anchoredPosition = new Vector2(-xBaseOffset, -yOffset);
                        text.alignment = TextAlignmentOptions.Right;
                        break;
                    case AlignmentMode.Center:
                        rectTransform.anchorMin = new Vector2(0.5f, 1);
                        rectTransform.anchorMax = new Vector2(0.5f, 1);
                        rectTransform.pivot = new Vector2(0.5f, 1);
                        rectTransform.anchoredPosition = new Vector2(xBaseOffset - Screen.width / 2, -yOffset);
                        text.alignment = TextAlignmentOptions.Center;
                        break;
                }
                rectTransform.sizeDelta = new Vector2(Screen.width / 3f, fontSize + 5f);

                // Set all the text properties
                text.fontSize = fontSize;
                if (fontMode == FontMode.GameFont && _fontAsset != null) {
                    text.font = _fontAsset;
                    text.fontSharedMaterial = _fontAsset.material;
                }
                text.color = Color.white;
                text.outlineColor = Color.black;
                text.text = instance.DefaultText.GetLocalizedString();
                text.raycastTarget = false;
                text.enableWordWrapping = false;

                if (outlineMode == OutlineMode.Outline) {
                    text.outlineWidth = 0.5f;
                }

                StatTextObjects.Add(statType, text);
                yOffset += fontSize + 5f;

                instance.OnTextCreation(text, colorizedMode);
            }
        }
    }

    /// <summary>
    /// Register a new stat class to be handled by the plugin.
    /// </summary>
    public static void RegisterStat<T>() where T : IG_Stat, new() {
        // Check if the stat is already registered
        if (StatTypes.Contains(typeof(T))) {
            Debug.LogWarning($"Stat {typeof(T).Name} is already registered.");
            return;
        }
        IG_Stat instance = Activator.CreateInstance<T>();
        StatTypes.Add(typeof(T));
        StatInstances.Add(typeof(T), instance);
        // If instance is not null, RecreateUI() to update the UI
        Instance?.RecreateUI();
    }

    internal void CallOnStartNewRun() {
        foreach (IG_Stat instance in StatInstances.Values) {
            try {
                instance.OnStartNewRun();
            } catch (Exception e) {
                Debug.LogError($"Error in OnStartNewRun for {instance.GetType().Name}: {e}");
            }
        }
    }

    internal void CallOnNewLevel() {
        // Check if the player is not null and if the character is not null
        Player player = Player.localPlayer;
        if (player == null) return;

        PlayerMovement movement = player.character.refs.movement;
        movement.landAction -= CallOnLandAction;
        movement.landAction += CallOnLandAction;

        foreach (IG_Stat instance in StatInstances.Values) {
            try {
                instance.OnNewLevel();
            } catch (Exception e) {
                Debug.LogError($"Error in OnNewLevel for {instance.GetType().Name}: {e}");
            }
        }
    }

    internal void CallOnSpawnedInHub() {
        foreach (IG_Stat instance in StatInstances.Values) {
            try {
                instance.OnSpawnedInHub();
            } catch (Exception e) {
                Debug.LogError($"Error in OnSpawnedInHub for {instance.GetType().Name}: {e}");
            }
        }
    }

    internal void CallOnMainMenuPlayButton() {
        foreach (IG_Stat instance in StatInstances.Values) {
            try {
                instance.OnMainMenuPlayButton();
            } catch (Exception e) {
                Debug.LogError($"Error in OnMainMenuPlayButton for {instance.GetType().Name}: {e}");
            }
        }
    }

    internal void CallOnLandAction(PlayerCharacter playerCharacter, LandingType landingType, bool saved) {
        foreach (IG_Stat instance in StatInstances.Values) {
            try {
                instance.OnLandAction(playerCharacter, landingType, saved);
            } catch (Exception e) {
                Debug.LogError($"Error in OnLandAction for {instance.GetType().Name}: {e}");
            }
        }
    }

    internal void CallOnHitLandingObjectComputed(object landing) {
        foreach (IG_Stat instance in StatInstances.Values) {
            try {
                instance.OnHitLandingObjectComputed(landing);
            } catch (Exception e) {
                Debug.LogError($"Error in OnHitLandingObjectComputed for {instance.GetType().Name}: {e}");
            }
        }
    }

    internal void CallOnLandingScoreComputed(float landingScore) {
        foreach (IG_Stat instance in StatInstances.Values) {
            try {
                instance.OnLandingScoreComputed(landingScore);
            } catch (Exception e) {
                Debug.LogError($"Error in OnLandingScoreComputed for {instance.GetType().Name}: {e}");
            }
        }
    }
}
