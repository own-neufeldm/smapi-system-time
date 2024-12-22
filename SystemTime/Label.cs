using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;

namespace SystemTime
{
  public class Label
  {
    public string? Style
    {
      get
      {
        return this._style;
      }
    }
    private string? _style;

    public Texture2D? Frame
    {
      get
      {
        return this._frame;
      }
    }
    private Texture2D? _frame;

    public SpriteFont? Font
    {
      get
      {
        return this._font;
      }
    }
    private SpriteFont? _font;

    public void LoadStyle(string style, IModHelper helper)
    {
      if (style.Equals(Styles.Default))
      {
        this._frame = helper.GameContent.Load<Texture2D>("LooseSprites/textBox");
        this._font = Game1.smallFont;
      }
      else
      {
        throw new Exception($"Style '{style}' is not defined.");
      }

      this._style = style;
    }
  }
}
