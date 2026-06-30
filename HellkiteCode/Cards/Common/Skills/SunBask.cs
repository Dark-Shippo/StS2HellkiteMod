using BaseLib.Extensions;
using Hellkite.HellkiteCode.Commands;
using Hellkite.HellkiteCode.DynamicVars;
using Hellkite.HellkiteCode.Fire_Up;
using Hellkite.HellkiteCode.Powers;
using Hellkite.HellkiteCode.Structs;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Hellkite.HellkiteCode.Cards.Common.Skills;

public sealed class SunBask() : HellkiteCard(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        //new ChargeCostVar(2), 
        //new ChargeCostVar("NextCharge", 1)
        new FireUpVar(1).WithUpgrade(1)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        //await ChargeHandler.GainCharge(Owner.Creature, DynamicVars[ChargeCostVar.DefaultName].IntValue, choiceContext); 
        int amount = DynamicVars[FireUpVar.defaultName].IntValue;
        await HellkitePlayerCmd.GainFireUp(new FireUp(amount), Owner, play);
        await PowerCmd.Apply<FireUpNextTurnPower>(choiceContext, Owner.Creature, amount + 3, Owner.Creature, this);
    }
    
    protected override void OnUpgrade()
    {
    }
}