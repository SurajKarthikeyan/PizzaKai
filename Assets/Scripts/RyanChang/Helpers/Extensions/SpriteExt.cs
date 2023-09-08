using UnityEngine;

/// <summary>
/// Contains methods pertaining to sprites, sprite renderers, and colors.
/// </summary>
public static class SpriteExt
{
    /// <summary>
    /// Gets Unity color from a hex code.
    /// </summary>
    /// <param name="hexCode"></param>
    /// <returns></returns>
    public static Color GetColorFromHEX(this string hexCode)
    {
        if (!hexCode.Contains("#"))
        {
            hexCode = "#" + hexCode;
        }

        Color color;

        ColorUtility.TryParseHtmlString(hexCode, out color);

        return color;
    }

    /// <summary>
    /// Gets a copy of the texture on the sprite.
    /// </summary>
    /// <param name="sprite"></param>
    /// <returns></returns>
    public static Texture2D GetTexture(this Sprite sprite)
    {
        Texture2D croppedTexture = new Texture2D((int)sprite.rect.width,
            (int)sprite.rect.height);
        Color[] pixels = sprite.texture.GetPixels
        (
            (int)sprite.textureRect.x,
            (int)sprite.textureRect.y,
            (int)sprite.textureRect.width,
            (int)sprite.textureRect.height
        );

        croppedTexture.SetPixels(pixels);
        croppedTexture.Apply();
        return croppedTexture;
    }
}