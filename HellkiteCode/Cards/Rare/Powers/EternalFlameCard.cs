using Hellkite.HellkiteCode.Fire_Up;
using Hellkite.HellkiteCode.Powers;
using Hellkite.HellkiteCode.Structs;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Hellkite.HellkiteCode.Cards.Rare.Powers;

public sealed class EternalFlameCard() : HellkiteCard(1, CardType.Power, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new("Power", 2M),
        //new ChargeCostVar(3)
    ];

    public override FireUp CanonicalFireUpCost => new(3);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        //await ChargeHandler.LoseCharge(Owner.Creature, DynamicVars[ChargeCostVar.DefaultName].IntValue, choiceContext);
        await PowerCmd.Apply<EternalFlamePower>(choiceContext, Owner.Creature, DynamicVars["Power"].BaseValue, Owner.Creature, this);
    }
    
    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Innate);
        DynamicVars["Power"].UpgradeValueBy(1M);
    }
}