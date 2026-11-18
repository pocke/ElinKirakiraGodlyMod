using HarmonyLib;

namespace KirakiraGodly;

[HarmonyPatch]
public static class GodlyFurnitureSparklePatch
{
  private const string ExtraId = "c_unique";

  [HarmonyPostfix, HarmonyPatch(typeof(CardRenderer), nameof(CardRenderer.RefreshExtra))]
  public static void CardRenderer_RefreshExtra_Postfix(CardRenderer __instance)
  {
    RefreshSparkle(__instance);
  }

  // Card.SetEncLv delegates to ModEncLv, so this also covers it.
  [HarmonyPostfix, HarmonyPatch(typeof(Card), nameof(Card.ModEncLv))]
  public static void Card_ModEncLv_Postfix(Card __instance)
  {
    var renderer = __instance.renderer;
    // Off-screen renderers get RefreshExtra from OnEnterScreen.
    if (renderer == null || !renderer.isSynced)
    {
      return;
    }

    RefreshSparkle(renderer);
  }

  private static void RefreshSparkle(CardRenderer renderer)
  {
    if (!renderer.owner.isThing)
    {
      return;
    }

    if (IsGodly(renderer.owner.Thing))
    {
      renderer.AddExtra(ExtraId);
    }
    else
    {
      renderer.RemoveExtra(ExtraId);
    }
  }

  // Same threshold as the "quality_furniture" prefix in Thing.GetName, whose last entry is "世界最高の" / "godly".
  // GetName omits the prefix for renamed things (c_altName), but renamed furniture still sparkles.
  private static bool IsGodly(Thing thing)
  {
    return thing.category.tag.Contains("enc") && thing.encLV >= Lang.GetList("quality_furniture").Length;
  }
}
