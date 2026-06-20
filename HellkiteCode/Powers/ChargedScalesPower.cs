using Hellkite.HellkiteCode.Fire_Up;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Hellkite.HellkiteCode.Powers;

public sealed class ChargedScalesPower : HellkitePower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    public override async Task AfterCardPlayed(
        PlayerChoiceContext choiceContext,
        CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != Owner.Player)
            return;

        if (!cardPlay.IsLastInSeries)
            return;

        await DealDamageToAllEnemies(choiceContext);
    }

    public async Task AfterChargeGained(
        PlayerChoiceContext choiceContext,
        int amount,
        Player gainer)
    {
        if (amount <= 0)
            return;

        if (gainer != Owner.Player)
            return;

        await DealDamageToAllEnemies(choiceContext);
    }

    public async Task AfterChargeSpent(
        PlayerChoiceContext choiceContext,
        decimal amount,
        Player spender)
    {
        if (amount <= 0)
            return;

        if (spender != Owner.Player)
            return;

        await DealDamageToAllEnemies(choiceContext);
    }

    private async Task DealDamageToAllEnemies(
        PlayerChoiceContext choiceContext)
    {
        if (Amount <= 0)
            return;

        if (CombatState.HittableEnemies.Count == 0)
            return;

        Flash();

        await HellkiteCmd.DamageAllEnemies(
            choiceContext,
            CombatState,
            Owner,
            Amount);
    }
}