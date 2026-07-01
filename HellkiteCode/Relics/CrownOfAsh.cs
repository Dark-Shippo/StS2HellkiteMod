using Hellkite.HellkiteCode.Hooks;
using Hellkite.HellkiteCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Hellkite.HellkiteCode.Relics;

/// <summary>Rare relic. Whenever you enter Overcharge, gain 3 Razor Scales and 3 Plating.</summary>
public class CrownOfAsh() : HellkiteRelic, IAfterEnterOvercharge
{
    private const int RazorScales = 3;
    private const int Plating = 3;

    public override RelicRarity Rarity => RelicRarity.Rare;

    public async Task AfterEnterOvercharge()
    {
        await PowerCmd.Apply<RazorScalesPower>(null, Owner.Creature, RazorScales, Owner.Creature, null);
        await PowerCmd.Apply<PlatingPower>(null, Owner.Creature, Plating, Owner.Creature, null);
    }
}
