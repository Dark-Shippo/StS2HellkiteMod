using Hellkite.HellkiteCode.Extensions;
using Hellkite.HellkiteCode.Fire_Up;
using Hellkite.HellkiteCode.Structs;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Hellkite.HellkiteCode.Cards.Rare.Skills;

public sealed class NightsRest() : HellkiteCard(2, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    public override bool CanBeGeneratedInCombat => false;
    
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Exhaust
    ];
    
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new HealVar(10M),
        new HealVar("BonusHeal", 5M),
        //new ChargeCostVar(15)
    ];

    public override FireUp CanonicalFireUpCost => new(15);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CreatureCmd.Heal(Owner.Creature, DynamicVars.Heal.BaseValue);

        var fireUp = Owner.PlayerCombatState?.GetFireUp() ?? new FireUp();
        var bonusCost = new FireUp(15);
        
        if (fireUp.CanSpend(bonusCost))
        {
            await CreatureCmd.Heal(Owner.Creature, DynamicVars["BonusHeal"].BaseValue);
            await SpendFireUp(bonusCost);
            //await ChargeHandler.LoseCharge(Owner.Creature, DynamicVars[ChargeCostVar.DefaultName].IntValue, choiceContext);
        }
    }
    
    protected override void OnUpgrade()
    {
        DynamicVars["Heal"].UpgradeValueBy(3M);
    }
}