using Hellkite.HellkiteCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Hellkite.HellkiteCode.Cards.Uncommon.Skills;

public sealed class ConserveCard() : HellkiteCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => 
        [
            new EnergyVar(1),
            new PowerVar<FireUpNextTurnPower>(1M)
        ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await PowerCmd.Apply<EnergyNextTurnPower>(choiceContext, Owner.Creature, DynamicVars.Energy.BaseValue, Owner.Creature, this);
        await PowerCmd.Apply<FireUpNextTurnPower>(choiceContext, Owner.Creature, DynamicVars[nameof(FireUpNextTurnPower)].BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars[nameof(FireUpNextTurnPower)].UpgradeValueBy(1M);
    }
}