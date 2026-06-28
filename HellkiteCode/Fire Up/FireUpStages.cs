using Hellkite.HellkiteCode.Commands;
using Hellkite.HellkiteCode.Extensions;
using Hellkite.HellkiteCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;

namespace Hellkite.HellkiteCode.Fire_Up;

public class FireUpStages
{
    /// <summary>Hard cap of the Fired Up meter. Reaching it enters Overcharge.</summary>
    public const int Max = 30;

    public const int LowStageThreshold = 10;
    public const int HighStageThreshold = 20;
    public const int OverchargeThreshold = Max;
    
    public const bool ResetMeterOnOvercharge = true;
 
    public static int GetKindleStacks(int fireUp)
    {
        return fireUp switch
        {
            >= OverchargeThreshold => 0,
            >= HighStageThreshold => 2,
            >= LowStageThreshold => 1,
            _ => 0
        };
    }
    
    public static async Task SyncKindle(Player player)
    {
        if (player.Creature.CombatState == null)
            return;

        var hellkiteState = player.PlayerCombatState?.Hellkite();
        if (hellkiteState == null)
            return;

        int fireUp = hellkiteState.FireUp.Total;
        int desiredAmount = GetKindleStacks(fireUp);

        KindlePower? kindle = player.Creature.GetPower<KindlePower>();
        int totalKindle = kindle?.Amount ?? 0;

        // Only alter the Kindle stacks supplied by FireUp. Any Kindle granted by
        // cards or other powers is left untouched.
        int trackedAmount = Math.Min(hellkiteState.FireUpKindleStacks, totalKindle);
        hellkiteState.SetFireUpKindleStacks(trackedAmount);

        int difference = desiredAmount - trackedAmount;

        if (difference > 0)
        {
            await PowerCmd.Apply<KindlePower>(
                choiceContext: null,
                player.Creature,
                difference,
                player.Creature,
                null);

            hellkiteState.SetFireUpKindleStacks(desiredAmount);
        }
        else if (difference < 0 && kindle != null)
        {
            int amountToRemove = Math.Min(-difference, totalKindle);

            await PowerCmd.ModifyAmount(
                choiceContext: null,
                kindle,
                -amountToRemove,
                player.Creature,
                null);

            hellkiteState.SetFireUpKindleStacks(trackedAmount - amountToRemove);
        }
    }
    
    /// <summary>True when a gain crossed the player into the Overcharge tier this call.</summary>
    public static bool JustReachedOvercharge(int before, int after)
        => before < OverchargeThreshold && after >= OverchargeThreshold;

    public static async Task EnterOvercharge(Player player)
    {
        if (player.Creature.CombatState == null)
            return;

        // Do not stack OverCharge repeatedly, but still keep Kindle synchronized.
        if (player.Creature.GetPower<OverChargePower>() != null)
        {
            await SyncKindle(player);
            return;
        }

        int meter = player.PlayerCombatState?.GetFireUp().Total ?? Max;
        int overchargeAmount = Math.Clamp(meter, 0, Max);

        if (ResetMeterOnOvercharge)
            await HellkitePlayerCmd.ResetFireUp(player);

        await PowerCmd.Apply<OverChargePower>(
            choiceContext: null,
            player.Creature,
            overchargeAmount,
            player.Creature,
            null);
    }
}