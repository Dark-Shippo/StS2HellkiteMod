using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Hellkite.HellkiteCode.Powers;

public sealed class HeatStrokePower : HellkitePower
{
    private const int RequiredApplications = 3;

    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    public override int DisplayAmount =>
        GetInternalData<Data>().TimesScorched;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new RepeatVar(RequiredApplications)
    ];

    protected override object InitInternalData() => new Data();

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        new HoverTip()
    ];

    public override async Task AfterPowerAmountChanged(
        PlayerChoiceContext choiceContext,
        PowerModel power,
        decimal amount,
        Creature? applier,
        CardModel? cardSource)
    {
        // Only count positive Scorch applications made by this power's owner.
        if (power is not ScorchPower)
            return;

        if (amount <= 0M)
            return;

        if (applier != Owner)
            return;

        Data data = GetInternalData<Data>();

        data.TimesScorched++;

        if (data.TimesScorched < RequiredApplications)
        {
            InvokeDisplayAmountChanged();
            return;
        }

        data.TimesScorched %= RequiredApplications;
        InvokeDisplayAmountChanged();

        List<Creature> targets =
            CombatState.HittableEnemies.ToList();

        if (targets.Count == 0)
            return;

        Flash();

        foreach (Creature target in targets)
        {
            await CreatureCmd.Damage(
                choiceContext,
                target,
                Amount,
                ValueProp.Unpowered,
                Owner,
                null);
        }
    }

    private sealed class Data
    {
        public int TimesScorched;
    }
}