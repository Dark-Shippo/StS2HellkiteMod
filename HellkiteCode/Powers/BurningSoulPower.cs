using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;


namespace Hellkite.HellkiteCode.Powers;

public sealed class BurningSoulPower : HellkitePower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    private CardModel? _firstScorchCard;
    private bool _firstScorchCardFinished;
    private bool _isDuplicatingScorch;
    
    public override Task AfterPlayerTurnStart(
        PlayerChoiceContext choiceContext,
        Player player)
    {
        if (player != Owner.Player)
            return Task.CompletedTask;

        _firstScorchCard = null;
        _firstScorchCardFinished = false;
        _isDuplicatingScorch = false;

        return Task.CompletedTask;
    }
    
    public override async Task AfterPowerAmountChanged(
        PowerModel power,
        decimal amount,
        Creature? applier,
        CardModel? cardSource)
    {
        // Prevent the duplicated Scorch from triggering this method again.
        if (_isDuplicatingScorch)
            return;

        // Only react to positive Scorch applications.
        if (power is not ScorchPower scorchPower || amount <= 0M)
            return;

        // Burning Soul only doubles Scorch applied by its owner.
        if (applier != Owner)
            return;

        // Environmental, relic, potion, and power-based Scorch do not count.
        // Remove this check if all Scorch sources should qualify.
        if (cardSource == null)
            return;

        // The first card to apply Scorch becomes this turn's doubled card.
        if (_firstScorchCard == null && !_firstScorchCardFinished)
        {
            _firstScorchCard = cardSource;
        }

        // Scorch from later cards is not doubled.
        if (cardSource != _firstScorchCard)
            return;

        try
        {
            _isDuplicatingScorch = true;
            Flash();

            // Apply exactly the amount that was just added.
            await PowerCmd.Apply<ScorchPower>(
                scorchPower.Owner,
                amount,
                Owner,
                cardSource);
        }
        finally
        {
            _isDuplicatingScorch = false;
        }
    }
    
    public override Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != Owner.Player)
            return Task.CompletedTask;

        if (!cardPlay.IsLastInSeries)
            return Task.CompletedTask;

        if (_firstScorchCard == cardPlay.Card)
        {
            _firstScorchCardFinished = true;
        }

        return Task.CompletedTask;
    }
}
