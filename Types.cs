using TMPro;

namespace InGameStats;

/// <summary>
/// Enum of available stats
/// </summary>
public enum StatType {
    PerfectLandingStreak,
    BestLandingStreak,
    AverageLandingScore,
    DistanceTravelled,
    GroundDistanceTravelled,
    AirDistanceTravelled,
    Luck,
    Boost,
    Health,
    MaxHealth,
    MaxEnergy,
    PickupRange,
    Speed,
    LevelSpeed,
    PropBudget,
    UpcomingLevel,
    ItemUnlockProgression,
    Shard,
    Level,
    Seed,
    NoHit,
    NoDeath,
    NoItems,
    OnlyPerfectLanding,
    OnlySRanks,
}

/// <summary>
/// Enum of available perfect landing detection mode
/// </summary>
public enum PerfectLandingStreakType {
    None,
    Standard,
    Strict,
}

/// <summary>
/// Enum of available alignment modes
/// </summary>
public enum AlignmentMode {
    Left,
    Right,
    Center,
}

/// <summary>
/// Enum of available item unlock Progression modes
/// </summary>
public enum ItemUnlockProgressionMode {
    None,
    Percentage,
    RawValue,
    NumberOfItems,
}

public abstract class Stat {
    public abstract string DefaultName { get; }
    public abstract bool Enabled();
    public abstract string UpdateContent(TextMeshProUGUI text, bool colorized, bool isRightAligned);
    public virtual void OnStartNewRun() {} // Called when a new run starts
    public virtual void OnNewLevel() {} // Called when a new level starts
    public virtual void OnSpawnedInHub() {} // Called when the player spawns in the hub
    public virtual void OnMainMenuPlayButton() {} // Called when the player presses the play button in the main menu
}
