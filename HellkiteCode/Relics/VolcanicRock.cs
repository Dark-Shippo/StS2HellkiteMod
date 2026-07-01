using Hellkite.HellkiteCode.Powers;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Hellkite.HellkiteCode.Relics;

/// <summary>Shop relic. Enemies with Scorch take 25% more attack damage.</summary>
public class VolcanicRock() : HellkiteRelic
{
    private const decimal DamageMultiplier = 1.25M;

    public override RelicRarity Rarity => RelicRarity.Shop;

    public override decimal ModifyDamageMultiplicative(
        Creature? target,
        decimal amount,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource)
    {
        if (target == null
            || dealer != Owner.Creature
            || !props.IsPoweredAttack())
            return 1M;

        return target.GetPowerAmount<ScorchPower>() > 0 ? DamageMultiplier : 1M;
    }
}
