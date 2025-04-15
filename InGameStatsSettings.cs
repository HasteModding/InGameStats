using Landfall.Haste;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Localization;
using Zorro.Settings;

namespace InGameStats;

[HasteSetting]
public class OnlyInRunSetting : BoolSetting, IExposedSetting {
    public override LocalizedString OffString { get; } = new UnlocalizedString("Always Displayed");
    public override LocalizedString OnString { get; } = new UnlocalizedString("In-Run Only");
    public override void ApplyValue() {
        Debug.Log($"OnlyInRun: {Value}");
        InGameStats.Instance.onlyInRun = Value;
    }
    protected override bool GetDefaultValue() => false;
    public string GetCategory() => InGameStatsProgram.GetCategory();
    public LocalizedString GetDisplayName() => new UnlocalizedString("Only Display During Runs");

    public override void Load(ISettingsSaveLoad loader) {
        base.Load(loader);
        InGameStats.Instance.onlyInRun = Value;
    }
}

[HasteSetting]
public class XBaseOffsetSetting : FloatSetting, IExposedSetting {
    public override void ApplyValue() {
        Debug.Log($"XBaseOffset: {Value}");
        InGameStats.Instance.xBaseOffset = Value;
        InGameStats.Instance.CreateStatUI();
    }
    protected override float GetDefaultValue() => 10f;
    public LocalizedString GetDisplayName() => new UnlocalizedString("X Base Offset");
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
    public override void ApplyValue() {
        Debug.Log($"FontSize: {Value}");
        InGameStats.Instance.fontSize = Value;
        InGameStats.Instance.CreateStatUI();
    }
    protected override int GetDefaultValue() => 12;
    public LocalizedString GetDisplayName() => new UnlocalizedString("Font Size");
    public string GetCategory() => InGameStatsProgram.GetCategory();

    public override void Load(ISettingsSaveLoad loader) {
        base.Load(loader);
        InGameStats.Instance.fontSize = Value;
        InGameStats.Instance.CreateStatUI();
    }
}

[HasteSetting]
public class RightSideSetting : BoolSetting, IExposedSetting {
    public override LocalizedString OffString { get; } = new UnlocalizedString("Left Side");
    public override LocalizedString OnString { get; } = new UnlocalizedString("Right Side");
    public override void ApplyValue() {
        Debug.Log($"RightSide: {Value}");
        InGameStats.Instance.rightSide = Value;
        InGameStats.Instance.CreateStatUI();
    }
    protected override bool GetDefaultValue() => false;
    public string GetCategory() => InGameStatsProgram.GetCategory();
    public LocalizedString GetDisplayName() => new UnlocalizedString("Right Side");

    public override void Load(ISettingsSaveLoad loader) {
        base.Load(loader);
        InGameStats.Instance.rightSide = Value;
        InGameStats.Instance.CreateStatUI();
    }
}

[HasteSetting]
public class FontColorSetting : BoolSetting, IExposedSetting {
    public override LocalizedString OffString { get; } = new UnlocalizedString("Uncolorized");
    public override LocalizedString OnString { get; } = new UnlocalizedString("Colorized");
    public override void ApplyValue() {
        Debug.Log($"FontColor: {Value}");
        InGameStats.Instance.colors = Value;
        InGameStats.Instance.CreateStatUI();
    }
    protected override bool GetDefaultValue() => true;
    public string GetCategory() => InGameStatsProgram.GetCategory();
    public LocalizedString GetDisplayName() => new UnlocalizedString("Colorized Stats");

    public override void Load(ISettingsSaveLoad loader) {
        base.Load(loader);
        InGameStats.Instance.colors = Value;
        InGameStats.Instance.CreateStatUI();
    }
}

[HasteSetting]
public class CustomFontSetting : BoolSetting, IExposedSetting {
    public override LocalizedString OffString { get; } = new UnlocalizedString("Default Font");
    public override LocalizedString OnString { get; } = new UnlocalizedString("Game Font");
    public override void ApplyValue() {
        Debug.Log($"CustomFont: {Value}");
        InGameStats.Instance.font = Value;
        InGameStats.Instance.CreateStatUI();
    }
    protected override bool GetDefaultValue() => true;
    public string GetCategory() => InGameStatsProgram.GetCategory();
    public LocalizedString GetDisplayName() => new UnlocalizedString("Custom Font");

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
    public LocalizedString GetDisplayName() => new UnlocalizedString("Perfect Landing Streak Mode");
    public string GetCategory() => InGameStatsProgram.GetCategory();

    public override List<LocalizedString> GetLocalizedChoices() {
        return new List<LocalizedString> {
            new UnlocalizedString("None"),
            new UnlocalizedString("Standard"),
            new UnlocalizedString("Strict")
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
    public override LocalizedString OffString { get; } = new UnlocalizedString("Hidden");
    public override LocalizedString OnString { get; } = new UnlocalizedString("Shown");
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
    public LocalizedString GetDisplayName() => new UnlocalizedString(InGameStatsUtils.statDisplayNames[StatType]);

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
public class UpcomingLevelSetting : EnableStatSetting {
    protected override StatType StatType => StatType.UpcomingLevel;
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
