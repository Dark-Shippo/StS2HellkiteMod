using Hellkite.HellkiteCode.Extensions;
using Hellkite.HellkiteCode.Fire_Up;
using Hellkite.HellkiteCode.Structs;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Hellkite.HellkiteCode.Cards.Uncommon.Attacks;

public sealed class SearingRush() : HellkiteCard(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(6M, ValueProp.Move), 
        new PowerVar<VigorPower>(4M),
        //new ChargeCostVar(2)
    ];
    
    public override FireUp CanonicalFireUpCost => new(2);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        //await ChargeHandler.LoseCharge(Owner.Creature, DynamicVars[ChargeCostVar.DefaultName].IntValue, choiceContext);

        var fireUp = Owner.PlayerCombatState?.GetFireUp() ?? new FireUp();
        bool highHeat = fireUp.CanSpend(new FireUp(21));
        //bool highHeat = ChargeHandler.GetCharge(Owner.Creature) >= 21;
        if (play.Target != null)
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(this)
                .Targeting(play.Target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);
        if (highHeat) await PowerCmd.Apply<VigorPower>(choiceContext, Owner.Creature, DynamicVars[nameof(VigorPower)].BaseValue, Owner.Creature, this);
    }
    
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2M); 
        DynamicVars[nameof(VigorPower)].UpgradeValueBy(2M);
    }
}