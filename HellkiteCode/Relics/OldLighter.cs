using Hellkite.HellkiteCode.Commands;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;

namespace Hellkite.HellkiteCode.Relics;

/// <summary>Common relic. At the start of combat, apply 2 Scorch to all enemies.</summary>
public class OldLighter() : HellkiteRelic
{
    private const int Scorch = 2;

    public override RelicRarity Rarity => RelicRarity.Common;

    public override async Task AfterSideTurnStart(
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (side != CombatSide.Player
            || Owner.PlayerCombatState == null
            || Owner.PlayerCombatState.TurnNumber > 1
            || !participants.Contains(Owner.Creature))
            return;

        await HellkiteCmd.ApplyScorchAll(combatState, Scorch, Owner.Creature, null, null!);
    }
}
