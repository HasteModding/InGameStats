using TMPro;
using UnityEngine.Localization;

namespace InGameStats;

public abstract class IG_Stat {
    public abstract bool Enabled { get; set; }

    public abstract LocalizedString DefaultText { get; }
    public virtual void OnUpdate(TextMeshProUGUI? text, ColorizedMode colorized) {}
    public virtual void OnTextCreation(TextMeshProUGUI text, ColorizedMode colorized) {}
    public virtual void OnTextDestroy(TextMeshProUGUI text) {}
    public virtual void OnStartNewRun() {}
    public virtual void OnNewLevel() {}
    public virtual void OnSpawnedInHub() {}
    public virtual void OnMainMenuPlayButton() {}
    public virtual void OnLandAction(LandingType landingType, bool saved) {}
    public virtual void OnHitLandingObjectComputed(object landing) {}
    public virtual void OnLandingScoreComputed(float landingScore) {}
}

/// <summary>
/// Enum of available alignment modes
/// </summary>
public enum AlignmentMode {
    Left,
    Center,
    Right,
}

/// <summary>
/// Enum of available colorized modes
/// </summary>
public enum ColorizedMode {
    None,
    Colorized,
}

/// <summary>
/// Enum of available font modes
/// </summary>
public enum FontMode {
    None,
    GameFont,
}

/// <summary>
/// Enum of available outline modes
/// </summary>
public enum OutlineMode {
    None,
    Outline,
}

/// <summary>
/// Enum of available display modes
/// </summary>
public enum DisplayMode {
    Always,
    InRun,
}
