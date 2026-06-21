using Hellkite.HellkiteCode.Fire_Up;
using Hellkite.HellkiteCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Hellkite.HellkiteCode.Cards.Uncommon.Attacks;

public sealed class LavaBath() : HellkiteCard(0, CardType.Attack, CardRarity.Uncommon, TargetType.RandomEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(4M, ValueProp.Move), 
        new PowerVar<ScorchPower>(2M)];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        decimal x = await HellkiteCmd.SpendUpToCharge(Owner.Creature, ChargeHandler.GetCharge(Owner.Creature), choiceContext);
        for (int i = 0; i < x; i++)
        {
            var target = HellkiteCmd.RandomEnemy(Owner.Creature);
            if (target == null) break;
            if (play.Target != null)
                await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                    .FromCard(this)
                    .Targeting(play.Target)
                    .WithHitFx("vfx/vfx_attack_slash")
                    .Execute(choiceContext);
            await HellkiteCmd.ApplyScorch(target, DynamicVars[nameof(ScorchPower)].BaseValue, Owner.Creature, this, choiceContext);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(1M); 
        DynamicVars[nameof(ScorchPower)].UpgradeValueBy(1M);
    }
}