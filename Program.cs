using Landfall.Modding;
using UnityEngine;
using Object = UnityEngine.Object;

namespace InGameStats;

/// <summary>
/// InGameStats plugin class. This is the entry point for the mod.
/// </summary>
[LandfallPlugin]
public class Program {
    static Program() {
        GameObject instance = new (nameof(InGameStats));
        Object.DontDestroyOnLoad(instance);
        instance.AddComponent<InGameStats>();

        InGameStats.RegisterStat<PerfectLandingStreak>();
        InGameStats.RegisterStat<BestLandingStreak>();
        InGameStats.RegisterStat<AverageLandingScore>();
        InGameStats.RegisterStat<TotalDistanceTravelled>();
        InGameStats.RegisterStat<GroundDistanceTravelled>();
        InGameStats.RegisterStat<AirDistanceTravelled>();
        InGameStats.RegisterStat<LuckStat>();
        InGameStats.RegisterStat<BoostStat>();
        InGameStats.RegisterStat<HealthStat>();
        InGameStats.RegisterStat<MaxHealthStat>();
        InGameStats.RegisterStat<MaxEnergyStat>();
        InGameStats.RegisterStat<PickUpRangeStat>();
        InGameStats.RegisterStat<SpeedStat>();
        InGameStats.RegisterStat<ItemUnlockProgressionStat>();
        InGameStats.RegisterStat<CollapseSpeedStat>();
        InGameStats.RegisterStat<ObstacleDensityStat>();
        InGameStats.RegisterStat<UpcomingLevelTypeStat>();
        InGameStats.RegisterStat<CurrentShardStat>();
        InGameStats.RegisterStat<CurrentLevelStat>();
        InGameStats.RegisterStat<CurrentSeedStat>();
        InGameStats.RegisterStat<NoHitTracker>();
        InGameStats.RegisterStat<NoDeathTracker>();
        InGameStats.RegisterStat<NoItemTracker>();
        InGameStats.RegisterStat<OnlyPerfectLandingsTracker>();
        InGameStats.RegisterStat<OnlySRanksTracker>();
    }
}
