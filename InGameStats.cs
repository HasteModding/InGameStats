﻿using UnityEngine;
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
    /// Indicates if only the true perfect landings should be counted.
    /// </summary>
    public bool strictPerfectLanding = false;
    /// <summary>
    /// The current perfect landing streak.
    /// </summary>
    public int perfectLandingStreak = 0;
    /// <summary>
    /// The best perfect landing streak (across the current run).
    /// </summary>
    public int bestPerfectLandingStreak = 0;

    /// <summary>
    /// Indicates if the current run has no hit.
    /// </summary>
    public bool noHit = true;
    /// <summary>
    /// Indicates if the current run has no death.
    /// </summary>
    public bool noDeath = true;
    /// <summary>
    /// Indicates if the current run has no items.
    /// </summary>
    public bool noItems = true;
    /// <summary>
    /// Indicates if the current run has only perfect landings.
    /// </summary>
    public bool onlyPerfectLanding = true;
    /// <summary>
    /// Indicates if the current run has only S ranks.
    /// </summary>
    public bool onlySRanks = true;

    /// <summary>
    /// List of enabled stats.
    /// This list contains the StatType enum values for the stats that will be displayed.
    /// </summary>
    public List<StatType> enabledStats = new() {
        StatType.PerfectLandingStreak,
        StatType.BestLandingStreak,
        StatType.DistanceTravelled,
        StatType.GroundDistanceTravelled,
        StatType.AirDistanceTravelled,
        StatType.Luck,
        StatType.Boost,
        StatType.Health,
        StatType.MaxHealth,
        StatType.MaxEnergy,
        StatType.PickupRange,
        StatType.Shard,
        StatType.Level,
        StatType.Seed,
        StatType.NoHit,
        StatType.NoDeath,
        StatType.NoItems,
        StatType.OnlyPerfectLanding,
        StatType.OnlySRanks,
    };

    /// <summary>
    /// Reference to the Canvas component.
    /// This is used to set up the UI elements for the stats display.
    /// </summary>
    private Canvas? _canvas;

    /// <summary>
    /// Dictionary to hold the stat texts for each stat type.
    /// This is used to update the text values of the stats in the UI.
    /// </summary>
    private Dictionary<StatType, TextMeshProUGUI> _statTexts = new();

    /// <summary>
    /// Dictionary to hold the display names for each stat type.
    /// </summary>
    public static readonly Dictionary<StatType, string> statDisplayNames = new() {
        { StatType.PerfectLandingStreak, "Perfect Streak" },
        { StatType.BestLandingStreak, "Best Streak" },
        { StatType.DistanceTravelled, "Distance" },
        { StatType.GroundDistanceTravelled, "Ground Distance" },
        { StatType.AirDistanceTravelled, "Air Distance" },
        { StatType.Luck, "Luck" },
        { StatType.Boost, "Boost" },
        { StatType.Health, "Health" },
        { StatType.MaxHealth, "Max Health" },
        { StatType.MaxEnergy, "Max Energy" },
        { StatType.PickupRange, "Pickup Range" },
        { StatType.Shard, "Shard" },
        { StatType.Level, "Level" },
        { StatType.Seed, "Seed" },
        { StatType.NoHit, "No Hit" },
        { StatType.NoDeath, "No Death" },
        { StatType.NoItems, "No Items" },
        { StatType.OnlyPerfectLanding, "Only Perfect Landing" },
        { StatType.OnlySRanks, "Only S Ranks" },
    };

    /// <summary>
    /// Called when the player lands.
    /// This method is used to update the perfect landing streaks when not in strict mode.
    /// </summary>
    /// <param name="landingType">The type of landing (Perfect, Good, Bad)</param>
    /// <param name="saved">Indicates if the landing was saved (unused)</param>
    private void OnLanding(LandingType landingType, bool saved) {
        if (strictPerfectLanding) return;
        if (landingType == LandingType.Perfect) {
            if (++perfectLandingStreak > bestPerfectLandingStreak)
                bestPerfectLandingStreak = perfectLandingStreak;
        }
        else perfectLandingStreak = 0;
    }

    /// <summary>
    /// Called when the player lands.
    /// This method is used to update the perfect landing streaks when in strict mode.
    /// </summary>
    /// <param name="landing">PlayerMovement.Landing internal type with all infos about the landing</param>
    private void OnLanding(object landing) {
        if (!strictPerfectLanding) return;
        const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        Type landingClassType = landing.GetType();
        FieldInfo landingScoreField = landingClassType.GetField("landingScore", bindingFlags);
        FieldInfo sinceGroundedBeforeLandingField = landingClassType.GetField("sinceGroundedBeforeLanding", bindingFlags);
        if ((float) sinceGroundedBeforeLandingField.GetValue(landing) > 0.6f) {
            bool isPerfect = (float) landingScoreField.GetValue(landing) > 0.95f;
            if (isPerfect) {
                perfectLandingStreak++;
                if (perfectLandingStreak > bestPerfectLandingStreak)
                    bestPerfectLandingStreak = perfectLandingStreak;
            } else {
                perfectLandingStreak = 0;
            }
        }
    }

    /// <summary>
    /// Called when a new run (shard) starts.
    /// This method is used to reset the perfect landing streaks.
    /// </summary>
    private void OnStartNewRun() {
        // Reset the perfect landing streaks when a new run starts
        perfectLandingStreak = 0;
        bestPerfectLandingStreak = 0;
        noHit = true;
        noDeath = true;
        noItems = true;
        onlyPerfectLanding = true;
        onlySRanks = true;
    }

    /// <summary>
    /// Called when a new level (fragment) is loaded.
    /// This method is used to set up the landing action for the player character.
    /// </summary>
    private void OnNewLevel() {
        // Check if the player is not null and if the character is not null
        Player player = Player.localPlayer;
        if (player == null) return;

        PlayerMovement movement = player.character.refs.movement;
        movement.landAction = (Action<LandingType, bool>)Delegate.Remove(movement.landAction, new Action<LandingType, bool>(OnLanding));
        movement.landAction = (Action<LandingType, bool>)Delegate.Combine(movement.landAction, new Action<LandingType, bool>(OnLanding));
    }

    interface PlayerMovementLanding {
        float sinceGroundedBeforeLanding { get; }
    }

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// This method is used to initialize the plugin and set up the event handlers.
    /// </summary>
    private void Awake() {
        Instance = this;
        On.GM_API.OnStartNewRun += (orig) => {OnStartNewRun(); orig();};
        On.GM_API.OnSpawnedInHub += (orig) => {OnStartNewRun(); orig();};
        On.GM_API.OnNewLevel += (orig) => {OnNewLevel(); orig();};
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
        canvasScaler.referenceResolution = new Vector2(1920, 1080);

        // Create UI elements for each enabled stat
        CreateStatUI();
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
        foreach (StatType stat in enabledStats) {
            GameObject statObject = new(stat.ToString());
            statObject.transform.SetParent(_canvas!.transform);

            TextMeshProUGUI text = statObject.AddComponent<TextMeshProUGUI>();
            text.fontSize = fontSize;
            text.color = Color.white;
            text.text = "N/A";
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

    /// <summary>
    /// Update is called once per frame.
    /// It updates the stat texts with the current run values.
    /// </summary>
    private void Update() {
        if (noHit && !GetDamageTaken())
            noHit = false;
        if (onlyPerfectLanding && !GetOnlyPerfectLanding())
            onlyPerfectLanding = false;
        if (onlySRanks && !GetOnlySRank())
            onlySRanks = false;

        // Update the stat texts with the current values
        foreach (StatType stat in enabledStats)
            if (_statTexts.TryGetValue(stat, out TextMeshProUGUI? text) && text != null) {
                try {
                    string value = GetStatValue(stat);
                    text.text = $"{statDisplayNames[stat]}: {value}";
                } catch {
                    text.text = $"{statDisplayNames[stat]}: N/A";
                }
            }
    }

    /// <summary>
    /// Computes the value of a stat based on its base value and multiplier.
    /// </summary>
    /// <param name="stat">The PlayerStat object containing the base value and multiplier.</param>
    /// <example>For example, if baseValue is 10 and multiplier is 1.5, the result will be 15.</example>
    /// <returns>The computed stat value.</returns>
    private static float ComputeStatValue(PlayerStat stat) => stat.baseValue * stat.multiplier;
    
    /// <summary>
    /// Converts a percentile value to a string representation.
    /// The value is multiplied by 100 and formatted to two decimal places, followed by a percent sign.
    /// </summary>
    /// <param name="percentile">The percentile value to convert.</param>
    /// <example>0.5f will be converted to "50.00%"</example>
    /// <returns>The formatted string representation of the percentile value.</returns>
    private static string Percentile(float percentile) => (percentile * 100).ToString("F2") + "%";

    /// <summary>
    /// Checks if the current run has taken damage.
    /// This is done by checking the number of damage taken in the current run stats.
    /// </summary>
    /// <returns></returns>
    private static bool GetDamageTaken() {
        return (
            !HasteStats.TryGetRunStat(HasteStatType.STAT_DAMAGE_TAKEN, out int damageTakenCount) ||
            damageTakenCount == 0
        );
    }

    /// <summary>
    /// Checks if the current run has only perfect landings.
    /// This is done by checking the number of landings in the current run and comparing it to the number of perfect landings.
    /// </summary>
    /// <returns></returns>
    private static bool GetOnlyPerfectLanding() {
        return (
            !HasteStats.TryGetRunStat(HasteStatType.STAT_TOTAL_LANDINGS, out int totalLandingCount) ||
            (
                HasteStats.TryGetRunStat(HasteStatType.STAT_PERFECT_LANDINGS, out int perfectLandingCount) &&
                perfectLandingCount >= totalLandingCount
            )
        );
    }

    /// <summary>
    /// Gets the total distance travelled in the current run.
    /// This is done by checking the total distance in the current run stats.
    /// </summary>
    private static float GetTotalDistance() {
        try {
            const BindingFlags bindingFlags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
            FieldInfo? groundDistanceField = typeof(HasteStats).GetField("m_RUN_DistanceTravelled_Ground", bindingFlags);
            FieldInfo? airDistanceField = typeof(HasteStats).GetField("m_RUN_DistanceTravelled_Air", bindingFlags);
            if (groundDistanceField == null || airDistanceField == null)
                throw new MissingMemberException("Fields 'm_RUN_DistanceTravelled_Ground' or 'm_RUN_DistanceTravelled_Air' not found on HasteStats.");
            object? groundValue = groundDistanceField.GetValue(null);
            object? airValue = airDistanceField.GetValue(null);
            if (groundValue is not float groundDistanceValue || airValue is not float airDistanceValue)
                throw new InvalidCastException("Fields 'm_RUN_DistanceTravelled_Ground' or 'm_RUN_DistanceTravelled_Air' are not of type float.");
            return groundDistanceValue + airDistanceValue;
        } catch (Exception ex) {
            Debug.LogError($"Error getting total distance: {ex.Message}");
            return HasteStats.TryGetRunStat(HasteStatType.STAT_TOTAL_DISTANCE, out int totalDistance) ? totalDistance * 1000f : 0f;
        }
    }

    /// <summary>
    /// Gets the ground distance travelled in the current run.
    /// This is done by checking the ground distance in the current run stats.
    /// </summary>
    private static float GetGroundDistance() {
        try {
            const BindingFlags bindingFlags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
            FieldInfo? groundDistanceField = typeof(HasteStats).GetField("m_RUN_DistanceTravelled_Ground", bindingFlags);
            if (groundDistanceField == null)
                throw new MissingMemberException("Field 'm_RUN_DistanceTravelled_Ground' not found on HasteStats.");
            object? value = groundDistanceField.GetValue(null);
            if (value is not float groundDistanceValue)
                throw new InvalidCastException("Field 'm_RUN_DistanceTravelled_Ground' is not of type float.");
            return groundDistanceValue;
        } catch (Exception ex) {
            Debug.LogError($"Error getting ground distance: {ex.Message}");
            return HasteStats.TryGetRunStat(HasteStatType.STAT_DISTANCE_GROUND, out int groundDistance) ? groundDistance * 1000f : 0f;
        }
    }

    /// <summary>
    /// Gets the air distance travelled in the current run.
    /// This is done by checking the air distance in the current run stats.
    /// </summary>
    private static float GetAirDistance() {
        try {
            const BindingFlags bindingFlags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
            FieldInfo? airDistanceField = typeof(HasteStats).GetField("m_RUN_DistanceTravelled_Air", bindingFlags);
            if (airDistanceField == null)
                throw new MissingMemberException("Field 'm_RUN_DistanceTravelled_Air' not found on HasteStats.");
            object? value = airDistanceField.GetValue(null);
            if (value is not float airDistanceValue)
                throw new InvalidCastException("Field 'm_RUN_DistanceTravelled_Air' is not of type float.");
            return airDistanceValue;
        } catch (Exception ex) {
            Debug.LogError($"Error getting air distance: {ex.Message}");
            return HasteStats.TryGetRunStat(HasteStatType.STAT_DISTANCE_AIR, out int airDistance) ? airDistance * 1000f : 0f;
        }
    }

    /// <summary>
    /// Checks if the current run has only S ranks.
    /// This is done by checking the number of ranks not equal to S in the current run stats.
    /// </summary>
    /// <returns></returns>
    private static bool GetOnlySRank() {
        HasteStatType[] unapprovedRanks = {
            HasteStatType.STAT_RANK_A_LEVELS,
            HasteStatType.STAT_RANK_B_LEVELS,
            HasteStatType.STAT_RANK_C_LEVELS,
            HasteStatType.STAT_RANK_D_LEVELS,
            HasteStatType.STAT_RANK_E_LEVELS,
        };

        foreach (HasteStatType rank in unapprovedRanks) {
            if (HasteStats.TryGetRunStat(rank, out int rankCount) && rankCount > 0)
                return false;
        }

        return true;
    }
    
    /// <summary>
    /// Gets the value of a specific stat based on the provided StatType.
    /// </summary>
    /// <param name="stat">The StatType of the stat to retrieve.</param>
    /// <returns>The string representation of the stat value.</returns>
    private string GetStatValue(StatType stat) {
        Player? player = Player.localPlayer;

        if (!player) return "N/A";

        return stat switch {
            StatType.PerfectLandingStreak => perfectLandingStreak.ToString() + (strictPerfectLanding ? " (Strict)" : ""),
            StatType.BestLandingStreak => bestPerfectLandingStreak.ToString(),
            StatType.DistanceTravelled => GetTotalDistance().ToString("F1"),
            StatType.GroundDistanceTravelled => GetGroundDistance().ToString("F1"),
            StatType.AirDistanceTravelled => GetAirDistance().ToString("F1"),
            StatType.Luck => Percentile(ComputeStatValue(player.stats.luck)),
            StatType.Boost => Percentile(player.character.data.GetBoost()),
            StatType.Health => player.data.currentHealth.ToString("F1"),
            StatType.MaxHealth => ComputeStatValue(player.stats.maxHealth).ToString("F1"),
            StatType.MaxEnergy => ComputeStatValue(player.stats.maxEnergy).ToString("F1"),
            StatType.PickupRange => Percentile(ComputeStatValue(player.stats.sparkPickupRange)),
            StatType.Shard => (RunHandler.RunData.shardID + 1).ToString(),
            StatType.Level => (RunHandler.RunData.currentLevel + 1).ToString() + "/" + RunHandler.RunData.MaxLevels.ToString(),
            StatType.Seed => RunHandler.RunData.currentSeed.ToString(),
            StatType.NoDeath => RunHandler.statsCollector.Deaths.Count != 0 ? "No" : "Yes",
            StatType.NoItems => RunHandler.statsCollector.Items.Count != 0 ? "No" : "Yes",
            StatType.NoHit => noHit ? "Yes" : "No",
            StatType.OnlyPerfectLanding => onlyPerfectLanding ? "Yes" : "No",
            StatType.OnlySRanks => onlySRanks ? "Yes" : "No",
            _ => "N/A"
        };
    }
}
