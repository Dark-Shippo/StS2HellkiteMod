using Hellkite.HellkiteCode.Fire_Up;
using Hellkite.HellkiteCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Hellkite.HellkiteCode.Cards.Rare.Attacks;

public sealed class BearDown() : HellkiteCard(0, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(40M, ValueProp.Move), 
        new PowerVar<ScorchPower>(12M),
        new ChargeCostVar(5M)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await ChargeHandler.LoseCharge(Owner.Creature, DynamicVars[nameof(ChargeCostVar)].BaseValue);
        if (play.Target != null)
        {
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(this)
                .Targeting(play.Target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);
            await HellkiteCmd.ApplyScorch(play.Target, DynamicVars[nameof(ScorchPower)].BaseValue, Owner.Creature,
                this);
        }
    }
    
    protected override void OnUpgrade() => DynamicVars[nameof(ScorchPower)].UpgradeValueBy(4M);
}