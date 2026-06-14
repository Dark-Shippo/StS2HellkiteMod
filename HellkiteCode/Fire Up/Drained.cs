using Hellkite.HellkiteCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Hellkite.HellkiteCode.Fire_Up;

public sealed class Drained : HellkitePower
{
    public override PowerType Type => PowerType.Debuff;

    public override PowerStackType StackType => PowerStackType.Single;
    
    public override Decimal ModifyDamageMultiplicative(
        Creature? target,
        Decimal amount,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource)
    {
        return target != Owner || !props.IsPoweredAttack() ? 1M : 2M;
    }

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        Drained power = this;
        // Remove the two kindle gained from Charge
        int kindleAmount = power.Owner.GetPowerAmount<KindlePower>();
        if (kindleAmount > 0)
        {
            decimal toRemove = Math.Min(2M, kindleAmount);
            var kindle = power.Owner.GetPower<KindlePower>();
            if (kindle != null)
            {
                await PowerCmd.ModifyAmount(kindle, -toRemove, power.Owner, null);
            }
        }
        await PowerCmd.Remove(power);
    }
}