using Godot;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace Hellkite.HellkiteCode.Powers;

public sealed class KindlePower : HellkitePower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;
    
    public override Color AmountLabelColor => _normalAmountLabelColor;
}