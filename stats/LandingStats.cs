// Current Perfect Landing Streak, Best Landing Streak stats and Average Landing Score stats
// for InGameStats mod for Haste by Landfall Games

using InGameStats;
using Landfall.Haste;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;

public enum PL_DetectionMode {
    None,
    Strict,
    Standard,
}

public class PerfectLandingStreak : IG_Stat {
    private int _perfectLandingStreakStandard = 0;
    private int _perfectLandingStreakStrict = 0;
    private static bool _enabled = true;
    public override bool Enabled {
        get => _enabled;
        set => _enabled = value;
    }
    public static void SetEnabled(bool enabled) {
        _enabled = enabled;
    }
    public static PL_DetectionMode detectionMode = PL_DetectionMode.Standard;

    public override LocalizedString DefaultText => new UnlocalizedString("Perfect Landing Streak: Loaded");

    public override void OnUpdate(TextMeshProUGUI? text, ColorizedMode colorized) {
        if (text == null || !Enabled) return;
        if (detectionMode == PL_DetectionMode.Standard) {
            text.text = $"Perfect Landing Streak: {_perfectLandingStreakStandard}";
        } else if (detectionMode == PL_DetectionMode.Strict) {
            text.text = $"Perfect Landing Streak: {_perfectLandingStreakStrict}";
        } else {
            text.text = "Perfect Landing Streak: Missing Detection Mode";
        }
    }

    public override void OnStartNewRun() {
        _perfectLandingStreakStandard = 0;
        _perfectLandingStreakStrict = 0;
    }

    public override void OnLandAction(LandingType landingType, bool saved) {
        if (landingType == LandingType.Perfect) {
            ++_perfectLandingStreakStandard;
        } else {
            _perfectLandingStreakStandard = 0;
        }
    }
    public override void OnLandingScoreComputed(float landingScore) {
        float difficultyTweak = Mathf.Lerp(-0.05f, 0.0f, GameDifficulty.currentDif.landingPresicion);

        LandingType landingType = LandingType.Bad;
        if (landingScore >= 0.7f + difficultyTweak) landingType = LandingType.Ok;
        if (landingScore >= 0.9f + difficultyTweak) landingType = LandingType.Good;
        if (landingScore >= 0.95f + difficultyTweak) landingType = LandingType.Perfect;

        if (landingType == LandingType.Perfect) {
            ++_perfectLandingStreakStrict;
        } else {
            _perfectLandingStreakStrict = 0;
        }
    }
}

public class BestLandingStreak : IG_Stat {
    private int _perfectLandingStreakStandard = 0;
    private int _perfectLandingStreakStrict = 0;
    private int _bestLandingStreakStandard = 0;
    private int _bestLandingStreakStrict = 0;
    private static bool _enabled = true;
    public override bool Enabled {
        get => _enabled;
        set => _enabled = value;
    }
    public static void SetEnabled(bool enabled) {
        _enabled = enabled;
    }
    public static PL_DetectionMode detectionMode = PL_DetectionMode.Standard;

    public override LocalizedString DefaultText => new UnlocalizedString("Best Landing Streak: Loaded");

    public override void OnUpdate(TextMeshProUGUI? text, ColorizedMode colorized) {
        if (text == null || !Enabled) return;
        if (detectionMode == PL_DetectionMode.Standard) {
            text.text = $"Best Landing Streak: {_bestLandingStreakStandard}";
        } else if (detectionMode == PL_DetectionMode.Strict) {
            text.text = $"Best Landing Streak: {_bestLandingStreakStrict}";
        } else {
            text.text = "Best Landing Streak: Missing Detection Mode";
        }
    }

    public override void OnStartNewRun() {
        _perfectLandingStreakStandard = 0;
        _perfectLandingStreakStrict = 0;
        _bestLandingStreakStandard = 0;
        _bestLandingStreakStrict = 0;
    }

    public override void OnLandAction(LandingType landingType, bool saved) {
        if (landingType == LandingType.Perfect) {
            ++_perfectLandingStreakStandard;
            _bestLandingStreakStandard = Mathf.Max(_bestLandingStreakStandard, _perfectLandingStreakStandard);
        } else {
            _perfectLandingStreakStandard = 0;
        }
    }
    public override void OnLandingScoreComputed(float landingScore) {
        float difficultyTweak = Mathf.Lerp(-0.05f, 0.0f, GameDifficulty.currentDif.landingPresicion);

        LandingType landingType = LandingType.Bad;
        if (landingScore >= 0.7f + difficultyTweak) landingType = LandingType.Ok;
        if (landingScore >= 0.9f + difficultyTweak) landingType = LandingType.Good;
        if (landingScore >= 0.95f + difficultyTweak) landingType = LandingType.Perfect;

        if (landingType == LandingType.Perfect) {
            ++_perfectLandingStreakStrict;
            _bestLandingStreakStrict = Mathf.Max(_bestLandingStreakStrict, _perfectLandingStreakStrict);
        } else {
            _perfectLandingStreakStrict = 0;
        }
    }
}

public class AverageLandingScore : IG_Stat {
    private float _averageLandingScore = 0f;
    private int _landingCount = 0;
    private static bool _enabled = true;
    public override bool Enabled {
        get => _enabled;
        set => _enabled = value;
    }
    public static void SetEnabled(bool enabled) {
        _enabled = enabled;
    }
    public override LocalizedString DefaultText => new UnlocalizedString("Average Landing Score: Loaded");

    public override void OnUpdate(TextMeshProUGUI? text, ColorizedMode colorized) {
        if (text == null || !Enabled) return;
        text.text = $"Average Landing Score: {_averageLandingScore * 100:F2}%";
    }

    public override void OnStartNewRun() {
        _averageLandingScore = 0f;
        _landingCount = 0;
    }

    public override void OnLandingScoreComputed(float landingScore) {
        _averageLandingScore += (landingScore - _averageLandingScore) / ++_landingCount;
    }
}
