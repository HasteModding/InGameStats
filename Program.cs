using Landfall.Modding;
using UnityEngine;
using Object = UnityEngine.Object;

namespace InGameStats;

/// <summary>
/// InGameStats plugin class. This is the entry point for the mod.
/// </summary>
[LandfallPlugin]
public class InGameStatsProgram {
    /// <summary>
    /// InGameStats plugin name. This is used for the settings tab.
    /// </summary>
    /// <returns>The name of the plugin.</returns>
    public static string GetCategory() => "InGameStats";

    static InGameStatsProgram() {
        GameObject instance = new (nameof(InGameStats));
        Object.DontDestroyOnLoad(instance);
        instance.AddComponent<InGameStats>();
    }
}
