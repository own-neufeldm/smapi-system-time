using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace SystemTime
{
  public static class DrawOns
  {
    public static readonly string Rendered = "Rendered";
    public static readonly string RenderedActiveMenu = "Rendered active menu";
    public static readonly string RenderedHud = "Rendered HUD";
    public static readonly string RenderedStep = "Rendered step";
    public static readonly string RenderedWorld = "Rendered world";

    public static List<string> All()
    {
      List<string> all = new();

      foreach (FieldInfo fieldInfo in typeof(DrawOns).GetFields())
      {
        string? value = (string?)fieldInfo.GetValue(null);
        if (value is not null)
          all.Add(value);
      }

      return all;
    }
  }

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

      helper.Events.Display.Rendered += this.OnRendered;
      helper.Events.Display.RenderedActiveMenu += this.OnRenderedActiveMenu;
      helper.Events.Display.RenderedHud += this.OnRenderedHud;
      helper.Events.Display.RenderedStep += this.OnRenderedStep;
      helper.Events.Display.RenderedWorld += this.OnRenderedWorld;
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
          name: () => "Draw on:",
          tooltip: () => "When to draw the label in the rendering process.",
          getValue: () => this.Config.DrawOn,
          setValue: value => this.Config.DrawOn = value,
          allowedValues: DrawOns.All().ToArray()
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
      if (this.Config is null || !this.Config.DrawOn.Equals(DrawOns.Rendered))
        return;

      this.DrawLabel();
    }

    private void OnRenderedActiveMenu(object? sender, RenderedActiveMenuEventArgs e)
    {
      if (this.Config is null || !this.Config.DrawOn.Equals(DrawOns.RenderedActiveMenu))
        return;

      this.DrawLabel();
    }

    private void OnRenderedHud(object? sender, RenderedHudEventArgs e)
    {
      if (this.Config is null || !this.Config.DrawOn.Equals(DrawOns.RenderedHud))
        return;

      this.DrawLabel();
    }

    private void OnRenderedStep(object? sender, RenderedStepEventArgs e)
    {
      if (this.Config is null || !this.Config.DrawOn.Equals(DrawOns.RenderedStep))
        return;

      this.DrawLabel();
    }

    private void OnRenderedWorld(object? sender, RenderedWorldEventArgs e)
    {
      if (this.Config is null || !this.Config.DrawOn.Equals(DrawOns.RenderedWorld))
        return;

      this.DrawLabel();
    }

    private void DrawLabel()
    {
      if (this.LabelFont is null || this.LabelTexture is null || !this.Draw)
        return;

      string text = DateTime.Now.ToShortTimeString();
      Game1.drawDialogueBox(
        centerX: 100,
        centerY: 17,
        speaker: false,
        drawOnlyBox: true,
        message: text
      );
      Utility.drawTextWithShadow(
        b: Game1.spriteBatch,
        text: text,
        font: Game1.dialogueFont,
        position: new Vector2(55, 25),
        color: Color.Black
      );
    }
  }
}
