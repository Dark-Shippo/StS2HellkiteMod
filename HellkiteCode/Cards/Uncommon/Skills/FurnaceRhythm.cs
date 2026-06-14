using Hellkite.HellkiteCode.Fire_Up;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Hellkite.HellkiteCode.Cards.Uncommon.Skills;

public sealed class FurnaceRhythm() : HellkiteCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new ChargeCostVar(3M), 
        new CardsVar(2)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await ChargeHandler.GainCharge(Owner.Creature, DynamicVars[ChargeCostVar.DefaultName].BaseValue); 
        CardModel? card = (
            await CardSelectCmd.FromHandForDiscard(choiceContext, Owner, new CardSelectorPrefs(CardSelectorPrefs.DiscardSelectionPrompt,
                1), null, this)).FirstOrDefault();
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.IntValue, Owner);
        if (card == null)
            return;
        await CardCmd.Discard(choiceContext, card);
    }
    
    protected override void OnUpgrade() => AddKeyword(CardKeyword.Innate);
}