using Hellkite.HellkiteCode.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Players;

namespace Hellkite.HellkiteCode.Powers;

public sealed class MovingOnPower : HellkitePower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override Task AfterSideTurnStart(CombatSide side, CombatState combatState)
    {
        try
        {
            if (Owner.Player != null)
            {
                Player player = Owner.Player;
                Decimal count = DynamicVars.Cards.IntValue;
                if (ChargeHandler.GetCharge(Owner) <= 5)
                {
                    ModifyHandDraw(player, count);
                }
                else if (ChargeHandler.GetCharge(Owner) > 5)
                {
                    ModifyHandDraw(player, 0M);
                }
            }

            return Task.CompletedTask;
        }
        catch (Exception exception)
        {
            return Task.FromException(exception);
        }
    }
    
    public override Decimal ModifyHandDraw(Player player, Decimal count)
    {   
        if (ChargeHandler.GetCharge(Owner) <= 5)
        {
            return player != Owner.Player ? count : count + Amount;
        }
        return 0;
    }
}