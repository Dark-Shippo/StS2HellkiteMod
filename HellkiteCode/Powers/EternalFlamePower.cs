using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Hellkite.HellkiteCode.Powers;

public sealed class EternalFlamePower : HellkitePower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    protected override object InitInternalData() => new Data();

    public override Task BeforeCardPlayed(CardPlay cardPlay)
    {
        if (Owner.Player == null ||
            cardPlay.Card.Owner != Owner.Player)
        {
            return Task.CompletedTask;
        }

        // Snapshot the power amount when the card begins resolving.
        GetInternalData<Data>()
            .AmountsForPlayedCards[cardPlay.Card] = Amount;

        return Task.CompletedTask;
    }

    public override async Task AfterCardPlayed(
        PlayerChoiceContext choiceContext,
        CardPlay cardPlay)
    {
        Data data = GetInternalData<Data>();

        // Check this before using combat RNG. Otherwise every unrelated
        // card play advances the random-target sequence.
        if (!data.AmountsForPlayedCards.Remove(
                cardPlay.Card,
                out int scorchAmount))
        {
            return;
        }

        if (scorchAmount <= 0)
            return;

        Creature? target =
            Owner.Player?.RunState.Rng.CombatTargets.NextItem(
                CombatState.HittableEnemies);

        if (target == null)
            return;

        Flash();

        await PowerCmd.Apply<ScorchPower>(
            choiceContext,
            target,
            scorchAmount,
            Owner,
            null);
    }

    private sealed class Data
    {
        public readonly Dictionary<CardModel, int>
            AmountsForPlayedCards = new();
    }
}