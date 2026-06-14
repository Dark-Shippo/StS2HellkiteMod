using MegaCrit.Sts2.Core.Entities.Powers;

namespace Hellkite.HellkiteCode.Powers;

public sealed class RekindlePower: HellkitePower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;
}