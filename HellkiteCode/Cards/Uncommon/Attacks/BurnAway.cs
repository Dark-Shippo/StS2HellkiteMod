using Hellkite.HellkiteCode.Commands;
using Hellkite.HellkiteCode.Fire_Up;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Hellkite.HellkiteCode.Cards.Uncommon.Attacks;

public sealed class BurnAway() : HellkiteCard(0, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    public override bool HasFireUpCostX => true;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(5M, ValueProp.Move)];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .WithHitCount(ResolveFireUpXValue())
            .FromCard(this)
            .TargetingRandomOpponents(CombatState)
            .WithHitFx("vfx/vfx_starry_impact")
            .Execute(choiceContext);
    }

    protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(2M);
}