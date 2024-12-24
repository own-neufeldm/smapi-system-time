using StardewModdingAPI.Utilities;

namespace SystemTime
{
  public sealed class ModConfig
  {
    public KeybindList ToggleKeybind { get; set; } = KeybindList.Parse("Q");
    public string Visibility { get; set; } = SystemTime.Visibility.Always;
  }
}
