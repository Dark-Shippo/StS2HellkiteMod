using Hellkite.HellkiteCode.Commands;
using Hellkite.HellkiteCode.Fire_Up;
using Hellkite.HellkiteCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Hellkite.HellkiteCode.Cards.Uncommon.Skills;

public sealed class CatchFire() : HellkiteCard(2, CardType.Skill, CardRarity.Uncommon, TargetType.RandomEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<ScorchPower>(3M), 
        new RepeatVar(3)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        for (int i = 0; i < DynamicVars.Repeat.IntValue; i++)
        {
            var target = HellkiteCmd.RandomEnemy(Owner.Creature); 
            if (target == null) break; 
            await HellkiteCmd.ApplyScorch(target, DynamicVars[nameof(ScorchPower)].BaseValue, Owner.Creature, this, choiceContext);
        }
    }
    protected override void OnUpgrade() => DynamicVars.Repeat.UpgradeValueBy(1M);
}