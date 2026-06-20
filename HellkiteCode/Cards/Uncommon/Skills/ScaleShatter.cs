using Hellkite.HellkiteCode.Fire_Up;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Hellkite.HellkiteCode.Cards.Uncommon.Skills;

public sealed class ScaleShatter() : HellkiteCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        int lost = await HellkiteCmd.RemoveAllRazorScales(Owner.Creature,choiceContext); 
        if (lost > 0) await HellkiteCmd.AttackAll(choiceContext, this, lost);
    }
    protected override void OnUpgrade() => EnergyCost.UpgradeBy(-1);
}