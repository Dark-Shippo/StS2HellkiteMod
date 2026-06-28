using Hellkite.HellkiteCode.Field;
using Hellkite.HellkiteCode.Fire_Up;
using Hellkite.HellkiteCode.Structs;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Players;

namespace Hellkite.HellkiteCode.Extensions;

public static class PlayerCombatStateExtension
{
    public class HellkiteCombatState(PlayerCombatState combatState)
    {
        public FireUp FireUp
        {
            get;
            private set
            {
                if (field == value) return;
                var fireUp = field;
                field = value;
                var state = combatState._player.Creature.CombatState;
                if (state != null)
                    CombatManager.Instance.History.FireUpModified(state,
                        field - fireUp, combatState._player);
                FireUpChanged?.Invoke(fireUp, field);
            }
        } = new();

        public event Action<FireUp, FireUp>? FireUpChanged;

        private bool _hasSpentFireUpThisTurn;

        public int FireUpKindleStacks { get; private set; }
        
        public void SetFireUpKindleStacks(int amount)
        {
            FireUpKindleStacks = Math.Max(0, amount);
        }
        
        public bool HasSpentFireUpThisTurn() => _hasSpentFireUpThisTurn;

        public void ResetTurnTracking() => _hasSpentFireUpThisTurn = false;

        public void GainFireUp(FireUp amount)
        {
            if (amount.Total < 0) throw new ArgumentException("Must not be negative", nameof(amount));
            
            FireUp = (FireUp + amount).ClampZero();
        }

        public void LoseFireUp(FireUp amount)
        {
            if (amount.Total < 0) throw new ArgumentException("Must not be negative", nameof(amount));

            var before = FireUp;
            FireUp = (FireUp - amount).Clamp(0, FireUpStages.Max);

            if (FireUp.Total < before.Total) _hasSpentFireUpThisTurn = true;
        }
    }

    public static FireUp GetFireUp(this PlayerCombatState playerCombatState)
    {
        var hellkiteCombatState = playerCombatState.Hellkite();
        return hellkiteCombatState?.FireUp ?? new FireUp();
    }

    public static HellkiteCombatState? Hellkite(this PlayerCombatState playerCombatState)
    {
        return HellkiteField.HellkiteCombatState[playerCombatState];
    }
}