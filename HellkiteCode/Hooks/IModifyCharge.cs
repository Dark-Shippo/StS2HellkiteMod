using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Hellkite.HellkiteCode.Hooks;

public interface IModifyCharge
{
    public decimal ModifyCharge(Player player, decimal charge, ValueProp props, CardModel? cardSource);
}

public interface IAfterModifyingCharge
{
    public Task AfterModifyingCharge();
}