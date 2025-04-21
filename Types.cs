using TMPro;
using UnityEngine.Localization;
using Zorro.Settings;

namespace InGameStats;

public abstract class IG_Stat {
    public abstract bool Enabled { get; set; }

    public abstract LocalizedString DefaultText { get; }
    /// <summary>
    /// Called when the text is updated. This is where you should set the text and color of the TextMeshProUGUI object.
    /// </summary>
    /// <param name="text">The TextMeshProUGUI object to update.</param>
    /// <param name="colorized">The colorized mode to use.</param>
    /// <remarks>Note: This method is called every frame, so make sure to optimize your code to avoid performance issues.</remarks>
    public virtual void OnUpdate(TextMeshProUGUI? text, ColorizedMode colorized) {}
    /// <summary>
    /// Called when the text is created. This is where you should initialize the text and color of the TextMeshProUGUI object.
    /// </summary>
    /// <param name="text">The TextMeshProUGUI object to initialize.</param>
    /// <param name="colorized">The colorized mode to use.</param>
    /// <remarks>Note: If not used, the text will initialize white, with DefaultText as content.</remarks>
    public virtual void OnTextCreation(TextMeshProUGUI text, ColorizedMode colorized) {}
    /// <summary>
    /// Called when the text is destroyed. This is where you should clean up any resources used by the text.
    /// </summary>
    /// <param name="text">The TextMeshProUGUI object about to be destroyed.</param>
    /// <remarks>Note: If not used, the text will be destroyed without any cleanup. DO NOT DESTROY THE TEXT YOURSELF.</remarks>
    public virtual void OnTextDestroy(TextMeshProUGUI text) {}
    /// <summary>
    /// Called when the player enters a shard (new run).
    /// </summary>
    public virtual void OnStartNewRun() {}
    /// <summary>
    /// Called when the player enters a level (new fragment).
    /// </summary>
    public virtual void OnNewLevel() {}
    /// <summary>
    /// Called when the player enters the hub.
    /// </summary>
    public virtual void OnSpawnedInHub() {}
    /// <summary>
    /// Called when the player leaves the main menu (clicks play).
    /// </summary>
    public virtual void OnMainMenuPlayButton() {}
    /// <summary>
    /// Called when the player lands and triggers a landing event.
    /// </summary>
    /// <param name="landingType">The type of landing (e.g. bad, ok, good, perfect).</param>
    /// <param name="saved">Whether the landing was saved or not.</param>
    public virtual void OnLandAction(LandingType landingType, bool saved) {}
    /// <summary>
    /// Called when the player hits the ground and triggers a landing event.
    /// </summary>
    /// <param name="landing">The raw landing object.</param>
    /// <remarks>Note: A hit is not always a landing! You should check time the player last touched the ground.</remarks>
    /// <remarks>Note: `landing` is with type object because PlayerMovement.Landing isn't exposed. Use System.Reflection to get infos.</remarks>
    public virtual void OnHitLandingObjectComputed(object landing) {}
    /// <summary>
    /// Called when the player lands and triggers a landing event.
    /// </summary>
    /// <param name="landingScore">The computed landing score.</param>
    public virtual void OnLandingScoreComputed(float landingScore) {}
}

public abstract class IGSSettingTemplate : BoolSetting, IExposedSetting {
    public override LocalizedString OffString => new ("IGS_Stats", "Setting_Off");

    public override LocalizedString OnString => new ("IGS_Stats", "Setting_On");

    public virtual string GetCategory() => InGameStats.Category.GetLocalizedString();

    public abstract LocalizedString GetDisplayName();

    public override void Load(ISettingsSaveLoad loader) {
        base.Load(loader);
        ApplyValue();
    }
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
    None,
    Always,
    InRun,
}
