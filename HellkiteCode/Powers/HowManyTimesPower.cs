using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Hellkite.HellkiteCode.Powers;

public sealed class HowManyTimesPower : HellkitePower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    protected override object InitInternalData() => new Data();

    public override Task BeforeCardPlayed(CardPlay cardPlay)
    {
        if (Applier?.Player == null)
            return Task.CompletedTask;

        if (cardPlay.Card.Owner != Applier.Player)
            return Task.CompletedTask;

        // Snapshot Amount in case the power changes before resolution finishes.
        GetInternalData<Data>()
            .AmountsForPlayedCards[cardPlay.Card] = Amount;

        return Task.CompletedTask;
    }

    public override async Task AfterCardPlayed(
        PlayerChoiceContext choiceContext,
        CardPlay cardPlay)
    {
        Data data = GetInternalData<Data>();

        if (!data.AmountsForPlayedCards.Remove(
                cardPlay.Card,
                out int scorchAmount))
        {
            return;
        }

        if (scorchAmount <= 0)
            return;

        Flash();

        await PowerCmd.Apply<ScorchPower>(
            choiceContext,
            Owner,
            scorchAmount,
            Applier,
            null);
    }

    public override async Task AfterSideTurnEnd(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IEnumerable<Creature> participants)
    {
        // The card says "this turn," so remove it when the player side ends.
        if (side != CombatSide.Player)
            return;

        await PowerCmd.Remove(this);
    }

    private sealed class Data
    {
        public readonly Dictionary<CardModel, int>
            AmountsForPlayedCards = new();
    }
}