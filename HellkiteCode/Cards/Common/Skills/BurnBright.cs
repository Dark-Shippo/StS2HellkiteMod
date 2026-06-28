using BaseLib.Extensions;
using Hellkite.HellkiteCode.Commands;
using Hellkite.HellkiteCode.DynamicVars;
using Hellkite.HellkiteCode.Fire_Up;
using Hellkite.HellkiteCode.Structs;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Hellkite.HellkiteCode.Cards.Common.Skills;

public sealed class BurnBright() : HellkiteCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        //new ChargeCostVar(8)
        new FireUpVar(8).WithUpgrade(1)
    ];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play) => 
        //await ChargeHandler.GainCharge(Owner.Creature, DynamicVars[ChargeCostVar.DefaultName].IntValue, choiceContext);
    await HellkitePlayerCmd.GainFireUp(new FireUp(this), Owner, play);
    
    protected override void OnUpgrade() {}
        //DynamicVars[ChargeCostVar.DefaultName].UpgradeValueBy(2M);
}