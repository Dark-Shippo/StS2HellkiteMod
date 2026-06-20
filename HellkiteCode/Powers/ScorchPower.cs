using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Color = Godot.Color;

namespace Hellkite.HellkiteCode.Powers;

public sealed class ScorchPower : HellkitePower
{
    public override PowerType Type => PowerType.Debuff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    public override Color AmountLabelColor =>
        _normalAmountLabelColor;

    /// <summary>
    /// Kindle causes Scorch to trigger additional times.
    /// It cannot trigger more times than the initial Scorch amount.
    /// </summary>
    private int TriggerCount
    {
        get
        {
            ICombatState? combatState = Owner.CombatState;

            if (combatState == null || Amount <= 0)
                return 0;

            int kindleAmount = combatState
                .GetOpponentsOf(Owner)
                .Where(creature => creature.IsAlive)
                .Sum(creature =>
                    creature.GetPowerAmount<KindlePower>());

            return Math.Min(
                Amount,
                1 + kindleAmount);
        }
    }

    /// <summary>
    /// Calculates the total damage Scorch will deal when it next
    /// triggers, including Kindle and damage modifiers.
    ///
    /// Scorch grows after each trigger, so its damage sequence is:
    /// Amount, Amount + 1, Amount + 2, etc.
    ///
    /// Find a way to implement this to the UI.
    /// </summary>
    public int CalculateTotalDamageAtTurnEnd()
    {
        ICombatState? combatState = Owner.CombatState;

        if (combatState == null || Amount <= 0)
            return 0;

        decimal totalDamage = 0M;
        int iterations = TriggerCount;

        for (int i = 0; i < iterations; i++)
        {
            decimal rawDamage = Amount + i;

            decimal modifiedDamage = Hook.ModifyDamage(
                combatState.RunState,
                combatState,
                Owner,
                null,
                rawDamage,
                ValueProp.Unpowered | ValueProp.Unblockable,
                null,
                ModifyDamageHookType.All,
                CardPreviewMode.None,
                out IEnumerable<AbstractModel> _);

            totalDamage += modifiedDamage;
        }

        return (int)totalDamage;
    }

    public override async Task BeforeSideTurnEnd(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IEnumerable<Creature> participants)
    {
        if (side != Owner.Side)
            return;

        // Important for individual extra turns in multiplayer.
        if (!participants.Contains(Owner))
            return;

        int iterations = TriggerCount;

        for (int i = 0; i < iterations; i++)
        {
            if (!Owner.IsAlive)
            {
                await Cmd.CustomScaledWait(0.1f, 0.25f);
                continue;
            }

            await TriggerOnce(choiceContext);
        }
    }

    /// <summary>
    /// Triggers this Scorch one time.
    ///
    /// Automatic turn-end triggers use no damage source.
    /// Cards that manually trigger Scorch may provide a source and card.
    /// </summary>
    public async Task TriggerOnce(
        PlayerChoiceContext choiceContext,
        Creature? damageSource = null,
        CardModel? cardSource = null)
    {
        if (Amount <= 0 || !Owner.IsAlive)
            return;

        int damage = Amount;

        await CreatureCmd.Damage(
            choiceContext,
            Owner,
            damage,
            ValueProp.Unpowered | ValueProp.Unblockable,
            damageSource,
            cardSource);

        if (Owner.IsAlive)
        {
            // The growth comes from Scorch itself, not from the card or
            // creature that originally applied or manually triggered it.
            await PowerCmd.ModifyAmount(
                choiceContext,
                this,
                1M,
                null,
                null);
        }
        else
        {
            await Cmd.CustomScaledWait(0.1f, 0.25f);
        }
    }
}
