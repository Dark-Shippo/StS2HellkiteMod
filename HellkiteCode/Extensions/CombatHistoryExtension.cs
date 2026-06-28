using Hellkite.HellkiteCode.Combat;
using Hellkite.HellkiteCode.Structs;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History;
using MegaCrit.Sts2.Core.Entities.Players;

namespace Hellkite.HellkiteCode.Extensions;

public static class CombatHistoryExtension
{
    public static void FireUpModified(this CombatHistory combatHistory, ICombatState combatState, FireUp amount,
        Player player)
    {
        combatHistory.Add(combatState, new FireUpModifiedEntry(amount, player, combatState.RoundNumber,
            combatState.CurrentSide,
            combatHistory,
            [player]));
    }
}