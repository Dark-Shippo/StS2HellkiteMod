using Hellkite.HellkiteCode.Cards;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace Hellkite.HellkiteCode.Powers;

public sealed class ChargeNextTurnPower : HellkitePower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    public override async Task AfterEnergyReset(Player player)
    {
        if (player != Owner.Player) return;
        await ChargeHandler.GainCharge(Owner, Amount);
        await PowerCmd.Remove(this);
    }
}