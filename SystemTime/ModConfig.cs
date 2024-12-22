using StardewModdingAPI.Utilities;

namespace SystemTime
{
  public sealed class ModConfig
  {
    public KeybindList ToggleKeybind { get; set; } = KeybindList.Parse("Q");
    public string Style { get; set; } = Styles.Default;
  }
}
