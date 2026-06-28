using Hellkite.HellkiteCode.Combat;
using Hellkite.HellkiteCode.Structs;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Hellkite.HellkiteCode.Powers;

public sealed class TemperPower : HellkitePower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;
    
    public override async Task AfterCardPlayed(
        PlayerChoiceContext choiceContext,
        CardPlay cardPlay)
    {
        Flash();

        await CreatureCmd.GainBlock(
            Owner,
            Amount,
            ValueProp.Unpowered,
            null,
            true);
        
    }

    public async Task AfterFireUpSpent(FireUp amount, Player spender)
    {
        if (amount.Total <= 0)
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
            choiceContext: null,
            target,
            Amount,
            Owner,
            null);
    }
    
    public override async Task AfterSideTurnEnd(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IEnumerable<Creature> participants)
    {
        if (side != CombatSide.Player)
            return;
        await PowerCmd.Remove(this);
    }
}