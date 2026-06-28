using BaseLib.Extensions;
using Hellkite.HellkiteCode.Commands;
using Hellkite.HellkiteCode.DynamicVars;
using Hellkite.HellkiteCode.Fire_Up;
using Hellkite.HellkiteCode.Structs;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Hellkite.HellkiteCode.Cards.Common.Skills;

public sealed class BreatheDeep() : HellkiteCard(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    protected override HashSet<CardTag> CanonicalTags => [CardTag.Defend];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new BlockVar(7M, ValueProp.Move), 
        new FireUpVar(1).WithUpgrade(1)
        //new ChargeCostVar(1)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await HellkitePlayerCmd.GainFireUp(new FireUp(this), Owner, play);
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, play); 
        //await ChargeHandler.GainCharge(Owner.Creature, DynamicVars[ChargeCostVar.DefaultName].IntValue, choiceContext);
    }
    
    protected override void OnUpgrade() => DynamicVars.Block.UpgradeValueBy(3M);
}