using Hellkite.HellkiteCode.Commands;
using Hellkite.HellkiteCode.Fire_Up;
using Hellkite.HellkiteCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Hellkite.HellkiteCode.Cards.Uncommon.Skills;

public sealed class ThirdDegree() : HellkiteCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<ScorchPower>(6M), 
        new CardsVar(1)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (play.Target != null && play.Target.GetPowerAmount<ScorchPower>() <= 0) return;
        if (play.Target != null)
            await HellkiteCmd.ApplyScorch(play.Target, DynamicVars[nameof(ScorchPower)].BaseValue, Owner.Creature,
                this, choiceContext);
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.IntValue, Owner);
    }
    
    protected override void OnUpgrade() => DynamicVars[nameof(ScorchPower)].UpgradeValueBy(2M);
}