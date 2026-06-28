using HarmonyLib;
using Hellkite.HellkiteCode.Cards;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;

namespace Hellkite.HellkiteCode.Patches;

[HarmonyPatch(typeof(UnplayableReasonExtensions), nameof(UnplayableReasonExtensions.GetPlayerDialogueLine))]
public class UnplayableReasonExtensionGetPlayerDialogueLinePatch
{
    [HarmonyPrefix]
    private static bool Prefix(ref LocString? __result, UnplayableReason reason, AbstractModel? preventer)
    {
        if (reason.HasFlag(UnplayableReason.StarCostTooHigh) && preventer is HellkiteCard)
        {
            __result = new LocString("combat_messages", "HELLKITE-NOT_ENOUGH_CHARGE");
            return false;
        }

        return true;
    }

}