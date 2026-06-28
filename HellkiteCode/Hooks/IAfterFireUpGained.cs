using Hellkite.HellkiteCode.Structs;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;

namespace Hellkite.HellkiteCode.Hooks;

public interface IAfterFireUpGained
{
    public Task AfterFireUpGained(ICombatState combatState, FireUp amount, Player player,
        CardPlay? cardPlay = null);
}