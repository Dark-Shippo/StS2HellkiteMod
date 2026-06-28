using BaseLib.Extensions;
using Hellkite.HellkiteCode.Commands;
using Hellkite.HellkiteCode.DynamicVars;
using Hellkite.HellkiteCode.Fire_Up;
using Hellkite.HellkiteCode.Structs;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Hellkite.HellkiteCode.Cards.Uncommon.Skills;

public sealed class EmberDraft() : HellkiteCard(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        //new ChargeCostVar(2),
        new CardsVar(1),
        new FireUpVar(2).WithUpgrade(1)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await HellkitePlayerCmd.GainFireUp(new FireUp(this), Owner, play);
        //await ChargeHandler.GainCharge(Owner.Creature, DynamicVars[ChargeCostVar.DefaultName].IntValue, choiceContext); 
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.IntValue, Owner);
    }

    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Retain);
        DynamicVars.Cards.UpgradeValueBy(1M);
    }
}