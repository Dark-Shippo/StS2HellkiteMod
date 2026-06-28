using Hellkite.HellkiteCode.Structs;
using MegaCrit.Sts2.Core.Entities.Players;

namespace Hellkite.HellkiteCode.Hooks;

public interface IAfterFireUpSpent
{
    public Task AfterFireUpSpent(FireUp fireUp, Player spender);
}