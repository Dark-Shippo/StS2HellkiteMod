using Hellkite.HellkiteCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Hellkite.HellkiteCode.Cards.Uncommon.Skills;

public sealed class BurningReprisalCard() : HellkiteCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    
    protected override HashSet<CardTag> CanonicalTags => [CardTag.Defend];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => 
        [
            new BlockVar(8M, ValueProp.Move),
            new PowerVar<RazorScalesPower>(2M),
            new PowerVar<ScorchPower>(2M),
            new("Power", 2M)
        ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, play);
        await PowerCmd.Apply<RazorScalesPower>(choiceContext, Owner.Creature, DynamicVars[nameof(RazorScalesPower)].BaseValue, Owner.Creature, this);
        await PowerCmd.Apply<BurningReprisalPower>(
            choiceContext, 
            Owner.Creature,
            DynamicVars["Power"].BaseValue,
            Owner.Creature,
            this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(3M);
        DynamicVars[nameof(RazorScalesPower)].UpgradeValueBy(1M);
    }
}