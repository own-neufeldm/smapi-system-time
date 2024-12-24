using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;

namespace SystemTime
{
  public class TextBox
  {
    private readonly SpriteFont font;
    private readonly Texture2D texture;
    private readonly Rectangle topLeftCornerRectangle = new(0, 0, 16, 16);
    private readonly Rectangle topSideRectangle = new(16, 0, 16, 16);
    private readonly Rectangle topRightCornerRectangle = new(172, 0, 16, 16);
    private readonly Rectangle rightSideRectangle = new(172, 16, 16, 16);
    private readonly Rectangle bottomRightCornerRectangle = new(172, 32, 16, 16);
    private readonly Rectangle bottomSideRectangle = new(16, 32, 16, 16);
    private readonly Rectangle bottomLeftCornerRectangle = new(0, 32, 16, 16);
    private readonly Rectangle leftSideRectangle = new(0, 16, 16, 16);
    private readonly Rectangle centerRectangle = new(16, 16, 16, 16);

    public TextBox()
    {
      this.font = Game1.content.Load<SpriteFont>("Fonts/SmallFont");
      this.texture = Utils.CopyTexture(
        Game1.content.Load<Texture2D>("LooseSprites/textBox"),
        sourceRectangle: new Rectangle(4, 0, 188, 48)
      );
    }

    public void Draw(
      SpriteBatch spriteBatch,
      string text,
      Vector2 position,
      Vector2 dimensions // use Vector2.Zero for auto-fit
    )
    {
      Vector2 textDimensions = this.font.MeasureString(text);
      if (dimensions.Equals(Vector2.Zero))
        dimensions = new(
          Math.Max(32, (((int)textDimensions.X / 16) + 1) * 16) + 16,
          Math.Max(32, (((int)textDimensions.Y / 16) + 1) * 16)
        );

      for (int offsetX = 0; offsetX < dimensions.X; offsetX += 16)
      {
        for (int offsetY = 0; offsetY < dimensions.Y; offsetY += 16)
        {
          Rectangle sourceRectangle;
          if (offsetX == 0 && offsetY == 0)
            sourceRectangle = this.topLeftCornerRectangle;
          else if (offsetX == 0 && offsetY < (dimensions.Y - 16))
            sourceRectangle = this.leftSideRectangle;
          else if (offsetX == 0 && offsetY == (dimensions.Y - 16))
            sourceRectangle = this.bottomLeftCornerRectangle;
          else if (offsetX < (dimensions.X - 16) && offsetY == 0)
            sourceRectangle = this.topSideRectangle;
          else if (offsetX < (dimensions.X - 16) && offsetY < (dimensions.Y - 16))
            sourceRectangle = this.centerRectangle;
          else if (offsetX < (dimensions.X - 16) && offsetY == (dimensions.Y - 16))
            sourceRectangle = this.bottomSideRectangle;
          else if (offsetX == (dimensions.X - 16) && offsetY == 0)
            sourceRectangle = this.topRightCornerRectangle;
          else if (offsetX == (dimensions.X - 16) && offsetY < (dimensions.Y - 16))
            sourceRectangle = this.rightSideRectangle;
          else if (offsetX == (dimensions.X - 16) && offsetY == (dimensions.Y - 16))
            sourceRectangle = this.bottomRightCornerRectangle;
          else
            // dimensions are not divisible by 16, texture will be incomplete
            continue;

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
        position.X + dimensions.X / 2 - textDimensions.X / 2,
        // offset by additional 2 pixel, for unknown reasons text is too high
        // -> maybe because the texture map for this font is not "clean"
        position.Y + dimensions.Y / 2 - textDimensions.Y / 2 + 2
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
