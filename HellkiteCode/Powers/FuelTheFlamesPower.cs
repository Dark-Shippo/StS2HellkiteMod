using Hellkite.HellkiteCode.Extensions;
using Hellkite.HellkiteCode.Fire_Up;
using Hellkite.HellkiteCode.Structs;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Hellkite.HellkiteCode.Powers;

public sealed class FuelTheFlamesPower : HellkitePower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    public override async Task AfterPlayerTurnStartLate(
        PlayerChoiceContext choiceContext,
        Player player)
    {
        if (player != Owner.Player)
            return;

        var fireUp = player.PlayerCombatState?.GetFireUp() ?? new FireUp();
        if (fireUp.Total < 11)
            return;

        Flash();

        await PowerCmd.Apply<VigorPower>(
            choiceContext,
            Owner,
            Amount,
            Owner,
            null);
    }
}