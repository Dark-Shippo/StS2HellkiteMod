using Hellkite.HellkiteCode.Fire_Up;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Hellkite.HellkiteCode.Cards.Uncommon.Attacks;

public sealed class BurnAway() : HellkiteCard(0, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(3M, ValueProp.Move)];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (play.Target != null)
            await HellkiteCmd.AttackTarget
            (
                choiceContext,
                this,
                play.Target,
                DynamicVars.Damage.BaseValue * ChargeHandler.GetChargeGainedThisTurn(Owner.Creature)
            );
    }

    protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(1M);
}