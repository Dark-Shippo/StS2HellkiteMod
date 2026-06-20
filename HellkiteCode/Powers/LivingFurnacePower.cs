using Hellkite.HellkiteCode.Fire_Up;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Hellkite.HellkiteCode.Powers;

public sealed class LivingFurnacePower : HellkitePower
{
    private const decimal ChargePerTurn = 2M;

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

        await ChargeHandler.GainCharge(
            Owner,
            ChargePerTurn,
            choiceContext);

        await PowerCmd.Apply<KindlePower>(
            choiceContext,
            Owner,
            Amount,
            Owner,
            null);
    }
}