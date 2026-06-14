using Hellkite.HellkiteCode.Cards;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace Hellkite.HellkiteCode.Powers;

public sealed class LivingFurnacePower : HellkitePower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterSideTurnStart(CombatSide side, CombatState combatState)
    {
        if (side == Owner.Side)
        {
            await ChargeHandler.GainCharge(Owner, 2M);
            await PowerCmd.Apply<KindlePower>(Owner, 1M, Owner, null);
        }
    }
}