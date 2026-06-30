using Godot;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;

namespace Hellkite.HellkiteCode.Powers;

public sealed class KindlePower : HellkitePower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override Color AmountLabelColor => _normalAmountLabelColor;

    // Kindle works like Snecko Skull / Accelerant, but for Scorch: every Scorch the owner
    // applies gains this many additional stacks. (Extra Scorch *triggers* are Rekindle.)
    public override decimal ModifyPowerAmountGivenAdditive(PowerModel power, Creature target, decimal amount,
        Creature? applier, CardModel? card)
    {
        return power is ScorchPower ? Amount : 0M;
    }
}
