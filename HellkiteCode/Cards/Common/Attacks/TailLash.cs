using Hellkite.HellkiteCode.Commands;
using Hellkite.HellkiteCode.Fire_Up;
using Hellkite.HellkiteCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Hellkite.HellkiteCode.Cards.Common.Attacks;

public sealed class TailLash() : HellkiteCard(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => 
        [
            new DamageVar(6M, ValueProp.Move), 
            new DamageVar("BonusDamage", 4M, ValueProp.Move)
        ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (play.Target != null)
        {
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(this)
                .Targeting(play.Target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);
            if (Owner.Creature.GetPowerAmount<RazorScalesPower>() > 0) 
                DamageCmd.Attack(DynamicVars["BonusDamage"].BaseValue);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2M); 
        DynamicVars["BonusDamage"].UpgradeValueBy(2M);
    }
}