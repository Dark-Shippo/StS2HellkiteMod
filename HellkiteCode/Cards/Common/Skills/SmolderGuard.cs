using Hellkite.HellkiteCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Hellkite.HellkiteCode.Cards.Common.Skills;

public sealed class SmolderGuard() : HellkiteCard(2, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    protected override HashSet<CardTag> CanonicalTags => [CardTag.Defend];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => 
        [
            new BlockVar(6M, ValueProp.Move), 
            new PowerVar<KindlePower>(1M)
        ];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, play); 
        if (Owner.Creature.GetPowerAmount<RazorScalesPower>() > 0 || Owner.Creature.GetPowerAmount<PlatingPower>() > 0) 
            await PowerCmd.Apply<KindlePower>(choiceContext, Owner.Creature, DynamicVars[nameof(KindlePower)].BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(2M); 
        DynamicVars[nameof(KindlePower)].UpgradeValueBy(1M);
    }
}