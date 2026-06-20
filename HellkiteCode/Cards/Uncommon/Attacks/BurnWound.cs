using Hellkite.HellkiteCode.Fire_Up;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Hellkite.HellkiteCode.Cards.Uncommon.Attacks;

public class BurnWound() : HellkiteCard(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new ChargeCostVar(1M), 
        new DamageVar(10M, ValueProp.Move)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay player)
    {
        if (!await ChargeHandler.TrySpendCharge(Owner.Creature, DynamicVars[ChargeCostVar.DefaultName].BaseValue, choiceContext)) return;
        if (player.Target != null)
        {
            await HellkiteCmd.AttackTarget(choiceContext, this, player.Target, DynamicVars.Damage.BaseValue);
            if (player.Target != null)
                await HellkiteCmd.TriggerScorchOnce(choiceContext, player.Target, Owner.Creature, this);
        }
    }
    
    protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(3M);
}