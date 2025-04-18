using System.Reflection;

namespace InGameStats;

public static class InGameStatsUtils {
    /// <summary>
    /// Computes the value of a stat based on its base value and multiplier.
    /// </summary>
    /// <param name="stat">The PlayerStat object containing the base value and multiplier.</param>
    /// <example>For example, if baseValue is 10 and multiplier is 1.5, the result will be 15.</example>
    /// <returns>The computed stat value.</returns>
    public static float ComputeStatValue(PlayerStat stat) => stat.baseValue * stat.multiplier;
    
    /// <summary>
    /// Converts a percentile value to a string representation.
    /// The value is multiplied by 100 and formatted to two decimal places, followed by a percent sign.
    /// </summary>
    /// <param name="percentile">The percentile value to convert.</param>
    /// <example>0.5f will be converted to "50.00%"</example>
    /// <returns>The formatted string representation of the percentile value.</returns>
    public static string Percentile(float percentile) => (percentile * 100).ToString("F2") + "%";

    /// <summary>
    /// Returns a PlayerMovement.Landing's landing score.
    /// </summary>
    /// <param name="landing">The PlayerMovement.Landing object to compute the score from.</param>
    /// <returns>The computed landing score.</returns>
    /// <remarks>
    /// If the sinceGroundedBeforeLanding field is less than or equal to 0.6f, -1f is returned.
    /// This is to avoid getting landing scores when the player is just running on the ground.
    /// </remarks>
    public static float ComputeLandingScore(object landing) {
        const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        Type landingClassType = landing.GetType();
        FieldInfo sinceGroundedBeforeLandingField = landingClassType.GetField("sinceGroundedBeforeLanding", bindingFlags);
        if ((float) sinceGroundedBeforeLandingField.GetValue(landing) > 0.6f) {
            FieldInfo landingScoreField = landingClassType.GetField("landingScore", bindingFlags);
            return (float) landingScoreField.GetValue(landing);
        }
        return -1f;
    }
}
