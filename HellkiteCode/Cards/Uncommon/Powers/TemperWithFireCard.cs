using Hellkite.HellkiteCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Hellkite.HellkiteCode.Cards.Uncommon.Powers;

public sealed class TemperWithFireCard() : HellkiteCard(2, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<PlatingPower>(4M), 
        new PowerVar<KindlePower>(1M)
    ];

    protected override Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        try
        {
            Owner.Creature.GetPowerAmount<PlatingPower>();
            Owner.Creature.GetPowerAmount<KindlePower>();
            return Task.CompletedTask;
        }
        catch (Exception exception)
        {
            return Task.FromException(exception);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars[nameof(PlatingPower)].UpgradeValueBy(2M);
    }
}