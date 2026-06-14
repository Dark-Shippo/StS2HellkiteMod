using Hellkite.HellkiteCode.Cards;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Hellkite.HellkiteCode.Powers;

public sealed class ScaleMailPower : HellkitePower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;
    
    public override async Task BeforeTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (side == Owner.Side && ChargeHandler.GetCharge(Owner) <= 10)
        {
            await PowerCmd.Apply<PlatingPower>(Owner, DynamicVars[nameof(PlatingPower)].BaseValue, Owner, null);
        }
    }
}