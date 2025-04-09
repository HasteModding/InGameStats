using Landfall.Haste;
using Landfall.Modding;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using Zorro.Settings;
using Object = UnityEngine.Object;
using TMPro;

namespace InGameStats;

/// <summary>
/// Enum of available stats
/// </summary>
public enum StatType {
    PerfectLandingStreak,
    BestLandingStreak,
    DistanceTraveled,
    Luck,
    Boost,
    Health,
    MaxHealth,
    MaxEnergy,
    PickupRange,
    Shard,
    Level,
    Seed,
}

/// <summary>
/// InGameStats plugin class. This is the entry point for the mod.
/// </summary>
[LandfallPlugin]
public class InGameStatsProgram {
    public static string GetCategory() => "InGameStats";

    static InGameStatsProgram() {
        GameObject instance = new (nameof(InGameStats));
        Object.DontDestroyOnLoad(instance);
        instance.AddComponent<InGameStats>();
    }
}

/// <summary>
/// Stats display plugin object.
/// </summary>
[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(CanvasScaler))]
[RequireComponent(typeof(GraphicRaycaster))]
public class InGameStats : MonoBehaviour {
    public static InGameStats Instance { get; private set; } = null!;

    // Positioning and font size settings
    public float yBaseOffset = 10f;
    public float xBaseOffset = 10f;
    public int fontSize = 12;

    // Manual streak stats
    public int perfectLandingStreak = 0;
    public int bestPerfectLandingStreak = 0;

    // Flag to check if the perfect counter is initialized
    // This is used to avoid multiple subscriptions to the landing action
    // and to ensure that the perfect counter is only initialized once
    public bool perfectCounterInit = true;

    // List of enabled stats
    // This list is populated by the settings and is used to determine which stats to display
    public List<StatType> enabledStats = new() {
        StatType.PerfectLandingStreak,
        StatType.BestLandingStreak,
        StatType.DistanceTraveled,
        StatType.Luck,
        StatType.Boost,
        StatType.Health,
        StatType.MaxHealth,
        StatType.MaxEnergy,
        StatType.PickupRange,
        StatType.Shard,
        StatType.Level,
        StatType.Seed,
    };

    // Contains the Canvas stats will be displayed on
    private Canvas? _canvas;

    // Dictionary to hold the TextMeshProUGUI components for each stat
    private Dictionary<StatType, TextMeshProUGUI> _statTexts = new();

    // Dictionary to hold the display names for each stat
    public static readonly Dictionary<StatType, string> statDisplayNames = new() {
        { StatType.PerfectLandingStreak, "Perfect Streak" },
        { StatType.BestLandingStreak, "Best Streak" },
        { StatType.DistanceTraveled, "Distance Traveled" },
        { StatType.Luck, "Luck" },
        { StatType.Boost, "Boost" },
        { StatType.Health, "Health" },
        { StatType.MaxHealth, "Max Health" },
        { StatType.MaxEnergy, "Max Energy" },
        { StatType.PickupRange, "Pickup Range" },
        { StatType.Shard, "Shard" },
        { StatType.Level, "Level" },
        { StatType.Seed, "Seed" },
    };

    // Event handler for landing action (update the perfect landing streak)
    private void OnLanding(LandingType landingType, bool saved) {
        if (landingType == LandingType.Perfect) {
            if (++perfectLandingStreak > bestPerfectLandingStreak)
                bestPerfectLandingStreak = perfectLandingStreak;
        }
        else
            perfectLandingStreak = 0;
    }

    private void OnStartNewRun() {
        // Reset the perfect landing streaks when a new run starts
        perfectLandingStreak = 0;
        bestPerfectLandingStreak = 0;
    }

    private void OnNewLevel() {
        // Check if the player is not null and if the character is not null
        var movement = Player.localPlayer?.character.refs.movement;
        if (movement != null) {
            movement.landAction = (Action<LandingType, bool>)Delegate.Remove(movement.landAction, new Action<LandingType, bool>(OnLanding));
            movement.landAction = (Action<LandingType, bool>)Delegate.Combine(movement.landAction, new Action<LandingType, bool>(OnLanding));
            perfectCounterInit = true;
        }
    }

    // Awake is called when the script instance is being loaded
    private void Awake() {
        Instance = this;
        On.GM_API.OnStartNewRun += (_) => OnStartNewRun();
        On.GM_API.OnNewLevel += (_) => OnNewLevel();
        
        // Set up the Canvas
        _canvas = GetComponent<Canvas>();
        _canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        CanvasScaler canvasScaler = GetComponent<CanvasScaler>();
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2(1920, 1080);

        // Create UI elements for each enabled stat
        CreateStatUI();
    }

    // Create the UI elements for each enabled stat
    // This method is called when the mod is loaded and when settings are changed
    public void CreateStatUI() {
        // Destroy existing stat texts
        foreach (KeyValuePair<StatType, TextMeshProUGUI> text in _statTexts)
            Destroy(text.Value.gameObject);
        _statTexts.Clear();

        // Create new stat texts based on the enabled stats
        float yOffset = yBaseOffset;
        foreach (StatType stat in enabledStats) {
            GameObject statObject = new(stat.ToString());
            statObject.transform.SetParent(_canvas!.transform);

            TextMeshProUGUI text = statObject.AddComponent<TextMeshProUGUI>();
            text.fontSize = fontSize;
            text.color = Color.white;
            // TODO: AkzidenzGroteskPro-BoldCnIt SDF font should be used for the text
            // text.font = TMP_FontAsset.CreateFontAsset(new Font)

            RectTransform rectTransform = text.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0, 1);
            rectTransform.anchorMax = new Vector2(0, 1);
            rectTransform.pivot = new Vector2(0, 1);
            rectTransform.anchoredPosition = new Vector2(xBaseOffset, -yOffset);
            rectTransform.sizeDelta = new Vector2(Screen.width, fontSize + 5f);

            _statTexts[stat] = text;
            // Add spacing for the next stat depending on the font size
            yOffset += fontSize + 5f;
        }
    }

    private void Update() {
        // Update the stat texts with the current values
        foreach (StatType stat in enabledStats) {
            string value = GetStatValue(stat);
            if (_statTexts.TryGetValue(stat, out TextMeshProUGUI? text))
                text.text = $"{statDisplayNames[stat]}: {value}";
        }
    }

    private static float ComputeStatValue(PlayerStat stat) => stat.baseValue * stat.multiplier;
    
    private static string Percentile(float percentile) => (percentile * 100).ToString("F2") + "%";
    
    private string GetStatValue(StatType stat) {
        Player? player = Player.localPlayer;

        if (!player) return "N/A";

        PlayerStats stats = player.stats;
        PersistentPlayerData persistentData = player.data;
        PlayerCharacter.PlayerData characterData = player.character.data;

        return stat switch {
            StatType.PerfectLandingStreak => perfectLandingStreak.ToString(),
            StatType.BestLandingStreak => bestPerfectLandingStreak.ToString(),
            StatType.DistanceTraveled => persistentData.distanceTraveled.ToString("F1"),
            StatType.Luck => Percentile(ComputeStatValue(stats.luck)),
            StatType.Boost => Percentile(characterData.GetBoost()),
            StatType.Health => persistentData.currentHealth.ToString("F1"),
            StatType.MaxHealth => ComputeStatValue(stats.maxHealth).ToString("F1"),
            StatType.MaxEnergy => ComputeStatValue(stats.maxEnergy).ToString("F1"),
            StatType.PickupRange => Percentile(ComputeStatValue(stats.sparkPickupRange)),
            StatType.Shard => (RunHandler.RunData.shardID + 1).ToString(),
            StatType.Level => (RunHandler.RunData.currentLevel + 1).ToString(),
            StatType.Seed => RunHandler.RunData.currentSeed.ToString(),
            _ => "N/A"
        };
    }
}

[HasteSetting]
public class XBaseOffsetSetting : FloatSetting, IExposedSetting {
    public override void ApplyValue() => Debug.Log($"XBaseOffset: {Value}");
    protected override float GetDefaultValue() => 10f;
    public LocalizedString GetDisplayName() => new UnlocalizedString("X Base Offset");
    public string GetCategory() => InGameStatsProgram.GetCategory();

    protected override float2 GetMinMaxValue() => new (0f, Screen.height);

    public override void Load(ISettingsSaveLoad loader) {
        base.Load(loader);
        InGameStats.Instance.xBaseOffset = Value;
    }
}

[HasteSetting]
public class YBaseOffsetSetting : FloatSetting, IExposedSetting {
    public override void ApplyValue() => Debug.Log($"YBaseOffset: {Value}");
    protected override float GetDefaultValue() => 0f;
    public LocalizedString GetDisplayName() => new UnlocalizedString("Y Base Offset");
    public string GetCategory() => InGameStatsProgram.GetCategory();

    protected override float2 GetMinMaxValue() => new (0f, Screen.height);
    
    public override void Load(ISettingsSaveLoad loader) {
        base.Load(loader);
        InGameStats.Instance.yBaseOffset = Value;
        InGameStats.Instance.CreateStatUI();
    }
}

[HasteSetting]
public class FontSizeSetting : IntSetting, IExposedSetting {
    public override void ApplyValue() => Debug.Log($"FontSize: {Value}");
    protected override int GetDefaultValue() => 12;
    public LocalizedString GetDisplayName() => new UnlocalizedString("Font Size");
    public string GetCategory() => InGameStatsProgram.GetCategory();

    public override void Load(ISettingsSaveLoad loader) {
        base.Load(loader);
        InGameStats.Instance.fontSize = Value;
        InGameStats.Instance.CreateStatUI();
    }
}

public abstract class EnableStatSetting : BoolSetting, IExposedSetting {
    public override LocalizedString OffString { get; } = new UnlocalizedString("Off");
    public override LocalizedString OnString { get; } = new UnlocalizedString("On");
    public override void ApplyValue() => Debug.Log($"Enable {InGameStats.statDisplayNames[StatType]}: {Value}");
    protected override bool GetDefaultValue() => true;
    public string GetCategory() => InGameStatsProgram.GetCategory();

    protected abstract StatType StatType { get; }
    public LocalizedString GetDisplayName() => new UnlocalizedString(InGameStats.statDisplayNames[StatType]);

    public override void Load(ISettingsSaveLoad loader) {
        base.Load(loader);
        InGameStats.Instance.enabledStats.Remove(StatType);
        if (Value)
            InGameStats.Instance.enabledStats.Add(StatType);
        else
            InGameStats.Instance.CreateStatUI();
    }
}

[HasteSetting]
public class PerfectLandingStreakSetting : EnableStatSetting {
    protected override StatType StatType => StatType.PerfectLandingStreak;
}

[HasteSetting]
public class BestLandingStreakSetting : EnableStatSetting {
    protected override StatType StatType => StatType.BestLandingStreak;
}

[HasteSetting]
public class DistanceTraveledSetting : EnableStatSetting {
    protected override StatType StatType => StatType.DistanceTraveled;
}

[HasteSetting]
public class LuckSetting : EnableStatSetting {
    protected override StatType StatType => StatType.Luck;
}

[HasteSetting]
public class BoostSetting : EnableStatSetting {
    protected override StatType StatType => StatType.Boost;
}

[HasteSetting]
public class HealthSetting : EnableStatSetting {
    protected override StatType StatType => StatType.Health;
}

[HasteSetting]
public class MaxHealthSetting : EnableStatSetting {
    protected override StatType StatType => StatType.MaxHealth;
}

[HasteSetting]
public class MaxEnergySetting : EnableStatSetting {
    protected override StatType StatType => StatType.MaxEnergy;
}

[HasteSetting]
public class PickupRangeSetting : EnableStatSetting {
    protected override StatType StatType => StatType.PickupRange;
}

[HasteSetting]
public class ShardSetting : EnableStatSetting {
    protected override StatType StatType => StatType.Shard;
}

[HasteSetting]
public class LevelSetting : EnableStatSetting {
    protected override StatType StatType => StatType.Level;
}

[HasteSetting]
public class SeedSetting : EnableStatSetting {
    protected override StatType StatType => StatType.Seed;
}

[HasteSetting]
public class ApplySettings : ButtonSetting, IExposedSetting {
    public override void OnClicked() {
        Debug.Log("ApplySettings clicked");
        InGameStats.Instance.yBaseOffset = GameHandler.Instance.SettingsHandler.GetSetting<YBaseOffsetSetting>().Value;
        InGameStats.Instance.xBaseOffset = GameHandler.Instance.SettingsHandler.GetSetting<XBaseOffsetSetting>().Value;
        InGameStats.Instance.fontSize = GameHandler.Instance.SettingsHandler.GetSetting<FontSizeSetting>().Value;
        InGameStats.Instance.enabledStats.Clear();
        if (GameHandler.Instance.SettingsHandler.GetSetting<PerfectLandingStreakSetting>().Value)
            InGameStats.Instance.enabledStats.Add(StatType.PerfectLandingStreak);
        if (GameHandler.Instance.SettingsHandler.GetSetting<BestLandingStreakSetting>().Value)
            InGameStats.Instance.enabledStats.Add(StatType.BestLandingStreak);
        if (GameHandler.Instance.SettingsHandler.GetSetting<LuckSetting>().Value)
            InGameStats.Instance.enabledStats.Add(StatType.Luck);
        if (GameHandler.Instance.SettingsHandler.GetSetting<BoostSetting>().Value)
            InGameStats.Instance.enabledStats.Add(StatType.Boost);
        if (GameHandler.Instance.SettingsHandler.GetSetting<HealthSetting>().Value)
            InGameStats.Instance.enabledStats.Add(StatType.Health);
        if (GameHandler.Instance.SettingsHandler.GetSetting<MaxHealthSetting>().Value)
            InGameStats.Instance.enabledStats.Add(StatType.MaxHealth);
        if (GameHandler.Instance.SettingsHandler.GetSetting<MaxEnergySetting>().Value)
            InGameStats.Instance.enabledStats.Add(StatType.MaxEnergy);
        if (GameHandler.Instance.SettingsHandler.GetSetting<PickupRangeSetting>().Value)
            InGameStats.Instance.enabledStats.Add(StatType.PickupRange);
        if (GameHandler.Instance.SettingsHandler.GetSetting<ShardSetting>().Value)
            InGameStats.Instance.enabledStats.Add(StatType.Shard);
        if (GameHandler.Instance.SettingsHandler.GetSetting<LevelSetting>().Value)
            InGameStats.Instance.enabledStats.Add(StatType.Level);
        if (GameHandler.Instance.SettingsHandler.GetSetting<SeedSetting>().Value)
            InGameStats.Instance.enabledStats.Add(StatType.Seed);
        InGameStats.Instance.CreateStatUI();
    }

    public string GetCategory() => InGameStatsProgram.GetCategory();
    public LocalizedString GetDisplayName() => new UnlocalizedString("Apply Settings");
    public override string GetButtonText() => "Apply Settings";
}
