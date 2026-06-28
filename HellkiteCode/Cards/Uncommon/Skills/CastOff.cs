using BaseLib.Extensions;
using Hellkite.HellkiteCode.Commands;
using Hellkite.HellkiteCode.DynamicVars;
using Hellkite.HellkiteCode.Fire_Up;
using Hellkite.HellkiteCode.Structs;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Hellkite.HellkiteCode.Cards.Uncommon.Skills;

public sealed class CastOff() : HellkiteCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override HashSet<CardTag> CanonicalTags => [CardTag.Defend];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new BlockVar(6M, ValueProp.Move),
        new PowerVar<PlatingPower>(4M),
        //new ChargeCostVar(1)
        new FireUpVar(1).WithUpgrade(1)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await PowerCmd.Apply<PlatingPower>(
            choiceContext, 
            Owner.Creature,
            DynamicVars[nameof(PlatingPower)].BaseValue,
            Owner.Creature,
            this);

        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, play);
        await HellkitePlayerCmd.GainFireUp(new FireUp(this), Owner, play);
        //await ChargeHandler.GainCharge(Owner.Creature,DynamicVars[nameof(ChargeCostVar)].IntValue, choiceContext);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}