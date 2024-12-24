using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;

namespace SystemTime
{
  public class TextBox
  {
    private readonly SpriteFont font;
    private readonly Texture2D texture;
    private readonly Rectangle topLeftCorner;
    private readonly Rectangle topSide;
    private readonly Rectangle topRightCorner;
    private readonly Rectangle leftSide;
    private readonly Rectangle centerPiece;
    private readonly Rectangle rightSide;
    private readonly Rectangle bottomLeftCorner;
    private readonly Rectangle bottomSide;
    private readonly Rectangle bottomRightCorner;

    public TextBox()
    {
      this.font = Game1.content.Load<SpriteFont>("Fonts/SmallFont");
      this.texture = Utils.CopyTexture(
        Game1.content.Load<Texture2D>("LooseSprites/textBox"),
        sourceRectangle: new Rectangle(4, 0, 188, 48)
      );
      this.topLeftCorner = new(0, 0, 24, 24);
      this.topSide = new(12, 0, 1, 24);
      this.topRightCorner = new(164, 0, 24, 24);
      this.leftSide = new(0, 12, 24, 24);
      this.centerPiece = new(4, 12, 1, 24);
      this.rightSide = new(164, 12, 24, 24);
      this.bottomLeftCorner = new(0, 24, 24, 24);
      this.bottomSide = new(12, 24, 1, 24);
      this.bottomRightCorner = new(164, 24, 24, 24);
    }

    public void Draw(
      SpriteBatch spriteBatch,
      string text,
      Vector2 position,
      Vector2 dimensions, // use Vector2.Zero for auto-fit
      Vector2 textOffset
    )
    {
      int factor = 24;
      Vector2 textDimensions = this.font.MeasureString(text);
      if (dimensions.Equals(Vector2.Zero))
        dimensions = new(
          Math.Max(48, (int)textDimensions.X + factor),
          Math.Max(48, (((int)textDimensions.Y / factor) + 1) * factor)
        );

      Rectangle sourceRectangle = this.topLeftCorner;
      for (int offsetX = 0; offsetX < dimensions.X; offsetX += sourceRectangle.Width)
      {
        for (int offsetY = 0; offsetY < dimensions.Y; offsetY += factor)
        {
          if (offsetX == 0 && offsetY == 0)
            sourceRectangle = this.topLeftCorner;
          else if (offsetX == 0 && offsetY < (dimensions.Y - factor))
            sourceRectangle = this.leftSide;
          else if (offsetX == 0 && offsetY == (dimensions.Y - factor))
            sourceRectangle = this.bottomLeftCorner;
          else if (offsetX < (dimensions.X - topRightCorner.Width) && offsetY == 0)
            sourceRectangle = this.topSide;
          else if (offsetX < (dimensions.X - topRightCorner.Width) && offsetY < (dimensions.Y - factor))
            sourceRectangle = this.centerPiece;
          else if (offsetX < (dimensions.X - topRightCorner.Width) && offsetY == (dimensions.Y - factor))
            sourceRectangle = this.bottomSide;
          else if (offsetX == (dimensions.X - topRightCorner.Width) && offsetY == 0)
            sourceRectangle = this.topRightCorner;
          else if (offsetX == (dimensions.X - topRightCorner.Width) && offsetY < (dimensions.Y - factor))
            sourceRectangle = this.rightSide;
          else if (offsetX == (dimensions.X - topRightCorner.Width) && offsetY == (dimensions.Y - factor))
            sourceRectangle = this.bottomRightCorner;
          else
            // in theory, this case will never happen because width can be
            // drawn per pixel and height is forced to be a multiple of 24  
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
        position.X + dimensions.X / 2 - textDimensions.X / 2 + textOffset.X,
        position.Y + dimensions.Y / 2 - textDimensions.Y / 2 + textOffset.Y
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
