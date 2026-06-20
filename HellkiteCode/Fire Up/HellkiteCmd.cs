using Hellkite.HellkiteCode.Cards;
using Hellkite.HellkiteCode.Powers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Hellkite.HellkiteCode.Fire_Up;

public static class HellkiteCmd
{
    public static async Task AttackTarget(
        PlayerChoiceContext choiceContext,
        HellkiteCard card,
        Creature target,
        decimal damage,
        int hits = 1)
    {
        if (damage <= 0M || hits <= 0)
            return;

        await CreatureCmd.TriggerAnim(
            card.Owner.Creature,
            "Cast",
            card.Owner.Character.AttackAnimDelay);

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
        PlayerChoiceContext choiceContext,
        HellkiteCard? card,
        decimal damage)
    {
        if (card != null && (damage <= 0M || card.CombatState == null))
            return;

        if (card != null)
        {
            await CreatureCmd.TriggerAnim(
                card.Owner.Creature,
                "Cast",
                card.Owner.Character.AttackAnimDelay);

            if (card.CombatState != null)
                await DamageCmd.Attack(damage)
                    .FromCard(card)
                    .TargetingAllOpponents(card.CombatState)
                    .WithHitFx("vfx/vfx_attack_slash")
                    .Execute(choiceContext);
        }
    }

    public static async Task DamageAllEnemies(
        PlayerChoiceContext choiceContext,
        ICombatState combatState,
        Creature source,
        decimal damage,
        ValueProp props = ValueProp.Unpowered,
        CardModel? sourceCard = null)
    {
        if (damage <= 0M)
            return;

        // Snapshot the targets because damage may kill or remove creatures.
        List<Creature> targets =
            combatState.HittableEnemies.ToList();

        foreach (Creature target in targets)
        {
            await CreatureCmd.Damage(
                choiceContext,
                target,
                damage,
                props,
                source,
                sourceCard);
        }
    }

    public static async Task ApplyScorch(
        Creature target,
        decimal amount,
        Creature source,
        CardModel? sourceCard,
        PlayerChoiceContext choiceContext)
    {
        if (amount <= 0M)
            return;

        await PowerCmd.Apply<ScorchPower>(
            choiceContext,
            target,
            amount,
            source,
            sourceCard);
    }

    public static async Task ApplyScorchAll(
        ICombatState combatState,
        decimal amount,
        Creature source,
        CardModel? sourceCard,
        PlayerChoiceContext choiceContext)
    {
        if (amount <= 0M)
            return;

        List<Creature> targets =
            combatState.HittableEnemies.ToList();

        if (targets.Count == 0)
            return;

        await PowerCmd.Apply<ScorchPower>(
            choiceContext,
            targets,
            amount,
            source,
            sourceCard);
    }

    public static async Task TriggerScorchOnce(
        PlayerChoiceContext choiceContext,
        Creature target,
        Creature source,
        CardModel? sourceCard)
    {
        ScorchPower? scorch =
            target.GetPower<ScorchPower>();

        if (scorch == null || scorch.Amount <= 0)
            return;

        await scorch.TriggerOnce(choiceContext, source, sourceCard);
    }

    public static Creature? RandomEnemy(Creature owner)
    {
        if (owner.CombatState == null ||
            owner.Player == null)
        {
            return null;
        }

        var enemies =
            owner.CombatState.HittableEnemies;

        return enemies.Count == 0
            ? null
            : owner.Player.RunState.Rng.CombatTargets.NextItem(enemies);
    }

    public static async Task<int> RemoveAllRazorScales(
        Creature owner,
        PlayerChoiceContext choiceContext)
    {
        RazorScalesPower? power =
            owner.GetPower<RazorScalesPower>();

        if (power == null || power.Amount <= 0)
            return 0;

        int amountRemoved = power.Amount;

        await PowerCmd.ModifyAmount(
            choiceContext,
            power,
            -amountRemoved,
            owner,
            null);

        return amountRemoved;
    }

    public static async Task<int> SpendUpToCharge(
        Creature owner,
        int maxAmount,
        PlayerChoiceContext choiceContext)
    {
        if (maxAmount <= 0)
            return 0;

        int chosen = Math.Min(
            maxAmount,
            ChargeHandler.GetCharge(owner));

        if (chosen <= 0)
            return 0;

        await ChargeHandler.LoseCharge(
            owner,
            chosen,
            choiceContext);

        return chosen;
    }
}
