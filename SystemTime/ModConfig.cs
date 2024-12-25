using StardewModdingAPI.Utilities;

namespace SystemTime
{
  public sealed class ModConfig
  {
    public KeybindList ToggleKeybind { get; set; } = KeybindList.Parse("Q");
    public string TimeFormat { get; set; } = Utils.TimeFormat.HH_mm;
    public string Visibility { get; set; } = Utils.Visibility.WhilePlaying;
  }
}
