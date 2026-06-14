using Hellkite.HellkiteCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Hellkite.HellkiteCode.Cards.Common.Skills;

public sealed class Kindling() : HellkiteCard(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => 
        [
            CardKeyword.Exhaust
        ];
    protected override IEnumerable<DynamicVar> CanonicalVars => 
        [
            new PowerVar<KindlePower>(1M)
        ];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play) => 
        await PowerCmd.Apply<KindlePower>(Owner.Creature, DynamicVars[nameof(KindlePower)].BaseValue, Owner.Creature, this);
    
    protected override void OnUpgrade() => DynamicVars[nameof(KindlePower)].UpgradeValueBy(1M);
}