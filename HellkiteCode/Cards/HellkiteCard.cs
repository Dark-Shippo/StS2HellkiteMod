using System.Runtime.CompilerServices;
using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using Hellkite.HellkiteCode.Character;
using Hellkite.HellkiteCode.Extensions;
using Hellkite.HellkiteCode.Fire_Up;
using Hellkite.HellkiteCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Hellkite.HellkiteCode.Cards;

[Pool(typeof(HellkiteCardPool))]
public abstract class HellkiteCard(int cost, CardType type, CardRarity rarity, TargetType target) : CustomCardModel(cost, type, rarity, target)
{
    protected bool FirstAttack = true;
    
    public override Task AfterCardPlayed(PlayerChoiceContext playerChoiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Type == CardType.Attack) FirstAttack = false;
        return Task.CompletedTask;
    }
    
    // Need a different way to block cards from being played if they don't have enough charge.'
    //protected override bool IsPlayable =>
    //    base.IsPlayable && (DynamicVars[ChargeCostVar.DefaultName].BaseValue < 0 ||
    //                        ChargeHandler.GetCharge(Owner.Creature) >=
    //                        DynamicVars[ChargeCostVar.DefaultName].BaseValue);
//
    //public override bool ShouldGlowRedInternal =>
    //    base.IsPlayable && (DynamicVars[ChargeCostVar.DefaultName].BaseValue < 0 ||
    //                        ChargeHandler.GetCharge(Owner.Creature) >=
    //                        DynamicVars[ChargeCostVar.DefaultName].BaseValue);
    
    //Image size:
    //Normal art: 1000x760 (Using 500x380 should also work, it will simply be scaled.)
    //Full art: 606x852
    public override string CustomPortraitPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigCardImagePath();

    //Smaller variants of card images for efficiency:
    //Smaller variant of full art: 250x350
    //Smaller variant of normal art: 250x190

    //Uses card_portraits/card_name.png as an image path. These should be smaller images.
    public override string PortraitPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();
}

public sealed class ChargeCostVar: DynamicVar
{
    public const string DefaultName = "Charge";

    public ChargeCostVar(decimal amount)
        : base(DefaultName, amount)
    {
    }

    public ChargeCostVar(string name, decimal amount)
        : base(name, amount)
    {
    }
}

public abstract class ChargeHandler
{
    private sealed class ChargeState
    {
        public int CurrentCharge;
        public int ChargeGainedThisTurn;
        public int ChargeSpentThisTurn;
        public bool Threshold1Reached;
        public bool Threshold2Reached;
    }

    private static readonly ConditionalWeakTable<Creature, ChargeState> States = new();

    private static ChargeState GetState(Creature creature) => States.GetOrCreateValue(creature);

    public static int GetCharge(Creature creature) => GetState(creature).CurrentCharge;
    public static int GetChargeGainedThisTurn(Creature creature) => GetState(creature).ChargeGainedThisTurn;
    public static int GetChargeSpentThisTurn(Creature creature) => GetState(creature).ChargeSpentThisTurn;
    public static bool HasSpentChargeThisTurn(Creature creature) => GetChargeSpentThisTurn(creature) > 0;

    public static async Task GainCharge(Creature creature, decimal amount)
    {
        if (amount <= 0) return;

        var state = GetState(creature);
        int oldCharge = state.CurrentCharge;
        state.CurrentCharge = Math.Min(30, state.CurrentCharge + (int)amount);
        int gained = state.CurrentCharge - oldCharge;
        if (gained <= 0) return;

        state.ChargeGainedThisTurn += gained;

        var chargedScales = creature.GetPower<ChargedScalesPower>();
        if (chargedScales != null && creature.Player != null)
        {
            await chargedScales.AfterChargeGained(gained, creature.Player);
        }

        if (state is { Threshold1Reached: false, CurrentCharge: >= 11 })
        {
            state.Threshold1Reached = true;
            await PowerCmd.Apply<KindlePower>(creature, 1M, creature, null);
        }

        if (state is { Threshold2Reached: false, CurrentCharge: >= 21 })
        {
            state.Threshold2Reached = true;
            await PowerCmd.Apply<KindlePower>(creature, 1M, creature, null);
        }

        if (state.CurrentCharge >= 30)
        {
            await PowerCmd.Apply<OverChargePower>(creature, 1M, creature, null);
            state.CurrentCharge = 0;
            state.Threshold1Reached = false;
            state.Threshold2Reached = false;
        }
    }

    public static async Task LoseCharge(Creature creature, decimal amount)
    {
        if (amount <= 0) return;

        var state = GetState(creature);
        int oldCharge = state.CurrentCharge;
        state.CurrentCharge = Math.Max(0, state.CurrentCharge - (int)amount);
        int spent = oldCharge - state.CurrentCharge;
        if (spent <= 0) return;

        state.ChargeSpentThisTurn += spent;

        var chargedScales = creature.GetPower<ChargedScalesPower>();
        if (chargedScales != null && creature.Player != null)
        {
            await chargedScales.AfterChargeSpent(spent, creature.Player);
        }

        var temper = creature.GetPower<TemperPower>();
        if (temper != null && creature.Player != null)
        {
            await temper.AfterChargeSpent(spent, creature.Player);
        }
    }

    public static async Task<bool> TrySpendCharge(Creature creature, decimal amount)
    {
        if (GetCharge(creature) < amount) return false;
        await LoseCharge(creature, amount);
        return true;
    }

    public static async Task<int> SpendAllCharge(Creature creature)
    {
        int current = GetCharge(creature);
        if (current > 0)
        {
            await LoseCharge(creature, current);
        }
        return current;
    }
}