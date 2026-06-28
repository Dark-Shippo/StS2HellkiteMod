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

namespace Hellkite.HellkiteCode.Cards.Multiplayer;

public sealed class SharedPyre() : HellkiteCard(1, CardType.Skill, CardRarity.Rare, TargetType.AllAllies)
{
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<VigorPower>(3M), 
        //new ChargeCostVar(3)
        new FireUpVar(3)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await HellkitePlayerCmd.GainFireUp(new FireUp(this), Owner, play);
        await PowerCmd.Apply<VigorPower>(choiceContext, Owner.Creature, DynamicVars[nameof(VigorPower)].BaseValue, Owner.Creature, this);
        //await ChargeHandler.GainCharge(Owner.Creature, DynamicVars[ChargeCostVar.DefaultName].IntValue, choiceContext);
    }
    
    protected override void OnUpgrade() => RemoveKeyword(CardKeyword.Exhaust);
}