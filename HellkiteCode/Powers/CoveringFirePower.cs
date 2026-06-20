using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Hellkite.HellkiteCode.Powers;

public sealed class CoveringFirePower : HellkitePower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    public override async Task AfterCardPlayed(
        PlayerChoiceContext choiceContext,
        CardPlay cardPlay)
    {
        if (cardPlay.Card.Type != CardType.Attack)
            return;

        Creature attacker = cardPlay.Card.Owner.Creature;

        if (attacker == Owner || attacker.Side != Owner.Side)
            return;

        if (cardPlay.Target != null)
        {
            if (cardPlay.Target.Side == Owner.Side)
                return;

            Flash();

            await PowerCmd.Apply<ScorchPower>(
                choiceContext,
                cardPlay.Target,
                Amount,
                Owner,
                null);

            return;
        }

        if (cardPlay.Card.TargetType != TargetType.AllEnemies)
            return;

        if (Owner.CombatState != null)
        {
            var targets = Owner.CombatState
                .GetOpponentsOf(Owner)
                .Where(creature => creature.IsAlive)
                .ToList();

            if (targets.Count == 0)
                return;

            Flash();

            await PowerCmd.Apply<ScorchPower>(
                choiceContext,
                targets,
                Amount,
                Owner,
                null);
        }
    }
}