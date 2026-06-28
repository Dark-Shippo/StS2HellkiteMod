using HarmonyLib;
using Hellkite.HellkiteCode.Utilities;
using MegaCrit.Sts2.Core.Assets;

namespace Hellkite.HellkiteCode.Patches;

[HarmonyPatch(typeof(AssetSets), nameof(AssetSets.CommonAssets), MethodType.Getter)]
public class AssetSetsPatches
{
    [HarmonyPostfix]
    private static void Postfix(ref IReadOnlySet<string> __result)
    {
        __result = __result.Concat(HellkiteResource.AssetPaths).ToHashSet();
    }
}