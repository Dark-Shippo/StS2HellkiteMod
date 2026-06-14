using Hellkite.HellkiteCode.Fire_Up;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Hellkite.HellkiteCode.Cards.Rare.Skills;

public sealed class QuenchTheHeart() : HellkiteCard(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    protected override HashSet<CardTag> CanonicalTags => [CardTag.Defend];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new BlockVar(30M, ValueProp.Move)
    ];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        int spent = await ChargeHandler.SpendAllCharge(Owner.Creature);
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, play);
        if (spent > 0) await PowerCmd.Apply<PlatingPower>(Owner.Creature, Math.Floor(spent / 2M), Owner.Creature, this);
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.IntValue, Owner);
    }
    protected override void OnUpgrade() => DynamicVars.Cards.UpgradeValueBy(1M);
}