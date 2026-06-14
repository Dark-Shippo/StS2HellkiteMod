using Hellkite.HellkiteCode.Cards;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Hellkite.HellkiteCode.Powers;

public sealed class FuelTheFlamesPower : HellkitePower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterSideTurnStart(CombatSide side, CombatState combatState)
    {
        if (side == Owner.Side && ChargeHandler.GetCharge(Owner) >= 11)
        {
            await PowerCmd.Apply<VigorPower>(Owner, DynamicVars[nameof(VigorPower)].BaseValue, Owner, null);
        }
    }
}