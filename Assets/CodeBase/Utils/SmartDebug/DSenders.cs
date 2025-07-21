namespace CodeBase.Utils.SmartDebug
{
  public static class DSenders
  {
    public static readonly DSender Application = new(name: "[Application]".Green());
    public static readonly DSender Empty = new(name: "");
    public static readonly DSender UI = new(name: "[UI]");
    public static readonly DSender Saves = new(name: "[Saves]");
    public static readonly DSender Initialization = new(name: "[Initialization]");
    public static readonly DSender SceneData = new(name: "[Scene Data]");
    public static readonly DSender Localization = new(name: "[Localization]");
    public static readonly DSender Steam = new(name: "[Steam]");
  }
}