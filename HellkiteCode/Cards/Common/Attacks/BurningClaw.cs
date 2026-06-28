using Hellkite.HellkiteCode.Commands;
using Hellkite.HellkiteCode.Fire_Up;
using Hellkite.HellkiteCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Hellkite.HellkiteCode.Cards.Common.Attacks;

public class BurningClaw() : HellkiteCard(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => 
        [
            new DamageVar(7M, ValueProp.Move), 
            new PowerVar<ScorchPower>(3M)
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

            await HellkiteCmd.ApplyScorch(play.Target, DynamicVars[nameof(ScorchPower)].BaseValue, Owner.Creature,
                this, choiceContext);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3M); 
        DynamicVars[nameof(ScorchPower)].UpgradeValueBy(1M);
    }
}
