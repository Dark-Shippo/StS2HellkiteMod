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

    protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p)
    {
        if (!await ChargeHandler.TrySpendCharge(Owner.Creature, DynamicVars[ChargeCostVar.DefaultName].BaseValue)) return; 
        await HellkiteCmd.AttackTarget(c, this, p.Target, DynamicVars.Damage.BaseValue);
        if (p.Target != null) await HellkiteCmd.TriggerScorchOnce(c, p.Target, Owner.Creature, this);
    }
    
    protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(3M);
}