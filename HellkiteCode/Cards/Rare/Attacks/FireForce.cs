using Hellkite.HellkiteCode.Fire_Up;
using Hellkite.HellkiteCode.Powers;
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
        new ChargeCostVar(3M)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await ChargeHandler.LoseCharge(Owner.Creature, DynamicVars[nameof(ChargeCostVar)].BaseValue);
        await HellkiteCmd.AttackAll(choiceContext, this, DynamicVars.Damage.BaseValue);
        if (CombatState != null)
            await HellkiteCmd.ApplyScorchAll(CombatState, DynamicVars[nameof(ScorchPower)].BaseValue, Owner.Creature,
                this);
        await PowerCmd.Apply<VigorPower>(Owner.Creature, DynamicVars[nameof(VigorPower)].BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4M); 
        DynamicVars[nameof(ScorchPower)].UpgradeValueBy(2M); 
        DynamicVars[nameof(VigorPower)].UpgradeValueBy(2M);
    }
}