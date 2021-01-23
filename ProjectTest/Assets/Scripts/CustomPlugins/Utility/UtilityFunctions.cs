using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Contains group of extension functions used for easier and less repeating programming.
/// </summary>
public static class UtilityFunctions
{
    /// <summary>
    /// Changes alpha parameter of image.
    /// </summary>
    /// <param name="image">The image component.</param>
    /// <param name="newAplha">The new alpha value.</param>
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

    /// <summary>
    /// Converts dictionary to single string using provided format.
    /// </summary>
    /// <param name="value">The dictionary that will be parse</param>
    /// <param name="parseFormat">The parse format of every key value element.</param>
    /// <returns></returns>
    public static string ToString(this Dictionary<string, uint> value, string parseFormat)
    {
        StringBuilder sb = new StringBuilder();
        foreach (var currency in value)
        {
            sb.Append(string.Format(parseFormat, currency.Key, currency.Value));
        }

        return sb.ToString();
    }
}
