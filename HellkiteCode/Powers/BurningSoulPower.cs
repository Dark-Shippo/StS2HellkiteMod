using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Hellkite.HellkiteCode.Powers;

public sealed class BurningSoulPower : HellkitePower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<BurningSoulPower>(Amount)
    ];

    // The first card this turn that successfully attempts to apply Scorch.
    private CardModel? _doubledScorchCard;

    // Becomes true once that card's entire play series has finished.
    private bool _doubledScorchCardFinished;

    public override Task AfterPlayerTurnStart(
        PlayerChoiceContext choiceContext,
        Player player)
    {
        if (player != Owner.Player)
            return Task.CompletedTask;

        _doubledScorchCard = null;
        _doubledScorchCardFinished = false;

        return Task.CompletedTask;
    }

    public override decimal ModifyPowerAmountGivenMultiplicative(
        PowerModel power,
        Creature giver,
        decimal amount,
        Creature? target,
        CardModel? cardSource)
    {
        // Burning Soul has already been consumed for this turn.
        if (_doubledScorchCardFinished)
            return 1M;

        // Only positive Scorch applications qualify.
        if (power is not ScorchPower || amount <= 0M)
            return 1M;

        // Only Scorch applied by this power's owner qualifies.
        if (giver != Owner)
            return 1M;

        // Exclude Scorch from powers, relics, potions, and environmental effects.
        if (cardSource is null)
            return 1M;

        // Ensure this is actually one of the owner's cards.
        if (cardSource.Owner != Owner.Player)
            return 1M;

        // The first card that applies Scorch becomes the doubled card.
        _doubledScorchCard ??= cardSource;

        // Scorch from any later card is unaffected.
        if (!ReferenceEquals(cardSource, _doubledScorchCard))
            return 1M;

        // One Burning Soul stack doubles Scorch.
        // Additional stacks add another copy: 2 stacks = 3x, etc.
        return 1M + Amount;
    }

    public override Task AfterModifyingPowerAmountGiven(PowerModel power)
    {
        // This callback only runs for models that actually modified the amount.
        if (power is ScorchPower)
            Flash();

        return Task.CompletedTask;
    }

    public override Task AfterCardPlayed(
        PlayerChoiceContext choiceContext,
        CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != Owner.Player)
            return Task.CompletedTask;

        // Treat repeated/autoplayed instances in one series as the same card play.
        if (!cardPlay.IsLastInSeries)
            return Task.CompletedTask;

        if (ReferenceEquals(cardPlay.Card, _doubledScorchCard))
            _doubledScorchCardFinished = true;

        return Task.CompletedTask;
    }
}