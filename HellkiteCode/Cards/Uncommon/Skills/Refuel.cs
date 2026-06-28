using Hellkite.HellkiteCode.Fire_Up;
using Hellkite.HellkiteCode.Structs;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Hellkite.HellkiteCode.Cards.Uncommon.Skills;

public sealed class Refuel() : HellkiteCard(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new EnergyVar(2),
        //new ChargeCostVar(3)
    ];
    
    public override FireUp CanonicalFireUpCost => new(3);


    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay pl)
    {
        await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, Owner);
    }
    
    protected override void OnUpgrade() => DynamicVars.Energy.UpgradeValueBy(1M);
}