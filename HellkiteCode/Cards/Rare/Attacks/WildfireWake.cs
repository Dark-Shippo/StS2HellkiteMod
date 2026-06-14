using Hellkite.HellkiteCode.Fire_Up;
using Hellkite.HellkiteCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Hellkite.HellkiteCode.Cards.Rare.Attacks;

public sealed class WildfireWake() : HellkiteCard(0, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(7M, ValueProp.Move)];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (CombatState != null)
        {
            var storedScorch = CombatState.HittableEnemies.ToDictionary(e => e, e => e.GetPowerAmount<ScorchPower>());
            await HellkiteCmd.AttackAll(choiceContext, this, DynamicVars.Damage.BaseValue);

            foreach (var pair in storedScorch)
            {
                if (!pair.Key.IsAlive && pair.Value > 0)
                {
                    foreach (var enemy in CombatState.HittableEnemies)
                    {
                        await HellkiteCmd.ApplyScorch(enemy, pair.Value, Owner.Creature, this);
                    }
                }
            }
        }
    }
    protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(2M);
}