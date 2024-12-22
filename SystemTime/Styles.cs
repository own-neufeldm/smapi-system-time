using System.Reflection;

namespace SystemTime
{
  public static class Styles
  {
    public static readonly string Default = "Default";

    public static string[] All()
    {
      List<string> fieldValues = new();
      foreach (FieldInfo fieldInfo in typeof(Styles).GetFields())
      {
        string? fieldValue = (string?)fieldInfo.GetValue(null);
        if (fieldValue is not null)
        {
          fieldValues.Add(fieldValue);
        }
      }
      return fieldValues.ToArray();
    }
  }
}
