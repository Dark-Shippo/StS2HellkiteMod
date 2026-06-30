using Hellkite.HellkiteCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Hellkite.HellkiteCode.Cards.Uncommon.Powers;

public sealed class StaticShock() : HellkiteCard(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new PowerVar<DexterityPower>(1M),
            new PowerVar<RazorScalesPower>(4M)
        ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await PowerCmd.Apply<DexterityPower>(choiceContext, Owner.Creature, DynamicVars[nameof(DexterityPower)].BaseValue, Owner.Creature, this);
        await PowerCmd.Apply<RazorScalesPower>(choiceContext, Owner.Creature, DynamicVars[nameof(RazorScalesPower)].BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars[nameof(RazorScalesPower)].UpgradeValueBy(2M);
    }
}