using Hellkite.HellkiteCode.Fire_Up;
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
        new ChargeCostVar("RequiredCharge", 15)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CreatureCmd.Heal(Owner.Creature, DynamicVars.Heal.BaseValue);

        if (ChargeHandler.GetCharge(Owner.Creature) >= 15)
        {
            await CreatureCmd.Heal(Owner.Creature, DynamicVars["BonusHeal"].BaseValue);
            await ChargeHandler.LoseCharge(Owner.Creature, DynamicVars["RequiredCharge"].BaseValue);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Heal"].UpgradeValueBy(3M);
    }
}