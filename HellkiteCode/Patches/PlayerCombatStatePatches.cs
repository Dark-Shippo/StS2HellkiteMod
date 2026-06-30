using System.Reflection.Emit;
using BaseLib.Utils.Patching;
using HarmonyLib;
using Hellkite.HellkiteCode.Cards;
using Hellkite.HellkiteCode.Extensions;
using Hellkite.HellkiteCode.Field;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;

namespace Hellkite.HellkiteCode.Patches;

[HarmonyPatch(typeof(PlayerCombatState), MethodType.Constructor)]
[HarmonyPatch([typeof(Player)])]
internal class PlayerCombatStateConstructorPatch
{
    [HarmonyPostfix]
    private static void Postfix(Player player, PlayerCombatState __instance)
    {
        var hellkiteCombatState = new PlayerCombatStateExtension.HellkiteCombatState(__instance);

        HellkiteField.HellkiteCombatState[__instance] = hellkiteCombatState;

        CombatManager.Instance.StateTracker.SubscribeFireUp(hellkiteCombatState);
    }
}

[HarmonyPatch(typeof(PlayerCombatState), nameof(PlayerCombatState.AfterCombatEnd))]
internal class PlayerCombatStateAfterCombatEndPatch
{
    [HarmonyPostfix]
    public static void Postfix(PlayerCombatState __instance)
    {
        var hellkiteCombatState = __instance.Hellkite();
        if (hellkiteCombatState != null) CombatManager.Instance.StateTracker.UnsubscribeFireUp(hellkiteCombatState);
    }
}



[HarmonyPatch(typeof(PlayerCombatState), nameof(PlayerCombatState.HasEnoughResourcesFor))]
internal class PlayerCombatStateHasEnoughResourcesForPatch
{
    [HarmonyTranspiler]
    private static List<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        return new InstructionPatcher(instructions).Match(new InstructionMatcher()
            .ldarg_2()
            .opcode(OpCodes.Ldc_I4_0)
            .opcode(OpCodes.Stind_I4)
        ).Insert([
            CodeInstruction.LoadArgument(0), // this (PlayerCombatState)
            CodeInstruction.LoadArgument(1), // card
            CodeInstruction.LoadArgument(2), // ref reason
            CodeInstruction.Call(typeof(PlayerCombatStateHasEnoughResourcesForPatch), nameof(HasEnoughFireUp))
        ]);
    }

    private static void HasEnoughFireUp(PlayerCombatState instance, CardModel card, ref UnplayableReason reason)
    {
        if (card is not HellkiteCard hellkiteCard) return;
        if (!instance.GetFireUp().CanSpend(hellkiteCard.GetFireUpCostWithModifiers()))
            reason |= UnplayableReason.StarCostTooHigh;
    }
    
    [HarmonyPatch(typeof(PlayerCombatState), nameof(PlayerCombatState.IncrementTurnNumber))]
    internal class PlayerCombatStateIncrementTurnNumberPatch
    {
        [HarmonyPostfix]
        private static void Postfix(PlayerCombatState __instance)
            => __instance.Hellkite()?.ResetTurnTracking();
    }
}