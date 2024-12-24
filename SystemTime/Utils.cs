using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SystemTime
{
  public static class Utils
  {
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
