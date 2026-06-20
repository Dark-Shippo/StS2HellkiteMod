using Hellkite.HellkiteCode.Fire_Up;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace Hellkite.HellkiteCode.Powers;

public sealed class MovingOnPower : HellkitePower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    public override decimal ModifyHandDraw(
        Player player,
        decimal count)
    {
        if (player != Owner.Player)
            return count;

        if (ChargeHandler.GetCharge(Owner) > 5)
            return count;

        return count + Amount;
    }
}