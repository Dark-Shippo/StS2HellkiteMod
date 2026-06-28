using Hellkite.HellkiteCode.Cards;
using Hellkite.HellkiteCode.Hooks;
using Hellkite.HellkiteCode.Structs;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Helpers.Models;

namespace Hellkite.HellkiteCode.Utilities;

public static class HellkiteCardCostHelper
{
    public static CardCostColor GetFireUpCostColor(HellkiteCard card, ICombatState? state)
    {
        if (state == null) return CardCostColor.Unmodified;

        if (!card.CanPlay(out var reason, out var model)
            && reason.HasFlag(UnplayableReason.StarCostTooHigh)
            && model == card)
            return CardCostColor.InsufficientResources;

        if (TryModifyFireUpCostWithHook(card, state, out var hookModifiedCost))
            return CardCostHelper.GetColorForHookModifiedCost(hookModifiedCost.Total, card.BaseFireUpCost.Total);

        if (card.TemporaryFireUpCost != null)
            return CardCostHelper.GetColorForLocalCost(card.TemporaryFireUpCost.Cost, card.BaseFireUpCost.Total);

        return CardCostColor.Unmodified;
    }

    private static bool TryModifyFireUpCostWithHook(HellkiteCard card, ICombatState state,
        out FireUp hookModifiedCost)
    {
        hookModifiedCost = card.BaseFireUpCost;

        (var isModified, hookModifiedCost) = HellkiteHook.TryModifyFireUpCost(state, card, hookModifiedCost);

        return isModified;
    }
}