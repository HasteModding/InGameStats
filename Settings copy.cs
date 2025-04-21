// using Landfall.Haste;
// using Unity.Mathematics;
// using UnityEngine;
// using UnityEngine.Localization;
// using Zorro.Settings;

// namespace InGameStats;

// [HasteSetting]
// public class OnlyInRunSetting : BoolSetting, IExposedSetting {
//     public override LocalizedString OffString { get; } = new LocalizedString("Always Displayed");
//     public override LocalizedString OnString { get; } = new LocalizedString("In-Run Only");
//     public override void ApplyValue() {
//         Debug.Log($"OnlyInRun: {Value}");
//         InGameStats.Instance.onlyInRun = Value;
//     }
//     protected override bool GetDefaultValue() => false;
//     public string GetCategory() => Program.GetCategory();
//     public LocalizedString GetDisplayName() => new LocalizedString("Only Display During Runs");

//     public override void Load(ISettingsSaveLoad loader) {
//         base.Load(loader);
//         InGameStats.Instance.onlyInRun = Value;
//     }
// }

// [HasteSetting]
// public class XBaseOffsetSetting : FloatSetting, IExposedSetting {
//     public override void ApplyValue() {
//         Debug.Log($"XBaseOffset: {Value}");
//         InGameStats.Instance.xBaseOffset = Value;
//         InGameStats.Instance.CreateStatUI();
//     }
//     protected override float GetDefaultValue() => 10f;
//     public LocalizedString GetDisplayName() => new LocalizedString("X Base Offset");
//     public string GetCategory() => Program.GetCategory();

//     protected override float2 GetMinMaxValue() => new (0f, Screen.width);

//     public override void Load(ISettingsSaveLoad loader) {
//         base.Load(loader);
//         InGameStats.Instance.xBaseOffset = Value;
//     }
// }

// [HasteSetting]
// public class YBaseOffsetSetting : FloatSetting, IExposedSetting {
//     public override void ApplyValue() {
//         Debug.Log($"YBaseOffset: {Value}");
//         InGameStats.Instance.yBaseOffset = Value;
//         InGameStats.Instance.CreateStatUI();
//     }
//     protected override float GetDefaultValue() => 0f;
//     public LocalizedString GetDisplayName() => new LocalizedString("Y Base Offset");
//     public string GetCategory() => Program.GetCategory();

//     protected override float2 GetMinMaxValue() => new (0f, Screen.height);
    
//     public override void Load(ISettingsSaveLoad loader) {
//         base.Load(loader);
//         InGameStats.Instance.yBaseOffset = Value;
//         InGameStats.Instance.CreateStatUI();
//     }
// }

// [HasteSetting]
// public class FontSizeSetting : IntSetting, IExposedSetting {
//     public override void ApplyValue() {
//         Debug.Log($"FontSize: {Value}");
//         InGameStats.Instance.fontSize = Value;
//         InGameStats.Instance.CreateStatUI();
//     }
//     protected override int GetDefaultValue() => 12;
//     public LocalizedString GetDisplayName() => new LocalizedString("Font Size");
//     public string GetCategory() => Program.GetCategory();

//     public override void Load(ISettingsSaveLoad loader) {
//         base.Load(loader);
//         InGameStats.Instance.fontSize = Value;
//         InGameStats.Instance.CreateStatUI();
//     }
// }

// [HasteSetting]
// public class AlignmentSetting : EnumSetting<AlignmentMode>, IExposedSetting {
//     public override void ApplyValue() {
//         Debug.Log($"Alignment: {Value}");
//         InGameStats.Instance.alignment = Value;
//         InGameStats.Instance.CreateStatUI();
//     }
//     protected override AlignmentMode GetDefaultValue() => AlignmentMode.Left;
//     public LocalizedString GetDisplayName() => new LocalizedString("Alignment Mode");
//     public string GetCategory() => Program.GetCategory();

//     public override List<LocalizedString> GetLocalizedChoices() {
//         return new List<LocalizedString> {
//             new LocalizedString("Left Side"),
//             new LocalizedString("Right Side"),
//             new LocalizedString("Centered")
//         };
//     }

//     public override void Load(ISettingsSaveLoad loader) {
//         base.Load(loader);
//         InGameStats.Instance.alignment = Value;
//         InGameStats.Instance.CreateStatUI();
//     }
// }

// [HasteSetting]
// public class FontColorSetting : BoolSetting, IExposedSetting {
//     public override LocalizedString OffString { get; } = new LocalizedString("Uncolorized");
//     public override LocalizedString OnString { get; } = new LocalizedString("Colorized");
//     public override void ApplyValue() {
//         Debug.Log($"FontColor: {Value}");
//         InGameStats.Instance.colors = Value;
//         InGameStats.Instance.CreateStatUI();
//     }
//     protected override bool GetDefaultValue() => true;
//     public string GetCategory() => Program.GetCategory();
//     public LocalizedString GetDisplayName() => new LocalizedString("Colorized Stats");

//     public override void Load(ISettingsSaveLoad loader) {
//         base.Load(loader);
//         InGameStats.Instance.colors = Value;
//         InGameStats.Instance.CreateStatUI();
//     }
// }

// [HasteSetting]
// public class TextOutlineSetting : BoolSetting, IExposedSetting {
//     public override LocalizedString OffString { get; } = new LocalizedString("No Outline");
//     public override LocalizedString OnString { get; } = new LocalizedString("Outline");
//     public override void ApplyValue() {
//         Debug.Log($"TextOutline: {Value}");
//         InGameStats.Instance.outline = Value;
//         InGameStats.Instance.CreateStatUI();
//     }
//     protected override bool GetDefaultValue() => true;
//     public string GetCategory() => Program.GetCategory();
//     public LocalizedString GetDisplayName() => new LocalizedString("Outline Text");

//     public override void Load(ISettingsSaveLoad loader) {
//         base.Load(loader);
//         InGameStats.Instance.outline = Value;
//         InGameStats.Instance.CreateStatUI();
//     }
// }

// [HasteSetting]
// public class CustomFontSetting : BoolSetting, IExposedSetting {
//     public override LocalizedString OffString { get; } = new LocalizedString("Default Font");
//     public override LocalizedString OnString { get; } = new LocalizedString("Game Font");
//     public override void ApplyValue() {
//         Debug.Log($"CustomFont: {Value}");
//         InGameStats.Instance.font = Value;
//         InGameStats.Instance.CreateStatUI();
//     }
//     protected override bool GetDefaultValue() => true;
//     public string GetCategory() => Program.GetCategory();
//     public LocalizedString GetDisplayName() => new LocalizedString("Custom Font");

//     public override void Load(ISettingsSaveLoad loader) {
//         base.Load(loader);
//         InGameStats.Instance.font = Value;
//         InGameStats.Instance.CreateStatUI();
//     }
// }

// [HasteSetting]
// public class PerfectLandingStreakSetting : EnumSetting<PerfectLandingStreakType>, IExposedSetting {
//     public override void ApplyValue() {
//         Debug.Log($"PerfectLandingStreak: {Value}");
//         InGameStats.Instance.enabledStats.Remove(StatType.PerfectLandingStreak);
//         switch (Value) {
//             case PerfectLandingStreakType.None:
//                 InGameStats.Instance.strictPerfectLanding = false;
//                 break;
//             case PerfectLandingStreakType.Standard:
//                 InGameStats.Instance.enabledStats.Add(StatType.PerfectLandingStreak);
//                 InGameStats.Instance.strictPerfectLanding = false;
//                 break;
//             case PerfectLandingStreakType.Strict:
//                 InGameStats.Instance.enabledStats.Add(StatType.PerfectLandingStreak);
//                 InGameStats.Instance.strictPerfectLanding = true;
//                 break;
//         }
//         InGameStats.Instance.CreateStatUI();
//     }
//     protected override PerfectLandingStreakType GetDefaultValue() => PerfectLandingStreakType.Standard;
//     public LocalizedString GetDisplayName() => new LocalizedString("Perfect Landing Streak Mode");
//     public string GetCategory() => Program.GetCategory();

//     public override List<LocalizedString> GetLocalizedChoices() {
//         return new List<LocalizedString> {
//             new LocalizedString("None"),
//             new LocalizedString("Standard"),
//             new LocalizedString("Strict")
//         };
//     }

//     public override void Load(ISettingsSaveLoad loader) {
//         base.Load(loader);
//         InGameStats.Instance.enabledStats.Remove(StatType.PerfectLandingStreak);
//         if (Value != PerfectLandingStreakType.None)
//             InGameStats.Instance.enabledStats.Add(StatType.PerfectLandingStreak);
//         InGameStats.Instance.strictPerfectLanding = Value == PerfectLandingStreakType.Strict;
//         InGameStats.Instance.CreateStatUI();
//     }
// }

// public abstract class EnableStatSetting : BoolSetting, IExposedSetting {
//     public override LocalizedString OffString { get; } = new LocalizedString("Hidden");
//     public override LocalizedString OnString { get; } = new LocalizedString("Shown");
//     public override void ApplyValue() {
//         Debug.Log($"{Utils.statDisplayNames[StatType]}: {Value}");
//         InGameStats.Instance.enabledStats.Remove(StatType);
//         if (Value)
//             InGameStats.Instance.enabledStats.Add(StatType);
//         InGameStats.Instance.CreateStatUI();
//     }
//     protected override bool GetDefaultValue() => true;
//     public string GetCategory() => Program.GetCategory();

//     protected abstract StatType StatType { get; }
//     public LocalizedString GetDisplayName() => new LocalizedString(Utils.statDisplayNames[StatType]);

//     public override void Load(ISettingsSaveLoad loader) {
//         base.Load(loader);
//         InGameStats.Instance.enabledStats.Remove(StatType);
//         if (Value)
//             InGameStats.Instance.enabledStats.Add(StatType);
//         InGameStats.Instance.CreateStatUI();
//     }
// }

// [HasteSetting]
// public class BestLandingStreakSetting : EnableStatSetting {
//     protected override StatType StatType => StatType.BestLandingStreak;
// }

// [HasteSetting]
// public class AverageLandingScoreSetting : EnableStatSetting {
//     protected override StatType StatType => StatType.AverageLandingScore;
// }

// [HasteSetting]
// public class DistanceTravelledSetting : EnableStatSetting {
//     protected override StatType StatType => StatType.DistanceTravelled;
// }

// [HasteSetting]
// public class GroundDistanceTravelledSetting : EnableStatSetting {
//     protected override StatType StatType => StatType.GroundDistanceTravelled;
// }

// [HasteSetting]
// public class AirDistanceTravelledSetting : EnableStatSetting {
//     protected override StatType StatType => StatType.AirDistanceTravelled;
// }

// [HasteSetting]
// public class LuckSetting : EnableStatSetting {
//     protected override StatType StatType => StatType.Luck;
// }

// [HasteSetting]
// public class BoostSetting : EnableStatSetting {
//     protected override StatType StatType => StatType.Boost;
// }

// [HasteSetting]
// public class HealthSetting : EnableStatSetting {
//     protected override StatType StatType => StatType.Health;
// }

// [HasteSetting]
// public class MaxHealthSetting : EnableStatSetting {
//     protected override StatType StatType => StatType.MaxHealth;
// }

// [HasteSetting]
// public class MaxEnergySetting : EnableStatSetting {
//     protected override StatType StatType => StatType.MaxEnergy;
// }

// [HasteSetting]
// public class PickupRangeSetting : EnableStatSetting {
//     protected override StatType StatType => StatType.PickupRange;
// }

// [HasteSetting]
// public class SpeedSetting : EnableStatSetting {
//     protected override StatType StatType => StatType.Speed;
// }

// [HasteSetting]
// public class LevelSpeedSetting : EnableStatSetting {
//     protected override StatType StatType => StatType.LevelSpeed;
// }

// [HasteSetting]
// public class PropBudgetSetting : EnableStatSetting {
//     protected override StatType StatType => StatType.PropBudget;
// }

// [HasteSetting]
// public class UpcomingLevelSetting : EnableStatSetting {
//     protected override StatType StatType => StatType.UpcomingLevel;
// }

// [HasteSetting]
// public class ItemUnlockProgressionSetting : EnumSetting<ItemUnlockProgressionMode>, IExposedSetting {
//     public override void ApplyValue() {
//         Debug.Log($"PerfectLandingStreak: {Value}");
//         InGameStats.Instance.enabledStats.Remove(StatType.ItemUnlockProgression);
//         if (Value != ItemUnlockProgressionMode.None)
//             InGameStats.Instance.enabledStats.Add(StatType.ItemUnlockProgression);
//         InGameStats.Instance.itemUnlockProgressionMode = Value;
//         InGameStats.Instance.CreateStatUI();
//     }
//     protected override ItemUnlockProgressionMode GetDefaultValue() => ItemUnlockProgressionMode.Percentage;
//     public LocalizedString GetDisplayName() => new LocalizedString("Item Unlock Progression Mode");
//     public string GetCategory() => Program.GetCategory();

//     public override List<LocalizedString> GetLocalizedChoices() {
//         return new List<LocalizedString> {
//             new LocalizedString("None"),
//             new LocalizedString("Percentage"),
//             new LocalizedString("Raw Value"),
//             new LocalizedString("Number of Items")
//         };
//     }

//     public override void Load(ISettingsSaveLoad loader) {
//         base.Load(loader);
//         InGameStats.Instance.enabledStats.Remove(StatType.PerfectLandingStreak);
//         if (Value != ItemUnlockProgressionMode.None)
//             InGameStats.Instance.enabledStats.Add(StatType.ItemUnlockProgression);
//         InGameStats.Instance.itemUnlockProgressionMode = Value;
//         InGameStats.Instance.CreateStatUI();
//     }
// }

// [HasteSetting]
// public class ShardSetting : EnableStatSetting {
//     protected override StatType StatType => StatType.Shard;
// }

// [HasteSetting]
// public class LevelSetting : EnableStatSetting {
//     protected override StatType StatType => StatType.Level;
// }

// [HasteSetting]
// public class SeedSetting : EnableStatSetting {
//     protected override StatType StatType => StatType.Seed;
// }

// [HasteSetting]
// public class NoHitSetting : EnableStatSetting {
//     protected override StatType StatType => StatType.NoHit;
// }

// [HasteSetting]
// public class NoDeathSetting : EnableStatSetting {
//     protected override StatType StatType => StatType.NoDeath;
// }

// [HasteSetting]
// public class NoItemsSetting : EnableStatSetting {
//     protected override StatType StatType => StatType.NoItems;
// }

// [HasteSetting]
// public class OnlyPerfectLandingSetting : EnableStatSetting {
//     protected override StatType StatType => StatType.OnlyPerfectLanding;
// }

// [HasteSetting]
// public class OnlySRanksSetting : EnableStatSetting {
//     protected override StatType StatType => StatType.OnlySRanks;
// }
