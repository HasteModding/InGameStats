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
/// Enum of available stats
/// </summary>
public enum PerfectLandingStreakType {
    None,
    Standard,
    Strict,
}
