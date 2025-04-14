using System.Reflection;
using Landfall.Haste;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;

namespace InGameStats;

public static class InGameStatsUtils {
    /// <summary>
    /// Computes the value of a stat based on its base value and multiplier.
    /// </summary>
    /// <param name="stat">The PlayerStat object containing the base value and multiplier.</param>
    /// <example>For example, if baseValue is 10 and multiplier is 1.5, the result will be 15.</example>
    /// <returns>The computed stat value.</returns>
    public static float ComputeStatValue(PlayerStat stat) => stat.baseValue * stat.multiplier;
    
    /// <summary>
    /// Converts a percentile value to a string representation.
    /// The value is multiplied by 100 and formatted to two decimal places, followed by a percent sign.
    /// </summary>
    /// <param name="percentile">The percentile value to convert.</param>
    /// <example>0.5f will be converted to "50.00%"</example>
    /// <returns>The formatted string representation of the percentile value.</returns>
    public static string Percentile(float percentile) => (percentile * 100).ToString("F2") + "%";

    /// <summary>
    /// Gets the total distance travelled in the current run.
    /// This is done by checking the total distance in the current run stats.
    /// </summary>
    public static float GetTotalDistance() {
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
    public static float GetGroundDistance() {
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
    public static float GetAirDistance() {
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
    /// 
    /// </summary>
    public static string GetLevelStats() {
        int level = RunHandler.RunData.currentLevel;
        if (RunHandler.isEndless) {
            return $"{level} (Endless)";
        }

        int maxLevels = RunHandler.RunData.MaxLevels;
        if (level <= maxLevels) {
            return $"{level}/{maxLevels}";
        }

        return $"{level} (Boss Fight)";
    }

    /// <summary>
    /// Checks if the current run has no death.
    /// This is done by checking the number of death in the current run handler stats collector.
    /// </summary>
    /// <returns></returns>
    public static bool GetNoDeath() {
        return RunHandler.statsCollector.Deaths.Count == 0;
    }

    /// <summary>
    /// Checks if the current run has no items.
    /// This is done by checking the number of items in the current run handler stats collector.
    /// </summary>
    /// <returns></returns>
    public static bool GetNoItems() {
        return RunHandler.statsCollector.Items.Count == 0;
    }

    /// <summary>
    /// Checks if the current run has taken damage.
    /// This is done by checking the number of damage taken in the current run stats.
    /// </summary>
    /// <returns></returns>
    public static bool GetDamageTaken() {
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
    public static bool GetOnlyPerfectLanding() {
        return (
            !HasteStats.TryGetRunStat(HasteStatType.STAT_TOTAL_LANDINGS, out int totalLandingCount) ||
            (
                HasteStats.TryGetRunStat(HasteStatType.STAT_PERFECT_LANDINGS, out int perfectLandingCount) &&
                perfectLandingCount >= totalLandingCount
            )
        );
    }

    /// <summary>
    /// Checks if the current run has only S ranks.
    /// This is done by checking the number of ranks not equal to S in the current run stats.
    /// </summary>
    /// <returns></returns>
    public static bool GetOnlySRank() {
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

    internal static LocalizedString TryGetLocalizedString(string key, string fallback = "") {
        TableReference tableReference = (TableReference) "InGameStats";
        TableEntryReference tableEntryReference = (TableEntryReference) key;
        LocalizedString title = new(tableReference, tableEntryReference);
        return
            title.TableReference.ReferenceType != TableReference.Type.Empty &&
            title.TableEntryReference.ReferenceType != TableEntryReference.Type.Empty
                ? title
                : new UnlocalizedString(fallback);
    }

    /// <summary>
    /// Dictionary of stat types and their display names.
    /// </summary>
    public static readonly Dictionary<StatType, LocalizedString> statDisplayNames = new() {
        { StatType.PerfectLandingStreak, TryGetLocalizedString("PerfectLandingStreak", "Perfect Land. Streak") },
        { StatType.BestLandingStreak, TryGetLocalizedString("BestLandingStreak", "Best Land. Streak") },
        { StatType.AverageLandingScore, TryGetLocalizedString("AverageLandingScore", "Average Land. Score") },
        { StatType.DistanceTravelled, TryGetLocalizedString("DistanceTravelled", "Distance") },
        { StatType.GroundDistanceTravelled, TryGetLocalizedString("GroundDistanceTravelled", "Ground Distance") },
        { StatType.AirDistanceTravelled, TryGetLocalizedString("AirDistanceTravelled", "Air Distance") },
        { StatType.Luck, TryGetLocalizedString("Luck", "Luck") },
        { StatType.Boost, TryGetLocalizedString("Boost", "Boost") },
        { StatType.Health, TryGetLocalizedString("Health", "Health") },
        { StatType.MaxHealth, TryGetLocalizedString("MaxHealth", "Max Health") },
        { StatType.MaxEnergy, TryGetLocalizedString("MaxEnergy", "Max Energy") },
        { StatType.PickupRange, TryGetLocalizedString("PickupRange", "Pickup Range") },
        { StatType.Speed, TryGetLocalizedString("Speed", "Speed") },
        { StatType.LevelSpeed, TryGetLocalizedString("LevelSpeed", "Collapse Speed") },
        { StatType.Shard, TryGetLocalizedString("Shard", "Shard") },
        { StatType.Level, TryGetLocalizedString("Level", "Level") },
        { StatType.Seed, TryGetLocalizedString("Seed", "Seed") },
        { StatType.NoHit, TryGetLocalizedString("NoHit", "No Hit") },
        { StatType.NoDeath, TryGetLocalizedString("NoDeath", "No Death") },
        { StatType.NoItems, TryGetLocalizedString("NoItems", "No Items") },
        { StatType.OnlyPerfectLanding, TryGetLocalizedString("OnlyPerfectLanding", "Only Perfect Landing") },
        { StatType.OnlySRanks, TryGetLocalizedString("OnlySRanks", "Only S Ranks") },
    };
}