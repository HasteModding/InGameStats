// Collapse Speed, Obstacle Density, Upcoming Level Type
// for InGameStats mod for Haste by Landfall Games

using InGameStats;
using Landfall.Haste;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using Zorro.Core;

public class CollapseSpeedStat : IG_Stat {
    private static bool _enabled = true;
    public override bool Enabled {
        get => _enabled;
        set => _enabled = value;
    }
    public static void SetEnabled(bool enabled) {
        _enabled = enabled;
    }
    public override LocalizedString DefaultText => new UnlocalizedString("Collapse Speed: Loaded");
    internal LocalizedString prefix = new UnlocalizedString("Collapse Speed: ");

    public override void OnUpdate(TextMeshProUGUI? text, ColorizedMode colorized) {
        if (text == null || !Enabled) return;
        try {
            text.text = $"{prefix}{RunHandler.GetLevelSpeed():F1} m/s";
        } catch {
            text.text = $"{prefix}N/A";
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
    public override LocalizedString DefaultText => new UnlocalizedString("Obstacle Density: Loaded");
    internal LocalizedString prefix = new UnlocalizedString("Obstacle Density: ");

    public override void OnUpdate(TextMeshProUGUI? text, ColorizedMode colorized) {
        if (text == null || !Enabled) return;
        try {
            text.text = $"{prefix}{LevelGenerator.instance.config.keyPropBudget}";
        } catch {
            text.text = $"{prefix}N/A";
        }
    }
}

public class UpcomingLevelTypeStat : IG_Stat {
    internal static string GetUpcomingLevelStats() {
        if (RunHandler.RunData.isEndless || RunHandler.RunData.isKeepRunningMode) {
            return "Endless";
        }
        Queue<LevelSelectionNode.Data> nodes = RunHandler.RunData.QueuedNodes;
        if (nodes.Count == 0) {
            return "No planned levels";
        }
        LevelSelectionNode.Data node = nodes.Peek();
        LevelSelectionNode.NodeType nodeType = node.Type;
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
    public override LocalizedString DefaultText => new UnlocalizedString("Upcoming Level Type: Loaded");
    internal LocalizedString prefix = new UnlocalizedString("Upcoming Level Type: ");

    public override void OnUpdate(TextMeshProUGUI? text, ColorizedMode colorized) {
        if (text == null || !Enabled) return;
        try {
            text.text = $"{prefix}{GetUpcomingLevelStats()}";
        } catch {
            text.text = $"{prefix}N/A";
        }
    }
}
