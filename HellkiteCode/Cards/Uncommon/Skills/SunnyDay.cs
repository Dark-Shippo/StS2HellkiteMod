using Hellkite.HellkiteCode.Fire_Up;
using Hellkite.HellkiteCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Hellkite.HellkiteCode.Cards.Uncommon.Skills;

public sealed class SunnyDay() : HellkiteCard(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<ScorchPower>(4M)];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (CombatState != null)
            await HellkiteCmd.ApplyScorchAll(CombatState, DynamicVars[nameof(ScorchPower)].BaseValue, Owner.Creature,
                this);
    }

    protected override void OnUpgrade() => DynamicVars[nameof(ScorchPower)].UpgradeValueBy(2M);
}