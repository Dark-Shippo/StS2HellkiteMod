using Hellkite.HellkiteCode.Commands;
using Hellkite.HellkiteCode.Extensions;
using Hellkite.HellkiteCode.Structs;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Hellkite.HellkiteCode.Cards.Rare.Attacks;

public sealed class WorldBreaker() : HellkiteCard(2, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies)
{
    // Damage is delivered as ONE consolidated hit scaled by all Charge spent, so the
    // DamageVar's base value is the per-Charge damage and the preview multiplies it by
    // the current Charge.
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new ChargeDamageVar(2M)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        var pool = Owner.PlayerCombatState?.GetFireUp() ?? new FireUp();
        var spent = pool.Total;
        await SpendFireUp(pool);

        if (spent <= 0)
            return;

        // Count all Charge spent, then multiply the per-Charge damage into a single hit
        var totalDamage = DynamicVars.Damage.BaseValue * spent;
        await HellkiteCmd.AttackAll(choiceContext, this, totalDamage);

        if (CombatState != null)
            await HellkiteCmd.ApplyScorchAll(CombatState, spent, Owner.Creature, this, choiceContext);
    }

    protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(1M);

    private static int CurrentCharge(CardModel card)
        => card.Owner?.PlayerCombatState?.GetFireUp().Total ?? 0;

    // Damage preview = per-Charge damage × current Charge.
    private sealed class ChargeDamageVar(decimal perCharge) : DamageVar(perCharge, ValueProp.Move)
    {
        public override void UpdateCardPreview(CardModel card, CardPreviewMode previewMode, Creature? target,
            bool runGlobalHooks)
            => PreviewValue = BaseValue * CurrentCharge(card);
    }
}
