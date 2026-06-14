using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Combat;

namespace Hellkite.HellkiteCode.Powers;

public sealed class BurningAuraPower : HellkitePower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterSideTurnStart(CombatSide side, CombatState combatState)
    {
        if (side == Owner.Side)
        {
            foreach (Creature target in combatState.HittableEnemies)
            {
                await PowerCmd.Apply<ScorchPower>(target, Amount, Owner, null);
            }
        }
    }
}