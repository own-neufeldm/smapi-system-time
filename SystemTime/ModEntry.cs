using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;


namespace SystemTime
{
  internal sealed class ModEntry : Mod
  {
    private bool draw = true;
    private Texture2D? frame;

    public override void Entry(IModHelper helper)
    {
      this.frame = helper.GameContent.Load<Texture2D>("LooseSprites/textBox");
      helper.Events.Input.ButtonPressed += this.OnButtonPressed;
      helper.Events.Display.Rendered += this.OnRendered;
    }

    private void OnButtonPressed(object? sender, ButtonPressedEventArgs e)
    {
      if (!e.Button.Equals(SButton.Q))
      {
        return;
      }
      this.draw = !this.draw;
      string state = this.draw ? "on" : "off";
      this.Monitor.Log($"Toggled {state}.", LogLevel.Debug);
    }

    private void OnRendered(object? sender, RenderedEventArgs e)
    {
      if (!this.draw)
      {
        return;
      }
      Vector2 positionFrame = new(0, 3);
      float scaleFrame = 0.8f;
      Game1.spriteBatch.Draw(
        texture: this.frame,
        position: positionFrame,
        sourceRectangle: null,
        color: Color.White,
        rotation: 0,
        origin: Vector2.Zero,
        scale: scaleFrame,
        effects: SpriteEffects.None,
        layerDepth: 1
      );
      Vector2 positionText = new(positionFrame.X + 48, positionFrame.Y + 4);
      string text = DateTime.Now.ToShortTimeString();
      Game1.spriteBatch.DrawString(
        spriteFont: Game1.smallFont,
        text: text,
        position: positionText,
        color: Color.Black
      );
    }
  }
}
