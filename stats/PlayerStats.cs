// Luck, Boost, Health, Max Health, Max Energy, Pick-up Range and Speed Stats, Item Unlock Progression
// for InGameStats mod for Haste by Landfall Games

using InGameStats;
using Landfall.Haste;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using Zorro.Core;

public enum ItemUnlockProgressionMode {
    None,
    Percentage,
    RawValue,
    NumberOfItems,
}

public class LuckStat : IG_Stat {
    private static bool _enabled = true;
    public override bool Enabled {
        get => _enabled;
        set => _enabled = value;
    }
    public static void SetEnabled(bool enabled) {
        _enabled = enabled;
    }
    public override LocalizedString DefaultText => new ("IGS_Stats", "Luck_Default");
    internal LocalizedString prefix = new ("IGS_Stats", "Luck_Prefix");

    public override void OnUpdate(TextMeshProUGUI? text, ColorizedMode colorized) {
        if (text == null || !Enabled) return;
        try {
            string value = Utils.Percentile(Utils.ComputeStatValue(Player.localPlayer.stats.luck));
            text.text = $"{prefix.GetLocalizedString()}{value}";
        } catch {
            text.text = $"{prefix.GetLocalizedString()}N/A";
        }
    }
}

public class BoostStat : IG_Stat {
    private static bool _enabled = true;
    public override bool Enabled {
        get => _enabled;
        set => _enabled = value;
    }
    public static void SetEnabled(bool enabled) {
        _enabled = enabled;
    }
    public override LocalizedString DefaultText => new ("IGS_Stats", "Boost_Default");
    internal LocalizedString prefix = new ("IGS_Stats", "Boost_Prefix");

    public override void OnUpdate(TextMeshProUGUI? text, ColorizedMode colorized) {
        if (text == null || !Enabled) return;
        try {
            string value = Utils.Percentile(Player.localPlayer.character.data.GetBoost());
            text.text = $"{prefix.GetLocalizedString()}{value}";
        } catch {
            text.text = $"{prefix.GetLocalizedString()}N/A";
        }
    }
}

public class HealthStat : IG_Stat {
    private static bool _enabled = true;
    public override bool Enabled {
        get => _enabled;
        set => _enabled = value;
    }
    public static void SetEnabled(bool enabled) {
        _enabled = enabled;
    }
    public override LocalizedString DefaultText => new ("IGS_Stats", "Health_Default");
    internal LocalizedString prefix = new ("IGS_Stats", "Health_Prefix");

    public override void OnUpdate(TextMeshProUGUI? text, ColorizedMode colorized) {
        if (text == null || !Enabled) return;
        try {
            string value = $"{Player.localPlayer.data.currentHealth:F1}";
            text.text = $"{prefix.GetLocalizedString()}{value}";
        } catch {
            text.text = $"{prefix.GetLocalizedString()}N/A";
        }
    }
}

public class MaxHealthStat : IG_Stat {
    private static bool _enabled = true;
    public override bool Enabled {
        get => _enabled;
        set => _enabled = value;
    }
    public static void SetEnabled(bool enabled) {
        _enabled = enabled;
    }
    public override LocalizedString DefaultText => new ("IGS_Stats", "MaxHealth_Default");
    internal LocalizedString prefix = new ("IGS_Stats", "MaxHealth_Prefix");

    public override void OnUpdate(TextMeshProUGUI? text, ColorizedMode colorized) {
        if (text == null || !Enabled) return;
        try {
            string value = $"{Utils.ComputeStatValue(Player.localPlayer.stats.maxHealth):F1}";
            text.text = $"{prefix.GetLocalizedString()}{value}";
        } catch {
            text.text = $"{prefix.GetLocalizedString()}N/A";
        }
    }
}

public class MaxEnergyStat : IG_Stat {
    private static bool _enabled = true;
    public override bool Enabled {
        get => _enabled;
        set => _enabled = value;
    }
    public static void SetEnabled(bool enabled) {
        _enabled = enabled;
    }
    public override LocalizedString DefaultText => new ("IGS_Stats", "MaxEnergy_Default");
    internal LocalizedString prefix = new ("IGS_Stats", "MaxEnergy_Prefix");

    public override void OnUpdate(TextMeshProUGUI? text, ColorizedMode colorized) {
        if (text == null || !Enabled) return;
        try {
            string value = $"{Utils.ComputeStatValue(Player.localPlayer.stats.maxEnergy):F1}";
            text.text = $"{prefix.GetLocalizedString()}{value}";
        } catch {
            text.text = $"{prefix.GetLocalizedString()}N/A";
        }
    }
}

public class PickUpRangeStat : IG_Stat {
    private static bool _enabled = true;
    public override bool Enabled {
        get => _enabled;
        set => _enabled = value;
    }
    public static void SetEnabled(bool enabled) {
        _enabled = enabled;
    }
    public override LocalizedString DefaultText => new ("IGS_Stats", "PickUpRange_Default");
    internal LocalizedString prefix = new ("IGS_Stats", "PickUpRange_Prefix");

    public override void OnUpdate(TextMeshProUGUI? text, ColorizedMode colorized) {
        if (text == null || !Enabled) return;
        try {
            string value = Utils.Percentile(Utils.ComputeStatValue(Player.localPlayer.stats.sparkPickupRange));
            text.text = $"{prefix.GetLocalizedString()}{value}";
        } catch {
            text.text = $"{prefix.GetLocalizedString()}N/A";
        }
    }
}

public class SpeedStat : IG_Stat {
    private static bool _enabled = true;
    public override bool Enabled {
        get => _enabled;
        set => _enabled = value;
    }
    public static void SetEnabled(bool enabled) {
        _enabled = enabled;
    }
    public override LocalizedString DefaultText => new ("IGS_Stats", "Speed_Default");
    internal LocalizedString prefix = new ("IGS_Stats", "Speed_Prefix");

    public override void OnUpdate(TextMeshProUGUI? text, ColorizedMode colorized) {
        if (text == null || !Enabled) return;
        try {
            // player.character.refs.rig.velocity.magnitude.ToString("F1") + " m/s",
            string value = $"{Player.localPlayer.character.refs.rig.velocity.magnitude:F1} m/s";
            text.text = $"{prefix.GetLocalizedString()}{value}";
        } catch {
            text.text = $"{prefix.GetLocalizedString()}N/A";
        }
    }
}

public class ItemUnlockProgressionStat : IG_Stat {
    internal static (int, int) GetShardStats() {
        MetaProgression metaProgression = SingletonAsset<MetaProgression>.Instance;
        if (metaProgression == null) {
            return (0, 0);
        }

        int onLoseOrEndless = metaProgression.baseCurrencyGet;
        int onWin = metaProgression.baseCurrencyGet + metaProgression.currencyWinBonus;
        if (RunHandler.RunData.shardID >= PlayerProgress.CurrentUnlockedShard && !RunHandler.RunData.isEndless)
            onWin += metaProgression.firstWinBonus;

        onLoseOrEndless += Mathf.RoundToInt(
            metaProgression.currencyPerDifficulty *
            Mathf.LerpUnclamped(
                RunHandler.config.startDifficulty,
                RunHandler.config.endDifficulty,
                RunHandler.GetRunProgressUnclamped()
            ) *
            Mathf.Clamp01(RunHandler.GetRunProgressUnclamped() * 5f)
        ) + metaProgression.currencyPerLevel * RunHandler.RunData.currentLevel;

        float onWinRunProgress = RunHandler.RunData.currentLevel < 0 || RunHandler.RunData.MaxLevels <= 1 ? 0.0f : (RunHandler.RunData.MaxLevels + 2) / Mathf.Clamp(RunHandler.RunData.MaxLevels - 1f, 1f, 10000f);
        onWin += Mathf.RoundToInt(
            metaProgression.currencyPerDifficulty *
            Mathf.LerpUnclamped(
                RunHandler.config.startDifficulty,
                RunHandler.config.endDifficulty,
                onWinRunProgress
            ) *
            Mathf.Clamp01(onWinRunProgress * 5f)
        ) + metaProgression.currencyPerLevel * (RunHandler.RunData.MaxLevels + 1);

        if (RunHandler.RunData.isEndless) {
            onLoseOrEndless = Mathf.RoundToInt(onLoseOrEndless * metaProgression.endessMultiplier);
            onWin = Mathf.RoundToInt(onWin * metaProgression.endessMultiplier);
        }
        onWin = Mathf.RoundToInt(
            Mathf.RoundToInt(onWin * metaProgression.winMultiplier) *
            metaProgression.totalMultiplier
        );
        onLoseOrEndless = Mathf.RoundToInt(onLoseOrEndless * metaProgression.totalMultiplier);
        return (onLoseOrEndless, onWin);
    }

    internal static string GetItemUnlockProgression(ItemUnlockProgressionMode itemUnlockProgressionMode) {
        int resources = Mathf.RoundToInt(FactSystem.GetFact(MetaProgression.MetaProgressionResourceForItemUnlock));
        int itemEveryCurrency = SingletonAsset<MetaProgression>.Instance.itemEveryCurrency;
        if (RunHandler.InRun && RunHandler.lastRunState == RunHandler.LastRunState.None) {
            (int onLose, int onWin) = GetShardStats();
            string currentState = itemUnlockProgressionMode switch {
                ItemUnlockProgressionMode.Percentage => $"{(float) (resources + onLose) / itemEveryCurrency * 100:F2}%",
                ItemUnlockProgressionMode.RawValue => $"{resources + onLose}/{itemEveryCurrency}",
                ItemUnlockProgressionMode.NumberOfItems => $"{(resources + onLose) / itemEveryCurrency} items",
                _ => $"N/A",
            };
            if (RunHandler.RunData.isEndless)
                return currentState;

            return currentState + itemUnlockProgressionMode switch {
                ItemUnlockProgressionMode.Percentage => $" (+{(float) (onWin - onLose) / itemEveryCurrency * 100:F2}%)",
                ItemUnlockProgressionMode.RawValue => $" (+{onWin - onLose})",
                ItemUnlockProgressionMode.NumberOfItems => $" (+{(onWin - onLose + ((resources + onLose) % itemEveryCurrency)) / itemEveryCurrency})",
                _ => $"N/A",
            };
        }
        return itemUnlockProgressionMode switch {
            ItemUnlockProgressionMode.Percentage => $"{(float) resources / itemEveryCurrency * 100:F2}%",
            ItemUnlockProgressionMode.RawValue => $"{resources}/{itemEveryCurrency}",
            ItemUnlockProgressionMode.NumberOfItems => $"{resources / itemEveryCurrency} items",
            _ => $"N/A",
        };
    }

    private static bool _enabled = true;
    public override bool Enabled {
        get => _enabled;
        set => _enabled = value;
    }
    public static void SetEnabled(bool enabled) {
        _enabled = enabled;
    }
    public static ItemUnlockProgressionMode itemUnlockProgressionMode = ItemUnlockProgressionMode.Percentage;

    public override LocalizedString DefaultText => new ("IGS_Stats", "IUP_Default");
    internal LocalizedString prefix = new ("IGS_Stats", "IUP_Prefix");

    public override void OnUpdate(TextMeshProUGUI? text, ColorizedMode colorized) {
        if (text == null || !Enabled) return;
        try {
            string value = GetItemUnlockProgression(itemUnlockProgressionMode);
            text.text = $"{prefix.GetLocalizedString()}{value}";
        } catch {
            text.text = $"{prefix.GetLocalizedString()}N/A";
        }
    }
}
