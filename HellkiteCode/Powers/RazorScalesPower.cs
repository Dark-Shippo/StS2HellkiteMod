using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Hellkite.HellkiteCode.Powers;

public sealed class RazorScalesPower : HellkitePower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterDamageReceived(
        PlayerChoiceContext choiceContext,
        Creature target,
        DamageResult result,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource)
    {
        if (target != Owner || result.UnblockedDamage <= 0 || dealer == null) return;

        await CreatureCmd.Damage(
            new ThrowingPlayerChoiceContext(),
            dealer,
            Amount,
            ValueProp.Unpowered | ValueProp.Unblockable,
            Owner,
            null);

        await PowerCmd.ModifyAmount(this, -1M, Owner, null);
    }
}