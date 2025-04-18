// No Hit, No Death, No Item, Only Perfect Landings and Only S Ranks trackers
// for InGameStats mod for Haste by Landfall Games

using InGameStats;
using Landfall.Haste;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using Zorro.Core;

public class NoHitTracker : IG_Stat {
    private static bool _enabled = true;
    public override bool Enabled {
        get => _enabled;
        set => _enabled = value;
    }
    public static void SetEnabled(bool enabled) {
        _enabled = enabled;
    }
    public override LocalizedString DefaultText => new UnlocalizedString("No Hit Tracker: Loaded");
    internal LocalizedString prefix = new UnlocalizedString("No Hit Tracker: ");

    public override void OnUpdate(TextMeshProUGUI? text, ColorizedMode colorized) {
        if (text == null || !Enabled) return;
        bool noHit = !HasteStats.TryGetRunStat(HasteStatType.STAT_DAMAGE_TAKEN, out int damageTakenCount) || damageTakenCount == 0;
        if (noHit) {
            text.text = $"{prefix}No Hit";
            if (colorized == ColorizedMode.Colorized) text.color = Color.green;
        } else {
            text.text = $"{prefix}Hit";
            if (colorized == ColorizedMode.Colorized) text.color = Color.red;
        }
    }
}

public class NoDeathTracker : IG_Stat {
    private static bool _enabled = true;
    public override bool Enabled {
        get => _enabled;
        set => _enabled = value;
    }
    public static void SetEnabled(bool enabled) {
        _enabled = enabled;
    }
    public override LocalizedString DefaultText => new UnlocalizedString("No Death Tracker: Loaded");
    internal LocalizedString prefix = new UnlocalizedString("No Death Tracker: ");

    public override void OnUpdate(TextMeshProUGUI? text, ColorizedMode colorized) {
        if (text == null || !Enabled) return;
        bool noDeath = RunHandler.statsCollector.Deaths.Count == 0;
        if (noDeath) {
            text.text = $"{prefix}No Death";
            if (colorized == ColorizedMode.Colorized) text.color = Color.green;
        } else {
            text.text = $"{prefix}Death";
            if (colorized == ColorizedMode.Colorized) text.color = Color.red;
        }
    }
}

public class NoItemTracker : IG_Stat {
    private static bool _enabled = true;
    public override bool Enabled {
        get => _enabled;
        set => _enabled = value;
    }
    public static void SetEnabled(bool enabled) {
        _enabled = enabled;
    }
    public override LocalizedString DefaultText => new UnlocalizedString("No Item Tracker: Loaded");
    internal LocalizedString prefix = new UnlocalizedString("No Item Tracker: ");

    public override void OnUpdate(TextMeshProUGUI? text, ColorizedMode colorized) {
        if (text == null || !Enabled) return;
        bool noItem = RunHandler.statsCollector.Items.Count == 0;
        if (noItem) {
            text.text = $"{prefix}No Item";
            if (colorized == ColorizedMode.Colorized) text.color = Color.green;
        } else {
            text.text = $"{prefix}Item";
            if (colorized == ColorizedMode.Colorized) text.color = Color.red;
        }
    }
}

public class OnlyPerfectLandingsTracker : IG_Stat {
    private static bool _enabled = true;
    public override bool Enabled {
        get => _enabled;
        set => _enabled = value;
    }
    public static void SetEnabled(bool enabled) {
        _enabled = enabled;
    }
    public override LocalizedString DefaultText => new UnlocalizedString("Only Perfect Landings Tracker: Loaded");
    internal LocalizedString prefix = new UnlocalizedString("Only Perfect Landings Tracker: ");

    public override void OnUpdate(TextMeshProUGUI? text, ColorizedMode colorized) {
        if (text == null || !Enabled) return;
        bool onlyPerfectLandings =
            HasteStats.TryGetRunStat(HasteStatType.STAT_TOTAL_LANDINGS, out int totalLandingCount) ||
            (
                HasteStats.TryGetRunStat(HasteStatType.STAT_PERFECT_LANDINGS, out int perfectLandingCount) &&
                perfectLandingCount >= totalLandingCount
            );
        if (onlyPerfectLandings) {
            text.text = $"{prefix}Only Perfect Landings";
            if (colorized == ColorizedMode.Colorized) text.color = Color.green;
        } else {
            text.text = $"{prefix}Not Only Perfect Landings";
            if (colorized == ColorizedMode.Colorized) text.color = Color.red;
        }
    }
}

public class OnlySRanksTracker : IG_Stat {
    private static readonly HasteStatType[] unapprovedRanks = {
        HasteStatType.STAT_RANK_A_LEVELS,
        HasteStatType.STAT_RANK_B_LEVELS,
        HasteStatType.STAT_RANK_C_LEVELS,
        HasteStatType.STAT_RANK_D_LEVELS,
        HasteStatType.STAT_RANK_E_LEVELS,
    };
    private static bool _enabled = true;
    public override bool Enabled {
        get => _enabled;
        set => _enabled = value;
    }
    public static void SetEnabled(bool enabled) {
        _enabled = enabled;
    }
    public override LocalizedString DefaultText => new UnlocalizedString("Only S Ranks Tracker: Loaded");
    internal LocalizedString prefix = new UnlocalizedString("Only S Ranks Tracker: ");

    public override void OnUpdate(TextMeshProUGUI? text, ColorizedMode colorized) {
        if (text == null || !Enabled) return;
        bool onlySRanks = unapprovedRanks.All(rank => !HasteStats.TryGetRunStat(rank, out int rankCount) || rankCount == 0);
        if (onlySRanks) {
            text.text = $"{prefix}Only S Ranks";
            if (colorized == ColorizedMode.Colorized) text.color = Color.green;
        } else {
            text.text = $"{prefix}Not Only S Ranks";
            if (colorized == ColorizedMode.Colorized) text.color = Color.red;
        }
    }
}
