using Hellkite.HellkiteCode.Fire_Up;
using Hellkite.HellkiteCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Hellkite.HellkiteCode.Cards.Rare.Attacks;

public sealed class ShowOff() : HellkiteCard(0, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
{
    protected override bool HasEnergyCostX => true;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(7M, ValueProp.Move)];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        int x = this.ResolveEnergyXValue();
        if (x <= 0 || play.Target == null) return;

        for (int i = 0; i < x; i++)
        {
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(this)
                .Targeting(play.Target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext); 
            await HellkiteCmd.ApplyScorch(play.Target, 1M, Owner.Creature, this);
        }

        if (x >= 4)
        {
            int scorch = play.Target.GetPowerAmount<ScorchPower>();
            if (scorch > 0) await HellkiteCmd.ApplyScorch(play.Target, scorch, Owner.Creature, this);
        }
    }
    protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(2M);
}