using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SystemTime
{
  public static class Utils
  {
    public static class TimeFormat
    {
      public static readonly string HH_mm = "HH:mm";
      public static readonly string hh_mm = "hh:mm";

      public static string[] GetValues()
      {
        return Utils.GetFieldValues(typeof(TimeFormat)).ToArray();
      }
    }

    public static class Visibility
    {
      public static readonly string WhilePlaying = "While playing";
      public static readonly string Always = "Always";

      public static string[] GetValues()
      {
        return Utils.GetFieldValues(typeof(Visibility)).ToArray();
      }
    }

    public static List<string> GetFieldValues(Type type)
    {
      List<string> values = new();
      foreach (FieldInfo fieldInfo in type.GetFields())
      {
        string? value = (string?)fieldInfo.GetValue(null);
        if (value is not null)
          values.Add(value);
      }
      return values;
    }

    public static Color[] GetTextureData(
      Texture2D sourceTexture,
      Rectangle sourceRectangle
    )
    {
      Color[] data = new Color[sourceRectangle.Width * sourceRectangle.Height];
      sourceTexture.GetData(
        level: 0,
        rect: sourceRectangle,
        data: data,
        startIndex: 0,
        elementCount: data.Length
      );
      return data;
    }

    public static Texture2D CopyTexture(
      Texture2D sourceTexture,
      Rectangle sourceRectangle
    )
    {
      Color[] data = Utils.GetTextureData(sourceTexture, sourceRectangle);
      Texture2D destinationTexture = new(
        sourceTexture.GraphicsDevice,
        sourceRectangle.Width,
        sourceRectangle.Height
      );
      destinationTexture.SetData(data);
      return destinationTexture;
    }
  }
}
