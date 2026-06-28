using System.Reflection.Emit;
using BaseLib.Utils.Patching;
using HarmonyLib;
using Hellkite.HellkiteCode.Cards;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models;
using CardModelOnPlayWrapperPatch = Hellkite.HellkiteCode.Patches.CardModelOnPlayWrapperPatch;

namespace Hellkite.HellkiteCode.Patches;

[HarmonyPatch(typeof(CardModel), nameof(CardModel.SetToFreeThisTurn))]
internal class CardModelSetToFreeThisTurnPatch
{
    [HarmonyPostfix]
    private static void Postfix(CardModel __instance)
    {
        if (__instance is HellkiteCard card) card.SetFireUpCostThisTurn(0);
    }
}

[HarmonyPatch(typeof(CardModel), nameof(CardModel.SetToFreeThisCombat))]
internal class CardModelSetToFreeThisCombatPatch
{
    [HarmonyPostfix]
    private static void Postfix(CardModel __instance)
    {
        if (__instance is HellkiteCard card) card.SetFireUpCostThisCombat(0);
    }
}

[HarmonyPatch(typeof(CardModel), nameof(CardModel.EndOfTurnCleanup))]
internal class CardModelEndOfTurnCleanupPatch
{
    [HarmonyPostfix]
    private static void Postfix(CardModel __instance)
    {
        if (__instance is HellkiteCard card)
            if (card.TemporaryFireUpCosts.RemoveAll(c => c.ClearsWhenTurnEnds) > 0)
                card.InvokeFireUpCostChanged();
    }
}

[HarmonyPatch(typeof(CardModel), nameof(CardModel.SpendResources))]
internal class CardModelSpendResourcesPatch
{
    [HarmonyPostfix]
    private static async Task<(int, int)> Postfix(Task<(int, int)> results, CardModel __instance)
    {
        var ret = await results;
        if (__instance is not HellkiteCard card) return ret;
        var fireUpToSpend = card.GetFireUpCostWithModifiers().ClampZero();
        await card.SpendFireUp(fireUpToSpend);
        return ret;
    }
}

[HarmonyPatch(typeof(CardModel), nameof(CardModel.OnPlayWrapper), MethodType.Async)]
internal class CardModelOnPlayWrapperPatch
{
    [HarmonyTranspiler]
    private static List<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        return new InstructionPatcher(instructions)
            .Match(new InstructionMatcher()
                .ldloc_1()
                .ldnull()
                .call(typeof(CardModel), "set_CurrentTarget", [typeof(Creature)])
            ).Insert([
                CodeInstruction.LoadLocal(1),
                CodeInstruction.Call(typeof(CardModelOnPlayWrapperPatch), nameof(FireUpCostChanged))
            ]);
    }

    private static void FireUpCostChanged(CardModel instance)
    {
        if (instance is not HellkiteCard card) return;
        if (card.TemporaryFireUpCosts.RemoveAll(c => c.ClearsWhenCardIsPlayed) > 0) card.InvokeFireUpCostChanged();
    }
}

[HarmonyPatch(typeof(CardModel), nameof(CardModel.CostsEnergyOrStars))]
internal class CardModelCostsEnergyOrStarPatch
{
    [HarmonyPostfix]
    private static void Postfix(ref bool __result, bool includeGlobalModifiers, CardModel __instance)
    {
        if (__result) return;
        if (__instance is not HellkiteCard card) return;
        if (card.HasFireUpCostX) return;
        if (includeGlobalModifiers)
            if (card.GetFireUpCostWithModifiers().Total > 0)
                __result = true;

        if (card.CurrentFireUpCost.Total > 0) __result = true;
    }
}

[HarmonyPatch(typeof(CardModel), nameof(CardModel.FinalizeUpgradeInternal))]
internal class CardModelFinalizeUpgradeInternalPatch
{
    [HarmonyPostfix]
    private static void Postfix(CardModel __instance)
    {
        if (__instance is not HellkiteCard card) return;

        card.WasFireUpCostJustUpgraded = false;
    }
}

[HarmonyPatch(typeof(CardModel), nameof(CardModel.CanPlay), [typeof(UnplayableReason), typeof(AbstractModel)],
    [ArgumentType.Out, ArgumentType.Out])]
internal class CardModelCanPlayPatch
{
    [HarmonyTranspiler]
    private static List<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        // Patch before return
        return new InstructionPatcher(instructions).Match(new InstructionMatcher()
            .opcode(OpCodes.Or)
            .opcode(OpCodes.Stind_I4)
            .ldarg_1()
            .opcode(OpCodes.Ldind_I4)
            .opcode(OpCodes.Ldc_I4_0)
            .opcode(OpCodes.Ceq)
        ).Step(-3).Insert([
            CodeInstruction.LoadArgument(0),
            CodeInstruction.LoadArgument(1),
            new CodeInstruction(OpCodes.Ldind_I4), // Load value of ref reason
            CodeInstruction.LoadArgument(2), // AbstractModel? preventer
            CodeInstruction.Call(typeof(CardModelCanPlayPatch), nameof(GetPreventerModel))
        ]);
    }

    // Only override the preventer if the 'star' cost is too high when it's a hellkite card
    // This also override the preventer that's from hooks, but it shouldn't matter when UnplayableReason.StarCostTooHigh
    // is checked and returned first when getting loc string.
    private static void GetPreventerModel(CardModel card, UnplayableReason reason, ref AbstractModel? preventer)
    {
        if (reason == UnplayableReason.None) return;
        if (card is not HellkiteCard hellkiteCard) return;
        if (reason.HasFlag(UnplayableReason.StarCostTooHigh)) preventer = hellkiteCard;
    }
}