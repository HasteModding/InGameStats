using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Reflection;

namespace InGameStats;

/// <summary>
/// Stats display plugin object.
/// </summary>
[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(CanvasScaler))]
[RequireComponent(typeof(GraphicRaycaster))]
public class InGameStats : MonoBehaviour {
    /// <summary>
    /// Singleton instance of the InGameStats class.
    /// This is used to access the instance of the class from other scripts.
    /// </summary>
    public static InGameStats Instance { get; private set; } = null!;

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
    /// Whether the stats should be displayed on the right or left side of the screen.
    /// </summary>
    public bool rightSide = false;
    /// <summary>
    /// Indicates if only the true perfect landings should be counted.
    /// </summary>
    public bool strictPerfectLanding = false;
    /// <summary>
    /// Indicates if the colors should be used for the stats text.
    /// </summary>
    public bool colors = true;
    /// <summary>
    /// Indicates if the game font should be used for the stats text.
    /// </summary>
    public bool font = true;
    /// <summary>
    /// This list contains the StatType enum values for the stats that will be displayed.
    /// </summary>
    public List<StatType> enabledStats = new() {
        StatType.PerfectLandingStreak,
        StatType.BestLandingStreak,
        StatType.AverageLandingScore,
        StatType.DistanceTravelled,
        StatType.GroundDistanceTravelled,
        StatType.AirDistanceTravelled,
        StatType.Luck,
        StatType.Boost,
        StatType.Health,
        StatType.MaxHealth,
        StatType.MaxEnergy,
        StatType.PickupRange,
        StatType.Speed,
        StatType.LevelSpeed,
        StatType.PropBudget,
        StatType.Shard,
        StatType.Level,
        StatType.Seed,
        StatType.NoHit,
        StatType.NoDeath,
        StatType.NoItems,
        StatType.OnlyPerfectLanding,
        StatType.OnlySRanks,
    };

    private int _perfectLandingStreak = 0;
    private float _averageLandingScore = 0f;
    private int _landingCount = 0;
    private int _bestPerfectLandingStreak = 0;

    private bool _noHit = true;
    private bool _noDeath = true;
    private bool _noItems = true;
    private bool _onlyPerfectLanding = true;
    private bool _onlySRanks = true;
    private TMP_FontAsset _fontAsset = null!;

    private Canvas? _canvas;
    private readonly Dictionary<StatType, TextMeshProUGUI> _statTexts = new();

    private void HandleLanding(LandingType landingType) {
        if (landingType == LandingType.Perfect) {
            _perfectLandingStreak++;
            if (_perfectLandingStreak > _bestPerfectLandingStreak) {
                if (colors && _statTexts.TryGetValue(StatType.PerfectLandingStreak, out TextMeshProUGUI? text))
                    text.color = Color.yellow;
                _bestPerfectLandingStreak = _perfectLandingStreak;
            }
        } else {
            if (colors && _statTexts.TryGetValue(StatType.PerfectLandingStreak, out TextMeshProUGUI? text))
                text.color = Color.white;
            _perfectLandingStreak = 0;
        }
    }

    private void OnLanding(LandingType landingType, bool saved) {
        if (strictPerfectLanding) return;
        HandleLanding(landingType);
    }

    private void OnLanding(object landing) {
        const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        Type landingClassType = landing.GetType();
        FieldInfo landingScoreField = landingClassType.GetField("landingScore", bindingFlags);
        FieldInfo sinceGroundedBeforeLandingField = landingClassType.GetField("sinceGroundedBeforeLanding", bindingFlags);
        if ((float) sinceGroundedBeforeLandingField.GetValue(landing) > 0.6f) {
            // Update the average landing score and count
            float landingScore = (float) landingScoreField.GetValue(landing);
            float difficultyTweak = Mathf.Lerp(-0.05f, 0.0f, GameDifficulty.currentDif.landingPresicion);
            _landingCount++;
            _averageLandingScore += (landingScore - _averageLandingScore) / _landingCount;
            // Update the perfect landing streaks if in strict mode
            if (!strictPerfectLanding) return;
            LandingType landingType = LandingType.Bad;
            if (landingScore >= 0.7f + difficultyTweak) landingType = LandingType.Ok;
            if (landingScore >= 0.9f + difficultyTweak) landingType = LandingType.Good;
            if (landingScore >= 0.95f + difficultyTweak) landingType = LandingType.Perfect;
            HandleLanding(landingType);
        }
    }

    private void OnStartNewRun() {
        // Reset the perfect landing streaks when a new run starts
        _perfectLandingStreak = 0;
        _bestPerfectLandingStreak = 0;
        _averageLandingScore = 0f;
        _landingCount = 0;
        _noHit = true;
        if (colors && _statTexts.TryGetValue(StatType.NoHit, out TextMeshProUGUI? text))
            text.color = Color.green;
        _noDeath = true;
        if (colors && _statTexts.TryGetValue(StatType.NoDeath, out TextMeshProUGUI? statText))
            statText.color = Color.green;
        _noItems = true;
        if (colors && _statTexts.TryGetValue(StatType.NoItems, out TextMeshProUGUI? text1))
            text1.color = Color.green;
        _onlyPerfectLanding = true;
        if (colors && _statTexts.TryGetValue(StatType.OnlyPerfectLanding, out TextMeshProUGUI? statText1))
            statText1.color = Color.green;
        _onlySRanks = true;
        if (colors && _statTexts.TryGetValue(StatType.OnlySRanks, out TextMeshProUGUI? text2))
            text2.color = Color.green;
    }

    private void OnNewLevel() {
        // Check if the player is not null and if the character is not null
        Player player = Player.localPlayer;
        if (player == null) return;

        PlayerMovement movement = player.character.refs.movement;
        movement.landAction -= OnLanding;
        movement.landAction += OnLanding;
    }

    private void Awake() {
        Instance = this;
        GM_API.StartNewRun += OnStartNewRun;
        GM_API.SpawnedInHub += OnStartNewRun;
        GM_API.NewLevel += OnNewLevel;
        GM_API.MainMenuPlayButton += CreateStatUI;
        On.PlayerMovement.GetLanding += (orig, self, hit) => {
            object landing = orig(self, hit);
            OnLanding(landing);
            return landing;
        };

        // Set up the Canvas
        _canvas = GetComponent<Canvas>();
        _canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        CanvasScaler canvasScaler = GetComponent<CanvasScaler>();
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2(Screen.width, Screen.height);
    }

    private void Update() {
        if (_noDeath && !InGameStatsUtils.GetNoDeath()) {
            _noDeath = false;
            if (colors && _statTexts.TryGetValue(StatType.NoDeath, out TextMeshProUGUI? text))
                text.color = Color.red;
        }
        if (_noItems && !InGameStatsUtils.GetNoItems()) {
            _noItems = false;
            if (colors && _statTexts.TryGetValue(StatType.NoItems, out TextMeshProUGUI? text))
                text.color = Color.red;
        }
        if (_noHit && !InGameStatsUtils.GetDamageTaken()) {
            _noHit = false;
            if (colors && _statTexts.TryGetValue(StatType.NoHit, out TextMeshProUGUI? text))
                text.color = Color.red;
        }
        if (_onlyPerfectLanding && !InGameStatsUtils.GetOnlyPerfectLanding()) {
            _onlyPerfectLanding = false;
            if (colors && _statTexts.TryGetValue(StatType.OnlyPerfectLanding, out TextMeshProUGUI? text))
                text.color = Color.red;
        }
        if (_onlySRanks && !InGameStatsUtils.GetOnlySRank()) {
            _onlySRanks = false;
            if (colors && _statTexts.TryGetValue(StatType.OnlySRanks, out TextMeshProUGUI? text))
                text.color = Color.red;
        }

        // Update the stat texts with the current values
        foreach (StatType stat in enabledStats)
            if (_statTexts.TryGetValue(stat, out TextMeshProUGUI? text) && text != null) {
                try {
                    string value = GetStatValue(stat);
                    text.text = $"{InGameStatsUtils.statDisplayNames[stat]}: {value}";
                } catch {
                    text.text = $"{InGameStatsUtils.statDisplayNames[stat]}: N/A";
                }
            }
    }
    
    /// <summary>
    /// Creates the empty UI elements for the enabled stats.
    /// This method is called when the mod is loaded or when the settings are changed.
    /// </summary>
    public void CreateStatUI() {
        // Destroy existing stat texts
        foreach (KeyValuePair<StatType, TextMeshProUGUI> text in _statTexts)
            Destroy(text.Value.gameObject);
        _statTexts.Clear();

        // Create new stat texts based on the enabled stats
        float yOffset = yBaseOffset;
        foreach (StatType stat in Enum.GetValues(typeof(StatType))) {
            if (!enabledStats.Contains(stat)) continue;
            // Create the stat text
            GameObject statObject = new(stat.ToString());
            statObject.transform.SetParent(_canvas!.transform);

            TextMeshProUGUI text = statObject.AddComponent<TextMeshProUGUI>();

            RectTransform rectTransform = text.GetComponent<RectTransform>();
            if (rightSide) {
                rectTransform.anchorMin = new Vector2(1, 1);
                rectTransform.anchorMax = new Vector2(1, 1);
                rectTransform.pivot = new Vector2(1, 1);
                rectTransform.anchoredPosition = new Vector2(-xBaseOffset, -yOffset);
            } else {
                rectTransform.anchorMin = new Vector2(0, 1);
                rectTransform.anchorMax = new Vector2(0, 1);
                rectTransform.pivot = new Vector2(0, 1);
                rectTransform.anchoredPosition = new Vector2(xBaseOffset, -yOffset);
            }
            rectTransform.sizeDelta = new Vector2(Screen.width / 3f, fontSize + 5f);
            // Set text alignment based on the side
            text.alignment = rightSide ? TextAlignmentOptions.Right : TextAlignmentOptions.Left;
            // Set the text properties
            text.fontSize = fontSize;
            text.color = Color.white;
            text.outlineColor = Color.black;
            text.text = "N/A";

            if (font) {
                // Set the font asset if it is not already set
                if (_fontAsset == null) {
                    TMP_FontAsset[] fonts = Resources.FindObjectsOfTypeAll<TMP_FontAsset>();
                    Debug.Log($"Found {fonts.Length} font assets.");
                    foreach (TMP_FontAsset font in fonts) {
                        Debug.Log($"Font name: {font.name}");
                        if (font.name == "AkzidenzGroteskPro-Bold SDF") {
                            _fontAsset = font;
                            break;
                        }
                    }
                    if (_fontAsset == null) {
                        Debug.LogError("Font asset not found. Using default font.");
                    } else {
                        Debug.Log($"Font asset found: {_fontAsset.name}");
                    }
                }

                // Set the font asset to the text component
                // Check if the font asset is not null before assigning it to the text component
                if (_fontAsset != null) {
                    text.font = _fontAsset;
                    text.fontSharedMaterial = _fontAsset.material;
                    text.fontSharedMaterial.mainTexture = _fontAsset.material.mainTexture;
                }
            }

            if (colors) {
                // Set the color of the text based on the stat type
                switch (stat) {
                    case StatType.BestLandingStreak:
                        text.color = Color.yellow;
                        break;
                    case StatType.NoHit:
                        text.color = _noHit ? Color.green : Color.red;
                        break;
                    case StatType.NoDeath:
                        text.color = _noDeath ? Color.green : Color.red;
                        break;
                    case StatType.NoItems:
                        text.color = _noItems ? Color.green : Color.red;
                        break;
                    case StatType.OnlyPerfectLanding:
                        text.color = _onlyPerfectLanding ? Color.green : Color.red;
                        break;
                    case StatType.OnlySRanks:
                        text.color = _onlySRanks ? Color.green : Color.red;
                        break;
                }
            }

            _statTexts[stat] = text;
            // Add spacing for the next stat depending on the font size
            yOffset += fontSize + 5f;
        }
    }

    /// <summary>
    /// Gets the value of a specific stat based on the provided StatType.
    /// </summary>
    /// <param name="stat">The StatType of the stat to retrieve.</param>
    /// <returns>The string representation of the stat value.</returns>
    private string GetStatValue(StatType stat) {
        Player? player = Player.localPlayer;
        return stat switch {
            StatType.PerfectLandingStreak => _perfectLandingStreak.ToString() + (strictPerfectLanding ? " (Strict)" : ""),
            StatType.BestLandingStreak => _bestPerfectLandingStreak.ToString(),
            StatType.AverageLandingScore => InGameStatsUtils.Percentile(_averageLandingScore),
            StatType.DistanceTravelled => InGameStatsUtils.GetTotalDistance().ToString("F1") + " m",
            StatType.GroundDistanceTravelled => InGameStatsUtils.GetGroundDistance().ToString("F1") + " m",
            StatType.AirDistanceTravelled => InGameStatsUtils.GetAirDistance().ToString("F1") + " m",
            StatType.Luck => InGameStatsUtils.Percentile(InGameStatsUtils.ComputeStatValue(player.stats.luck)),
            StatType.Boost => InGameStatsUtils.Percentile(player.character.data.GetBoost()),
            StatType.Health => player.data.currentHealth.ToString("F1"),
            StatType.MaxHealth => InGameStatsUtils.ComputeStatValue(player.stats.maxHealth).ToString("F1"),
            StatType.MaxEnergy => InGameStatsUtils.ComputeStatValue(player.stats.maxEnergy).ToString("F1"),
            StatType.PickupRange => InGameStatsUtils.Percentile(InGameStatsUtils.ComputeStatValue(player.stats.sparkPickupRange)),
            StatType.Speed => player.character.refs.rig.velocity.magnitude.ToString("F1") + " m/s",
            StatType.LevelSpeed => RunHandler.GetLevelSpeed().ToString("F1") + " m/s",
            StatType.PropBudget => LevelGenerator.instance.config.keyPropBudget.ToString(),
            StatType.Shard => RunHandler.isEndless ? "Endless" : (RunHandler.RunData.shardID + 1).ToString(),
            StatType.Level => InGameStatsUtils.GetLevelStats(),
            StatType.Seed => RunHandler.RunData.currentSeed.ToString(),
            StatType.NoDeath => _noDeath ? "Yes" : "No",
            StatType.NoItems => _noItems ? "Yes" : "No",
            StatType.NoHit => _noHit ? "Yes" : "No",
            StatType.OnlyPerfectLanding => _onlyPerfectLanding ? "Yes" : "No",
            StatType.OnlySRanks => _onlySRanks ? "Yes" : "No",
            _ => "N/A"
        };
    }
}
