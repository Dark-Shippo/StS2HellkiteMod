using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace Hellkite.HellkiteCode.Powers;

public sealed class TemperPower : HellkitePower
{
    private const decimal ScorchPerChargeSpend = 3M;

    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    public override async Task AfterCardPlayed(
        PlayerChoiceContext choiceContext,
        CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != Owner.Player)
            return;

        if (!cardPlay.IsLastInSeries)
            return;

        Flash();

        await CreatureCmd.GainBlock(
            Owner,
            Amount,
            ValueProp.Unpowered,
            null,
            true);
    }

    public async Task AfterChargeSpent(
        PlayerChoiceContext choiceContext,
        decimal amount,
        Player spender)
    {
        if (amount <= 0)
            return;

        if (spender != Owner.Player)
            return;

        IReadOnlyList<Creature> enemies =
            CombatState.HittableEnemies;

        if (enemies.Count == 0)
            return;

        Creature? target =
            spender.RunState.Rng.CombatTargets.NextItem(enemies);

        if (target == null)
            return;

        Flash();

        await PowerCmd.Apply<ScorchPower>(
            choiceContext,
            target,
            ScorchPerChargeSpend,
            Owner,
            null);
    }
    
    public override async Task AfterSideTurnStart(
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (!participants.Contains(Owner))
            return;
        await PowerCmd.Decrement(this);
    }
}