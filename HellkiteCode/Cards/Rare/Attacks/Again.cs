using Hellkite.HellkiteCode.Fire_Up;
using Hellkite.HellkiteCode.Structs;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Hellkite.HellkiteCode.Cards.Rare.Attacks;

public sealed class Again() : HellkiteCard(0, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(20M, ValueProp.Move), 
        new RepeatVar(2),
        //new ChargeCostVar(6)
    ];

    public override FireUp CanonicalFireUpCost => new(6);

    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Exhaust
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (play.Target != null)
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(this)
                .Targeting(play.Target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);

        // Pick the next Attack in hand (not this card) and auto-play it Repeat times.
        // AutoPlay is free (it does not pay the card's energy/star cost), and we pass the
        // same target so each replay hits the intended enemy instead of a random one.
        CardSelectorPrefs prefs = new(SelectionScreenPrompt, 1)
        {
            PretendCardsCanBePlayed = true
        };
        CardModel? card = (await CardSelectCmd.FromHand(choiceContext, Owner, prefs,
            c => c != this && c.Type == CardType.Attack && !c.Keywords.Contains(CardKeyword.Unplayable),
            this)).FirstOrDefault();
        if (card == null)
            return;

        for (int i = 0; i < DynamicVars.Repeat.IntValue; ++i)
            await CardCmd.AutoPlay(choiceContext, card, play.Target);
    }
    
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(6M); 
        DynamicVars.Repeat.UpgradeValueBy(1M);
    }
}