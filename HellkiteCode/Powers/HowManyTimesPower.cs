using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace Hellkite.HellkiteCode.Powers;

public sealed class HowManyTimesPower : HellkitePower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override object InitInternalData() => new Data();

    public override Task BeforeCardPlayed(CardPlay cardPlay)
    {
        if (Applier?.Player == null || cardPlay.Card.Owner != Applier.Player)
            return Task.CompletedTask;
        GetInternalData<Data>().AmountsForPlayedCards.Add(cardPlay.Card, Amount);
        return Task.CompletedTask;
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (!GetInternalData<Data>().AmountsForPlayedCards.Remove(cardPlay.Card, out int amount))
            return;
        Flash();
        await PowerCmd.Apply<ScorchPower>(Owner, DynamicVars[nameof(ScorchPower)].BaseValue, Applier, null);
    }

    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        HowManyTimesPower power = this;
        if (side != CombatSide.Player)
            return;
        await PowerCmd.Remove( power);
    }

    private class Data
    {
        public readonly Dictionary<CardModel, int> AmountsForPlayedCards = new ();
    }
}