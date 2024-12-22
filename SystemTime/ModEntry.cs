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
    private readonly Label Label = new();

    public override void Entry(IModHelper helper)
    {
      this.Config = helper.ReadConfig<ModConfig>();
      if (!Styles.All().Contains(this.Config.Style))
      {
        this.Config.Style = Styles.Default;
        helper.WriteConfig(this.Config);
      }
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

      configMenu.AddTextOption(
          mod: this.ModManifest,
          name: () => "Label style:",
          tooltip: () => "Defines the frame and font to use for the label.",
          getValue: () => this.Config.Style,
          setValue: value => this.Config.Style = value,
          allowedValues: Styles.All()
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
      if (this.Config is null || !this.Draw)
        return;

      if (!this.Config.Style.Equals(this.Label.Style))
        this.Label.LoadStyle(this.Config.Style, this.Helper);

      Vector2 positionFrame = new(0, 3);
      float scaleFrame = 0.8f;
      Game1.spriteBatch.Draw(
        texture: this.Label.Frame,
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
        spriteFont: this.Label.Font,
        text: text,
        position: positionText,
        color: Color.Black
      );
    }
  }
}
