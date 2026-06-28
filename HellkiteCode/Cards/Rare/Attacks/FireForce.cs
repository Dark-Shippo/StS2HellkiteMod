using Hellkite.HellkiteCode.Commands;
using Hellkite.HellkiteCode.Fire_Up;
using Hellkite.HellkiteCode.Powers;
using Hellkite.HellkiteCode.Structs;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Hellkite.HellkiteCode.Cards.Rare.Attacks;

public sealed class FireForce() : HellkiteCard(0, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(10M, ValueProp.Move), 
        new PowerVar<ScorchPower>(5M), 
        new PowerVar<VigorPower>(5M),
        //new ChargeCostVar(3)
    ];

    public override FireUp CanonicalFireUpCost => new(3);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        //await ChargeHandler.LoseCharge(Owner.Creature, DynamicVars[ChargeCostVar.DefaultName].IntValue, choiceContext);
        await HellkiteCmd.AttackAll(choiceContext, this, DynamicVars.Damage.BaseValue);
        if (CombatState != null)
            await HellkiteCmd.ApplyScorchAll(CombatState, DynamicVars[nameof(ScorchPower)].BaseValue, Owner.Creature,
                this, choiceContext);
        await PowerCmd.Apply<VigorPower>(choiceContext, Owner.Creature, DynamicVars[nameof(VigorPower)].BaseValue, Owner.Creature, this);
    }
    
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4M); 
        DynamicVars[nameof(ScorchPower)].UpgradeValueBy(2M); 
        DynamicVars[nameof(VigorPower)].UpgradeValueBy(2M);
    }
}