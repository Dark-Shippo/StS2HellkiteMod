using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Hellkite.HellkiteCode.Powers;

public sealed class TemperPower : HellkitePower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;
    
    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay play)
    {
        if (play.Card.Owner != Owner.Player || !play.IsLastInSeries)
            return;
        await CreatureCmd.GainBlock(Owner, DynamicVars.Block, play);
    }

    public async Task AfterChargeSpent(int amount, Player spender)
    {
        CombatState combatState = CombatState;
        if (amount <= 0 || spender != Owner.Player)
            return;
        var hittableEnemies = combatState.HittableEnemies;
        if (hittableEnemies is { Count: > 0 })
        {
            var randomEnemy = spender.RunState.Rng.CombatTargets.NextItem(hittableEnemies);
            if (randomEnemy != null)
            {
                    await PowerCmd.Apply<ScorchPower>(
                        randomEnemy,
                        DynamicVars[nameof(ScorchPower)].BaseValue,
                        this.Owner,
                        null);
            }
        }
    }
}