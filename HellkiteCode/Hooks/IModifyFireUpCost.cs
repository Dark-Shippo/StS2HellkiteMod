using Hellkite.HellkiteCode.Structs;
using MegaCrit.Sts2.Core.Models;

namespace Hellkite.HellkiteCode.Hooks;

public interface IModifyFireUpCost
{
    bool TryModifyFireUpCost(CardModel card, FireUp originalCost, out FireUp modifiedCost);
}