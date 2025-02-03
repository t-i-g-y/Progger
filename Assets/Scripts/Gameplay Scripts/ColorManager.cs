using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColorManager
{
    public static string LOCAL_GREEN = "#5FE762";
    public static string LOCAL_RED = "#FF3131";

    public static void ChangeSpriteColor(SpriteRenderer sprite, string hexColor)
    {
        ColorUtility.TryParseHtmlString(hexColor, out Color newColor);
        sprite.color = newColor;
    }
}
