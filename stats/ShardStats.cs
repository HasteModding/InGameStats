// Current Shard, Current Level and Current Seed stats
// for InGameStats mod for Haste by Landfall Games

using InGameStats;
using Landfall.Haste;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using Zorro.Core;

public class CurrentShardStat : IG_Stat {
    internal static string GetCurrentShardNumber() {
        if (RunHandler.RunData.isEndless || RunHandler.RunData.isKeepRunningMode) {
            return "Endless";
        }
        return $"{RunHandler.RunData.shardID + 1}";
    }

    private static bool _enabled = true;
    public override bool Enabled {
        get => _enabled;
        set => _enabled = value;
    }
    public static void SetEnabled(bool enabled) {
        _enabled = enabled;
    }
    public override LocalizedString DefaultText => new UnlocalizedString("Current Shard: Loaded");
    internal LocalizedString prefix = new UnlocalizedString("Current Shard: ");

    public override void OnUpdate(TextMeshProUGUI? text, ColorizedMode colorized) {
        if (text == null || !Enabled) return;
        try {
            text.text = $"{prefix}{GetCurrentShardNumber()}";
        } catch {
            text.text = $"{prefix}N/A";
        }
    }
}

public class CurrentLevelStat : IG_Stat {
    internal static string GetCurrentLevelNumber() {
        int level = RunHandler.RunData.currentLevel;
        if (RunHandler.RunData.isEndless || RunHandler.RunData.isKeepRunningMode) {
            return $"{level} (Endless)";
        }

        int maxLevels = RunHandler.RunData.MaxLevels;
        if (level <= maxLevels) {
            return $"{level}/{maxLevels}";
        }

        return $"{level} (Boss Fight)";
    }

    private static bool _enabled = true;
    public override bool Enabled {
        get => _enabled;
        set => _enabled = value;
    }
    public static void SetEnabled(bool enabled) {
        _enabled = enabled;
    }
    public override LocalizedString DefaultText => new UnlocalizedString("Current Level: Loaded");
    internal LocalizedString prefix = new UnlocalizedString("Current Level: ");

    public override void OnUpdate(TextMeshProUGUI? text, ColorizedMode colorized) {
        if (text == null || !Enabled) return;
        try {
            text.text = $"{prefix}{GetCurrentLevelNumber()}";
        } catch {
            text.text = $"{prefix}N/A";
        }
    }
}

public class CurrentSeedStat : IG_Stat {
    private static bool _enabled = true;
    public override bool Enabled {
        get => _enabled;
        set => _enabled = value;
    }
    public static void SetEnabled(bool enabled) {
        _enabled = enabled;
    }
    public override LocalizedString DefaultText => new UnlocalizedString("Current Seed: Loaded");
    internal LocalizedString prefix = new UnlocalizedString("Current Seed: ");

    public override void OnUpdate(TextMeshProUGUI? text, ColorizedMode colorized) {
        if (text == null || !Enabled) return;
        try {
            text.text = $"{prefix}{RunHandler.RunData.currentSeed}";
        } catch {
            text.text = $"{prefix}N/A";
        }
    }
}
