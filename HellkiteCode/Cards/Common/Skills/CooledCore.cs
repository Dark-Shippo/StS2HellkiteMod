using BaseLib.Extensions;
using Hellkite.HellkiteCode.DynamicVars;
using Hellkite.HellkiteCode.Fire_Up;
using Hellkite.HellkiteCode.Structs;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Hellkite.HellkiteCode.Cards.Common.Skills;

public sealed class CooledCore() : HellkiteCard(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        //new ChargeCostVar("MaxCharge", 3),
        new FireUpVar(3).WithUpgrade(1) ,
        new CardsVar(1)
    ];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        int spent = await SpendUpToX();
        //decimal spent = await HellkiteCmd.SpendUpToCharge(Owner.Creature, (int)DynamicVars["MaxCharge"].BaseValue, choiceContext);
        if (spent > 0) await PowerCmd.Apply<PlatingPower>(choiceContext, Owner.Creature, spent, Owner.Creature, this);
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.IntValue, Owner);
    }

    protected override void OnUpgrade()
    {
        //DynamicVars["MaxCharge"].UpgradeValueBy(1M); 
        DynamicVars.Cards.UpgradeValueBy(1M);
    }
}