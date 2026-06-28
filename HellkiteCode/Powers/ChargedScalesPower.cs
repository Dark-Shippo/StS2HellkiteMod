using Hellkite.HellkiteCode.Commands;
using Hellkite.HellkiteCode.Fire_Up;
using Hellkite.HellkiteCode.Hooks;
using Hellkite.HellkiteCode.Structs;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Hellkite.HellkiteCode.Powers;

public sealed class ChargedScalesPower : HellkitePower, IAfterFireUpGained, IAfterFireUpSpent
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override object InitInternalData() => new Data();

    public override async Task AfterCardPlayed(
        PlayerChoiceContext choiceContext,
        CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != Owner.Player)
            return;

        await FlushPendingHits(choiceContext);

        if (!cardPlay.IsLastInSeries)
            return;

        await DealDamageToAllEnemies(choiceContext);
    }

    // Gain hook — no context, so record and flush later.
    public Task AfterFireUpGained(
        ICombatState combatState,
        FireUp amount,
        Player player,
        CardPlay? cardPlay)
    {
        if (amount.Total <= 0)
            return Task.CompletedTask;

        if (player != Owner.Player)
            return Task.CompletedTask;

        GetInternalData<Data>().pendingHits++;
        return Task.CompletedTask;
    }

    // Spend hook — no context, so record and flush later.
    public Task AfterFireUpSpent(FireUp amount, Player spender)
    {
        if (amount.Total <= 0)
            return Task.CompletedTask;

        if (spender != Owner.Player)
            return Task.CompletedTask;

        GetInternalData<Data>().pendingHits++;
        return Task.CompletedTask;
    }

    private async Task FlushPendingHits(PlayerChoiceContext choiceContext)
    {
        Data data = GetInternalData<Data>();

        while (data.pendingHits > 0)
        {
            data.pendingHits--;
            await DealDamageToAllEnemies(choiceContext);
        }
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

    private sealed class Data
    {
        public int pendingHits;
    }
}