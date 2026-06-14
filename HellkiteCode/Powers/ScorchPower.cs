using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Color = Godot.Color;

namespace Hellkite.HellkiteCode.Powers;

public sealed class ScorchPower : HellkitePower
{
    public override PowerType Type => PowerType.Debuff;

    public override PowerStackType StackType => PowerStackType.Counter;
    
    public override Color AmountLabelColor => _normalAmountLabelColor;

    private int TriggerCount
    {
        get
        {
            if (Owner.CombatState != null)
            {
                int baseTriggers = 1 + Owner.CombatState.GetOpponentsOf(Owner).Where(c => c.IsAlive).Sum(a => a.GetPowerAmount<KindlePower>());
                int additionalTriggers = Owner.CombatState.GetOpponentsOf(Owner).Where(c => c.IsAlive).Sum(a => a.GetPowerAmount<RekindlePower>());
                return Math.Min(Amount, baseTriggers + additionalTriggers + baseTriggers);
            }

            return 0;
        }
    }

    public override async Task BeforeTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (side != Owner.Side) return;
        
        ScorchPower power = this;
        if (side != power.Owner.Side)
            return;
        if (side == Owner.Side)
        {
            int iterations = power.TriggerCount;
            for (int i = 0; i < iterations; ++i)
            {
                if (power.Owner.IsAlive)
                    await PowerCmd.ModifyAmount(power, 1M, null, null);
                else
                    await Cmd.CustomScaledWait(0.1f, 0.25f);
            }
        }
    }
}