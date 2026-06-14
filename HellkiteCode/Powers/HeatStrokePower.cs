using Hellkite.HellkiteCode.Fire_Up;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Hellkite.HellkiteCode.Powers;

public sealed class HeatStrokePower : HellkitePower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override int DisplayAmount => GetInternalData<Data>().TimesScorched;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new RepeatVar(3)];

    protected override object InitInternalData() => new Data();

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [new HoverTip()];
    
    public override async Task AfterPowerAmountChanged(
        PowerModel power,
        Decimal amount,
        Creature? applier,
        CardModel? cardSource)
    {
        var data = GetInternalData<Data>();
        ++data.TimesScorched;
        if (data.TimesScorched >= 3)
        {
            InvokeDisplayAmountChanged();
            Flash();
            await HellkiteCmd.AttackAll(null, null, DynamicVars.Damage.BaseValue);
            data.TimesScorched %= 3;
        }
        InvokeDisplayAmountChanged();
    }

    private class Data
    {
        public int TimesScorched;
    }
}