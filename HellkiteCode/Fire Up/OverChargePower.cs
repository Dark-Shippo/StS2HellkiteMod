using Hellkite.HellkiteCode.Powers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Hellkite.HellkiteCode.Fire_Up;

public sealed class OverChargePower : HellkitePower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Single;

    public override Decimal ModifyDamageMultiplicative(
        Creature? target,
        Decimal amount,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource)
    {
        return dealer != null && (dealer != Owner && !Owner.Pets.Contains(dealer) || !props.IsPoweredAttack() || cardSource == null) ? 1M : 2M;
    }
    
    public override async Task BeforeSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (side != CombatSide.Player) return;
        await HellkiteCmd.AttackAll(choiceContext, null, Amount);
        await PowerCmd.Remove(this);
        await PowerCmd.Apply<Drained>(choiceContext, Owner, 1M, Owner, null);
    }
}