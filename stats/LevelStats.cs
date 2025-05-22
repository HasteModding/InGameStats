// Collapse Speed, Obstacle Density, Upcoming Level Type
// for InGameStats mod for Haste by Landfall Games

using InGameStats;
using TMPro;
using UnityEngine.Localization;

public class CollapseSpeedStat : IG_Stat {
    private static bool _enabled = true;
    public override bool Enabled {
        get => _enabled;
        set => _enabled = value;
    }
    public static void SetEnabled(bool enabled) {
        _enabled = enabled;
    }
    public override LocalizedString DefaultText => new ("IGS_Stats", "CollapseSpeed_Default");
    internal LocalizedString prefix = new ("IGS_Stats", "CollapseSpeed_Prefix");

    public override void OnUpdate(TextMeshProUGUI? text, ColorizedMode colorized) {
        if (text == null || !Enabled) return;
        try {
            text.text = $"{prefix.GetLocalizedString()}{RunHandler.GetLevelSpeed():F1} m/s";
        } catch {
            text.text = $"{prefix.GetLocalizedString()}N/A";
        }
    }
}

public class ObstacleDensityStat : IG_Stat {
    private static bool _enabled = true;
    public override bool Enabled {
        get => _enabled;
        set => _enabled = value;
    }
    public static void SetEnabled(bool enabled) {
        _enabled = enabled;
    }
    public override LocalizedString DefaultText => new ("IGS_Stats", "ObstacleDensity_Default");
    internal LocalizedString prefix = new ("IGS_Stats", "ObstacleDensity_Prefix");

    public override void OnUpdate(TextMeshProUGUI? text, ColorizedMode colorized) {
        if (text == null || !Enabled) return;
        try {
            text.text = $"{prefix.GetLocalizedString()}{LevelGenerator.instance.config.keyPropBudget}";
        } catch {
            text.text = $"{prefix.GetLocalizedString()}N/A";
        }
    }
}

public class UpcomingLevelTypeStat : IG_Stat {
    internal static string GetUpcomingLevelStats() {
        if (RunHandler.RunData.runConfig.isEndless || RunHandler.RunData.isKeepRunningMode) {
            return "Endless";
        }
        Queue<LevelSelectionNode.Data> nodes = RunHandler.RunData.QueuedNodes;
        if (nodes.Count == 0) {
            return "No planned levels";
        }
        LevelSelectionNode.Data node = nodes.Peek();
        LevelSelectionNode.NodeType nodeType = node.type;
        return nodeType switch {
            LevelSelectionNode.NodeType.Default => "Fragment",
            LevelSelectionNode.NodeType.Shop => "Shop",
            LevelSelectionNode.NodeType.Challenge => "Challenge",
            LevelSelectionNode.NodeType.Encounter => "Encounter",
            LevelSelectionNode.NodeType.Boss => "Boss",
            LevelSelectionNode.NodeType.RestStop => "Rest",
            _ => nodeType.ToString(),
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
    public override LocalizedString DefaultText => new ("IGS_Stats", "UpcomingLevelType_Default");
    internal LocalizedString prefix = new ("IGS_Stats", "UpcomingLevelType_Prefix");

    public override void OnUpdate(TextMeshProUGUI? text, ColorizedMode colorized) {
        if (text == null || !Enabled) return;
        try {
            text.text = $"{prefix.GetLocalizedString()}{GetUpcomingLevelStats()}";
        } catch {
            text.text = $"{prefix.GetLocalizedString()}N/A";
        }
    }
}
