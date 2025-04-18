// Total, Ground, and Air Distance Travelled stats
// for InGameStats mod for Haste by Landfall Games

using System.Reflection;
using InGameStats;
using Landfall.Haste;
using TMPro;
using UnityEngine.Localization;

public class TotalDistanceTravelled : IG_Stat {
    private static bool _enabled = true;
    public override bool Enabled {
        get => _enabled;
        set => _enabled = value;
    }
    public static void SetEnabled(bool enabled) {
        _enabled = enabled;
    }
    public override LocalizedString DefaultText => new UnlocalizedString("Total Distance Travelled: Loaded");
    internal LocalizedString prefix = new UnlocalizedString("Total Distance Travelled: ");

    public override void OnUpdate(TextMeshProUGUI? text, ColorizedMode colorized) {
        if (text == null || !Enabled) return;
        const BindingFlags bindingFlags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
        FieldInfo? groundDistanceField = typeof(HasteStats).GetField("m_RUN_DistanceTravelled_Ground", bindingFlags);
        FieldInfo? airDistanceField = typeof(HasteStats).GetField("m_RUN_DistanceTravelled_Air", bindingFlags);
        if (groundDistanceField == null || airDistanceField == null)
            throw new MissingMemberException("Fields 'm_RUN_DistanceTravelled_Ground' or 'm_RUN_DistanceTravelled_Air' not found on HasteStats.");
        object? groundValue = groundDistanceField.GetValue(null);
        object? airValue = airDistanceField.GetValue(null);
        if (groundValue is not float groundDistanceValue || airValue is not float airDistanceValue)
            throw new InvalidCastException("Fields 'm_RUN_DistanceTravelled_Ground' or 'm_RUN_DistanceTravelled_Air' are not of type float.");
        text.text = $"{prefix}{groundDistanceValue + airDistanceValue} m";
    }
}

public class GroundDistanceTravelled : IG_Stat {
    private static bool _enabled = true;
    public override bool Enabled {
        get => _enabled;
        set => _enabled = value;
    }
    public static void SetEnabled(bool enabled) {
        _enabled = enabled;
    }
    public override LocalizedString DefaultText => new UnlocalizedString("Ground Distance Travelled: Loaded");
    internal LocalizedString prefix = new UnlocalizedString("Ground Distance Travelled: ");

    public override void OnUpdate(TextMeshProUGUI? text, ColorizedMode colorized) {
        if (text == null || !Enabled) return;
        const BindingFlags bindingFlags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
        FieldInfo? groundDistanceField = typeof(HasteStats).GetField("m_RUN_DistanceTravelled_Ground", bindingFlags);
        if (groundDistanceField == null)
            throw new MissingMemberException("Field 'm_RUN_DistanceTravelled_Ground' not found on HasteStats.");
        object? groundValue = groundDistanceField.GetValue(null);
        if (groundValue is not float groundDistanceValue)
            throw new InvalidCastException("Field 'm_RUN_DistanceTravelled_Ground' is not of type float.");
        text.text = $"{prefix}{groundDistanceValue} m";
    }
}

public class AirDistanceTravelled : IG_Stat {
    private static bool _enabled = true;
    public override bool Enabled {
        get => _enabled;
        set => _enabled = value;
    }
    public static void SetEnabled(bool enabled) {
        _enabled = enabled;
    }
    public override LocalizedString DefaultText => new UnlocalizedString("Air Distance Travelled: Loaded");
    internal LocalizedString prefix = new UnlocalizedString("Air Distance Travelled: ");

    public override void OnUpdate(TextMeshProUGUI? text, ColorizedMode colorized) {
        if (text == null || !Enabled) return;
        const BindingFlags bindingFlags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
        FieldInfo? airDistanceField = typeof(HasteStats).GetField("m_RUN_DistanceTravelled_Air", bindingFlags);
        if (airDistanceField == null)
            throw new MissingMemberException("Field 'm_RUN_DistanceTravelled_Air' not found on HasteStats.");
        object? airValue = airDistanceField.GetValue(null);
        if (airValue is not float airDistanceValue)
            throw new InvalidCastException("Field 'm_RUN_DistanceTravelled_Air' is not of type float.");
        text.text = $"{prefix}{airDistanceValue} m";
    }
}
