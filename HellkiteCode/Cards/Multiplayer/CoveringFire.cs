using Hellkite.HellkiteCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Hellkite.HellkiteCode.Cards.Multiplayer;

public sealed class CoveringFire() : HellkiteCard(2, CardType.Power, CardRarity.Rare, TargetType.Self)
{
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<CoveringFirePower>(1M)];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play) => 
        await PowerCmd.Apply<CoveringFirePower>(Owner.Creature, 
            DynamicVars[nameof(CoveringFirePower)].BaseValue, Owner.Creature, this);
    
    protected override void OnUpgrade() => DynamicVars[nameof(CoveringFirePower)].UpgradeValueBy(1M);
}