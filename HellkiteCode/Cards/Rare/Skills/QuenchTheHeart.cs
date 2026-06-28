using Hellkite.HellkiteCode.Extensions;
using Hellkite.HellkiteCode.Structs;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Hellkite.HellkiteCode.Cards.Rare.Skills;

public sealed class QuenchTheHeart() : HellkiteCard(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    protected override HashSet<CardTag> CanonicalTags => [CardTag.Defend];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        // Block is computed dynamically at play time (= Charge spent), but the var must
        // exist so DynamicVars.Block resolves and the {Block} loc placeholder renders.
        new BlockVar(0, ValueProp.Move),
        new CardsVar(1)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        // Lose all FireUp; capture the amount. Routing through SpendFireUp fires
        // AfterFireUpSpent and records LastFireUpSpent.
        var pool = Owner.PlayerCombatState?.GetFireUp() ?? new FireUp();
        var spent = pool.Total;
        await SpendFireUp(pool);

        // Block == spent, Plating == floor(spent / 2).
        DynamicVars.Block._baseValue = spent;
        if (spent > 0)
        {
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, play);
            await PowerCmd.Apply<PlatingPower>(choiceContext, Owner.Creature, spent / 2, Owner.Creature, this);
        }

        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.IntValue, Owner);
    }

    protected override void OnUpgrade() => DynamicVars.Cards.UpgradeValueBy(1M);
}