using Hellkite.HellkiteCode.Extensions;
using Hellkite.HellkiteCode.Fire_Up;
using Hellkite.HellkiteCode.Hooks;
using Hellkite.HellkiteCode.Powers;
using Hellkite.HellkiteCode.Structs;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.ValueProps;

namespace Hellkite.HellkiteCode.Commands;

public static class HellkitePlayerCmd
{
    public static async Task GainFireUp(FireUp amount, Player player, CardPlay? cardPlay = null)
    {
        if (amount.Total > 0 && !CombatManager.Instance.IsEnding && player.Creature.CombatState != null)
        {
            var combatState = player.Creature.CombatState;
            var hellkiteCombatState = player.PlayerCombatState?.Hellkite();
            var finalAmount = HellkiteHook.ModifyFireUpGain(
                combatState, 
                player, 
                amount, 
                ValueProp.Move,
                cardPlay?.Card, 
                out var modifiers);     
            
            await HellkiteHook.AfterModifyingFireUpGain(combatState, modifiers);
            if (finalAmount.Total > 0)
            {
                var before = player.PlayerCombatState?.GetFireUp().Total ?? 0;
                //charge gain sound here
                hellkiteCombatState?.GainFireUp(finalAmount);
                var after = player.PlayerCombatState?.GetFireUp().Total ?? 0;
                
                if (FireUpStages.JustReachedOvercharge(before, after))
                    await FireUpStages.EnterOvercharge(player);
                else
                    await FireUpStages.SyncKindle(player);
            }
            await HellkiteHook.AfterFireUpGained(combatState, finalAmount, player, cardPlay);
        }
    }

    public static async Task LoseFireUp(FireUp amount, Player player)
    {
        if (amount.Total <= 0 || CombatManager.Instance.IsEnding) return;
        
        int before = player.PlayerCombatState?.GetFireUp().Total ?? 0;
        
        player.PlayerCombatState?.Hellkite()?.LoseFireUp(amount);
        int after = player.PlayerCombatState?.GetFireUp().Total ?? 0;
        if (after != before)
            await FireUpStages.SyncKindle(player);
        
    }

    public static async Task ResetFireUp(Player player)
    {
        if (!CombatManager.Instance.IsEnding)
        {
            var fireUp = player.PlayerCombatState?.GetFireUp() ?? new FireUp();
            await LoseFireUp(fireUp, player);        }
    }
    
    public static async Task<int> SpendAllFireUp(Player player)
    {
        if (CombatManager.Instance.IsEnding)
            return 0;

        var current = player.PlayerCombatState?.GetFireUp() ?? new FireUp();
        if (current.Total <= 0)
            return 0;

        await LoseFireUp(current, player);
        return current.Total;
    }
}