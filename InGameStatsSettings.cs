using Landfall.Haste;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Localization;
using Zorro.Settings;

namespace InGameStats;

[HasteSetting]
public class XBaseOffsetSetting : FloatSetting, IExposedSetting {
    public override void ApplyValue() => Debug.Log($"XBaseOffset: {Value}");
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

[HasteSetting]
public class ApplySettings : ButtonSetting, IExposedSetting {
    public override void OnClicked() {
        Debug.Log("ApplySettings clicked");
        InGameStats.Instance.yBaseOffset = GameHandler.Instance.SettingsHandler.GetSetting<YBaseOffsetSetting>().Value;
        InGameStats.Instance.xBaseOffset = GameHandler.Instance.SettingsHandler.GetSetting<XBaseOffsetSetting>().Value;
        InGameStats.Instance.fontSize = GameHandler.Instance.SettingsHandler.GetSetting<FontSizeSetting>().Value;
        InGameStats.Instance.colors = GameHandler.Instance.SettingsHandler.GetSetting<FontColorSetting>().Value;
        InGameStats.Instance.font = GameHandler.Instance.SettingsHandler.GetSetting<CustomFontSetting>().Value;

        InGameStats.Instance.enabledStats.Clear();
        switch (GameHandler.Instance.SettingsHandler.GetSetting<PerfectLandingStreakSetting>().Value) {
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

        if (GameHandler.Instance.SettingsHandler.GetSetting<BestLandingStreakSetting>().Value)
            InGameStats.Instance.enabledStats.Add(StatType.BestLandingStreak);
        if (GameHandler.Instance.SettingsHandler.GetSetting<AverageLandingScoreSetting>().Value)
            InGameStats.Instance.enabledStats.Add(StatType.AverageLandingScore);
        if (GameHandler.Instance.SettingsHandler.GetSetting<DistanceTravelledSetting>().Value)
            InGameStats.Instance.enabledStats.Add(StatType.DistanceTravelled);
        if (GameHandler.Instance.SettingsHandler.GetSetting<GroundDistanceTravelledSetting>().Value)
            InGameStats.Instance.enabledStats.Add(StatType.GroundDistanceTravelled);
        if (GameHandler.Instance.SettingsHandler.GetSetting<AirDistanceTravelledSetting>().Value)
            InGameStats.Instance.enabledStats.Add(StatType.AirDistanceTravelled);
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
        if (GameHandler.Instance.SettingsHandler.GetSetting<NoHitSetting>().Value)
            InGameStats.Instance.enabledStats.Add(StatType.NoHit);
        if (GameHandler.Instance.SettingsHandler.GetSetting<NoDeathSetting>().Value)
            InGameStats.Instance.enabledStats.Add(StatType.NoDeath);
        if (GameHandler.Instance.SettingsHandler.GetSetting<NoItemsSetting>().Value)
            InGameStats.Instance.enabledStats.Add(StatType.NoItems);
        if (GameHandler.Instance.SettingsHandler.GetSetting<OnlyPerfectLandingSetting>().Value)
            InGameStats.Instance.enabledStats.Add(StatType.OnlyPerfectLanding);
        if (GameHandler.Instance.SettingsHandler.GetSetting<OnlySRanksSetting>().Value)
            InGameStats.Instance.enabledStats.Add(StatType.OnlySRanks);
        InGameStats.Instance.CreateStatUI();
    }

    public string GetCategory() => InGameStatsProgram.GetCategory();
    public LocalizedString GetDisplayName() => new UnlocalizedString("Apply Settings");
    public override string GetButtonText() => "Apply Settings";
}

[HasteSetting]
public class FontColorSetting : BoolSetting, IExposedSetting {
    public override LocalizedString OffString { get; } = new UnlocalizedString("Uncolorized");
    public override LocalizedString OnString { get; } = new UnlocalizedString("Colorized");
    public override void ApplyValue() => Debug.Log($"FontColor: {Value}");
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
    public override void ApplyValue() => Debug.Log($"CustomFont: {Value}");
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
    public override void ApplyValue() => Debug.Log($"PerfectLandingStreak: {Value}");
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

[HasteSetting]
public class BottomApplySettings : ApplySettings {}
