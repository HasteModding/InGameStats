using Landfall.Haste;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Localization;
using Zorro.Settings;

namespace InGameStats;

[HasteSetting]
public class XBaseOffsetSetting : FloatSetting, IExposedSetting {
    public override void ApplyValue() {
        Debug.Log($"XBaseOffset: {Value}");
        InGameStats.Instance.xBaseOffset = Value;
        InGameStats.Instance.CreateStatUI();
    }
    protected override float GetDefaultValue() => 10f;
    public LocalizedString GetDisplayName() => InGameStatsUtils.TryGetLocalizedString("XOffset", "X Base Offset");
    public string GetCategory() => InGameStatsProgram.GetCategory();

    protected override float2 GetMinMaxValue() => new (0f, Screen.width);

    public override void Load(ISettingsSaveLoad loader) {
        base.Load(loader);
        InGameStats.Instance.xBaseOffset = Value;
    }
}

[HasteSetting]
public class YBaseOffsetSetting : FloatSetting, IExposedSetting {
    public override void ApplyValue() {
        Debug.Log($"YBaseOffset: {Value}");
        InGameStats.Instance.yBaseOffset = Value;
        InGameStats.Instance.CreateStatUI();
    }
    protected override float GetDefaultValue() => 0f;
    public LocalizedString GetDisplayName() => InGameStatsUtils.TryGetLocalizedString("YOffset", "Y Base Offset");
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
    public override void ApplyValue() {
        Debug.Log($"FontSize: {Value}");
        InGameStats.Instance.fontSize = Value;
        InGameStats.Instance.CreateStatUI();
    }
    protected override int GetDefaultValue() => 12;
    public LocalizedString GetDisplayName() => InGameStatsUtils.TryGetLocalizedString("FontSize", "Font Size");
    public string GetCategory() => InGameStatsProgram.GetCategory();

    public override void Load(ISettingsSaveLoad loader) {
        base.Load(loader);
        InGameStats.Instance.fontSize = Value;
        InGameStats.Instance.CreateStatUI();
    }
}

[HasteSetting]
public class RightSideSetting : BoolSetting, IExposedSetting {
    public override LocalizedString OffString { get; } = InGameStatsUtils.TryGetLocalizedString("LeftSide", "Left Side");
    public override LocalizedString OnString { get; } = InGameStatsUtils.TryGetLocalizedString("RightSide", "Right Side");
    public override void ApplyValue() {
        Debug.Log($"RightSide: {Value}");
        InGameStats.Instance.rightSide = Value;
        InGameStats.Instance.CreateStatUI();
    }
    protected override bool GetDefaultValue() => false;
    public string GetCategory() => InGameStatsProgram.GetCategory();
    public LocalizedString GetDisplayName() => InGameStatsUtils.TryGetLocalizedString("DisplaySide", "Display Side");

    public override void Load(ISettingsSaveLoad loader) {
        base.Load(loader);
        InGameStats.Instance.rightSide = Value;
        InGameStats.Instance.CreateStatUI();
    }
}

[HasteSetting]
public class FontColorSetting : BoolSetting, IExposedSetting {
    public override LocalizedString OffString { get; } = InGameStatsUtils.TryGetLocalizedString("Uncolorized", "Uncolorized");
    public override LocalizedString OnString { get; } = InGameStatsUtils.TryGetLocalizedString("Colorized", "Colorized");
    public override void ApplyValue() {
        Debug.Log($"FontColor: {Value}");
        InGameStats.Instance.colors = Value;
        InGameStats.Instance.CreateStatUI();
    }
    protected override bool GetDefaultValue() => true;
    public string GetCategory() => InGameStatsProgram.GetCategory();
    public LocalizedString GetDisplayName() => InGameStatsUtils.TryGetLocalizedString("StatsColor", "Stats Color");

    public override void Load(ISettingsSaveLoad loader) {
        base.Load(loader);
        InGameStats.Instance.colors = Value;
        InGameStats.Instance.CreateStatUI();
    }
}

[HasteSetting]
public class CustomFontSetting : BoolSetting, IExposedSetting {
    public override LocalizedString OffString { get; } = InGameStatsUtils.TryGetLocalizedString("DefaultFont", "Default Font");
    public override LocalizedString OnString { get; } = InGameStatsUtils.TryGetLocalizedString("GameFont", "Game Font");
    public override void ApplyValue() {
        Debug.Log($"CustomFont: {Value}");
        InGameStats.Instance.font = Value;
        InGameStats.Instance.CreateStatUI();
    }
    protected override bool GetDefaultValue() => true;
    public string GetCategory() => InGameStatsProgram.GetCategory();
    public LocalizedString GetDisplayName() => InGameStatsUtils.TryGetLocalizedString("CustomFont", "Custom Font");

    public override void Load(ISettingsSaveLoad loader) {
        base.Load(loader);
        InGameStats.Instance.font = Value;
        InGameStats.Instance.CreateStatUI();
    }
}

[HasteSetting]
public class PerfectLandingStreakSetting : EnumSetting<PerfectLandingStreakType>, IExposedSetting {
    public override void ApplyValue() {
        Debug.Log($"PerfectLandingStreak: {Value}");
        InGameStats.Instance.enabledStats.Remove(StatType.PerfectLandingStreak);
        switch (Value) {
            case PerfectLandingStreakType.None:
                InGameStats.Instance.strictPerfectLanding = false;
                break;
            case PerfectLandingStreakType.Standard:
                InGameStats.Instance.enabledStats.Add(StatType.PerfectLandingStreak);
                InGameStats.Instance.strictPerfectLanding = false;
                break;
            case PerfectLandingStreakType.Strict:
                InGameStats.Instance.enabledStats.Add(StatType.PerfectLandingStreak);
                InGameStats.Instance.strictPerfectLanding = true;
                break;
        }
        InGameStats.Instance.CreateStatUI();
    }
    protected override PerfectLandingStreakType GetDefaultValue() => PerfectLandingStreakType.Standard;
    public LocalizedString GetDisplayName() => InGameStatsUtils.TryGetLocalizedString("PerfectLandingStreakMode", "Perfect Landing Streak Mode");
    public string GetCategory() => InGameStatsProgram.GetCategory();

    public override List<LocalizedString> GetLocalizedChoices() {
        return new List<LocalizedString> {
            InGameStatsUtils.TryGetLocalizedString("Hidden", "Hidden"),
            InGameStatsUtils.TryGetLocalizedString("Standard", "Standard"),
            InGameStatsUtils.TryGetLocalizedString("Strict", "Strict")
        };
    }

    public override void Load(ISettingsSaveLoad loader) {
        base.Load(loader);
        InGameStats.Instance.enabledStats.Remove(StatType.PerfectLandingStreak);
        switch (Value) {
            case PerfectLandingStreakType.None:
                InGameStats.Instance.strictPerfectLanding = false;
                break;
            case PerfectLandingStreakType.Standard:
                InGameStats.Instance.enabledStats.Add(StatType.PerfectLandingStreak);
                InGameStats.Instance.strictPerfectLanding = false;
                break;
            case PerfectLandingStreakType.Strict:
                InGameStats.Instance.enabledStats.Add(StatType.PerfectLandingStreak);
                InGameStats.Instance.strictPerfectLanding = true;
                break;
        }
        InGameStats.Instance.CreateStatUI();
    }
}

public abstract class EnableStatSetting : BoolSetting, IExposedSetting {
    public override LocalizedString OffString { get; } = InGameStatsUtils.TryGetLocalizedString("Hidden", "Hidden");
    public override LocalizedString OnString { get; } = InGameStatsUtils.TryGetLocalizedString("Shown", "Shown");
    public override void ApplyValue() {
        Debug.Log($"{InGameStatsUtils.statDisplayNames[StatType]}: {Value}");
        InGameStats.Instance.enabledStats.Remove(StatType);
        if (Value)
            InGameStats.Instance.enabledStats.Add(StatType);
        InGameStats.Instance.CreateStatUI();
    }
    protected override bool GetDefaultValue() => true;
    public string GetCategory() => InGameStatsProgram.GetCategory();

    protected abstract StatType StatType { get; }
    public LocalizedString GetDisplayName() => InGameStatsUtils.statDisplayNames[StatType];

    public override void Load(ISettingsSaveLoad loader) {
        base.Load(loader);
        InGameStats.Instance.enabledStats.Remove(StatType);
        if (Value)
            InGameStats.Instance.enabledStats.Add(StatType);
        InGameStats.Instance.CreateStatUI();
    }
}

[HasteSetting]
public class BestLandingStreakSetting : EnableStatSetting {
    protected override StatType StatType => StatType.BestLandingStreak;
}

[HasteSetting]
public class AverageLandingScoreSetting : EnableStatSetting {
    protected override StatType StatType => StatType.AverageLandingScore;
}

[HasteSetting]
public class DistanceTravelledSetting : EnableStatSetting {
    protected override StatType StatType => StatType.DistanceTravelled;
}

[HasteSetting]
public class GroundDistanceTravelledSetting : EnableStatSetting {
    protected override StatType StatType => StatType.GroundDistanceTravelled;
}

[HasteSetting]
public class AirDistanceTravelledSetting : EnableStatSetting {
    protected override StatType StatType => StatType.AirDistanceTravelled;
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
public class SpeedSetting : EnableStatSetting {
    protected override StatType StatType => StatType.Speed;
}

[HasteSetting]
public class LevelSpeedSetting : EnableStatSetting {
    protected override StatType StatType => StatType.LevelSpeed;
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
public class NoHitSetting : EnableStatSetting {
    protected override StatType StatType => StatType.NoHit;
}

[HasteSetting]
public class NoDeathSetting : EnableStatSetting {
    protected override StatType StatType => StatType.NoDeath;
}

[HasteSetting]
public class NoItemsSetting : EnableStatSetting {
    protected override StatType StatType => StatType.NoItems;
}

[HasteSetting]
public class OnlyPerfectLandingSetting : EnableStatSetting {
    protected override StatType StatType => StatType.OnlyPerfectLanding;
}

[HasteSetting]
public class OnlySRanksSetting : EnableStatSetting {
    protected override StatType StatType => StatType.OnlySRanks;
}
