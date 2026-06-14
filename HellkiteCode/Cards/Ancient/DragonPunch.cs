using Hellkite.HellkiteCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Hellkite.HellkiteCode.Cards.Ancient;

public sealed class DragonPunch() : HellkiteCard(1, CardType.Attack, CardRarity.Ancient, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        new DamageVar(10M, ValueProp.Move),
        new PowerVar<ScorchPower>(4M)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        if (play.Target != null)
        {
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(this)
                .Targeting(play.Target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);
            await PowerCmd.Apply<ScorchPower>(
                play.Target,
                DynamicVars[nameof(ScorchPower)].BaseValue,
                Owner.Creature,
                this);

            int targetScorch = play.Target.GetPowerAmount<ScorchPower>();
            if (targetScorch > 0)
            {
                await PowerCmd.Apply<ScorchPower>(play.Target, targetScorch, Owner.Creature, this);
            }
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(14M);
        DynamicVars[nameof(ScorchPower)].UpgradeValueBy(5M);
    }
}