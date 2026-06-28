using Hellkite.HellkiteCode.Structs;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Hellkite.HellkiteCode.Hooks;

public interface IModifyFireUpGain
{
    public FireUp ModifyFireUpGain(Player player, FireUp amount, ValueProp props, CardModel? cardSource);
}

public interface IAfterModifyingFireUpGain
{
    public Task AfterModifyingFireUpGain();
}