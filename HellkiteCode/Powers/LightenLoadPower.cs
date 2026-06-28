using Hellkite.HellkiteCode.Extensions;
using Hellkite.HellkiteCode.Fire_Up;
using Hellkite.HellkiteCode.Structs;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Entities.Players;

namespace Hellkite.HellkiteCode.Powers;

public sealed class LightenLoadPower : HellkitePower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterEnergyReset(Player player)
    {
        var fireUp = player.PlayerCombatState?.GetFireUp() ?? new FireUp();
        if (fireUp.Total > 21)
            await PlayerCmd.GainEnergy(Amount, player);
    }
}