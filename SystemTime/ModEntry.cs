using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace SystemTime
{
  internal sealed class ModEntry : Mod
  {
    private ModConfig? Config;
    private TextBox? TextBox;
    private bool Draw = true;

    public override void Entry(IModHelper helper)
    {
      this.Config = helper.ReadConfig<ModConfig>();
      this.TextBox = new();
      helper.Events.GameLoop.GameLaunched += this.OnGameLaunched;
      helper.Events.Input.ButtonPressed += this.OnButtonPressed;
      helper.Events.Display.Rendered += this.OnRendered;
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
          name: () => "Visible when:",
          tooltip: () => "Defines when the label should be visible.",
          getValue: () => this.Config.Visibility,
          setValue: value => this.Config.Visibility = value,
          allowedValues: Utils.Visibility.GetValues()
      );

      configMenu.AddTextOption(
          mod: this.ModManifest,
          name: () => "Time format:",
          tooltip: () => "Format to use when drawing time.",
          getValue: () => this.Config.TimeFormat,
          setValue: value => this.Config.TimeFormat = value,
          allowedValues: Utils.TimeFormat.GetValues()
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

    private void OnRendered(object? sender, RenderedEventArgs e)
    {
      if (
        this.Config is null ||
        !this.Draw ||
        !this.Config.Visibility.Equals(Utils.Visibility.Always)
      )
        return;

      this.DrawTextBox();
    }

    private void OnRenderedHud(object? sender, RenderedHudEventArgs e)
    {
      if (
        this.Config is null ||
        !this.Draw ||
        !this.Config.Visibility.Equals(Utils.Visibility.WhilePlaying)
      )
        return;

      this.DrawTextBox();
    }

    private void DrawTextBox()
    {
      if (this.Config is null || this.TextBox is null)
        return;

      this.TextBox.Draw(
        spriteBatch: Game1.spriteBatch,
        text: DateTime.Now.ToString(this.Config.TimeFormat),
        position: new Vector2(75, 10),
        dimensions: new Vector2(90, 48), // ideal for 00:00 text
        textOffset: new Vector2(0, 2) // ideal for displaying time
      );
    }
  }
}
