using Hellkite.HellkiteCode.Commands;
using Hellkite.HellkiteCode.Structs;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Hellkite.HellkiteCode.Relics;

/// <summary>Rare relic. At the end of your turn, gain 1 Charge.</summary>
public class BlazingCrown() : HellkiteRelic
{
    private const int Charge = 1;

    public override RelicRarity Rarity => RelicRarity.Rare;

    public override async Task AfterSideTurnEnd(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IEnumerable<Creature> participants)
    {
        if (side != CombatSide.Player || !participants.Contains(Owner.Creature))
            return;

        await HellkitePlayerCmd.GainFireUp(new FireUp(Charge), Owner);
    }
}
