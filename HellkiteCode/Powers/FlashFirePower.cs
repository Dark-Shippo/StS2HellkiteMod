using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Hellkite.HellkiteCode.Powers;

public sealed class FlashFirePower : HellkitePower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    public override async Task AfterCardDrawn(
        PlayerChoiceContext choiceContext,
        CardModel card,
        bool fromHandDraw)
    {
        // Ignore the normal cards drawn during the turn's hand-draw step.
        if (fromHandDraw)
            return;

        // In multiplayer, do not react to another player's draws.
        if (card.Owner != Owner.Player)
            return;

        List<Creature> targets =
            CombatState.HittableEnemies.ToList();

        if (targets.Count == 0)
            return;

        Flash();

        await PowerCmd.Apply<ScorchPower>(
            choiceContext,
            targets,
            Amount,
            Owner,
            null);
    }
}