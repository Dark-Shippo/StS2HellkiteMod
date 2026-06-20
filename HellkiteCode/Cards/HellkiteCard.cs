using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using Hellkite.HellkiteCode.Character;
using Hellkite.HellkiteCode.Extensions;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Hellkite.HellkiteCode.Cards;

[Pool(typeof(HellkiteCardPool))]
public abstract class HellkiteCard(int cost, CardType type, CardRarity rarity, TargetType target) : CustomCardModel(cost, type, rarity, target)
{
    protected bool FirstAttack = true;
    
    public override Task AfterCardPlayed(PlayerChoiceContext playerChoiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Type == CardType.Attack) FirstAttack = false;
        return Task.CompletedTask;
    }
    
    // Need a different way to block cards from being played if they don't have enough charge.'
    //protected override bool IsPlayable =>
    //    base.IsPlayable && (DynamicVars[ChargeCostVar.DefaultName].BaseValue < 0 ||
    //                        ChargeHandler.GetCharge(Owner.Creature) >=
    //                        DynamicVars[ChargeCostVar.DefaultName].BaseValue);
//
    //public override bool ShouldGlowRedInternal =>
    //    base.IsPlayable && (DynamicVars[ChargeCostVar.DefaultName].BaseValue < 0 ||
    //                        ChargeHandler.GetCharge(Owner.Creature) >=
    //                        DynamicVars[ChargeCostVar.DefaultName].BaseValue);
    
    //Image size:
    //Normal art: 1000x760 (Using 500x380 should also work, it will simply be scaled.)
    //Full art: 606x852
    public override string CustomPortraitPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigCardImagePath();

    //Smaller variants of card images for efficiency:
    //Smaller variant of full art: 250x350
    //Smaller variant of normal art: 250x190

    //Uses card_portraits/card_name.png as an image path. These should be smaller images.
    public override string PortraitPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();
}