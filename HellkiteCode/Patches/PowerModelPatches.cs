using HarmonyLib;
using Hellkite.HellkiteCode.Powers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;

namespace Hellkite.HellkiteCode.Patches;

[HarmonyPatch(typeof(PowerModel), nameof(PowerModel.AddDumbVariablesToDescription))]
internal class PowerModelAddDumbVariablesToDescriptionPatche
{
    [HarmonyPostfix]
    private static void Postfix(PowerModel __instance, LocString description)
    {
        if (__instance is HellkitePower) description.Add("fireup", 0);
    }
}