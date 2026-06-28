using Hellkite.HellkiteCode.Fire_Up;
using Hellkite.HellkiteCode.Structs;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Hellkite.HellkiteCode.Cards.Common.Skills;

public sealed class TemperedHide() : HellkiteCard(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<PlatingPower>(4M), 
        //new ChargeCostVar(1)
        ];

    public override FireUp CanonicalFireUpCost => new(1);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        //await ChargeHandler.LoseCharge(Owner.Creature, DynamicVars[ChargeCostVar.DefaultName].IntValue, choiceContext);
        await PowerCmd.Apply<PlatingPower>(choiceContext, Owner.Creature, DynamicVars[nameof(PlatingPower)].BaseValue, Owner.Creature, this);
    }
    protected override void OnUpgrade() => DynamicVars[nameof(PlatingPower)].UpgradeValueBy(2M);
}