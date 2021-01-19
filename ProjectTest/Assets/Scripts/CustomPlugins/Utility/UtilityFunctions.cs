using UnityEngine;
using UnityEngine.UI;

public static class UtilityFunctions
{
    public static void ChangeAlpha(this Image image, float newAplha)
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, newAplha);
    }
}
