using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace SystemTime
{
  internal sealed class ModEntry : Mod
  {
    private ModConfig? Config;
    private bool Draw = true;
    private Texture2D? LabelTexture;
    private SpriteFont? LabelFont;

    public override void Entry(IModHelper helper)
    {
      this.Config = helper.ReadConfig<ModConfig>();
      this.LabelTexture = helper.GameContent.Load<Texture2D>("LooseSprites/textBox");
      this.LabelFont = helper.GameContent.Load<SpriteFont>("Fonts/SmallFont");
      helper.Events.GameLoop.GameLaunched += this.OnGameLaunched;
      helper.Events.Input.ButtonPressed += this.OnButtonPressed;
      helper.Events.Display.RenderedHud += this.OnRenderedHud;
    }

    private void OnGameLaunched(object? sender, GameLaunchedEventArgs e)
    {
      IGenericModConfigMenuApi? configMenu = this.Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
      if (this.Config is null || configMenu is null)
        return;

      configMenu.Register(
          mod: this.ModManifest,
          reset: () => this.Config = new ModConfig(),
          save: () => this.Helper.WriteConfig(this.Config)
      );

      configMenu.AddKeybindList(
          mod: this.ModManifest,
          name: () => "Toggle with:",
          tooltip: () => "The keybinding with which you can toggle the label on/off.",
          getValue: () => this.Config.ToggleKeybind,
          setValue: value => this.Config.ToggleKeybind = value
      );
    }

    private void OnButtonPressed(object? sender, ButtonPressedEventArgs e)
    {
      if (this.Config is null || !this.Config.ToggleKeybind.JustPressed())
        return;

      this.Draw = !this.Draw;
      string state = this.Draw ? "on" : "off";
      this.Monitor.Log($"Toggled {state}.", LogLevel.Debug);
    }

    private void OnRenderedHud(object? sender, RenderedHudEventArgs e)
    {
      if (this.LabelFont is null || this.LabelTexture is null || !this.Draw)
        return;

      string text = DateTime.Now.ToShortTimeString();
      Vector2 textSize = this.LabelFont.MeasureString(text);
      Rectangle textureDestinationRectangle = new(
        x: 0,
        y: 2,
        width: (int)(textSize.X + 12),
        height: (int)textSize.Y
      );
      Vector2 textPosition = new(
        x: textureDestinationRectangle.X + 7.5f,
        y: textureDestinationRectangle.Y + 2.0f
      );

      Game1.spriteBatch.Draw(
        texture: this.LabelTexture,
        destinationRectangle: textureDestinationRectangle,
        sourceRectangle: null,
        color: Color.White,
        rotation: 0,
        origin: Vector2.Zero,
        effects: SpriteEffects.None,
        layerDepth: 1
      );
      Game1.spriteBatch.DrawString(
        spriteFont: this.LabelFont,
        text: text,
        position: textPosition,
        color: Color.Black
      );
      Game1.spriteBatch.DrawString(
        spriteFont: this.LabelFont,
        text: text,
        position: textPosition + new Vector2(-1.5f, 1.5f),
        color: Color.Black * 0.1f
      );
    }
  }
}
