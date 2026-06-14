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
        if (Owner.Player != null)
        {
            Creature? target = Owner.Player.RunState.Rng.CombatTargets.NextItem(CombatState.HittableEnemies);
            if (!GetInternalData<Data>().AmountsForPlayedCards.Remove(cardPlay.Card, out _))
                return;
            Flash();

            if (target != null)
                await PowerCmd.Apply<ScorchPower>(target, DynamicVars[nameof(ScorchPower)].BaseValue, Applier, null);
        }
    }

    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (side != CombatSide.Player)
            return;
        await PowerCmd.Remove(this);
    }

    private class Data
    {
        public readonly Dictionary<CardModel, int> AmountsForPlayedCards = new ();
    }
}