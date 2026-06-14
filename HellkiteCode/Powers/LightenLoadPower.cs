using Hellkite.HellkiteCode.Cards;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Entities.Players;

namespace Hellkite.HellkiteCode.Powers;

public sealed class LightenLoadPower : HellkitePower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterEnergyReset(Player player)
    {
        if (ChargeHandler.GetCharge(Owner) >= 21)
        {
            LightenLoadPower power = this;
            await PlayerCmd.GainEnergy(power.Amount, player);
        }
    }
}