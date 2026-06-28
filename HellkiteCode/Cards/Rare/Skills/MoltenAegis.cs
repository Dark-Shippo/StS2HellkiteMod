using Hellkite.HellkiteCode.Fire_Up;
using Hellkite.HellkiteCode.Powers;
using Hellkite.HellkiteCode.Structs;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Hellkite.HellkiteCode.Cards.Rare.Skills;

public sealed class MoltenAegis() : HellkiteCard(2, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<PlatingPower>(5M), 
        new PowerVar<RazorScalesPower>(5M),
        //new ChargeCostVar(3)
    ];
    
    public override FireUp CanonicalFireUpCost => new(3);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        //await ChargeHandler.LoseCharge(Owner.Creature, DynamicVars[ChargeCostVar.DefaultName].IntValue, choiceContext);
        await PowerCmd.Apply<PlatingPower>(choiceContext, Owner.Creature, DynamicVars[nameof(PlatingPower)].BaseValue, Owner.Creature, this); 
        await PowerCmd.Apply<RazorScalesPower>(choiceContext, Owner.Creature, DynamicVars[nameof(RazorScalesPower)].BaseValue, Owner.Creature, this);
    }
    
    protected override void OnUpgrade()
    {
        DynamicVars[nameof(PlatingPower)].UpgradeValueBy(2M); 
        DynamicVars[nameof(RazorScalesPower)].UpgradeValueBy(2M);
    }
}