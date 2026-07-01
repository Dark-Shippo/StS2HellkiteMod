using Hellkite.HellkiteCode.Powers;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Hellkite.HellkiteCode.Relics;

/// <summary>Rare relic. The first time you apply Scorch each turn, apply it twice.</summary>
public class OldDragonsFang() : HellkiteRelic
{
    private bool _doubledThisTurn;

    public override RelicRarity Rarity => RelicRarity.Rare;

    public override Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player == Owner)
            _doubledThisTurn = false;

        return Task.CompletedTask;
    }

    public override decimal ModifyPowerAmountGivenMultiplicative(
        PowerModel power,
        Creature giver,
        decimal amount,
        Creature? target,
        CardModel? cardSource)
    {
        // Already spent this turn.
        if (_doubledThisTurn)
            return 1M;

        // Only positive Scorch applied by this relic's owner qualifies.
        if (power is not ScorchPower || amount <= 0M || giver != Owner.Creature)
            return 1M;

        // Double it (apply twice).
        return 2M;
    }

    public override Task AfterModifyingPowerAmountGiven(PowerModel power)
    {
        // Only fires when this relic actually modified the amount (real execution, not preview).
        if (power is ScorchPower)
            _doubledThisTurn = true;

        return Task.CompletedTask;
    }
}
