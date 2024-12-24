using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;

namespace SystemTime
{
  public class TextBox
  {
    private readonly Rectangle topLeftCornerRectangle = new(0, 0, 16, 16);
    private readonly Rectangle topSideRectangle = new(16, 0, 16, 16);
    private readonly Rectangle topRightCornerRectangle = new(172, 0, 16, 16);
    private readonly Rectangle rightSideRectangle = new(172, 16, 16, 16);
    private readonly Rectangle bottomRightCornerRectangle = new(172, 32, 16, 16);
    private readonly Rectangle bottomSideRectangle = new(16, 32, 16, 16);
    private readonly Rectangle bottomLeftCornerRectangle = new(0, 32, 16, 16);
    private readonly Rectangle leftSideRectangle = new(0, 16, 16, 16);
    private readonly Rectangle centerRectangle = new(16, 16, 16, 16);
    private readonly SpriteFont font;
    private readonly Texture2D texture;

    public TextBox(IGameContentHelper gameContent)
    {
      this.font = gameContent.Load<SpriteFont>("Fonts/SmallFont");
      this.texture = Utils.CopyTexture(
        gameContent.Load<Texture2D>("LooseSprites/textBox"),
        sourceRectangle: new Rectangle(4, 0, 188, 48)
      );
    }

    public void Draw(SpriteBatch spriteBatch, string text, Vector2 position)
    {
      Vector2 textDimensions = this.font.MeasureString(text);
      Vector2 textureDimensions = new(
        Math.Max(32, (((int)textDimensions.X / 16) + 1) * 16) + 16,
        Math.Max(32, (((int)textDimensions.Y / 16) + 1) * 16)
      );

      for (int offsetX = 0; offsetX < textureDimensions.X; offsetX += 16)
      {
        for (int offsetY = 0; offsetY < textureDimensions.Y; offsetY += 16)
        {
          Rectangle sourceRectangle;
          if (offsetX == 0 && offsetY == 0)
            sourceRectangle = this.topLeftCornerRectangle;
          else if (offsetX == 0 && offsetY < (textureDimensions.Y - 16))
            sourceRectangle = this.leftSideRectangle;
          else if (offsetX == 0 && offsetY == (textureDimensions.Y - 16))
            sourceRectangle = this.bottomLeftCornerRectangle;
          else if (offsetX < (textureDimensions.X - 16) && offsetY == 0)
            sourceRectangle = this.topSideRectangle;
          else if (offsetX < (textureDimensions.X - 16) && offsetY < (textureDimensions.Y - 16))
            sourceRectangle = this.centerRectangle;
          else if (offsetX < (textureDimensions.X - 16) && offsetY == (textureDimensions.Y - 16))
            sourceRectangle = this.bottomSideRectangle;
          else if (offsetX == (textureDimensions.X - 16) && offsetY == 0)
            sourceRectangle = this.topRightCornerRectangle;
          else if (offsetX == (textureDimensions.X - 16) && offsetY < (textureDimensions.Y - 16))
            sourceRectangle = this.rightSideRectangle;
          else if (offsetX == (textureDimensions.X - 16) && offsetY == (textureDimensions.Y - 16))
            sourceRectangle = this.bottomRightCornerRectangle;
          else
            continue; // this should never happen

          Vector2 offsetPosition = new(
            position.X + offsetX,
            position.Y + offsetY
          );
          spriteBatch.Draw(
            texture: this.texture,
            position: offsetPosition,
            sourceRectangle: sourceRectangle,
            color: Color.White
          );
        }
      }

      Vector2 textPosition = new(
        position.X + textureDimensions.X / 2 - textDimensions.X / 2,
        position.Y + textureDimensions.Y / 2 - textDimensions.Y / 2
      );

      spriteBatch.DrawString(
        spriteFont: this.font,
        text: text,
        position: textPosition,
        color: Color.Black
      );
    }
  }
}
