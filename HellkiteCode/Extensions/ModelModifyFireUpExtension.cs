using Hellkite.HellkiteCode.Structs;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.Relics;

namespace Hellkite.HellkiteCode.Extensions;

public static class ModelModifyFireUpExtension
{
    public static bool TryModifyFireUpCost(this BrilliantScarf model, CardModel card, FireUp originalCost,
        out FireUp modifiedCost)
    {
        modifiedCost = originalCost;
        if (!model.ShouldModifyCost(card)) return false;
        modifiedCost = new FireUp(0);
        return true;
    }

    public static bool TryModifyFireUpCost(this VoidFormPower model, CardModel card, FireUp originalCost,
        out FireUp modifiedCost)
    {
        modifiedCost = originalCost;
        if (model.ShouldSkip(card)) return false;
        modifiedCost = new FireUp(0);
        return true;
    }
}