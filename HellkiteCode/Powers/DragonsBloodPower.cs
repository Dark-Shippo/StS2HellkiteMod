using Hellkite.HellkiteCode.Commands;
using Hellkite.HellkiteCode.Fire_Up;
using Hellkite.HellkiteCode.Structs;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Hellkite.HellkiteCode.Powers;

public sealed class DragonsBloodPower : HellkitePower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    public override async Task AfterPlayerTurnStart(
        PlayerChoiceContext choiceContext,
        Player player)
    {
        if (player != Owner.Player)
            return;

        Flash();

        await HellkitePlayerCmd.GainFireUp(new FireUp(Amount), Owner.Player);

    }
}