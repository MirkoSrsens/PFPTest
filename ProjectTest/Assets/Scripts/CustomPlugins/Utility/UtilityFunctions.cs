using UnityEngine;
using UnityEngine.UI;

public static class UtilityFunctions
{
    public static void ChangeAlpha(this Image image, float newAplha)
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, newAplha);
    }

    /// <summary>
    /// Calculate cost of next upgrade locally to get some overhead from server,
    /// since its just visual.
    /// </summary>
    /// <param name="level">Level upgrade</param>
    /// <returns></returns>
    public static int CalculateCost(this int level)
    {
        return ((level * 10) + 2) / 2;
    }
}
