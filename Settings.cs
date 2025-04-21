using Landfall.Haste;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Localization;
using Zorro.Settings;
using Zorro.Settings.DebugUI;

namespace InGameStats;

[HasteSetting]
public class DisplaySetting : EnumSetting<DisplayMode>, IExposedSetting {
    public override void ApplyValue() {
        Debug.Log($"DisplayMode set to {Value}");
        InGameStats.Instance.displayMode = Value;
        InGameStats.Instance.RecreateUI();
    }

    public string GetCategory() => InGameStats.Category.GetLocalizedString();

    public LocalizedString GetDisplayName() => new ("InGameStats", "UI_DisplayMode");

    public override List<LocalizedString> GetLocalizedChoices() => new () {
        new ("InGameStats", "DisplayMode_Off"),
        new ("InGameStats", "DisplayMode_Always"),
        new ("InGameStats", "DisplayMode_InRun")
    };

    protected override DisplayMode GetDefaultValue() => DisplayMode.Always;

    public override void Load(ISettingsSaveLoad loader) {
        base.Load(loader);
        ApplyValue();
    }
}

[HasteSetting]
public class OffsetXSetting : FloatSetting, IExposedSetting {
    public override void ApplyValue() {
        Debug.Log($"OffsetX set to {Value}");
        InGameStats.Instance.xBaseOffset = Value;
        InGameStats.Instance.RecreateUI();
    }

    public string GetCategory() => InGameStats.Category.GetLocalizedString();

    public LocalizedString GetDisplayName() => new ("InGameStats", "UI_OffsetX");

    protected override float GetDefaultValue() => 10f;

    protected override float2 GetMinMaxValue() => new (0f, Screen.width);

    public override void Load(ISettingsSaveLoad loader) {
        base.Load(loader);
        ApplyValue();
    }
}

[HasteSetting]
public class OffsetYSetting : FloatSetting, IExposedSetting {
    public override void ApplyValue() {
        Debug.Log($"OffsetY set to {Value}");
        InGameStats.Instance.yBaseOffset = Value;
        InGameStats.Instance.RecreateUI();
    }

    public string GetCategory() => InGameStats.Category.GetLocalizedString();

    public LocalizedString GetDisplayName() => new ("InGameStats", "UI_OffsetY");

    protected override float GetDefaultValue() => 10f;

    protected override float2 GetMinMaxValue() => new (0f, Screen.height);

    public override void Load(ISettingsSaveLoad loader) {
        base.Load(loader);
        ApplyValue();
    }
}

[HasteSetting]
public class FontSizeSetting : IntSetting, IExposedSetting {
    public override void ApplyValue() {
        Debug.Log($"FontSize set to {Value}");
        InGameStats.Instance.fontSize = Value;
        InGameStats.Instance.RecreateUI();
    }

    public string GetCategory() => InGameStats.Category.GetLocalizedString();

    public LocalizedString GetDisplayName() => new ("InGameStats", "UI_FontSize");

    protected override int GetDefaultValue() => 20;

    public override void Load(ISettingsSaveLoad loader) {
        base.Load(loader);
        ApplyValue();
    }
}

[HasteSetting]
public class AlignmentSetting : EnumSetting<AlignmentMode>, IExposedSetting {
    public override void ApplyValue() {
        Debug.Log($"AlignmentMode set to {Value}");
        InGameStats.Instance.alignmentMode = Value;
        InGameStats.Instance.RecreateUI();
    }

    public string GetCategory() => InGameStats.Category.GetLocalizedString();

    public LocalizedString GetDisplayName() => new ("InGameStats", "UI_AlignmentMode");

    public override List<LocalizedString> GetLocalizedChoices() => new () {
        new ("InGameStats", "AlignmentMode_Left"),
        new ("InGameStats", "AlignmentMode_Center"),
        new ("InGameStats", "AlignmentMode_Right")
    };

    protected override AlignmentMode GetDefaultValue() => AlignmentMode.Left;

    public override void Load(ISettingsSaveLoad loader) {
        base.Load(loader);
        ApplyValue();
    }
}

[HasteSetting]
public class ColorizingSetting : EnumSetting<ColorizedMode>, IExposedSetting {
    public override void ApplyValue() {
        Debug.Log($"Colorizing set to {Value}");
        InGameStats.Instance.colorizedMode = Value;
        InGameStats.Instance.RecreateUI();
    }

    public string GetCategory() => InGameStats.Category.GetLocalizedString();

    public LocalizedString GetDisplayName() => new ("InGameStats", "UI_Colorizing");

    public override List<LocalizedString> GetLocalizedChoices() => new () {
        new ("InGameStats", "Colorizing_Off"),
        new ("InGameStats", "Colorizing_On")
    };

    protected override ColorizedMode GetDefaultValue() => ColorizedMode.Colorized;

    public override void Load(ISettingsSaveLoad loader) {
        base.Load(loader);
        ApplyValue();
    }
}

[HasteSetting]
public class FontSetting : EnumSetting<FontMode>, IExposedSetting {
    public override void ApplyValue() {
        Debug.Log($"Font set to {Value}");
        InGameStats.Instance.fontMode = Value;
        InGameStats.Instance.RecreateUI();
    }

    public string GetCategory() => InGameStats.Category.GetLocalizedString();

    public LocalizedString GetDisplayName() => new ("InGameStats", "UI_Font");

    public override List<LocalizedString> GetLocalizedChoices() => new () {
        new ("InGameStats", "Font_Unchanged"),
        new ("InGameStats", "Font_InGameFont")
    };

    protected override FontMode GetDefaultValue() => FontMode.GameFont;

    public override void Load(ISettingsSaveLoad loader) {
        base.Load(loader);
        ApplyValue();
    }
}

[HasteSetting]
public class OutlineSetting : EnumSetting<OutlineMode>, IExposedSetting {
    public override void ApplyValue() {
        Debug.Log($"Outline set to {Value}");
        InGameStats.Instance.outlineMode = Value;
        InGameStats.Instance.RecreateUI();
    }

    public string GetCategory() => InGameStats.Category.GetLocalizedString();

    public LocalizedString GetDisplayName() => new ("InGameStats", "UI_Outline");

    public override List<LocalizedString> GetLocalizedChoices() => new () {
        new ("InGameStats", "Outline_Off"),
        new ("InGameStats", "Outline_On")
    };

    protected override OutlineMode GetDefaultValue() => OutlineMode.Outline;

    public override void Load(ISettingsSaveLoad loader) {
        base.Load(loader);
        ApplyValue();
    }
}

// LandingStats

[HasteSetting]
public class LandingDetectionSetting : EnumSetting<PL_DetectionMode>, IExposedSetting {
    public override void ApplyValue() {
        Debug.Log($"LandingDetection set to {Value}");
        PerfectLandingStreak.detectionMode = Value;
        BestLandingStreak.detectionMode = Value;
    }

    public string GetCategory() => InGameStats.Category.GetLocalizedString();

    public LocalizedString GetDisplayName() => new ("IGS_Stats", "Stat_LandingDetection");

    public override List<LocalizedString> GetLocalizedChoices() => new () {
        new ("IGS_Stats", "LandingDetection_Strict"),
        new ("IGS_Stats", "LandingDetection_Standard")
    };

    protected override PL_DetectionMode GetDefaultValue() => PL_DetectionMode.Standard;

    public override void Load(ISettingsSaveLoad loader) {
        base.Load(loader);
        ApplyValue();
    }
}

[HasteSetting]
public class PerfectLandingStreakSetting : IGSSettingTemplate {
    public override void ApplyValue() {
        Debug.Log($"PerfectLandingStreak set to {Value}");
        PerfectLandingStreak.SetEnabled(Value);
    }

    public override LocalizedString GetDisplayName() => new ("IGS_Stats", "Stat_PerfectLandingStreak");

    protected override bool GetDefaultValue() => true;
}

[HasteSetting]
public class BestLandingStreakSetting : IGSSettingTemplate {
    public override void ApplyValue() {
        Debug.Log($"BestLandingStreak set to {Value}");
        BestLandingStreak.SetEnabled(Value);
    }

    public override LocalizedString GetDisplayName() => new ("IGS_Stats", "Stat_BestLandingStreak");

    protected override bool GetDefaultValue() => true;
}

[HasteSetting]
public class AverageLandingScoreSetting : IGSSettingTemplate {
    public override void ApplyValue() {
        Debug.Log($"AverageLandingScore set to {Value}");
        AverageLandingScore.SetEnabled(Value);
    }

    public override LocalizedString GetDisplayName() => new ("IGS_Stats", "Stat_AverageLandingScore");

    protected override bool GetDefaultValue() => true;
}

// DistanceTravelledStats

[HasteSetting]
public class TotalDistanceTravelledSetting : IGSSettingTemplate {
    public override void ApplyValue() {
        Debug.Log($"DistanceTravelled set to {Value}");
        TotalDistanceTravelled.SetEnabled(Value);
    }

    public override LocalizedString GetDisplayName() => new ("IGS_Stats", "Stat_TotalDistanceTravelled");

    protected override bool GetDefaultValue() => true;
}

[HasteSetting]
public class GroundDistanceTravelledSetting : IGSSettingTemplate {
    public override void ApplyValue() {
        Debug.Log($"GroundDistanceTravelled set to {Value}");
        GroundDistanceTravelled.SetEnabled(Value);
    }

    public override LocalizedString GetDisplayName() => new ("IGS_Stats", "Stat_GroundDistanceTravelled");

    protected override bool GetDefaultValue() => true;
}

[HasteSetting]
public class AirDistanceTravelledSetting : IGSSettingTemplate {
    public override void ApplyValue() {
        Debug.Log($"AirDistanceTravelled set to {Value}");
        AirDistanceTravelled.SetEnabled(Value);
    }

    public override LocalizedString GetDisplayName() => new ("IGS_Stats", "Stat_AirDistanceTravelled");

    protected override bool GetDefaultValue() => true;
}

// PlayerStats

[HasteSetting]
public class LuckSetting : IGSSettingTemplate {
    public override void ApplyValue() {
        Debug.Log($"Luck set to {Value}");
        LuckStat.SetEnabled(Value);
    }

    public override LocalizedString GetDisplayName() => new ("IGS_Stats", "Stat_Luck");

    protected override bool GetDefaultValue() => true;
}

[HasteSetting]
public class BoostSetting : IGSSettingTemplate {
    public override void ApplyValue() {
        Debug.Log($"Boost set to {Value}");
        BoostStat.SetEnabled(Value);
    }

    public override LocalizedString GetDisplayName() => new ("IGS_Stats", "Stat_Boost");

    protected override bool GetDefaultValue() => true;
}

[HasteSetting]
public class HealthSetting : IGSSettingTemplate {
    public override void ApplyValue() {
        Debug.Log($"Health set to {Value}");
        HealthStat.SetEnabled(Value);
    }

    public override LocalizedString GetDisplayName() => new ("IGS_Stats", "Stat_Health");

    protected override bool GetDefaultValue() => true;
}

[HasteSetting]
public class MaxHealthSetting : IGSSettingTemplate {
    public override void ApplyValue() {
        Debug.Log($"MaxHealth set to {Value}");
        MaxHealthStat.SetEnabled(Value);
    }

    public override LocalizedString GetDisplayName() => new ("IGS_Stats", "Stat_MaxHealth");

    protected override bool GetDefaultValue() => true;
}

[HasteSetting]
public class MaxEnergySetting : IGSSettingTemplate {
    public override void ApplyValue() {
        Debug.Log($"MaxEnergy set to {Value}");
        MaxEnergyStat.SetEnabled(Value);
    }

    public override LocalizedString GetDisplayName() => new ("IGS_Stats", "Stat_MaxEnergy");

    protected override bool GetDefaultValue() => true;
}

[HasteSetting]
public class PickUpRangeSetting : IGSSettingTemplate {
    public override void ApplyValue() {
        Debug.Log($"PickupRange set to {Value}");
        PickUpRangeStat.SetEnabled(Value);
    }

    public override LocalizedString GetDisplayName() => new ("IGS_Stats", "Stat_PickUpRange");

    protected override bool GetDefaultValue() => true;
}

[HasteSetting]
public class SpeedSetting : IGSSettingTemplate {
    public override void ApplyValue() {
        Debug.Log($"Speed set to {Value}");
        SpeedStat.SetEnabled(Value);
    }

    public override LocalizedString GetDisplayName() => new ("IGS_Stats", "Stat_Speed");

    protected override bool GetDefaultValue() => true;
}

[HasteSetting]
public class ItemUnlockProgressionSetting : EnumSetting<ItemUnlockProgressionMode>, IExposedSetting {
    public override void ApplyValue() {
        Debug.Log($"ItemUnlockProgression set to {Value}");
        ItemUnlockProgressionStat.itemUnlockProgressionMode = Value;
        ItemUnlockProgressionStat.SetEnabled(Value != ItemUnlockProgressionMode.None);
    }

    public string GetCategory() => InGameStats.Category.GetLocalizedString();

    public LocalizedString GetDisplayName() => new ("IGS_Stats", "Stat_ItemUnlockProgression");

    public override List<LocalizedString> GetLocalizedChoices() => new () {
        new ("IGS_Stats", "ItemUnlockProgression_Off"),
        new ("IGS_Stats", "ItemUnlockProgression_Percentage"),
        new ("IGS_Stats", "ItemUnlockProgression_RawValue"),
        new ("IGS_Stats", "ItemUnlockProgression_NumberOfItems")
    };

    protected override ItemUnlockProgressionMode GetDefaultValue() => ItemUnlockProgressionMode.Percentage;
}

// LevelStats

[HasteSetting]
public class CollapseSpeedSetting : IGSSettingTemplate {
    public override void ApplyValue() {
        Debug.Log($"CollapseSpeed set to {Value}");
        CollapseSpeedStat.SetEnabled(Value);
    }

    public override LocalizedString GetDisplayName() => new ("IGS_Stats", "Stat_CollapseSpeed");

    protected override bool GetDefaultValue() => true;
}

[HasteSetting]
public class ObstacleDensitySetting : IGSSettingTemplate {
    public override void ApplyValue() {
        Debug.Log($"ObstacleDensity set to {Value}");
        ObstacleDensityStat.SetEnabled(Value);
    }

    public override LocalizedString GetDisplayName() => new ("IGS_Stats", "Stat_ObstacleDensity");

    protected override bool GetDefaultValue() => true;
}

[HasteSetting]
public class UpcomingLevelTypeSetting : IGSSettingTemplate {
    public override void ApplyValue() {
        Debug.Log($"UpcomingLevelType set to {Value}");
        UpcomingLevelTypeStat.SetEnabled(Value);
    }

    public override LocalizedString GetDisplayName() => new ("IGS_Stats", "Stat_UpcomingLevelType");

    protected override bool GetDefaultValue() => true;
}

// ShardStats

[HasteSetting]
public class CurrentShardSetting : IGSSettingTemplate {
    public override void ApplyValue() {
        Debug.Log($"CurrentShard set to {Value}");
        CurrentShardStat.SetEnabled(Value);
    }

    public override LocalizedString GetDisplayName() => new ("IGS_Stats", "Stat_CurrentShard");

    protected override bool GetDefaultValue() => true;
}

[HasteSetting]
public class CurrentLevelSetting : IGSSettingTemplate {
    public override void ApplyValue() {
        Debug.Log($"CurrentLevel set to {Value}");
        CurrentLevelStat.SetEnabled(Value);
    }

    public override LocalizedString GetDisplayName() => new ("IGS_Stats", "Stat_CurrentLevel");

    protected override bool GetDefaultValue() => true;
}

[HasteSetting]
public class CurrentSeedSetting : IGSSettingTemplate {
    public override void ApplyValue() {
        Debug.Log($"CurrentSeed set to {Value}");
        CurrentSeedStat.SetEnabled(Value);
    }

    public override LocalizedString GetDisplayName() => new ("IGS_Stats", "Stat_CurrentSeed");

    protected override bool GetDefaultValue() => true;
}

// AchievementTrackers

[HasteSetting]
public class NoHitSetting : IGSSettingTemplate {
    public override void ApplyValue() {
        Debug.Log($"NoHit set to {Value}");
        NoHitTracker.SetEnabled(Value);
    }

    public override LocalizedString GetDisplayName() => new ("IGS_Stats", "Stat_NoHit");

    protected override bool GetDefaultValue() => true;
}

[HasteSetting]
public class NoDeathSetting : IGSSettingTemplate {
    public override void ApplyValue() {
        Debug.Log($"NoDeath set to {Value}");
        NoDeathTracker.SetEnabled(Value);
    }

    public override LocalizedString GetDisplayName() => new ("IGS_Stats", "Stat_NoDeath");

    protected override bool GetDefaultValue() => true;
}

[HasteSetting]
public class NoItemSetting : IGSSettingTemplate {
    public override void ApplyValue() {
        Debug.Log($"NoItem set to {Value}");
        NoItemTracker.SetEnabled(Value);
    }

    public override LocalizedString GetDisplayName() => new ("IGS_Stats", "Stat_NoItem");

    protected override bool GetDefaultValue() => true;
}

[HasteSetting]
public class OnlyPerfectLandingsSetting : IGSSettingTemplate {
    public override void ApplyValue() {
        Debug.Log($"OnlyPerfectLandings set to {Value}");
        OnlyPerfectLandingsTracker.SetEnabled(Value);
    }

    public override LocalizedString GetDisplayName() => new ("IGS_Stats", "Stat_OnlyPerfectLandings");

    protected override bool GetDefaultValue() => true;
}

[HasteSetting]
public class OnlySRanksSetting : IGSSettingTemplate {
    public override void ApplyValue() {
        Debug.Log($"OnlySRanks set to {Value}");
        OnlySRanksTracker.SetEnabled(Value);
    }

    public override LocalizedString GetDisplayName() => new ("IGS_Stats", "Stat_OnlySRanks");

    protected override bool GetDefaultValue() => true;
}
