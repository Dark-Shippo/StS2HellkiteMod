using Hellkite.HellkiteCode.Fire_Up;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Hellkite.HellkiteCode.Cards.Uncommon.Skills;

public sealed class CastOff() : HellkiteCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override HashSet<CardTag> CanonicalTags => [CardTag.Defend];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new BlockVar(6M, ValueProp.Move),
        new PowerVar<PlatingPower>(4M),
        new ChargeCostVar(1M)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await PowerCmd.Apply<PlatingPower>(
            choiceContext, 
            Owner.Creature,
            DynamicVars[nameof(PlatingPower)].BaseValue,
            Owner.Creature,
            this);

        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, play);
        await ChargeHandler.GainCharge(
            Owner.Creature,
            DynamicVars[nameof(ChargeCostVar)].BaseValue, choiceContext);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}