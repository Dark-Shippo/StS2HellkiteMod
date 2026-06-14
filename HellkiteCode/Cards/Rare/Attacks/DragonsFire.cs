using Hellkite.HellkiteCode.Fire_Up;
using Hellkite.HellkiteCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Hellkite.HellkiteCode.Cards.Rare.Attacks;

public sealed class DragonsFire() : HellkiteCard(1, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(10M, ValueProp.Move), 
        new PowerVar<ScorchPower>(3M), 
        new PowerVar<VigorPower>(2M)];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (CombatState != null)
        {
            int alreadyBurning = CombatState.HittableEnemies.Count(e => e.GetPowerAmount<ScorchPower>() > 0);
            await HellkiteCmd.AttackAll(choiceContext, this, DynamicVars.Damage.BaseValue);
            await HellkiteCmd.ApplyScorchAll(CombatState, DynamicVars[nameof(ScorchPower)].BaseValue, Owner.Creature, this);
            if (alreadyBurning > 0) await PowerCmd.Apply<VigorPower>(Owner.Creature, DynamicVars[nameof(VigorPower)].BaseValue * alreadyBurning, Owner.Creature, this);
        }
    }
    
    protected override void OnUpgrade() { DynamicVars.Damage.UpgradeValueBy(4M); DynamicVars[nameof(ScorchPower)].UpgradeValueBy(1M); }
}