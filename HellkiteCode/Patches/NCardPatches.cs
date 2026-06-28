using BaseLib.Utils.Patching;
using Godot;
using HarmonyLib;
using Hellkite.HellkiteCode.Cards;
using Hellkite.HellkiteCode.Field;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Nodes.Cards;

namespace Hellkite.HellkiteCode.Patches;

// TODO Implement check for forceUnpoweredPreview?
[HarmonyPatch(typeof(NCard), nameof(NCard.UpdateVisuals))]
internal class NCardUpdateVisualsPatch
{
    [HarmonyTranspiler]
    private static List<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        return new InstructionPatcher(instructions).Match(new InstructionMatcher()
            .ldarg_0()
            .ldarg_1()
            .call(typeof(NCard), nameof(NCard.UpdateStarCostVisuals), [typeof(PileType)])
        ).Insert([
            CodeInstruction.LoadArgument(0),
            CodeInstruction.LoadArgument(1),
            //CodeInstruction.Call(typeof(NCardUpdateVisualsPatch), nameof(UpdateHellkiteVisuals))
        ]);
    }

    //private static void UpdateHellkiteVisuals(NCard instance, PileType pileType)
    //{
    //    
//
    //    var fireUpIcon = HellkiteNode.NFireUpIcon[instance];
    //    fireUpIcon?.UpdateFireUpCostVisuals(pileType);
    //}
}

[HarmonyPatch(typeof(NCard), nameof(NCard.SetPretendCardCanBePlayed))]
internal class NCardSetPretendCardCanBePlayedPatch
{
    [HarmonyPostfix]
    private static void Postfix(NCard __instance)
    {
        //var fireUpIcon = HellkiteNode.NFireUpIcon[__instance];
        //fireUpIcon?.UpdateFireUpCostVisuals(__instance.DisplayingPile);
    }
}

[HarmonyPatch(typeof(NCard), nameof(NCard.UpdateEnchantmentVisuals))]
internal class NCardUpdateEnchantmentVisualsPatch
{
    [HarmonyPostfix]
    private static void Postfix(NCard __instance)
    {
        if (__instance.Model is HellkiteCard { BaseFireUpCost.Total: >= 0 })
            __instance._enchantmentTab.Position = __instance._defaultEnchantmentPosition + Vector2.Down * 20f;
    }
}