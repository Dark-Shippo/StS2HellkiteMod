using HarmonyLib;
using Hellkite.HellkiteCode.Commands;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Players;

namespace Hellkite.HellkiteCode.Patches;

[HarmonyPatch(typeof(CombatManager), nameof(CombatManager.HandlePlayerDeath))]
public static class CombatManagerHandlePlayerDeathPatch
{
    [HarmonyPostfix]
    private static async Task Postfix(Task results, CombatManager __instance, Player player)
    {
        await results;
        if (__instance.IsInProgress) await HellkitePlayerCmd.ResetFireUp(player);
    }
}