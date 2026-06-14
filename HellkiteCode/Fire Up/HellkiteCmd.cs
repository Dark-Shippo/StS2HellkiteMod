using Hellkite.HellkiteCode.Cards;
using Hellkite.HellkiteCode.Powers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;    
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Hellkite.HellkiteCode.Fire_Up;

public static class HellkiteCmd
{
    public static async Task AttackTarget(
        PlayerChoiceContext choiceContext,
        HellkiteCard card,
        Creature? target,
        decimal damage,
        int hits = 1)
    {
        ArgumentNullException.ThrowIfNull(target);
        await CreatureCmd.TriggerAnim(card.Owner.Creature, "Cast", card.Owner.Character.AttackAnimDelay);

        var attack = DamageCmd.Attack(damage)
            .FromCard(card)
            .Targeting(target)
            .WithHitFx("vfx/vfx_attack_slash");

        if (hits > 1)
        {
            attack = attack.WithHitCount(hits);
        }
        
        await attack.Execute(choiceContext);
    }

    public static async Task AttackAll(
        PlayerChoiceContext? choiceContext,
        HellkiteCard? card,
        decimal damage)
    {
        if (card != null)
        {
            await CreatureCmd.TriggerAnim(card.Owner.Creature, "Cast", card.Owner.Character.AttackAnimDelay);

            if (card.CombatState != null)
                await DamageCmd.Attack(damage)
                    .FromCard(card)
                    .TargetingAllOpponents(card.CombatState)
                    .WithHitFx("vfx/vfx_attack_slash")
                    .Execute(choiceContext);
        }
    }

    public static async Task ApplyScorch(Creature target, decimal amount, Creature source, CardModel sourceCard)
    {
        if (amount <= 0) return;
        await PowerCmd.Apply<ScorchPower>(target, amount, source, sourceCard);
    }

    public static async Task ApplyScorchAll(CombatState combatState, decimal amount, Creature source, CardModel sourceCard)
    {
        if (amount <= 0) return;
        foreach (var enemy in combatState.HittableEnemies)
        {
            await PowerCmd.Apply<ScorchPower>(enemy, amount, source, sourceCard);
        }
    }

    public static async Task TriggerScorchOnce(PlayerChoiceContext choiceContext, Creature target, Creature source, CardModel sourceCard)
    {
        var scorch = target.GetPower<ScorchPower>();
        if (scorch == null || scorch.Amount <= 0) return;

        await CreatureCmd.Damage(
            choiceContext,
            target,
            scorch.Amount,
            ValueProp.Unpowered | ValueProp.Unblockable,
            source,
            sourceCard);

        if (target.IsAlive)
        {
            await PowerCmd.ModifyAmount(scorch, 1M, source, sourceCard);
        }
    }

    public static Creature? RandomEnemy(Creature owner)
    {
        if (owner is { CombatState: not null, Player: not null })
        {
            var enemies = owner.CombatState.HittableEnemies;
            return enemies.Count == 0 ? null : owner.Player.RunState.Rng.CombatTargets.NextItem(enemies);
        }
        return null;       
    }

    public static async Task<int> RemoveAllRazorScales(Creature owner)
    {
        int current = owner.GetPowerAmount<RazorScalesPower>();
        if (current <= 0) return 0;

        var power = owner.GetPower<RazorScalesPower>();
        if (power != null)
        {
            await PowerCmd.ModifyAmount(power, -current, owner, null);
        }

        return current;
    }

    public static async Task<int> SpendUpToCharge(
        Creature owner,
        int maxAmount)
    {
        // For now leave as a full drain and wait for feedback
        var chosen = Math.Min(maxAmount, ChargeHandler.GetCharge(owner));
        if (chosen > 0)
        {
            await ChargeHandler.LoseCharge(owner, chosen);
        }
        return chosen;
    }
}

public sealed class HellkiteVar(string name, decimal amount) : DynamicVar(name, amount)
{
    public override void UpdateCardPreview(
        CardModel card,
        CardPreviewMode previewMode,
        Creature? target,
        bool runGlobalHooks)
    {
        EnchantedValue = BaseValue;
        PreviewValue = BaseValue;
    }
}
