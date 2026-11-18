using BepInEx;
using HarmonyLib;

namespace KirakiraGodly;

internal static class ModInfo
{
  internal const string Guid = "me.pocke.kirakira-godly";
  internal const string Name = "Kirakira Godly";
  internal const string Version = "1.0.0";
}

[BepInPlugin(ModInfo.Guid, ModInfo.Name, ModInfo.Version)]
internal class KirakiraGodly : BaseUnityPlugin
{
  public void Awake()
  {
    new Harmony(ModInfo.Guid).PatchAll();
  }
}
