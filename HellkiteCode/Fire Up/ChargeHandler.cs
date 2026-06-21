using System.Runtime.CompilerServices;
using Hellkite.HellkiteCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Hellkite.HellkiteCode.Fire_Up;

public abstract class ChargeHandler
{
    private sealed class ChargeState(decimal chargeSpentThisTurn)
    {
        public decimal CurrentCharge;
        public decimal ChargeGainedThisTurn;
        public decimal ChargeSpentThisTurn = chargeSpentThisTurn;
        public bool Threshold1Reached;
        public bool Threshold2Reached;
    }

    private static readonly ConditionalWeakTable<Creature, ChargeState> States = new();

    private static ChargeState GetState(Creature creature) => States.GetOrCreateValue(creature);

    public static decimal GetCharge(Creature creature) => GetState(creature).CurrentCharge;
    public static decimal GetChargeGainedThisTurn(Creature creature) => GetState(creature).ChargeGainedThisTurn;
    public static decimal GetChargeSpentThisTurn(Creature creature) => GetState(creature).ChargeSpentThisTurn;
    public static bool HasSpentChargeThisTurn(Creature creature) => GetChargeSpentThisTurn(creature) > 0;

    public static async Task GainCharge(Creature creature, decimal amount, PlayerChoiceContext choiceContext)
    {
        if (amount <= 0) return;

        var state = GetState(creature);
        decimal oldCharge = state.CurrentCharge;
        state.CurrentCharge = Math.Min(30, state.CurrentCharge + (int)amount);
        decimal gained = state.CurrentCharge - oldCharge;
        if (gained <= 0) return;

        state.ChargeGainedThisTurn += gained;

        Player? player = creature.Player; if (player != null) 
        { 
            ChargedScalesPower? chargedScales = creature.GetPower<ChargedScalesPower>();
            if (chargedScales != null)
            {
                await chargedScales.AfterChargeGained( choiceContext, gained, player);
            } 
        }

        if (state is { Threshold1Reached: false, CurrentCharge: >= 11 })
        {
            state.Threshold1Reached = true;
            await PowerCmd.Apply<KindlePower>(choiceContext, creature, 1M, creature, null);
        }

        if (state is { Threshold2Reached: false, CurrentCharge: >= 21 })
        {
            state.Threshold2Reached = true;
            await PowerCmd.Apply<KindlePower>(choiceContext, creature, 1M, creature, null);
        }

        if (state.CurrentCharge >= 30)
        {
            await PowerCmd.Apply<OverChargePower>(choiceContext, creature, 1M, creature, null);
            state.CurrentCharge = 0;
            state.Threshold1Reached = false;
            state.Threshold2Reached = false;
        }
    }

    public static async Task LoseCharge(Creature creature, decimal amount, PlayerChoiceContext choiceContext)
    {
        Player? player = creature.Player;
        ChargeState state = GetState(creature);
        if (player != null)
        {
            ChargedScalesPower? chargedScales = creature.GetPower<ChargedScalesPower>();
            if (chargedScales != null)
            {
                await chargedScales.AfterChargeSpent(choiceContext, amount, player);
            }

            TemperPower? temper = creature.GetPower<TemperPower>();
            if (temper != null)
            {
                await temper.AfterChargeSpent(choiceContext, amount, player);
            }
            state.CurrentCharge = Math.Max(0, state.CurrentCharge - (int)amount);
        }
    }

    public static async Task<bool> TrySpendCharge(Creature creature, decimal amount, PlayerChoiceContext choiceContext)
    {
        if (GetCharge(creature) < amount) return false; 
        await LoseCharge( creature, amount, choiceContext); return true;
    }

    public static async Task<decimal> SpendAllCharge(Creature creature, PlayerChoiceContext choiceContext)
    {
        decimal current = GetCharge(creature); if (current > 0) { await LoseCharge( creature, current, choiceContext); } 
        return current;
    }
}