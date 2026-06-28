using Hellkite.HellkiteCode.Extensions;
using Hellkite.HellkiteCode.Fire_Up;
using Hellkite.HellkiteCode.Structs;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Hellkite.HellkiteCode.Powers;

public sealed class ScaleMailPower : HellkitePower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    public override async Task BeforeSideTurnEnd(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IEnumerable<Creature> participants)
    {
        if (side != Owner.Side)
            return;

        if (!participants.Contains(Owner))
            return;

        var fireUp = Owner.Player?.PlayerCombatState?.GetFireUp() ?? new FireUp();
        if (fireUp.Total > 10)
            return;

        Flash();

        await PowerCmd.Apply<PlatingPower>(
            choiceContext,
            Owner,
            Amount,
            Owner,
            null);
    }
}
