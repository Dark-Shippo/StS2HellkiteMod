using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Hellkite.HellkiteCode.Powers;

public sealed class FlashFirePower : HellkitePower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterCardDrawnEarly(
        PlayerChoiceContext choiceContext,
        CardModel card,
        bool fromHandDraw)
    {
        if (fromHandDraw)
        {
            foreach (Creature target in CombatState.HittableEnemies)
            {
                await PowerCmd.Apply<ScorchPower>(target, DynamicVars[nameof(ScorchPower)].BaseValue, Owner, null);
            }
        }
    }
}