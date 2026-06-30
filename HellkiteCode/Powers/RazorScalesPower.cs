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

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    public override async Task AfterDamageReceived(
        PlayerChoiceContext choiceContext,
        Creature target,
        DamageResult result,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource)
    {
        if (target != Owner)
            return;

        if (dealer == null || dealer.Side == Owner.Side)
            return;

        // Retaliate on any incoming attack, even if it was fully blocked.
        if (result.TotalDamage <= 0)
            return;

        int retaliationDamage = Amount;

        Flash();

        await CreatureCmd.Damage(
            choiceContext,
            dealer,
            retaliationDamage,
            ValueProp.Unpowered | ValueProp.Unblockable,
            Owner,
            null);

        await PowerCmd.ModifyAmount(
            choiceContext,
            this,
            -1M,
            Owner,
            null);
    }
}