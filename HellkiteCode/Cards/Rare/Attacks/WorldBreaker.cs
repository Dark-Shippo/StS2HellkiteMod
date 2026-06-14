using Hellkite.HellkiteCode.Fire_Up;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Hellkite.HellkiteCode.Cards.Rare.Attacks;

public sealed class WorldBreaker() : HellkiteCard(2, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(2M, ValueProp.Move)];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        int spent = await ChargeHandler.SpendAllCharge(Owner.Creature);
        await HellkiteCmd.AttackAll(choiceContext, this, DynamicVars.Damage.BaseValue);
        if (spent > 0)
            if (CombatState != null)
                await HellkiteCmd.ApplyScorchAll(CombatState, spent, Owner.Creature, this);
    }
    
    protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(1M);
}