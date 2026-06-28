using Hellkite.HellkiteCode.Structs;
using MegaCrit.Sts2.Core.Combat;

namespace Hellkite.HellkiteCode.Extensions;

public static class CombatStateTrackerExtension
{
    private static void OnFireUpChanged(this CombatStateTracker tracker, FireUp _, FireUp __)
    {
        tracker.NotifyCombatStateChanged("OnPlayerCombatStateValueChanged");
    }

    public static void SubscribeFireUp(this CombatStateTracker tracker, PlayerCombatStateExtension.HellkiteCombatState combatState)
    {
        combatState.FireUpChanged += tracker.OnFireUpChanged;
    }

    public static void UnsubscribeFireUp(this CombatStateTracker tracker, PlayerCombatStateExtension.HellkiteCombatState combatState)
    {
        combatState.FireUpChanged -= tracker.OnFireUpChanged;
    }
}