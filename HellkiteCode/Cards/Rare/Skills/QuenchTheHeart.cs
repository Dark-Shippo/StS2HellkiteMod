using Hellkite.HellkiteCode.Extensions;
using Hellkite.HellkiteCode.Structs;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Hellkite.HellkiteCode.Cards.Rare.Skills;

public sealed class QuenchTheHeart() : HellkiteCard(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    protected override HashSet<CardTag> CanonicalTags => [CardTag.Defend];

    // Block == Charge spent, Plating == half of it. The custom vars below preview from the
    // current Charge so the card shows the real numbers before it's played.
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new ChargeBlockVar(),
        new HalfChargePlatingVar(),
        new CardsVar(1)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        // Lose all Charge; Block == amount spent, Plating == floor(half).
        var pool = Owner.PlayerCombatState?.GetFireUp() ?? new FireUp();
        var spent = pool.Total;
        await SpendFireUp(pool);

        if (spent > 0)
        {
            DynamicVars.Block._baseValue = spent;
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, play);
            await PowerCmd.Apply<PlatingPower>(choiceContext, Owner.Creature, spent / 2, Owner.Creature, this);
        }

        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.IntValue, Owner);
    }

    protected override void OnUpgrade() => DynamicVars.Cards.UpgradeValueBy(1M);

    private static int CurrentCharge(CardModel card)
        => card.Owner?.PlayerCombatState?.GetFireUp().Total ?? 0;

    // Block preview = current Charge.
    private sealed class ChargeBlockVar() : BlockVar(0M, ValueProp.Move)
    {
        public override void UpdateCardPreview(CardModel card, CardPreviewMode previewMode, Creature? target,
            bool runGlobalHooks)
            => PreviewValue = CurrentCharge(card);
    }

    // Plating preview (named "PlatingPower" so the loc placeholder resolves) = half of Charge.
    private sealed class HalfChargePlatingVar() : DynamicVar(nameof(PlatingPower), 0M)
    {
        public override void UpdateCardPreview(CardModel card, CardPreviewMode previewMode, Creature? target,
            bool runGlobalHooks)
            => PreviewValue = CurrentCharge(card) / 2;
    }
}
