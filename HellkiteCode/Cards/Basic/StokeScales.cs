using BaseLib.Extensions;
using BaseLib.Utils;
using Hellkite.HellkiteCode.Commands;
using Hellkite.HellkiteCode.DynamicVars;
using Hellkite.HellkiteCode.Fire_Up;
using Hellkite.HellkiteCode.Structs;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Hellkite.HellkiteCode.Cards.Basic;

public sealed class StokeScales() : HellkiteCard(1, 
    CardType.Skill, CardRarity.Basic, 
    TargetType.Self)
{
    public override bool GainsBlock => true;
    
    protected override HashSet<CardTag> CanonicalTags => [CardTag.Defend];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => 
        [
            new BlockVar(5M, ValueProp.Move).WithUpgrade(3M),
            //new ChargeCostVar("Charge", 1)
            new FireUpVar(1).WithUpgrade(1)
        ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, play);
        //await ChargeHandler.GainCharge(Owner.Creature, DynamicVars[ChargeCostVar.DefaultName].IntValue, choiceContext);
        await HellkitePlayerCmd.GainFireUp(new FireUp(this), Owner, play);
    }

    protected override void OnUpgrade()
    {
        //DynamicVars.Block.UpgradeValueBy(3M);
    }
}