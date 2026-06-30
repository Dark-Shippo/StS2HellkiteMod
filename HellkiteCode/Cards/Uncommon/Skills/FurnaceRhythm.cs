using BaseLib.Extensions;
using Hellkite.HellkiteCode.Commands;
using Hellkite.HellkiteCode.DynamicVars;
using Hellkite.HellkiteCode.Fire_Up;
using Hellkite.HellkiteCode.Structs;
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
        //new ChargeCostVar(3), 
        new CardsVar(2),
        new FireUpVar(3).WithUpgrade(1)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await HellkitePlayerCmd.GainFireUp(new FireUp(this), Owner, play);
        // Draw first, then choose a card to discard (matches "Draw X. Discard 1.").
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.IntValue, Owner);
        CardModel? card = (
            await CardSelectCmd.FromHandForDiscard(choiceContext, Owner, new CardSelectorPrefs(CardSelectorPrefs.DiscardSelectionPrompt,
                1), null, this)).FirstOrDefault();
        if (card == null)
            return;
        await CardCmd.Discard(choiceContext, card);
    }
    
    protected override void OnUpgrade() => AddKeyword(CardKeyword.Innate);
}