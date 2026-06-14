using Hellkite.HellkiteCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Hellkite.HellkiteCode.Cards.Rare.Powers;

public sealed class EternalFlameCard() : HellkiteCard(1, CardType.Power, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<EternalFlamePower>(4M),
        new PowerVar<ScorchPower>(4M),
        new ChargeCostVar(3M)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await ChargeHandler.LoseCharge(Owner.Creature, DynamicVars[nameof(ChargeCostVar)].BaseValue);
        await PowerCmd.Apply<EternalFlamePower>(Owner.Creature, DynamicVars[nameof(EternalFlamePower)].BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Innate);
        DynamicVars[nameof(ScorchPower)].UpgradeValueBy(1M);
    }
}