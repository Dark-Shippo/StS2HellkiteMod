using BaseLib.Utils.Patching;
using Godot;
using HarmonyLib;
using Hellkite.HellkiteCode.Extensions;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;

namespace Hellkite.HellkiteCode.Patches;

// Patch to trigger card flash on card being enhanced or stasis
[HarmonyPatch(typeof(NHandCardHolder), nameof(NHandCardHolder.SubscribeToEvents))]
internal class NHandCardHolderSubscribePatch
{
    [HarmonyPrefix]
    private static void Prefix(
        NHandCardHolder __instance, ref CardModel? card
    )
    {
        if (__instance.CardNode != null)
        {
            if (card == null) return;
            var modifier = card.GetCardModelModifier();
        }
    }
}

[HarmonyPatch(typeof(NHandCardHolder), nameof(NHandCardHolder.UnsubscribeFromEvents))]
internal class NHandCardHolderUnsubscribePatch
{
    [HarmonyPrefix]
    private static void Prefix(
        NHandCardHolder __instance, ref CardModel? card
    )
    {
        if (card == null) return;
        var modifier = card.GetCardModelModifier();
    }
}

[HarmonyPatch(typeof(NHandCardHolder), nameof(NHandCardHolder.Flash))]
internal class NHandCardHolderFlashPatch
{
    private static readonly Color BeigeGlow = new("dfbd81fa");

    private static void ShouldGlowBeige(NHandCardHolder instance)
    {
        var cardModel = instance.CardNode?.Model;
        if (cardModel == null) return;

        var modifier = cardModel.GetCardModelModifier();

    }

    [HarmonyTranspiler]
    private static List<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        return new InstructionPatcher(instructions).Match(new InstructionMatcher()
            .ldarg_0()
            .call(typeof(NHandCardHolder), "get_ShouldGlowGold")
            .brfalse_s()
        ).Step(-3).Insert([
            CodeInstruction.LoadArgument(0),
            CodeInstruction.Call((NHandCardHolder instance) => ShouldGlowBeige(instance))
        ]);
    }
}